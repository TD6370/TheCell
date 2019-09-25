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
    public class PortalData : ObjectData
    {
        public List<DataObjectInventory> Resources { get; set; }
        public List<string> ChildrensId { get; set; }
        public virtual int Life { get; set; }

        [XmlIgnore]
        public float LastTimeIncubation;
        [XmlIgnore]
        public virtual TypesBiomNPC TypeBiom { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabPortal; } }
        [XmlIgnore]
        public ManagerPortals.TypeResourceProcess CurrentProcess = ManagerPortals.TypeResourceProcess.None;

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