using UnityEngine;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider2D))]
public class CompletePlayerController : MonoBehaviour {

    public Camera MainCamera;
    public GameObject UIController;

    private GenerateGridFields m_scriptGrid;
    //private UIEvents m_UIEvents;

    [Header("Speed move hero")]
    public float Speed;             //Floating point variable to store the player's movement speed.
    [Space]
    public Text txtMessage;			//Store a reference to the UI Text component which will display the 'You win' message.
    public Text txtLog;
    public Color ColorCurrentField = Color.yellow;
    public Color ColorSelectedCursorObject = Color.cyan;
    public Color ColorFindCursorObject = Color.magenta;
    //List<string> listLog = new List<string>();

    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
                                    //[SerializeField]
    private int _count;				//Integer to store the number of pickups collected so far.
    private int _posLastX = 0;
    private int _posLastY = 0;
    

    private Vector2 _movement;

    private Vector2 _MousePositionClick;
    private Vector2 _MousePosition;
    private Vector2 _PosHeroToField;
    private string _fieldHero;

    private Rect _positionLastTarget = new Rect(0, 0, 100, 100);
    private Rect _rectCursor = new Rect(-1, -1, 100, 100);
    private string _infoPoint = "";
    private string _fieldCursor = "Empty";
    private int _offsetLabelX = 10;
    private int _offsetLabelY = 10;
    float _diffCenterX = 0;
    float _diffCenterY = 0;
    private bool _RotateMenu = false;
    

    #region Events

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        InitData();
        _count = 0;
        SetTextMessage = "";

        SetCountText();

        //Set Start Position
        rb2d.MovePosition(new Vector2(40, -40));

        //if(UIController==null)
        //{
        //    Debug.Log("################## UIController is EMPTY");
        //    return;
        //}
        //m_UIEvents = UIController.GetComponent<UIEvents>();
        //if (m_UIEvents == null)
        //{
        //    Debug.Log("################## m_UIEvents is EMPTY");
        //    return;
        //}
    }

    void Awake()
    {
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (Storage.Instance.IsLoadingWorld)
        {
            Debug.Log("_______________ LOADING WORLD ....._______________");
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        _movement = new Vector2(moveHorizontal, moveVertical);

        rb2d.MovePosition(rb2d.position + _movement * Speed * Time.deltaTime);

        if (_movement.x != 0 || _movement.y != 0)
        {
            RestructGrid();
        }

        //CalculateDiffCenterHero();
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("&&&&&& GetMousePositionOnScene.....Input.GetMouseButtonDown  &  Input.mousePosition");
            _MousePositionClick = Input.mousePosition;
            //Debug.Log("&&&&&& GetMousePositionOnScene.....Input.GetMouseButtonDown  &  Input.mousePosition " + _MousePositionClick);
        }
        if (Input.GetMouseButtonDown(1))
        {
            _RotateMenu = true;
        }
        _MousePosition = Input.mousePosition;
    }

    void Update()
    {
        //GetMousePositionOnScene2()
        //_MousePosition = Input.mousePosition;
    }

    void OnMouseDown()
    {
        float nextSize = 8.0f;
        float size = MainCamera.orthographicSize;
        if (size == 8.0f)
            nextSize = 22.0f;
        else if (size == 22.0f)
            nextSize = 35.0f;
        if (size == 35.0f)
            nextSize = 10.0f;
        else
            nextSize = 8.0f;

        MainCamera.orthographicSize = nextSize;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("OnTriggerEnter2D.............................................");

        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("PrefabUfo"))
        {
            //DestroyObject(other.gameObject);  //        var gObj = other.gameObject;
            StartCoroutine(DestroyObjectC(other.gameObject));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter.............................................");
    }

    void OnGUI()
    {
        //Debug.Log("Current detected event: " + Event.current);
        GetMouseCursorClick();
    }
    #endregion

    #region Coroutine

    IEnumerator EndGame()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            SetTextMessage = "END GAME";
            Application.Quit();
        }
    }


    IEnumerator DestroyObjectC(GameObject gObj)
    {
        yield return new WaitForSeconds(0.5f);

        //if (m_scriptGrid == null)
        //{
        //    Debug.Log("scriptGrid null");
        //    yield break;
        //}
        //m_scriptGrid.DestroyRealObject(gObj);
        //@DESTROY@
        Storage.Instance.DestroyFullObject(gObj);

        BeforeDestroyUfo();

        yield break;
    }

    #endregion

    #region Public

    public GameObject FindFieldCurrent()
    {
        int scale = 2;
        int posX = 0;
        int posY = 0;
        posX = (int)((transform.position.x / scale) + 0.5);
        posY = (int)((transform.position.y / scale) - 0.5);
        posY = (int)(Mathf.Abs(posY));

        if (_posLastX == posX && _posLastY == posY)
            return null;

        _posLastX = posX;
        _posLastY = posY;

        _fieldHero = Helper.GetNameField(posX, posY);
        _PosHeroToField = new Vector2(posX, posY);

        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("MainCamera null");
            return null;
        }

        if (Storage.Instance == null)
        {
            Debug.Log("scriptStorage null");
            return null;
        }

        Storage.Instance.SetHeroPosition(posX, posY, transform.position.x, transform.position.y);

        if (m_scriptGrid == null)
        {
            m_scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
        }
        m_scriptGrid.GenGridLook(_movement, posX, Storage.Instance.LimitHorizontalLook, posY, Storage.Instance.LimitVerticalLook);

        if (!Storage.Instance.Fields.ContainsKey(_fieldHero))
            return null;

        GameObject prefabFind = Storage.Instance.Fields[_fieldHero];

        return prefabFind;
    }

    #endregion

    private void InitData()
    {
        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("MainCamera null");
            return;
        }
        m_scriptGrid = camera.GetComponent<GenerateGridFields>();
        if (m_scriptGrid == null)
        {
            Debug.Log("Error scriptGrid is null !!!");
            return;
        }
    }

    private void RestructGrid()
    {
        var prefabFind = FindFieldCurrent();
        if (prefabFind != null)
        {
            //txtLog.text = prefabFind.name.ToString();
            //>>>>>> SetTextLog(prefabFind.name.ToString());
            prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorCurrentField;
        }
    }

    private void BeforeDestroyUfo()
    {
        _count = _count + 1;
        SetCountText();
    }

    private Vector2 CalculatePosCutsorToGrid()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_MousePosition);

        //_rectCursor = new Rect(_MousePositionClick.x, Screen.height - _MousePositionClick.y, 300, 800);
        var _rectCursorReal = new Rect(_MousePosition.x, Screen.height - _MousePosition.y, 300, 800);

        float zoom = MainCamera.orthographicSize / 10;

        Vector2 posHero = transform.position;
        Vector2 posCursorToField = new Vector2(0, 0);
        Vector2 posHeroStartInCentre = new Vector2(17, -9);
        Vector2 posCursorNormalize = new Vector2(0, 0);
        float offsetUI_World_lenToCenterScreenX = 0;
        float offsetUI_World_lenToCenterScreenY = 0;
        float normalX = 1;
        float normalY = 1;
        string testCalc = "";

        offsetUI_World_lenToCenterScreenX = Screen.width / 35;
        offsetUI_World_lenToCenterScreenY = Screen.height / 19;
        testCalc += "\n *ffsetY(" + offsetUI_World_lenToCenterScreenY + ") = Screen.height(" + Screen.height + ") / 19;";

        normalX = _rectCursorReal.x / offsetUI_World_lenToCenterScreenX;
        normalY = _rectCursorReal.y / offsetUI_World_lenToCenterScreenY;
        testCalc += "\n *posCursorToField.Y(" + normalY + ") = _rectCursorReal.y(" + _rectCursorReal.y + ") / offsetY(" + offsetUI_World_lenToCenterScreenY + ")";
        posCursorToField = new Vector2(normalX, normalY);

        float addPosHeroX = posHero.x - posHeroStartInCentre.x + posCursorToField.x;
        //float addPosHeroY = posHero.y - posHeroStartInCentre.y + posCursorToField.y; ///!!!!!!!!!!!!!!!
        float addPosHeroY = Math.Abs(posHero.y) - Math.Abs(posHeroStartInCentre.y) + Math.Abs(posCursorToField.y); ///!!!!!!!!!!!!!!!
        addPosHeroY *= -1;

        testCalc += "\naddPosHeroY(" + addPosHeroY + ") = posHero.y(" + posHero.y + ") - posHeroStartInCentre.y(" + posHeroStartInCentre.y + ") + *posCursorToField.y(" + posCursorToField.y + ")";
        posCursorToField = new Vector2(addPosHeroX, addPosHeroY);


        //var posGridX = (int)((posCursorToField.x / Storage.ScaleWorld));
        //var posGridY = (int)(((posCursorToField.y) / Storage.ScaleWorld));

        //_infoPoint =
        //    "\nMouse X=" + _rectCursorReal.x + " Y=" + _rectCursorReal.y +
        //    "\nSize: " + Screen.width + "x" + Screen.height +
        //    "\nHero: " + transform.position.x + "x" + transform.position.y +
        //    "\nZoom=" + zoom +
        //    "\nField: " + _fieldCursor +
        //    "\nOffset: " + offsetUI_World_lenToCenterScreenX + " : " + offsetUI_World_lenToCenterScreenY +
        //    "\nNormali : " + normalX + " x " + normalY +
        //    "\nAdd pos Hero: " + addPosHeroX + " x " + addPosHeroY
        //    + testCalc;

        return posCursorToField;
    }

    private void VeiwCursorGameObjectData(string fieldCursor)
    {
        //Storage.Events.ListLogClear();
        

        GameObject prefabFind = Storage.Instance.Fields[_fieldCursor];

        if (prefabFind != null)
        {
            //txtLog.text = prefabFind.name.ToString();
            prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorSelectedCursorObject;
        }

        foreach (var gobj in Storage.Person.GetAllRealPersons(_fieldCursor))
        {
            //listLog.Add(gobj.name);
            Storage.Events.ListLogAdd = "FIND (" + _fieldCursor + "): " + gobj.name;

            //Debug.Log("^^^^^^^^ GetMouseCursorClick _MousePositionClick: GOBJ: " + gobj.name);
            gobj.GetComponent<SpriteRenderer>().color = ColorFindCursorObject;
        }

        //SetTextLog = Storage.Events.ListLogtoString;// String.Join("\n", listLog.ToArray()); 
    }

    private void GetMouseCursorClick()
    {


        _rectCursor = new Rect(_MousePositionClick.x, Screen.height - _MousePositionClick.y, 300, 800);

        if (_positionLastTarget != _rectCursor)
        {
            _positionLastTarget = _rectCursor;

            Vector2 posCursorToField = CalculatePosCutsorToGrid();
            _fieldCursor = Helper.GetNameFieldPosit(posCursorToField.x, posCursorToField.y);
            _infoPoint = "Cursor :" + posCursorToField + "\nfind:" + _fieldCursor;

            VeiwCursorGameObjectData(_fieldCursor);
        }

        if (_RotateMenu == true)
        {
            _RotateMenu = false;
            if (_offsetLabelX == 10 && _offsetLabelY == 10)
            {
                _offsetLabelX = -150;
            }
            else if (_offsetLabelX == -150 && _offsetLabelY == 10)
            {
                _offsetLabelY = -130;
            }
            else if (_offsetLabelX == -150 && _offsetLabelY == -130)
            {
                _offsetLabelX = 10;
            }
            else if (_offsetLabelX == 10 && _offsetLabelY == -130)
            {
                _offsetLabelY = 10;
            }
        }

        var _rectCursorReal = new Rect(_MousePosition.x, Screen.height - _MousePosition.y, 300, 800);
        Rect rectPosLabel = new Rect(_rectCursorReal.x + _offsetLabelX, _rectCursorReal.y + _offsetLabelY, _rectCursorReal.width, _rectCursorReal.height);
        var test = _infoPoint;
        SetLabelCursor(rectPosLabel, test);
    }

    private void SetLabelCursor(Rect position, string text)
    {
        GUIStyle styleLabel = new GUIStyle();
        styleLabel.fontSize =16;
        styleLabel.richText = true;
        styleLabel.normal.textColor =Color.black;
        styleLabel.fontStyle = FontStyle.Bold;
        //shadow.alignment = TextAnchor.MiddleCenter;

        GUI.Label(position, text, styleLabel);
    }

    //private string SetTextLog
    //{
    //    set
    //    {
    //        txtLog.text = value;
    //    }
    //}

    private string SetTextMessage
    {
        set
        {
            txtMessage.text = value;
        }
    }
    

    void SetCountText()
    {
        int limitEndGame = 150;
        SetTextMessage = "Count: " + _count.ToString() + " / " + limitEndGame;

        if (_count >= limitEndGame)
        {
            SetTextMessage = "You win! :" + _count;
            StartCoroutine(EndGame());
        }
    }

}
