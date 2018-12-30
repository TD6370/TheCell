﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class StoragePerson : MonoBehaviour {

    public bool IsShowTittlePerson = false;

    public Vector3 PersonsTargetPosition { get; set; }

    public Color ColorSelectedCursorObject = Color.cyan;
    public Color ColorFindCursorObject = Color.magenta;

    public static Texture2D TextureBossRedMap;
    public Texture2D TextureBossRed;
    public static Texture2D TextureBossLizardMap;
    public Texture2D TextureBossLizard;
    public static Texture2D TextureBossBandosMap;
    public Texture2D TextureBossBandos;
    public static Texture2D TextureBossBoobleMap;
    public Texture2D TextureBossBooble;
    public static Texture2D TextureBossAlienMap;
    public Texture2D TextureAlienBoss;
    public static Texture2D TextureBossDroidMap;
    public Texture2D TextureBossDroid;
    public static Texture2D TextureBossArmMap;
    public Texture2D TextureBossArm;
    public static Texture2D TextureBossMap;
    public Texture2D TextureBoss;
    //public static Texture2D TextureBossMap;
    //public Texture2D TextureBoss;

    //new TypeBoss() { NameTextura2D = "SpriteBossLizard", Level = 1, },
    //                    new TypeBoss() { NameTextura2D = "SpriteBossRed", Level = 2, ColorTrack = ManagerPalette.ColorBossLizard, TextureMap = StoragePerson.TextureBossRedMap },
    //                    new TypeBoss() { NameTextura2D = "SpriteBoss", Level = 3, ColorTrack = ManagerPalette.ColorBossBandos  },
    //                    new TypeBoss() { NameTextura2D = "SpriteBoss", Level = 4, ColorTrack = ManagerPalette.ColorBossBooble  },
    //                    new TypeBoss() { NameTextura2D = "SpriteBoss", Level = 5, ColorTrack = ManagerPalette.ColorBossAlien },
    //                    new TypeBoss() { NameTextura2D = "SpriteBossDroid", Level = 6, ColorTrack = ManagerPalette.ColorBossDroid },
    //                    new TypeBoss() { NameTextura2D = "SpriteBoss", Level = 7, ColorTrack = ManagerPalette.ColorBossArm },
    //                    new TypeBoss() { NameTextura2D = "SpriteBoss", Level = 8, ColorTrack = ManagerPalette.ColorBoss  },

    public static string _Ufo { get { return SaveLoadData.TypePrefabs.PrefabUfo.ToString(); } }
    public static string _Boss { get { return SaveLoadData.TypePrefabs.PrefabBoss.ToString(); } }

    public SpriteAtlas SpriteAtlasNPC;

    public TypeModifPerson ModificatorPerson = TypeModifPerson.PointPos;

    public enum TypeModifPerson
    {
        Alpha, 
        Kill,
        PointPos,
        Paint,
    }

    //#Atlas SpriteCollection
    private Dictionary<string, Sprite> m_SpriteCollection;
    public Dictionary<string, Sprite> SpriteCollection
    {
        get
        {
            if (m_SpriteCollection == null || m_SpriteCollection.Count ==0)
            {
                m_SpriteCollection = new Dictionary<string, Sprite>();

                Sprite[] spritesPrefabsAtlas = GetSpritesAtlasNPC();
                foreach (var sprt in spritesPrefabsAtlas)
                {
                    string namePrefab = sprt.name;//.GetNamePrefabByTextureMap();
                    namePrefab = namePrefab.Replace("(Clone)", "");
                    //Texture2D _texture = sprt.texture;
                    //Texture2D _texture = SpriteUtility.GetSpriteTexture(sprt, false /* getAtlasData */);
                    //_texture.Apply();
                    m_SpriteCollection.Add(namePrefab, sprt);
                }
            }
            return m_SpriteCollection;
        }
    }

    private static Dictionary<int, Color> _colorsPresent = null;
    public static Dictionary<int, Color> GetColorsLevel
    {
        get
        {
            if (_colorsPresent == null)
            {
                _colorsPresent = new Dictionary<int, Color>();
                foreach (var typeItem in TypeBoss.TypesBoss)
                {
                    _colorsPresent.Add(typeItem.Level, typeItem.ColorTrack);
                }
            }
            return _colorsPresent;
        }
    }

    //public Vector3 PersonsTargetPosition { get; set; }

    private ModelNPC.LevelData _personsData;
    public ModelNPC.LevelData PersonsData
    {
        get { return _personsData; }
    }

    public void PersonsDataInit(ModelNPC.LevelData _newData = null)
    {
        if (_newData == null)
            _personsData = new ModelNPC.LevelData();
        else
            _personsData = _newData;

        
    }

    void Awake()
    {
        

        PersonsDataInit();
        LoadTexturesMap();
        
    }

    // Use this for initialization
    void Start() {
        LoadSprites();
    }

    // Update is called once per frame
    void Update() {

    }

    private void LoadTexturesMap()
    {
        TextureBossRedMap = TextureBossRed;
        TextureBossLizardMap = TextureBossLizard;
        TextureBossBandosMap = TextureBossBandos;
        TextureBossBoobleMap = TextureBossBooble;
        TextureBossAlienMap = TextureAlienBoss;
        TextureBossDroidMap = TextureBossDroid;
        TextureBossArmMap = TextureBossArm;
        TextureBossMap = TextureBoss;
    }

    //private void LoadSprites()
    //{
    //    string indErr = "";
    //    try
    //    {
    //        indErr = "start";
    //        string pathSprites = "Textures/NPC/";
    //        int colSprites = 0;

    //        Debug.Log("Loading Sprites from Resources...");

    //        SpriteCollection = new Dictionary<string, Sprite>();

    //        foreach (string nameStrite in TypeBoss.TypesBoss.Select(p => p.NameTextura2D).Distinct())
    //        {
    //            Texture2D[] _texturesBoss = Resources.LoadAll<Texture2D>(pathSprites + nameStrite);

    //            if (_texturesBoss == null || _texturesBoss.Length == 0)
    //            {
    //                Debug.Log("############# Not Texture2D " + pathSprites + nameStrite + " IN Resources");
    //                continue;
    //            }
    //            Texture2D _texture = _texturesBoss[0];
    //            //Debug.Log("@@@@@@ Texture2D " + pathSprites + nameStrite + " IN Resources   " + _texture.width + "x" + _texture.height);
    //            Sprite spriteBoss = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //            if (!SpriteCollection.ContainsKey(nameStrite))
    //            {
    //                indErr = "6.";
    //                SpriteCollection.Add(nameStrite, spriteBoss);
    //                colSprites++;
    //            }
    //            else
    //            {
    //                Debug.Log("Sprite already exist in SpriteCollection  : " + nameStrite);
    //            }
    //        }
    //        Debug.Log("Loaded Sprites Boss : " + colSprites);
    //    }
    //    catch (Exception x)
    //    {
    //        Debug.Log("################# GetSpriteBoss #" + indErr + "  : " + x.Message);
    //    }
    //}

    public Sprite[] GetSpritesAtlasNPC()
    {
        Sprite[] spritesAtlas = new Sprite[SpriteAtlasNPC.spriteCount];
        SpriteAtlasNPC.GetSprites(spritesAtlas);
        return spritesAtlas;
    }

    private void LoadSprites()
    {
        if(SpriteAtlasNPC==null)
        {
            Debug.Log("########### SpriteAtlasNPCis empty");
            return;
        }

        //if (m_SpriteCollection == null || m_SpriteCollection.Count == 0)
        //{
        //    m_SpriteCollection = new Dictionary<string, Sprite>();

        //    Sprite[] spritesPrefabsAtlas = GetSpritesAtlasNPC();
        //    foreach (var sprt in spritesPrefabsAtlas)
        //    {
        //        string namePrefab = sprt.name;
        //        namePrefab = namePrefab.Replace("(Clone)", "");
        //        m_SpriteCollection.Add(namePrefab, sprt);
        //    }
        //}
        //--------------------------

        //SpriteCollection = new Dictionary<string, Sprite>();

        //Sprite[] spritesPrefabsAtlas = GetSpritesAtlasNPC();
        //foreach (var sprt in spritesPrefabsAtlas)
        //{
        //    string namePrefab = sprt.name;//.GetNamePrefabByTextureMap();
        //    namePrefab = namePrefab.Replace("(Clone)", "");
        //    //Texture2D _texture = sprt.texture;
        //    //Texture2D _texture = SpriteUtility.GetSpriteTexture(sprt, false /* getAtlasData */);
        //    //_texture.Apply();
        //    SpriteCollection.Add(namePrefab, sprt);
        //}

        //--------------------------

        //SpriteCollection = 

        //string indErr = "";
        //try
        //{
        //    indErr = "start";
        //    string pathSprites = "Textures/NPC/";
        //    int colSprites = 0;

        //    Debug.Log("Loading Sprites from Resources...");

        //    SpriteCollection = new Dictionary<string, Sprite>();

        //    foreach (string nameStrite in TypeBoss.TypesBoss.Select(p => p.NameTextura2D).Distinct())
        //    {
        //        Texture2D[] _texturesBoss = Resources.LoadAll<Texture2D>(pathSprites + nameStrite);

        //        if (_texturesBoss == null || _texturesBoss.Length == 0)
        //        {
        //            Debug.Log("############# Not Texture2D " + pathSprites + nameStrite + " IN Resources");
        //            continue;
        //        }
        //        Texture2D _texture = _texturesBoss[0];
        //        //Debug.Log("@@@@@@ Texture2D " + pathSprites + nameStrite + " IN Resources   " + _texture.width + "x" + _texture.height);
        //        Sprite spriteBoss = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        //        if (!SpriteCollection.ContainsKey(nameStrite))
        //        {
        //            indErr = "6.";
        //            SpriteCollection.Add(nameStrite, spriteBoss);
        //            colSprites++;
        //        }
        //        else
        //        {
        //            Debug.Log("Sprite already exist in SpriteCollection  : " + nameStrite);
        //        }
        //    }
        //    Debug.Log("Loaded Sprites Boss : " + colSprites);
        //}
        //catch (Exception x)
        //{
        //    Debug.Log("################# GetSpriteBoss #" + indErr + "  : " + x.Message);
        //}
    }

    //private void LoadSprites()
    //{
    //    string indErr = "";
    //    try
    //    {
    //        indErr = "start";
    //        string pathSprites = "Textures/NPC/";
    //        int colSprites = 0;

    //        Debug.Log("Loading Sprites from Resources...");

    //        SpriteCollection = new Dictionary<string, Sprite>();
    //        foreach (string nameStrite in TypeBoss.TypesBoss.Select(p => p.NameTextura2D).Distinct())
    //        {
    //            Texture2D[] _texturesBoss = Resources.LoadAll<Texture2D>(pathSprites + nameStrite);

    //            if (_texturesBoss == null || _texturesBoss.Length == 0)
    //            {
    //                Debug.Log("############# Not Texture2D " + pathSprites + nameStrite + " IN Resources");
    //                continue;
    //            }
    //            Texture2D _texture = _texturesBoss[0];
    //            //Debug.Log("@@@@@@ Texture2D " + pathSprites + nameStrite + " IN Resources   " + _texture.width + "x" + _texture.height);
    //            Sprite spriteBoss = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //            if (!SpriteCollection.ContainsKey(nameStrite))
    //            {
    //                indErr = "6.";
    //                SpriteCollection.Add(nameStrite, spriteBoss);
    //                colSprites++;
    //            }
    //            else
    //            {
    //                Debug.Log("Sprite already exist in SpriteCollection  : " + nameStrite);
    //            }
    //        }
    //        Debug.Log("Loaded Sprites Boss : " + colSprites);
    //    }
    //    catch (Exception x)
    //    {
    //        Debug.Log("################# GetSpriteBoss #" + indErr + "  : " + x.Message);
    //    }
    //}


    public IEnumerable<GameObject> GetAllRealPersons()
    {
        //??????????????? MissingReferenceException: The object of type 'GameObject' has been destroyed but you are still trying to access it.
        //        Your script should either check if it is null or you should not destroy the object.
        //        StoragePerson.< GetAllRealPersons > m__1(UnityEngine.GameObject p)(at Assets / Scripts / Storage / StoragePerson.cs:49)
        //System.Linq.Enumerable +< CreateWhereIterator > c__Iterator1D`1[UnityEngine.GameObject].MoveNext()
        //System.Collections.Generic.List`1[UnityEngine.GameObject].AddEnumerable(IEnumerable`1 enumerable)(at / Users / builduser / buildslave / mono / build / mcs /class/corlib/System.Collections.Generic/List.cs:128)
        //System.Collections.Generic.List`1[UnityEngine.GameObject]..ctor(IEnumerable`1 collection) (at /Users/builduser/buildslave/mono/build/mcs/class/corlib/System.Collections.Generic/List.cs:65)
        //System.Linq.Enumerable.ToList[GameObject] (IEnumerable`1 source)
        //StoragePerson.GetAllRealPersons() (at Assets/Scripts/Storage/StoragePerson.cs:49)
        //GenerateGridFields+<CalculateTealsObjects>c__Iterator0.MoveNext() (at Assets/Scripts/Storage/GenerateGridFields.cs:64)
        //UnityEngine.SetupCoroutine.InvokeMoveNext(IEnumerator enumerator, IntPtr returnValueAddress) (at C:/buildslave/unity/build/Runtime/Export/Coroutines.cs:17)

        return Storage.Instance.GamesObjectsReal.
                SelectMany(x => x.Value).
                Where(p => p!=null && (p.tag == _Ufo || p.tag == _Boss)).ToList();
    }

    public IEnumerable<GameObject> GetAllRealPersons(string field, bool  isModif = false)
    {
        try
        {
            //var count1= Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).ToList().Count();
            //Debug.Log("PERSON PAIR (" + field + ")  COUNT " + count1);

            foreach (GameObject gobjItem in Storage.Instance.GamesObjectsReal.
                Where(p => p.Key == field).
                SelectMany(x => x.Value).ToList())
            {
                if (isModif)
                {
                    ModifObject(gobjItem);
                }
                Debug.Log("OBJECT(" + field + ") : " + gobjItem);
            }

            return Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).
                    SelectMany(x => x.Value).
                    Where(p => p.tag == _Ufo || p.tag == _Boss).ToList();
        }catch(Exception x)
        {
            Debug.Log("###### GetAllRealPersons: " + x.Message);
        }
        return null;
    }

    public IEnumerable<GameObject> GetAllRealPersonsForID(string name)
    {
        string id = Helper.GetID(name);

        return Storage.Instance.GamesObjectsReal.
                SelectMany(x => x.Value).
                Where(p => p.name.IndexOf(id)!=-1).ToList();
    }

    public IEnumerable<ModelNPC.ObjectData> GetAllDataPersonsForID(string name)
    {
        string id = Helper.GetID(name);
        return GetAllDataPersonsForName(id);
    }

    public IEnumerable<ModelNPC.ObjectData> GetAllDataPersonsForName(string name)
    {
        return Storage.Instance.GridDataG.FieldsD.
            Select(x => x.Value).
            SelectMany(x => x.Objects).
            Where(p => p.NameObject.IndexOf(name) != -1).ToList();
    }


   

    public FindPersonData GetFindPersonsDataForName(string nameFind)
    {
        FindPersonData persData = null;
        
        foreach (var item in Storage.Instance.GridDataG.FieldsD)
        {
            string field = item.Key;
            ModelNPC.ObjectData dataObj = item.Value.Objects.Find(p => p.NameObject.IndexOf(nameFind) != -1);
            if(dataObj!=null)
            {

                int index = item.Value.Objects.FindIndex(p => p.NameObject.IndexOf(nameFind) != -1);
                persData = new FindPersonData()
                {
                    DataObj = dataObj,
                    Field = field,
                    Index = index
                };
                break;
            }
            //Storage.Instance.GridDataG.FieldsD.
            //   Select(x => x.Value).
            //   SelectMany(x => x.Objects).
            //   Where(p => p.NameObject.IndexOf(name) != -1).ToList();


        }
        return persData;
    }

    public List<SaveLoadData.TypePrefabs> TypesPersons
    {
        get
        {
            return new List<SaveLoadData.TypePrefabs>()
                {
                     SaveLoadData.TypePrefabs.PrefabUfo,
                     SaveLoadData.TypePrefabs.PrefabBoss
                };
        }
    }

    public List<string> NamesPersons
    {
        get
        {
            return new List<string>()
                {
                     SaveLoadData.TypePrefabs.PrefabUfo.ToString(),
                     SaveLoadData.TypePrefabs.PrefabBoss.ToString()
                };
        }
    }

    public void SelectedID(string gobjID)
    {

    }

    public void VeiwCursorGameObjectData(string _fieldCursor)
    {
        try
        {
            if (!Storage.Instance.Fields.ContainsKey(_fieldCursor))
                return;

            Storage.Instance.SelectFieldCursor = _fieldCursor;
            Storage.Events.ListLogAdd = "SelectFieldCursor: " + Storage.Instance.SelectFieldCursor;
            //Storage.Events.ListLogClear();
            GameObject prefabFind = Storage.Instance.Fields[_fieldCursor];

            if (prefabFind != null)
            {
                prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorSelectedCursorObject;
            }


            if (Storage.Map.IsOpen)
                return;

            foreach (var gobj in Storage.Person.GetAllRealPersons(_fieldCursor, true))
            {
                //Storage.Events.ListLogAdd = "FIND (" + _fieldCursor + "): " + gobj.name;

                gobj.GetComponent<SpriteRenderer>().color = ColorFindCursorObject;

                MovementNPC movement = gobj.GetComponent<MovementNPC>();
                ModelNPC.ObjectData findData = movement.GetData();
                var objData = SaveLoadData.FindObjectData(gobj);
                if (findData != objData)
                {
                    Storage.Events.ListLogAdd = "#### " + gobj.name + " conflict DATA";
                    Debug.Log("#### " + gobj.name + " conflict DATA");
                }

                var dataNPC = findData as ModelNPC.GameDataNPC;
                if (dataNPC != null)
                {
                    Storage.Events.ListLogAdd = "VeiwCursorGameObjectData: " + gobj.name + " NPC Params: " + dataNPC.GetParamsString;
                    Debug.Log("VeiwCursorGameObjectData: " + gobj.name + " NPC Params: " + dataNPC.GetParamsString);

                    //#EXPAND
                    Storage.Events.AddExpandPerson(dataNPC.NameObject,
                        dataNPC.GetParams,
                        new List<string> { "Pause", "Kill", "StartTrack" },
                        gobjObservable: gobj);
                }
                else
                {
                    Debug.Log("VeiwCursorGameObjectData: " + gobj.name + "  Not is NPC");
                    //ModifObject(gobj);
                }


                var dataBoss = findData as ModelNPC.GameDataBoss;
                if (dataBoss != null)
                {
                    Storage.Events.ListLogAdd = "YES GameDataBoss " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ";
                    Debug.Log("YES GameDataBoss " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ");
                    dataBoss.ColorRender = Color.magenta;

                    //#EXPAND
                    Storage.Events.AddExpandPerson(dataBoss.NameObject,
                        dataBoss.GetParams,
                        new List<string> { "Pause", "Kill", "StartTrack" },
                        gobjObservable: gobj);
                }
                else
                {
                    if (gobj.tag == _Boss)
                    {
                        Storage.Events.ListLogAdd = "#### " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ";
                        Debug.Log("#### " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ");
                    }
                }

            }
        }catch(Exception x)
        {
            Debug.Log("##############  VeiwCursorGameObjectData: " + x.Message);
        }
    }

    //public void TartgetPositionAll()
    //{
    public void SetTartgetPositionAll(Vector2 posCursorToField)
    {
        Debug.Log("SetTartgetPositionAll : to " + posCursorToField.x + "" + posCursorToField.y);

        //foreach (var gobj in Storage.Person.GetAllRealPersons())
        //{
        //    Debug.Log("SetTartgetPositionAll : " + gobj.name + " to : " + posCursorToField.x + "" + posCursorToField.y);
        //    MovementNPC movem = gobj.GetComponent<MovementNPC>();
        //    ModelNPC.GameDataNPC dataNPC = movem.GetData();
        //    dataNPC.SetTargetPosition(posCursorToField);
        //}
        PersonsTargetPosition = posCursorToField;// new Vector3(posCursorToField.x,posCursorToField.y,0);
        TartgetPositionAll(); //<< dataNPC.SetTargetPosition(Storage.Instance.PersonsTargetPosition);
    }

    //#TARGET
    public void TartgetPositionAll()
    {
        Debug.Log("^^^^^^^^ TARGET --- TartgetPositionAll");//#TARGET

        //PersonsTargetPosition
        foreach (GameObject gobj in Storage.Person.GetAllRealPersons().ToList())
        {
            //if (Storage.Person.NamesPersons.Contains(gobj.tag.ToString()))
            //if (typeP.IsPerson())
            if (gobj.tag.ToString().IsPerson())
            {
                var movementUfo = gobj.GetMoveUfo();
                if (movementUfo != null)
                    movementUfo.SetTarget();

                var movementNPC = gobj.GetMoveNPC();
                if (movementNPC != null)
                    movementNPC.SetTarget();

                //var movementNPC = gobj.GetMoveBoss();
                //if (movementNPC != null)
                //    movementNPC.SetTarget();
            }
        }
    }

    public void ModifObject(GameObject gobj)
    {
        Storage.Events.ListLogAdd = "Modif : " + ModificatorPerson + " > " + gobj.name;
        switch(ModificatorPerson)
        {
            case TypeModifPerson.Alpha:
                gobj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
                break;
            case TypeModifPerson.Kill:
                Storage.Instance.AddDestroyGameObject(gobj);
                break;

            case TypeModifPerson.PointPos:
                Storage.Events.PointGO.transform.position = gobj.transform.position;
                break;
            default:
                Storage.Events.ListLogAdd = "Empty modificator > " + ModificatorPerson.ToString();
                break;
        }

        
    }

    public ModelNPC.GameDataAlien GenTypeAlien()
    {
        int maxT = Storage.GridData.NamesPrefabNPC.Count;
        int indT = UnityEngine.Random.Range(0, maxT);
        //Level = rng.Next(1, 7);

        ModelNPC.GameDataAlien obj = null;

        //TypeAlien = Storage.Person.GenTypeAlien()
        string TypeAlien = Storage.GridData.NamesPrefabNPC[indT];
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Inspector.ToString())
            obj = new ModelNPC.GameDataAlienInspector();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Machinetool.ToString())
            obj = new ModelNPC.GameDataAlienMachinetool();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Mecha.ToString())
            obj = new ModelNPC.GameDataAlienMecha();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Dendroid.ToString())
            obj = new ModelNPC.GameDataAlienDendroid();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Dendroid.ToString())
            obj = new ModelNPC.GameDataAlienDendroid();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Garry.ToString())
            obj = new ModelNPC.GameDataAlienGarry();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Lollipop.ToString())
            obj = new ModelNPC.GameDataAlienLollipop();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Blastarr.ToString())
            obj = new ModelNPC.GameDataAlienBlastarr();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Hydragon.ToString())
            obj = new ModelNPC.GameDataAlienHydragon();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Pavuk.ToString())
            obj = new ModelNPC.GameDataAlienPavuk();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Skvid.ToString())
            obj = new ModelNPC.GameDataAlienSkvid();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Fantom.ToString())
            obj = new ModelNPC.GameDataAlienFantom();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Mask.ToString())
            obj = new ModelNPC.GameDataAlienMask();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Vhailor.ToString())
            obj = new ModelNPC.GameDataAlienVhailor();
        //if (TypeAlien == SaveLoadData.TypePrefabNPC..ToString())
        //    obj = new ModelNPC.GameDataAlien();
        obj.TypeAlien = TypeAlien;
        return obj;
    }
}

public static class PersonsExtensions
{
    public static bool IsPerson(this string typePrefab)
    {
        return Storage.Person.NamesPersons.Contains(typePrefab);
    }
    public static bool IsSpritePerson(this string typePrefab)
    {
        return typePrefab.IndexOf("Boss") != -1 ||
            typePrefab.IndexOf("Ufo") != -1; 
        //return Storage.Person.NamesPersons.Contains(typePrefab);
    }

    public static MovementUfo GetMoveUfo(this GameObject gobj)
    {
        var moveUfo = gobj.GetComponent<MovementUfo>();
        if (moveUfo != null)
            return moveUfo;
        return null;
    }

    public static MovementNPC GetMoveNPC(this GameObject gobj)
    {
        if (gobj == null)
            return null;
        var moveNPC = gobj.GetComponent<MovementNPC>();
        if (moveNPC != null)
            return moveNPC;
        return null;
    }

    public static EventsObject GetEvent(this GameObject gobj)
    {
        var evObj = gobj.GetComponent<EventsObject>();
        if (evObj != null)
            return evObj;
        return null;
    }

    //public ModelNPC.GameDataNPC GetData()
    public static ModelNPC.GameDataNPC GetDataNPC(this GameObject gobj)
    {
        var movNPC = gobj.GetMoveNPC();
        if (movNPC != null)
            return movNPC.GetData();
        return null;
    }

    //public static T GetDataModel(this GameObject gobj)
    //{
    //    var movNPC = gobj.GetMoveNPC();
    //    if (movNPC != null)
    //        return movNPC.GetData();
    //    return null;
    //}

    public static bool IsNPC(this GameObject gobj)
    {
        if (gobj = null)
            return false;
        var movNPC = gobj.GetMoveNPC();
        if (movNPC == null)
            return false;

        var dataNPC = movNPC.GetData();
        if (dataNPC == null)
            return false;

        return true;
    }

    public static bool IsUFO(this GameObject gobj)
    {
        return gobj.tag.Equals(StoragePerson._Ufo); 
    }

    public static void DisableComponents(this GameObject gobj)
    {
        var evObj = gobj.GetComponent<EventsObject>();
        if (evObj != null)
            evObj.PoolCase.IsDesrtoy = true;

        var movObj = gobj.GetComponent<MovementNPC>();
        if (movObj != null)
        {
            movObj.Pause();
            if(movObj.GetData()!=null)
                movObj.GetData().IsReality = false;
        }
    }
}

public class FindPersonData
{
    public FindPersonData() { }
    public ModelNPC.ObjectData DataObj { get; set; }
    public string Field { get; set; }
    public int Index { get; set; }
}

public class TypeBoss
{
    public List<TypeBoss> _TypesBoss;

    public static List<TypeBoss> TypesBoss
    {
        get
        {
            return Instance._TypesBoss;
        }
    }

    public TypeBoss() { }

    private static TypeBoss _instance;
    public static TypeBoss Instance
    {
        get
        {
            if (_instance == null)
            {
                //TextureBossLizardMap = TextureBossLizard;
                //TextureBossBandosMap = TextureBossBandos;
                //TextureBossBoobleMap = TextureBossBooble;
                //TextureBossAlienMap = TextureAlienBoss;
                //TextureBossDroidMap = TextureBossDroid;
                //TextureBossArmMap = TextureBossArm;
                //TextureBossMap =

                        _instance = new TypeBoss();
                _instance._TypesBoss = new List<TypeBoss>()
                    {
                         new TypeBoss(){ NameTextura2D= "SpriteBossLizard", Level=1, TextureMap= StoragePerson.TextureBossLizardMap},
                        new TypeBoss(){ NameTextura2D=  "SpriteBossRed", Level=2, ColorTrack=ManagerPalette.ColorBossLizard, TextureMap= StoragePerson.TextureBossRedMap },
                        new TypeBoss(){ NameTextura2D=  "SpriteBossBandos", Level=3, ColorTrack=ManagerPalette.ColorBossBandos,  TextureMap= StoragePerson.TextureBossBandosMap  },
                        new TypeBoss(){ NameTextura2D=  "SpriteBossBooble", Level=4, ColorTrack=ManagerPalette.ColorBossBooble,  TextureMap= StoragePerson.TextureBossBoobleMap  },
                        new TypeBoss(){ NameTextura2D=  "SpriteBossAlien", Level=5, ColorTrack=ManagerPalette.ColorBossAlien,  TextureMap= StoragePerson.TextureBossAlienMap },
                        new TypeBoss(){ NameTextura2D=  "SpriteBossDroid", Level=6, ColorTrack=ManagerPalette.ColorBossDroid,  TextureMap= StoragePerson.TextureBossDroidMap },
                        new TypeBoss(){ NameTextura2D= "SpriteBossArm", Level=7, ColorTrack=ManagerPalette.ColorBossArm,  TextureMap= StoragePerson.TextureBossArmMap },
                        new TypeBoss(){ NameTextura2D= "SpriteBoss", Level=8, ColorTrack=ManagerPalette.ColorBoss,  TextureMap= StoragePerson.TextureBossMap  },
                        new TypeBoss(){ NameTextura2D= "SpriteBoss", Level=9, ColorTrack=ManagerPalette.ColorBoss },
                        new TypeBoss(){ NameTextura2D= "SpriteBoss", Level=10, ColorTrack=ManagerPalette.ColorBoss },
                        new TypeBoss(){ NameTextura2D= "SpriteBoss", Level=12, ColorTrack=ManagerPalette.ColorBoss },
                        new TypeBoss(){ NameTextura2D= "SpriteBoss", Level=13, ColorTrack=ManagerPalette.ColorBoss },
                        new TypeBoss(){ NameTextura2D= "SpriteBoss", Level=14, ColorTrack=ManagerPalette.ColorBoss },
                        new TypeBoss(){ NameTextura2D= "SpriteBoss", Level=15, ColorTrack=ManagerPalette.ColorBoss },
                        new TypeBoss(){ NameTextura2D= "SpriteBoss", Level=16, ColorTrack=ManagerPalette.ColorBoss },
                    };
            }
            return _instance;
        }
    }

    public string NameTextura2D { get; set; }
    public int Level { get; set; }
    public Color ColorTrack { get; set; }
    public Texture2D TextureMap { get; set; }

    public string GetNameSpriteForIndexLevel(int p_level)
    {
        //string spriteName =  NemesSpritesBoss[index];
        //string spriteName = Instance._TypesBoss.Where(p => p.Level == p_level).Select(p => p.NameTextura2D).FirstOrDefault();
        if(!GetNamesSpritesNPC.ContainsKey(p_level))
        {
            Debug.Log("######### GetNameSpriteForIndexLevel not level " + p_level);
            return "Error";
        }
        string spriteName = GetNamesSpritesNPC[p_level];
        return spriteName;
    }

    private Dictionary<int, string> m_getNamesSpritesNPC = null;
    public Dictionary<int, string> GetNamesSpritesNPC
    {
        get
        {
            if(m_getNamesSpritesNPC==null)
            {
                m_getNamesSpritesNPC = new Dictionary<int, string>();
                foreach (var item in Instance._TypesBoss)
                {
                    m_getNamesSpritesNPC.Add(item.Level, item.NameTextura2D);
                }

            }
            return m_getNamesSpritesNPC;
        }
    }

    public Texture2D GetNameTextureMapForIndexLevel(int p_level)
    {
        //string _textureName = NemesTextureBoss[index];
        Texture2D _texture = Instance._TypesBoss.Where(p => p.Level == p_level).Select(p => p.TextureMap).FirstOrDefault();
        if(_texture==null)
        {
            Debug.Log("########### GetNameTextureMapForIndexLevel Not Texture in TypesBoss level = " + p_level);
        }

        //Texture2D _texture = Storage.Person.SpriteCollection[_textureName];
        return _texture;
        //-----
        //return null;
    }

    

    

    //public Texture2D GetNameTextureMapForIndexLevel(int p_level)
    //{
    //    //string _textureName = NemesTextureBoss[index];
    //    //Texture2D _texture = Instance._TypesBoss.Where(p => p.Level == p_level).Select(p => p.TextureMap).FirstOrDefault();
    //    //Texture2D _texture = Storage.Person.SpriteCollection[_textureName];
    //    //return _texture;
    //    //-----
    //    return null;
    //}

}
