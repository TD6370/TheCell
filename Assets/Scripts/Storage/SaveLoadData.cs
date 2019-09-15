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

        if (Storage.Instance.GridDataG == null)
        {
            Storage.EventsUI.SetTittle = "CreateDataGamesObjectsWorld GridDataG is Empty";
            Storage.EventsUI.ListLogAdd = "CreateDataGamesObjectsWorld GridDataG is Empty";
            Storage.EventsUI.ListLogAdd = "CreateDataGamesObjectsWorld GridDataG is Empty";
        }

        //test
        Storage.GenWorld.GenericWorldLegacy();
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
        ModelNPC.ObjectData objDataSave = BilderGameDataObjects.BildObjectData(prefabName, false);
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

   






}





