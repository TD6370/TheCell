using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

public partial class ModelNPC
{
    [XmlType("Floor")]
    public class FloorData : TerraData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolFloor; } }

        //public WallData(bool isGen) : base(isGen) { }
        public FloorData() : base() { }
    }


    [XmlType("Weed")]
    public class Weed : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Weed; } }
        public Weed() : base() { TypePrefabName = TypePrefab.ToString(); }
    }


    [XmlType("Kishka")]
    public class Kishka : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Kishka; } }
        public Kishka() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Nerv")]
    public class Nerv : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Nerv; } }
        public Nerv() : base() { TypePrefabName = TypePrefab.ToString(); }
    }




    [XmlType("Desert")]
    public class Desert : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Desert; } }
        public Desert() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Parket")]
    public class Parket : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Parket; } }
        public Parket() : base() { TypePrefabName = TypePrefab.ToString(); }
    }


    [XmlType("Grass")]
    public class Grass : FloorData
    {
        public override int BlockResources { get { return 5; } }
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Grass; } }
        public Grass() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    [XmlType("GrassMedium")]
    public class GrassMedium : FloorData
    {
        public override int BlockResources { get { return 3; } }
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.GrassMedium; } }
        public GrassMedium() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    [XmlType("GrassSmall")]
    public class GrassSmall : FloorData
    {
        public override int BlockResources { get { return 1; } }
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.GrassSmall; } }
        public GrassSmall() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Ground")]
    public class Ground : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Ground; } }
        public Ground() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    [XmlType("Ground02")]
    public class Ground02 : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Ground02; } }
        public Ground02() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    [XmlType("Ground03")]
    public class Ground03 : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Ground03; } }
        public Ground03() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Ground04")]
    public class Ground04 : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Ground04; } }
        public Ground04() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    [XmlType("Ground05")]

    public class Ground05 : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Ground05; } }
        public Ground05() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Chip")]
    public class Chip : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Chip; } }
        public Chip() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Gecsagon")]
    public class Gecsagon : FloorData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Gecsagon; } }
        public Gecsagon() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
}

