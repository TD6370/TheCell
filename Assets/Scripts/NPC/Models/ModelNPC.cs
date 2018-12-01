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

        //public Vector3 Position { get; set; }
        public virtual Vector3 Position { get; set; }

        [XmlIgnore]
        public bool IsReality = false;

        public ObjectData()
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
            Helper.ValidPiontInZonaWorld(ref xT, ref yT, distX);

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
                newName = Storage.Instance.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona);
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

            newName = Storage.Instance.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona, true);
            if (!isInZona && !string.IsNullOrEmpty(newName))
            {
                //Destroy(gobj);
                //Storage.Instance.AddDestroyGameObject(gobj);
                //@???@
                Storage.Instance.DestroyObject(gobj);
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
                    "IsReality: " + IsReality
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
        //public Color ColorRender = Color.red;

        public GameDataUfo()
            : base()
        {
            Init();
        }

        private void Init()
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

            //#fix  Color
            //Init();

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
            if (Level == 0)
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

        public GameDataBoss() : base()
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

    [XmlType("Terra")]
    public class TerraData : ObjectData
    {
        public string TileName { get; set; }
        public int Index { get; set; }
        public bool IsGen { get; set; }
        public int BlockResources { get; set; }

        private string idTerra = "?";

        //public TerraData(bool isGen) {
        public TerraData()
        {
            idTerra = Guid.NewGuid().ToString().Substring(0,4);

            bool isGen = true;

            if (Storage.TilesManager==null)
            {
                Debug.Log("######## Init TerraData: Storage.TilesManager is Empty");
                return;
            }
            if (Storage.TilesManager.ListTexturs == null)
            {
                Debug.Log("######## Init TerraData: TilesManager.ListTexturs is Empty");
                return;
            }

            if (isGen)
                TileName = Storage.TilesManager.GenNameTileTerra();
            else
                TileName = Storage.TilesManager.ListTexturs[0].name;
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            {
                //return;

                if(objGame.name.IndexOf("Field1x0")!=-1 )
                {
                    Debug.Log("TEST Filed " + TileName + "  IsGen=" + IsGen + " id=" + idTerra);
                }

                if (Storage.TilesManager == null)
                {
                    Debug.Log("############## NOT Update new Sprite " + NameObject + " TilesManager  is Empty");
                    return;
                }
                if (Storage.TilesManager.CollectionSpriteTiles == null)
                {
                    Debug.Log("############## NOT Update new Sprite " + NameObject + " TilesManager.CollectionTextureTiles  is Empty");
                    return;
                }

                if (!Storage.TilesManager.CollectionSpriteTiles.ContainsKey(TileName))
                {
                    Debug.Log("############## NOT Update new Sprite " + NameObject + " not found TileName: " + TileName);
                    return;
                }

                Sprite spriteTile = Storage.TilesManager.CollectionSpriteTiles[TileName];
                if (spriteTile != null)
                {
                    objGame.GetComponent<SpriteRenderer>().sprite = spriteTile;
                }
                else
                {
                    Debug.Log("############## NOT Update new Sprite " + NameObject + " by TileName L " + TileName);
                }
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

        //public WallData(bool isGen) : base(isGen) { }
        public WallData() { }
    }

}

