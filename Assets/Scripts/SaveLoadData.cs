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
    //public Camera MainCamera;
    //@ST@
    //public GameObject DataStorage;
    

    //private string _datapathLevel;
    //private string _datapathPerson;
    //@ST@ private GridData _gridData;
    //@ST@ 
    //private LevelData _personsData;
    
    private GenerateGridFields _scriptGrid;
    //@ST@ private CreateNPC _scriptNPC;
    //@ST@
    //private Storage _scriptStorage;

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
        isAlwaysCreate = true;
        //var _gridData = Storage.Instance.GridDataG;

        //if (_gridData != null && !isAlwaysCreate)
        if (Storage.Instance.GridDataG != null && !isAlwaysCreate)
        {
            Debug.Log("# CreateDataGamesObjectsWorld... Game is loaded");
            //@ST@ _scriptGrid.GridData = _gridData;
            //_scriptNPC.SartCrateNPC();
            //@ST@ Storage.SetGridData(_gridData);
            return;
        }

        //Dictionary<string, List<GameObject>> _gamesObjectsActive = new Dictionary<string, List<GameObject>>();
        int coutCreateObjects = 0;

        Debug.Log("# CreateDataGamesObjectsWorld...");

        //#D- 
        //@ST@ Dictionary<string, FieldData> dictFields = new Dictionary<string, FieldData>();
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

        //Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects + "  count fields: " + _scriptGrid.GamesObjectsActive.Count);
        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);
    }

    //+++ CreatePrefabByName +++
    //public static ObjectData CreateObjectData(GameObject p_gobject, bool isNewGen = false)
    //{
    //    ObjectData newObject;
    //    //#PPPP
    //    TypePrefabs prefabType = TypePrefabs.PrefabField;

    //    if (!String.IsNullOrEmpty(p_gobject.tag))
    //        prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), p_gobject.tag.ToString()); ;

    //    switch (prefabType)
    //    {
    //        case TypePrefabs.PrefabUfo:
    //            if (isNewGen)
    //            {
    //                //Debug.Log("_______________________GameDataUfo_____________: " + p_gobject.name);
    //                newObject = new GameDataUfo()
    //                {
    //                    NameObject = p_gobject.name,
    //                    TagObject = p_gobject.tag,
    //                    Position = p_gobject.transform.position
    //                };

    //                //Debug.Log("_______________________UpdateGameObject DATA_____________: " + p_gobject.name);
    //                //NEW DATA  -------->>>>  PERSONA  #P#
    //                Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "   newObject=" + newObject + "             ~~~~~ DO: pos=" + newObject.Position + "  GO:  pos=" + p_gobject.transform.position);
    //                newObject.UpdateGameObject(p_gobject);

    //                //Debug.Log("_______________________UpdateGameObject DATA_____________ updated.    :" + p_gobject.name);
    //                //Debug.Log("DATA NewGen PrefabUfo (" + p_gobject.name + ") SAVE : " + testC + " to:" + newObject.ToString());
    //            }
    //            else
    //            {
    //                //Debug.Log("_______________________GameDataUfo_____________Update... " + p_gobject.name);

    //                newObject = new GameDataUfo();
    //                //string idObject = Storage.GetGameObjectID(p_gobject);
    //                string nameField = Storage.GetNameFieldByName(p_gobject.name);

    //                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                {
    //                    Debug.Log("!!!!!!!!! Error CreateObjectData FIELD NOT FOUND :" + nameField);
    //                    return null;
    //                }
    //                var objects = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
    //                var index = objects.FindIndex(p => p.NameObject == p_gobject.name);
    //                if (index == -1)
    //                {
    //                    Debug.Log("################# Error CreateObjectData DATA OBJECT NOT FOUND : " + p_gobject.name + "   in Field: " + nameField);
    //                    Storage.Instance.DebugKill(p_gobject.name);

    //                    //@KOSTIL@ --------------------------------------------------------------
    //                    //newObject = new GameDataUfo()
    //                    //{
    //                    //    NameObject = p_gobject.name,
    //                    //    TagObject = p_gobject.tag,
    //                    //    Position = p_gobject.transform.position
    //                    //};
    //                    //newObject.UpdateGameObject(p_gobject);
    //                    //--------------------------------------------------------------

    //                    //# 
    //                    return null;
    //                }
    //                newObject = objects[index] as GameDataUfo;

    //            }
    //            break;
    //        default:
    //            //Debug.Log("_______________________CreateObjectData_____________default " + prefabType);
    //            newObject = new ObjectData()
    //            {
    //                NameObject = p_gobject.name,
    //                TagObject = p_gobject.tag,
    //                Position = p_gobject.transform.position
    //            };
    //            break;
    //    }
    //    return newObject;
    //}

    public static ObjectData CreateObjectData(GameObject p_gobject, bool isNewGen = false)
    {
        ObjectData newObject;
        //#PPPP
        TypePrefabs prefabType = TypePrefabs.PrefabField;

        if (!String.IsNullOrEmpty(p_gobject.tag))
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
                    Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "   newObject=" + newObject + "             ~~~~~ DO: pos=" + newObject.Position + "  GO:  pos=" + p_gobject.transform.position);
                    newObject.UpdateGameObject(p_gobject);
                }
                else
                {
                    newObject = new GameDataUfo()
                    {
                        NameObject = p_gobject.name,
                        TagObject = p_gobject.tag,
                        Position = p_gobject.transform.position
                    };

                    //++++
                    //MovementUfo movementUfo = p_gobject.GetComponent<MovementUfo>();
                    //if (movementUfo == null)
                    //{
                    //    Debug.Log("####################  CreateObjectData (" + p_gobject.name + ")  movementUfo is EMPTY");
                    //    return null;
                    //}
                    //GameDataUfo newObjectClone = (GameDataUfo)movementUfo.DataUfo.Clone();
                    //if (newObjectClone == null)
                    //{
                    //    Debug.Log("####################  CreateObjectData (" + p_gobject.name + ")  movementUfo DataUfo is EMPTY");
                    //    return null;
                    //}
                    //if(newObjectClone.NameObject != newObject.NameObject)
                    //    Debug.Log("####################  CreateObjectData DataUfoClone(" + newObjectClone.NameObject + ")  DataUfo GO: (" + newObject.NameObject + ")");
                    //if (newObjectClone.Position != newObject.Position)
                    //    Debug.Log("####################  CreateObjectData  Position>> DataUfoClone(" + newObjectClone.Position + ")  DataUfo GO: (" + newObject.Position + ")");
                    //if (newObjectClone.TagObject != newObject.TagObject)
                    //    Debug.Log("####################  CreateObjectData DataUfoClone(" + newObjectClone.TagObject + ")  DataUfo GO: (" + newObject.TagObject + ")");
                    //if (newObjectClone.TargetPosition != ((GameDataUfo)newObject).TargetPosition)
                    //    Debug.Log("####################  CreateObjectData TargetPosition>>  DataUfoClone(" + newObjectClone.TagObject + ")  DataUfo GO: (" + ((GameDataUfo)newObject).TargetPosition + ")");
                    //++++

                    newObject.UpdateGameObject(p_gobject);
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
                    Debug.Log("################# Error FindObjectData DATA OBJECT NOT FOUND : " + p_gobject.name + "   in Field: " + nameField);
                    Storage.Instance.DebugKill(p_gobject.name);

                    //@KOSTIL@ --------------------------------------------------------------
                    //newObject = new GameDataUfo()
                    //{
                    //    NameObject = p_gobject.name,
                    //    TagObject = p_gobject.tag,
                    //    Position = p_gobject.transform.position
                    //};
                    //newObject.UpdateGameObject(p_gobject);
                    //--------------------------------------------------------------

                    //# 
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

        [XmlIgnore]
        public bool IsReality = false;

        public ObjectData() {
        }

        public virtual void UpdateGameObject(GameObject objGame)
        {
            //@PD@
            //var tempData = objGame.GetComponent<PersonalData>();
            //if (tempData != null)
            //{
            //    Debug.Log(">>>> UpdateGameObject  SAVE PERSONAL DATA ");

            //    tempData.PersonalObjectData = (ObjectData)this.Clone();
            //}
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
        //[XmlIgnore]
        //public Vector3 NewPosition;

        //[XmlIgnore]
        //public bool IsReality = false;

        //-----------------------------------for Ufo
        private Vector3 m_Position = new Vector3(0, 0, 0);
        public override Vector3 Position
        {
            get { return m_Position; }
            set
            {
                //@POS@ 
                //@POS@ Debug.Log("GameDataNPC (" + NameObject + ") --- SET Position (" + m_Position + ") = " + value);
                m_Position = value;
                //m_Position = Storage.ConvVector3(value);

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
            //NewPosition = new Vector3(0, 0, 0);
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

        public virtual string TestNextPosition(GameObject gobj, string lastNewName) //, Vector3 p_newPosition)
        {
            Debug.Log("***** lastNewName: " + lastNewName);
            var res = NextPosition(gobj);
            return res;
        }

        //public virtual void NextPosition(GameObject gobj) //, Vector3 p_newPosition)
        public virtual string NextPosition(GameObject gobj) //, Vector3 p_newPosition)
        {
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
                    Debug.Log("Error NextPosition (" + gobj.name + ")**************************** Field name: " + posFieldName + "   posFieldOld: " + posFieldOld + "   posFieldReal: " + posFieldReal + "   DN:" + NameObject );
                    //gobj.name = SaveLoadData.CreateName(TagObject, posFieldOld, "", gobj.name);
                    //nameObject = gobj.name;

                    Storage.Instance.GetHistory(gobj.name);

                    //gobj.PlayAnimation();
                    //Destroy(gobj, 3f);
                    Destroy(gobj);
                    return "";
                }

                bool isInZona = true;

                if (!Storage.Instance.IsValidPiontInZona(_newPosition.x, _newPosition.y))
                {
                    //@POS@ Debug.Log("######### NextPosition object (" + gobj.name + ") Not in RealZona.....");
                    //SetTargetPosition();
                    isInZona = false;
                }

                //-------------------
                //int p_x = (int)_newPosition.x;
                //int p_y = (int)_newPosition.y;
                //int x = (int)(p_x / 2);
                //int y = (int)(p_y / 2);
                //string fieldR = "Field" + (int)x + "x" + Mathf.Abs((int)y);
                //string strX = "  x: " + _newPosition.x + " -> " + p_x + " -> " + (p_x / 2) + " -> " + x;
                //string strY = "  y: " + _newPosition.y + " -> " + p_y + " -> " + (p_y / 2) + " -> " + y;
                //fieldR += strX + strY;
                //------------------

                //@POS@ Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~ NextPosition " + nameObject + "          " + posFieldOld + " > " + posFieldReal + "     " + _oldPosition + "  >>  " + _newPosition);
                //Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~ NextPosition " + fieldR);

                newName = Storage.Instance.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, !isInZona);
                //newName = Storage.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this);

                if (!isInZona && !string.IsNullOrEmpty(newName))
                {
                    //@POS@ Debug.Log("######### NextPosition Destroy on not in RealZona....");
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
                //Debug.Log("Error UpdateGameObject DATA:" + nameObjData + "  OBJECT: " + objGame.name);
                //Debug.Log("-------------------------------------------------------------------/" + nameObjData + "/ " + objGame.name + "/");
                //return;
                objGame.name = nameObjData;
            }

            //MovementUfo movementUfo = objGame.GetComponent<MovementUfo>();
            //if (movementUfo == null)
            //{
            //    Debug.Log("#################### UpdateGameObject (" + objGame.name + ")  movementUfo is EMPTY");
            //    return;
            //}
            //movementUfo.DataUfo = (GameDataUfo)this.Clone();

            objGame.GetComponent<SpriteRenderer>().color = ColorRender;
        }

        public override string ToString()
        {
            //return NameObject + " " + TagObject + " " + Position + " " + ColorRender;
            return NameObject + " " + TagObject + " " + Position.x + " " + Position.y;
            //return base.ToString();
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
            //objGame.GetComponent<PersonalData>().PersonalObjectData = (GameDataUfo)this.Clone();
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

        //#ST@ if (_gridData == null)
        if (Storage.Instance.GridDataG == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }

        Serializator.SaveGridXml(Storage.Instance.GridDataG, Storage.Instance.DataPathLevel);
    }

    //public void LoadObjectsNearHero()
    //{
    //    _scriptGrid.LoadObjectsNearHero();
    //}

    
    //public static class DictionaryExtensions
    //{
    //    public static IEnumerable<KeyValuePair<int, string>> GetComponents(this IEnumerable<KeyValuePair<int, string>> thisObj)
    //    {

    //        yield return new KeyValuePair<int, string>(1111, "aaa");
    //    }
    //}

    

}


