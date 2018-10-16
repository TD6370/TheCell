using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNPC : MonoBehaviour {

    private Coroutine moveObject;
    private Material m_material;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D _rb2d;
    private string objID;
    private UIEvents _scriptUIEvents;
    private SaveLoadData.GameDataNPC _dataNPC;
    string testId;
    string _resName = "";

    void Awake()
    {
    }

    // Use this for initialization
    public virtual void Start()
    {

        InitData();
        moveObject = StartCoroutine(MoveObjectToPosition());
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
    public virtual void Update()
    {
    }

    private void InitData()
    {
        m_material = this.GetComponent<Renderer>().material;
        m_spriteRenderer = this.GetComponent<SpriteRenderer>();
        _rb2d = this.GetComponent<Rigidbody2D>();
    }

    IEnumerator ChangeColor()
    {

        while (true)
        {
            ChangeRandomColor();
            yield return new WaitForSeconds(2f);
        }
    }

    private void ChangeRandomColor()
    {
        m_spriteRenderer.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
    }

    IEnumerator MoveObject()
    {
        int distantion = 0;
        float x = transform.position.x;
        int compas = 1;

        while (true)
        {
            distantion++;
            if (distantion > 200)
            {
                distantion = 0;
                compas *= -1;
            }

            transform.Translate(compas * Time.deltaTime, 0, 0);
            yield return null;
        }
    }




    IEnumerator MoveObjectToPosition()
    {
        Vector3 lastPositionForLock = transform.position;
        Vector3 lastPositionForMoveField = transform.position;

        int stepTest = 0;
        int stepLimitTest = 10;
        float minDist = 0.005f;  

        int speed = 5;
        float step = speed * Time.deltaTime;


        if (!Helper.IsDataInit(this.gameObject))
        {
            yield break;
        }

        string info = "MoveObjectToPosition Init";
        _dataNPC = FindObjectData(info) as SaveLoadData.GameDataNPC;

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

            if (_dataNPC == null)
            {
                Debug.Log("########################## UFO MoveObjectToPosition dataUfo is EMPTY");
                //Storage.Fix.CorrectData(null, this.gameObject, "MoveObjectToPosition");
                yield break;
            }

            stepTest++;
            if (stepTest > stepLimitTest)
            {
                float distLock = Vector3.Distance(lastPositionForLock, transform.position);
                if (distLock < minDist)
                {
                    _dataNPC.SetTargetPosition();
                }
                lastPositionForLock = transform.position;
                stepTest = 0;
            }

            Vector3 targetPosition = _dataNPC.TargetPosition;
            Vector3 pos = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (_rb2d != null)
            {
                _rb2d.MovePosition(pos);
            }
            else
            {
                //Debug.Log("NPC MoveObjectToPosition Set position 2 original........");
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
            if (dist < minDist)
            {
                _dataNPC.SetTargetPosition();
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

        if (!string.IsNullOrEmpty(_resName) && _resName != this.gameObject.name)
        {
            Debug.Log("################## ERROR MoveObjectToPosition ===========PRED========= rael name: " + this.gameObject.name + "  new name: " + _resName);
        }

        string oldName = this.gameObject.name;
        _resName = _dataNPC.NextPosition(this.gameObject);

        if (_resName == "Error")
            return false;

        if (!string.IsNullOrEmpty(_resName))
        {
            if (oldName != _resName)
            {
                string callInfo = "ResavePositionData >> oldName(" + oldName + ") != _resName(" + _resName + ")";
                _dataNPC = FindObjectData(callInfo) as SaveLoadData.GameDataUfo;
                if (_dataNPC == null)
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

    private SaveLoadData.GameDataUfo FindObjectData(string callFunc)
    {
        var dataUfo = SaveLoadData.FindObjectData(this.gameObject) as SaveLoadData.GameDataUfo;


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

    public void UpdateData(string callFunc)
    {
        _dataNPC = FindObjectData(callFunc) as SaveLoadData.GameDataNPC;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 13;
        style.normal.textColor = Color.black;


        if (objID == Storage.Instance.SelectGameObjectID)
        {
            style.fontSize = 15;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.yellow;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Rect position1 = new Rect(screenPosition.x - 10, Screen.height - screenPosition.y - 30, 300, 100);
        GUI.Label(position1, objID, style);
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
