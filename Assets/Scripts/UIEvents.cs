using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour {

    public Text txtMessage;			//Store a reference to the UI Text component which will display the 'You win' message.
    public Text txtLog;
    public Button btnExit;
    public Button btnTest;
    public InputField tbxTest;
    public TextMesh textListLogs1;
    public GameObject textListLogs;
    public GameObject contentList;
    public Text prefabText;
    public Button prefabButtonCommand;

    [Header("Menu Command")]
    public Dropdown dpnMenuCommandTest;

    public Camera MainCamera;
    
    private SaveLoadData m_scriptData;
    private List<string> m_CommandLogList = new List<string>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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

        btnTest.onClick.AddListener(LoadTest); 
    }

    private void ExitGame()
    {

        Application.Quit();
    }

    

    private void LoadTest()
    {
        int selIndex = dpnMenuCommandTest.value;
        var menuCommands = dpnMenuCommandTest.options.ToArray();

        List<string> messages = new List<string>();

        messages.Add("Sel GO: [" + tbxTest.text + "]");

        string selectCommand = menuCommands[selIndex].text.ToString();

        Debug.ClearDeveloperConsole();
        Debug.Log(">>>>>  COMMAND >>>>> " + selectCommand);
        CommandExecute(selectCommand);

        
        CreateCommandLogButton(selectCommand, Color.white);

        //txtMessage.text = string.Join("\n", messages.ToArray()); // "Selected: [" + tbxTest.text + "]"; 
        txtLog.text = string.Join("\n", messages.ToArray());
        Storage.Instance.SelectGameObjectID = tbxTest.text;
    }

    private void CommandExecute(string selectCommand)
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
                txtMessage.text = "TartgetPositionAll...";
                //Storage.Instance.TartgetPositionAll();
                Storage.Instance.IsTartgetPositionAll = true;
                break;
            case "LoadXML":
                txtMessage.text = "LoadXML...";
                Storage.Instance.LoadData();
                break;
            default:
                Debug.Log("################ EMPTY COMMAND : " + selectCommand);
                break;
        }

        //m_CommandLogList.Add(selectCommand);

    }

    public void CreateCommandLogText(string p_text, Color color)
    {
        

        //string nameGO = "text" + p_text.Replace(" ", "-");
        Vector3 pos = new Vector3(0, 0, 0);
        Text resGO = (Text)Instantiate(prefabText, pos, Quaternion.identity);
        //resGO.name = nameGO;
        resGO.text = p_text;
        resGO.transform.SetParent(contentList.transform);
    }

    public void CreateCommandLogButton(string p_text, Color color)
    {
        string nameBtn = "ButtonCommand" + p_text;

        //GameObject[] findObjects = GameObject.FindGameObjectsWithTag("PrefabCommandButton");
        GameObject findObjects = GameObject.Find(nameBtn); ;
        if (findObjects == null)
        {
            Vector3 pos = new Vector3(0, 0, 0);
            Button resGO = (Button)Instantiate(prefabButtonCommand, pos, Quaternion.identity);
            resGO.GetComponent<Text>().text = p_text;
            resGO.transform.SetParent(contentList.transform);
            resGO.name = nameBtn;
            resGO.onClick.AddListener(delegate
            {
                CommandExecute(p_text);
            });
        }
        else{
            CreateCommandLogText(p_text, Color.white);
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

}
