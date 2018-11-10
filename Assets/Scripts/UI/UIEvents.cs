using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour {

    //public static bool IsCursorVisible = true;
    public bool IsCursorVisible = true;
    public bool IsTrackPointsVisible = false;
    public int LimitLogView = 10;

    public Text txtMessage;			//Store a reference to the UI Text component which will display the 'You win' message.
    public Text txtLog;
    public Button btnExit;
    public Button btnTest;
    public Button btnPanelPersonOpen;
    public Button btnPanelPersonClose;
    public Toggle btnShowLog;
    public InputField tbxTest;
    public GameObject PanelLog;
    public GameObject contentList;
    public GameObject ListBoxExpandPerson;
    public GameObject contentListExpandPerson;
    public GameObject PrefabExpandPanel;
    public GameObject ScrollListBoxPerson;
    public GameObject PointGO;

    public Text prefabText;
    public Button prefabButtonCommand;
    public static string ColorExpClose = "#FFFFFF";
    public static string ColorExpOpen = "#FFFA00";
    public static string ColorYelow = "#CDA143";
    public static string ColorGreen = "#468C44";
    public static string ColorAlert = "#EC4D56";

    [Header("Menu Command")]
    public Dropdown dpnMenuCommandTest;

    public Camera MainCamera;

    private SaveLoadData m_scriptData;
    //private List<string> m_CommandLogList = new List<string>();
    private List<string> m_ListLog = new List<string>();




    void Awake()
    {
        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("UIEvents: Error MainCamera null");
            return;
        }
        m_scriptData = MainCamera.GetComponent<SaveLoadData>();
        if (m_scriptData == null)
        {
            Debug.Log("UIEvents: Error scriptData is null !!!");
            return;
        }

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


        btnTest.onClick.AddListener(TestClick);


        string typePrefub = this.gameObject.tag.ToString();

        SaveLoadData.TypePrefabs typePrefab = Helper.GetTypePrefab(this.gameObject);
        if (Helper.IsTerra(typePrefab))
        {

        }

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
    }

    // Update is called once per frame
    void Update()
    {
        //AlphaTerra();
    }


    //public bool IsMeTerra = false;

    //private void AlphaTerra()
    //{


    //    string typePrefub = this.gameObject.tag.ToString();

    //    SaveLoadData.TypePrefabs typePrefab = Helper.GetTypePrefab(this.gameObject);
    //    if(Helper.IsTerra(typePrefab))
    //    {
    //        float posHeroY = Storage.PlayerController.transform.position.y;
    //        float gobjY = this.transform.position.y;
    //        float dist = Math.Abs(gobjY) - Math.Abs(posHeroY);
    //        bool isNear = false;
    //        if (dist < 5)
    //            isNear = true;

    //        if (gobjY > posHeroY && isNear)
    //        {
    //            Single _alpha = 0.5f;

    //            //this.gameObject.GetComponent<SpriteRenderer>().color;
    //            this.gameObject.SetAlpha(_alpha);
    //        }
    //    }
    //}

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


    private void ExitGame()
    {
        Storage.Player.SavePosition();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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

        
        CreateCommandLogButton(selectCommand, Color.white, contentList.transform, null, true);

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
                txtMessage.text = "...";
                break;
            case "SaveWorld":
                txtMessage.text = "Level saving...";
                m_scriptData.SaveLevel();
                txtMessage.text = "Level saved.";
                break;
            case "LoadWorld":
                //m_scriptData

                txtMessage.text = "Level loading...";
                Storage.Instance.LoadLevels();
                txtMessage.text = "Level loaded.";
                break;
            case "ReloadWorld":
                txtMessage.text = "Reload Level...";
                //txtMessage.text = "Level saving...";
                //m_scriptData.SaveLevel();
                //txtMessage.text = "Level loading...";
                //Storage.Instance.LoadLevels();
                //txtMessage.text = "Level loaded.";
                StartCoroutine(StartReloadWorld());
                break;
            case "CreateWorld":
                txtMessage.text = "Create Level...";
                Storage.Instance.CreateWorld();
                break;
            case "TartgetPositionAll":
                txtMessage.text = @"TartgetPositionAll On\Off...";
                //Storage.Instance.IsTartgetPositionAll = true;
                Storage.Instance.IsTartgetPositionAllOn();
                break;
            case "LoadXML":
                txtMessage.text = "LoadXML...";
                Storage.Instance.LoadData();
                break;
            case "Pause":
                //Debug.Log("CommandExecuteParson(" + selectCommand + ") :  " + selectCommand);
                txtMessage.text = "Game Pause...";
                Storage.GamePause = !Storage.GamePause;
                break;
            case "ClearLog":
                txtMessage.text = "Clear Log...";
                ListLogClear();
                break;
            case "LogPerson":
                txtMessage.text = "Create Expand Pesron Test...";

                //CreateCommandLogButton("TEST", Color.gray, contentListExpandPerson.transform);
                //break;
                AddExpandPerson("Tittle TEST",
                    new List<string> { "Param1: qqqq", "Param2: ssss", "Param3: dddd" }, 
                    new List<string> { "Pause", "Kill" },
                    gobjObservable: null);
                break;
            case "CursorSelection":
                txtMessage.text = @"Cursor selection On\Off...";
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
                Storage.GridData.CreateDataGamesObjectsExtremalWorld();
                break;
            default:
                Debug.Log("################ EMPTY COMMAND : " + selectCommand);
                break;
        }

        ListLogAdd = "CALL >> " + selectCommand;

        //m_CommandLogList.Add(selectCommand);

    }


    public void CommandExecutePerson(string selectCommand, GameObject gobjObservable)
    {
        if(gobjObservable == null)
        {
            Debug.Log("############ CommandExecuteParson(" + selectCommand  + ") : gobjObservable is Empty");
            return;
        }
        MovementNPC movem = gobjObservable.GetComponent<MovementNPC>();

        switch (selectCommand)
        {
            case "None":
                txtMessage.text = "...";
                break;
            case "Pause":
                txtMessage.text = "Level saving...";
                if(movem==null)
                {
                    Debug.Log("############ CommandExecuteParson(" + selectCommand  + ") : " + gobjObservable.name  + " MovementNPC is Empty");
                    return;
                }
                Debug.Log("CommandExecuteParson(" + selectCommand + ") : " + gobjObservable.name + " : " + selectCommand);
                movem.Pause();
                break;
            case "Kill":
                //EventsObject scriptEvents = gobjObservable.GetComponent<EventsObject>();
                //if (scriptEvents == null)
                //{
                //    Debug.Log("############ CommandExecuteParson(" + selectCommand + ") : " + gobjObservable.name + " scriptEvents is Empty");
                //    return;
                //}
                //scriptEvents.Kill();
                Storage.Instance.AddDestroyGameObject(gobjObservable);
                break;
            case "StartTrack":
                if (movem == null)
                {
                    Debug.Log("############ CommandExecuteParson(" + selectCommand + ") : " + gobjObservable.name + " MovementNPC is Empty");
                    return;
                }
                movem.TrackOn();
                break;
            default:
                Debug.Log("################ EMPTY COMMAND : " + selectCommand);
                break;
        }

    }

    public void CursorClickAction(Vector2 posCursorToField, string _fieldCursor)
    {
        string _infoPoint = "Cursor :" + posCursorToField + "\nfind:" + _fieldCursor;

        if (Storage.PaletteMap.IsCursorOn)
        {
            Storage.Person.VeiwCursorGameObjectData(_fieldCursor);
        }

        if (Storage.PaletteMap.IsPaintsOn)
        {
            Storage.PaletteMap.PaintAction();
        }

        if (Storage.Instance.IsTartgetPositionAll)
            Storage.Person.SetTartgetPositionAll(posCursorToField);
    }

    public void CreateCommandLogText(string p_text, Color color, Transform p_parent)
    {
        

        //string nameGO = "text" + p_text.Replace(" ", "-");
        Vector3 pos = new Vector3(0, 0, 0);
        Text resGO = (Text)Instantiate(prefabText, pos, Quaternion.identity);
        //resGO.name = nameGO;
        resGO.text = p_text;
        resGO.transform.SetParent(p_parent);

        //Debug.Log("CreateCommandLogText : " + p_text);
    }

    public void CreateCommandLogButton(string p_text, Color color, Transform p_parent, GameObject gobjObservable = null, bool isValidExistCommand = false)
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

            //ADD EVENT COMMAND
            buttonCommand.onClick.AddListener(delegate
            {
                if (isPersonComm)
                {
                    if(gobjObservable ==null)
                    {
                        buttonCommand.SetColor(ColorAlert);// "#EC4D56");
                        Debug.Log("buttonCommand.onClick isPersonComm gobjObservable == null");
                        return;
                    }
                    CommandExecutePerson(textBtn, gobjObservable);
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

    IEnumerator StartReloadWorld()
    {
        bool isSave = false;
        while(!isSave)
        {
            yield return null;

            yield return new WaitForSeconds(0.3f);

            txtMessage.text = "Level saving...";
            m_scriptData.SaveLevel();

            yield return new WaitForSeconds(0.3f);

            isSave = true;
        }
        txtMessage.text = "Level loading...";
        Storage.Instance.LoadLevels();
        txtMessage.text = "Level loaded.";
    }

    public void SetTestText(string text)
    {
        tbxTest.text = text;
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
            //Debug.Log("ME " + newNameExpand + "  IsOpen=" + scriptExp.IsOpen);
            //else
            //    scriptExp.SetColorText("#FFFA00");

        }
        //ExpandControl scriptExpand = resultFind.GetExpandControl();
        //scriptExpand.SetColorText("#FFFA00");
        //scriptExpand.ExpandPanelOn(p_isOpen: true);
        //}

        if (resultFind != null)
        {
            ExpandControl scriptExpand = resultFind.GetExpandControl();
            scriptExpand.SetColorText(ColorExpOpen);
            scriptExpand.ExpandPanelOn(p_isOpen: true);
            return;
        }

        Vector3 pos = new Vector3(0, 0, 0);
        GameObject expandGO = (GameObject)Instantiate(PrefabExpandPanel, pos, Quaternion.identity);

        //string index = UnityEngine.Random.Range(0, 10).ToString();
        //var gobjName = (gobjObservable == null)? index : gobjObservable.name;
        //expandGO.name = "ExpandPanel" + tittle.Replace(" ","_") + "" + gobjName + listText.Count;
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
        scriptEvents.AddList(tittle, listText, listCommand);

        //scriptEvents = resultFind.GetExpandControl();
        scriptEvents.SetColorText(ColorExpOpen);
        scriptEvents.ExpandPanelOn(p_isOpen: true);

        //var scroll = ScrollListBoxPerson;
        //var scrollbar = expandGO.GetComponentInChildren<Scrollbar>();
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
                CreateCommandLogButton(nameCommand, Color.white, contentList.transform, null, true);
            }

            //Debug.Log("---- LoadCommandTool Loaded..........");
        }
    }

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
        foreach (var posStr in positInput)
        {
            float posInt = -1f;
            if(x==-1f)
            {
                if(!float.TryParse(posStr, out x))
                {
                    Debug.Log("######## GetPositTeleport posStr X not valid: [" + posStr + "]");
                    break;
                }
            }
            else
            {
                if (!float.TryParse(posStr, out y))
                {
                    Debug.Log("######## GetPositTeleport posStr Y not valid: [" + posStr + "]");
                    break;
                }
            }
        }
        if(x!=-1f && y!=-1f)
        {
            posTeleport = new Vector2(x, y);
        }
        else
        {
            Debug.Log("######## GetPositTeleport NOT teleporting !!!");
        }
        return posTeleport;
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

