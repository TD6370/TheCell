using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

using UnityEditor;


public class Storage : MonoBehaviour {


    

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
    public GameObject UIController;
    //_scriptEvents = UIController.GetComponent<UIEvents>();

    public ZonaFieldLook ZonaField { get; set; }
    public ZonaRealLook ZonaReal { get; set; }
    public List<string> KillObject = new List<string>();
    public List<GameObject> DestroyObjectList;
    public bool IsCorrectData = false;
    public string CorrectCreateName = "";
    public bool IsLoadingWorld = false;
    public bool IsTartgetPositionAll = false;
    public void IsTartgetPositionAllOn()
    {
        IsTartgetPositionAll = !IsTartgetPositionAll;
    }

    public Vector3 PersonsTargetPosition { get; set; }


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

    public static UIEvents Events
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

    private int _limitHorizontalLook = 22;
    public int LimitHorizontalLook
    {
        get { return _limitHorizontalLook; }
    }
    private int _limitVerticalLook = 18;
    public int LimitVerticalLook
    {
        get { return _limitVerticalLook; }
    }

    private int _heroPositionX = 0;
    public int HeroPositionX
    {
        get { return _heroPositionX; }
    }
    private int _heroPositionY = 0;
    public int HeroPositionY
    {
        get { return _heroPositionY; }
    }

    //private SaveLoadData.LevelData _personsData;
    //public SaveLoadData.LevelData PersonsData
    //{
    //    get { return _personsData; }
    //}

    public Dictionary<string, GameObject> Fields;

    //private Dictionary<string, List<SaveLoadData.ObjectData>> _GamesObjectsPersonalData;
    //public Dictionary<string, List<SaveLoadData.ObjectData>> GamesObjectsPersonalData
    //{
    //    get
    //    {
    //        return _GamesObjectsPersonalData;
    //    }
    //}

    private Dictionary<string, List<GameObject>> _GamesObjectsReal = null;
    public Dictionary<string, List<GameObject>> GamesObjectsReal
    {
        get { return _GamesObjectsReal; }
    }

    private ModelNPC.GridData _GridDataG = null;
    public ModelNPC.GridData GridDataG
    {
        get { return _GridDataG; }
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



    private CompletePlayerController _screiptHero;
    //public CompletePlayerController PlayerController
    //{
    //    get
    //    {
    //        return _screiptHero;
    //    }
    //}


    private CreateNPC _scriptNPC;
    private UIEvents _scriptUIEvents;


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

        ZonaField = null;
        ZonaReal = null;

        //Fields = new Dictionary<string, GameObject>();
        //_GamesObjectsReal = new Dictionary<string, List<GameObject>>();
        //_GridDataG = new SaveLoadData.GridData();
        //_personsData = new SaveLoadData.LevelData();
        //_listHistoryGameObject = new List<HistoryGameObject>();
        //DestroyObjectList = new List<GameObject>();
        //_GamesObjectsPersonalData = new Dictionary<string, List<SaveLoadData.ObjectData>>();
        InitObjectsGrid();

        //LoadData();

        InitComponents();

        //StartGenGrigField();

        LoadData();

        LoadGameObjects();

        //LoadData();
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

        _scriptNPC = MainCamera.GetComponent<CreateNPC>();
        if (_scriptNPC == null)
        {
            Debug.Log("Storage.Start scriptNPC not load !!!!!");
            return;
        }
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
        _StorageCorrect = gameObject.GetComponent<StorageCorrect>();
        if (_StorageCorrect != null)
        {
            //Debug.Log("InitObjectsGrid Destroy(_StorageCorrect) __________________________");
            Destroy(_StorageCorrect);
        }
        _StorageCorrect = gameObject.AddComponent<StorageCorrect>();


        //Reinit Component
        _StoragePerson = gameObject.GetComponent<StoragePerson>();
        if (_StoragePerson != null)
        {
            Destroy(_StoragePerson);
        }
        _StoragePerson = gameObject.AddComponent<StoragePerson>();
        _StoragePerson.PersonsDataInit();

        //_PlayerManager = gameObject.AddComponent<PlayerManager>();
        //_PlayerManager = GetComponentInParent<PlayerManager>();
        _PlayerManager = GetComponent<PlayerManager>();
        if (_PlayerManager==null)
        {
            Debug.Log("########## InitComponents PlayerManager is Empty");
            return;
        }
        _PlayerManager.Init();

        _Palette = gameObject.GetComponent<ManagerPalette>();
        if (_Palette == null)
        {
            Debug.Log("########## InitComponents _Palette is Empty");
            return;
        }
    }

    private void InitObjectsGrid()
    {
        //Debug.Log("III InitObjectsGrid_______________");
        Fields = new Dictionary<string, GameObject>();
        _GamesObjectsReal = new Dictionary<string, List<GameObject>>();
        //_GridDataG = new SaveLoadData.GridData();


        _StorageLog = new StorageLog();
        _StorageLog.Init();
        _UpdateData = new UpdateData();
        //_StorageCorrect = new StorageCorrect();

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
        if (_scriptNPC != null)
            _scriptNPC.StopCrateNPC();

        //---
        DestroyAllGamesObjects();

        InitObjectsGrid();
    }

    public void CreateWorld()
    {
        StopGame();
        LoadGameObjects(true, true);
    }

    private void LoadGameObjects(bool isLoadRealtime = false, bool isCreate = false)
    {
        Debug.Log("III LoadGameObjects ::::_______________");

        //TTTT
        _screiptHero.FindFieldCurrent();

        //_scriptGrid.StartGenGrigField(isLoadRealtime);
        _scriptGrid.StartGenGrigField(true);

        //Debug.Log("III CreateDataGamesObjectsWorld_______________");
        _scriptData.CreateDataGamesObjectsWorld(isCreate);

        //Debug.Log("III ....Init Position HERO......");
        //_screiptHero.FindFieldCurrent();

        //if (!isLoadRealtime)
        //{
        //    Debug.Log("....................._scriptGrid.StartGenGrigField(true)");
        //    _scriptGrid.StartGenGrigField(true);
        //}

        if (isLoadRealtime)
            StartCoroutine(StartFindLookObjects());
        else
        {
            //Debug.Log("III ....Init LoadObjectsNearHero ......");
            _scriptGrid.LoadObjectsNearHero();
            //Debug.Log("III ....Sart Crate NPC......");
            _scriptNPC.SartCrateNPC();
        }
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
        _scriptNPC.SartCrateNPC();
    }

    // Update is called once per frame

    //public static void SetGridData(SaveLoadData.GridData p_GridData)
    //{
    //    _GridData = p_GridData;
    //}
    //public static void SetGamesObjectsReal(Dictionary<string, List<GameObject>> p_GamesObjectsReal)
    //{
    //    _GamesObjectsReal = p_GamesObjectsReal;
    //}

    public void LoadData()
    {
        //_datapath = Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";
        //_datapath = Application.dataPath + "/SavedData" + Application.loadedLevel + ".xml";
        //_datapathLevel = Application.dataPath + "/Levels/LevelData" + Application.loadedLevel + ".xml";

        //Debug.Log("# LoadPathData... " + _datapathLevel);

        if (File.Exists(_datapathLevel))
        {
            //@ST@ _gridData = Serializator.LoadGridXml(_datapathLevel);
            _GridDataG = Serializator.LoadGridXml(_datapathLevel);
        }
        else
        {
            Debug.Log("# LoadPathData not exist: " + _datapathLevel);
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

    public void SetHeroPosition(int x, int y, float xH, float yH)
    {
        //Debug.Log("SetHeroPosition...");

        int scale = 2;
        _heroPositionX = x;
        _heroPositionY = y;

        int _limitX = _limitHorizontalLook / 2;
        int _limitY = _limitVerticalLook / 2;
        {
            int fX = x - _limitX;
            int fY = y - _limitY;

            if (fX < 0) fX = 0;
            if (fY < 0) fY = 0;
            int fX2 = x + _limitX;
            int fY2 = y + _limitY;

            ZonaField = new ZonaFieldLook()
            {
                X = fX,
                Y = fY,
                X2 = fX2,
                Y2 = fY2
            };
            //Debug.Log("ZonaField: X:" + ZonaField.X + " Y:" + ZonaField.Y + " X2:" + ZonaField.X2 + " Y2:" + ZonaField.Y2);
        }
        {
            float limitH = _limitHorizontalLook / 2;
            float limitV = _limitVerticalLook / 2;

            float rX = xH - (_limitX * scale);
            float rY = yH + (_limitY * scale);
            float margin = 0.1f;
            if (rX < 0)
            {
                rX = 0.1f;
                limitH -= margin;
            }
            if (rY > 0)
            {
                rY = -0.1f;
                limitV -= margin;
            }
            int LevelX = Helper.WidthLevel * scale;
            int LevelY = Helper.HeightLevel * scale;

            float rX2 = xH + (limitH * scale);
            float rY2 = yH - (limitV * scale);
            if (rX2 > LevelX) rX2 = LevelX;
            if (rY2 > LevelY) rY2 = LevelY;

            ZonaReal = new ZonaRealLook()
            {
                X = rX,
                Y = rY,
                X2 = rX2,
                Y2 = rY2
            };
            //Debug.Log("ZonaReal: X:" + ZonaReal.X + " Y:" + ZonaReal.Y + " X2:" + ZonaReal.X2 + " Y2:" + ZonaReal.Y2);
            //Draw result
            //DrawRect(rX,rY,rX2,rY2);
        }
    }


    public string UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, ModelNPC.ObjectData objData, Vector3 p_newPosition, GameObject thisGameObject, bool isDestroy = false, bool NotValid = false)
    {
        if (Storage.Instance.IsLoadingWorld && !NotValid)
        {
            Debug.Log("_______________ LOADING WORLD ....._______________");
            return "";
        }

        if (Storage.Instance.IsCorrectData && !NotValid)
        {
            Debug.Log("_______________ RETURN LoadGameObjectDataForLook ON CORRECT_______________");
            return "Error";
        }

        if (_GamesObjectsReal == null || _GamesObjectsReal.Count == 0)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      GamesObjectsReal is EMPTY");
            return "";
        }
        if (_GridDataG == null || _GridDataG.FieldsD == null || _GridDataG.FieldsD.Count == 0)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      GridData is EMPTY");
            return "";
        }

        if (!_GamesObjectsReal.ContainsKey(p_OldField))
        {
            Debug.Log("********** (" + p_NameObject + ") ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition      GamesObjectsReal not found OldField = " + p_OldField);
            if (p_NameObject != null)
                Storage.Instance.SelectGameObjectID = Helper.GetID(p_NameObject);

            Storage.Log.GetHistory(p_NameObject);

            //@@CORRECT
            //Destroy(thisGameObject, 1f);
            //return "Error";
            return "";
        }
        if (!_GridDataG.FieldsD.ContainsKey(p_OldField))
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition      GridData not found OldField = " + p_OldField);
            return "";
        }

        List<GameObject> realObjectsOldField = _GamesObjectsReal[p_OldField];
        List<ModelNPC.ObjectData> dataObjectsOldField = _GridDataG.FieldsD[p_OldField].Objects;

        if (realObjectsOldField == null)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition     realObjectsOldField is Null !!!!");
            if (!_GamesObjectsReal.ContainsKey(p_OldField))
            {
                Debug.Log("********** UpdatePosition     in GamesObjectsReal not found OldField = " + p_OldField);
                return "";
            }
            else
            {
                _GamesObjectsReal[p_OldField] = new List<GameObject>();
            }
            return "";
        }

        //#TEST -----
        for (int i = realObjectsOldField.Count - 1; i >= 0; i--)
        {
            if (realObjectsOldField[i] == null)
            {
                Debug.Log("UGP: (" + p_NameObject + ") ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
                Debug.Log("^^^^ UpfatePosition  -- remove destroy realObjects");
                realObjectsOldField.RemoveAt(i);
            }
        }
        //--------------

        int indReal = realObjectsOldField.FindIndex(p => p.name == p_NameObject);
        if (indReal == -1)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpfatePosition Not Real object (" + p_NameObject + ") in field: " + p_OldField);
            if (p_NameObject != null)
                Storage.Instance.SelectGameObjectID = Helper.GetID(p_NameObject);

            //Storage.Fix.CorrectData(p_NameObject, "UpfatePosition Not Real");


            //return "Error";

            Storage.Log.GetHistory(p_NameObject);
            return "";
        }
        int indData = dataObjectsOldField.FindIndex(p => p.NameObject == p_NameObject);
        if (indData == -1)
        {
            //--------------------
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            var posI = thisGameObject.transform.position;
            string info = " >>>>>> thisGameOobject : " + thisGameObject.name + "       pos = " + Helper.GetNameFieldPosit(posI.x, posI.y);

            Debug.Log("^^^^ UpfatePosition Not DATA object (" + p_NameObject + ") in field: " + p_OldField + "     " + info);
            foreach (var itemObj in dataObjectsOldField)
            {
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- object : " + itemObj.NameObject);
            }
            if (dataObjectsOldField.Count == 0)
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- objects ZERO !!!!!");
            //--------------------

            Storage.Fix.CorrectData(p_NameObject, "UpfatePosition IN DATA");


            return "Error";
        }
        GameObject gobj = realObjectsOldField[indReal];
        if (gobj == null)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      gobj is Destroy");
            return "";
        }

        if (!gobj.Equals(thisGameObject))
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("################ ERROR Not Equals thisGameOobject (" + thisGameObject + ")  and RealObject (" + gobj + ")");
            Storage.Instance.SelectGameObjectID = Helper.GetID(gobj.name);
            //@CD@
            //_StorageCorrect.CorrectData(gobj, thisGameObject, "UpdateGamePosition");
            //return "Error";
            return "";
        }


        //add to new Field
        if (!_GridDataG.FieldsD.ContainsKey(p_NewField))
        {
            //#!!!!  Debug.Log("SaveListObjectsToData GridData ADD new FIELD : " + posFieldReal);
            _GridDataG.FieldsD.Add(p_NewField, new ModelNPC.FieldData());
        }

        if (p_newPosition != gobj.transform.position)
        {
            Debug.Log("********** (" + gobj.name + ")^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("############### ERROR UpdatePosition 1.  ERROR POSITOIN : GAME OBJ NEW POS: " + p_newPosition + "       REAL OBJ POS: " + gobj.transform.position + "  REAL FIELD: " + Helper.GetNameFieldPosit(gobj.transform.position.x, gobj.transform.position.y));



            return "";
        }

        //Debug.Log("--------------------PRED NAME :" + objDataNow.NameObject);
        objData.NameObject = Helper.CreateName(objData.TagObject, p_NewField, "", p_NameObject);
        gobj.name = objData.NameObject;
        //Debug.Log("--------------------POST NAME :" + objDataNow.NameObject);

        //Debug.Log("UpdateGamePosition TEST POSITION GameObj ref: " + p_newPosition + "     GameObj realObjects: " + gobj.transform.position);

        //@POS@ Debug.Log("---- SET POS --- GO:" + gobj.name + "    DO:" + objData.NameObject);
        if (p_newPosition != gobj.transform.position)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** ERROR UpdatePosition 2.   ERROR POSITOIN :  GAME OBJ NEW POS: " + p_newPosition + "       REAL OBJ POS: " + gobj.transform.position);
            return "";
        }
        objData.Position = gobj.transform.position;

        if (isDestroy)
            objData.IsReality = false;

        if (!_GamesObjectsReal.ContainsKey(p_NewField))
        {
            _GamesObjectsReal.Add(p_NewField, new List<GameObject>());
        }

        //add
        if (!isDestroy)
            _GamesObjectsReal[p_NewField].Add(gobj);
        _GridDataG.FieldsD[p_NewField].Objects.Add(objData);

        //remove
        dataObjectsOldField.RemoveAt(indData);
        realObjectsOldField.RemoveAt(indReal);

        return gobj.name;
    }

    public GameObject CreatePrefab(ModelNPC.ObjectData objDataSave)
    {
        return _scriptGrid.CreatePrefabByName(objDataSave);
    }

    public void ClearGridData()
    {
        _GridDataG = new ModelNPC.GridData();
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



    #region Destroy

    public void DestroyObject(GameObject gobj)
    {
        Destroy(gobj);
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
                    Destroy(objDel);
                }
            }
        }

        foreach (var item in Fields)
        {
            string nameField2 = item.Key;
            GameObject resField = Fields[nameField2];
            if (resField != null)
            {
                Destroy(resField);
            }
        }
    }

    public void AddDestroyGameObject(GameObject gObj)
    {
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
                Debug.Log("*** POOL DESTROY GAME OBJECt INCORRECT : " + gObj.name);
                DestroyFullObject(gObj);
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

        if (gObj != null)
        {
            Destroy(gObj);
        }
        else
        {
            Debug.Log("+++ DestroyFullObject ++++ Destroy ---- object is null");
        }

        KillObject.Add(setName);
        //-----------------------------------------------
        bool isRemovedCorrect = false;
        bool isRemReal = false;
        bool isRemData = false;
        if (isCorrect)
        {
            isRemReal = _UpdateData.RemoveAllFindRealObject(setName);
        }

        //Destroy to Data
        if (!_GridDataG.FieldsD.ContainsKey(nameField))
        {
            Debug.Log("+++++ ------- DestroyRealObject ----- !GridData.FieldsD not field=" + nameField);
            return false;
        }
        List<ModelNPC.ObjectData> dataObjects = _GridDataG.FieldsD[nameField].Objects;
        int indObj = dataObjects.FindIndex(p => p.NameObject == gObj.name);
        if (!isCorrect)
        {
            if (indObj == -1)
            {
                Debug.Log("!!!! DestroyRealObject GridData not object=" + gObj.name);
                //RemoveAllFindRealObject(gObj.name);
                _UpdateData.RemoveAllFindDataObject(gObj.name);
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

    //public void StartCor(string nameObj, string _info)
    //{
    //    StartCoroutine(Fix.StartCreateNewCorrectObject(nameObj, _info));
    //}

    #region Graphic

    //public void DrawTrack2(List<Vector3> trackPoints, Color colorTrack)
    //{
    //    foreach(var point in trackPoints)
    //    {
    //        Debug.Log("DrawPolyLine Point : " + point);
    //    }
    //    Handles.DrawPolyLine(trackPoints.ToArray());
    //}

    //public static Vector3 PositionHandle(Vector3 position, Quaternion rotation)
    //{
    //    float handleSize = HandleUtility.GetHandleSize(position);
    //    Color color = Handles.color;
    //    Handles.color = Handles.xAxisColor;
    //    position = Handles.Slider(position, rotation * Vector3.right, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), SnapSettings.move.x);
    //    Handles.color = Handles.yAxisColor;
    //    position = Handles.Slider(position, rotation * Vector3.up, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), SnapSettings.move.y);
    //    Handles.color = Handles.zAxisColor;
    //    position = Handles.Slider(position, rotation * Vector3.forward, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), SnapSettings.move.z);
    //    Handles.color = Handles.centerColor;
    //    position = Handles.FreeMoveHandle(position, rotation, handleSize * 0.15f, SnapSettings.move, new Handles.DrawCapFunction(Handles.RectangleCap));
    //    Handles.color = color;
    //    return position;
    //}



    IEnumerator AminateAlphaColor(GameObject obj)
    {
        while (true)
        {
            var color = obj.GetComponent<Renderer>().material.color;
            for (float i = 1; i >= 0; i -= 0.1f)
            {
                color.a = i;
                obj.GetComponent<Renderer>().material.color = color;
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            for (float i = 0; i < 1; i += 0.1f)
            {
                color.a = i;
                obj.GetComponent<Renderer>().material.color = color;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void DrawRect(float x, float y, float x2, float y2)
    {
        //return;
        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.Log("LineRenderer is null !!!!");
            return;
        }

        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //lineRenderer.SetColors(c1, c2);
        lineRenderer.SetColors(Color.green, Color.green);
        lineRenderer.SetWidth(0.2F, 0.2F);
        int size = 5;
        lineRenderer.SetVertexCount(size);

        Vector3 pos1 = new Vector3(x, y, -2);
        lineRenderer.SetPosition(0, pos1);
        Vector3 pos2 = new Vector3(x2, y, -2);
        lineRenderer.SetPosition(1, pos2);
        Vector3 pos3 = new Vector3(x2, y2, -2);
        lineRenderer.SetPosition(2, pos3);
        Vector3 pos4 = new Vector3(x, y2, -2);
        lineRenderer.SetPosition(3, pos4);
        Vector3 pos5 = new Vector3(x, y, -2);
        lineRenderer.SetPosition(4, pos5);
    }

    //---------------------
    public GameObject prefabCompas;
    public int numberOfObjects = 10;
    public float radius = 0.1f;

    void StartGenCircle()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;

            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            pos += new Vector3(10, -5, 0);

            Instantiate(prefabCompas, pos, Quaternion.identity);
        }
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
