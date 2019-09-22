using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;


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
        public virtual TypesBiomNPC TypeBiom { get; set; }

        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }

        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabPortal; } }
       
        [XmlIgnore]
        public ManagerPortals.TypeResourceProcess CurrentProcess = ManagerPortals.TypeResourceProcess.None;
    }

    [XmlType("PortalRed")]
    public class PortalRed : PortalData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PortalRed; } }
        [XmlIgnore]
        public virtual TypesBiomNPC TypeBiom { get { return TypesBiomNPC.Red; } }
    }

    [XmlType("PortalBlue")]
    public class PortalBlue : PortalData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PortalBlue; } }
        [XmlIgnore]
        public virtual TypesBiomNPC TypeBiom { get { return TypesBiomNPC.Blue; } }
    }

    [XmlType("PortalGreen")]
    public class PortalGreen : PortalData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PortalGreen; } }
        [XmlIgnore]
        public virtual TypesBiomNPC TypeBiom { get { return TypesBiomNPC.Green; } }
    }

    [XmlType("PortalViolet")]
    public class PortalViolet : PortalData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPortal; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PortalViolet; } }
        [XmlIgnore]
        public virtual TypesBiomNPC TypeBiom { get { return TypesBiomNPC.Violet; } }
    }
  
}