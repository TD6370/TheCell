using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Linq;


public class SaveLoadData : MonoBehaviour {

    //public GenerateGridFields ScriptGridGeneric;
    public GameObject prefabVood;
    public GameObject prefabRock;
    public GameObject prefabUfo;
    public Camera MainCamera;

    private string _datapath;
    private GridData _gridData;
    private GenerateGridFields _scriptGrid;
    private float Spacing = 2f;

    private List<string> _namesPrefabs = new List<string>
    {
        "PrefabField","PrefabRock","PrefabVood","PrefabUfo" //,"","","",""
    };

    public enum TypePrefabs
    {
        PrefabField,
        PrefabRock,
        PrefabVood,
        PrefabUfo
    }
    

    void Start()
    {
        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("MainCamera null");
            return;
        }
        _scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
        //Dictionary<string, GameObject> Fields = scriptGrid.Fields;

        LoadPathData();

        //LoadDataGrid();

        //CreateGamesObjectsWorld();
        
        //#.D 
        CreateDataGamesObjectsWorld();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadPathData()
    {
        //_datapath = Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";
        _datapath = Application.dataPath + "/SavedData" + Application.loadedLevel + ".xml";
        Debug.Log("# LoadPathData... " + _datapath);

        if (File.Exists(_datapath))
        {
            _gridData = Serializator.DeXml(_datapath);
        }
        else
        {
            Debug.Log("# LoadPathData not exist: " + _datapath);
        }
    }

    //#.D 
    private void CreateDataGamesObjectsWorld()
    {
        //return; //Always geretate world now


        //if (_gridData == null)
        //{
        //    Debug.Log("# CreateDataGamesObjectsWorld... gridData IS EMPTY");
        //    return;
        //}
        if (_gridData != null)
        {
            Debug.Log("# CreateDataGamesObjectsWorld... Game is loaded");
            _scriptGrid.GridData = _gridData;
            Debug.Log("CreateDataGamesObjectsWorld IN Data World");

            return;
        }

        Dictionary<string, List<GameObject>> _gamesObjectsActive = new Dictionary<string, List<GameObject>>();
        int maxWidth = 100;// (int)GridY * -1;
        int maxHeight = 100; //(int)GridX;
        int coutCreateObjects = 0;


        Debug.Log("# CreateDataGamesObjectsWorld...");

        List<FieldData> listFields = new List<FieldData>();
        Dictionary<string, FieldData> dictFields = new Dictionary<string, FieldData>();

        for (int y = 0; y < maxWidth; y++)
        {
            for (int x = 0; x < maxHeight; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 3);

                int maxObjectInField = (intRndCount == 0) ? 1 : 0;
                string nameFiled = GenerateGridFields.GetNameField(x, y);

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {

                    //Type prefab
                    //int intTypePrefab = UnityEngine.Random.Range(1, 3);
                    int intTypePrefab = UnityEngine.Random.Range(1, 4);
                    //DebugLogT("CreateGamesObjectsWorld  " + nameFiled + "  intTypePrefab=" + intTypePrefab);

                    TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;
                    //DebugLogT("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName);

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

                    string nameOnject = CreateName(prefabName.ToString(), nameFiled);// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ObjectData objGameSave = new ObjectData()
                    {
                        NameObject = nameOnject,
                        TagObject = prefabName.ToString(),
                        //Position = new Vector3(x, y*(-1), -1) 
                        Position = pos
                    };

                    coutCreateObjects++;

                    FieldData fieldData;
                    fieldData = listFields.Find(p => p.NameField == nameFiled);
                    //create new Field in data
                    if (fieldData == null)
                    {
                        fieldData = new FieldData() { NameField = nameFiled };
                        listFields.Add(fieldData);
                    }
                    fieldData.Objects.Add(objGameSave);

                    //#.D_2 -------------------
                    FieldData fieldData2;
                    if (!dictFields.ContainsKey(nameFiled))
                    {
                        fieldData2 = new FieldData() { NameField = nameFiled };
                        dictFields.Add(nameFiled, fieldData2);
                    }
                    else
                    {
                        fieldData2 = dictFields[nameFiled];
                    }
                    fieldData2.Objects.Add(objGameSave);
                   //-------------------
                }
                //# _gamesObjectsActive.Add(nameFiled, ListNewObjects);
            }
        }
        
        //_scriptGrid.GamesObjectsActive = _gamesObjectsActive;
        GridData data = new GridData()
        {
            Fields = listFields,
            FieldsD = dictFields
        };

        _gridData = data;
        _scriptGrid.GridData = _gridData;
        Serializator.SaveXml(data, _datapath);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects + "  count fields: " + _scriptGrid.GamesObjectsActive.Count);

        //step 2.
        //SaveGrid();
        //Dictionary<string, List<GameObject>> p_gamesObjectsActive = _scriptGrid.GamesObjectsActive;

        //step 3.
        //LoadDataGrid();
    }

    private void CreateGamesObjectsWorld()
    {
        Dictionary<string, List<GameObject>> _gamesObjectsActive = new Dictionary<string, List<GameObject>>();
        int maxWidth = 100;// (int)GridY * -1;
        int maxHeight = 100; //(int)GridX;
        int coutCreateObjects = 0;
        

        Debug.Log("# CreateGamesObjectsWorld...");

        for (int y = 0; y < maxWidth; y++)
        {
            for (int x = 0; x < maxHeight; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 3);
                //Debug.Log("CreateGamesObjectsWorld intRndCount intRndCount=" + intRndCount);

                int maxObjectInField = (intRndCount==0)? 1: 0;
                //string nameFiled  = "Filed" + x + "x" + Mathf.Abs(y);
                string nameFiled  = GenerateGridFields.GetNameField(x,y);

                List<GameObject> ListNewObjects = new List<GameObject>();
                for(int i=0; i< maxObjectInField; i++){

                    //Type prefab
                    //int intTypePrefab = UnityEngine.Random.Range(1, 3);
                    int intTypePrefab = UnityEngine.Random.Range(1, 4);
                    //DebugLogT("CreateGamesObjectsWorld  " + nameFiled + "  intTypePrefab=" + intTypePrefab);
                    
                    TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;
                    //DebugLogT("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName);

                    int _y = y*(-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

                    string nameOnject = prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ObjectData objGame = new ObjectData()
                    {
                        NameObject = nameOnject,
                        TagObject = prefabName.ToString(),
                        //Position = new Vector3(x, y*(-1), -1) 
                        Position = pos 
                    };
                    GameObject newObjGame = CreatePrefabByObjectData(objGame);
                    if (newObjGame != null)
                    {
                        ListNewObjects.Add(newObjGame);
                        //Debug.Log("CreateGamesObject IN Data World ++++ " + nameFiled + "   " + nameOnject);
                        coutCreateObjects++;
                    }
                }
                _gamesObjectsActive.Add(nameFiled, ListNewObjects);
            }
        }
        _scriptGrid.GamesObjectsActive = _gamesObjectsActive;
        _scriptGrid.GridData = _gridData;

        Debug.Log("CreateGamesObject IN Data World COUNT====" + coutCreateObjects + "     count fields: " + _scriptGrid.GamesObjectsActive.Count);

        //step 2.
        //SaveGrid();

        //step 3.
        //LoadDataGrid();
    }

    //public Dictionary<string, List<GameObject>> GamesObjectsActive;
    //public void SaveGrid(Dictionary<string, List<GameObject>> p_gamesObjectsActive)
    private void SaveGrid()
    {
        Debug.Log("# SaveGrid...");

        Dictionary<string, List<GameObject>> p_gamesObjectsActive = _scriptGrid.GamesObjectsActive;

        List<FieldData> listFields = new List<FieldData>();

        Debug.Log("# SaveGrid count object=" + p_gamesObjectsActive.Count);

        foreach (var item in p_gamesObjectsActive)
        {
            List<GameObject> gobjects = item.Value;
            var nameFiled = item.Key;

            FieldData fieldData;

            fieldData = listFields.Find(p => p.NameField == nameFiled);
            //create new Field in data
            if (fieldData == null)
            {
                fieldData = new FieldData() { NameField = nameFiled};
                listFields.Add(fieldData);
            }

            if(gobjects.Count>0)
                Debug.Log("# SaveGrid " + nameFiled + " add object=" + gobjects.Count);

            foreach (var obj in gobjects)
            {
                ObjectData objectSave = CreateObjectData(obj);
                fieldData.Objects.Add(objectSave);
            }
        }

        GridData data = new GridData()
        {
            Fields = listFields
        };

        Serializator.SaveXml(data, _datapath);
    }

    private void LoadDataGrid()
    {
        //_datapath = Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";
        if (_gridData == null)
        {
            Debug.Log("# LoadDataGrid... gridData IS EMPTY");
            return;
        }

        Debug.Log("# LoadDataGrid... " + _datapath);

        Dictionary<string, List<GameObject>> _gamesObjectsActive = new Dictionary<string, List<GameObject>>();
        foreach (var field in _gridData.Fields)
        {
            Debug.Log("# LoadDataGrid field: " + field.NameField);

            List<GameObject> ListNewObjects = new List<GameObject>();
            foreach (ObjectData objGame in field.Objects)
            {
                Debug.Log("# LoadDataGrid objGame: " + objGame.NameObject + "   " + objGame.TagObject);

                GameObject newObjGame = CreatePrefabByObjectData(objGame);
                if (newObjGame != null)
                    ListNewObjects.Add(newObjGame);
            }
            _gamesObjectsActive.Add(field.NameField, ListNewObjects);

        }
        _scriptGrid.GamesObjectsActive = _gamesObjectsActive;

    }


    public class Serializator {

        static public void SaveXml(GridData state, string datapath)
        {
            //D_2 
            //Debug.Log("SaveXml GridData " + state.Fields.Count + "  " + state.FieldsD.Count + "     datapath=" + datapath);
            //Debug.Log("SaveXml GridData " + state.Fields.Count + "       datapath=" + datapath);


            Type[] extraTypes = { typeof(FieldData), typeof(ObjectData) };
            //## 
            //state.FieldsIXML = state.FieldsD;
            state.FieldsXML = state.FieldsD.ToList();

            //## 
            Debug.Log("SaveXml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);

            XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes);

		    FileStream fs = new FileStream(datapath, FileMode.Create);

		    serializer.Serialize(fs, state); 
		    fs.Close();

            //## 
            state.FieldsXML = null;
            //state.FieldsIXML = null;

            //## 
            //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
	    }
	
	    static public GridData DeXml(string datapath){
            GridData state = null;
            try
            {
                Debug.Log("Loaded Xml GridData start...");

                //Type[] extraTypes= { typeof(PositData), typeof(Lamp)};
                //XmlSerializer serializer = new XmlSerializer(typeof(RoomState), extraTypes);
                Type[] extraTypes = { typeof(FieldData), typeof(ObjectData) };
                XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes);

                FileStream fs = new FileStream(datapath, FileMode.Open);
                state = (GridData)serializer.Deserialize(fs);
                fs.Close();

                //state.FieldsD = new Dictionary<string, FieldData>();
                //state.FieldsD = ToDict(state.FieldsXML).ToDictionary(x => x.Key, x => x.Value);
                //## 
                state.FieldsD = state.FieldsXML.ToDictionary(x => x.Key, x => x.Value);

                Debug.Log("Loaded Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
                //## 
                state.FieldsXML = null;
            }
            catch (Exception x)
            {
                state = null;
                Debug.Log("Error DeXml: " + x.Message);
            }

		    return state;
	    }

        //public static IEnumerable<KeyValuePair<string, FieldData>> ToDict(IEnumerable<KeyValuePair<string, FieldData>> list)
        //{
        //    foreach (var item in list)
        //    {
        //        yield return new KeyValuePair<string, FieldData>(item.Key, item.Value);
        //    }
        //}
    }

    [XmlRoot("Grid")]
    [XmlInclude(typeof(FieldData))] 
    public class GridData
    {
        [XmlArray("Fields")]
        [XmlArrayItem("FieldData")]
        public List<FieldData> Fields = new List<FieldData>();

        //[XmlArray("FieldsD")]
        //[XmlArrayItem("FieldDataD")]
        //public Dictionary<string, FieldData> FieldsD = new Dictionary<string, FieldData>();
        //public Dictionary<int, string> FieldsD = new Dictionary<int, string>();

        public List<KeyValuePair<string, FieldData>> FieldsXML = new List<KeyValuePair<string, FieldData>>();
        
        //[XmlIgnore]
        //public IEnumerable<KeyValuePair<string, FieldData>> FieldsIXML = new List<KeyValuePair<string, FieldData>>();
        //public IEnumerable<KeyValuePair<int, String>> FieldsXML2 = new List<KeyValuePair<int, String>>();

        [XmlIgnore]
        public Dictionary<string, FieldData> FieldsD = new Dictionary<string, FieldData>();

        public GridData() { }
    }

    [XmlInclude(typeof(ObjectData))] 
    public class FieldData
    {
        public string NameField { get; set; }

        [XmlArray("Objects")]
        [XmlArrayItem("ObjectsData")]
        public List<ObjectData> Objects = new List<ObjectData>();

        public FieldData() { }  

    }

    public class ObjectData
    {		
        //public string NameField { get; set; }

        public string NameObject { get; set; }

        public string TagObject { get; set; }

        public Vector3 Position { get; set; }

        public ObjectData() { } 
    }

    private void DebugLog(string log)
    {
        Debug.Log(log);
    }

    private void DebugLogT(string log)
    {
        return;
        Debug.Log(log);
    }

    public static ObjectData CreateObjectData(GameObject p_gobject)
    {
        //Debug.Log("# CreateObjectData from " + p_gobject.name + " " + p_gobject.tag);

        ObjectData newObject = new ObjectData()
        {
            NameObject = p_gobject.name,
            TagObject = p_gobject.tag,
            Position = p_gobject.transform.position
        };
        return newObject;
    }

    //#TEST
    //public static GameObject CreatePrefabByObjectData(ObjectData objGameData)
    public GameObject CreatePrefabByObjectData(ObjectData objGameData)
    {
        string nameFind = objGameData.NameObject;
        string tagFind = objGameData.TagObject;
        Vector3 pos = objGameData.Position;
        GameObject newPrefab = null;

        string typeFind = String.IsNullOrEmpty(tagFind) ? nameFind : tagFind;

        newPrefab = FindPrefab(typeFind);

        if (newPrefab == null)
        {
            Debug.Log("# CreatePrefabByObjectData Not Find Prefab =" + typeFind);
            return null;
        }

        GameObject newObjGame = (GameObject)Instantiate(newPrefab, pos, Quaternion.identity);
        newObjGame.name = nameFind;
        //Hide active object
        newObjGame.SetActive(false);

        return newObjGame;
    }

    public GameObject FindPrefab(string namePrefab)
    {
        //#TEST #PREFABF.1
        //return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
        //#TEST #PREFABF.2
        return FindPrefabHieracly(namePrefab);
    }

    private GameObject FindPrefabHieracly(string namePrefab)
    {
        try
        {
            TypePrefabs prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), namePrefab);
            GameObject resPrefab = null;

            switch (prefabType)
            {
                case TypePrefabs.PrefabRock:
                    resPrefab = Instantiate(prefabRock);
                    break;
                case TypePrefabs.PrefabVood:
                    resPrefab = Instantiate(prefabVood);
                    break;
                case TypePrefabs.PrefabUfo:
                    resPrefab = Instantiate(prefabUfo);
                    break;
                default:
                    Debug.Log("!!! FindPrefabHieracly no type : " + prefabType.ToString());
                    break;

            }
            //Debug.Log("FindPrefabHieracly: " + prefabType.ToString());
            return resPrefab;
        }
        catch (Exception x)
        {
            Debug.Log("Error FindPrefabHieracly: " + x.Message);
        }
        return null;
    }

    public static string CreateName(string tag, string nameFiled, int i=0)
    {
        return tag + "_" + nameFiled + "_" + i;
    }

    //public static class DictionaryExtensions
    //{
    //    public static IEnumerable<KeyValuePair<int, string>> GetComponents(this IEnumerable<KeyValuePair<int, string>> thisObj)
    //    {

    //        yield return new KeyValuePair<int, string>(1111, "aaa");
    //    }
    //}

}


