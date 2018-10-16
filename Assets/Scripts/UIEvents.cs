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
    [Header("Menu Command")]
    public Dropdown dpnMenuCommandTest;

    public Camera MainCamera;
    
    private SaveLoadData m_scriptData;

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

        //var selText = dpnMenuCommandTest.itemText.text;
        //var selTag = dpnMenuCommandTest.itemText.tag;


        List<string> messages = new List<string>();

        messages.Add("Sel GO: [" + tbxTest.text + "]");
        //messages.Add("Sel comm: value " + selVal.ToString() );
        //messages.Add("Sel comm: text " + selText.ToString());
        //messages.Add("Sel comm: Tag " + selTag.ToString());
        //foreach (var item in options)
        //{
        //    messages.Add("options: " + item.text.ToString());
        //}

        string selectCommand = menuCommands[selIndex].text.ToString();

        Debug.ClearDeveloperConsole();
        Debug.Log(">>>>>  COMMAND >>>>> " + selectCommand);

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
                //txtMessage.text = "Level saving...";
                //m_scriptData.SaveLevel();
                //txtMessage.text = "Level loading...";
                //Storage.Instance.LoadLevels();
                //txtMessage.text = "Level loaded.";
                StartCoroutine(StartReloadWorld());
                break;
        }

        //txtMessage.text = string.Join("\n", messages.ToArray()); // "Selected: [" + tbxTest.text + "]"; 
        txtLog.text = string.Join("\n", messages.ToArray());
        Storage.Instance.SelectGameObjectID = tbxTest.text;
    }

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
