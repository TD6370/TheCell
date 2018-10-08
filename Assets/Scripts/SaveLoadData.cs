using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Linq;

public class SaveLoadData : MonoBehaviour {

    //public GenerateGridFields ScriptGridGeneric;
    public GameObject PrefabVood;
    public GameObject PrefabRock;
    public GameObject PrefabUfo;
    public GameObject PrefabBoss;
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
        PrefabUfo,
        PrefabBoss
    }
    

    void Start()
    {
        InitData();

        LoadData();

        //#.D 
        CreateDataGamesObjectsWorld();

        LoadObjectsNearHero();

        _scriptNPC.SartCrateNPC();
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
        isAlwaysCreate = true;

        if (_gridData != null && !isAlwaysCreate)
        {
            Debug.Log("# CreateDataGamesObjectsWorld... Game is loaded");
            _scriptGrid.GridData = _gridData;
            //_scriptNPC.SartCrateNPC();
            Storage.SetGridData(_gridData);
            return;
        }

        Dictionary<string, List<GameObject>> _gamesObjectsActive = new Dictionary<string, List<GameObject>>();
        int coutCreateObjects = 0;

        Debug.Log("# CreateDataGamesObjectsWorld...");

        //#D- 
        Dictionary<string, FieldData> dictFields = new Dictionary<string, FieldData>();
        //#D+
        //Dictionary<string, List<ObjectData>> dictFields = new Dictionary<string, List<ObjectData>>();

        for (int y = 0; y < Storage.WidthLevel; y++)
        {
            for (int x = 0; x < Storage.HeightLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 3);

                int maxObjectInField = (intRndCount == 0) ? 1 : 0;
                string nameFiled = Storage.GetNameField(x, y);

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {

                    //Type prefab
                    //int intTypePrefab = UnityEngine.Random.Range(1, 4);
                    //#TT NOT UFO
                    int intTypePrefab = UnityEngine.Random.Range(1, 3);

                    TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

                    string nameObject = CreateName(prefabName.ToString(), nameFiled, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
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
        Storage.SetGridData(_gridData);
        Serializator.SaveGridXml(data, _datapathLevel);

        //Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects + "  count fields: " + _scriptGrid.GamesObjectsActive.Count);
        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        //Start generic NPC
        //_scriptNPC.SartCrateNPC();
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

        //public virtual void NextPosition(Vector3 p_newPosition)
        //{
        //    string posFieldOld = Storage.GetNameFieldPosit(Position.x, Position.y);
        //    string posFieldReal = Storage.GetNameFieldPosit(p_newPosition.x, p_newPosition.y);

        //    //!!!!!!!!!!!! p_nameField === posFieldOld
        //    if (posFieldOld != posFieldReal)
        //    {
        //        Debug.Log("NextPosition " + NameObject);
        //        Storage.UpdateGamePosition(posFieldOld, posFieldReal, NameObject);
        //    }
        //}

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

    //[XmlType("Person")]
    //public class PersonData : ObjectData
    //{
    //    public string Id { get; set; }

    //    public PersonData() : base()
    //    {
    //        Id = Guid.NewGuid().ToString();
    //    }
    //}

    //#################################################################################################
    //>>> ObjectData -> GameDataNPC -> PersonData -> 
    //>>> ObjectData -> GameDataNPC -> PersonData -> PersonDataBoss -> GameDataBoss
    //>>> ObjectData -> GameDataUfo
    //>>> ObjectData -> GameDataNPC -> GameDataOther
    //#################################################################################################

    [XmlType("NPC")]
    //[XmlInclude(typeof(GameDataUfo))] 
    public class GameDataNPC : ObjectData
    {
        [XmlIgnore]
        public Vector3 TargetPosition;
        [XmlIgnore]
        public Vector3 NewPosition;

        //-----------------------------------for Ufo
        private Vector3 m_Position = new Vector3(0, 0, 0);
        public override Vector3 Position
        {
            get { return m_Position; }
            set
            {
                Debug.Log("GameDataNPC --- SET Position (" + m_Position + ") = " + value);
                //m_Position = value;
                m_Position = Storage.ConvVector3(value);

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
        //-----------------------------------

        public GameDataNPC() : base()
        {
            NewPosition = new Vector3(0, 0, 0);
        }

        public void SetTargetPosition()
        {
            var _position = Position;
            int distX = UnityEngine.Random.Range(-15, 15);
            int distY = UnityEngine.Random.Range(-15, 15);

            float xT = _position.x + distX;
            float yT = _position.y + distY;

            //validate
            if (yT > -1)
                yT = _position.y - distY;
            if (xT < 1)
                xT = _position.x - distX;

            //----------------------------- valid Limit look hero
            Storage.ZonaRealLook zona = Storage.ZonaReal;
            if (zona != null)
                Storage.ValidPiontInZona(ref xT, ref yT, distX);

            TargetPosition = new Vector3(xT, yT, -1);
        }
        
        const string FieldKey = "Field";
        private string GetNameFieldPosit_2(System.Single p_x, System.Single p_y)
        {
            int x = (int)p_x;
            int y = (int)p_y;
            //x = (int)(x / 2);
            //y = (int)(y / 2);
            x = (int)(x / 2);
            y = (int)(y / 2);
            return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
        }

        public virtual void NextPosition(Vector3 p_newPosition)
        {
            //string posFieldOld = Storage.GetNameFieldPosit(Position.x, Position.y);
            //string posFieldReal = Storage.GetNameFieldPosit(p_newPosition.x, p_newPosition.y);
            string posFieldOld = GetNameFieldPosit_2(Position.x, Position.y);
            string posFieldReal = GetNameFieldPosit_2(p_newPosition.x, p_newPosition.y);

            ////!!!!!!!!!!!! p_nameField === posFieldOld
            //if (posFieldOld != posFieldReal && NewPosition != p_newPosition)
            if (posFieldOld != posFieldReal)
            {
                string posFieldOld_2 = GetNameFieldPosit_2(Position.x, Position.y);
                string posFieldReal_2 = GetNameFieldPosit_2(p_newPosition.x, p_newPosition.y);

                //-------------------
                int p_x = (int)p_newPosition.x;
                int p_y = (int)p_newPosition.y;
                int x = (int)(p_x / 2);
                int y = (int)(p_y / 2);
                string fieldR = FieldKey + (int)x + "x" + Mathf.Abs((int)y);
                string strX = "  x: " + p_newPosition.x + " -> " + p_x + " -> " + (p_x / 2) + " -> " + x;
                string strY = "  y: " + p_newPosition.y + " -> " + p_y + " -> " + (p_y / 2) + " -> " + y;
                fieldR += strX + strY;
                //------------------

                //123456789
                //NewPosition = p_newPosition;
                Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~ NextPosition " + NameObject + "          " + posFieldOld + " > " + posFieldReal + "     " + Position + "  >>  " + p_newPosition + "          " + posFieldOld_2 + " > " + posFieldReal_2);
                Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~ NextPosition " + fieldR);
                Storage.UpdateGamePosition(posFieldOld, posFieldReal, NameObject, p_newPosition);
                //123456789
                //NewPosition = p_newPosition;
            }
        }
    }

    [XmlType("Person")]
    public class PersonData : GameDataNPC
    {
        public string Id { get; set; }

        public PersonData()
            : base()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    //#P
    //[XmlType("Ufo")]
    //public class PersonDataUfo : PersonData
    //{
    //    public PersonDataUfo() : base()
    //    {
    //    }
    //}

    //#P public class GameDataUfo :  PersonDataUfo

    //-----------------
    [XmlType("Ufo")] //?
    public class GameDataUfo : GameDataNPC //?
    {
        [XmlIgnore]
        public Color ColorRender = Color.black;
        //[XmlIgnore]
        //public Vector3 TargetPosition;


        //................................................................
        //private Vector3 m_Position = new Vector3(0, 0, 0);
        //public override Vector3 Position
        //{
        //    get { return m_Position; }
        //    set
        //    {
        //        m_Position = value;
        //        if (IsCanSetTargetPosition)
        //            SetTargetPosition();
        //    }
        //}

        //private bool IsCanSetTargetPosition
        //{
        //    get
        //    {
        //        return (TargetPosition == null || TargetPosition == new Vector3(0, 0, 0)) && m_Position != null && m_Position != new Vector3(0, 0, 0);
        //    }
        //}
        //................................................................

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

         public override void UpdateGameObject(GameObject objGame)
        {
            string nameObjData = ((GameDataUfo)this).NameObject;
            if (nameObjData != objGame.name)
            {
                Debug.Log("Error UpdateGameObject DATA:" + nameObjData + "  OBJECT: " + objGame.name);
                Debug.Log("-------------------------------------------------------------------/" + nameObjData + "/ " + objGame.name + "/");
                return;
            }

            objGame.GetComponent<SpriteRenderer>().color = ColorRender;
            Debug.Log("UPDATE CLONE THIS " + ((GameDataUfo)this).NameObject + "     TO  " + objGame.name);
            objGame.GetComponent<PersonalData>().PersonalObjectData = (GameDataUfo)this.Clone();
        }

        public override string ToString()
        {
            return NameObject + " " + TagObject + " " + Position + " " + ColorRender;
            //return base.ToString();
        }
    }

    //-------------
    //[XmlType("Ufo")] //?
    //public class GameDataUfo : ObjectData //?
    //{
    //    [XmlIgnore]
    //    public Color ColorRender = Color.black;
    //    [XmlIgnore]
    //    public Vector3 TargetPosition;

    //    private Vector3 m_Position = new Vector3(0, 0, 0);
    //    public override Vector3 Position
    //    {
    //        get { return m_Position; }
    //        set
    //        {
    //            m_Position = value;
    //            if (IsCanSetTargetPosition)
    //                SetTargetPosition();
    //        }
    //    }

    //    private bool IsCanSetTargetPosition
    //    {
    //        get
    //        {
    //            return (TargetPosition == null || TargetPosition == new Vector3(0, 0, 0)) && m_Position != null && m_Position != new Vector3(0, 0, 0);
    //        }
    //    }

    //    public GameDataUfo()
    //        : base()
    //    {
    //        ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);

    //        //if (IsCanSetTargetPosition)
    //        //{
    //        //    //Debug.Log("UFO set TargetPosition after init");
    //        //    SetTargetPosition();
    //        //}
    //    }

    //    //public void SetTargetPosition(int limX, int limY, int xHero, int yHero)
    //    //public void SetTargetPosition(Storage.ZonaFieldLook zona)
    //    public void SetTargetPosition()
    //    {
    //        int distX = UnityEngine.Random.Range(-15, 15);
    //        int distY = UnityEngine.Random.Range(-15, 15);
    //        //int ufoX = (int)((m_Position.x / 2) + 0.5);
    //        //int ufoY = (int)((m_Position.y / 2) - 0.5);
    //        //ufoY = (int)(Mathf.Abs(ufoY));

    //        float xT = m_Position.x + distX;
    //        float yT = m_Position.y + distY;

    //        //validate
    //        if (yT > -1)
    //            yT = m_Position.y - distY;
    //        if (xT < 1)
    //            xT = m_Position.x - distX;

    //        //----------------------------- valid Limit look hero
    //        Storage.ZonaRealLook zona = Storage.ZonaReal;
    //        if (zona != null)
    //        {
    //            //Debug.Log("........Validate zone limit for target position");
    //            Storage.ValidPiontInZona(ref xT, ref yT, distX);
    //        }
    //        //else
    //            //Debug.Log("........Validate zone limit for target position: ZonaReal is NULL !!!");
    //        //-----------------------------

    //        TargetPosition = new Vector3(xT, yT, -1);
    //        //Debug.Log("UFO SetTargetPosition ==== " + TargetPosition);
    //    }

    //    public override void UpdateGameObject(GameObject objGame)
    //    {
    //        objGame.GetComponent<SpriteRenderer>().color = ColorRender;
    //        //Debug.Log("UPDATE CLONE THIS " + ((ObjectDataUfo)this).ToString());
    //        objGame.GetComponent<PersonalData>().PersonalObjectData = (GameDataUfo)this.Clone();
    //    }

    //    public override string ToString()
    //    {
    //        return NameObject + " " + TagObject + " " + Position + " " + ColorRender;
    //        //return base.ToString();
    //    }
    //}
    //-------------

    //..............
    [XmlType("Bos")]
    public class PersonDataBoss : PersonData
    {
        public int Level { get; set; }
        public int Life { get; set; }

        public PersonDataBoss()
            : base()
        {
            if(Level==0)
                Level = UnityEngine.Random.Range(1, 5);

            if (Life == 0)
                Life = 100;
        }
    }

    //public class ObjectDataUfo : ObjectData
    public class GameDataBoss : PersonDataBoss
    {
        [XmlIgnore]
        public Color ColorRender = Color.black;

        public GameDataBoss()
            : base()
        {
            Dictionary<int, Color> colorsPresent = new Dictionary<int, Color>();
            colorsPresent.Add(0, Color.black);
            colorsPresent.Add(1, Color.grey);
            colorsPresent.Add(2, Color.yellow);
            colorsPresent.Add(3, Color.green);
            colorsPresent.Add(4, Color.blue);
            colorsPresent.Add(5, Color.red);
            ColorRender = colorsPresent[Level];
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            Debug.Log("______________________UpdateGameObject BOSS.1__________________");

            objGame.GetComponent<SpriteRenderer>().color = ColorRender;
            objGame.GetComponent<PersonalData>().PersonalObjectData = (GameDataUfo)this.Clone();
        }
    }
    //-----------------------------------

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
                    //Debug.Log("_______________________GameDataUfo_____________: " + p_gobject.name);
                    newObject = new GameDataUfo()
                    {
                        NameObject = p_gobject.name,
                        TagObject = p_gobject.tag,
                        Position = p_gobject.transform.position
                    };

                    //Debug.Log("_______________________UpdateGameObject DATA_____________: " + p_gobject.name);
                    //NEW DATA  -------->>>>  PERSONA  #P#
                    Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name);
                    newObject.UpdateGameObject(p_gobject);

                    //Debug.Log("_______________________UpdateGameObject DATA_____________ updated.    :" + p_gobject.name);
                    //Debug.Log("DATA NewGen PrefabUfo (" + p_gobject.name + ") SAVE : " + testC + " to:" + newObject.ToString());
                }
                else
                {
                    //Debug.Log("_______________________GameDataUfo_____________Update... " + p_gobject.name);

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
                //Debug.Log("_______________________CreateObjectData_____________default " + prefabType);
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
                    resPrefab = Instantiate(PrefabRock);
                    break;
                case TypePrefabs.PrefabVood:
                    resPrefab = Instantiate(PrefabVood);
                    break;
                case TypePrefabs.PrefabUfo:
                    resPrefab = Instantiate(PrefabUfo);
                    break;
                case TypePrefabs.PrefabBoss:
                    resPrefab = Instantiate(PrefabBoss);
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

  
    

    //public static string CreateName(string tag, string nameFiled, int i=0)
    //{
    //    return tag + "_" + nameFiled + "_" + i;
    //}

    public static string CreateName(string tag, string nameFiled, string id = "", string nameObjOld = "")
    {
        if (string.IsNullOrEmpty(id))
        {
            if (string.IsNullOrEmpty(nameObjOld))
            {
                Debug.Log("!!!!!! Error create name !!!!!!!!!!");
            }
            else
            {
                //int i = nameObjOld.IndexOf("_");
                int i = nameObjOld.LastIndexOf("_");
                if (i != -1)
                {
                    //123456789
                    //Debug.Log("_______________________CREATE NAME i=" + i + " len=" + nameObjOld.Length + "      :" + nameObjOld);
                    id = nameObjOld.Substring(i + 1, nameObjOld.Length - i -1);
                    //Debug.Log("_______________________CREATE NAME :" + id);
                }
                else
                    Debug.Log("!!!!!! Error create name prefix !!!!!!!!!!");
            }
        }
        if (id == "-1")
        {
            id = Guid.NewGuid().ToString().Substring(1, 4);
        }

        return tag + "_" + nameFiled + "_" + id;
    }

    //public static class DictionaryExtensions
    //{
    //    public static IEnumerable<KeyValuePair<int, string>> GetComponents(this IEnumerable<KeyValuePair<int, string>> thisObj)
    //    {

    //        yield return new KeyValuePair<int, string>(1111, "aaa");
    //    }
    //}

    

}


