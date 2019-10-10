using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;
using System.Linq;

public partial class ModelNPC
{
    
    [XmlType("Portal")]
    //[XmlInclude(typeof(DataObjectInventory))]
    [XmlInclude(typeof(SaveLoadData.TypePrefabs))]
    public class PortalData : ObjectData
    {
        public List<DataObjectInventory> Resources { get; set; }
        public List<string> ChildrensId { get; set; }
        public virtual int Life { get; set; }
        public List<KeyValuePair<SaveLoadData.TypePrefabs, List<string>>> ConstructionsIdXML { get; set; }
        public int BuildSize = 54; //23;

        private Dictionary<SaveLoadData.TypePrefabs, List<string>> m_ConstructionsId;
        [XmlIgnore]
        public Dictionary<SaveLoadData.TypePrefabs, List<string>> ConstructionsId
        {
            get
            {
                if (m_ConstructionsId == null)
                {
                    if (ConstructionsIdXML == null)
                        m_ConstructionsId = new Dictionary<SaveLoadData.TypePrefabs, List<string>>();
                    else
                        m_ConstructionsId = ConstructionsIdXML.ToDictionary(x => x.Key, x => x.Value);
                }
                return m_ConstructionsId;
            }
        }

        [XmlIgnore]
        public float LastTimeFabrication;
        [XmlIgnore]
        public virtual TypesBiomNPC TypeBiom { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabPortal; } }
        [XmlIgnore]
        public ManagerPortals.TypeResourceProcess CurrentProcess = ManagerPortals.TypeResourceProcess.None;

        public void AddChild(string id)
        {
            ChildrensId.Add(id);
        }

        public void AddConstruction(SaveLoadData.TypePrefabs typeContr, string id)
        {
            if (m_ConstructionsId == null)
                m_ConstructionsId = new Dictionary<SaveLoadData.TypePrefabs, List<string>>();
            List<string> listId = null;
            m_ConstructionsId.TryGetValue(typeContr, out listId);
            if (listId == null)
                m_ConstructionsId.Add(typeContr, new List<string> { id });
            else
                listId.Add(id);
            Save();
        }

        public void Save()
        {
            ConstructionsIdXML = m_ConstructionsId.ToList();
        }

        public bool ChildrenPreparationIncubation()
        {
            string id;
            int fieldX_NPC = 0;
            int fieldY_NPC = 0;
            int fieldX_Portal= 0;
            int fieldY_Portal = 0;
            bool isParkingLock = false;
            Helper.GetFieldPositByWorldPosit(ref fieldX_Portal, ref fieldY_Portal, Position);
            if (!Storage.Instance.ReaderSceneIsValid)
                return false;
            for(int i = ChildrensId.Count -1 ; i >= 0; i--)
            {
                id = ChildrensId[i];
                var dataNPC = ReaderScene.GetInfoID(id);
                if (dataNPC == null)
                    ChildrensId.RemoveAt(i);
                else
                {
                    Helper.GetFieldPositByWorldPosit(ref fieldX_NPC, ref fieldY_NPC, dataNPC.Data.Position);
                    isParkingLock = fieldX_NPC == fieldX_Portal && fieldY_NPC == fieldY_Portal;
                    if(isParkingLock)
                        return false;
                }
            }
            if (ChildrensId.Count == 0)
                return true;
            return true;
        }
        
        public Vector3 SearchParking(ref string nameField)
        {
            List<ObjectData> dataObjs;
            int x = 0;
            int y = 0;
            Helper.GetFieldPositByWorldPosit(ref x, ref y, Position);
            List<Vector2Int> findedFileds = new List<Vector2Int>();
            Helper.GetSpiralFields(ref findedFileds, x, y, 20);
            foreach (Vector2Int fieldNext in findedFileds)
            {
                Helper.GetNameField_Cache(ref nameField, fieldNext.x, fieldNext.y);
                if(ReaderScene.IsFieldFree(nameField))
                {
                    return Helper.NormalizFieldToWorld(fieldNext);
                }
            }
            return Vector3.zero;
        }
                       
        public string GetInfo()
        {
            int xOut = 0;
            int yOut = 0;
            string resInfo = (Resources == null) ? "." : Resources.Count.ToString();
            string childInfo = (ChildrensId == null) ? "..." : ChildrensId.Count.ToString();
            
            Helper.GetFieldPositByWorldPosit(ref xOut, ref yOut, Position);
            return String.Format("{0} F:{1}x{2} R:{3} NPC:{4} P:{5}",
                TypeBiom.ToString(),
                xOut,
                yOut,
                resInfo,
                childInfo,
                CurrentProcess);
        }

        public void EndFabrication()
        {
            LastTimeFabrication = Time.time;
        }

        //Clear location for portal
        public void ClearLocationAndCreateBiomFloor(int fieldX, int fieldY)
        {
            var portal = this;
            TypesBiomNPC typePortal = portal.TypeBiom;

            string nameField = string.Empty;
            List<Vector2Int> findedFileds = new List<Vector2Int>();
            Helper.GetSpiralFields(ref findedFileds, fieldX, fieldY, BuildSize);
            findedFileds.Add(new Vector2Int(fieldX, fieldY));
            foreach (Vector2Int fieldNext in findedFileds)
            {
                Helper.GetNameField_Cache(ref nameField, fieldNext.x, fieldNext.y);
                GenericWorldManager.ClearLayerForStructure(nameField, true);
                SaveLoadData.TypePrefabs portalFloorType = ManagerPortals.PortalBiomFloorsBase[typePortal];

                //Create object Biom Floor
                var objDataSave = BilderGameDataObjects.BildObjectData(portalFloorType);
                string nameObject = string.Empty;
                Helper.CreateName_Cache(ref nameObject, portalFloorType.ToString(), nameField, "-1");
                objDataSave.Position = Helper.NormalizFieldToWorld(fieldNext);
                Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "GenericPortal");
                objDataSave.SetNameObject(nameObject, true);

                //%CLUSTER FILL
                //if (objDataSave.IsFloor())
                //{
                Vector2Int posField = Helper.GetFieldPositByWorldPosit(objDataSave.Position);
                int clusterSize = AlienJobsManager.GetClusterSize(posField.x, posField.y, objDataSave.TypePrefab);
                (objDataSave as ModelNPC.TerraData).ClusterFillSize = clusterSize;
                (objDataSave as ModelNPC.TerraData).DataCreate = DateTime.Now;
                //}

                //... Check on Real
                bool isZonaReal = Helper.IsValidPiontInZona(objDataSave.Position.x, objDataSave.Position.y);
                if (!objDataSave.IsReality && isZonaReal)
                    Storage.GenGrid.LoadObjectToReal(nameField);
            }
        }


    }

    [XmlType("PortalRed")]
    public class PortalRed : PortalData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PortalRed; } }
        [XmlIgnore]
        public override TypesBiomNPC TypeBiom { get { return TypesBiomNPC.Red; } }
    }

    [XmlType("PortalBlue")]
    public class PortalBlue : PortalData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PortalBlue; } }
        [XmlIgnore]
        public override TypesBiomNPC TypeBiom { get { return TypesBiomNPC.Blue; } }
    }

    [XmlType("PortalGreen")]
    public class PortalGreen : PortalData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PortalGreen; } }
        [XmlIgnore]
        public override TypesBiomNPC TypeBiom { get { return TypesBiomNPC.Green; } }
    }

    [XmlType("PortalViolet")]
    public class PortalViolet : PortalData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PortalViolet; } }
        [XmlIgnore]
        public override TypesBiomNPC TypeBiom { get { return TypesBiomNPC.Violet; } }
    }
  
}