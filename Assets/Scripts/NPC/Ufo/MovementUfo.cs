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
    //@SAVE@
    private ModelNPC.GameDataUfo _dataUfo;
    string testId;
    //string testNewName = "";
    string _resName = "";

    void Awake()
    {
    }

	// Use this for initialization
	void Start () {

        InitData();
        StartCoroutine(MoveObjectToPosition());
        objID = Helper.GetID(this.name);
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

    
    

    IEnumerator MoveObjectToPosition()
    {
        Vector3 lastPositionForLock = transform.position;
        Vector3 lastPositionForMoveField = transform.position;

        int stepTest = 0;
        int stepLimitTest = 10;
        float minDist = 0.005f;  //0.01f;

        int speed = 5;
        float step = speed * Time.deltaTime;


        if (!Helper.IsDataInit(this.gameObject))
        {
            //yield return null;
            yield break;
        }

        //@SAVE@  var dataUfo = FindObjectData() as SaveLoadData.GameDataUfo;
        _dataUfo = FindObjectData("MoveObjectToPosition Init") as ModelNPC.GameDataUfo;

        while (true)
        {
            if (Storage.Instance.IsLoadingWorld)
            {
                Debug.Log("_______________ LOADING WORLD ....._______________");
                yield return null;
            }

            if (Storage.Instance.IsCorrectData)
            {
                Debug.Log("_______________ RETURN CorrectData ON CORRECT_______________");
                yield return null;
            }

            if (_dataUfo == null)
            {
                Debug.Log("########################## UFO MoveObjectToPosition dataUfo is EMPTY");
                //Debug.Log("*** DESTROY : " + this.gameObject.name);
                //Storage.Instance.AddDestroyRealObject(this.gameObject);
                //@CD@
                Storage.Fix.CorrectData(null, this.gameObject, "MoveObjectToPosition");

                yield break;
            }

            stepTest++;
            if (stepTest > stepLimitTest)
            {
                float distLock = Vector3.Distance(lastPositionForLock, transform.position);
                if (distLock < minDist)
                {
                    //Debug.Log("MoveObjectToPosition ------ UFO LOCK !!!!  > " + distLock);
                    _dataUfo.SetTargetPosition();
                }
                lastPositionForLock = transform.position;
                stepTest = 0;
            }

            Vector3 targetPosition  = _dataUfo.TargetPosition;
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

            //+++++++++++ RESAVE Next Position ++++++++++++
            bool res = ResavePositionData();

            if (!res)
            {
                Debug.Log("########### STOP MOVE ON ERROR MOVE");
                Destroy(this.gameObject);
                yield break;
            }

            float dist = Vector3.Distance(targetPosition, transform.position);
            //Debug.Log("MoveObjectToPosition ------ UFO distance to point  > " + dist);
            if (dist < minDist)
            {
                _dataUfo.SetTargetPosition();
            }

            yield return null;
        }
    }


    

    private bool ResavePositionData()
    {
        if (Storage.Instance.IsCorrectData)
        {
            Debug.Log("_______________ResavePositionData     RETURN CorrectData ON CORRECT_______________");
            return true;
        }

        //+++++++++++ RESAVE Next Position ++++++++++++
        if (!string.IsNullOrEmpty(_resName) && _resName != this.gameObject.name)
        {
            Debug.Log("################## ERROR MoveObjectToPosition ===========PRED========= rael name: " + this.gameObject.name + "  new name: " + _resName);
        }

        string oldName = this.gameObject.name;
        _resName = _dataUfo.NextPosition(this.gameObject);

        if (_resName == "Error")
            return false;

        //if (_resName == "Update")
        //{
        //    //CORRECT
        //    Debug.Log("++++ ResavePositionData CORRECT +++ (" + this.gameObject.name + ")-- call FindObjectData");
        //    _dataUfo = FindObjectData("ResavePositionData >> CORRECT") as SaveLoadData.GameDataUfo;
        //    oldName = this.gameObject.name;
        //}

        if (!string.IsNullOrEmpty(_resName))
        {
            if (oldName != _resName)
            {
                string callInfo = "ResavePositionData >> oldName(" + oldName + ") != _resName(" + _resName + ")";
                _dataUfo = FindObjectData(callInfo) as ModelNPC.GameDataUfo;
                if (_dataUfo == null)
                {
                    Debug.Log("################## ERROR MoveObjectToPosition dataUfo is Empty   GO:" + this.gameObject.name);
                    return false;
                }
            }
            if (_resName != this.gameObject.name)
            {
                Debug.Log("################## ERROR MoveObjectToPosition ===========POST========= rael name: " + this.gameObject.name + "  new name: " + _resName);
                this.gameObject.name = _resName;
            }
        }

        return true;
        //+++++++++++++++++++++++
    }

    private ModelNPC.GameDataUfo FindObjectData(string callFunc)
    {
        var dataUfo = SaveLoadData.FindObjectData(this.gameObject) as ModelNPC.GameDataUfo;


        if (dataUfo == null)
        {
            Debug.Log("#################### Error UFO MoveObjectToPosition dataUfo is Empty !!!!    :" + callFunc);
            return null;
        }

        if (dataUfo.NameObject != this.name)
        {
            Debug.Log("#################### Error UFO MoveObjectToPosition dataUfo: " + dataUfo.NameObject + "  GO: " + this.name + "   :" + callFunc);
            return null;
        }

        if (dataUfo.TargetPosition == new Vector3(0, 0, 0))
        {
            Debug.Log("#################### Error UFO dataUfo.TargetPosition is zero !!!!   :" + callFunc);
            return null;
        }

        return dataUfo;
    }

    //@SAVE@
    public void UpdateData(string callFunc)
    {
        //Debug.Log("_____________GameObject.UpdateData ________________" + this.name);
        _dataUfo = FindObjectData(callFunc) as ModelNPC.GameDataUfo;
        //testNewName = dataUfo.NameObject;
    }

    //#TARGET
    public void SetTarget()
    {
        Debug.Log("^^^^^^^^ TARGET --- SetTarget UFO");//#TARGET
        if (_dataUfo!=null)
        {
            //_dataUfo.SetTargetPosition(Storage.Person.PersonsTargetPosition);
            _dataUfo.SetTargetPosition(Storage.Instance.PersonsTargetPosition);
            
        }
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

        //if (objID == Storage.Instance.SelectGameObjectID)
        //{
        //    style.fontSize = 15;
        //    style.fontStyle = FontStyle.Bold;
        //    style.normal.textColor = Color.yellow;
        //}
        IsSelectedMe();
        //if(isSelected)
        if (objID == Storage.Instance.SelectGameObjectID)
        {
            style.fontSize = 15;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.yellow;
        }else
        {
            style.fontSize = 13;
            style.fontStyle = FontStyle.Normal;
            style.normal.textColor = Color.black;
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

    public void SaveData()
    {
        string _nameField = Helper.GetNameFieldByName(_dataUfo.NameObject);

        //Storage.Data.AddDataObjectInGrid(_dataUfo, field, "SaveData");
        //Debug.Log("SSSSSSSSSSSSS SaveData ++ NextPosition   : " + this.name);
        _dataUfo.Update(this.gameObject);

        if (this.gameObject == null)
        {
            Debug.Log("############# SaveData ++ This GameObject is null");
            return;
        }

        //Debug.Log("SSSSSSSSSSSSS SaveData ++ UpdateDataObect   : " + this.name);
        //Storage.Data.UpdateDataObect(_nameField, this.gameObject.name, _dataUfo, "SaveData", this.gameObject.transform.position);

        //_dataUfo.UpdateGameObject(this.gameObject);
        //Storage.Instance.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona);
    }

    private bool _isSelected = false;
    private bool IsSelectedMe()
    {
        if (objID == Storage.Instance.SelectGameObjectID)
        {
            //Storage.Events.ListLogAdd ="SelectedMe " + this.name;
            //Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^ SELECTED " + this.name);

            if (!_isSelected)
            {

                //Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^ SELECTED 2." + this.name);
                _isSelected = true;
                SelectedMe();
                return true;
            }
        }
        else
        {
            _isSelected = false;
        }
        return false;
    }

    private void SelectedMe()
    {
        //Storage.Instance.SelectGameObjectID
        //Storage.Events.ListLogAdd = "SelectedMe: " + this.gameObject.name;
    }

    private void OnMouseDown()
    {
        SelectIdFromTextBox();
    }

    private void SelectIdFromTextBox()
    {
        _scriptUIEvents.SetTestText(objID);
    }
}
