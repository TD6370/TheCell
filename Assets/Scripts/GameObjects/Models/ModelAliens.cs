using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;
using System.Collections.Generic;

public partial class ModelNPC
{

    [XmlType("Alien")]
    public class GameDataAlien : PersonData
    {
        public virtual int Life { get; set; }
        public virtual int PortalId { get; set; }

        [XmlIgnore]
        public virtual int Level { get; set; }

        [XmlIgnore]
        public float TimeEndCurrentAction = -1f;
        //[XmlIgnore]
        //public Vector3 MovePosition;
        [XmlIgnore]
        public float TimeTargetPriorityWait = 3f;
        [XmlIgnore]
        public string PrevousTargetID = "";
        [XmlIgnore]
        public string BaseLockedTargetID;

        public GameDataAlien() : base()
        {
        }

        public void ReturnBaseTarget()
        {
            TargetID = BaseLockedTargetID;
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

            if (PersonActions == null)
                PersonActions = new string[] { };
            IsLoadad = true;
        }

        public override void UpdateGameObject(GameObject objGame)
        {
            base.UpdateGameObject(objGame);
            //#INTI
            if (!IsLoadad && IsReality)
                Init();

            bool isUpdateStyle = false;


            if (NameObject != objGame.name)
            {
                objGame.name = NameObject;
            }
            //fix legacy pool type
            if (ModelView == null)
                ModelView = TypePrefabName;

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

            if (Storage.Instance.ReaderSceneIsValid)
                Storage.ReaderWorld.UpdateLinkData(this);
        }

        public void OnTargetCompleted()
        {
            TargetID = string.Empty;
            TargetPosition = Vector3.zero;
        }

        [XmlIgnore]
        public override List<string> GetParams
        {
            get
            {
                var list = new List<string> {
                        "Name: " + NameObject,
                        "Pool: " + TypePoolPrefabName,
                        "Prefab: " + TypePrefabName,
                        "View: " + ModelView,
                        "Pos : " + Position,
                        "Target : " + TargetPosition,
                        "Life: " + Life,
                        "Speed: " + Speed,
                        "Individ: " + ModelView,
                        "Action: " + CurrentAction,
                       "Actions:",
                      };

                //string actions =
                //    PersonActions == null ?
                //    string.Empty :
                //    "Actions:\n" + string.Join("\n", PersonActions);
                list.AddRange(PersonActions);

                return list;
            }
        }
    }

    //-----------------------------------

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
        public override int Life { get { return 50; } set { } }
        public override int Level { get { return 6; } set { } }
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


    [XmlType("Ej")]
    public class GameDataAlienEj : GameDataAlien
    {
        [XmlIgnore]
        public override SaveLoadData.TypePrefabs TypePrefab { get { return SaveLoadData.TypePrefabs.Ej; } }
        public override int Life { get { return 20; } set { } }
        public override int Level { get { return 1; } set { } }
        public override void Init()
        {
            base.Init();
            TypePrefabName = TypePrefab.ToString();
        }
    }
}