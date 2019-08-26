using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

//using UnityEditor;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;

public class Storage : MonoBehaviour {

    private CompletePlayerController _screiptHero;
    //private CreateNPC _scriptNPC;
    private UIEvents _scriptUIEvents;

    public static string FieldKey
    {
        get { return "Field"; }
    }
    public static float ScaleWorld
    {
        get { return 2; }
    }

    public static bool GamePause
    {

        get { return (Time.timeScale == 0.0f); }
        set
        {
            if (Time.timeScale == 1.0f)
                Time.timeScale = 0.0f;
            else
                //if (Time.timeScale == 0.0f)
                //    Time.timeScale = 0.7f;
                //else
                Time.timeScale = 1.0f;
        }
    }

    public GameObject HeroObject;
    public GameObject HeroModel;
    public GameObject UIController;
    public GameObject FramePaletteMap;
    public GameObject ContentGridPaletteMap;
    private GameObject DataStorage {
        get { return gameObject; }
    }
    
    private DispatcherWorldActions m_DispatcherWorldActions;

    //_scriptEvents = UIController.GetComponent<UIEvents>();

    public ZonaFieldLook ZonaField { get; private set; }
    public ZonaRealLook ZonaReal { get; private set; }
    public List<string> KillObjectHistory = new List<string>();
    public List<GameObject> DestroyObjectList;
    public bool IsCorrectData = false;

    public string CorrectCreateName = "";
    public bool IsLoadingWorld = false;
    public bool IsLoadingWorldThread = false;
    public bool IsTartgetPositionAll = false;
    public void IsTartgetPositionAllOn()
    {
        IsTartgetPositionAll = !IsTartgetPositionAll;
    }

    public static string SetMessageAlert
    {
        set{
            EventsUI.SetMessageBox = value;
            EventsUI.SetTittle = value;
            EventsUI.ListLogAdd = value;
        }
    }

    
    private MovementCamera _MoveCamera;
    public static MovementCamera MoveCamera
    {
        get
        {
            return Instance._MoveCamera;
        }
    }

    private SceneDebuger _SceneDebuger;
    public static SceneDebuger SceneDebug 
    {
        get
        {
            return Instance._SceneDebuger;
        }
    }

    private DiskData _DiskData;
    public static DiskData Disk
    {
        get
        {
            return Instance._DiskData;
        }
    }

    private PoolGameObjects _PoolObgects;
    public static PoolGameObjects Pool
    {
        get
        {
            return Instance._PoolObgects;
        }
    }

    private DrawGeometry _DrawGeom;
    public static DrawGeometry DrawGeom
    {
        get
        {
            return Instance._DrawGeom;
        }
    }

    private SceneLighting _SceneLight;
    public static SceneLighting SceneLight
    {
        get
        {
            return Instance._SceneLight;
        }
    }

    private DataTilesManager _TilesManager;
    public static DataTilesManager TilesManager
    {
        get
        {
            return Instance._TilesManager;
        }
    }

    private PaletteMapController _PaletteMapController;
    public static PaletteMapController PaletteMap
    {
        get
        {
            return Instance._PaletteMapController;
        }
    }

    private ManagerPalette _Palette;
    public static ManagerPalette Palette
    {
        get
        {
            return Instance._Palette;
        }
    }

    private PlayerManager _PlayerManager;
    public static PlayerManager Player
    {
        get
        {
            return Instance._PlayerManager;
        }
    }

    private StorageLog _StorageLog;
    public static StorageLog Log {
        get
        {
            return Instance._StorageLog;
        }
    }
    private StorageCorrect _StorageCorrect;
    public static StorageCorrect Fix
    {
        get
        {
            return Instance._StorageCorrect;
        }
    }

    private UpdateData _UpdateData;
    public static UpdateData Data
    {
        get { return Instance._UpdateData; }
    }

    private StoragePerson _StoragePerson;
    public static StoragePerson Person
    {
        get { return Instance._StoragePerson; }
    }

    private SaveLoadData _scriptData;
    public static SaveLoadData GridData
    {
        get { return Instance._scriptData; }
    }

    public static UIEvents EventsUI
    {
        get { return Instance._scriptUIEvents; }
    }

    public static CompletePlayerController PlayerController
    {
        get
        {
            return Instance._screiptHero;
        }
    }

    private GenerateGridFields _scriptGrid;
    public static GenerateGridFields GenGrid
    {
        get
        {
            return Instance._scriptGrid;
        }
    }

    private MapWorld _scriptMapWorld;
    public static MapWorld Map
    {
        get
        {
            return Instance._scriptMapWorld;
        }
    }

    public static Storage Instance { get; private set; }

    public Dictionary<string, GameObject> Fields;

    private Dictionary<string, List<GameObject>> _GamesObjectsReal = null;
    public Dictionary<string, List<GameObject>> GamesObjectsReal
    {
        get { return _GamesObjectsReal; }
    }

    private ReaderScene m_ReaderWorld;
    public static ReaderScene ReaderWorld
    {
        get
        {
            return Instance.m_ReaderWorld;
        }
    }
    
    private ModelNPC.GridData _GridDataG = null;
    public ModelNPC.GridData GridDataG
    {
        get { return _GridDataG; }
    }

    private string _datapathVersionOptGenericWorld = null;
    public string DataPathVersion
    {
        get { return _datapathVersionOptGenericWorld; }
    }

    private string _datapathLevel = null;
    public string DataPathLevel
    {
        get { return _datapathLevel; }
    }

    private string _datapathPerson = null;
    public string DataPathPerson
    {
        get { return _datapathPerson; }
    }

    private string _datapathTiles = null;
    public string DataPathTiles
    {
        get { return _datapathTiles; }
    }


    private string _datapathUserData = null;
    public string DataPathUserData
    {
        get { return _datapathUserData; }
    }

    private string _datapatPlayerData = null;
    public string DataPathPlayer
    {
        get { return _datapatPlayerData; }
    }


    public delegate void EventID(string id);
    public event EventID OnSelectGameObjectID;

    public Camera MainCamera;

    public string SelectFieldPosHero = "";
    public string SelectFieldCursor = "";

    //public string SelectGameObjectID="?";
    private string m_SelectGameObjectID = "?";
    public string SelectGameObjectID
    {
        get { return m_SelectGameObjectID; }
        set
        {
            if (m_SelectGameObjectID != value)
            {
                if (value == null)
                {
                    Debug.Log("############### SelectGameObjectID set value 1. --- NULL");
                }

                if (OnSelectGameObjectID != null)
                    OnSelectGameObjectID(value);
                Person.SelectedID(value);//m_SelectGameObjectID
            }
            m_SelectGameObjectID = value;
            if (value == null)
            {
                Debug.Log("############### SelectGameObjectID set value 2. --- NULL");
            }
        }
    }
      
    //public static Storage Instance { get; private set; }
    public void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start() {

        _datapathLevel = Application.dataPath + "/Levels/LevelData" + Application.loadedLevel + ".xml";
        _datapathUserData = Application.dataPath + "/UserConfig/UserData.xml";
        _datapatPlayerData = Application.dataPath + "/Player/PlayerData.xml";
        _datapathTiles = Application.dataPath + "/Levels/TilesData.xml";
        _datapathVersionOptGenericWorld = Application.dataPath + "/UserConfig/VersionsOptGeneric.xml";

        ZonaField = null;
        ZonaReal = null;

        InitObjectsGrid();

        InitComponents();

        //StartGenGrigField();

        LoadData();

        LoadGameObjects();

        LoadDefaultUI();

        Player.LoadPositionHero();
    }

    void Update()
    {
        DestroyRealObjectInList();
    }

    private void InitComponents()
    {
        Debug.Log("..................InitComponents");

        //var camera = MainCamera;
        if (MainCamera == null)
        {
            Debug.Log("MainCamera null");
            return;
        }

        _scriptData = MainCamera.GetComponent<SaveLoadData>();
        if (_scriptData == null)
        {
            Debug.Log("Storage.Start : sctiptData not load !!!");
            return;
        }

        _scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
        if (_scriptGrid == null)
        {
            Debug.Log("Storage.Start : scriptGrid not load !!!");
            return;
        }

        m_DispatcherWorldActions = MainCamera.GetComponent<DispatcherWorldActions>();
        if (m_DispatcherWorldActions == null)
        {
            Debug.Log("Storage.Start : DispatcherWorldActions not load !!!");
            return;
        }
        

        //_scriptNPC = MainCamera.GetComponent<CreateNPC>();
        //if (_scriptNPC == null)
        //{
        //    Debug.Log("Storage.Start scriptNPC not load !!!!!");
        //    return;
        //}
        _scriptMapWorld = MainCamera.GetComponent<MapWorld>();
        if (_scriptMapWorld == null)
        {
            Debug.Log("Storage.Start scriptMapWorld not load !!!!!");
            return;
        }
        _screiptHero = HeroObject.GetComponent<CompletePlayerController>();
        if (_screiptHero == null)
        {
            Debug.Log("Storage.Start scriptHero not load !!!!!");
            return;
        }
        if (UIController == null)
        {
            Debug.Log("Storage.Start UIController not setting !!!!!");
            return;
        }
        _scriptUIEvents = UIController.GetComponent<UIEvents>();
        if (_scriptUIEvents == null)
        {
            Debug.Log("Storage.Start scriptUIEvents not load !!!!!");
            return;
        }

        //Reinit Component
        _StorageCorrect = DataStorage.GetComponent<StorageCorrect>();
        if (_StorageCorrect != null)
        {
            //Debug.Log("InitObjectsGrid Destroy(_StorageCorrect) __________________________");
            Destroy(_StorageCorrect);
        }
        _StorageCorrect = DataStorage.AddComponent<StorageCorrect>();

        //Reinit Component
        _StoragePerson = MainCamera.GetComponent<StoragePerson>();
        _StoragePerson.PersonsDataInit();

        _Palette = DataStorage.GetComponent<ManagerPalette>();
        if (_Palette == null)
        {
            Debug.Log("########## InitComponents _Palette is Empty");
            return;
        }
        _Palette.LoadSpritePrefabs();

        _PlayerManager = GetComponent<PlayerManager>();
        if (_PlayerManager==null)
        {
            Debug.Log("########## InitComponents PlayerManager is Empty");
            return;
        }
        _PlayerManager.Init();

        _TilesManager = DataStorage.GetComponent<DataTilesManager>();
        if (_TilesManager == null)
        {
            Debug.Log("########## InitComponents _TilesManager is Empty");
            return;
        }

        _PaletteMapController = ContentGridPaletteMap.GetComponent<PaletteMapController>();
        if (_PaletteMapController == null)
        {
            Debug.Log("########## InitComponents _PaletteMapController is Empty");
            return;
        }
        _PaletteMapController.Init();

        _DrawGeom = GetComponent<DrawGeometry>();
        if (_DrawGeom == null)
        {
            Debug.Log("########## InitComponents _DrawGeom is Empty");
            return;
        }
        _SceneLight = GetComponent<SceneLighting>();
        if (_SceneLight == null)
        {
            Debug.Log("########## InitComponents _SceneLight is Empty");
            return;
        }

        _PoolObgects = new PoolGameObjects();

        _DiskData = GetComponent<DiskData>();
        if (_DiskData == null)
        {
            Debug.Log("########## InitComponents _DiskData is Empty");
            return;
        }

        _SceneDebuger = UIController.GetComponent<SceneDebuger>();
        if (_SceneDebuger == null)
        {
            Debug.Log("########## InitComponents _SceneDebuger is Empty");
            return;
        }
        _MoveCamera = UIController.GetComponent<MovementCamera>();
        if (_MoveCamera == null)
        {
            Debug.Log("########## InitComponents _MoveCamera is Empty");
            return;
        }
        

        //_PoolObgects
        //DrawGeometry 
    }

    private void InitObjectsGrid()
    {
        //Debug.Log("III InitObjectsGrid_______________");
        Fields = new Dictionary<string, GameObject>();
        _GamesObjectsReal = new Dictionary<string, List<GameObject>>();

        _StorageLog = new StorageLog();
        _StorageLog.Init();
        _UpdateData = new UpdateData();

        DestroyObjectList = new List<GameObject>();
    }

    public void LoadLevels()
    {
        IsLoadingWorld = true;

        Debug.Log("III LoadLevels ::::_______________");

        StopGame();

        LoadData();

        LoadGameObjects(true);

        IsLoadingWorld = false;

    }

    public void StopGame()
    {
        //if (_scriptNPC != null)
        //    _scriptNPC.StopCrateNPC();

        //---
        DestroyAllGamesObjects();

        //@#FIX 
        Pool.Restart();

        InitObjectsGrid();

        if (ReaderWorld != null)
        {
            ReaderWorld.Clear();
            m_DispatcherWorldActions.ResetDispatcher();
        }
    }

    public void CreateWorld(bool isGenNewWorld = false)
    {
        StopGame();
        LoadGameObjects(true, true, isGenNewWorld);
    }

    public void LoadGameObjects(bool isLoadRealtime = false, bool isCreate = false, bool isGenNewWorld = false)
    {
        Debug.Log("III LoadGameObjects ::::_______________");

        //--------------------- NEW -------------------
        _screiptHero.FindFieldCurrent(false);
        _scriptGrid.StartGenGrigField(true);
        _screiptHero.FindFieldCurrent();

        //------------------------------

        //Debug.Log("III CreateDataGamesObjectsWorld_______________");
        if (isGenNewWorld)
        {
            _scriptData.GenericWorld();
        }
        else
        {
            _scriptData.CreateDataGamesObjectsWorld(isCreate);
        }

        if (isLoadRealtime)
            StartCoroutine(StartFindLookObjects());
        else
        {
            //Debug.Log("III ....Init LoadObjectsNearHero ......");
            _scriptGrid.LoadObjectsNearHero();
            //Debug.Log("III ....Sart Crate NPC......");
            //_scriptNPC.SartCrateNPC();
        }

        Map.RefreshFull();

        if (ReaderWorld != null)
        {
            ReaderWorld.Clear();
            ReaderWorld.InitCollectionID();
            m_DispatcherWorldActions.ResetDispatcher();
        }
    }

    private void LoadDefaultUI()
    {
        PaletteMap.Show();
        SceneLight.UpadteGameGraphSetting();
    }

    IEnumerator StartFindLookObjects()
    {
        //Debug.Log("III ....Init StartFindLookObjects ::: ......");

        bool isLoadedObjects = false;

        while (!isLoadedObjects)
        {
            yield return null;

            //Debug.Log("III ....Init LoadObjectsNearHero   start ......");

            yield return new WaitForSeconds(0.1f);

            //Debug.Log("III ....Init LoadObjectsNearHero ......");
            _scriptGrid.LoadObjectsNearHero();

            isLoadedObjects = true;

            yield return new WaitForSeconds(0.1f);
        }

        //Debug.Log("III ....Sart Crate NPC......");
        //_scriptNPC.SartCrateNPC();
    }
       

    public void LoadData()
    {

        Storage.EventsUI.SetTittle = "##### LoadPathData";
        //string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
        _datapathLevel = Application.dataPath + "/Levels/LevelDataPart1x1.xml";
        if (File.Exists(_datapathLevel))
        {
            Storage.Instance.IsLoadingWorldThread = true;

            _GridDataG = Serializator.LoadGridXml(_datapathLevel);
            //--load parts 
            //StartCoroutine(StartLoadDataPartsXML());
            //--load cash

            //StartCoroutine(StartLoadDataBigXML());

            //-- load old style
            //StartCoroutine(StartInGameLoadDataBigXML());
            _DiskData.LoadDataBigXML(); //<<<

            //-- Load Async
            //StartCoroutine(StartBackgroundLoadDataBigXML());

            //-- Load Async
            //StartCoroutine(StartThreadLoadDataBigXML());
            //GridData.LoadDataBigThreadXML();
        }
        else
        {
            Debug.Log("# LoadPathData not exist: " + _datapathLevel);
            Storage.EventsUI.ListLogAdd = "##### LoadPathData";
            Storage.EventsUI.ListLogAdd = "##### LoadPathData not exist: " + _datapathLevel;
            Storage.EventsUI.ListLogAdd = "##### LoadPathData";
        }

        _datapathPerson = Application.dataPath + "/Levels/PersonData" + Application.loadedLevel + ".xml";
        if (File.Exists(_datapathPerson))
        {
            var _personsData = Serializator.LoadPersonXml(_datapathPerson);
            _StoragePerson.PersonsDataInit(_personsData);
        }
        else
        {
            Debug.Log("# LoadPathData not exist: " + _datapathPerson);
        }
    }

    public bool ReaderSceneIsValid
    {
        get
        {
            return m_ReaderWorld != null
                && m_ReaderWorld.IsLoaded
                && !IsLoadingWorld
                && !GamePause
                && ReaderWorld.CollectionInfoID != null; 
                //&& Storage.ReaderWorld.CollectionInfoID.Count > 0;
        }
    }

    public void InitCollectionID()
    {
        m_ReaderWorld = new ReaderScene();
    }

    public void ReloadWorld()
    {
        StartCoroutine(StartReloadWorld());
    }

    IEnumerator StartReloadWorld()
    {
        bool isSave = false;
        while (!isSave)
        {
            yield return null;

            yield return new WaitForSeconds(0.3f);

            EventsUI.SetTittle = "Level saving...";
            Storage.Disk.SaveLevel();

            yield return new WaitForSeconds(0.3f);

            isSave = true;
        }
        EventsUI.SetTittle = "Level loading...";
        LoadLevels();
        EventsUI.SetTittle = "Level loaded.";
    }

    public void SetZonaField(int X, int Y, int X2, int Y2)
    {
        ZonaField = new ZonaFieldLook()
        {
            X = X,
            Y = Y,
            X2 = X2,
            Y2 = Y2
        };
    }

    public void SetZonaRealLook(float X, float Y, float X2, float Y2)
    {
        ZonaReal = new ZonaRealLook()
        {
            X = X,
            Y = Y,
            X2 = X2,
            Y2 = Y2
        };
    }
 
    public bool TestExistField(string field)
    {
        if (!_GridDataG.FieldsD.ContainsKey(field))
        {
            Debug.Log("********** TestExitField      GridData not found Field = " + field);
            return false;
        }
        return true;
    }

    public void ClearGridData()
    {
        _GridDataG = new ModelNPC.GridData();
    }

    #region Destroy

    public void DestroyObject(GameObject gobj)
    {
        //if(gobj!=null)
        //    Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%  DestroyObject " + gobj.name);
        if (PoolGameObjects.IsUsePoolObjects) {
            Storage.Pool.DestroyPoolGameObject(gobj);
        }
        else {
            Destroy(gobj);
        }
    }

    public void DestroyAllGamesObjects()
    {
        foreach (var item in _GamesObjectsReal)
        {
            string nameField = item.Key;
            List<GameObject> resListData = _GamesObjectsReal[nameField];

          
            if (resListData != null)
            {
                for (int i = 0; i < resListData.Count(); i++)
                {
                    GameObject objDel = resListData[i];
                    //if (objDel != null)
                    //    Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%  DestroyAllGamesObjects " + objDel.name);
                    if (PoolGameObjects.IsUsePoolObjects)
                    {
                        Storage.Pool.DestroyPoolGameObject(objDel);
                    }
                    else
                    {
                        Destroy(objDel);
                    }
                }
            }
        }

        foreach (var item in Fields)
        {
            string nameField2 = item.Key;
            GameObject resField = Fields[nameField2];

            if (Storage.Map.IsGridMap)
                Storage.Map.CheckSector(nameField2);

            if (resField != null)
            {
                //Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%  DestroyAllGamesObjects " + resField.name);
                if (PoolGameObjects.IsUsePoolObjects)
                {
                    Storage.Pool.DestroyPoolGameObject(resField);
                }
                else
                {
                    Destroy(resField);
                }
            }
        }
    }

    public void AddDestroyGameObject(GameObject gObj)
    {
        //if(PoolGameObjects.IsUsePoolObjects)
        //{
        //    var evObj = gObj.GetComponent<EventsObject>();
        //    if (evObj != null)
        //        evObj.PoolCase.IsDesrtoy = true;
        //}

        DestroyObjectList.Add(gObj);
    }

    private void DestroyRealObjectInList()
    {
        if (DestroyObjectList.Count == 0)
            return;

        for (int i = DestroyObjectList.Count - 1; i >= 0; i--)
        {
            GameObject gObj = DestroyObjectList[i];
            if (gObj != null)
            {
                //Debug.Log("*** POOL DESTROY GAME OBJECt INCORRECT : " + gObj.name);
                DestroyFullObject(gObj);
                DestroyObjectList.RemoveAt(i);
            }
        }
    }

    //@DESTROY@
    public bool DestroyFullObject(GameObject gObj, bool isCorrect = false)
    {
        if (gObj == null)
        {
            Debug.Log("+++ DestroyFullObject ++++ object is null");
            return false;
        }

        //if (isCorrect)
        //    Debug.Log("++++++++++++ DestroyFullObject ++++ : " + gObj);

        string setName = gObj.name;

        string nameField = Helper.GetNameFieldByName(setName);
        if (nameField == null)
            return false;

        //#FIX
        if(!_GamesObjectsReal.ContainsKey(nameField))
        {
            Debug.Log("####### DestroyFullObject not field : " + nameField);
            return false;
        }

        List<GameObject> listObjInField = _GamesObjectsReal[nameField];

        for (int i = listObjInField.Count - 1; i >= 0; i--)
        {
            if (listObjInField[i] == null)
            {
                _UpdateData.RemoveRealObject(i, nameField, "DestroyRealObject");
            }
        }
        if (listObjInField.Count > 0)
        {
            int indRealData = listObjInField.FindIndex(p => p.name == setName);
            if (indRealData == -1)
            {
                Debug.Log("+++ ------  DestroyFullObject: ------  Hero destroy >>> Not find GamesObjectsReal : " + gObj.name);
            }
            else
            {
                _UpdateData.RemoveRealObject(indRealData, nameField, "DestroyRealObject");
            }
        }

        bool isDestroyOnBild = false;
        if (gObj != null)
        {
            //if (gObj != null)
            //    Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%  DestroyFullObject " + gObj.name);
            if (PoolGameObjects.IsUsePoolObjects)
            {
                var evObj = gObj.GetComponent<EventsObject>();
                if (evObj != null && evObj.PoolCase.IsDesrtoy) //Replace Data on Construct when Paint Prafab
                {
                    isDestroyOnBild = true;
                }
            }
            //@@@-
            //Destroy(gObj);
            //@@@+
            gObj.GetEvent().PoolCase.Deactivate();// .IsDesrtoy = true;
        }
        else
        {
            Debug.Log("+++ DestroyFullObject ++++ Destroy ---- object is null");
        }

        KillObjectHistory.Add(setName); //++ history
        //-----------------------------------------------
        bool isRemovedCorrect = false;
        bool isRemReal = false;
        bool isRemData = false;
        if (isCorrect)
        {
            isRemReal = _UpdateData.RemoveAllFindRealObject(setName);
        }

        //Destroy to Data
        if (false == ReaderScene.IsGridDataFieldExist(nameField))
        {
            Debug.Log("+++++ ------- DestroyRealObject ----- !GridData.FieldsD not field=" + nameField);
            return false;
        }
        List<ModelNPC.ObjectData> dataObjects = ReaderScene.GetObjectsDataFromGrid(nameField);
        int indObj = dataObjects.FindIndex(p => p.NameObject == gObj.name);
        if (!isCorrect)
        {
            if (indObj == -1)
            {
                if(!isDestroyOnBild)
                { 
                    Debug.Log("!!!! ObjectData GridData not object=" + gObj.name);
                    //RemoveAllFindRealObject(gObj.name);
                    _UpdateData.RemoveAllFindDataObject(gObj.name);
                }
            }
            else
            {
                //@DD@ dataObjects.RemoveAt(indObj);
                _UpdateData.RemoveDataObjectInGrid(nameField, indObj, "DestroyRealObject");
            }
        }
        else
        {
            if (indObj != -1)
            {
                //@DD@ dataObjects.RemoveAt(indObj);
                _UpdateData.RemoveDataObjectInGrid(nameField, indObj, "DestroyRealObject");
            }
            isRemData = _UpdateData.RemoveAllFindDataObject(setName);
        }

        if (isRemData || isRemReal)
            isRemovedCorrect = true;

        return isRemovedCorrect;
    }

    #endregion

    #region class

    public class ZonaFieldLook
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public ZonaFieldLook() { }
    }

    public class ZonaRealLook
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public ZonaRealLook() { }
    }

    #endregion
    
}
