using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Linq;

public class SaveLoadData : MonoBehaviour {

    //@NEWPREFAB@
    public GameObject PrefabVood;
    public GameObject PrefabRock;
    public GameObject PrefabUfo;
    public GameObject PrefabBoss;

    //public GameObject 
    public static float Spacing = 2f;

    private GenerateGridFields _scriptGrid;
   

    //#################################################################################################
    //>>> ObjectData -> GameDataNPC -> PersonData -> 
    //>>> ObjectData -> GameDataNPC -> PersonData -> PersonDataBoss -> GameDataBoss
    //>>> ObjectData -> GameDataUfo
    //>>> ObjectData -> GameDataNPC -> GameDataOther
    //#################################################################################################

    static Type[] extraTypes = {
            typeof(FieldData),
            typeof(ObjectData),

            typeof(GameDataUfo),

            typeof(GameDataNPC),
            typeof(PersonData),

            typeof(PersonDataBoss),
            typeof(GameDataBoss) };  

    private IEnumerable<string> _namesPrefabs
    {   get
        {
            var list = new List<string>();
            foreach (var nextType in Enum.GetValues(typeof(TypePrefabs)))
            {
                list.Add(nextType.ToString());
            }
            return list;
        }
    }

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

        
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void InitData()
    {
        _scriptGrid = GetComponent<GenerateGridFields>();
    }

    //#.D 
    public void CreateDataGamesObjectsWorld(bool isAlwaysCreate = false)
    {
        //# On/Off
        //isAlwaysCreate = true;

        if (Storage.Instance.GridDataG != null && !isAlwaysCreate)
        {
            Debug.Log("# CreateDataGamesObjectsWorld... Game is loaded              Storage.Instance.GridDataG:    " + Storage.Instance.GridDataG);
            return;
        }

        int coutCreateObjects = 0;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        for (int y = 0; y < Helper.WidthLevel; y++)
        {
            for (int x = 0; x < Helper.HeightLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 3);

                int maxObjectInField = (intRndCount == 0) ? 1 : 0;
                string nameField = Helper.GetNameField(x, y);

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {

                    //Type prefab
                    
                    //#TT YES BOSS
                    int intTypePrefab = UnityEngine.Random.Range(1, 5);
                    //#TT YES UFO
                    //int intTypePrefab = UnityEngine.Random.Range(1, 4);
                    //#TT NOT UFO
                    //int intTypePrefab = UnityEngine.Random.Range(1, 3);

                    TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ObjectData objDataSave = BildObjectData(prefabName);
                    objDataSave.NameObject = nameObject;
                    objDataSave.TagObject = prefabName.ToString();
                    objDataSave.Position = pos;

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
        }

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);
    }

   
    public static ObjectData CreateObjectData(GameObject p_gobject)
    {
        ObjectData newObject;
        //#PPPP
        TypePrefabs prefabType = TypePrefabs.PrefabField;

        if (!String.IsNullOrEmpty(p_gobject.tag))
            prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), p_gobject.tag.ToString()); ;

        switch (prefabType)
        {
            case TypePrefabs.PrefabUfo:
                //var newObject1 = new GameDataUfo()
                newObject = new GameDataUfo()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position
                };
                //Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "   newObject=" + newObject + "             ~~~~~ DO: pos=" + newObject.Position + "  GO:  pos=" + p_gobject.transform.position);
                //newObject1.UpdateGameObject(p_gobject);
                newObject.UpdateGameObject(p_gobject);
                break;
            case TypePrefabs.PrefabBoss:
                //var newObject2 = new GameDataBoss()
                newObject = new GameDataBoss()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position
                };
                Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "   newObject=" + newObject + "             ~~~~~ DO: pos=" + newObject.Position + "  GO:  pos=" + p_gobject.transform.position);
                Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "  TYPE: " + newObject.GetType());
                //newObject2.UpdateGameObject(p_gobject);
                newObject.UpdateGameObject(p_gobject);
                break;
            default:
                //Debug.Log("_______________________CreateObjectData_____________default " + prefabType);
                //var newObject3 = new ObjectData()
                newObject = new ObjectData()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position
                };
                //newObject3.UpdateGameObject(p_gobject);
                newObject.UpdateGameObject(p_gobject);
                break;
        }
        return newObject;
    }

    public static ObjectData FindObjectData(GameObject p_gobject)
    {
        ObjectData newObject;
        //#PPPP
        TypePrefabs prefabType = TypePrefabs.PrefabField;

        if (!String.IsNullOrEmpty(p_gobject.tag))
            prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), p_gobject.tag.ToString()); ;

        string nameField = "";
        int index = -1;
        List<ObjectData> objects;

        //Debug.Log("FindObjectData -------- prefabType: " + prefabType);

        switch (prefabType)
        {
            case TypePrefabs.PrefabUfo:
                newObject = new GameDataUfo();
                nameField = Helper.GetNameFieldByName(p_gobject.name);
                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                {
                    Debug.Log("################# Error FindObjectData FIELD NOT FOUND :" + nameField);
                    return null;
                }
                objects = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
                index = objects.FindIndex(p => p.NameObject == p_gobject.name);
                if (index == -1)
                {
                    Debug.Log("################# Error FindObjectData DATA OBJECT NOT FOUND : " + p_gobject.name + "   in Field: " + nameField);
                    //Storage.Instance.DebugKill(p_gobject.name);
                    //Storage.Log.GetHistory(p_gobject.name);
                    //Storage.Fix.CorrectData(null, p_gobject, "FindObjectData");
                    return null;
                }
                newObject = objects[index] as GameDataUfo;
                break;
            case TypePrefabs.PrefabBoss:
                newObject = new GameDataBoss();
                nameField = Helper.GetNameFieldByName(p_gobject.name);
                //Debug.Log("FindObjectData ------------ nameField :" + nameField);

                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                {
                    Debug.Log("################# Error FindObjectData FIELD NOT FOUND :" + nameField);
                    return null;
                }
                objects = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
                //Debug.Log("FindObjectData ------------ objects in " + nameField + ":" + objects.Count());
                index = objects.FindIndex(p => p.NameObject == p_gobject.name);
                if (index == -1)
                {
                    Debug.Log("################# Error FindObjectData DATA OBJECT NOT FOUND : " + p_gobject.name + "   in Field: " + nameField);
                    //Storage.Log.GetHistory(p_gobject.name);
                    //Storage.Fix.CorrectData(null, p_gobject, "FindObjectData");
                    return null;
                }
                //Debug.Log("FindObjectData ------------ objects[" + index + "] " + objects[index] + " type: " + objects[index].GetType());

                newObject = objects[index] as GameDataBoss;
                //var t2 = (GameDataBoss)objects[index];
                //if(t2==null)
                //    Debug.Log("t2=null ");
                //else Debug.Log("t2 type=" + t2.GetType());

                if (newObject==null)
                {
                    Debug.Log("FindObjectData ------------newObject is null ");
                    Debug.Log("FindObjectData ------------newObject is null , Type:" + objects[index].GetType());
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

        //Debug.Log("FindObjectData -------- newObject: " + newObject);
        //Debug.Log("FindObjectData -------- newObject: " + newObject.NameObject);

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

    

    //---------------------------------------------------------------------------------------------------------------------

    public class Serializator {

        
        static public void SaveGridXml(GridData state, string datapath, bool isNewWorld = false)
        {

            if (isNewWorld)
            {
                if (File.Exists(datapath))
                {
                    try
                    {
                        File.Delete(datapath);
                    }catch(Exception x)
                    {
                        Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapath + " : " + x.Message);
                    }
                }
            }

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
                XmlSerializer serializer = new XmlSerializer(typeof(LevelData), extraTypes);
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

        //static public CommandStore LoadCommandsStore(string datapath)
        //{
        //    string stepErr = "start";
        //    CommandStore state = null;
        //    try
        //    {
        //        Debug.Log("Loaded Xml GridData start...");

        //        stepErr = ".1";
        //        stepErr = ".2";
        //        XmlSerializer serializer = new XmlSerializer(typeof(CommandStore), extraTypes);
        //        stepErr = ".3";
        //        FileStream fs = new FileStream(datapath, FileMode.Open);
        //        stepErr = ".4";
        //        state = (CommandStore)serializer.Deserialize(fs);
        //        stepErr = ".5";
        //        fs.Close();
        //        stepErr = ".6";
        //        stepErr = ".7";
        //    }
        //    catch (Exception x)
        //    {
        //        state = null;
        //        Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        //    }

        //    return state;
        //}

        static public T LoadXml<T>(string datapath) where T : class
        {
            string stepErr = "start";
            T state = null;
            try
            {
                //Debug.Log("Loaded Xml GridData start...");

                stepErr = ".1";
                stepErr = ".2";
                XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);
                stepErr = ".3";
                FileStream fs = new FileStream(datapath, FileMode.Open);
                stepErr = ".4";
                state = (T)serializer.Deserialize(fs);
                stepErr = ".5";
                fs.Close();
                stepErr = ".6";
                stepErr = ".7";
            }
            catch (Exception x)
            {
                state = null;
                Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
            }

            return state;
        }

        static public void SaveXml<T>(T state, string datapath, bool isResave = false) where T : class
        {

            if (isResave)
            {
                if (File.Exists(datapath))
                {
                    try
                    {
                        File.Delete(datapath);
                    }
                    catch (Exception x)
                    {
                        Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapath + " : " + x.Message);
                    }
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);

            FileStream fs = new FileStream(datapath, FileMode.Create);

            serializer.Serialize(fs, state);
            fs.Close();

        }
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
    

    //++++
    [XmlType("Object")] //++
    [XmlInclude(typeof(PersonData))]
    //[XmlInclude(typeof(GameDataNPC))] //$$
    
    public class ObjectData : ICloneable
    {		
        public string NameObject { get; set; }

        public string TagObject { get; set; }

        //public Vector3 Position { get; set; }
        public virtual Vector3 Position { get; set; }

        [XmlIgnore]
        public bool IsReality = false;

        public ObjectData() {
        }

        public virtual void UpdateGameObject(GameObject objGame)
        {
            
        }

        

        public object Clone()
        {
            return this.MemberwiseClone(); 
        }

        public override string ToString()
        {
            return NameObject + " " + TagObject + " " + Position;
        }
    }

   

    //#################################################################################################
    //>>> ObjectData -> GameDataNPC -> PersonData -> 
    //>>> ObjectData -> GameDataNPC -> PersonData -> PersonDataBoss -> GameDataBoss
    //>>> ObjectData -> GameDataUfo
    //>>> ObjectData -> GameDataNPC -> GameDataOther
    //#################################################################################################

    [XmlType("NPC")]
    //[XmlInclude(typeof(GameDataUfo))] 
    //[XmlInclude(typeof(GameDataUfo))] //$$
    public class GameDataNPC : ObjectData
    {
        [XmlIgnore]
        public virtual Vector3 TargetPosition { get; set; }

        [XmlIgnore]
        public int Speed { get; set; }

        //-----------------------------------for Ufo
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
        //-----------------------------------

        public GameDataNPC() : base()
        {
            Speed = 1;
        }

        public void SetTargetPosition(Vector3 p_SetTarget)
        {
            //#TARGET
            Debug.Log("^^^^^  Set Target + " + p_SetTarget + "    Name  " + NameObject);
            TargetPosition = p_SetTarget;
            return;
        }

        public void SetTargetPosition()
        {
            var _position = Position;
            int distX = UnityEngine.Random.Range(-15, 15);
            int distY = UnityEngine.Random.Range(-15, 15);

            float xT = _position.x + distX;
            float yT = _position.y + distY;

            //validate
            //if (yT > -1)
            //    yT = _position.y - distY;
            //if (xT < 1)
            //    xT = _position.x - distX;

            //----------------------------- valid Limit look hero
            //Storage.ZonaRealLook zona = Storage.Instance.ZonaReal;
            //if (zona != null)
            //ValidPiontInZona(ref xT, ref yT, distX);z
            ValidPiontInZonaWorld(ref xT, ref yT, distX);

            TargetPosition = new Vector3(xT, yT, -1);
        }

        //public virtual string TestNextPosition(GameObject gobj, string lastNewName) //, Vector3 p_newPosition)
        //{
        //    Debug.Log("***** lastNewName: " + lastNewName);
        //    var res = NextPosition(gobj);
        //    return res;
        //}

        [XmlIgnore]
        private bool _isError = false;

        public virtual string NextPosition(GameObject gobj) //, Vector3 p_newPosition)
        {
            if (Storage.Instance.IsLoadingWorld)
            {
                //Debug.Log("_______________ LOADING WORLD ....._______________");
                return "";
            }

            if (_isError)
            {
                Debug.Log("################ Error NextPosition (" + gobj.name + ")   already IS ERROR ");
                return "Error";
            }
            if (Storage.Instance.IsCorrectData)
            {
                Debug.Log("_______________ RETURN CorrectData ON CORRECT_______________");
                return "Error";
            }
            if(!IsReality)
            {
                Debug.Log("_______________  Error NextPosition (" + gobj.name + ")   Not IsReality _______________");
                return "Update";
            }

            Vector3 _newPosition = gobj.transform.position;
            Vector3 _oldPosition = Position;
            string nameObject = gobj.name;
            string posFieldName = Helper.GetNameFieldByName(nameObject);

            string posFieldOld = Helper.GetNameFieldPosit(_oldPosition.x, _oldPosition.y);
            string posFieldReal = Helper.GetNameFieldPosit(_newPosition.x, _newPosition.y);
            string newName = "";

            if (posFieldOld != posFieldReal)
            {
                newName = "?";

                if (posFieldName != posFieldOld)
                {
                    //Create dublicate
                    Debug.Log("################ Error NextPosition (" + gobj.name + ")   ERROR NAMES:  Old Field name: " + posFieldName + " !=  posFieldOld: " + posFieldOld + "  ------  posFieldReal: " + posFieldReal + "   DN:" + NameObject  + " DataPos: " + Position.x + "x" + Position.y);
                    Storage.Log.GetHistory(gobj.name);
                    //gobj.PlayAnimation();
                    //Destroy(gobj, 3f);

                    //Storage.Instance.AddDestroyRealObject(gobj);
                    //@CD@
                    _isError = true;
                    Storage.Fix.CorrectData(null, gobj, "NextPosition");
                    return "Error";

                    
                }

                bool isInZona = true;

                if (!Helper.IsValidPiontInZona(_newPosition.x, _newPosition.y))
                {
                    
                    isInZona = false;
                }



                //newName = Storage.Instance.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, !isInZona);
                //@CD@ 
                newName = Storage.Instance.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona);
                //

                if (!isInZona && !string.IsNullOrEmpty(newName))
                {
                    
                    Destroy(gobj);
                }

            }
            return newName;
        }

        public virtual string Upadete(GameObject gobj) 
        {
            Vector3 _newPosition = gobj.transform.position;
            Vector3 _oldPosition = Position;
            string nameObject = gobj.name;
            string posFieldName = Helper.GetNameFieldByName(nameObject);

            string posFieldOld = Helper.GetNameFieldPosit(_oldPosition.x, _oldPosition.y);
            string posFieldReal = Helper.GetNameFieldPosit(_newPosition.x, _newPosition.y);
            string newName = "";

            bool isInZona = true;
            if (!Helper.IsValidPiontInZona(_newPosition.x, _newPosition.y))
            {
                isInZona = false;
            }
            
            newName = Storage.Instance.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona, true);
            if (!isInZona && !string.IsNullOrEmpty(newName))
            {
                Destroy(gobj);
            }
            return newName;
        }

        public virtual List<string> GetParams
        {
            get
            {
                return new List<string> {
                    "Name: " + NameObject,
                    "Type : " + TagObject,
                    "Pos : " + Position,
                    "Target : " + TargetPosition,
                    //"Life: " + Life,
                    //"Speed: " + Speed,
                    //"Color : " + ColorLevel
                  };
            }
        }

        public string GetParamsString
        {
            get
            {
                return string.Join("\n", GetParams.ToArray());
            }
        }

    }

    [XmlType("Person")]
    public class PersonData : GameDataNPC
    {
        public override Vector3 TargetPosition { get; set; }

        public string Id { get; set; }

        public PersonData()
            : base()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    

    //-----------------
    [XmlType("Ufo")] //?
    public class GameDataUfo : GameDataNPC //?
    {
        [XmlIgnore]
        public Color ColorRender = Color.black;

        public GameDataUfo()
            : base()
        {
            ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);

            Speed = 3;
        }

         public override void UpdateGameObject(GameObject objGame)
        {
            string nameObjData = ((GameDataUfo)this).NameObject;
            if (nameObjData != objGame.name)
            {
                
                objGame.name = nameObjData;
            }

            

            objGame.GetComponent<SpriteRenderer>().color = ColorRender;
        }

        public override string ToString()
        {
            
            return NameObject + " " + TagObject + " " + Position.x + " " + Position.y;
            
        }

        public override List<string> GetParams
        {
            get
            {
                return new List<string> {
                    "Name: " + NameObject,
                    "Type : " + TagObject,
                    "Pos : " + Position,
                    "Target : " + TargetPosition,
                  };
            }
        }
    }

    //..............
    [XmlType("PersonBoss")]
    public class PersonDataBoss : PersonData
    {
        public int Level { get; set; }
        public int Life { get; set; }

        public PersonDataBoss()
            : base()
        {
            if(Level==0)
                Level = UnityEngine.Random.Range(1, 7);

            if (Life == 0)
                Life = Level * 10;

            Speed = Level;
        }
    }


    [XmlType("Boss")]
    public class GameDataBoss : PersonDataBoss
    //public class GameDataBoss : GameDataNPC
    {
        [XmlIgnore]
        Dictionary<int, Color> _colorsPresent = null;

        [XmlIgnore]
        private Color m_ColorRender = Color.clear;
        [XmlIgnore]
        public Color ColorRender
        {
            get
            {
                return m_ColorRender;
            }
            set
            {
                m_ColorRender = value;
                if (m_ColorRender != null && m_ColorRender != Color.clear)
                {
                    string colorStr = "#" + ColorUtility.ToHtmlStringRGB(m_ColorRender); //!!!!!!!!!!!!!!!!!!!!
                    if (ColorLevel != colorStr)
                    {
                        //Debug.Log("!!!!!!!!!!!!!!!!!!!! if (ColorLevel(" + ColorLevel + ") != color test(" + colorStr + "))");
                        ColorLevel = colorStr;

                    }
                }
            }
        }

        private string _ColorLevel = "";
        public string ColorLevel
        {
            get
            {
                return _ColorLevel;
            }
            set
            {
                _ColorLevel = value;

                if (!string.IsNullOrEmpty(_ColorLevel) && _ColorLevel != "#000000")
                {
                    Color testColor = _ColorLevel.ToColor(ColorRender);
                    if (ColorRender != testColor)
                    {
                        //Debug.Log("!!!!!!!!!!!!!!!!!!!! SET R (ColorRender(" + ColorRender + ") != color test(" + testColor + "))   _ColorLevel=" + _ColorLevel);
                        ColorRender = testColor;
                    }
                }

            }

        }

        public GameDataBoss()
            : base()
        {

            Speed = 5;

            if (m_ColorRender != Color.clear)
                return;

            InitColor();
        }


        private void InitColor()
        {
            
            ColorRender = StoragePerson.GetColorsLevel[Level]; 
            //ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
            //Debug.Log(">>>>>>>>> colorStr INIT ==" + ColorRender + "    Level:" + Level);
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            bool isUpdateStyle = false; 

            string nameObjData = ((GameDataBoss)this).NameObject;
            if (nameObjData != objGame.name)
            {
                objGame.name = nameObjData;
            }

            if (ColorRender != StoragePerson.GetColorsLevel[Level])
            {
                ColorRender = StoragePerson.GetColorsLevel[Level];
                isUpdateStyle = true;
                //Debug.Log(">>>>>>>>> colorStr ==" + ColorRender + "    Level:" + Level + "    GetColor:  " + GetColorsLevel[Level]);
            }
            

            //if (isUpdateStyle)
            //{
                Sprite spriteMe = Storage.GridData.GetSpriteBoss(Level);
                if (spriteMe != null)
                {
                    //string _nameSprite = Storage.GridData.GetNameSpriteForIndexLevel(Level);
                    //Debug.Log("____________________Update new Sprite " + NameObject + "  Level=" + Level + "   >> " + _nameSprite);
                    
                    objGame.GetComponent<SpriteRenderer>().sprite = spriteMe;
                }
                else
                {
                    Debug.Log("############## NOT Update new Sprite " + NameObject + "  Level=" + Level + " ???????????????");
                    objGame.GetComponent<SpriteRenderer>().color = ColorRender;
                }
            //}
        }

        public override List<string> GetParams
        {
            get {
                return new List<string> {
                    "Name: " + NameObject,
                    "Type : " + TagObject,
                    "Pos : " + Position,
                    "Target : " + TargetPosition,
                    "Life: " + Life,
                    "Speed: " + Speed,
                    "Color : " + ColorLevel
                  };
            }
        }
    }
    //------------------------------------------------------------------------------
    //------------------------------------------------------------------------------

    

   

    public Sprite GetSpriteBossTrue(int index)
    {
        string indErr = "";
        try
        {
            indErr = "start";
            //if (NemesSpritesBoss.Length <= index)
            //{
            //    Debug.Log("############ NemeSpriteBoss int on range index = " + index + "   sprites count= " + NemesSpritesBoss.Length);
            //    return null;
            //}

            indErr = "2";
            //Case 1.
            //string spriteName = NemesSpritesBoss[index];
            string spriteName = 

            indErr = "3";
            if (!Storage.Person.SpriteCollection.ContainsKey(spriteName))
            {
                Debug.Log("############ NOT in SpriteCollection name: " + spriteName);
                return null;
            }
            indErr = "4";
            Sprite spriteBoss = Storage.Person.SpriteCollection[spriteName];
            if (spriteBoss == null)
            {
                Debug.Log("############ spritesBoss is null");
                return null;
            }
            else
            {
                return spriteBoss;
            }

        }catch(Exception x)
        {
            Debug.Log("################# GetSpriteBoss #" + indErr + " [" + index + "] : " + x.Message);
            return GetSpriteBossTrue(index);
        }

        return null;
    }

    public Sprite GetSpriteBoss(int index)
    {
        
        try
        {
            string spriteName = TypeBoss.Instance.GetNameSpriteForIndexLevel(index);
            Sprite spriteBoss = Storage.Person.SpriteCollection[spriteName];
            
            return spriteBoss;
        }
        catch (Exception x)
        {
            Debug.Log("################# GetSpriteBoss [" + index + "] : " + x.Message);
        }

        return null;
    }

    public static ObjectData BildObjectData(TypePrefabs prefabType)
    {
        ObjectData objGameBild;

        switch (prefabType)
        {
            case SaveLoadData.TypePrefabs.PrefabUfo:
                objGameBild = new GameDataUfo();
                break;
            case SaveLoadData.TypePrefabs.PrefabBoss:
                objGameBild = new GameDataBoss(); //$$
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
        if (Storage.Instance.GridDataG == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }

        Serializator.SaveGridXml(Storage.Instance.GridDataG, Storage.Instance.DataPathLevel, true);
    }

    private static Vector2 ValidPiontInZona(ref float x, ref float y, float offset = 0)
    {
        offset = Mathf.Abs(offset);

        if (x < Storage.Instance.ZonaReal.X)
            x = Storage.Instance.ZonaReal.X + offset;
        if (y > Storage.Instance.ZonaReal.Y) //*-1
            y = Storage.Instance.ZonaReal.Y - offset;
        if (x > Storage.Instance.ZonaReal.X2)
            x = Storage.Instance.ZonaReal.X2 - offset;
        if (y < Storage.Instance.ZonaReal.Y2) //*-1
            y = Storage.Instance.ZonaReal.Y + offset;
        Vector2 result = new Vector2(x, y);
        return result;
    }

    private static Vector2 ValidPiontInZonaWorld(ref float x, ref float y, float offset = 0)
    {
        offset = Mathf.Abs(offset);

        if (x < 1)
            x = 1 +  Math.Abs(offset);
        if (y > -1) //*-1
            y = offset - Math.Abs(offset);
        if (x > Helper.WidthLevel * Storage.ScaleWorld)
            x = (Helper.WidthLevel * Storage.ScaleWorld) -  Math.Abs(offset);
        if (y < (Helper.HeightLevel * Storage.ScaleWorld)*(-1)) //*-1
            y = ((Helper.HeightLevel * Storage.ScaleWorld) - Math.Abs(offset)) * (-1);
        Vector2 result = new Vector2(x, y);
        return result;
    }


}


