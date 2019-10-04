using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public partial class ModelNPC
{
    //Ej Iris Osoka Tussock Ground05 Ground04 Ground03 Ground02 Ground GrassSmall GrassMedium Grass RockDark RockValun RockBrown Klen Iva Sosna BlueBerry 
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
    //[XmlRoot("ObjRoot")]
    //[XmlType("Object")] //++
    //[XmlInclude(typeof(GameDataNPC))] //$$
    //[Serializable, XmlRoot("ObjRoot")]
    [XmlInclude(typeof(PersonData))]
    [XmlInclude(typeof(TerraData))]
    [XmlRoot(ElementName = "ObjectData"), XmlType("ObjectData")]
    public class ObjectData : ICloneable
    {
        //public string NameObject { get; set; }
        public string NameObject { get; set; }
        public string TypePoolPrefabName { get; set; }

        [XmlIgnore]
        public virtual int Defense { get; set; }
        public virtual int Health { get; set; }

        [XmlIgnore]
        public virtual PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolFloor; } }
        public string TypePrefabName { get; set; }

        [XmlIgnore]
        public virtual SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabField; } }
        public virtual string ModelView { get; set; }
        public virtual Vector3 Position { get; set; }
        //public virtual Vector3 Position { get { return position2; } }

        [XmlIgnore]
        public bool IsReality = false;

        [XmlIgnore]
        private string m_NameTransfer;
        [XmlIgnore]
        //public bool IsTransfer = false;
        public bool IsTransfer {  get { return !string.IsNullOrEmpty(m_NameTransfer); }  }
        
        public string Id { get; set; }

        public ObjectData()
        {
            TypePoolPrefabName = TypePoolPrefab.ToString();
            TypePrefabName = TypePrefab.ToString();
        }

        public void CreateID(string name)
        {
            Id = Helper.GetID(name);
            if (Storage.Instance.ReaderSceneIsValid)
                Storage.ReaderWorld.UpdateLinkData(this);
        }
        public void CreateID()
        {
            Id = Helper.GetID(NameObject);
            //error stackowerflow
            //if (Storage.Instance.ReaderSceneIsValid)
            //    Storage.ReaderWorld.UpdateLinkData(this);
        }

        public virtual void SetPosition(Vector3 newPosition)
        {
            Position = new Vector3(newPosition.x, newPosition.y, Position.z);
        }

        public virtual void SetNameObject(string newNameObject, bool isGeneric = false, string field = "", int index = -1, bool isTestValid = true)
        {
            NameObject = newNameObject;
            if (Storage.Instance.ReaderSceneIsValid && isTestValid)
                Storage.ReaderWorld.UpdateLinkData(this, isGeneric, field, index);
            //CreateID(NameObject);

            if (Id == null)
                CreateID();
        }

        public virtual void Init()
        {
        }

       
        public virtual void UpdateGameObject(GameObject objGame)
        {
        }

        public virtual void UpdateGameObjectAndID(GameObject objGame)
        {
            UpdateGameObject(objGame);
            if (Storage.Instance.ReaderSceneIsValid)
                Storage.ReaderWorld.UpdateLinkDataFormModel(this);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return NameObject + " " + TypePrefabName + " " + Position;
        }

        public void StartTransfer(string callName)
        {
            m_NameTransfer = callName;
            Storage.Data.LockResaveObjectsCount++;
        }
        public void StopTransfer()
        {
            m_NameTransfer = string.Empty;
            Storage.Data.LockResaveObjectsCount--;
            if (Storage.Data.LockResaveObjectsCount < 0)
                Storage.Data.LockResaveObjectsCount = 0;
        }
        public bool IsMoveValid()
        {
            //FIX~~TRANSFER
            //if (IsTransfer || Storage.Data.IsUpdatingLocationPersonGlobal)
            //    Debug.Log(Storage.EventsUI.ListLogAdd = "## Not IsMoveValid " + this.NameObject + " >> " + m_NameTransfer + "  IsHeroUpdate=" + Storage.Data.IsUpdatingLocationPersonGlobal);

            return IsTransfer == false && !Storage.Data.IsUpdatingLocationPersonGlobal;
        }
    }
     

    //#################################################################################################
    //>>> ObjectData -> GameDataNPC -> PersonData -> 
    //>>> ObjectData -> GameDataNPC -> PersonData -> PersonDataBoss -> GameDataBoss
    //>>> ObjectData -> GameDataUfo
    //>>> ObjectData -> GameDataNPC -> GameDataOther
    //#################################################################################################

    [XmlType("NPC")]
    public class GameDataNPC : ObjectData
    {
        [XmlIgnore]
        public bool IsLoadad { get; set; }

        [XmlIgnore]
        public virtual Vector3 TargetPosition { get; set; }

        public string TargetID { get; set; }

        [XmlIgnore]
        public int Speed { get; set; }

        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPerson; } }
       
        public string GetId
        {
            get
            {
                if (Id == null)
                    CreateID();
                return Id;
            }
        }

        private Vector3 m_Position = new Vector3(0, 0, 0);
        public override Vector3 Position
        {
            get { return m_Position; }
            set
            {
                m_Position = value;
                if (IsCanSetTargetPosition && IsReality)
                    SetTargetPosition();
            }
        }

        protected bool IsCanSetTargetPosition
        {
            get
            {
                return (TargetPosition == null || TargetPosition == new Vector3(0, 0, 0)) && m_Position != null && m_Position != new Vector3(0, 0, 0);
            }
        }

        public GameDataNPC() : base()
        {
            Speed = 1;
            TypePoolPrefabName = TypePoolPrefab.ToString();
            TypePrefabName = TypePrefab.ToString();
        }

        public void SetTargetPosition(Vector3 p_SetTarget)
        {
            TargetPosition = p_SetTarget;
            return;
        }

        public virtual void SetTargetPosition()
        {
            var _position = Position;
            int distX = UnityEngine.Random.Range(-6, 6);
            int distY = UnityEngine.Random.Range(-6, 6);
            float xT = _position.x + distX;
            float yT = _position.y + distY;

            Helper.ValidPiontInZonaWorld(ref xT, ref yT, distX);
            TargetPosition = new Vector3(xT, yT, -1);
        }

        [XmlIgnore]
        private bool _isError = false;

        public virtual string NextPosition(GameObject gobj) //, Vector3 p_newPosition)
        {
            if (Storage.Instance.IsLoadingWorld)
            {
                StopTransfer();
                return "";
            }
            if (ConfigDebug.IsTestDUBLICATE)
            {
                if (_isError)
                {
                    Debug.Log("################ Error NextPosition (" + gobj.name + ")   already IS ERROR ");
                    StopTransfer();
                    return "Error";
                }
                if (Storage.Instance.IsCorrectData)
                {
                    Debug.Log("_______________ RETURN CorrectData ON CORRECT_______________");
                    StopTransfer();
                    return "Error";
                }
                if (!IsReality)
                {
                    Debug.Log("_______________  Error NextPosition (" + gobj.name + ")   Not IsReality _______________");  //!@#$
                    StopTransfer();
                    return "Update";
                }
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

                if (ConfigDebug.IsTestDUBLICATE && posFieldName != posFieldOld)
                {
                    Debug.Log("################ Error NextPosition (" + gobj.name + ")   ERROR NAMES:  Old Field name: " + posFieldName + " !=  posFieldOld: " + posFieldOld + "  ------  posFieldReal: " + posFieldReal + "   DN:" + NameObject + " DataPos: " + Position.x + "x" + Position.y);
                    Storage.Log.GetHistory(gobj.name);
                    StopTransfer();
                    _isError = true;
                    Storage.Fix.CorrectData(null, gobj, "NextPosition");
                    return "Error";
                }

                bool isInZona = true;
                if (!Helper.IsValidPiontInZona(_newPosition.x, _newPosition.y))
                    isInZona = false;
                newName = Storage.Person.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona);
                if (!isInZona && !string.IsNullOrEmpty(newName))
                {
                    Storage.Instance.DestroyObject(gobj);
                }
            }
            return newName;
        }

        public virtual string Update(GameObject gobj)
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
            if (IsMoveValid())
            {
                StartTransfer("Update me");
                newName = Storage.Person.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona, true);
                StopTransfer();
            }

            if (!isInZona && !string.IsNullOrEmpty(newName))
            {
                Storage.Instance.DestroyObject(gobj);
            }
            if (Storage.Instance.ReaderSceneIsValid)
                Storage.ReaderWorld.UpdateLinkGobject(gobj);


            return newName;
        }

        [XmlIgnore] //@@@
        public virtual List<string> GetParams
        {
            get
            {
                return new List<string> {
                    "Name: " + NameObject,
                    "Pool: " + TypePoolPrefabName,
                    "Prefab: " + TypePrefabName,
                    "View: " + ModelView,
                    "Pos : " + Position,
                    "Target : " + TargetPosition,
                    "IsReality: " + IsReality,
                    //"Life: " + Life,
                    //"Speed: " + Speed,
                    //"Color : " + ColorLevel
                  };
            }
        }

        [XmlIgnore]
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
        public virtual string PortalId { get; set; }

        public string[] PersonActions { get; set; } //$$$
        public string CurrentAction { get; set; }
        public string JobName { get; set; }
        public DataObjectInventory Inventory { get; set; }

        [XmlIgnore]
        private AlienJob m_Job;
        [XmlIgnore]
        public AlienJob Job
        {
            get
            {
                if (m_Job == null && !string.IsNullOrEmpty(JobName))
                {
                    AlienJobsManager.GetJobFromName(ref m_Job, JobName, TypePrefab);
                }
                return m_Job;
            }
            set
            {
                m_Job = value;
                JobName = AlienJobsManager.GetNameJob(m_Job);
            }
        }


        public PersonData()
            : base()
        {
        }
    }

    #region Legacy Models

    [XmlType("Ufo")]
    public class GameDataUfo : GameDataNPC 
    {
        [XmlIgnore]
        public Color ColorRender = Color.black;
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPersonUFO; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabUfo; } }

        public GameDataUfo()
            : base()
        {
            TypePoolPrefabName = TypePoolPrefab.ToString();
            TypePrefabName = TypePrefab.ToString();
        }

        public override void Init()
        {
            System.Random rnd = new System.Random();
            TypePrefabName = TypePrefab.ToString();
            ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
            Speed = 3;
            IsLoadad = true;
            if (IsCanSetTargetPosition)
                SetTargetPosition();
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            base.UpdateGameObject(objGame);

            if (!IsLoadad && IsReality)
                Init();

            string nameObjData = ((GameDataUfo)this).NameObject;
            if (nameObjData != objGame.name)
            {

                objGame.name = nameObjData;
            }
            objGame.GetComponent<SpriteRenderer>().color = ColorRender;
        }

        public override string ToString()
        {

            return NameObject + " " + TypePrefabName + " " + Position.x + " " + Position.y;

        }

        [XmlIgnore] //@@@
        public override List<string> GetParams
        {
            get
            {
                return new List<string> {
                    "Name: " + NameObject,
                    "Pool: " + TypePoolPrefabName,
                    "Prefab: " + TypePrefabName,
                    "View: " + ModelView,
                    "Type : " + TypePrefabName,
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
        }
    }


    [XmlType("Boss")]
    public class GameDataBoss : PersonDataBoss
    //public class GameDataBoss : GameDataNPC
    {
        [XmlIgnore]
        Dictionary<int, Color> _colorsPresent = null;

        //[XmlIgnore]
        //public string NameSprite { get; private set; }

        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabBoss; } }

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
                if (IsReality &&  m_ColorRender != null && m_ColorRender != Color.clear)
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

                if (IsReality && !string.IsNullOrEmpty(_ColorLevel) && _ColorLevel != "#000000")
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

        public GameDataBoss() : base()
        {
            //TypePoolPrefabName = TypePoolPrefab.ToString();
            TypePrefabName = TypePrefab.ToString();
            //Init();
        }

        public override void Init()
        {
            //--- init person boss data
            //System.Random rng = new System.Random();
            TypePrefabName = TypePrefab.ToString();

            if (Level == 0)
            {
                Level = UnityEngine.Random.Range(1, 7);
                //Level = rng.Next(1, 7);
            }
            if (Life == 0)
                Life = Level * 10;
            Speed = Level;

            if (m_ColorRender != Color.clear)
                return;

            InitColor();

            //#fix load
            if (IsCanSetTargetPosition && IsReality)
                SetTargetPosition();

            IsLoadad = true;
        }

        private void InitColor()
        {
            //ColorRender = StoragePerson.GetColorsLevel[Level];
            ColorRender = Color.white;
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            base.UpdateGameObject(objGame);
            //#INTI
            if (!IsLoadad && IsReality)
                Init();

            bool isUpdateStyle = false;

            string nameObjData = ((GameDataBoss)this).NameObject;
            if (nameObjData != objGame.name)
            {
                objGame.name = nameObjData;
            }

            //if (ColorRender != StoragePerson.GetColorsLevel[Level])
            //{
            //    ColorRender = StoragePerson.GetColorsLevel[Level];
            //    isUpdateStyle = true;
            //    //Debug.Log(">>>>>>>>> colorStr ==" + ColorRender + "    Level:" + Level + "    GetColor:  " + GetColorsLevel[Level]);
            //}
            string _nameSprite = "";
            Sprite spriteMe = Storage.Palette.GetSpriteBoss(Level, out _nameSprite);
            ModelView = _nameSprite;
            if (spriteMe != null)
            {
                objGame.GetComponent<SpriteRenderer>().sprite = spriteMe;
            }
            else
            {
                Debug.Log("############## NOT Update new Sprite " + NameObject + "  Level=" + Level + " ???????????????");
                objGame.GetComponent<SpriteRenderer>().color = ColorRender;
            }
        }

        [XmlIgnore]
        public override List<string> GetParams
        {
            get
            {
                return new List<string> {
                    "Name: " + NameObject,
                    "Pool: " + TypePoolPrefabName,
                    "View: " + ModelView,
                    "Type : " + TypePrefabName,
                    "Pos : " + Position,
                    "Target : " + TargetPosition,
                    "Life: " + Life,
                    "Speed: " + Speed,
                    "Color : " + ColorLevel,
                    "Individ: " + ModelView
                  };
            }
        }
    }

    #endregion
    //--------------------------------------------------- TERRA ------------------------------

    [XmlType("Terra")]
    public class TerraData : ObjectData
    {
        //public string TileName { get; set; }
        public int Index { get; set; }
        public bool IsGen { get; set; }

        public virtual int BlockResources { get; set; }
        
        public virtual string Debuff { get; set; }
        public virtual string ParentId { get; set; }

        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolFloor; } }

        [XmlIgnore]
        private bool IsLoadad { get; set; }

        [XmlIgnore]
        private bool isUseAtlas = true; //false;//

        //Fix PrefabField
        //[XmlIgnore]
        //public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabField; } }
        //public TerraData() : base() { TypePrefabName = TypePrefab.ToString(); }

        public TerraData() : base()
        {
        }

        public void Init()
        {
            bool isGen = true;

            IsLoadad = true;

            if (Storage.TilesManager == null)
            {
                Debug.Log("######## Init TerraData: Storage.TilesManager is Empty");
                return;
            }
            //if (Storage.TilesManager.ListTexturs == null)
            //{
            //    Debug.Log("######## Init TerraData: TilesManager.ListTexturs is Empty");
            //    return;
            //}
            //#FIX null
            if ((!IsLoadad && !IsReality) || ModelView==null)
            {
                //fix legacy pool type
                if (ModelView == null && TypePrefabName != null)
                {
                    if (TypePrefabName == SaveLoadData.TypePrefabs.PrefabField.ToString())
                    {
                        //fix legacy pool type on gen World GenObjectWorld()
                        //ModelView = Storage.TilesManager.GenNameTileTerra();
                        Debug.Log("####### LEGACY CODE !!!!!!");
                        ModelView = null;
                    }
                    else
                    {
                        ModelView = TypePrefabName;
                    }
                }
                else
                {
                    Debug.Log("####### LEGACY CODE !!!!!!");
                    ModelView = null;
                }
                //ModelView = Storage.TilesManager.ListTexturs[0].name;
            }
            //if (Storage.Instance.ReaderSceneIsValid)
            //    Storage.ReaderWorld.UpdateLinkData(this);
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            base.UpdateGameObject(objGame);

            if ((!IsLoadad || ModelView == null) && IsReality)
            {
                Init();

                //if(PoolGameObjects.IsUseTypePoolPrefabs)
                //{
                //    objGame.name = NameObject;
                //    objGame.transform.position = Position;
                //}
            }

            //if (!isUseAtlas)
            //{
            //    objGame.GetComponent<SpriteRenderer>().sprite = Storage.TilesManager.CollectionSpriteTiles[ModelView];
            //}
            //else
            //------------------------- Atlas
            //{
                try
                {
                    objGame.GetComponent<SpriteRenderer>().sprite = Storage.Palette.SpritesPrefabs[ModelView];
                }catch(Exception x)
                {
                    Debug.Log("############## TerraData.UpdateGameObject ModelView: " + ModelView + "  " + x.Message);
                }
            //}
            //-------------------------

            //if (Storage.Instance.ReaderSceneIsValid)
            //    Storage.ReaderWorld.UpdateLinkDataFormModel(this);
        }
    }

    //------------------------------------------ WALLS
    [XmlType("Wall")]
    public class WallData : TerraData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolWall; } }

        //public WallData(bool isGen) : base(isGen) { }
        public WallData() : base() { }
    }

    public class WallRock : WallData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabWallRock; } }
        public WallRock() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    public class WallWood : WallData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabWallWood; } }
        public WallWood() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    //------------------------------------------

    

}

