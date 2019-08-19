using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.U2D;
using UnityEngine.UI;


//[RequireComponent(System.Type type)]
//Attributes:
//[Header("_text")]
//    public Vector2 range = new Vector2(0.1f, 1);
//[Tooltip(_text")]
//[ContextMenuItem("Reset", "resetTheValue")]
//    public float speed = 1f;
//[Tooltip("_text")]
//public float maxY = 0.5f;
//[Range(0.1f, 1f)]
//public float offXMax = 0.1f;


public class UIEvents : MonoBehaviour {

    //public static bool IsCursorVisible = true;
    public bool IsCursorVisible = true;
    public bool IsTrackPointsVisible = false;
    public int LimitLogView = 10;

    public Text txtMessage;         //Store a reference to the UI Text component which will display the 'You win' message.

    public string SetTittle
    {
        get
        {
            return txtMessage.text;
        }
        set
        {
            txtMessage.text = value;
        }
    }
    public string SetMessageBox
    {
        set
        {
            PanelMessage.SetActive(true);
            MessageBox.text = value;
        }
    }


    public Text txtLog;
    public Button btnExit;
    public Button btnTest;
    public Button btnPanelPersonOpen;
    public Button btnPanelPersonClose;
    public Toggle btnShowLog;
    public InputField tbxTest;
    public GameObject PanelLog;
    public GameObject ContentListLogCommand;
    public GameObject ListBoxExpandPerson;
    public GameObject contentListExpandPerson;
    public GameObject PrefabExpandPanel;
    public GameObject ScrollListBoxPerson;
    public GameObject PointGO;
    public GameObject PanelMessage;
    public GameObject PrefabListBox;

    public GameObject PanelInventory;


    public GameObject BackgroundTittle;
    public Text MessageBox;
    public Toggle checkHideCanvasUI;

    public Text prefabText;
    public Button prefabButtonCommand;
    public static string ColorExpClose = "#FFFFFF";
    public static string ColorExpOpen = "#FFFA00";
    public static string ColorYelow = "#CDA143";
    public static string ColorGreen = "#468C44";
    public static string ColorAlert = "#EC4D56";

    private bool IsProfilerUI = false;// false;

    [Header("Menu Command")]
    public Dropdown dpnMenuCommandTest;

    //private List<string> m_CommandLogList = new List<string>();
    private List<string> m_ListLog = new List<string>();

    public float ActionRate = 0.5f;
    private float DelayTimer = 0F;
    private int _CounterRealObj = 0;

    int allFPS = 0;
    int countFPS = 0;
    int itogFPS = 0;
    int itogPool = 0;
    int itogPoolObject = 0;

    void Awake()
    {
        btnExit.onClick.AddListener(delegate
        {

            ExitGame();
        });

        //tbxTest.OnUpdateSelected += () => { };
        tbxTest.onValueChange.AddListener(delegate
        {
            FindInputCommand();
        });


        btnPanelPersonOpen.onClick.AddListener(delegate
        {
            ListBoxExpandPerson.SetActive(true);
            btnPanelPersonClose.gameObject.SetActive(true);
            btnPanelPersonOpen.gameObject.SetActive(false);
        });
        btnPanelPersonClose.onClick.AddListener(delegate
        {
            ListBoxExpandPerson.SetActive(false);
            btnPanelPersonClose.gameObject.SetActive(false);
            btnPanelPersonOpen.gameObject.SetActive(true);
        });

        btnShowLog.onValueChanged.AddListener(delegate
        {
            PanelLog.SetActive(btnShowLog.isOn);
        });
        checkHideCanvasUI.onValueChanged.AddListener(delegate
        {
            PanelLog.SetActive(checkHideCanvasUI.isOn);
            if (!checkHideCanvasUI.isOn)
            {
                btnShowLog.isOn = checkHideCanvasUI.isOn;
            }
            PrefabListBox.SetActive(checkHideCanvasUI.isOn);
            dpnMenuCommandTest.gameObject.SetActive(checkHideCanvasUI.isOn);
            tbxTest.gameObject.SetActive(checkHideCanvasUI.isOn);
            btnTest.gameObject.SetActive(checkHideCanvasUI.isOn);
            PanelInventory.gameObject.SetActive(checkHideCanvasUI.isOn);
            BackgroundTittle.gameObject.SetActive(checkHideCanvasUI.isOn);
        });

        btnTest.onClick.AddListener(TestClick);

        //LoadInventoryCase();
    }

   

    // Use this for initialization
    void Start()
    {
        if (File.Exists(Storage.Instance.DataPathUserData))
        {
            LoadCommandTool();
        }
        else
        {
            Debug.Log("########## DataPathUserData not exist: " + Storage.Instance.DataPathUserData);
        }

        if (PoolGameObjects.IsStack)
        {
            StartCoroutine(CalculateTagsInPool());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //AlphaTerra();
        if (Input.GetKey("m") && Time.time > DelayTimer)
        {
            Storage.Map.Create();

            DelayTimer = Time.time + ActionRate;
        }
        if (Input.GetKey("/") && Time.time > DelayTimer)
        {
            Storage.Map.Create(true);

            DelayTimer = Time.time + ActionRate;
        }

        if (Input.GetKey("p") && Time.time > DelayTimer)
        {
            Storage.PaletteMap.Show();
            DelayTimer = Time.time + ActionRate / 2;
        }

        if (Input.GetKey("e") && Time.time > DelayTimer)
        {
            HeroExtremalOn();
            DelayTimer = Time.time + ActionRate / 2;
        }

        if (Input.GetKey("f") && Time.time > DelayTimer)
        {
            Storage.PlayerController.Speed = Storage.PlayerController.Speed == 20 ? 5 : 20;
            DelayTimer = Time.time + ActionRate;
        }
    }

    bool m_isFindedBug = false;

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());

        //GUI.Label(new Rect(0, 30, 100, 100), Counter.ToString());
        if (IsProfilerUI)
        {

            GUI.Label(new Rect(0, 550, 150, 200), "REAL GOBJ: " + _CounterRealObj.ToString());
            GUI.Label(new Rect(150, 550, 150, 200), "FPS: " + itogFPS.ToString());
            GUI.Label(new Rect(300, 550, 150, 200), "POOL: " + itogPoolObject.ToString() + " / " + itogPool.ToString());

        }

        if (PoolGameObjects.IsTestingDestroy)
        {
            var destroyedPrefabsTest = Storage.Pool.PoolGamesObjects.Where(p => p.IsLock && p.GameObjectNext == null).ToList();
            if (destroyedPrefabsTest.Count > 0)
            {
                if (m_isFindedBug == false)
                {
                    m_isFindedBug = true;
                    PoolGameObject _pool = destroyedPrefabsTest[0];
                    Debug.Log("/////// Pool contains null object (" + destroyedPrefabsTest.Count + ")  " + _pool.ToString());
                    GUI.Label(new Rect(400, 550, 150, 200), "Pool contains null object (" + destroyedPrefabsTest.Count + ")  " + _pool.ToString());

                    Storage.Log.GetHistory(_pool.NameObject);
                }
            }
        }

       
    }

    int m_savecountInPool = 0;

    
    public string ListLogToString
    {
        get
        {
            //Debug.Log("ListLogToString=" + string.Join("\n", m_ListLog.ToArray()));
            return string.Join("\n", m_ListLog.ToArray());
        }
    }
    public string ListLogAdd
    {
        set
        {
            if (m_ListLog.Count > LimitLogView)
            {
                //Debug.Log("ListLogAdd   m_ListLog.Count(" + m_ListLog.Count + ") > (" + LimitLogView + ")LimitLogView");

                m_ListLog.RemoveAt(0);
            }

            //Debug.Log(">>>>>>>>>>>>>>> ListLogAdd  Add + " + value + "  IN " + String.Join(",", m_ListLog.ToArray()));
            m_ListLog.Add(value);
            SetTextLog = ListLogToString;

        }
    }
    public void ListLogClear()
    {
        m_ListLog.Clear();
    }


    private string SetTextLog
    {
        set
        {
            //Debug.Log("SetTextLog===" + value);
            txtLog.text = value;
            //txtLog.text = "TEST_TEST_TEST_TEST_TEST_TEST_TEST_TEST_TEST_TEST_TEST_TEST_";
        }
    }

    private void FindInputCommand()
    {
        //if ((Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)))
        if (tbxTest.text.IndexOf("~") != -1)
        {
            tbxTest.text = tbxTest.text.Replace("~", "");
            tbxTest.text = tbxTest.text.Replace(".", ",");
            Debug.Log("FindInputCommand");
            //tbxTest.text = ""; //Clear Inputfield text
            //tbxTest.ActivateInputField(); //Re-focus on the input field
            //tbxTest.Select();//Re-focus on the input field
            TestClick();
        }
    }


    public void PlayerPressEscape()
    {
        ExitProgramm();
    }

    private void ExitProgramm()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetCountText(int count)
    {
        int limitEndGame = 150;
        SetTittle = "Count: " + count.ToString() + " / " + limitEndGame;

        if (count >= limitEndGame)
        {
            SetTittle = "You win! :" + count;
            Storage.PlayerController.EndGame();
        }
    }

    private void ExitGame()
    {
        Storage.Disk.SaveLevel();

        Storage.Player.SavePosition();

        ExitProgramm();
    }

    public string GetSelectCommandName
    {
        get
        {
            int selIndex = dpnMenuCommandTest.value;
            var menuCommands = dpnMenuCommandTest.options.ToArray();
            string selectCommand = menuCommands[selIndex].text.ToString();
            return selectCommand;
        }

    }

    public bool IsCommandTeleport
    {
        get
        {
            if (GetSelectCommandName == "Teleport")
                return true;
            return false;
        }
    }


    private void TestClick()
    {
        int selIndex = dpnMenuCommandTest.value;
        var menuCommands = dpnMenuCommandTest.options.ToArray();

        //List<string> messages = new List<string>();

        //messages.Add("Sel GO: [" + tbxTest.text + "]");

        string selectCommand = menuCommands[selIndex].text.ToString();

        Debug.ClearDeveloperConsole();
        //Debug.Log(">>>>>  COMMAND >>>>> " + selectCommand);
        //ListLogAdd = ">>>>>  COMMAND >>>>> " + selectCommand;
        CommandExecute(selectCommand);

        
        CreateCommandLogButton(selectCommand, Color.white, ContentListLogCommand.transform, null, true);

        //txtMessage.text = string.Join("\n", messages.ToArray()); // "Selected: [" + tbxTest.text + "]"; 
        //SetTextLog = string.Join("\n", messages.ToArray());
        //ListLogAdd = 
        Storage.Instance.SelectGameObjectID = tbxTest.text;
    }


    public void CommandExecute(string selectCommand)
    {
        switch (selectCommand)
        {
            case "None":
                SetTittle = "...";
                //TEST
                //Storage.Events.ListLogAdd = Helper.GetNameFieldByName("PrefabField_Field21x9_a87f");
                Sprite[] _sprites = Storage.Palette.GetSpritesAtlasMapPrefab();
                foreach (var item in _sprites)
                {
                    Storage.PaletteMap.CreateCellPalette(item);
                }
               
                break;
            case "SaveWorld":
                SetTittle = "Level saving...";
                //m_scriptData.SaveLevel();
                //#TEST
                //m_scriptData.SaveLevelParts();
                Storage.Disk.SaveLevelCash();
                SetTittle = "Level saved.";
                break;
            case "LoadWorld":
                //m_scriptData

                SetTittle = "Level loading...";
                Storage.Instance.LoadLevels();
                SetTittle = "Level loaded.";
                break;
            case "ReloadWorld":
                SetTittle = "Reload Level...";
                //SetTittle = "Level saving...";
                //m_scriptData.SaveLevel();
                //SetTittle = "Level loading...";
                //Storage.Instance.LoadLevels();
                //SetTittle = "Level loaded.";
                Storage.Instance.ReloadWorld();
                break;
            case "CreateWorld":
                SetTittle = "Create Level...";
                Storage.Instance.CreateWorld();
                break;
            case "TartgetPositionAll":
                SetTittle = @"TartgetPositionAll On\Off...";
                //Storage.Instance.IsTartgetPositionAll = true;
                Storage.Instance.IsTartgetPositionAllOn();
                break;
            case "LoadXML":
                SetTittle = "LoadXML...";
                Storage.Instance.LoadData();
                break;
            case "Pause":
                //Debug.Log("CommandExecuteParson(" + selectCommand + ") :  " + selectCommand);
                SetTittle = "Game Pause...";
                Storage.GamePause = !Storage.GamePause;
                break;
            case "ClearLog":
                SetTittle = "Clear Log...";
                ListLogClear();
                break;
            case "LogPerson":
                SetTittle = "Create Expand Pesron Test...";

                //CreateCommandLogButton("TEST", Color.gray, contentListExpandPerson.transform);
                //break;
                AddExpandPerson("Tittle TEST",
                    new List<string> { "Param1: qqqq", "Param2: ssss", "Param3: dddd" }, 
                    new List<string> { "Pause", "Kill" },
                    gobjObservable: null);
                break;
            case "CursorSelection":
                SetTittle = @"Cursor selection On\Off...";
                Storage.PlayerController.CursorSelectionOn();
                break;
            case "ClearCommands":
                ClearAllCommandButtonsTool();
                break;
            case "SaveCommandTool":
                SaveCommandTool();
                break;
            case "LoadCommandTool":
                LoadCommandTool();
                break;
            case "LoadGridLook":
                Storage.GenGrid.LoadObjectsNearHero();// LoadGridLook();
                break;
            case "ReloadGridLook":
                Storage.GenGrid.ReloadGridLook();
                break;
            case "MapCreate":
                Storage.Map.Create();
                break;
            case "SavePlayer":
                Storage.Player.SavePosition();
                break;
            case "LoadPlayer":
                Storage.Player.LoadPositionHero();
                break;
            //case "LoadGridTiles":
            //    Storage.TilesManager.LoadGridTiles();
            //    break;
            case "UpdateGridTiles":
                Storage.TilesManager.UpdateGridTiles();
                break;
            case "Teleport":
                Vector2 posTeleport = GetPositTeleport();
                Storage.Player.TeleportHero((int)posTeleport.x, (int)posTeleport.y);
                break;
            case "GenWorldExtremal":
                Storage.GamePause = true;
                Storage.Instance.StopGame();
                Storage.GridData.GenericWorldExtremal();
                Storage.Instance.ReloadWorld();
                Storage.GenGrid.LoadObjectsNearHero();
                break;
            case "LoadPrefabsOnPalette":
                Storage.PaletteMap.PrefabsOnPalette();
                break;
            case "HeroExtremal":
                HeroExtremalOn();
                //Storage.Player.HeroExtremal = !Storage.Player.HeroExtremal;
                break;
            case "GetInfo":
                IsProfilerUI = !IsProfilerUI;
                //Storage.Events.ListLogAdd = "Profiler.usedHeapSize: " + Profiler.usedHeapSize.ToString();
                string str = IsProfilerUI ? "On" : "Off";
                Storage.EventsUI.ListLogAdd = "ProfilerUI is " + str;
                if (IsProfilerUI)
                {
                    
                    StartCoroutine(CalculateObjectsAll());
                    StartCoroutine(CalculateProfiler());
                }
                break;
            case "OnParallax":
                Storage.DrawGeom.ParallaxOn();
                break;
            case "LoadMapGrid":
                Storage.Map.LoadGrid();
                break;
            case "NextDayOfTime":
                Storage.SceneLight.NextTimeOfDay();
                break;
            default:
                Debug.Log("################ EMPTY COMMAND : " + selectCommand);
                break;
        }

        ListLogAdd = "CALL >> " + selectCommand;
    }

    public void CommandExecutePerson(string selectCommand, GameObject gobjObservable, ModelNPC.GameDataNPC dataNPC)
    {
        MovementNPC movem = null;
        if (gobjObservable != null)
            movem = gobjObservable.GetComponent<MovementNPC>();

        switch (selectCommand)
        {
            case "None":
                SetTittle = "...";
                break;
            case "Pause":
                if (gobjObservable == null)
                    return;
                SetTittle = "Level saving...";
                if(movem==null)
                {
                    Debug.Log("############ CommandExecuteParson(" + selectCommand  + ") : " + gobjObservable.name  + " MovementNPC is Empty");
                    return;
                }
                Debug.Log("CommandExecuteParson(" + selectCommand + ") : " + gobjObservable.name + " : " + selectCommand);
                movem.Pause();
                break;
            case "Kill":
                if (gobjObservable != null)
                    Storage.Instance.AddDestroyGameObject(gobjObservable);
                break;
            case "StartTrack":
                if (gobjObservable == null)
                    return;
                if (movem == null)
                {
                    Debug.Log("############ CommandExecuteParson(" + selectCommand + ") : " + gobjObservable.name + " MovementNPC is Empty");
                    return;
                }
                movem.TrackOn();
                break;
            case "GoTo":
                Vector2 posTeleport = new Vector2();
                if (gobjObservable != null)
                {
                    posTeleport = gobjObservable.transform.position;
                }
                else
                {
                    if(dataNPC == null)
                    {
                        ListLogAdd = "######### GoTo: Observable DATA NPC is Empty";
                        return;
                    }
                    posTeleport = dataNPC.Position;
                }
                Storage.Player.TeleportHero((int)posTeleport.x, (int)posTeleport.y);
                break;
            default:
                Debug.Log("################ EMPTY COMMAND : " + selectCommand);
                break;
        }

    }

    public void CursorClickAction(Vector2 posCursorToField, string _fieldCursor)
    {
        string _infoPoint = "Cursor :" + posCursorToField + "\nfind:" + _fieldCursor;

        CursorClickAction(_fieldCursor, _infoPoint);

        if (Storage.Instance.IsTartgetPositionAll)
            Storage.Person.SetTartgetPositionAll(posCursorToField);
    }

    public void CursorClickAction(string _fieldCursor, string _infoPoint = "")
    {
        //if (Storage.PaletteMap.IsCursorOn)
        if (Storage.PaletteMap.ModePaint == ToolBarPaletteMapAction.Cursor)
        {
            if(string.IsNullOrEmpty(_infoPoint))
                _infoPoint = "Cursor : " + _fieldCursor;
            Storage.Instance.SelectFieldCursor = _fieldCursor;
            Storage.Person.SelectGameObjectDataByField(_fieldCursor);
        }

        if (Storage.PaletteMap.IsPaintsOn)
        {
            Storage.PaletteMap.PaintAction();
        }
    }

    public void CreateCommandLogText(string p_text, Color color, Transform p_parent)
    {
        Vector3 pos = new Vector3(0, 0, 0);
        Text resGO = (Text)Instantiate(prefabText, pos, Quaternion.identity);
        resGO.text = p_text;
        resGO.transform.SetParent(p_parent);
        //Debug.Log("CreateCommandLogText : " + p_text);
    }

    public void CreateListButtton(string p_text, Transform p_parent, out Button resGO, out Button btnSubCommand)
    {
        Vector3 pos = new Vector3(0, 0, 0);
        resGO = (Button)Instantiate(prefabButtonCommand, pos, Quaternion.identity);
        resGO.GetComponentInChildren<Text>().text = p_text;
        resGO.transform.SetParent(p_parent);

        Button btnSub = null;
        foreach (Transform itemSub in resGO.transform)
        {
            if(itemSub.gameObject.name == "btnSubCommand")
            {
                btnSub = itemSub.gameObject.GetComponent<Button>();
                continue;
            }
        }
        btnSubCommand = btnSub as Button;

        if (btnSubCommand != null)
            btnSubCommand.gameObject.SetActive(true);
        else
            Storage.EventsUI.ListLogAdd = "#### btnSubCommand is null";
    }

    public void CreateCommandLogButton(string p_text, Color color, Transform p_parent, GameObject gobjObservable = null, bool isValidExistCommand = false, ExpandControl expControl = null)
    {
        bool isPersonComm = false;
        if (gobjObservable != null)
            isPersonComm = true;

        //string nameBtn = "ButtonCommand" + p_text;
        string keyB = "ButtonCommand";
        string textBtn = p_text.Replace(keyB, "");
        string nameBtn = textBtn + keyB;

        GameObject findObjects;
        //GameObject[] findObjects = GameObject.FindGameObjectsWithTag("PrefabCommandButton");
        if (isValidExistCommand)
        {
            findObjects = GameObject.Find(nameBtn); 
            //Debug.Log("GameObject.Find(" + nameBtn + ") : " + findObjects);

            if (findObjects != null)
                isValidExistCommand = false;
        }
        else
            isValidExistCommand = true;

        //Debug.Log("CreateCommandLogButton " + p_text);

        //if (findObjects == null)
        if (isValidExistCommand)
        {
            //Debug.Log("CreateCommandLogButton 2. " + p_text);

            Vector3 pos = new Vector3(0, 0, 0);
            Button buttonCommand = (Button)Instantiate(prefabButtonCommand, pos, Quaternion.identity);
            Text compText = null;

            compText = buttonCommand.GetComponentInChildren<Text>();
            if (compText == null)
            {
                Debug.Log("######### CreateCommandLogButton compText is Empty");
                return;
            }


            compText.text = textBtn;
            buttonCommand.transform.SetParent(p_parent);
            buttonCommand.name = nameBtn;
            if (isPersonComm)
                buttonCommand.tag = "CommandButtonPerson";
            else
                buttonCommand.tag = "CommandButtonTool";

            if (isPersonComm)
            {
                buttonCommand.SetColor(ColorYelow); // "#CDA143");
            }
            else
            {
                buttonCommand.SetColor(ColorGreen);
            }

            //Debug.Log("ADD: CreateCommandLogText : " + nameBtn + "  parent: " + p_parent.name);


            string gobjID = "";
            ModelNPC.GameDataNPC dataObs = null;
            if (gobjObservable !=null && gobjObservable.IsNPC())
            {
                dataObs = gobjObservable.GetDataNPC();
            }

            if(gobjObservable!=null)
                gobjID = Helper.GetID(gobjObservable.name);
            //ExpandControl expControl = null

            //ADD EVENT COMMAND
            buttonCommand.onClick.AddListener(delegate
            {
                var _gobjID = gobjID;

                if (isPersonComm)
                {
                    //def color set
                    //buttonCommand.SetColor(ColorExpClose);

                    //if (gobjObservable == null)
                    if (expControl == null)
                    {
                        buttonCommand.SetColor(ColorAlert);// "#EC4D56");
                        Debug.Log("buttonCommand.onClick isPersonComm expControl == null");
                    }

                    if (expControl.IsAlert)
                    {
                        ListLogAdd = "PersonComm [" + textBtn + "] objObservable is Destroy & data empty";
                        buttonCommand.SetColor(ColorAlert);// "#EC4D56");
                        return;
                    }

                    //if (gobjObservable == null)
                    //{
                    gobjObservable = expControl.SelectedObserver();
                    //}
                    dataObs = (ModelNPC.GameDataNPC)expControl.DataObject;

                    if (dataObs == null)
                    {
                        ListLogAdd = "PersonComm [" + textBtn + "] objObservable is Destroy & data empty";
                    }
                    if (gobjObservable != null)
                        dataObs = gobjObservable.GetDataNPC();
                    

                    CommandExecutePerson(textBtn, gobjObservable, dataObs);
                }
                else
                {
                    //Debug.Log("######### gobjObservable  is NULL" + nameBtn);
                    CommandExecute(textBtn);
                }
                    
            });
        }
        else{
            //CreateCommandLogText(p_text, Color.white);
            //Debug.Log("Already exist CreateCommandLogButton " + nameBtn);
            
        }
    }

    private void HeroExtremalOn()
    {
        Storage.Player.HeroExtremal = !Storage.Player.HeroExtremal;
    }
    

    IEnumerator CalculateObjectsAll()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            if (Storage.Instance.GamesObjectsReal == null)
                yield return null;

            //_CounterRealObj = Storage.Person.GetAllRealPersons().ToList().Count(); //gobjsList.Count();
            _CounterRealObj = Storage.Instance.GamesObjectsReal.SelectMany(x => x.Value).Count();
        }

    }

   

    IEnumerator CalculateProfiler()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            int fps = (int)(1.0f / Time.smoothDeltaTime);
            allFPS += fps;
            countFPS++;
            itogFPS = allFPS / countFPS;

            if(allFPS>30000)
            {
                allFPS = 0;
                countFPS = 0;
            }

            if (PoolGameObjects.IsStack)
            {
                itogPoolObject = Storage.Pool.PoolGamesObjectsStack.SelectMany(p=>p.Value).Count();
                itogPool = itogPoolObject;
            }
            else
            {
                itogPoolObject = Storage.Pool.PoolGamesObjects.Where(p => p.IsLock).Count();
                itogPool = Storage.Pool.PoolGamesObjects.Count();
            }
        }

    }

    public void SetTestText(string text)
    {
        tbxTest.text = text;
    }

    public void ClearListExpandPersons()
    {
        var listExpandPersonControls = GameObject.FindGameObjectsWithTag("ExpandPersonControl");
        foreach (var exp in listExpandPersonControls)
        {
            if (exp.name == "PrefabExpandPanel")
                continue;
            Destroy(exp);
        }
    }


    public void AddExpandPerson(string tittle, List<string> listText, List<string> listCommand, GameObject gobjObservable)
    {
        //Debug.Log("AddExpandPerson .....");
        if (PrefabExpandPanel == null)
        {
            Debug.Log("########### PrefabExpandPanel is Empty");
            return;
        }
        string newNameExpand = "ExpandPanel" + Helper.GetID(gobjObservable.name);
        var resultFind = GameObject.Find(newNameExpand);
        
        //if (resultFind != null)
        //{
        var listExpandPersonControls = GameObject.FindGameObjectsWithTag("ExpandPersonControl");
        //Debug.Log("listExpandPersonControls count = " + listExpandPersonControls.Length);

        foreach(var exp in listExpandPersonControls)
        {
            if(exp.name == "PrefabExpandPanel")
            {
                continue;    
            }
            //Debug.Log("_EXP ME (" + newNameExpand + ")__________ expand find " + exp.name);

            ExpandControl scriptExp = exp.GetExpandControl();
            string nameExp = scriptExp.GetName;
            //if(nameExp!= newNameExpand)
            scriptExp.SetColorText(ColorExpClose);

            //Debug.Log("ME " + newNameExpand + "  FIND: " + nameExp);
            scriptExp.ExpandPanelOn(true, p_isOpen: false);
        }
   
        if (resultFind != null)
        {
            ExpandControl scriptExpand = resultFind.GetExpandControl();
            scriptExpand.SetColorText(ColorExpOpen);
            scriptExpand.ExpandPanelOn(p_isOpen: true);
            return;
        }

        Vector3 pos = new Vector3(0, 0, 0);
        GameObject expandGO = (GameObject)Instantiate(PrefabExpandPanel, pos, Quaternion.identity);

        expandGO.name = newNameExpand;
        expandGO.transform.SetParent(contentListExpandPerson.transform);

        if(gobjObservable == null)
        {
            Debug.Log("########### gobjObservable is Empty : " + tittle);
        }

        if (expandGO == null)
        {
            Debug.Log("########### expandGO is Empty");
            return;
        }

        if (expandGO.transform ==null)
        {
            Debug.Log("########### expandGO.transform = null");
            return;
        }

        ExpandControl scriptEvents =  expandGO.GetComponent<ExpandControl>();
        scriptEvents.SetGameObject(gobjObservable, tittle);
        scriptEvents.AddList(tittle, listText, listCommand, gobjObservable);

        //scriptEvents = resultFind.GetExpandControl();
        scriptEvents.SetColorText(ColorExpOpen);
        scriptEvents.ExpandPanelOn(p_isOpen: true);

        var scrollbar = ListBoxExpandPerson.GetComponentInChildren<Scrollbar>();
        
        if (scrollbar==null)
        {
            Debug.Log("########### ListBoxExpandPerson scrollbar = null");
            return;
        }
        scrollbar.numberOfSteps = 10;

    }

    private void ClearAllCommandButtonsTool()
    {
        GameObject[] listBtnCommandTool = GameObject.FindGameObjectsWithTag("CommandButtonTool");
        if(listBtnCommandTool==null || listBtnCommandTool.Length==0)
        {
            //Debug.Log("--- ClearAllCommandButtonsTool listBtnCommandTool is empty");
            return;
        }

        for(int i= listBtnCommandTool.Length-1; i>=0;i--)
        {
            Destroy(listBtnCommandTool[i]);
        }
    }

    private List<string> ListCommandsTool
    {
        get
        {
            GameObject[] listBtnCommandTool = GameObject.FindGameObjectsWithTag("CommandButtonTool");
            if (listBtnCommandTool == null || listBtnCommandTool.Length == 0)
            {
                Debug.Log("----- ClearAllCommandButtonsTool listBtnCommandTool is empty");
                return null;
            }

            return listBtnCommandTool.ToList().Select(p => p.name).ToList();

            //for (int i = listBtnCommandTool.Length - 1; i >= 0; i--)
            //{
            //    Destroy(listBtnCommandTool[i]);
            //}
        }
    }

    private void LoadCommandTool()
    {
        string path = Storage.Instance.DataPathUserData;
        CommandStore storeComm = Serializator.LoadXml<CommandStore>(path);
        
        if (storeComm==null)
        {
            Debug.Log("############ LoadCommandTool storeComm is Empty  path=" + path);
            return;
        }
        else
        {
            if(storeComm.CommadsTemplate==null)
            {
                Debug.Log("############ LoadCommandTool storeComm  CommadsTemplate is Empty");
                return;
            }
            if (storeComm.CommadsTemplate.Count==0)
            {
                Debug.Log("---- LoadCommandTool storeComm  CommadsTemplate is zero");
                return;
            }
            ClearAllCommandButtonsTool();
            foreach (string nameCommand in storeComm.CommadsTemplate)
            {
                CreateCommandLogButton(nameCommand, Color.white, ContentListLogCommand.transform, null, true);
            }

            //Debug.Log("---- LoadCommandTool Loaded..........");
        }
    }

//#if UNITY_EDITOR
//    //[MenuItem("Assets/Create/SaveCommandTool")]
//    [MenuItem("Assets/Create/SaveCommandTool")]
    public void SaveCommandTool()
    {
        CommandStore storeComm = new CommandStore();

        var listSave = ListCommandsTool.ToList();
        if (listSave != null && listSave.Count > 0)
        {
            storeComm = new CommandStore()
            {
                CommadsTemplate = listSave
            };

            string path = Storage.Instance.DataPathUserData;
            Serializator.SaveXml<CommandStore>(storeComm, path, true);
        }
        Debug.Log("Save Commands Tool count : " + storeComm.CommadsTemplate.Count);
        //ListLogAdd = "Save Commands Tool count : " + storeComm.CommadsTemplate.Count;
    }
//#endif

    private Vector2 GetPositTeleport()
    {
        Vector2 posTeleport = new Vector2(10, -10);
        string[] positInput = tbxTest.text.ToString().Split(',');
        if(positInput.Length!=2)
        {
            Debug.Log("######## GetPositTeleport Input Command not valid: [" + tbxTest.text + "] len=" + positInput.Length);
            return posTeleport;
        }

        float x = -1f;
        float y = -1f;
        if (positInput.Count() > 1) {
            float.TryParse(positInput[0], out x);
            float.TryParse(positInput[1], out y);
        }else
            Debug.Log("######## GetPositTeleport posStr X or Y not valid: [" + tbxTest.text + "]");

        if(x!=-1f && y!=-1f) 
            posTeleport = new Vector2(x, y);
        else  
            Debug.Log("######## GetPositTeleport NOT teleporting !!!");
        return posTeleport;
    }

    public void AddMenuPerson(ModelNPC.GameDataNPC _dataNPC, GameObject gobj)
    {
        AddExpandPerson(_dataNPC.NameObject,
            _dataNPC.GetParams,
            new List<string> { "GoTo", "Kill", "Pause", "StartTrack" },
            gobjObservable: gobj);
    }

    public void HideMessage()
    {
        PanelMessage.SetActive(false);
    }

    IEnumerator CalculateTagsInPool()
    {
        yield break;

        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (PoolGameObjects.IsStack)
            {

                int countInPool = Storage.Pool.PoolGamesObjectsStack.SelectMany(p => p.Value).Count();
                if (m_savecountInPool != countInPool)
                {
                    Debug.Log("-------------------- all pools Key:------------------------");
                    m_savecountInPool = countInPool;

                    var respools = Storage.Pool.PoolGamesObjectsStack;
                    foreach (var itemP in respools)
                    {
                        Debug.Log("-------------------- Key: " + itemP.Key + "   = " + itemP.Value.Count);
                    }
                }
            }
        }
    }

}

public class CommandStore
{
    public List<string> CommadsTemplate { get; set; }
}

public static class EventsExtensions
{
    public static ExpandControl GetExpandControl(this GameObject exp)
    {
        return exp.GetComponent<ExpandControl>();
    }
}

//public void CreateMessage(string text, Color color)
//{
//    if (color == null)
//    {
//        color = Color.green;
//    }
//    if (text == null)
//    {
//        text = "";
//    }

//    GameObject UItextGO = new GameObject(text.Replace(" ", "-"), typeof(RectTransform));
//    var newTextComp = UItextGO.AddComponent<Text>();
//    var canvas = UItextGO.AddComponent<CanvasRenderer>();
//    var transf = UItextGO.AddComponent<Transform>();
//    //canvas.transform

//    //Text newText = transform.gameObject.AddComponent<Text>();
//    newTextComp.text = text;
//    //newTextComp.font = fontMessage;
//    newTextComp.color = color;
//    newTextComp.alignment = TextAnchor.MiddleCenter;
//    newTextComp.fontSize = 10;

//    UItextGO.transform.SetParent(contentList.transform);
//}

//GameObject CreateText(string text, Color text_color)
//{
//    float x = 10;
//    float y = 10;
//    int font_size = 13;

//    GameObject UItextGO = new GameObject(text.Replace(" ", "-"));
//    UItextGO.transform.SetParent(contentList.transform);
//    UItextGO.transform.SetParent(textListLogs.transform);


//    RectTransform trans = UItextGO.AddComponent<RectTransform>();
//    trans.anchoredPosition = new Vector2(x, y);
//    trans.sizeDelta = new Vector2(50, 200);

//    Text newTextComp = UItextGO.AddComponent<Text>();
//    newTextComp.text = text;
//    newTextComp.fontSize = font_size;
//    newTextComp.color = text_color;

//    UItextGO.SetActive(true);

//    return UItextGO;
//}