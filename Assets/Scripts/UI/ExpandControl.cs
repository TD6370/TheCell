using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class ExpandControl : MonoBehaviour {

    public GameObject ContentExpandList;
    public GameObject ButtonExpandObj;
    public GameObject PanelContentExpandObj;

    public string TittleExpand { get; set; }

    private bool m_isExpand = false;
    private GameObject m_gobjObservable;

    public Button ButtonExpand
    {
        get
        {
            return ButtonExpandObj.GetComponent<Button>();
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
    void Start ()
    {
        ButtonExpand.onClick.AddListener(ExpandPanelOn);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ExpandPanelOn()
    {
        m_isExpand = PanelContentExpandObj.activeSelf;
        m_isExpand = !m_isExpand;
        PanelContentExpandObj.SetActive(m_isExpand);
        PanelContentExpandObj.transform.SetAsLastSibling();
        //------------------
        var listExpandPersonControls =  GameObject.FindGameObjectsWithTag("ExpandPersonControl");
        //if(listExpandPersonControls!=null)
        //    Debug.Log("Update transform ExpadPersonControl : " + listExpandPersonControls.Length);

        foreach (var gobj in listExpandPersonControls)
        {
            //Debug.Log("Update transform ExpadPersonControl "+ gobj.name);

            ExpandControl scriptExpand = gobj.GetComponent<ExpandControl>();
            if(scriptExpand == null)
            {
                Debug.Log("############# ExpandPanelOn scriptExpand is empty " + gobj.name);
                return;
            }
            else
            {
                var tittle = scriptExpand.TittleExpand;
                //Debug.Log("Update transform ExpadPersonControl  " + gobj.name + "  tittle:" + tittle);
            }
            

            gobj.transform.SetAsLastSibling();
        }

        //for (int i = 0; i < soldiers.Count(); i++)
        //{
        //    Vector3 position = soldiers[i].mono.transform.position;
        //    position.y = soldierPrefab.transform.position.y + (i * rowOffset);
        //    soldiers[i].mono.transform.position = position;
        //}
    }

    public void SetGameObject(GameObject gobjObservable, string maneTittle)
    {
        if(gobjObservable==null)
        {
            Debug.Log("...... ExpandControl: SetGameObject is Empty : " + maneTittle);
            return;
        }

        string nameObservable = gobjObservable.name;
        Debug.Log("ExpandControl: SetGameObject : " + nameObservable);

        m_gobjObservable = gobjObservable;
    }

    //string tittle, List<string> listText, List<Button> listButtonCommand{
    //public void AddList(text, Color.white, contentExpandGO.transform)
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

        foreach (string text in listText)
        {
            //Debug.Log("AddList CreateCommandLogText : " + text);
            Storage.Events.CreateCommandLogText(text, Color.white, ContentExpandList.transform);
        }
        foreach (string selectCommand in listCommand)
        {
            //Debug.Log("AddList CreateCommandLogButton : " + selectCommand);
            Storage.Events.CreateCommandLogButton(selectCommand, Color.white, ContentExpandList.transform, m_gobjObservable);
        }
    }
}
