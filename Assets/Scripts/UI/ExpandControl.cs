using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class ExpandControl : MonoBehaviour {

    public GameObject ContentExpandList;
    public GameObject ButtonExpandObj;
    public GameObject ButtonCloseObj;
    public GameObject PanelContentExpandObj;
    public GameObject ScrollListBoxPerson;

    private GameObject gobjExpandLast;
    private bool m_isColorAlert = false;

    //textExpanderButton.text = tittle;
    //private string m_TittleExpand;
    Text _textExpanderButton;

    public string TittleExpand
    {
        get {
            if(_textExpanderButton==null)
                return "Error";
            return _textExpanderButton.text;
        }
        set {
            _textExpanderButton = ButtonExpand.GetComponentInChildren<Text>();
            if (_textExpanderButton == null)
            {
                Debug.Log("########### textBlock ButtonExpand GetComponent<Text> is Empty");
                return;
            }
            _textExpanderButton.text = value;
        }
    }

    public ModelNPC.ObjectData DataObject;

    public bool IsAlert
    {
        get
        {
            if (!string.IsNullOrEmpty(ID))
            {
                var findData = Storage.Person.GetFindPersonsDataForName(ID);
                if (findData == null)
                {
                    //SetColorText(UIEvents.ColorAlert);
                    return true;
                }
                else
                {
                    //SetColorText(UIEvents.ColorExpClose);
                    DataObject = findData.DataObj;
                }
            }
            return false;    

            //return m_gobjObservable == null;
        }
    }

    private bool m_isExpand = false;
    public bool IsOpen
    {
        get
        {
            return m_isExpand;
        }
    }


    //private GameObject m_gobjObservable;
    public string ID = "";
    private string m_storeNameObservable = "";

    public string GetName
    {
        get
        {
            GameObject m_gobjObservable = null;
            if (!string.IsNullOrEmpty(ID))
            {
                var findData = Storage.Person.GetFindPersonsDataForName(ID);
                if(findData==null)
                {
                    Debug.Log("############# Not find person id=" + ID);
                    return "ERROR_ME_DESTROY";
                }
                else
                {
                    DataObject = findData.DataObj;
                }
                
                m_gobjObservable = GameObject.Find(findData.DataObj.NameObject);
                if (m_gobjObservable == null)
                {
                    SetColorText(UIEvents.ColorAlert);

                    //Debug.Log("############# gobjObservable == null     OLD NAME: " + m_storeNameObservable + "     -- " + this.gameObject.name);
                    //if (!string.IsNullOrEmpty(m_storeNameObservable))
                    //    Storage.Log.GetHistory(m_storeNameObservable);
                    return findData.DataObj.NameObject;
                    
                }
            }
            else
            {
                return "ERROR_ME_DESTROY";
            }
            StoreNameObservable = m_gobjObservable.gameObject.name;
            TittleExpand = StoreNameObservable;

            if(m_isColorAlert)
                SetColorText(UIEvents.ColorExpClose);
            
            return m_gobjObservable.name;
        }
    }

    public string StoreNameObservable
    {
        get
        {
            return m_storeNameObservable;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                return;

            m_storeNameObservable = value;
        }

    }

    public Button ButtonExpand
    {
        get
        {
            return ButtonExpandObj.GetComponent<Button>();
        }
    }
    public Button ButtonClose
    {
        get
        {
            return ButtonCloseObj.GetComponent<Button>();
        }
    }


    public Image PanelContentExpand
    {
        get
        {
            return PanelContentExpandObj.GetComponent<Image>();
        }
    }

    void Start ()
    {
        //ButtonExpand.onClick.AddListener(ExpandPanelOn);
        ButtonExpand.onClick.AddListener(ButtonExpandOnClick);

        ButtonClose.onClick.AddListener(delegate
        {
            Destroy(this.gameObject);
        });
    }

    // Update is called once per frame
    void Update () {
		
	}



    private void ButtonExpandOnClick()
    {
        SelectedObserver();
        ColorUnsetAllExpand();
        ExpandPanelOn(isOnly: false);
    }

    public void ColorUnsetAllExpand()
    {
        if (IsOpen)
            return;

            var listExpandPersonControls = GameObject.FindGameObjectsWithTag("ExpandPersonControl");
        foreach (var exp in listExpandPersonControls)
        {
            if (exp.name == "PrefabExpandPanel")
                continue;
            ExpandControl scriptExp = exp.GetExpandControl();
            scriptExp.SetColorText(UIEvents.ColorExpClose);

            string nameExp = scriptExp.GetName;
            scriptExp.ExpandPanelOn(true, p_isOpen: false);
        }

        SetColorText(UIEvents.ColorExpOpen);
    }

    public void ExpandPanelOn(bool isOnly = false, bool? p_isOpen = null)
    {
        bool isSet = p_isOpen.HasValue ? true : false;
        //Debug.Log("ExpandPanelOn (" + this.name + ")");

        m_isExpand = PanelContentExpandObj.activeSelf;
        if(isSet)
            m_isExpand = p_isOpen.Value;
        else
            m_isExpand = !m_isExpand;

        PanelContentExpandObj.SetActive(m_isExpand);

        if (isOnly)
            return;

        var listExpandPersonControls = GameObject.FindGameObjectsWithTag("ExpandPersonControl");
        gobjExpandLast = listExpandPersonControls[listExpandPersonControls.Length-1];

        var listComPerson = (GameObject)GameObject.FindGameObjectWithTag("ListCommandPerson");
        if (listComPerson!=null && listComPerson.activeSelf)
            StartCoroutine(UpdatePositionExpandPanels());
    }

    IEnumerator UpdatePositionExpandPanels()
    {
        var scriptExpand = gobjExpandLast.GetComponent<ExpandControl>();
        if (scriptExpand == null)
        {
            Debug.Log("############# ExpandPanelOn scriptExpand is empty " + gobjExpandLast.name);
            yield break;
        }
        else
        {
            //var tittle = scriptExpand.TittleExpand;
        }

        yield return new WaitForEndOfFrame();

        scriptExpand.ExpandPanelOn(true);
        gobjExpandLast.transform.SetAsLastSibling();

        //yield return new WaitForEndOfFrame();

        scriptExpand.ExpandPanelOn(true);
        gobjExpandLast.transform.SetAsLastSibling();

        //Debug.Log("############# ExpandPanelOn scriptExpand is empty " + gobjExpandLast.name);
    }

    public void SetGameObject(GameObject gobjObservable, string maneTittle)
    {
        if(gobjObservable==null)
        {
            Debug.Log("...... ExpandControl: SetGameObject is Empty : " + maneTittle);
            return;
        }

        string nameObservable = gobjObservable.name;
        //Debug.Log("ExpandControl: SetGameObject : " + nameObservable);

        //m_gobjObservable = gobjObservable;

        ID = Helper.GetID(gobjObservable.name);
    }



    // ADD DATA NPC
    public void AddList(string tittle, List<string> listText, List<string> listCommand,  GameObject m_gobjObservable)
    {
        TittleExpand = tittle;

        if (ContentExpandList == null)
        {
            Debug.Log("########### ContentExpandList is Empty ");
            return;
        }

        Transform transExpandButton = ButtonExpand.transform;
;

        if (transExpandButton == null)
        {
            Debug.Log("########### textBlock Expand is Empty");
            return;
        }
      
        foreach (string selectCommand in listCommand)
        {
            //Debug.Log("AddList CreateCommandLogButton : " + selectCommand);
            Storage.Events.CreateCommandLogButton(selectCommand, Color.white, ContentExpandList.transform, m_gobjObservable, false, this);
            //Storage.Events.CreateCommandLogButton(selectCommand, Color.white, ContentExpandList.transform, m_gobjObservable, true);
        }
        foreach (string text in listText)
        {
            //Debug.Log("AddList CreateCommandLogText : " + text);
            Storage.Events.CreateCommandLogText(text, Color.white, ContentExpandList.transform);
        }
    }

    
    public void SetColorText(string strColor)
    {
        if (IsAlert)
        {
            m_isColorAlert = true;
            ButtonExpand.SetColor("", strColorText: UIEvents.ColorAlert);
            return;
        }
        if (strColor == UIEvents.ColorAlert)
            m_isColorAlert = true;
        else
            m_isColorAlert = false;

        ButtonExpand.SetColor("", strColorText: strColor);
        //ButtonExpand.SetColor(strColor, strColor);
    }

    public GameObject SelectedObserver()
    {
        GameObject m_gobjObservable = null;
        if (!string.IsNullOrEmpty(ID))
        {
            var findData = Storage.Person.GetFindPersonsDataForName(ID);
            if (findData == null)
            {
                Debug.Log("############# Not find person id=" + ID);
            }
            DataObject = findData.DataObj;
            m_gobjObservable = GameObject.Find(findData.DataObj.NameObject);
        }

        if (m_gobjObservable == null)
        {
            //Storage.Events.ListLogAdd = "SelectedObserver EMPTY(" + this.gameObject.name + ") : " + m_storeNameObservable;
            Storage.Events.ListLogAdd = "EMPTY(" + this.gameObject.name + ") : " + m_storeNameObservable;
            SetColorText(UIEvents.ColorAlert);
            return null;
        }

        //if (m_isColorAlert)
        SetColorText(UIEvents.ColorExpOpen);

        //Storage.Events.ListLogAdd = "SELECTED --- " + GetName.Replace("_", " ");
        Storage.Instance.SelectGameObjectID = Helper.GetID(GetName);
        return m_gobjObservable;
    }
}
