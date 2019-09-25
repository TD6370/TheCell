using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

public partial class ModelNPC
{
    [XmlType("Flore")]
    public class FloreData : TerraData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolFlore; } }

        public FloreData() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Weedflower")]


    public class Weedflower : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Weedflower; } }
        public Weedflower() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Berry")]
    public class Berry : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Berry; } }
        public Berry() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Mashrooms")]
    public class Mashrooms : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Mashrooms; } }
        public Mashrooms() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Orbits")]
    public class Orbits : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Orbits; } }
        public Orbits() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Shampinion")]
    public class Shampinion : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Shampinion; } }
        public Shampinion() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Corals")]
    public class Corals : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Corals; } }
        public Corals() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Diods")]
    public class Diods : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Diods; } }
        public Diods() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Kamish")]
    public class Kamish : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Kamish; } }
        public Kamish() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Swamp")]
    public class Swamp : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Swamp; } }
        public Swamp() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Tussock")]
    public class Tussock : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Tussock; } }
        public Tussock() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Osoka")]
    public class Osoka : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Osoka; } }
        public Osoka() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Iris")]
    public class Iris : FloreData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Iris; } }
        public Iris() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
}
