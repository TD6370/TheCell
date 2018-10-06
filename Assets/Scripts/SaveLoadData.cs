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
    public GameObject DataStorage;
    

    private string _datapathLevel;
    private string _datapathPerson;
    private GridData _gridData;
    private LevelData _personsData;
    private GenerateGridFields _scriptGrid;
    private CreateNPC _scriptNPC;
    private Storage _scriptStorage;

    private float Spacing = 2f;
    int _lmitHorizontalLook = 0;
    int _limitVerticalLook = 0;

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
        InitData();

        LoadData();

        //#.D 
        CreateDataGamesObjectsWorld();

        LoadObjectsNearHero();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitData()
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
        if (_scriptNPC == null)
        {
            Debug.Log("StarLoadData     scriptNPC==null !!!!!");
        }

        var storage = DataStorage;
        if (storage == null)
        {
            Debug.Log("DataStorage null");
            return;
        }
        _scriptStorage = storage.GetComponent<Storage>();
        if (_scriptStorage == null)
        {
            Debug.Log("Error scriptStorage is null !!!");
            return;
        }
        _lmitHorizontalLook = _scriptStorage.LimitHorizontalLook;
        _limitVerticalLook = _scriptStorage.LimitVerticalLook;
    }

    private void LoadData()
    {
        //_datapath = Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";
        //_datapath = Application.dataPath + "/SavedData" + Application.loadedLevel + ".xml";
        _datapathLevel = Application.dataPath + "/Levels/LevelData" + Application.loadedLevel + ".xml";
        Debug.Log("# LoadPathData... " + _datapathLevel);

        if (File.Exists(_datapathLevel))
        {
            _gridData = Serializator.LoadGridXml(_datapathLevel);
        }
        else
        {
            Debug.Log("# LoadPathData not exist: " + _datapathLevel);
        }

        _datapathPerson = Application.dataPath + "/Levels/PersonData" + Application.loadedLevel + ".xml";
        if (File.Exists(_datapathPerson))
        {
            _personsData = Serializator.LoadPersonXml(_datapathPerson);
        }
        else
        {
            Debug.Log("# LoadPathData not exist: " + _datapathPerson);
        }
    }

    //#.D 
    private void CreateDataGamesObjectsWorld(bool isAlwaysCreate = false)
    {
        //# On/Off
        //isAlwaysCreate = true;

        if (_gridData != null && !isAlwaysCreate)
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

        //#D- 
        Dictionary<string, FieldData> dictFields = new Dictionary<string, FieldData>();
        //#D+
        //Dictionary<string, List<ObjectData>> dictFields = new Dictionary<string, List<ObjectData>>();

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
                   //------------------- #D+
                }
            }
        }
        
        GridData data = new GridData()
        {
            //Fields = listFields,
            FieldsD = dictFields
        };

        _gridData = data;
        _scriptGrid.GridData = _gridData;
        Serializator.SaveGridXml(data, _datapathLevel);

        //Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects + "  count fields: " + _scriptGrid.GamesObjectsActive.Count);
        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        //Start generic NPC
        _scriptNPC.SartCrateNPC();
    }

    public class Serializator {

        static Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(GameDataUfo) };

        static public void SaveGridXml(GridData state, string datapath)
        {
            //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
            //## 
            state.FieldsXML = state.FieldsD.ToList();

            //## 
            Debug.Log("SaveXml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);

            XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes);

		    FileStream fs = new FileStream(datapath, FileMode.Create);

		    serializer.Serialize(fs, state); 
		    fs.Close();

            state.FieldsXML = null;
            //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
	    }
	
	    static public GridData LoadGridXml(string datapath){
            string stepErr = "start";
            GridData state = null;
            try
            {
                Debug.Log("Loaded Xml GridData start...");

                stepErr = ".1";
                //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
                stepErr = ".2";
                XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes);
                stepErr = ".3";
                FileStream fs = new FileStream(datapath, FileMode.Open);
                stepErr = ".4";
                state = (GridData)serializer.Deserialize(fs);
                stepErr = ".5";
                fs.Close();

                stepErr = ".6";
                state.FieldsD = state.FieldsXML.ToDictionary(x => x.Key, x => x.Value);
                stepErr = ".7";
                Debug.Log("Loaded Xml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
                //## 
                state.FieldsXML = null;
            }
            catch (Exception x)
            {
                state = null;
                Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
            }

		    return state;
	    }

        static public LevelData LoadPersonXml(string datapath)
        {
            string stepErr = "start";
            LevelData state = null;
            try
            {
                Debug.Log("Loaded Xml GridData start...");

                stepErr = ".1";
                stepErr = ".2";
                XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes);
                stepErr = ".3";
                FileStream fs = new FileStream(datapath, FileMode.Open);
                stepErr = ".4";
                state = (LevelData)serializer.Deserialize(fs);
                stepErr = ".5";
                fs.Close();
                stepErr = ".6";
                state.Persons = state.PersonsXML.ToDictionary(x => x.Key, x => x.Value);
                stepErr = ".7";
                Debug.Log("Loaded Xml CasePersonData :" + state.Persons.Count() + "   XML:" + state.PersonsXML.Count() + "     datapath=" + datapath);
                state.PersonsXML = null;
            }
            catch (Exception x)
            {
                state = null;
                Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
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

    [XmlRoot("Level")]
    [XmlInclude(typeof(PersonData))] 
    public class LevelData
    {
        public List<KeyValuePair<string, PersonData>> PersonsXML = new List<KeyValuePair<string, PersonData>>();

        [XmlIgnore]
        public Dictionary<string, PersonData> Persons = new Dictionary<string, PersonData>();

        public LevelData() { }
    }

    //#D-
    [XmlRoot("Grid")]
    [XmlInclude(typeof(FieldData))]
    public class GridData
    {
        [XmlArray("Fields")]
        [XmlArrayItem("Field")]
        public List<KeyValuePair<string, FieldData>> FieldsXML = new List<KeyValuePair<string, FieldData>>();
        [XmlIgnore]
        public Dictionary<string, FieldData> FieldsD = new Dictionary<string, FieldData>();
        public GridData() { }
    }

    [XmlType("Field")] //++
    [XmlInclude(typeof(ObjectData))]
    public class FieldData
    {
        public string NameField { get; set; }
        [XmlArray("Objects")]
        [XmlArrayItem("ObjectData")]
        public List<ObjectData> Objects = new List<ObjectData>();
        public FieldData() { }
    }
    //#D+
    //[XmlRoot("Grid")]
    //[XmlInclude(typeof(ObjectData))]
    //public class GridData
    //{
    //    [XmlArray("Fields")]
    //    [XmlArrayItem("Field")]
    //    public List<KeyValuePair<string, List<ObjectData>>> FieldsXML = new List<KeyValuePair<string, List<ObjectData>>>();
    //    [XmlIgnore]
    //    public Dictionary<string, List<ObjectData>> FieldsD = new Dictionary<string, List<ObjectData>>();
    //    public GridData() { }
    //}

    //++++
    [XmlType("Object")] //++
    [XmlInclude(typeof(PersonData))] 
    public class ObjectData : ICloneable
    {		
        public string NameObject { get; set; }

        public string TagObject { get; set; }

        //public Vector3 Position { get; set; }
        public virtual Vector3 Position { get; set; }

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

    [XmlType("Person")]
    [XmlInclude(typeof(GameDataUfo))] 
    public class PersonData : ObjectData
    {
        public string Id { get; set; }

        public PersonData() : base()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    [XmlType("Ufo")]
    public class PersonDataUfo : PersonData
    {
        public PersonDataUfo() : base()
        {
        }
    }

    //public class ObjectDataUfo : ObjectData
    public class GameDataUfo :  PersonDataUfo
    {
        [XmlIgnore]
        public Color ColorRender = Color.black;
        [XmlIgnore]
        public Vector3 TargetPosition;

        private Vector3 m_Position = new Vector3(0, 0, 0);
        public override Vector3 Position
        {
            get { return m_Position; }
            set
            {
                m_Position = value;
                if (IsCanSetTargetPosition)
                    SetTargetPosition();
            }
        }

        private bool IsCanSetTargetPosition
        {
            get
            {
                return (TargetPosition == null || TargetPosition == new Vector3(0, 0, 0)) && m_Position != null && m_Position != new Vector3(0, 0, 0);
            }
        }

        public GameDataUfo()
            : base()
        {
            ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);

            //if (IsCanSetTargetPosition)
            //{
            //    //Debug.Log("UFO set TargetPosition after init");
            //    SetTargetPosition();
            //}
        }

        //public void SetTargetPosition(int limX, int limY, int xHero, int yHero)
        //public void SetTargetPosition(Storage.ZonaFieldLook zona)
        public void SetTargetPosition()
        {
            int distX = UnityEngine.Random.Range(-15, 15);
            int distY = UnityEngine.Random.Range(-15, 15);
            //int ufoX = (int)((m_Position.x / 2) + 0.5);
            //int ufoY = (int)((m_Position.y / 2) - 0.5);
            //ufoY = (int)(Mathf.Abs(ufoY));

            float xT = m_Position.x + distX;
            float yT = m_Position.y + distY;

            //validate
            if (yT > -1)
                yT = m_Position.y - distY;
            if (xT < 1)
                xT = m_Position.x - distX;

            //----------------------------- valid Limit look hero
            //Storage.ZonaFieldLook zona = Storage.ZonaField;
            //Storage.ZonaRealLook zona = Storage.ZonaReal;
            //if (zona != null)
            //{
            //    Debug.Log("........Validate zone limit for target position");
            //    if (xT < zona.X)
            //        xT = zona.X + 1;
            //    if (yT < zona.Y)
            //        yT = zona.Y + 1;
            //}
            //else
            //{
            //    Debug.Log("........Validate zone limit for target position: ZonaReal is NULL !!!");
            //}
            //-----------------------------

            TargetPosition = new Vector3(xT, yT, -1);
            //Debug.Log("UFO SetTargetPosition ==== " + TargetPosition);
        }

        //private void SetTargetPosition()
        //{
        //    SetTargetPosition(Storage.ZonaField);
        //}

        public override void UpdateGameObject(GameObject objGame)
        {
            objGame.GetComponent<SpriteRenderer>().color = ColorRender;
            //Debug.Log("UPDATE CLONE THIS " + ((ObjectDataUfo)this).ToString());
            objGame.GetComponent<PersonalData>().PersonalObjectData = (GameDataUfo)this.Clone();
        }

        public override string ToString()
        {
            return NameObject + " " + TagObject + " " + Position + " " + ColorRender;
            //return base.ToString();
        }
    }

    //#PPP
    //[XmlType("Ufo")]
    //public class ObjectDataUfo : ObjectData
    //{
    //    [XmlIgnore]
    //    public Color ColorRender = Color.black;
    //    [XmlIgnore]
    //    public Vector3 TargetPosition;

    //    private Vector3 m_Position = new Vector3(0, 0, 0);
    //    public override Vector3 Position 
    //    {
    //        get { return m_Position; }
    //        set { 
    //            m_Position = value;
    //            if (IsCanSetTargetPosition)
    //                SetTargetPosition();
    //        }
    //    }

    //    private bool IsCanSetTargetPosition {
    //        get {
    //            return (TargetPosition == null || TargetPosition == new Vector3(0, 0, 0)) && m_Position != null && m_Position != new Vector3(0, 0, 0);
    //        }
    //    }

    //    public PersonDataUfo() : base()
    //    {
    //        ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);

    //        //if (IsCanSetTargetPosition)
    //        //{
    //        //    //Debug.Log("UFO set TargetPosition after init");
    //        //    SetTargetPosition();
    //        //}
    //    }

    //    public void SetTargetPosition()
    //    {
    //        int distX = UnityEngine.Random.Range(-15, 15);
    //        int distY = UnityEngine.Random.Range(-15, 15);

    //        float x = m_Position.x + distX;
    //        float y = m_Position.y + distY;
    //        if (y > -1)
    //            y = m_Position.y - distY;
    //        if (x < 1)
    //            x = m_Position.x - distX;

    //        TargetPosition = new Vector3(x, y, -1);
    //        //Debug.Log("UFO SetTargetPosition ==== " + TargetPosition);
    //    }

    //    public override void UpdateGameObject(GameObject objGame)
    //    {
    //        objGame.GetComponent<SpriteRenderer>().color = ColorRender;
    //        //Debug.Log("UPDATE CLONE THIS " + ((ObjectDataUfo)this).ToString());
    //        objGame.GetComponent<PersonalData>().PersonalObjectData = (PersonDataUfo)this.Clone();
    //    }

    //    public override string ToString()
    //    {
    //        return NameObject + " " + TagObject + " " + Position + " " + ColorRender;
    //        //return base.ToString();
    //    }
    //}

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
                objGameBild = new GameDataUfo();
                break;
            default:
                objGameBild = new ObjectData();
                break;
        }
        return objGameBild;
    }

    public void SaveLevel()
    {
        _scriptGrid.SaveAllRealGameObjects();
        //Fields.Remove(nameField);
        //RemoveRealObjects(nameField);

        if (_gridData == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }
        Serializator.SaveGridXml(_gridData, _datapathLevel);
    }

    private void LoadObjectsNearHero()
    {
        _scriptGrid.LoadObjectsNearHero();
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
                    newObject = new GameDataUfo()
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
                    newObject = new GameDataUfo();
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
                    newObject = personalData.PersonalObjectData.Clone() as GameDataUfo;
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


