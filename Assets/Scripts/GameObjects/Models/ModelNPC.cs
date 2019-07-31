using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ModelNPC
{

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
        public string NameObject { get; set; }
        public string TypePoolPrefabName { get; set; }

        [XmlIgnore]
        public virtual PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolFloor; } }
        public string TypePrefabName { get; set; }

        [XmlIgnore]
        public virtual SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabField; } }
        public virtual string ModelView { get; set; }
        public virtual Vector3 Position { get; set; }

        [XmlIgnore]
        public bool IsReality = false;

        public ObjectData()
        {
            TypePoolPrefabName = TypePoolPrefab.ToString();
            TypePrefabName = TypePrefab.ToString();
        }

        public virtual void Init()
        {
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
            return NameObject + " " + TypePrefabName + " " + Position;
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

        [XmlIgnore]
        public int Speed { get; set; }

        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolPerson; } }

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
        //-----------------------------------

        public GameDataNPC() : base()
        {
            Speed = 1;
            TypePoolPrefabName = TypePoolPrefab.ToString();
            TypePrefabName = TypePrefab.ToString();
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

            Helper.ValidPiontInZonaWorld(ref xT, ref yT, distX);
            TargetPosition = new Vector3(xT, yT, -1);
        }

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
            if (!IsReality)
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
                    Debug.Log("################ Error NextPosition (" + gobj.name + ")   ERROR NAMES:  Old Field name: " + posFieldName + " !=  posFieldOld: " + posFieldOld + "  ------  posFieldReal: " + posFieldReal + "   DN:" + NameObject + " DataPos: " + Position.x + "x" + Position.y);
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

            newName = Storage.Person.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona, true);
            if (!isInZona && !string.IsNullOrEmpty(newName))
            {
                Storage.Instance.DestroyObject(gobj);
            }
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
                    "Type : " + TypePrefabName,
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

        public string Id { get; set; }

        public PersonData()
            : base()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

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
            if(!IsLoadad && IsReality)
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
            ColorRender = StoragePerson.GetColorsLevel[Level];
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            //#INTI
            if (!IsLoadad && IsReality)
                Init();

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
            string _nameSprite = "";
            Sprite spriteMe = Storage.GridData.GetSpriteBoss(Level, out _nameSprite);
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

    [XmlType("PersonAlien")]
    public class PersonDataAlien : PersonData
    {
        public PersonDataAlien()
            : base()
        {
        }
    }


    [XmlType("Alien")]
    public class GameDataAlien : PersonDataAlien
    {
        public virtual int Life { get; set; }
        [XmlIgnore]
        public virtual int Level { get; set; }

        public GameDataAlien() : base()
        {
        }

        public override void Init()
        {
            //--- init person boss data
            if (string.IsNullOrEmpty(TypePrefabName))
            {
                GameDataAlien obj = Storage.Person.GenTypeAlien();
                ModelView = obj.ModelView;
                Level = obj.Level;
                Life = obj.Life;
                Speed = Level * 10;
            }

            //#fix load
            if (IsCanSetTargetPosition && IsReality)
                SetTargetPosition();

            IsLoadad = true;
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            //#INTI
            if (!IsLoadad && IsReality)
                Init();

            bool isUpdateStyle = false;

            
            if (NameObject != objGame.name)
            {
                objGame.name = NameObject;
            }
            Sprite spriteMe = Storage.Palette.SpritesWorldPrefabs[ModelView];  //Storage.GridData.GetSpriteBoss(Level, out _nameSprite);
            if (spriteMe != null)
            {
                objGame.GetComponent<SpriteRenderer>().sprite = spriteMe;
            }
            else
            {
                Debug.Log("############## NOT Update new Sprite " + NameObject + "   ???????????????");
                objGame.GetComponent<SpriteRenderer>().color = Color.red;
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
                    "Prefab: " + TypePrefabName,
                    "View: " + ModelView,
                    "Type : " + TypePrefabName,
                    "Pos : " + Position,
                    "Target : " + TargetPosition,
                    "Life: " + Life,
                    "Speed: " + Speed,
                    "Individ: " + ModelView
                  };
            }
        }
    }

    //Inspector,
    //Machinetool,
    //Mecha,

    //Dendroid,
    //Garry,
    //Lollipop,

    //Blasarr,
    //Hydragon,
    //Pavuk,
    //Skvid,

    //Fantom,
    //Mask,
    //Vhailor,
    [XmlType("Inspector")]
    public class GameDataAlienInspector : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Inspector; } }
        public override int Life { get { return 30; } set { } }
        public override int Level { get { return 2; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Machinetool")]
    public class GameDataAlienMachinetool : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Machinetool; } }
        public override int Life { get { return 50; } set {} }
        public override int Level { get { return 6; } set {} }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Mecha")]
    public class GameDataAlienMecha : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Mecha; } }
        public override int Life { get { return 40; } set { } }
        public override int Level { get { return 3; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Dendroid")]
    public class GameDataAlienDendroid : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Dendroid; } }
        public override int Life { get { return 80; } set { } }
        public override int Level { get { return 6; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Garry")]
    public class GameDataAlienGarry : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Gary; } }
        public override int Life { get { return 40; } set { } }
        public override int Level { get { return 1; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Lollipop")]
    public class GameDataAlienLollipop : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Lollipop; } }
        public override int Life { get { return 10; } set { } }
        public override int Level { get { return 3; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Blastarr")]
    public class GameDataAlienBlastarr : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Blastarr; } }
        public override int Life { get { return 100; } set { } }
        public override int Level { get { return 8; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Hydragon")]
    public class GameDataAlienHydragon : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Hydragon; } }
        public override int Life { get { return 50; } set { } }
        public override int Level { get { return 6; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Pavuk")]
    public class GameDataAlienPavuk : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Pavuk; } }
        public override int Life { get { return 80; } set { } }
        public override int Level { get { return 7; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Skvid")]
    public class GameDataAlienSkvid : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Skvid; } }
        public override int Life { get { return 30; } set { } }
        public override int Level { get { return 4; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Fantom")]
    public class GameDataAlienFantom : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Fantom; } }
        public override int Life { get { return 60; } set { } }
        public override int Level { get { return 4; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Mask")]
    public class GameDataAlienMask : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Mask; } }
        public override int Life { get { return 10; } set { } }
        public override int Level { get { return 8; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }

    [XmlType("Vhailor")]
    public class GameDataAlienVhailor : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Vhailor; } }
        public override int Life { get { return 100; } set { } }
        public override int Level { get { return 5; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }
    //--------------------------------------------------- TERRA ------------------------------

    [XmlType("Terra")]
    public class TerraData : ObjectData
    {
        //public string TileName { get; set; }
        public int Index { get; set; }
        public bool IsGen { get; set; }

        public virtual int BlockResources { get; set; }
        public virtual int Defence { get; set; }
        public virtual string Debuff { get; set; }
        public virtual int HP { get; set; }
        public virtual string ParentId { get; set; }

        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolFloor; } }

        [XmlIgnore]
        private bool IsLoadad { get; set; }

        private string idTerra = "?";

        [XmlIgnore]
        private bool isUseAtlas = true; //false;//

        public TerraData() : base()
        {
        }

        public void Init()
        {
            idTerra = Guid.NewGuid().ToString().Substring(0, 4);
            bool isGen = true;

            IsLoadad = true;

            if (Storage.TilesManager == null)
            {
                Debug.Log("######## Init TerraData: Storage.TilesManager is Empty");
                return;
            }
            if (Storage.TilesManager.ListTexturs == null)
            {
                Debug.Log("######## Init TerraData: TilesManager.ListTexturs is Empty");
                return;
            }
            //#FIX null
            if ((!IsLoadad && !IsReality) || ModelView==null)
            {
                if (isGen)
                    ModelView = Storage.TilesManager.GenNameTileTerra();
                else
                    ModelView = Storage.TilesManager.ListTexturs[0].name;
            }
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            if ((!IsLoadad || ModelView == null) && IsReality)
                Init();

            if (!isUseAtlas)
            {
                objGame.GetComponent<SpriteRenderer>().sprite = Storage.TilesManager.CollectionSpriteTiles[ModelView];
            }
            else
            //------------------------- Atlas
            {
                objGame.GetComponent<SpriteRenderer>().sprite = Storage.Palette.SpritesPrefabs[ModelView];
            }
            //-------------------------
        }
    }

    [XmlType("Wall")]
    public class WallData : TerraData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolWall; } }

        //public WallData(bool isGen) : base(isGen) { }
        public WallData() : base() { }
    }
    
    [XmlType("Floor")]
    public class FloorData : TerraData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolFloor ; } }

        //public WallData(bool isGen) : base(isGen) { }
        public FloorData() : base() { }
    }

    [XmlType("Flore")]
    public class FloreData : TerraData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolFlore; } }

        public FloreData() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Wood")]
    public class WoodData : WallData
    {
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PoolWood; } }

        public WoodData() : base() { }
    }

    //------------------------------------------------

    [XmlType("Kolba")]
    public class Kolba : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }

        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Kolba; } }
        public Kolba() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    
    [XmlType("Lantern")]
    public class Lantern : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Lantern; } }
        public Lantern() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Bananas")]
    public class Bananas : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Bananas; } }
        public Bananas() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Cluben")]
    public class Cluben : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Cluben; } }
        public Cluben() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Chpok")]
    public class Chpok : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Chpok; } }
        public Chpok() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Pandora")]
    public class Pandora : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Pandora; } }
        public Pandora() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Nadmozg")]
    public class Nadmozg : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Nadmozg; } }
        public Nadmozg() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Triffid")]
    public class Triffid : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Triffid; } }
        public Triffid() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Aracul")]
    public class Aracul : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Aracul; } }
        public Aracul() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Cloudwood")]
    public class Cloudwood : WoodData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Cloudwood; } }
        public Cloudwood() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    
    //----------------------------------------------

    [XmlType("Chip")]
    public class Chip : FloorData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Chip; } }
        public Chip() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Gecsagon")]
    public class Gecsagon : FloorData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Gecsagon; } }
        public Gecsagon() : base() { TypePrefabName = TypePrefab.ToString(); }
    }


    [XmlType("Kamish")]
    public class Kamish : FloreData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Kamish; } }
        public Kamish() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Boloto")]
    public class Boloto : FloreData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Boloto; } }
        public Boloto() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Weed")]
    public class Weed : FloorData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Weed; } }
        public Weed() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Weedflower")]
    public class Weedflower : FloorData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Weedflower; } }
        public Weedflower() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Berry")]
    public class Berry : FloreData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Berry; } }
        public Berry() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Mashrooms")]
    public class Mashrooms : FloreData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Mashrooms; } }
        public Mashrooms() : base() { TypePrefabName = TypePrefab.ToString(); }
    }


    [XmlType("Kishka")]
    public class Kishka : FloorData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Kishka; } }
        public Kishka() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Nerv")]
    public class Nerv : FloorData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Nerv; } }
        public Nerv() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Orbits")]
    public class Orbits : FloreData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Orbits; } }
        public Orbits() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Shampinion")]
    public class Shampinion : FloreData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Shampinion; } }
        public Shampinion() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    

    [XmlType("Desert")]
    public class Desert : FloorData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Desert; } }
        public Desert() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Parket")]
    public class Parket : FloorData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Parket; } }
        public Parket() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Corals")]
    public class Corals : FloreData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Corals; } }
        public Corals() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    [XmlType("Diods")]
    public class Diods : FloreData
    {
        public override int Defence { get { return 10; } }
        public override int HP { get { return 10; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Diods; } }
        public Diods() : base() { TypePrefabName = TypePrefab.ToString(); }
    }

    //LagcyObjects ---------------------------
    public class Rock : WoodData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabRock; } }
        public Rock() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    public class Vood : WoodData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabVood; } }
        public Vood() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    public class Elka : WoodData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabElka; } }
        public Elka() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    public class WallRock : WoodData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabWallRock; } }
        public WallRock() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    public class WallWood : WoodData
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabWallWood; } }
        public WallWood() : base() { TypePrefabName = TypePrefab.ToString(); }
    }
    //------------------------------------------

}

