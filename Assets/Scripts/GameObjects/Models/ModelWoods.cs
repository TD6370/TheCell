using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

public partial class ModelNPC
{

    [XmlType("Wood")]
    public class WoodData : TerraData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolWood; } }

        public WoodData() : base() { }
    }

    //------------------------------------------------

    [XmlType("Kolba")]
    public class Kolba : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }

        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Kolba; } }
        public Kolba() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Lantern")]
    public class Lantern : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Lantern; } }
        public Lantern() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Bananas")]
    public class Bananas : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Bananas; } }
        public Bananas() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Cluben")]
    public class Cluben : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Cluben; } }
        public Cluben() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Chpok")]
    public class Chpok : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Chpok; } }
        public Chpok() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Pandora")]
    public class Pandora : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Pandora; } }
        public Pandora() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Nadmozg")]
    public class Nadmozg : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Nadmozg; } }
        public Nadmozg() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Triffid")]
    public class Triffid : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Triffid; } }
        public Triffid() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Aracul")]
    public class Aracul : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Aracul; } }
        public Aracul() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Cloudwood")]
    public class Cloudwood : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Cloudwood; } }
        public Cloudwood() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("BlueBerry")]
    public class BlueBerry : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.BlueBerry; } }
        public BlueBerry() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Sosna")]
    public class Sosna : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Sosna; } }
        public Sosna() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Iva")]
    public class Iva : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Iva; } }
        public Iva() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Klen")]
    public class Klen : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Klen; } }
        public Klen() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("RockBrow")]
    public class RockBrown : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.RockBrown; } }
        public RockBrown() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("RockValun")]
    public class RockValun : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.RockValun; } }
        public RockValun() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("RockDark")]
    public class RockDark : WoodData
    {
        public override int Defense { get { return 10; } }
        public override int Health { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.RockDark; } }
        public RockDark() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    //LagcyObjects ---------------------------
    public class Rock : WoodData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabRock; } }
        public Rock() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    public class Vood : WoodData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabVood; } }
        public Vood() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    public class Elka : WoodData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabElka; } }
        public Elka() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
}
