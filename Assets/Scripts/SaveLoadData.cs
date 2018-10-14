using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Linq;

public class SaveLoadData : MonoBehaviour {

    public GameObject PrefabVood;
    public GameObject PrefabRock;
    public GameObject PrefabUfo;
    public GameObject PrefabBoss;
    private GenerateGridFields _scriptGrid;
    public static float Spacing = 2f;
  

    //private List<string> _namesPrefabs = new List<string>
    //{
    //    "PrefabField","PrefabRock","PrefabVood","PrefabUfo" //,"","","",""
    //};

    //private IEnumerable<string> _namesPrefabs
    //{   get
    //    {
    //        return new List<string>{
    //            TypePrefabs.PrefabField.ToString(),
    //            TypePrefabs.PrefabRock.ToString(),
    //            TypePrefabs.PrefabVood.ToString(),
    //            TypePrefabs.PrefabUfo.ToString()
    //        };
    //    }
    //}

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
            Debug.Log("# CreateDataGamesObjectsWorld... Game is loaded");
            return;
        }

        int coutCreateObjects = 0;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        for (int y = 0; y < Storage.WidthLevel; y++)
        {
            for (int x = 0; x < Storage.HeightLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 3);

                int maxObjectInField = (intRndCount == 0) ? 1 : 0;
                string nameField = Storage.GetNameField(x, y);

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

                    string nameObject = Storage.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ObjectData objDataSave = BildObjectData(prefabName);
                    objDataSave.NameObject = nameObject;
                    objDataSave.TagObject = prefabName.ToString();
                    objDataSave.Position = pos;

                    coutCreateObjects++;

                    Storage.Instance.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
        }

        Storage.Instance.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);
    }

    //private void CreateNewCorrectObject(string idObj, TypePrefabs prefabName, string nameField)
    //{
    //    string nameObject = Storage.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
    //    ObjectData objDataSave = BildObjectData(prefabName);
    //    objDataSave.NameObject = nameObject;
    //    objDataSave.TagObject = prefabName.ToString();
    //    Vector3 pos = new Vector3(0, 0, 0);
    //    objDataSave.Position = pos;
    //    Storage.Instance.AddDataObjectInGrid(objDataSave, nameField, "CreateNewCorrectObject");
    //}

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
                newObject = new GameDataUfo()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position
                };
                Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "   newObject=" + newObject + "             ~~~~~ DO: pos=" + newObject.Position + "  GO:  pos=" + p_gobject.transform.position);
                newObject.UpdateGameObject(p_gobject);
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

    public static ObjectData FindObjectData(GameObject p_gobject)
    {
        ObjectData newObject;
        //#PPPP
        TypePrefabs prefabType = TypePrefabs.PrefabField;

        if (!String.IsNullOrEmpty(p_gobject.tag))
            prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), p_gobject.tag.ToString()); ;

        switch (prefabType)
        {
            case TypePrefabs.PrefabUfo:
                newObject = new GameDataUfo();
                string nameField = Storage.GetNameFieldByName(p_gobject.name);
                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                {
                    Debug.Log("################# Error FindObjectData FIELD NOT FOUND :" + nameField);
                    return null;
                }
                var objects = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
                var index = objects.FindIndex(p => p.NameObject == p_gobject.name);
                if (index == -1)
                {
                    //Debug.Log("################# Error FindObjectData DATA OBJECT NOT FOUND : " + p_gobject.name + "   in Field: " + nameField);
                    //Storage.Instance.DebugKill(p_gobject.name);
                    Storage.Instance.GetHistory(p_gobject.name);
                    Storage.Instance.CorrectData(null, p_gobject, "FindObjectData");
                    return null;
                }
                newObject = objects[index] as GameDataUfo;
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

        static Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(GameDataUfo) };

        static public void SaveGridXml(GridData state, string datapath, bool isNewWorld = false)
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
    public class GameDataNPC : ObjectData
    {
        [XmlIgnore]
        public Vector3 TargetPosition;
        

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
            Storage.ZonaRealLook zona = Storage.Instance.ZonaReal;
            if (zona != null)
                Storage.Instance.ValidPiontInZona(ref xT, ref yT, distX);

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
            if(_isError)
            {
                Debug.Log("################ Error NextPosition (" + gobj.name + ")   already IS ERROR ");
                return "Error";
            }
            if (Storage.Instance.IsCorrectData)
            {
                Debug.Log("_______________ RETURN CorrectData ON CORRECT_______________");
                return "Error";
            }

            Vector3 _newPosition = gobj.transform.position;
            Vector3 _oldPosition = Position;
            string nameObject = gobj.name;
            string posFieldName = Storage.GetNameFieldByName(nameObject);

            string posFieldOld = Storage.GetNameFieldPosit(_oldPosition.x, _oldPosition.y);
            string posFieldReal = Storage.GetNameFieldPosit(_newPosition.x, _newPosition.y);
            string newName = "";

            if (posFieldOld != posFieldReal)
            {
                newName = "?";

                if (posFieldName != posFieldOld)
                {
                    //Create dublicate
                    Debug.Log("################ Error NextPosition (" + gobj.name + ")   ERROR NAMES:  Old Field name: " + posFieldName + " !=  posFieldOld: " + posFieldOld + "  ------  posFieldReal: " + posFieldReal + "   DN:" + NameObject );
                    Storage.Instance.GetHistory(gobj.name);
                    //gobj.PlayAnimation();
                    //Destroy(gobj, 3f);

                    //Storage.Instance.AddDestroyRealObject(gobj);
                    //@CD@
                    _isError = true;
                    Storage.Instance.CorrectData(null, gobj, "NextPosition");
                    return "Error";

                    
                }

                bool isInZona = true;

                if (!Storage.Instance.IsValidPiontInZona(_newPosition.x, _newPosition.y))
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
    }

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
            
        }
    }
    //-----------------------------------

    //private ObjectData BildObjectData(TypePrefabs prefabType)
    //{
    //    ObjectData objGameBild;

    //    switch (prefabType)
    //    {
    //        case SaveLoadData.TypePrefabs.PrefabUfo:
    //            objGameBild = new GameDataUfo();
    //            break;
    //        default:
    //            objGameBild = new ObjectData();
    //            break;
    //    }
    //    return objGameBild;
    //}

    public static ObjectData BildObjectData(TypePrefabs prefabType)
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
        if (Storage.Instance.GridDataG == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }

        Serializator.SaveGridXml(Storage.Instance.GridDataG, Storage.Instance.DataPathLevel);
    }

    
    

}


