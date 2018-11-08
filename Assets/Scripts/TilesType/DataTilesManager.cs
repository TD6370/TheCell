using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataTilesManager : MonoBehaviour {

    //--- TAILS ---
    public GameObject BackPalette;
    public Grid GridTails;
    public GameObject TailsMapBackLayer;

    private Texture2D[] m_listTexturs;// = Resources.LoadAll<Texture2D>("Textures/Terra/Floor/");
    private TileBase[] m_listTiles;// = Resources.LoadAll<TileBase>("Textures/Terra/Floor/Tiles/");
    public Dictionary<string, Texture2D> CollectionTextureTiles;
    public Dictionary<string, TileBase> CollectionTiles;

    private Dictionary<string, List<DataTile>> m_CollectionDataMapTales;
    public Dictionary<string, List<DataTile>> DataMapTales
    {
        get
        {
            return m_CollectionDataMapTales;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    public void LoadGridTiles()
    {
        CreateDataTiles();

        LoadTextures();
    }

    public void CreateDataTiles()
    {
        m_CollectionDataMapTales = new Dictionary<string, List<DataTile>>();
        List<DataTile> listDataTiles = new List<DataTile>();

        Tilemap tilemap = TailsMapBackLayer.GetComponent<Tilemap>();

        BoundsInt boundsMap = tilemap.cellBounds;

        //BoundsInt bounds = new BoundsInt(boundsStruct.x + boundsMap.x, boundsStruct.y + boundsMap.y, boundsStruct.size);
        //BoundsInt bounds = new BoundsInt(new Vector3Int(boundsStruct.x - boundsMap.x, boundsStruct.y - boundsMap.y, 0), new Vector3Int(boundsStruct.size.x, boundsStruct.size.y, 1));

        TileBase[] allTiles = tilemap.GetTilesBlock(boundsMap);

        CreateStructDataTile("BildTest1", new Rect(0, 0, 3, 3), allTiles, boundsMap, TypesStructure.Terra);
        CreateStructDataTile("BildpolKochBol", new Rect(3, 0, 3, 3), allTiles, boundsMap, TypesStructure.Terra);

        foreach (var item in m_CollectionDataMapTales)
        {
            Storage.Events.ListLogAdd = "Structure : " + item.Key;
            foreach (DataTile tileData in m_CollectionDataMapTales[item.Key])
            {
                Storage.Events.ListLogAdd = "DataTile : " + tileData.Name + " " + tileData.X + "x" + tileData.Y;
                Debug.Log("DataTile : " + tileData.Name + " " + tileData.X + "x" + tileData.Y);
            }
        }

        SaveTilesData();
    }

    public void CreateStructDataTile(string NameStructMap, Rect boundsStruct, TileBase[] allTiles, BoundsInt boundsMap, TypesStructure p_tag)
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

        m_CollectionDataMapTales.Add(NameStructMap, listDataTiles);

    }

    public void LoadTextures()
    {
        CollectionTextureTiles = new Dictionary<string, Texture2D>();
        CollectionTiles = new Dictionary<string, TileBase>();

        m_listTiles = Resources.LoadAll<Tile>("Textures/Terra/Floor/Tiles/");
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
        m_listTexturs = Resources.LoadAll<Texture2D>("Textures/Terra/Floor/");
        foreach (var imageItem in m_listTexturs)
        {
            if (CollectionTextureTiles.ContainsKey(imageItem.name))
            {
                Debug.Log("########## LoadTextures CollectionTextureTiles already exit :" + imageItem.name);
                continue;
            }
            CollectionTextureTiles.Add(imageItem.name, imageItem);
        }

        TilesData data = Serializator.LoadTilesXml(Storage.Instance.DataPathTiles);
        if(data!=null)
            m_CollectionDataMapTales = data.TilesD;
    }

    public void SaveTilesData()
    {
        TilesData tilesDataSavw = new TilesData()
        {
            TilesD = m_CollectionDataMapTales
        };
        Serializator.SaveTilesDataXml(tilesDataSavw, Storage.Instance.DataPathTiles, true);
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
    Terra,
    Floor,
    Wall,
    Person
}

[XmlRoot("TilesData")]
[XmlInclude(typeof(DataTile))]
public class TilesData
{
    //private Dictionary<string, List<DataTile>> CollectionDataMapTales;
    public List<KeyValuePair<string, List<DataTile>>> TilesXML = new List<KeyValuePair<string, List<DataTile>>>();

    [XmlIgnore]
    public Dictionary<string, List<DataTile>> TilesD = new Dictionary<string, List<DataTile>>();

    public TilesData() { }
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


