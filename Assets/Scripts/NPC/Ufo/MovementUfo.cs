using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUfo : MonoBehaviour {

    private Coroutine moveObject;
    private Material m_material;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D _rb2d;
    private string objID;

    private UIEvents _scriptUIEvents;

    string testId;

    void Awake()
    {
    }

	// Use this for initialization
	void Start () {

        InitData();
        StartCoroutine(MoveObjectToPosition());
        objID = Storage.GetID(this.name);
        if (string.IsNullOrEmpty(objID))
            objID = "Empty";

        GameObject UIcontroller = GameObject.FindWithTag("UI");
        if (UIcontroller == null)
            Debug.Log("########### MovementUfo UIcontroller is Empty");
        _scriptUIEvents = UIcontroller.GetComponent<UIEvents>();
        if (_scriptUIEvents == null)
            Debug.Log("########### MovementUfo scriptUIEvents is Empty");
	}

    // Update is called once per frame
    void Update()
    {
    }

    private void InitData()
    {
        m_material = this.GetComponent<Renderer>().material;
        m_spriteRenderer = this.GetComponent<SpriteRenderer>();
        //@PD@ m_scriptPersonal = this.GetComponent<PersonalData>();
        _rb2d = this.GetComponent<Rigidbody2D>();
    }

    IEnumerator ChangeColor(){
        
        while(true){

            ChangeRandomColor();

            yield return new WaitForSeconds(2f);
        }
    }

    private void ChangeRandomColor()
    {
        //material.color = new Color(Random.value, Random.value, Random.value, 1);
        m_spriteRenderer.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
    }

    IEnumerator MoveObject()
    {
        int distantion = 0;
        float x = transform.position.x;
        int compas=1;

        while (true)
        {
            distantion++;
            if (distantion > 200)
            {
                distantion = 0;
                compas *= -1;
                //yield break;
            }

            transform.Translate(compas * Time.deltaTime, 0, 0);
            yield return null;
            //yield return new WaitForSeconds(1);
        }
    }

    //@SAVE@
    SaveLoadData.GameDataUfo dataUfo;
    string newName = "";

    IEnumerator MoveObjectToPosition()
    {
        Vector3 lastPositionForLock = transform.position;
        Vector3 lastPositionForMoveField = transform.position;

        

        int stepTest = 0;
        int stepLimitTest = 10;
        float minDist = 0.005f;  //0.01f;

        int speed = 2;
        float step = speed * Time.deltaTime;


        if (!Storage.IsDataInit(this.gameObject))
        {
            //yield return null;
            yield break;
        }

        //@SAVE@  var dataUfo = FindObjectData() as SaveLoadData.GameDataUfo;
        dataUfo = FindObjectData() as SaveLoadData.GameDataUfo;

        while (true)
        {
            if (dataUfo == null)
            {
                Debug.Log("########################## UFO MoveObjectToPosition dataUfo is EMPTY");
                yield break;
            }

            stepTest++;
            if (stepTest > stepLimitTest)
            {
                float distLock = Vector3.Distance(lastPositionForLock, transform.position);
                if (distLock < minDist)
                {
                    //Debug.Log("MoveObjectToPosition ------ UFO LOCK !!!!  > " + distLock);
                    dataUfo.SetTargetPosition();
                }
                lastPositionForLock = transform.position;
                stepTest = 0;
            }

            Vector3 targetPosition  = dataUfo.TargetPosition;
            Vector3 pos = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (_rb2d != null)
            {
                _rb2d.MovePosition(pos);
            }
            else
            {
                //@!@.2
                Debug.Log("UFO MoveObjectToPosition Set position 2 original........");
                transform.position = pos; 
            }

            if (!string.IsNullOrEmpty(newName) && newName != this.gameObject.name)
            {
                Debug.Log("################## ERROR MoveObjectToPosition ===========PRED========= rael name: " + this.gameObject.name + "  new name: " + newName);
            }

            string oldName = this.name;

            //+++++++++++++++++++++++
            string resName = dataUfo.NextPosition(this.gameObject);
            //+++++++++++++++++++++++

            if (!string.IsNullOrEmpty(resName))
            {
                if (oldName != resName)
                {
                    dataUfo = FindObjectData() as SaveLoadData.GameDataUfo;
                    if (dataUfo == null)
                    {
                        Debug.Log("################## ERROR MoveObjectToPosition dataUfo is Empty   GO:" + this.gameObject.name);
                        yield break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(resName))
                newName = resName;

            if (!string.IsNullOrEmpty(resName))
            {
                if (resName != this.gameObject.name)
                {
                    Debug.Log("ERROR =================================== MoveObjectToPosition ===========POST========= rael name: " + this.gameObject.name + "  new name: " + newName);
                }
                if (this.gameObject.name != newName)
                {
                    Debug.Log("ERROR **** MoveObjectToPosition POST ==== rael name: " + this.gameObject.name + "  new name: " + newName + "      old name:" + oldName);
                    this.gameObject.name = newName;
                }
            }

            float dist = Vector3.Distance(targetPosition, transform.position);
            //Debug.Log("MoveObjectToPosition ------ UFO distance to point  > " + dist);
            if (dist < minDist)
            {
                dataUfo.SetTargetPosition();
            }

            yield return null;
        }
    }

    private SaveLoadData.GameDataUfo FindObjectData()
    {
        var dataUfo = SaveLoadData.FindObjectData(this.gameObject) as SaveLoadData.GameDataUfo;


        if (dataUfo == null)
        {
            Debug.Log("#################### rror UFO MoveObjectToPosition dataUfo is Empty !!!!");
            return null;
        }

        if (dataUfo.NameObject != this.name)
        {
            Debug.Log("#################### Error UFO MoveObjectToPosition dataUfo: " + dataUfo.NameObject + "  GO: " + this.name);
            return null;
        }

        if (dataUfo.TargetPosition == new Vector3(0, 0, 0))
        {
            Debug.Log("#################### Error UFO dataUfo.TargetPosition is zero !!!!");
            return null;
        }

        return dataUfo;
    }

    //@SAVE@
    public void UpdateData()
    {
        //Debug.Log("_____________GameObject.UpdateData ________________" + this.name);
        dataUfo = FindObjectData() as SaveLoadData.GameDataUfo;
        newName = dataUfo.NameObject;
    }
    
    //--------------------
    //public string text = "TTTTTTTTTTTTTTTTTTTTTTTTTTT1";
    //public int textSize = 14;
    //public Font textFont;
    //public Color textColor = Color.white;
    //public float textHeight = 1.15f;
    //public bool showShadow = true;
    //public Color shadowColor = new Color(0, 0, 0, 0.5f);
    //public Vector2 shadowOffset = new Vector2(1, 1);
    //--------------------

    void OnGUI()
    {
        //-------------------------
        //GUI.depth = 9999;

        GUIStyle style = new GUIStyle();
        style.fontSize = 13;
        //style.richText = true;
        //if (textFont) style.font = textFont;
        style.normal.textColor = Color.black;
        //style.alignment = TextAnchor.MiddleCenter;

        //GUIStyle shadow = new GUIStyle();
        //shadow.fontSize = textSize;
        //shadow.richText = true;
        //if (textFont) shadow.font = textFont;
        //shadow.normal.textColor = shadowColor;
        //shadow.alignment = TextAnchor.MiddleCenter;

        //Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + 50, transform.position.z);
        //Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        //screenPosition.y = Screen.height - screenPosition.y;

        //if (showShadow) GUI.Label(new Rect(screenPosition.x + shadowOffset.x, screenPosition.y + shadowOffset.y, 0, 0), textShadow, shadow);
        //GUI.Label(new Rect(screenPosition.x, screenPosition.y, 0, 0), text, style);
        //-------------------------

        if (objID == Storage.Instance.SelectGameObjectID)
        {
            style.fontSize = 15;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.yellow;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Rect position1 = new Rect(screenPosition.x - 10, Screen.height - screenPosition.y - 30, 300, 100);
        //GUI.Label(position1, objID);
        GUI.Label(position1, objID, style);
    }

    //void OnBecameVisible()
    //{
    //    enabled = true;
    //}

    //void OnBecameInvisible()
    //{
    //    enabled = false;
    //}

    private void OnMouseDown()
    {
        SelectIdFromTextBox();
    }

    private void SelectIdFromTextBox()
    {
        _scriptUIEvents.SetTestText(objID);
    }
}
