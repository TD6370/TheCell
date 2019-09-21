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
    public GameObject PrefabWood;
    public GameObject PrefabFloor;

    public GameObject PrefabPortal;

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

    // TypePrefabs TypeInventoryObjects TypesBiomGray TypesBiomViolet TypesBiomGreen TypesBiomRed TypesBiomBlue TypePrefabNPC TypePrefabWall TypePrefabObjects TypePrefabFloors TypePrefabFlore
    public List<string> NamesPrefabObjects;
    public List<string> NamesPrefabFloors;
    public List<string> NamesPrefabFlore;
    public List<string> NamesPrefabsObjectsAndFlore;
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

    private List<TypePrefabs> m_collectionFloorGray;
    public List<TypePrefabs> GetFloorGray
    {
        get
        {
            if (m_collectionFloorGray == null)
            {
                m_collectionFloorGray = new List<TypePrefabs>();
                var list = new List<string>();
                bool isGray;
                bool isPrefab;
                //foreach (var typeFloor in Enum.GetValues(typeof(SaveLoadData.TypePrefabFloors)))
                foreach (var typeFloor in NamesPrefabFloors)
                {
                    isGray = Enum.IsDefined(typeof(TypesBiomGray), typeFloor);
                    isPrefab = Enum.IsDefined(typeof(TypePrefabs), typeFloor);
                    if(isPrefab && isGray)
                    {
                        TypePrefabs typePrefab = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), typeFloor.ToString());
                        m_collectionFloorGray.Add(typePrefab);
                    }
                }
            }
            return m_collectionFloorGray;
        }
    }

    private List<TypePrefabs> m_collectionPrefabObjectsAndFloreGray;
    public List<TypePrefabs> GetPrefabsObjectsAndFloreGray
    {
        get
        {
            if (m_collectionPrefabObjectsAndFloreGray == null)
            {
                m_collectionPrefabObjectsAndFloreGray = new List<TypePrefabs>();

                var list = new List<string>();
                bool isGray;
                bool isPrefab;
                foreach (var typePrefabNext in NamesPrefabsObjectsAndFlore)
                {
                    isGray = Enum.IsDefined(typeof(TypesBiomGray), typePrefabNext);
                    isPrefab = Enum.IsDefined(typeof(TypePrefabs), typePrefabNext);
                    if (isPrefab && isGray)
                    {
                        TypePrefabs typePrefab = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), typePrefabNext);
                        m_collectionPrefabObjectsAndFloreGray.Add(typePrefab);
                    }
                }
            }
            return m_collectionPrefabObjectsAndFloreGray;
        }
    }

    private List<TypePrefabs> m_collectionFloreGray;
    public List<TypePrefabs> GetFloreGray
    {
        get
        {
            if (m_collectionFloreGray == null)
            {
                m_collectionFloreGray = new List<TypePrefabs>();

                var list = new List<string>();
                bool isGray;
                bool isPrefab;
                foreach (var typeFloor in NamesPrefabFlore)
                {
                    isGray = Enum.IsDefined(typeof(TypesBiomGray), typeFloor);
                    isPrefab = Enum.IsDefined(typeof(TypePrefabs), typeFloor);
                    if (isPrefab && isGray)
                    {
                        TypePrefabs typePrefab = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), typeFloor.ToString());
                        m_collectionFloreGray.Add(typePrefab);
                    }
                }
                m_collectionFloreGray.Add(TypePrefabs.BlueBerry);

            }
            return m_collectionFloreGray;
        }
    }

    private List<TypePrefabs> m_collectionTreeGray;
    public List<TypePrefabs> GetTreeGray
    {
        get
        {
            if (m_collectionTreeGray == null)
            {
                m_collectionTreeGray = new List<TypePrefabs>()
                {
                    TypePrefabs.Iva,
                    TypePrefabs.Klen,
                    TypePrefabs.Sosna
                };
            }
            return m_collectionTreeGray;
        }
    }

    private List<TypePrefabs> m_collectionRockGray;
    public List<TypePrefabs> GetRockGray
    {
        get
        {
            if (m_collectionRockGray == null)
            {
                m_collectionRockGray = new List<TypePrefabs>() {
                    TypePrefabs.RockDark,
                    TypePrefabs.RockBrown,
                    TypePrefabs.RockValun
                };
            }
            return m_collectionRockGray;
        }
    }
        
    private List<TypePrefabs> m_collectionGrassGray;
    public List<TypePrefabs> GetGrassGray
    {
        get
        {
            if (m_collectionGrassGray == null)
            {
                m_collectionGrassGray = new List<TypePrefabs>() {
                    TypePrefabs.Grass,
                    TypePrefabs.GrassMedium,
                    TypePrefabs.GrassSmall
                };
            }
            return m_collectionGrassGray;
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

        PrefabPortal,
        PortalRed,
        PortalBlue,
        PortalGreen,
        PortalViolet,
        //-------------- Floor and Flore

        Grass,
        GrassMedium,
        GrassSmall,
        Ground,
        Ground02,
        Ground03,
        Ground04,
        Ground05,

        Iris,
        Osoka,
        Tussock,

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

        Ej,

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

        Sosna,
        Klen,
        Iva,
        BlueBerry,
        RockBrown,
        RockValun,
        RockDark,

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
    
    public enum TypePrefabFlore
    {
        Swamp,
        Kamish,

        Berry,
        Mashrooms,
        Weedflower,

        Orbits,
        Shampinion,

        Corals,
        Diods,

        Iris,
        Osoka,
        Tussock
    }

    //public enum TypePrefabFloors
    //{
    //    Boloto,
    //    Chip,
    //    Gecsagon,
    //    Kamish,

    //    Berry,
    //    Mashrooms,
    //    Weed,
    //    Weedflower,

    //    Kishka,
    //    Nerv,
    //    Orbits,
    //    Shampinion,

    //    Corals,
    //    Desert,
    //    Diods,
    //    Parket,
    //}
    
    public enum TypePrefabFloors
    {
        Chip,
        Gecsagon,

        Weed,

        Kishka,
        Nerv,

        Desert,
        Parket,

        Ground05,
        Ground04,
        Ground03,
        Ground02,
        Ground,
        GrassSmall,
        GrassMedium,
        Grass
    }
    
    public enum TypePrefabObjects
    {
        PrefabRock,
        PrefabVood,
        PrefabElka,

        Kolba,
        Lantern,

        Bananas,
        Cluben,
        Chpok,
        Pandora,

        Nadmozg,
        Triffid,

        Aracul,
        Cloudwood,

        RockDark,
        RockValun,
        RockBrown,
          Klen,
          Iva,
          Sosna,
             BlueBerry
    }

    //public enum TypePrefabObjects
    //{
    //    PrefabRock,
    //    PrefabVood,
    //    PrefabElka,
    //    PrefabWallRock,
    //    PrefabWallWood,

    //    Kolba,
    //    Lantern,

    //    Bananas,
    //    Cluben,
    //    Chpok,
    //    Pandora,

    //    Nadmozg,
    //    Triffid,

    //    Aracul,
    //    Cloudwood
    //}
    
    public enum TypePrefabWall
    {
        PrefabWallRock,
        PrefabWallWood,
    }
    
    public enum TypePrefabNPC
    {
        PrefabUfo,
        PrefabBoss,

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

        Ej
    }
    
    public enum TypesBiomBlue
    {
        //----- NPC
        Inspector,
        Machinetool,
        Mecha,

        //--------- Wall


        //--------- Wood
        Kolba,
        Lantern,

        //--------- Flore
        Kamish,
        Chip,

        //--------- Floor
        Swamp,
        Gecsagon,
    }
    
    public enum TypesBiomRed
    {
        //----- NPC
        Blastarr,
        Hydragon,
        Pavuk,
        Skvid,

        //--------- Wall

        //--------- Wood
        Nadmozg,
        Triffid,

        //--------- Flore
        Orbits,
        Shampinion,

        //--------- Floor
        Kishka,
        Nerv,
        
    }
    
    public enum TypesBiomGreen
    {
        //----- NPC
        Dendroid,
        Gary,
        Lollipop,

        //--------- Wall

        //--------- Wood
        Bananas,
        Cluben,

        //--------- Flore
        Berry,
        Mashrooms,
        Weedflower,

        //--------- Floor
        Weed,
    }
    
    public enum TypesBiomViolet
    {
        //----- NPC
        Fantom,
        Mask,
        Vhailor,

        //--------- Wall

        //--------- Wood
        Aracul,
        Chpok,
        Pandora,
        Cloudwood,

        //--------- Flore
        Corals,
        Diods,

        //--------- Floor
        Desert,
        Parket,
    }
    
    public enum TypesBiomGray
    {
        //----- NPC
        Ej,
        //--------- Wall

        //--------- Wood
        Ground05,
        Ground04,
        Ground03,
        Ground02,
        Ground,
        GrassSmall,
        GrassMedium,
        Grass,

        //--------- Flore
        Iris,
        Osoka,
        Tussock,

        //--------- Floor
        RockDark,
        RockValun,
        RockBrown,
        Klen,
        Iva,
        Sosna, 
        BlueBerry
    }
    //

    public enum TypeInventoryObjects
    {
        PrefabField,
        PrefabRock,
        PrefabVood,
        PrefabUfo,
        PrefabBoss,
        PrefabElka,
        PrefabWallRock,
        PrefabWallWood,

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
        Cloudwood,

        //--------Gray
        WoodBranchInv,
        BlueBerryInv,
        GrassInv,
    }

    private void Awake()
    {
        InitPrefabCollections();
        InitCacheDataEnum();
    }

    public int TypePrefabsCount = 0;
    public int TypeInventoryObjectsCount = 0;
    public int TypesBiomGrayCount = 0;
    public int TypesBiomVioletCount = 0;
    public int TypesBiomGreenCount = 0;
    public int TypesBiomRedCount = 0;
    public int TypesBiomBlueCount = 0;
    public int TypePrefabNPCCount = 0;
    public int TypePrefabWallCount = 0;
    public int TypePrefabObjectsCount = 0;
    public int TypePrefabFloorsCount = 0;
    public int TypePrefabFloreCount = 0;
    public int TypeFloorGrayCount = 0;
    public int TypeTreeGrayCount = 0;
    public int TypeRockGrayCount = 0;
    public int TypeFloreGrayCount = 0;
    public int TypeGrassGrayCount = 0;

    private void InitCacheDataEnum()
    {
        // TypePrefabs TypeInventoryObjects TypesBiomGray TypesBiomViolet TypesBiomGreen TypesBiomRed TypesBiomBlue TypePrefabNPC TypePrefabWall TypePrefabObjects TypePrefabFloors TypePrefabFlore
        TypePrefabsCount = Enum.GetValues(typeof(TypePrefabs)).Length - 1;
        TypeInventoryObjectsCount = Enum.GetValues(typeof(TypeInventoryObjects)).Length - 1;
        TypesBiomGrayCount = Enum.GetValues(typeof(TypesBiomGray)).Length - 1;
        TypesBiomVioletCount = Enum.GetValues(typeof(TypesBiomViolet)).Length - 1;
        TypesBiomGreenCount = Enum.GetValues(typeof(TypesBiomGreen)).Length - 1;
        TypesBiomRedCount = Enum.GetValues(typeof(TypesBiomRed)).Length - 1;
        TypesBiomBlueCount = Enum.GetValues(typeof(TypesBiomBlue)).Length - 1;
        TypePrefabNPCCount = Enum.GetValues(typeof(TypePrefabNPC)).Length - 1;
        TypePrefabWallCount = Enum.GetValues(typeof(TypePrefabWall)).Length - 1;
        TypePrefabObjectsCount = Enum.GetValues(typeof(TypePrefabObjects)).Length - 1;
        TypePrefabFloorsCount = Enum.GetValues(typeof(TypePrefabFloors)).Length - 1;
        TypePrefabFloreCount = Enum.GetValues(typeof(TypePrefabFlore)).Length - 1;
        TypeFloorGrayCount = GetFloorGray.Count();
        TypeTreeGrayCount = GetTreeGray.Count();
        TypeRockGrayCount = GetRockGray.Count();
        TypeFloreGrayCount = GetFloreGray.Count();
        TypeGrassGrayCount = GetGrassGray.Count();
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

        NamesPrefabsObjectsAndFlore = new List<string>();

        string[] floors = Enum.GetNames(typeof(TypePrefabFloors));
        NamesPrefabFloors = new List<string>();
        foreach (var item in floors)
        {
            NamesPrefabFloors.Add(item);
        }

        string[] flores = Enum.GetNames(typeof(TypePrefabFlore));
        NamesPrefabFlore = new List<string>();
        foreach (var item in flores)
        {
            NamesPrefabFlore.Add(item);
            NamesPrefabsObjectsAndFlore.Add(item);
        }
        
        string[] gameObjects = Enum.GetNames(typeof(TypePrefabObjects));
        NamesPrefabObjects = new List<string>();
        foreach (var item in gameObjects)
        {
            NamesPrefabObjects.Add(item);
            NamesPrefabsObjectsAndFlore.Add(item);
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

        if (Storage.Instance.GridDataG == null)
        {
            Storage.EventsUI.SetTittle = "CreateDataGamesObjectsWorld GridDataG is Empty";
            Storage.EventsUI.ListLogAdd = "CreateDataGamesObjectsWorld GridDataG is Empty";
            Storage.EventsUI.ListLogAdd = "CreateDataGamesObjectsWorld GridDataG is Empty";
        }

        //test
        //Storage.GenWorld.GenericWorldLegacy();
        //Storage.GenWorld.GenericWorldPriorityTerra(50, 2, 20, isGenNPC: true);
        int distantion = 20;
        Storage.GenWorld.GenericWorldPriorityTerra(30, distantion, 20, isStartWorld: true);
    }
       
    public static ModelNPC.ObjectData GetObjectDataByGobj(GameObject p_gobject)
    {
        ModelNPC.ObjectData newObject;
        string nameGameObject = p_gobject.name;
        //string typePrefab = p_gobject.tag.ToString();
        string nameField = Helper.GetNameFieldByName(nameGameObject);
        newObject = GetObjectDataFromGrid(nameGameObject, nameField);
        return newObject;
    }

   

    public static ModelNPC.ObjectData GetObjectDataFromGrid(string nameGameObject, string nameField)
    {
        if (!ReaderScene.IsGridDataFieldExist(nameField))
        {
            Debug.Log("################# Error FindObjectData FIELD NOT FOUND :" + nameField + "   find object: " + nameGameObject);
            //if (!Storage.Instance.IsLoadingWorldThread)
            //{
                //Storage.Data.AddNewFieldInGrid(nameField, "GetObjecsDataFromGrid", true);
            //}
            //else
            //{
            //    Debug.Log("################# Error FindObjectData FIELD NOT FOUND :" + nameField + "   find object: " + nameGameObject);
            return null;
            //    //return new ModelNPC.ObjectData();
            //}
        }
        List<ModelNPC.ObjectData> objects = ReaderScene.GetObjectsDataFromGrid(nameField);
        int index = objects.FindIndex(p => p.NameObject == nameGameObject);
        if (index == -1)
        {
            Debug.Log("################# Error FindObjectData DATA OBJECT NOT FOUND : " + nameGameObject + "   in Field: " + nameField);
            return null;
        }
        return objects[index];
    }

    private string GetTypeByName(string namePrefab)
    {
        if (PoolGameObjects.IsUseTypePoolPrefabs)
        {
            Debug.Log("############### IsUseTypePoolPrefabs - LEGACY CODE  - GetTypeByName");
            return "";
        }

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
        //if (PoolGameObjects.IsUseTypePoolPrefabs)
        //{
            return namePrefab;
        //}
    }

    public static GameObject CopyGameObject(GameObject defObj)
    {
        //GameObjectNext.transform.SetParent(Storage.GenGrid.PanelPool.transform);
        //return Instantiate(defObj);
        return Instantiate(defObj, parent: Storage.GenGrid.PanelPool.transform); //SPEEDFIX
    }
  
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
        ModelNPC.ObjectData objDataSave = BilderGameDataObjects.BildObjectData(prefabName);
        objDataSave.CreateID(nameObject);
        string typePool = objDataSave.TypePoolPrefabName; //test

        objDataSave.SetNameObject(nameObject);
        //objDataSave.TagObject = prefabName.ToString(); //@del
        //objDataSave.Position = pos;
        objDataSave.SetPosition(pos);//###ERR

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
                //objPerson.Level = TypeBoss.Instance._TypesBoss.Where(p => p.NameTextura2D == personTextureName).Select(p => p.Level).FirstOrDefault();
                //objPerson.Init();
            }
        }

        return Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld",
            p_TypeModeOptStartDelete, p_TypeModeOptStartCheck);
    }

    public void ClearWorld(bool isSync = false)
    {
        if (isSync)
        {
            Storage.Instance.GridDataG.FieldsD.Clear();

            //Storage.Player.LoadPositionHero(); //<<-- in Storage.Instance.StopGame();

            Storage.Instance.StopGame();
            Storage.Instance.LoadGameObjects(true); //NEW
        }
        else
        {
            StartCoroutine(StartClearWorld());
        }
    }

    IEnumerator StartClearWorld()
    {
        while (true)
        {
            Storage.Instance.GridDataG.FieldsD.Clear();
            yield return null;
            Storage.Instance.StopDispatcherAction();
            yield return null;
            Storage.Instance.StopGame();
            yield return null;
            Storage.Instance.LoadGameObjects(true); 
            //Storage.Player.LoadPositionHero();
            yield break;
        }
    }




}





