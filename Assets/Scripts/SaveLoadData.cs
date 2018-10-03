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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadDataGrid()
    {
        _datapath = Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";

        if (File.Exists(_datapath))	
            _gridData = Serializator.DeXml(_datapath);  
        if (_gridData != null)
        {
            Dictionary<string, List<GameObject>> _gamesObjectsActive = new Dictionary<string, List<GameObject>>();
            foreach (var field in _gridData.Fields)
            {
                 List<GameObject> ListNewObjects = new List<GameObject>();
                 foreach (ObjectData objGame in field.Objects)
                 {
                     GameObject newObjGame = CreatePrefabByObjectData(objGame);
                     if (newObjGame != null)
                         ListNewObjects.Add(newObjGame);
                 }
                _gamesObjectsActive.Add(field.NameField, ListNewObjects);

            }
            _scriptGrid.GamesObjectsActive = _gamesObjectsActive;
        }
    }

    private void CreateGameObjects()
    {
        Dictionary<string, List<GameObject>> _gamesObjectsActive = new Dictionary<string, List<GameObject>>();
        int maxWidth = 1000;// (int)GridY * -1;
        int maxHeight = 1000; //(int)GridX;

        for (int y = 0; y > maxWidth; y--)
        {
            for (int x = 0; x < maxHeight; x++)
            {
                int maxObjectInField = 1;
                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                List<GameObject> ListNewObjects = new List<GameObject>();
                for(int i=0; i< maxObjectInField; i++){

                    //Type prefab
                    int intTypePrefab = UnityEngine.Random.Range(0, 3);
                    TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;

                    ObjectData objGame = new ObjectData()
                    {
                        NameObject = prefabName.ToString(),
                        TagObject = prefabName.ToString(), 
                        Position = new Vector3(1,1,0)
                    };
                    GameObject newObjGame = CreatePrefabByObjectData(objGame);
                    if (newObjGame != null)
                        ListNewObjects.Add(newObjGame);
                }
                _gamesObjectsActive.Add(nameFiled, ListNewObjects);

            }
        }
        _scriptGrid.GamesObjectsActive = _gamesObjectsActive;
    }

    //public Dictionary<string, List<GameObject>> GamesObjectsActive;
    //public void SaveGrid(Dictionary<string, List<GameObject>> p_gamesObjectsActive)
    private void SaveGrid()
    {
        Dictionary<string, List<GameObject>> p_gamesObjectsActive = _scriptGrid.GamesObjectsActive;

        List<FieldData> listFields = new List<FieldData>();

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


    private GameObject FindPrefab(string namePrefab)
    {
        return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
    }

    

    //public GameObject prefab1;
    private GameObject CreatePrefabByObjectData(ObjectData objGameData)
    {
        string nameFind = objGameData.NameObject;
        string tagFind = objGameData.TagObject;
        Vector3 pos = objGameData.Position;
        GameObject newPrefab = null;
        
        //Find prefab !!!
        //var findPrefab = FindPrefab;

        string typeFind = String.IsNullOrEmpty(nameFind) ? tagFind : nameFind;
        newPrefab = FindPrefab(typeFind);
        if (newPrefab == null)
            return null;

        GameObject newObjGame = (GameObject)Instantiate(newPrefab, pos, Quaternion.identity);
        newObjGame.name = nameFind;
        newObjGame.tag = tagFind;

        return newObjGame;
    }

    private ObjectData CreateObjectData(GameObject p_gobject)
    {
        ObjectData newObject = new ObjectData()
        {
            NameObject = p_gobject.name,
            TagObject = p_gobject.tag,
            Position = p_gobject.transform.position
        };

        return newObject;
    }

    public class Serializator {

        static public void SaveXml(GridData state, string datapath)
        {

            Type[] extraTypes = { typeof(FieldData), typeof(ObjectData) };
            XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes); 

		    FileStream fs = new FileStream(datapath, FileMode.Create); 
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

    
}
