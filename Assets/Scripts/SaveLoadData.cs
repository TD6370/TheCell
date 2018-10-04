using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO; 


public class SaveLoadData : MonoBehaviour {

    //public GenerateGridFields ScriptGridGeneric;
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

        //LoadDataGrid();

        CreateGamesObjectsWorld();
	}
	
	// Update is called once per frame
	void Update () {
		
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
                string nameFiled  = GenerateGridFields.GetNameFiled(x,y);

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
        _datapath = Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";

        Debug.Log("# LoadDataGrid... " + _datapath);

        if (File.Exists(_datapath))
        {
            _gridData = Serializator.DeXml(_datapath);
        }
        else
        {
            Debug.Log("# LoadDataGrid not exist: " + _datapath);
        }
        if (_gridData != null)
        {
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
    }


    private GameObject FindPrefab(string namePrefab)
    {
        return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
    }

    //public GameObject prefab1;
    private GameObject CreatePrefabByObjectData(ObjectData objGameData)
    {
        DebugLogT("# CreatePrefabByObjectData... " + objGameData.NameObject);

        string nameFind = objGameData.NameObject;
        string tagFind = objGameData.TagObject;
        Vector3 pos = objGameData.Position;
        GameObject newPrefab = null;
        
        //Find prefab !!!
        //var findPrefab = FindPrefab;

        //string typeFind = String.IsNullOrEmpty(nameFind) ? tagFind : nameFind;
        string typeFind = String.IsNullOrEmpty(tagFind) ? nameFind : tagFind;
        //Debug.Log("# CreatePrefabByObjectData typeFind =" + typeFind);

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

        //Debug.Log("# CreatePrefabByObjectData Create GameObject TAG  : " + newObjGame.tag);
        //Debug.Log("# CreatePrefabByObjectData Create GameObject SET TAG !!! : " + tagFind);
        //newObjGame.tag = null;
        //newObjGame.tag = tagFind;

        DebugLogT("# CreatePrefabByObjectData Create GameObject ++++ " + newObjGame.name);

        return newObjGame;
    }

    

    public class Serializator {

        static public void SaveXml(GridData state, string datapath)
        {

            Type[] extraTypes = { typeof(FieldData), typeof(ObjectData) };


            XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes);
            //XmlSerializer serializer2 = new XmlSerializer(typeof(item[]),
            //                     new XmlRootAttribute() { ElementName = "items" });

		    FileStream fs = new FileStream(datapath, FileMode.Create);

            //serializer.Serialize(fs,
            //  state.FieldsD.Select(kv => new item() { id = kv.Key, value = kv.Value }).ToArray());

		    serializer.Serialize(fs, state); 
		    fs.Close(); 

	    }
	
	    static public GridData DeXml(string datapath){

		    //Type[] extraTypes= { typeof(PositData), typeof(Lamp)};
		    //XmlSerializer serializer = new XmlSerializer(typeof(RoomState), extraTypes);
            Type[] extraTypes = { typeof(FieldData), typeof(ObjectData) };
            XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes); 
		
		    FileStream fs = new FileStream(datapath, FileMode.Open); 
		    GridData state = (GridData)serializer.Deserialize(fs); 
		    fs.Close(); 

		    return state;
	    }
    }

    [XmlRoot("Grid")]
    [XmlInclude(typeof(FieldData))] 
    public class GridData
    {
        [XmlArray("Fields")]
        [XmlArrayItem("FieldData")]
        public List<FieldData> Fields = new List<FieldData>();

        [XmlArray("FieldsD")]
        [XmlArrayItem("FieldDataD")]
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
        Debug.Log("# CreateObjectData from " + p_gobject.name + " " + p_gobject.tag);

        ObjectData newObject = new ObjectData()
        {
            NameObject = p_gobject.name,
            TagObject = p_gobject.tag,
            Position = p_gobject.transform.position
        };
        return newObject;
    }

}
