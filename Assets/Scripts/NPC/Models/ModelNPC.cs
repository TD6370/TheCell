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
    [XmlInclude(typeof(PersonData))]
    [XmlInclude(typeof(TerraData))]
    //[XmlInclude(typeof(GameDataNPC))] //$$
    //[Serializable, XmlRoot("ObjRoot")]
    //[XmlRoot(Namespace = "www.my.com", ElementName = "MyGroupName", DataType = "string", IsNullable = true)]
    //[XmlRoot(Namespace = "www.my.com", ElementName = "ObjectData", IsNullable = true)]
    [XmlRoot(ElementName = "ObjectData"), XmlType("ObjectData")]
    public class ObjectData : ICloneable
    {
        public string NameObject { get; set; }

        public string TagObject { get; set; }

        //public string TypePoolPrefabName { get { return TypePoolPrefab.ToString(); } }
        public string TypePoolPrefabName { get; set; }
        [XmlIgnore]
        public virtual PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }

        public string TypePrefabName { get; set; }
        [XmlIgnore]
        public virtual SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.PrefabField; } }

        public virtual string ModelView { get; set; }
        
        //public Vector3 Position { get; set; }
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
    public class GameDataNPC : ObjectData
    {
        [XmlIgnore]
        public bool IsLoadad { get; set; }

        [XmlIgnore]
        public virtual Vector3 TargetPosition { get; set; }

        [XmlIgnore]
        public int Speed { get; set; }

        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.Person; } }

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



                //newName = Storage.Instance.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, !isInZona);
                //@CD@ 
                newName = Storage.Person.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona);
                //

                //@?????@
                if (!isInZona && !string.IsNullOrEmpty(newName))
                {
                    //Storage.Instance.AddDestroyGameObject(gobj);
                    Storage.Instance.DestroyObject(gobj);
                    //Destroy(gobj);
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
                //Destroy(gobj);
                //Storage.Instance.AddDestroyGameObject(gobj);
                //@???@
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
                    "Type : " + TagObject,
                    "Pos : " + Position,
                    "Target : " + TargetPosition,
                    "IsReality: " + IsReality
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

    //-----------------
    [XmlType("Ufo")] //?
    public class GameDataUfo : GameDataNPC //?
    {
        [XmlIgnore]
        public Color ColorRender = Color.black;
        //public Color ColorRender = Color.red;

        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.PersonUFO; } }

        public GameDataUfo()
            : base()
        {
            //Init();
        }

        public override void Init()
        {
            System.Random rnd = new System.Random();

            //float r = rnd.Next(1, 100) / 100;
            //float g = rnd.Next(1, 100) / 100;
            //float b = rnd.Next(1, 100) / 100;
            ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
            //ColorRender = new Color(r, g, b, 1);
            Speed = 3;
            IsLoadad = true;

            //if (IsCanSetTargetPosition && IsReality)
            if (IsCanSetTargetPosition)
                SetTargetPosition();
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            //#INTI
            if(!IsLoadad && IsReality)
                Init();

            string nameObjData = ((GameDataUfo)this).NameObject;
            if (nameObjData != objGame.name)
            {

                objGame.name = nameObjData;
            }

            //#fix  Color
            //Init();

            objGame.GetComponent<SpriteRenderer>().color = ColorRender;
        }

        public override string ToString()
        {

            return NameObject + " " + TagObject + " " + Position.x + " " + Position.y;

        }

        [XmlIgnore] //@@@
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
            //System.Random rng = new System.Random();

            //if (Level == 0)
            //{
            //    //Level = UnityEngine.Random.Range(1, 7);
            //    Level = rng.Next(1,7);
            //}

            //if (Life == 0)
            //    Life = Level * 10;

            //Speed = Level;
        }
    }


    [XmlType("Boss")]
    public class GameDataBoss : PersonDataBoss
    //public class GameDataBoss : GameDataNPC
    {
        [XmlIgnore]
        Dictionary<int, Color> _colorsPresent = null;

        [XmlIgnore]
        public string NameSprite { get; private set; }

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
            //Init();
        }


        public override void Init()
        {
            //--- init person boss data
            //System.Random rng = new System.Random();

            if (Level == 0)
            {
                Level = UnityEngine.Random.Range(1, 7);
                //Level = rng.Next(1, 7);
            }

            if (Life == 0)
                Life = Level * 10;

            Speed = Level;
            //---------------------

            //Speed = 5;

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
            //ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
            //Debug.Log(">>>>>>>>> colorStr INIT ==" + ColorRender + "    Level:" + Level);
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


            //if (isUpdateStyle)
            //{
            //Sprite spriteMe = Storage.GridData.GetSpriteBoss(Level);
            string _nameSprite = "";
            Sprite spriteMe = Storage.GridData.GetSpriteBoss(Level, out _nameSprite);
            //NameSprite = spriteMe.name;
            NameSprite = _nameSprite;
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

            //#pool#
            //var movement =  objGame.GetComponent<MovementBoss>();
            //if(PoolGameObjects.IsUsePoolObjects)
            //    movement.UpdateData("UpdateGameObject");
        }

        [XmlIgnore]
        public override List<string> GetParams
        {
            get
            {
                return new List<string> {
                    "Name: " + NameObject,
                    "Type : " + TagObject,
                    "Pos : " + Position,
                    "Target : " + TargetPosition,
                    "Life: " + Life,
                    "Speed: " + Speed,
                    "Color : " + ColorLevel,
                    "Individ: " + NameSprite
                  };
            }
        }
    }

    [XmlType("PersonAlien")]
    public class PersonDataAlien : PersonData
    {
        public string TypeAlien { get; set; }

        
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
        public string NameSprite { get; private set; }

        [XmlIgnore]
        public virtual int Level { get; set; }

        public GameDataAlien() : base()
        {
        }

        public override void Init()
        {
            //--- init person boss data
            //System.Random rng = new System.Random();

            if (string.IsNullOrEmpty(TypeAlien))
            {
                //int maxT = Storage.GridData.NamesPrefabNPC.Count;
                //int indT = UnityEngine.Random.Range(0, maxT);
                //Level = rng.Next(1, 7);

                //TypeAlien = Storage.Person.GenTypeAlien()
                //TypeAlien = Storage.GridData.NamesPrefabNPC[indT];
                //NameSprite = TypeAlien;
                //GameDataAlien obj = new GameDataAlienInspector();
                GameDataAlien obj = Storage.Person.GenTypeAlien();
                TypeAlien = obj.TypeAlien;// Storage.Person.GenTypeAlien()
                NameSprite = obj.NameSprite;
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

            //?????
            //isUpdateStyle = true;

            //if (ColorRender != StoragePerson.GetColorsLevel[Level])
            //{
            //    ColorRender = StoragePerson.GetColorsLevel[Level];
            //    isUpdateStyle = true;
            //    //Debug.Log(">>>>>>>>> colorStr ==" + ColorRender + "    Level:" + Level + "    GetColor:  " + GetColorsLevel[Level]);
            //}
            
            Sprite spriteMe = Storage.Palette.SpritesWorldPrefabs[NameSprite];  //Storage.GridData.GetSpriteBoss(Level, out _nameSprite);
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
                    "Type : " + TagObject,
                    "Pos : " + Position,
                    "Target : " + TargetPosition,
                    "Life: " + Life,
                    "Speed: " + Speed,
                    "Individ: " + NameSprite
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

        public override int Life
        {
            get
            {
                //return base.Life;
                return 30;
            }
            set
            {
                //base.Life = value;
            }
        }

        public override int Level
        {
            get
            {
                return 2;
            }

            set
            {
                //base.Level = value;
            }
        }
    }

    [XmlType("Machinetool")]
    public class GameDataAlienMachinetool : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Machinetool; } }
        public override int Life
        {
            get
            {
                //return base.Life;
                return 50;
            }
            set
            {
                //base.Life = value;
            }
        }

        public override int Level
        {
            get
            {
                return 6;
            }

            set
            {
                //base.Level = value;
            }
        }
    }

    [XmlType("Mecha")]
    public class GameDataAlienMecha : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Mecha; } }
        public override int Life
        {
            get
            {
                //return base.Life;
                return 40;
            }
        }

        public override int Level
        {
            get
            {
                return 3;
            }
        }
    }

    [XmlType("Dendroid")]
    public class GameDataAlienDendroid : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Dendroid; } }
        public override int Life
        {
            get
            {
                return 80;
            }
        }

        public override int Level
        {
            get
            {
                return 6;
            }
        }
    }

    [XmlType("Garry")]
    public class GameDataAlienGarry : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Garry; } }
        public override int Life
        {
            get
            {
                return 40;
            }
        }

        public override int Level
        {
            get
            {
                return 1;
            }
        }
    }

    [XmlType("Lollipop")]
    public class GameDataAlienLollipop : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Lollipop; } }
        public override int Life
        {
            get
            {
                return 10;
            }
        }

        public override int Level
        {
            get
            {
                return 3;
            }
        }
    }

    [XmlType("Blastarr")]
    public class GameDataAlienBlastarr : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Blastarr; } }
        public override int Life
        {
            get
            {
                return 100;
            }
        }

        public override int Level
        {
            get
            {
                return 8;
            }
        }
    }

    [XmlType("Hydragon")]
    public class GameDataAlienHydragon : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Hydragon; } }
        public override int Life
        {
            get
            {
                return 50;
            }
        }

        public override int Level
        {
            get
            {
                return 6;
            }
        }
    }

    [XmlType("Pavuk")]
    public class GameDataAlienPavuk : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Pavuk; } }
        public override int Life
        {
            get
            {
                return 80;
            }
        }

        public override int Level
        {
            get
            {
                return 7;
            }
        }
    }

    [XmlType("Skvid")]
    public class GameDataAlienSkvid : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Skvid; } }
        public override int Life
        {
            get
            {
                return 30;
            }
        }

        public override int Level
        {
            get
            {
                return 4;
            }
        }
    }

    [XmlType("Fantom")]
    public class GameDataAlienFantom : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Fantom; } }
        public override int Life
        {
            get
            {
                return 60;
            }
        }

        public override int Level
        {
            get
            {
                return 4;
            }
        }
    }

    [XmlType("Mask")]
    public class GameDataAlienMask : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Mask; } }
        public override int Life
        {
            get
            {
                return 10;
            }
        }

        public override int Level
        {
            get
            {
                return 8;
            }
        }
    }

    [XmlType("Vhailor")]
    public class GameDataAlienVhailor : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Vhailor; } }
        public override int Life
        {
            get
            {
                return 100;
            }
        }

        public override int Level
        {
            get
            {
                return 5;
            }
        }
    }

    //---------------------------------------------------

    [XmlType("Terra")]
    public class TerraData : ObjectData
    {
        //public string TileName { get; set; }
        public int Index { get; set; }
        public bool IsGen { get; set; }
        public int BlockResources { get; set; }

        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }

        [XmlIgnore]
        private bool IsLoadad { get; set; }

        private string idTerra = "?";

        [XmlIgnore]
        private bool isUseAtlas = true; //false;//

        //public TerraData(bool isGen) {
        public TerraData() : base()
        {
            //Init();
        }

        public void Init()
        {
            //if(!IsReality) //#FIX
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

            //#FIX
            //if (!IsLoadad && !IsReality)
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
            {
                //#INTI
                //if(!IsLoadad && IsReality)
                //#FIX null
                if ((!IsLoadad || ModelView == null) && IsReality)
                    Init();

                //return;
                if (!isUseAtlas)
                {
                    //if (Storage.TilesManager == null)
                    //{
                    //    Debug.Log("############## NOT Update new Sprite " + NameObject + " TilesManager  is Empty");
                    //    return;
                    //}
                    //if (Storage.TilesManager.CollectionSpriteTiles == null)
                    //{
                    //    Debug.Log("############## NOT Update new Sprite " + NameObject + " TilesManager.CollectionTextureTiles  is Empty");
                    //    return;
                    //}

                    //if (!Storage.TilesManager.CollectionSpriteTiles.ContainsKey(TileName))
                    //{
                    //    Debug.Log("############## NOT Update new Sprite " + NameObject + " not found TileName: " + TileName);
                    //    return;
                    //}
                    //Sprite spriteTile = Storage.TilesManager.CollectionSpriteTiles[TileName];
                    //if (spriteTile != null)
                    //{
                    //    objGame.GetComponent<SpriteRenderer>().sprite = spriteTile;
                    //}
                    //else
                    //{
                    //    Debug.Log("############## NOT Update new Sprite " + NameObject + " by TileName L " + TileName);
                    //}

                    objGame.GetComponent<SpriteRenderer>().sprite = Storage.TilesManager.CollectionSpriteTiles[ModelView];
                }
                else
                //------------------------- Atlas
                {
                    //if (!Storage.Palette.SpritesPrefabs.ContainsKey(TileName))
                    //{
                    //    Debug.Log("######### TerraData SpritesPrefabs not find TileName: " + TileName);
                    //    return;
                    //}
                    //Sprite spriteTile = Storage.TilesManager.CollectionSpriteTiles[TileName];
                    objGame.GetComponent<SpriteRenderer>().sprite = Storage.Palette.SpritesPrefabs[ModelView];
                }
                //-------------------------
            }
        }
    }

    [XmlType("Wall")]
    public class WallData : TerraData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }

        //public WallData(bool isGen) : base(isGen) { }
        public WallData() : base() { }
    }

    [XmlType("Floor")]
    public class FloorData : TerraData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }

        //public WallData(bool isGen) : base(isGen) { }
        public FloorData() : base() { }
    }

    [XmlType("Flore")]
    public class FloreData : TerraData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }

        public FloreData() : base() { }
    }

    //------------------------------------------------
    
    [XmlType("Kolba")]
    public class Kolba : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Kolba; } }
        public Kolba() : base() { }
    }
    
    [XmlType("Lantern")]
    public class Lantern : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Lantern; } }
        public Lantern() : base() { }
    }

    [XmlType("Bananas")]
    public class Bananas : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Bananas; } }
        public Bananas() : base() { }
    }

    [XmlType("Cluben")]
    public class Cluben : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Cluben; } }
        public Cluben() : base() { }
    }

    [XmlType("Chpok")]
    public class Chpok : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Chpok; } }
        public Chpok() : base() { }
    }

    [XmlType("Pandora")]
    public class Pandora : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Pandora; } }
        public Pandora() : base() { }
    }

    [XmlType("Nadmozg")]
    public class Nadmozg : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Nadmozg; } }
        public Nadmozg() : base() { }
    }

    [XmlType("Triffid")]
    public class Triffid : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Triffid; } }
        public Triffid() : base() { }
    }

    [XmlType("Aracul")]
    public class Aracul : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Aracul; } }
        public Aracul() : base() { }
    }

    [XmlType("Cloudwood")]
    public class Cloudwood : WallData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraWall; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Cloudwood; } }
        public Cloudwood() : base() { }
    }
    
    //----------------------------------------------

    [XmlType("Chip")]
    public class Chip : FloorData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Chip; } }
        public Chip() : base() { }
    }

    [XmlType("Gecsagon")]
    public class Gecsagon : FloorData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Gecsagon; } }
        public Gecsagon() : base() { }
    }


    [XmlType("Kamish")]
    public class Kamish : FloreData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFlore; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Kamish; } }
        public Kamish() : base() { }
    }

    [XmlType("Boloto")]
    public class Boloto : FloreData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFlore; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Boloto; } }
        public Boloto() : base() { }
    }

    [XmlType("Weed")]
    public class Weed : FloorData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Weed; } }
        public Weed() : base() { }
    }

    [XmlType("Weedflower")]
    public class Weedflower : FloorData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Weedflower; } }
        public Weedflower() : base() { }
    }

    [XmlType("Berry")]
    public class Berry : FloreData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFlore; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Berry; } }
        public Berry() : base() { }
    }

    [XmlType("Mashrooms")]
    public class Mashrooms : FloreData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFlore; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Mashrooms; } }
        public Mashrooms() : base() { }
    }


    [XmlType("Kishka")]
    public class Kishka : FloorData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Kishka; } }
        public Kishka() : base() { }
    }

    [XmlType("Nerv")]
    public class Nerv : FloorData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Nerv; } }
        public Nerv() : base() { }
    }

    [XmlType("Orbits")]
    public class Orbits : FloreData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFlore; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Orbits; } }
        public Orbits() : base() { }
    }

    [XmlType("Shampinion")]
    public class Shampinion : FloreData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFlore; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Shampinion; } }
        public Shampinion() : base() { }
    }
    

    [XmlType("Desert")]
    public class Desert : FloorData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Desert; } }
        public Desert() : base() { }
    }

    [XmlType("Parket")]
    public class Parket : FloorData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFloor; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Parket; } }
        public Parket() : base() { }
    }

    [XmlType("Corals")]
    public class Corals : FloreData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFlore; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Corals; } }
        public Corals() : base() { }
    }

    [XmlType("Diods")]
    public class Diods : FloreData
    {
        public int Defence { get; set; }
        public string Debuff { get; set; }
        public int HP { get; set; }
        public string ParentId { get; set; }
        [XmlIgnore]
        public override PoolGameObjects.TypePoolPrefabs TypePoolPrefab { get { return PoolGameObjects.TypePoolPrefabs.TerraFlore; } }
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Diods; } }
        public Diods() : base() { }
    }
    
        


}

