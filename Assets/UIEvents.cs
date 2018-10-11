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
            m_scriptData.SaveLevel();
            txtMessage.text = "Level saved...";
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
        txtMessage.text = "Selected: [" + tbxTest.text + "]"; 
        Storage.Instance.SelectGameObjectID = tbxTest.text;
    }

    public void SetTestText(string text)
    {
        tbxTest.text = text;
    }

}
