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

    public string TittleExpand { get; set; }

    public bool IsAlert
    {
        get
        {
            return m_gobjObservable == null;
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


    private GameObject m_gobjObservable;
    private string m_storeNameObservable = "";

    public string GetName
    {
        get
        {
            if (m_gobjObservable == null)
            {
                if (!string.IsNullOrEmpty(m_storeNameObservable))
                {
                    Debug.Log("############# GetName Find Observable on name: " + m_storeNameObservable);
                    m_gobjObservable = GameObject.Find(m_storeNameObservable);
                }
                if (m_gobjObservable == null)
                {
                    Debug.Log("############# gobjObservable == null     OLD NAME: " + m_storeNameObservable + "     -- " + this.gameObject.name);
                    if (!string.IsNullOrEmpty(m_storeNameObservable))
                        Storage.Log.GetHistory(m_storeNameObservable);
                    return "ERROR_ME_DESTROY";
                }
            }
            StoreNameObservable = m_gobjObservable.gameObject.name;
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

    //public Image PanelContentExpand
    //{
    //    get
    //    {
    //        return ContentExpandList.GetComponent<Image>();
    //    }
    //}

    // Use this for initialization

    //public bool IsOpen
    //{
    //    get
    //    {
    //        return PanelContentExpandObj.activeSelf;
    //    }
    //}


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

    ExpandControl scriptExpand;
    GameObject gobjExpandLast;
    float addHeight;

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

            //string nameExp = scriptExp.GetName;
            //scriptExp.ExpandPanelOn(true, p_isOpen: false);
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

        m_gobjObservable = gobjObservable;
    }



    // ADD DATA NPC
    public void AddList(string tittle, List<string> listText, List<string> listCommand)
    {
        TittleExpand = tittle;

        //var transContentExpandGO =  ContentExpandList;
;
        if (ContentExpandList == null)
        {
            Debug.Log("########### ContentExpandList is Empty ");
            return;
        }

        //GameObject contentExpandGO = transContentExpandGO.gameObject;
        //if (contentExpandGO == null)
        //{
        //    Debug.Log("########### Content Expand is Empty");
        //    return;
        //}

        Transform transExpandButton = ButtonExpand.transform;
;

        if (transExpandButton == null)
        {
            Debug.Log("########### textBlock Expand is Empty");
            return;
        }

        //Text textExpanderButton = ButtonExpand.GetComponent<Text>();
        //textExpanderButton
        Text textExpanderButton = ButtonExpand.GetComponentInChildren<Text>();
        if (textExpanderButton == null)
        {
            Debug.Log("########### textBlock ButtonExpand GetComponent<Text> is Empty");
            return;
        }


        //SET TITTLE
        textExpanderButton.text = tittle;

       
        foreach (string selectCommand in listCommand)
        {
            //Debug.Log("AddList CreateCommandLogButton : " + selectCommand);
            Storage.Events.CreateCommandLogButton(selectCommand, Color.white, ContentExpandList.transform, m_gobjObservable, false);
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
            ButtonExpand.SetColor("", strColorText: UIEvents.ColorAlert);
            return;
        }

        //Debug.Log("SetColorText Me : " + this.name + " color: " + strColor);

        ButtonExpand.SetColor("", strColorText: strColor);
        //ButtonExpand.SetColor(strColor, strColor);
    }

    public void SelectedObserver()
    {
        if (m_gobjObservable == null)
        {
            if (!string.IsNullOrEmpty(m_storeNameObservable))
            {
                Debug.Log("############# GetName Find Observable on name: " + m_storeNameObservable);
                m_gobjObservable = GameObject.Find(m_storeNameObservable);
            }

            if (m_gobjObservable == null)
            {
                //Storage.Events.ListLogAdd = "SelectedObserver EMPTY(" + this.gameObject.name + ") : " + m_storeNameObservable;
                Storage.Events.ListLogAdd = "EMPTY(" + this.gameObject.name + ") : " + m_storeNameObservable;
                SetColorText(UIEvents.ColorAlert);
                return;
            }
        }

        //Storage.Events.ListLogAdd = "SELECTED --- " + GetName.Replace("_", " ");
        Storage.Instance.SelectGameObjectID = Helper.GetID(GetName);
    }
}
