using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using System.Xml;
using System.Xml.Linq;

public class SaveLoadData : MonoBehaviour {

    //@NEWPREFAB@
    public GameObject PrefabVood;
    public GameObject PrefabRock;
    public GameObject PrefabUfo;
    public GameObject PrefabBoss;
    public GameObject PrefabElka;
    public GameObject PrefabWallRock;
    public GameObject PrefabWallWood;

    public GameObject PrefabField;

    public GameObject PrefabFlore;
    public GameObject PrefabPerson;
    public GameObject PrefabWall;
    public GameObject PrefabFloor;


    ////--- TAILS ---
    //public GameObject BackPalette;
    //public Grid GridTails;
    //public GameObject TailsMap;

    //public GameObject 
    public static float Spacing = 2f;

    private GenerateGridFields _scriptGrid;
    //private float LoadingWordlTimer = 0f;

    //#################################################################################################
    //>>> ObjectData -> GameDataNPC -> PersonData -> 
    //>>> ObjectData -> GameDataNPC -> PersonData -> PersonDataBoss -> GameDataBoss
    //>>> ObjectData -> GameDataUfo
    //>>> ObjectData -> GameDataNPC -> GameDataOther
    //#################################################################################################

    public List<string> NamesPrefabObjects;
    public List<string> NamesPrefabFloors;
    public List<string> NamesPrefabNPC;

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

    public enum TypePrefabsLegacy
    {
        PrefabField,
        PrefabRock,
        PrefabVood,
        PrefabUfo,
        PrefabBoss,
        PrefabElka,
        PrefabWallRock,
        PrefabWallWood,

        PrefabFlore,
        PrefabPerson,
        PrefabFloor
    }

    public enum TypePrefabs
    {
        PrefabField,
        PrefabRock,
        PrefabVood,
        PrefabUfo,
        PrefabBoss,
        PrefabElka,
        PrefabWallRock,
        PrefabWallWood,

        PrefabFlore,
        PrefabPerson,
        PrefabFloor,
        //-------------- Floor and Flore

        Swamp,
        Chip,
        Gecsagon,
        Kamish,

        Berry,
        Mashrooms,
        Weed,
        Weedflower,

        Kishka,
        Nerv,
        Orbits,
        Shampinion,

        Corals,
        Desert,
        Diods,
        Parket,
        //----- NPC
        Inspector,
        Machinetool,
        Mecha,

        Dendroid,
        Gary,
        Lollipop,

        Blastarr,
        Hydragon,
        Pavuk,
        Skvid,

        Fantom,
        Mask,
        Vhailor,
        //--------- Wall
        Kolba,
        Lantern,

        Bananas,
        Cluben,

        Chpok,
        Pandora,

        Nadmozg,
        Triffid,

        Aracul,
        Cloudwood
    }

    public enum TypePrefabFloors
    {
        Boloto,
        Chip,
        Gecsagon,
        Kamish,

        Berry,
        Mashrooms,
        Weed,
        Weedflower,

        Kishka,
        Nerv,
        Orbits,
        Shampinion,

        Corals,
        Desert,
        Diods,
        Parket,
    }

    public enum TypePrefabObjects
    {
        PrefabRock,
        PrefabVood,
        PrefabElka,
        PrefabWallRock,
        PrefabWallWood,

        Kolba,
        Lantern,

        Bananas,
        Cluben,
        Chpok,
        Pandora,

        Nadmozg,
        Triffid,

        Aracul,
        Cloudwood
    }

    public enum TypePrefabNPC
    {
        PrefabUfo,
        PrefabBoss,

        Inspector,
        Machinetool,
        Mecha,

        Dendroid,
        Garry,
        Lollipop,

        Blastarr,
        Hydragon,
        Pavuk,
        Skvid,

        Fantom,
        Mask,
        Vhailor,
    }

    private void Awake()
    {
        InitPrefabCollections();
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

    public void InitPrefabCollections()
    {
        string[] floors = Enum.GetNames(typeof(TypePrefabFloors));
        NamesPrefabFloors = new List<string>();
        foreach (var item in floors)
        {
            NamesPrefabFloors.Add(item);
        }

        string[] gameObjects = Enum.GetNames(typeof(TypePrefabObjects));
        NamesPrefabObjects = new List<string>();
        foreach (var item in gameObjects)
        {
            NamesPrefabObjects.Add(item);
        }

        string[] gameNPC = Enum.GetNames(typeof(TypePrefabNPC));
        NamesPrefabNPC = new List<string>();
        foreach (var item in gameNPC)
        {
            NamesPrefabNPC.Add(item);
        }

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
        TypePrefabs prefabName = TypePrefabs.PrefabField;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        int countAll = Helper.HeightLevel * 2;
        int index =1;

        for (int y = 0; y < Helper.HeightLevel; y++)
        {
            for (int x = 0; x < Helper.WidthLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 3);

                int maxObjectInField = (intRndCount == 0) ? 1 : 0;
                string nameField = Helper.GetNameField(x, y);

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {
                    //GEN -----
                    prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
                    //if (prefabName == TypePrefabs.PrefabField)
                    //    continue;
                    //-----------

                    index++;
                    Storage.EventsUI.SetTittle = String.Format("Loading {0} %", (countAll / index).ToString());

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ModelNPC.ObjectData objDataSave = BilderGameDataObjects.BildObjectData(prefabName, true);
                    objDataSave.NameObject = nameObject;
                    //objDataSave.TagObject = prefabName.ToString();//@del
                    objDataSave.Position = pos;

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
        }

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        Storage.EventsUI.SetTittle = String.Format("World is Loaded");
    }


    //GEN -----
    private TypePrefabs GenObjectWorld()
    {
        int intGen = UnityEngine.Random.Range(1, 4);
        int intTypePrefab = 0;
        TypePrefabs prefabName = TypePrefabs.PrefabField;

        if (intGen == 1)
            prefabName = TypePrefabs.PrefabBoss;
        else
        {
            intTypePrefab = UnityEngine.Random.Range(1, 8);
            prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;
        }

        //Type prefab
        //++ Elka, WallRock, WallWood
        //int intTypePrefab = UnityEngine.Random.Range(1, 8);
        //#TT YES BOSS
        //int intTypePrefab = UnityEngine.Random.Range(1, 5);
        //#TT YES UFO
        //int intTypePrefab = UnityEngine.Random.Range(1, 4);
        //#TT NOT UFO
        //int intTypePrefab = UnityEngine.Random.Range(1, 3);

        //int rnd1 = UnityEngine.Random.Range(1, 3);
        //int rnd1 = UnityEngine.Random.Range(1, 2);
        int rnd1 = UnityEngine.Random.Range(1, 3);
        if (rnd1!=1)
        {
            prefabName = TypePrefabs.PrefabField;
        }
        //TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;


        //prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
        return prefabName;
    }

    IEnumerator CreateDataGamesObjectsExtremalWorldProgress()
    {
        int coutCreateObjects = 0;
        TypePrefabs prefabName = TypePrefabs.PrefabField;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        int countAll = Helper.HeightLevel * 2;
        int index = 1;

        for (int y = 0; y < Helper.HeightLevel; y++)
        {
            for (int x = 0; x < Helper.WidthLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 5);

                int maxObjectInField = 1;

                if (intRndCount == 0)
                    maxObjectInField = 2;
                else
                    maxObjectInField = 1;

                intRndCount = UnityEngine.Random.Range(0, 3);

                string nameField = Helper.GetNameField(x, y);

                index++;

                
                Storage.EventsUI.SetTittle = String.Format("{0} % [{1}]", (countAll / index).ToString(), index.ToString());

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {
                    //GEN -----
                    //prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
                    //if (prefabName == TypePrefabs.PrefabField)
                    //continue;
                    //-----------
                    if (i == 0)
                        prefabName = TypePrefabs.PrefabField;
                    else
                        prefabName = TypePrefabs.PrefabBoss;

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ModelNPC.ObjectData objDataSave = BilderGameDataObjects.BildObjectData(prefabName, true);
                    objDataSave.NameObject = nameObject;
                    //objDataSave.TagObject = prefabName.ToString();//@del
                    objDataSave.Position = pos;

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
            yield return null;
        }

        Storage.EventsUI.SetTittle = String.Format("World Loaded");

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        yield break;
    }

    IEnumerator CreateDataGamesObjectsExtremalTerraWorldProgress()
    {
        int coutCreateObjects = 0;
        TypePrefabs prefabName = TypePrefabs.PrefabField;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        int countAll = Helper.HeightLevel * 2;
        int index = 1;

        for (int y = 0; y < Helper.HeightLevel; y++)
        {
            for (int x = 0; x < Helper.WidthLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 5);

                int maxObjectInField = 1;

                if (intRndCount == 0)
                    maxObjectInField = 2;
                else
                    maxObjectInField = 1;

                intRndCount = UnityEngine.Random.Range(0, 3);

                string nameField = Helper.GetNameField(x, y);

                index++;


                Storage.EventsUI.SetTittle = String.Format("{0} % [{1}]", (countAll / index).ToString(), index.ToString());

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {
                    //GEN -----
                    //prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
                    //if (prefabName == TypePrefabs.PrefabField)
                    //continue;
                    //-----------
                    if (i == 0)
                        prefabName = TypePrefabs.PrefabField;
                    else if (i == 1)
                    {
                        if(intRndCount==1)
                            prefabName = TypePrefabs.PrefabVood;
                        else
                            prefabName = TypePrefabs.PrefabWallWood;
                    }

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ModelNPC.ObjectData objDataSave = BilderGameDataObjects.BildObjectData(prefabName, true);
                    objDataSave.NameObject = nameObject;
                    //objDataSave.TagObject = prefabName.ToString(); //@del
                    objDataSave.Position = pos;

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
            yield return null;
        }

        Storage.EventsUI.SetTittle = String.Format("World Loaded");

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        yield break;
    }

    public void CreateDataGamesObjectsExtremalWorld()
    {
        //Storage.GamePause = true;
        //Storage.Instance.StopGame();
        //Storage.Instance.DestroyAllGamesObjects();

        StartCoroutine(CreateDataGamesObjectsExtremalWorldProgress());
        //StartCoroutine(CreateDataGamesObjectsExtremalTerraWorldProgress());

        //Storage.GamePause = false;
    }
       
    public static ModelNPC.ObjectData FindObjectData(GameObject p_gobject)
    {
        ModelNPC.ObjectData newObject;
        string nameGameObject = p_gobject.name;
        string typePrefab = p_gobject.tag.ToString();
        string nameField = Helper.GetNameFieldByName(nameGameObject);
        newObject = GetObjectDataFromGrid(nameGameObject, nameField);
        return newObject;
    }

    public static ModelNPC.ObjectData GetObjectDataFromGrid(string nameGameObject, string nameField)
    {
        if (!Storage.IsGridDataFieldExist(nameField))
        {
            Debug.Log("################# Error FindObjectData FIELD NOT FOUND :" + nameField);
            return null;
        }
        List<ModelNPC.ObjectData> objects = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
        int index = objects.FindIndex(p => p.NameObject == nameGameObject);
        if (index == -1)
        {
            Debug.Log("################# Error FindObjectData DATA OBJECT NOT FOUND : " + nameGameObject + "   in Field: " + nameField);
            return null;
        }
        return objects[index];
    }

    public GameObject FindPrefab(string namePrefab, string nameObject)
    {
        if (PoolGameObjects.IsUsePoolObjects && !string.IsNullOrEmpty(nameObject))
        {
            //#FIX POOL
            string tagPool = GetTypePool(namePrefab);
            GameObject gobj = Storage.Pool.GetPoolGameObject(nameObject, tagPool, new Vector3(0, 0, 0));
            if(gobj==null)
                Debug.Log("############## Error on FindPrefab gobj is null");
            return gobj;
        }
        else
        {
            //#TEST #PREFABF.1
            //return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
            //#TEST #PREFABF.2
            return FindPrefabHieracly(namePrefab);
        }
    }

    private string GetTypeByName(string namePrefab)
    {
        string resType="";

        TypePrefabs prefabType = TypePrefabs.PrefabField;
        try
        {
            //fix new pool prefabs type
            bool isLegacyType = Enum.IsDefined(typeof(TypePrefabsLegacy), namePrefab);
            if (isLegacyType)
                prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), namePrefab);
            else
                prefabType = TypePrefabs.PrefabField;
        }
        catch(Exception x)
        {
            Debug.Log("######## (SaveLoadData.GetTypeByName) ERROR TYPE POOL TO TYPE PREFAB : " + x.Message);
        }

        switch (prefabType)
        {
            case TypePrefabs.PrefabRock:
                resType = PrefabRock.tag;
                break;
            case TypePrefabs.PrefabVood:
                resType = PrefabVood.tag;
                break;
            case TypePrefabs.PrefabUfo:
                resType = PrefabUfo.tag;
                break;
            case TypePrefabs.PrefabBoss:
                resType = PrefabBoss.tag;
                break;
            case TypePrefabs.PrefabWallRock:
                resType = PrefabWallRock.tag;
                break;
            case TypePrefabs.PrefabWallWood:
                resType = PrefabWallWood.tag;
                break;
            case TypePrefabs.PrefabElka:
                resType = PrefabElka.tag;
                //TypePrefabs.PrefabElka.ToString();
                break;
            case TypePrefabs.PrefabField:
                resType = PrefabField.tag;
                break;
            case TypePrefabs.PrefabPerson:
                resType = PrefabField.tag;
                break;
            case TypePrefabs.PrefabFlore:
                resType = PrefabField.tag;
                break;
            default:
                Debug.Log("!!! FindPrefabHieracly no type : " + prefabType.ToString());
                break;
        }
        return resType;
    }

    public string GetTypePool(string namePrefab)
    {
        if (PoolGameObjects.IsUseTypePoolPrefabs)
        {
            return namePrefab;
        }

        string resType = "";
        if (namePrefab == TypePrefabs.PrefabElka.ToString())
        {
            return namePrefab;
        }
        else if (namePrefab == "Field")
        {
            return TypePrefabs.PrefabField.ToString();
        }
        else
        {
            if (PoolGameObjects.IsUseTypePoolPrefabs == false)
            {
                //fix legacy code
                if (namePrefab == PoolGameObjects.TypePoolPrefabs.PoolPerson.ToString())
                {
                    string oldType = TypePrefabs.PrefabPerson.ToString();
                    namePrefab = oldType;
                }
                else if (namePrefab == PoolGameObjects.TypePoolPrefabs.PoolFlore.ToString())
                {
                    string oldType = TypePrefabs.PrefabField.ToString();
                    namePrefab = oldType;
                }
                //else if(namePrefab == PoolGameObjects.TypePoolPrefabs.PoolPerson.ToString())
                //{
                //    var t = "TEST";
                //}
                //else if (namePrefab == PoolGameObjects.TypePoolPrefabs.PoolPersonUFO.ToString())
                //{
                //    var t = "TEST";
                //}
                //else
                //{
                //    string oldType = TypePrefabs.PrefabField.ToString();
                //    namePrefab = oldType;
                //}
            }
            return GetTypeByName(namePrefab);
        }
        return resType;
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
                case TypePrefabs.PrefabWallRock:
                    resPrefab = Instantiate(PrefabWallRock);
                    break;
                case TypePrefabs.PrefabWallWood:
                    resPrefab = Instantiate(PrefabWallWood);
                    break;
                case TypePrefabs.PrefabElka:
                    resPrefab = Instantiate(PrefabElka);
                    break;
                case TypePrefabs.PrefabField:
                    resPrefab = Instantiate(PrefabField);
                    break;
                case TypePrefabs.PrefabPerson:
                    resPrefab = Instantiate(PrefabPerson);
                    //fix legacy code
                    //string oldType = TypePrefabs.PrefabPerson.ToString();
                    //resPrefab.tag = oldType;
                    break;
                case TypePrefabs.PrefabFlore:
                    resPrefab = Instantiate(PrefabFlore);
                    break;
                default:
                    Debug.Log("!!! FindPrefabHieracly no type : " + prefabType.ToString());
                    break;

            }

            if (resPrefab == null)
                Debug.Log("############## Error on FindPrefabHieracly gobj is null");

            //Debug.Log("FindPrefabHieracly: " + prefabType.ToString());
            return resPrefab;
        }
        catch (Exception x)
        {
            Debug.Log("Error FindPrefabHieracly: " + x.Message);
        }
        Debug.Log("Error FindPrefabHieracly: object is null");

        return null;
    }

    public Sprite GetSpriteBoss(int index, out string spriteName)
    {
        
        try
        {
            spriteName = TypeBoss.Instance.GetNameSpriteForIndexLevel(index);
            Sprite spriteBoss = Storage.Person.SpriteCollection[spriteName];
            
            return spriteBoss;
        }
        catch (Exception x)
        {
            Debug.Log("################# GetSpriteBoss [" + index + "] : " + x.Message);
        }
        spriteName = "error";

        return null;
    }

    //public static ModelNPC.ObjectData BildObjectData(TypePrefabs prefabType, bool isTerraGen = false)
    //{
    //    ModelNPC.ObjectData objGameBild;

    //    switch (prefabType)
    //    {
    //        case SaveLoadData.TypePrefabs.PrefabUfo:
    //            objGameBild = new ModelNPC.GameDataUfo();
    //            break;
    //        case SaveLoadData.TypePrefabs.PrefabBoss:
    //            objGameBild = new ModelNPC.GameDataBoss(); //$$
    //            break;
    //        case SaveLoadData.TypePrefabs.PrefabField:
    //            //objGameBild = new ModelNPC.TerraData(isTerraGen); //$$
    //            objGameBild = new ModelNPC.TerraData(); //$$
    //            break;
    //        case SaveLoadData.TypePrefabs.PrefabNPC:
    //            objGameBild = new ModelNPC.GameDataAlien();
    //            break;
    //        case SaveLoadData.TypePrefabs.PrefabFlore:
    //            objGameBild = new ModelNPC.ObjectData();
    //            break;
    //        default:
    //            objGameBild = new ModelNPC.ObjectData();
    //            break;
    //    }
    //    return objGameBild;
    //}


    public bool AddConstructInGridData(string nameField, DataTile itemTile, bool isClaerField)
    {
        
        PaletteMapController.SelCheckOptDel modeDelete = PaletteMapController.SelCheckOptDel.None;
        if (isClaerField)
            modeDelete = PaletteMapController.SelCheckOptDel.DelFull;
        return AddConstructInGridData(nameField, itemTile, modeDelete);
    }

    public bool AddConstructInGridData(string nameField, DataTile itemTile,
        PaletteMapController.SelCheckOptDel p_TypeModeOptStartDelete = PaletteMapController.SelCheckOptDel.None,
        PaletteMapController.SelCheckOptDel p_TypeModeOptStartCheck = PaletteMapController.SelCheckOptDel.None)
    {
        TypePrefabs prefabName = TypePrefabs.PrefabField;
        string personTextureName = "";

        if (itemTile == null)
        {
            Debug.Log("####### AddConstructInGridData  itemTile == null");
            return false;
        }

        //ArgumentException: The requested value 'SpriteBossAlien(Clone)' was not found.

        TypesStructure structType = (TypesStructure)Enum.Parse(typeof(TypesStructure), itemTile.Tag); ;

        if (structType == TypesStructure.Terra)
        {
            prefabName = TypePrefabs.PrefabField; 
        }
        if (structType == TypesStructure.Person)
        {
            personTextureName = itemTile.Name.ClearClone();
            prefabName = TypePrefabs.PrefabBoss;
        }
        if (structType == TypesStructure.Prefab)
        {
            //ArgumentException: The requested value 'SpriteBossAlien(Clone)' was not found.
            prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), itemTile.Name);
        }

        Vector2 posStruct = Helper.GetPositByField(nameField);
        int x = (int)posStruct.x;
        int y = (int)posStruct.y;
        int _y = y * (-1);
        Vector3 pos = new Vector3(x, _y, 0) * Spacing;
        pos.z = -1;
        if (prefabName == TypePrefabs.PrefabUfo)
            pos.z = -2;

        string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
        ModelNPC.ObjectData objDataSave = BilderGameDataObjects.BildObjectData(prefabName, false);
        string typePool = objDataSave.TypePoolPrefabName; //test

        objDataSave.NameObject = nameObject;
        //objDataSave.TagObject = prefabName.ToString(); //@del
        objDataSave.Position = pos;

        if (PoolGameObjects.IsUseTypePoolPrefabs)
        {
            objDataSave.ModelView = itemTile.Name;
        }
        else
        {
            if (structType == TypesStructure.Terra)
            {
                var objTerra = objDataSave as ModelNPC.TerraData;
                if (objTerra == null)
                {
                    Debug.Log("####### AddConstructInGridData: structType is TypesStructure.Terra   objDataSave Not is ModelNPC.TerraData !!!!");
                    return false;
                }
                objTerra.ModelView = itemTile.Name;
            }
            if (structType == TypesStructure.TerraPrefab)
            {
                var objTerraPrefab = objDataSave as ModelNPC.WallData;
                if (objTerraPrefab == null)
                {
                    Debug.Log("####### AddConstructInGridData: structType is TypesStructure.TerraPrefab   objDataSave Not is ModelNPC.TerraData !!!!");
                    return false;
                }
                objTerraPrefab.ModelView = itemTile.Name;
            }
        }

        if (structType == TypesStructure.Person)
        {
            var objPerson = objDataSave as ModelNPC.GameDataBoss;
            if (objPerson == null)
            {
                Debug.Log("####### AddConstructInGridData: structType is TypesStructure.Terra   objDataSave Not is ModelNPC.TerraData !!!!");
                return false;
            }
            //objPerson.Level = Storage.Instance._TypesBoss.Where(p => p.TextureMap == personType).Select(p => p.Level).FirstOrDefault(); ;
            if (!string.IsNullOrEmpty(personTextureName))
            {
                objPerson.Level = TypeBoss.Instance._TypesBoss.Where(p => p.NameTextura2D == personTextureName).Select(p => p.Level).FirstOrDefault();
                //objPerson.Init();
            }
        }

        return Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld",
            p_TypeModeOptStartDelete, p_TypeModeOptStartCheck);
    }

    public void ClearWorld(bool isReload = true)
    {
        Storage.Instance.StopGame();
        Storage.Pool.Restart();
        Storage.Instance.DestroyAllGamesObjects();
        Storage.Pool.Restart();
        Storage.Instance.GridDataG.FieldsD.Clear();
        if(isReload)
            Storage.Player.LoadPositionHero();
        //Storage.Instance.GridDataG.FieldsD.Clear();
    }

    public void GenWorld()
    {
        Storage.Instance.IsLoadingWorld = true;

        if(Storage.PaletteMap.SelectedCell == null)
        { 
            Storage.PaletteMap.GenericOnWorld(false, SaveLoadData.TypePrefabs.PrefabWallWood);
        }
        else
        {
            Storage.PaletteMap.GenericOnWorld(true); 
        }
        Storage.Instance.IsLoadingWorld = false;
    }






}





