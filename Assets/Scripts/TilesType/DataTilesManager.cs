﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataTilesManager : MonoBehaviour {

    //--- TAILS ---
    public GameObject BackPalette;
    public Grid GridTiles;
    public GameObject TilesMapBackLayer;
    public GameObject TilesMapPrefabLayer;

    private Texture2D[] m_listTexturs;// = Resources.LoadAll<Texture2D>("Textures/Terra/Floor/");
    public  Texture2D[] ListTexturs
    {
        get{
            if (m_listTexturs == null)
            {
                try
                {
                    var listTexturs = Resources.LoadAll<Texture2D>("Textures/Terra/Floor/").ToList();
                    var listTextursPrefabs = Resources.LoadAll<Texture2D>("Textures/TilesPrefab/").ToList();
                    listTexturs.AddRange(listTextursPrefabs);
                    m_listTexturs = listTexturs.ToArray();
                }catch(Exception x)
                {
                    Debug.Log("############ ListTexturs init: " + x.Message);
                }

            }
            return m_listTexturs;
        }
    }

    private TileBase[] m_listTiles;// = Resources.LoadAll<TileBase>("Textures/Terra/Floor/Tiles/");

    private Dictionary<string, Texture2D> m_collectionTextureTiles;
    public Dictionary<string, Texture2D> CollectionTextureTiles
    {
        get
        {
            if(m_collectionTextureTiles==null)
                LoadTextures();
            return m_collectionTextureTiles;
        }

    }
    private Dictionary<string, Sprite> m_collectionSpriteTiles;
    public Dictionary<string, Sprite> CollectionSpriteTiles
    {
        get
        {
            if (m_collectionSpriteTiles == null)
                LoadTextures();
            return m_collectionSpriteTiles;
        }

    }
    public Dictionary<string, TileBase> CollectionTiles;

    //private Dictionary<string, List<DataTile>> m_CollectionDataMapTiles;
    //public Dictionary<string, List<DataTile>> DataMapTiles
    //{
    //    get
    //    {
    //        return m_CollectionDataMapTiles;
    //    }
    //}
    private Dictionary<string, DataConstructionTiles> m_CollectionDataMapTiles;
    public Dictionary<string, DataConstructionTiles> DataMapTiles
    {
        get
        {
            return m_CollectionDataMapTiles;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateGridTiles()
    {
        CreateDataTiles();

        LoadTextures();

        Storage.PaletteMap.LoadListConstructsControl();
    }

    public void CreateDataTiles()
    {
        List<TilesMapLocation> ListTilesMapLocations = new List<TilesMapLocation>();

        //--- Init tiles constructions
        ListTilesMapLocations.Add(new TilesMapLocation() { Name = "BildTest1", Row = 0, Size = 3 });
        ListTilesMapLocations.Add(new TilesMapLocation() { Name = "BildpolKochBol", Row = 0, Size = 3 });
        ListTilesMapLocations.Add(new TilesMapLocation() { Name = "BildDvor", Row = 0, Size = 5 });
        ListTilesMapLocations.Add(new TilesMapLocation() { Name = "BildBoloto", Row = 0, Size = 5 });
        ListTilesMapLocations.Add(new TilesMapLocation() { Name = "BildZal", Row = 0, Size = 10 });
        ListTilesMapLocations.Add(new TilesMapLocation() { Name = "BildPrefab", Row = 0, Size = 5, TypeTile = TypesStructure.TerraPrefab });

        //m_CollectionDataMapTiles = new Dictionary<string, List<DataTile>>();
        m_CollectionDataMapTiles = new Dictionary<string, DataConstructionTiles>();

        List<DataTile> listDataTiles = new List<DataTile>();

        if (TilesMapBackLayer == null)
        {
            Debug.Log("##### TilesMapBackLayer is Empty");
            return;
        }


        //Layer Terra
        Tilemap tilemapTerra = TilesMapBackLayer.GetComponent<Tilemap>();
        BoundsInt boundsMapTerra = tilemapTerra.cellBounds;
        TileBase[] allTilesTerra = tilemapTerra.GetTilesBlock(boundsMapTerra);
        Dictionary<string, List<DataTile>> listDataMapTilesTerra = new Dictionary<string, List<DataTile>>();

        //----------  Create tiles Terra
        int HeightRow = 5;
        int index = 0;
        //int offsetX = 0;
        int startX = 0;// offsetX + itemTile.Size;
        foreach (var itemTile in ListTilesMapLocations)
        {
            int startY = itemTile.Row * HeightRow;
            itemTile.Position = new Rect(startX, startY, itemTile.Size, itemTile.Size);
            var listDataTilesRes = CreateStructDataTile(itemTile.Name, itemTile.Position, allTilesTerra, boundsMapTerra, itemTile.TypeTile);
            listDataMapTilesTerra.Add(itemTile.Name, listDataTilesRes);

            index++;
            startX += itemTile.Size;
            itemTile.Index = index;
        }
        //-----------------------

        //----------  Create tiles Prefabs
        List<TypesStructure> prefabValid = new List<TypesStructure>()
        {
            TypesStructure.Prefab,
             TypesStructure.FloorPrefab,
             TypesStructure.TerraPrefab,
             TypesStructure.TerraFloorPrefab
        };

        if(TilesMapPrefabLayer==null)
        {
            Debug.Log("##### TilesMapPrefabLayer is Empty");
            return;
        }

        //Layer Prefub
        Tilemap tilemapPrefab = TilesMapPrefabLayer.GetComponent<Tilemap>();
        BoundsInt boundsMapPrefab = tilemapPrefab.cellBounds;
        TileBase[] allTilesPrefab = tilemapPrefab.GetTilesBlock(boundsMapPrefab);
        Dictionary<string, List<DataTile>> listDataMapTilesPrefabs = new Dictionary<string, List<DataTile>>();

        HeightRow = 5;
        index = 0;
        startX = 0;

        //foreach (var itemTile in ListTilesMapLocations.Where(p => prefabValid.Contains(p.TypeTile)))
        foreach (var itemTile in ListTilesMapLocations) //Where Exist Prefab strucrure
        {
            int startY = itemTile.Row * HeightRow;
            if (prefabValid.Contains(itemTile.TypeTile))
            {
                itemTile.Position = new Rect(startX, startY, itemTile.Size, itemTile.Size);
                var listDataTilesRes = CreateStructDataTile(itemTile.Name, itemTile.Position, allTilesPrefab, boundsMapPrefab, TypesStructure.Prefab);
                listDataMapTilesPrefabs.Add(itemTile.Name, listDataTilesRes);
            }
            index++;
            startX += itemTile.Size;
            itemTile.Index = index;
        }
        //-----------------------
      
        foreach (var itemTile in ListTilesMapLocations)
        {
            DataConstructionTiles newDataConstr = new DataConstructionTiles();
            newDataConstr.Name = itemTile.Name;
            newDataConstr.Height = itemTile.Size;
            newDataConstr.Wight = itemTile.Size;
            if(listDataMapTilesTerra.ContainsKey(itemTile.Name))
                newDataConstr.ListDataTileTerra = listDataMapTilesTerra[itemTile.Name];
            if (listDataMapTilesPrefabs.ContainsKey(itemTile.Name))
                newDataConstr.ListDataTilePrefabs = listDataMapTilesPrefabs[itemTile.Name];

            m_CollectionDataMapTiles.Add(itemTile.Name, newDataConstr);
        }

        //-------- Report
        //foreach (var item in m_CollectionDataMapTiles)
        //{
        //    Storage.Events.ListLogAdd = "Structure : " + item.Key;
        //    foreach (DataTile tileData in m_CollectionDataMapTiles[item.Key])
        //    {
        //        Storage.Events.ListLogAdd = "DataTile : " + tileData.Name + " " + tileData.X + "x" + tileData.Y;
        //        Debug.Log("DataTile : " + tileData.Name + " " + tileData.X + "x" + tileData.Y);
        //    }
        //}
        //-----------------

        SaveTilesData();

        
    }

    public List<DataTile> CreateStructDataTile(string NameStructMap, Rect boundsStruct, TileBase[] allTiles, BoundsInt boundsMap, TypesStructure p_tag)
    {
        int countFindTiles = 0;
        List<DataTile> listDataTiles = new List<DataTile>();

        int startX = (int)boundsStruct.x + Math.Abs(boundsMap.x);
        int startY = (int)boundsStruct.y + Math.Abs(boundsMap.y);
        int boundsSizeX = startX + (int)boundsStruct.width;
        int boundsSizeY = startY + (int)boundsStruct.height;

        for (int x = startX; x < boundsSizeX; x++)
        {
            for (int y = startY; y < boundsSizeY; y++)
            {
                TileBase tile = allTiles[x + y * boundsMap.size.x];


                if (tile != null)
                {
                    int cellX = x + -startX;
                    int cellY = y + -startY;

                    DataTile dataTiles = new DataTile()
                    {
                        X = cellX,
                        Y = cellY,
                        Name = tile.name,
                        Tag = p_tag.ToString(),
                    };

                    listDataTiles.Add(dataTiles);
                    countFindTiles++;
                }
            }
        }

        return listDataTiles;
        //m_CollectionDataMapTiles.Add(NameStructMap, listDataTiles);
    }



    public void LoadTextures()
    {
        m_collectionTextureTiles = new Dictionary<string, Texture2D>();
        CollectionTiles = new Dictionary<string, TileBase>();
        m_collectionSpriteTiles = new Dictionary<string, Sprite>();

        var listTiles = Resources.LoadAll<Tile>("Textures/Terra/Floor/Tiles/").ToList();
        var listTilesPrefabs = Resources.LoadAll<Tile>("Textures/TilesPrefab/Tiles/").ToList();
        listTiles.AddRange(listTilesPrefabs);
        m_listTiles = listTiles.ToArray();

        foreach (var tileItem in m_listTiles)
        {
            if (CollectionTiles.ContainsKey(tileItem.name))
            {
                Debug.Log("########## LoadTextures CollectionTiles already exit :" + tileItem.name);
                continue;
            }
            CollectionTiles.Add(tileItem.name, tileItem);
        }

        //m_listTiles = Resources.LoadAll<TileBase>("Textures/Terra/Floor/Tiles/");
        
        foreach (var imageItem in ListTexturs)
        {
            if (m_collectionTextureTiles.ContainsKey(imageItem.name))
            {
                Debug.Log("########## LoadTextures CollectionTextureTiles already exit :" + imageItem.name);
                continue;
            }
            m_collectionTextureTiles.Add(imageItem.name, imageItem);

            Sprite spriteTile = Sprite.Create(imageItem, new Rect(0.0f, 0.0f, imageItem.width, imageItem.height), new Vector2(0.5f, 0.5f), 100.0f);
            m_collectionSpriteTiles.Add(imageItem.name, spriteTile);
        }

        TilesData data = Serializator.LoadTilesXml(Storage.Instance.DataPathTiles);
        if(data!=null)
            m_CollectionDataMapTiles = data.TilesD;
        else
        {
            Debug.Log("############ Not load Tiles from XML " + Storage.Instance.DataPathTiles);
            Storage.TilesManager.UpdateGridTiles();
        }
    }

    public void SaveTilesData()
    {
        TilesData tilesDataSavw = new TilesData()
        {
            TilesD = m_CollectionDataMapTiles
        };
        Serializator.SaveTilesDataXml(tilesDataSavw, Storage.Instance.DataPathTiles, true);
    }

    private string[] m_listNameTiles;


    public string GenNameTileTerra()
    {
        string nameTile = "TileNone";

        if(ListTexturs == null || ListTexturs.Length ==0)
        {
            Debug.Log("###### GenNameTileTerra listTexturs is Empty");
            return nameTile;
        }

        //int ind = 0;
        //int selectedInd = UnityEngine.Random.Range(0, CollectionTiles.Values.Count-1);
        int selectedInd = UnityEngine.Random.Range(0, ListTexturs.Length - 1);
        
        //foreach(var tileItem in CollectionTiles)
        //{
        //    if (ind == selectedInd)
        //    {
        //        TileBase tile = CollectionTiles[tileItem.Key];
        //        nameTile = tile.name;
        //    }
        //    ind++;
        //}
        if (selectedInd> ListTexturs.Length -1)
        {
            Debug.Log("###### GenNameTileTerra selectedInd[" + selectedInd + "] > ListTexturs.Lengt[" + ListTexturs.Length + "]");
            return nameTile;
        }

        nameTile = ListTexturs[selectedInd].name;
        return nameTile;
    }

}

public static class TilemapExtensions
{
    public static T[] GetTiles<T>(this Tilemap tilemap) where T : TileBase
    {
        List<T> tiles = new List<T>();

        for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
        {
            for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
            {
                T tile = tilemap.GetTile<T>(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    tiles.Add(tile);
                }
            }
        }
        return tiles.ToArray();
    }
}


//[XmlRoot("Level")]
//[XmlInclude(typeof(PersonData))]
//public class LevelData
//{
//    public List<KeyValuePair<string, PersonData>> PersonsXML = new List<KeyValuePair<string, PersonData>>();

//    [XmlIgnore]
//    public Dictionary<string, PersonData> Persons = new Dictionary<string, PersonData>();

//    public LevelData() { }
//}

////#D-
//[XmlRoot("Grid")]
//[XmlInclude(typeof(FieldData))]
//public class GridData
//{
//    [XmlArray("Fields")]
//    [XmlArrayItem("Field")]
//    public List<KeyValuePair<string, FieldData>> FieldsXML = new List<KeyValuePair<string, FieldData>>();
//    [XmlIgnore]
//    public Dictionary<string, FieldData> FieldsD = new Dictionary<string, FieldData>();
//    public GridData() { }
//}

public enum TypesStructure
{
    None,
    Terra,
    Floor,
    Prefab,
    Person,
    TerraFloor,
    TerraPrefab,
    FloorPrefab,
    TerraFloorPrefab,
}

public class TilesMapLocation
{
    public string Name;
    public Rect Position;
    public int Row;
    public int Size;
    public int Index;
    public TypesStructure TypeTile = TypesStructure.Terra;
    public TilesMapLocation() { }
}

//[XmlRoot("TilesData")]
//[XmlInclude(typeof(DataTile))]
//public class TilesData
//{
//    //private Dictionary<string, List<DataTile>> CollectionDataMapTales;
//    public List<KeyValuePair<string, List<DataTile>>> TilesXML = new List<KeyValuePair<string, List<DataTile>>>();

//    [XmlIgnore]
//    public Dictionary<string, List<DataTile>> TilesD = new Dictionary<string, List<DataTile>>();

//    public TilesData() { }
//}

[XmlRoot("TilesData")]
[XmlInclude(typeof(DataTile))]
public class TilesData
{
    //private Dictionary<string, List<DataTile>> CollectionDataMapTales;
    public List<KeyValuePair<string, DataConstructionTiles>> TilesXML = new List<KeyValuePair<string, DataConstructionTiles>>();

    [XmlIgnore]
    public Dictionary<string, DataConstructionTiles> TilesD = new Dictionary<string, DataConstructionTiles>();

    public TilesData() { }
}

[XmlType("Construction")]
public class DataConstructionTiles
{
    public string Name { get; set; }

    public int Wight { get; set; }
    public int Height { get; set; }

    //[XmlType("Tile")]
    public List<DataTile> ListDataTileTerra { get; set; }
    public List<DataTile> ListDataTileFloor { get; set; }
    public List<DataTile> ListDataTilePrefabs { get; set; }
    public List<DataTile> ListDataTilePerson { get; set; }

    public DataConstructionTiles() { }
}

[XmlType("Tile")]
public class DataTile
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public bool IsLock { get; set; }

    public DataTile() { }
}


