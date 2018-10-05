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
    private CreateNPC _scriptNPC;

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

        _scriptNPC = GetComponent<CreateNPC>();
        if(_scriptNPC==null)
        {
            Debug.Log("StarLoadData     scriptNPC==null !!!!!");
        }

        LoadData();

        //#.D 
        CreateDataGamesObjectsWorld();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadData()
    {
        //_datapath = Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";
        //_datapath = Application.dataPath + "/SavedData" + Application.loadedLevel + ".xml";
        _datapath = Application.dataPath + "/Levels/LevelData" + Application.loadedLevel + ".xml";
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
        if (_gridData != null)
        {
            Debug.Log("# CreateDataGamesObjectsWorld... Game is loaded");
            _scriptGrid.GridData = _gridData;
            _scriptNPC.SartCrateNPC();
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
                    int intTypePrefab = UnityEngine.Random.Range(1, 4);
                    //#TT NOT UFO
                    //int intTypePrefab = UnityEngine.Random.Range(1, 3);

                    TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

                    string nameObject = CreateName(prefabName.ToString(), nameFiled);// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ObjectData objGameSave = BildObjectData(prefabName);
                    objGameSave.NameObject = nameObject;
                    objGameSave.TagObject = prefabName.ToString();
                    objGameSave.Position = pos;

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
            }
        }
        
        GridData data = new GridData()
        {
            Fields = listFields,
            FieldsD = dictFields
        };

        _gridData = data;
        _scriptGrid.GridData = _gridData;
        Serializator.SaveXml(data, _datapath);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects + "  count fields: " + _scriptGrid.GamesObjectsActive.Count);

        //Start generic NPC
        _scriptNPC.SartCrateNPC();
    }

    public class Serializator {

        static public void SaveXml(GridData state, string datapath)
        {
            //D_2 
            //Debug.Log("SaveXml GridData " + state.Fields.Count + "  " + state.FieldsD.Count + "     datapath=" + datapath);
            //Debug.Log("SaveXml GridData " + state.Fields.Count + "       datapath=" + datapath);


            //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData) };
            Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
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

    public class ObjectData : ICloneable
    {		
        public string NameObject { get; set; }

        public string TagObject { get; set; }

        public Vector3 Position { get; set; }

        public ObjectData() {
        }

        public virtual void UpdateGameObject(GameObject objGame)
        {
            var tempData = objGame.GetComponent<PersonalData>();
            if (tempData != null)
            {
                Debug.Log(">>>> UpdateGameObject  SAVE PERSONAL DATA ");

                tempData.PersonalObjectData = (ObjectData)this.Clone();
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone(); 
        }

        public override string ToString()
        {
            return NameObject + " " + TagObject + " " + Position;
            //return base.ToString();
        }
    }

    //#PPP
    public class ObjectDataUfo : ObjectData
    {
        [XmlIgnore]
        public Color ColorRender = Color.black;

        public ObjectDataUfo() : base()
        {
            ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            objGame.GetComponent<SpriteRenderer>().color = ColorRender;
            //Debug.Log("UPDATE CLONE THIS " + ((ObjectDataUfo)this).ToString());
            objGame.GetComponent<PersonalData>().PersonalObjectData = (ObjectDataUfo)this.Clone();
        }

        public override string ToString()
        {
            return NameObject + " " + TagObject + " " + Position + " " + ColorRender;
            //return base.ToString();
        }
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

    private ObjectData BildObjectData(TypePrefabs prefabType)
    {
        ObjectData objGameBild;

        switch (prefabType)
        {
            case SaveLoadData.TypePrefabs.PrefabUfo:
                objGameBild = new ObjectDataUfo();
                break;
            default:
                objGameBild = new ObjectData();
                break;
        }
        return objGameBild;
    }

    //+++ CreatePrefabByName +++
    public static ObjectData CreateObjectData(GameObject p_gobject, bool isNewGen = false)
    {
        ObjectData newObject;
        //#PPPP
        TypePrefabs prefabType = TypePrefabs.PrefabField;
        
        if(!String.IsNullOrEmpty(p_gobject.tag))
            prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), p_gobject.tag.ToString()); ;

        switch (prefabType) 
        { 
            case TypePrefabs.PrefabUfo:
                if (isNewGen)
                {
                    newObject = new ObjectDataUfo()
                    {
                        NameObject = p_gobject.name,
                        TagObject = p_gobject.tag,
                        Position = p_gobject.transform.position
                    };
                    //NEW DATA  -------->>>>  PERSONA  #P#
                    newObject.UpdateGameObject(p_gobject);
                    //Debug.Log("DATA NewGen PrefabUfo (" + p_gobject.name + ") SAVE : " + testC + " to:" + newObject.ToString());
                }
                else
                {
                    newObject = new ObjectDataUfo();
                    var personalData = p_gobject.GetComponent<PersonalData>();
                    if (personalData == null)
                    {
                        Debug.Log("ERROR CreateObjectData " + prefabType.ToString() + " not Personal Data !!!!");
                        break;
                    }
                    if (personalData.PersonalObjectData == null){
                        Debug.Log("ERROR CreateObjectData Personal Data is Empty !!!!");
                        break;
                    }

                    //RemoveRealObjects Update DATA <<<<------- PERSONA  #P#
                    newObject = personalData.PersonalObjectData.Clone() as ObjectDataUfo;
                    if (newObject == null){
                        Debug.Log("ERROR CreateObjectData PrefabUfo not is  ObjectDataUfo !!!!");
                        break;
                    }

                    //Debug.Log("DATA... UPDATE!!  PrefabUfo (" + p_gobject.name + ") SAVE Color =========== " + ((ObjectDataUfo)newObject).ColorRender);
                }
                break;
            default:
                newObject = new ObjectData()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position
                };
                break;
        }
        return newObject;
    }

    ////+++ CreatePrefabByName +++
    //public static ObjectData FindObjectData(GameObject p_gobject)
    //{
    //}

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


