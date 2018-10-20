using UnityEngine;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider2D))]
public class CompletePlayerController : MonoBehaviour {

    public Camera MainCamera;
    //private GenerateGridFields _scriptGrid;
    private GenerateGridFields m_scriptGrid;

    [Header("Speed move hero")]
    public float Speed;             //Floating point variable to store the player's movement speed.
    [Space]
    public Text txtMessage;			//Store a reference to the UI Text component which will display the 'You win' message.
    public Text txtLog;
    public Color ColorCurrentField = Color.yellow;

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

    #region Events

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        InitData();
        _count = 0;
        txtMessage.text = "";

        SetCountText();

        //Set Start Position
        rb2d.MovePosition(new Vector2(40, -40));


       
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

        CalculateDiffCenterHero();

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        

        GetMousePositionOnScene();
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
        if(size==8.0f)
            nextSize = 22.0f;
        else if(size==22.0f)
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
            txtMessage.text = "END GAME";
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

        //-----------------
        //float restX = (posX * scale) - 0.5f;
        //float restY = (posY * scale) + 0.5f;
        //-----------------

        posY = (int)(Mathf.Abs(posY));

        //-----------------
        //string textTest = "";
        //_diffCenterX = transform.position.x - restX;
        //_diffCenterY = transform.position.y - restY;
        ////float diffCenterY = Mathf.Abs(transform.position.y) - restY;
        //textTest = "\nDiff Center Hero=" + _diffCenterX + "x" + _diffCenterY;
        //-----------------

        if (_posLastX == posX && _posLastY == posY)
            return null;

        _posLastX = posX;
        _posLastY = posY;

        //# string nameFiled = "Filed" + posX + "x" + posY;
        _fieldHero = Helper.GetNameField(posX, posY);
        _PosHeroToField = new Vector2(posX, posY);

        //SetTextLog("?" + _fieldHero + " " + textTest);
        //SetTextLog("????");

        //Debug.Log("MainCamera.GetComponent GenerateGridFields");
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

    private void CalculateDiffCenterHero()
    {
        int scale = 2;
        int posX = 0;
        int posY = 0;
        //posX = (int)((transform.position.x / scale) + 0.5);
        //posY = (int)((transform.position.y / scale) - 0.5);

        ////-----------------
        //float restX = (posX * scale) - 0.5f;
        //float restY = (posY * scale) + 0.5f;
        ////-----------------

        posX = (int)((transform.position.x / scale));
        posY = (int)((transform.position.y / scale));

        //-----------------
        float restX = (posX * scale);
        float restY = (posY * scale);
        //-----------------

        //posY = (int)(Mathf.Abs(posY));

        //-----------------
        string textTest = "";
        _diffCenterX = transform.position.x - restX;
        _diffCenterY = transform.position.y - restY;
        float diffTestX;
        float diffTestY;
        float TestX = 1;
        float TestY = 1;

        if (Math.Abs(_diffCenterX) < 1)
        {
            //_diffCenterX = Math.Abs(_diffCenterX) * (-1);
            diffTestX = -1;
            TestX = Math.Abs(_diffCenterX) * 100;
            TestX = (float)Math.Round(TestX, 2);
        }
        else
        {
            //_diffCenterX = 1 - Math.Abs(_diffCenterX);
            diffTestX = 1;
            TestX = (Math.Abs(_diffCenterX)-1) * 100;
            TestX = (float)Math.Round(TestX, 2);
        }
        if (Math.Abs(_diffCenterY) < 1)
        {
            //_diffCenterY = Math.Abs(_diffCenterY) * (-1);
            diffTestY = -1;
            TestY = Math.Abs(_diffCenterY) * 100;
            TestY = (float)Math.Round(TestY, 2);
        }
        else
        {
            //_diffCenterY = 1 - Math.Abs(_diffCenterY);
            diffTestY = 1;
            TestY = (Math.Abs(_diffCenterY) - 1) * 100;
            TestY = (float)Math.Round(TestY, 2);
        }

        textTest = "\nDiff Center Hero=\nX=(" + _diffCenterX + ")\n" + diffTestX + "x" + diffTestY + "\nY=("  + _diffCenterY + ")" +
            "\n diffX=" + TestX +
            "\n diffY=" + TestY;

        SetTextLog("?" + _fieldHero + " " + textTest);
    }

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
        //Add one to the current value of our count variable.
        _count = _count + 1;

        //Update the currently displayed count by calling the SetCountText function.
        SetCountText();
    }
	//This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
	
   

    private void GetMousePositionOnScene_()
    {
        string errInd = "satrt";
        try
        {
            //return;
            errInd = "1";
            if (Event.current == null)
            {
                //Debug.Log("########## Error GetMousePositionOnScene Event.current==null");
                return;
            }
            errInd = "1.2";
            if (Event.current.button == null)
            {
                Debug.Log("########## Error GetMousePositionOnScene Event.current.button");
                return;
            }
            errInd = "1.3";
            if (Event.current.type != EventType.MouseDown || Event.current.button != 0)
                return;

            errInd = "2";
            // convert GUI coordinates to screen coordinates
            Vector3 screenPosition = Event.current.mousePosition;
            errInd = "3";
            if(Camera.current==null)
            {
                Debug.Log("########## Error GetMousePositionOnScene Camera.current = null");
                return;
            }

            screenPosition.y = Camera.current.pixelHeight - screenPosition.y;
            //screenPosition.y = MainCamera.current.pixelHeight - screenPosition.y;
            errInd = "4";
            Ray ray = Camera.current.ScreenPointToRay(screenPosition);
            errInd = "5";
            RaycastHit hit;
            errInd = "6";
            // use a different Physics.Raycast() override if necessary
            if (Physics.Raycast(ray, out hit))
            {
                errInd = "7";
                // do stuff here using hit.point
                // tell the event system you consumed the click
                Event.current.Use();
            }
            errInd = "8";
        }
        catch (Exception x)
        {
            Debug.Log("########## Error GetMousePositionOnScene (" + errInd + ") " + x.Message + "");
        }
    }

  
    private void GetMousePositionOnScene()
    {
        //GetMousePositionOnScene();
        //var t2 = Input.GetButtonDown("Q");
        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("&&&&&& GetMousePositionOnScene.....Input.GetMouseButtonDown  &  Input.mousePosition");
            _MousePositionClick = Input.mousePosition;
            //Debug.Log("&&&&&& GetMousePositionOnScene.....Input.GetMouseButtonDown  &  Input.mousePosition " + _MousePositionClick);
        }
        _MousePosition = Input.mousePosition;
    }

    private void GetMouseCursorClick()
    {
        //return;

        //Vector3 screenPosition = Camera.main.WorldToScreenPoint(_MousePositionClick);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_MousePosition);
        //Debug.Log("^^^^^^^^ GetMouseCursorClick _MousePositionClick: " + screenPosition);
        //if (screenPosition != new Vector3(0, 0, 0))
        //{
        _rectCursor = new Rect(_MousePositionClick.x, Screen.height - _MousePositionClick.y, 300, 800);
        //if (_positionLastTarget != _rectCursor)
        //{
            //_positionLastTarget = _rectCursor;

        //---calculation
        float zoom = MainCamera.orthographicSize / 10;
        //Vector2 posCursorToField = CalculatePositionCursorToField(zoom);
        //---calculation
        //============================
        var _rectCursorReal = new Rect(_MousePosition.x, Screen.height - _MousePosition.y, 300, 800);
        string testText = "";

        float ScreenWidth = Screen.width;
        float ScreenHeight = Screen.height; // 800;
        float diffX = 1;
        float diffY = 1;

        int Rows = 10;
        int Columns = 18;
        float sizeW = ScreenWidth / Columns;
        float sizeH = ScreenHeight / Rows;
        float korrectSizeY = 0;// -10;
        float korrectSizeX = 0;
        //float korrectSizeY = _diffCenterY * 10;// -10;
        //float korrectSizeX = _diffCenterX * -10;
        //float korrectSizeX = (_diffCenterX < 0) ?
        //    _diffCenterX * -10 :
        //    _diffCenterX * 10;
        //float korrectSizeX = 0;
        float mX = (_MousePosition.x + korrectSizeX);
        float mY = ScreenHeight - _MousePosition.y + korrectSizeY;

        diffX = (int)(mX / sizeW);
        diffY = (int)(mY / sizeH);
        //}
        Vector2 posCursorToField = CalculatePositionCursorToField((int)diffX, (int)diffY, 1);
        //string findField = Helper.GetNameField(fieldPosNormaliz.x, fieldPosNormaliz.y);
        
        // + "\nfindField: " + findField;
        //_infoPoint = testText;
        //============================
        //<<<<<

        _fieldCursor = Helper.GetNameField(posCursorToField.x, posCursorToField.y);

        _infoPoint =
            //"\nMouse X=" + _MousePosition.x + " Y=" + _MousePosition.y + 
            "\nMouse X=" + _rectCursorReal.x + " Y=" + _rectCursorReal.y +
            "\nSize: " + ScreenWidth + "x" + ScreenHeight +
            "\nHero: " + transform.position.x + "x" + transform.position.y +
            //"\nReal=" + _rectCursorReal.x + "x" + _rectCursorReal.y + 
            "\nScreen: " + screenPosition.x + "x" + screenPosition.y +
            "\nZoom=" + zoom +
            "\nField: " + _fieldCursor;

        if (_positionLastTarget != _rectCursor)
        {
            _positionLastTarget = _rectCursor;

             GameObject prefabFind = Storage.Instance.Fields[_fieldCursor];
            if (prefabFind != null)
            {
                //txtLog.text = prefabFind.name.ToString();
                prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorCurrentField;
            }

            _infoPoint += "Cursor :" + posCursorToField + "\nfind:" + _fieldCursor;

            foreach (var gobj in Storage.Person.GetAllRealPersons(_fieldCursor))
            {
                Debug.Log("^^^^^^^^ GetMouseCursorClick _MousePositionClick: GOBJ: " + gobj.name);
                gobj.GetComponent<SpriteRenderer>().color = Color.black;
            }

            if(_offsetLabelX == 10 && _offsetLabelY == 10)
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

        //GUI.Label(positionM, _fieldPoint + " " + _infoPoint );
        //Rect rectPosLabel = new Rect( _rectCursor.x + 10, _rectCursor.y + 10, _rectCursor.width, _rectCursor.height);

        //SetLabel(rectPosLabel, _infoPoint);
        //}


        
        Rect rectPosLabel = new Rect(_rectCursorReal.x + _offsetLabelX, _rectCursorReal.y + _offsetLabelY, _rectCursorReal.width, _rectCursorReal.height);

        var test = _infoPoint;//  + "\ncursor=" + _MousePosition.x + "x" + _MousePosition.y + "\ndiffLabel:" + _offsetLabelX + ":" + _offsetLabelY;
        SetLabel(rectPosLabel, test);
    }

    private Vector2 CalculatePositionCursorToField(float zoom)
    {
        float positionMx = _rectCursor.x / 28.4f;
        float positionMy = _rectCursor.y / 28.4f;
        Vector2 posCursorToField = Helper.NormalizPosToField(positionMx, positionMy);
        ////Correct zoom
        //if (zoom != 1)
        //{
        //    //zoom = zoom * 2.5f;
        //}
        int centerHeroX = 8;
        int centerHeroY = 5;
        int offsetX = (int)_PosHeroToField.x - centerHeroX;
        int offsetY = (int)_PosHeroToField.y - centerHeroY;
        posCursorToField += new Vector2(offsetX * zoom, offsetY * zoom);
        return posCursorToField;
    }

    private Vector2 CalculatePositionCursorToField(int x, int y, float zoom)
    {
        int centerHeroX = 8;
        int centerHeroY = 5;
        int offsetX = (int)_PosHeroToField.x - centerHeroX;
        int offsetY = (int)_PosHeroToField.y - centerHeroY;
        return new Vector2(x,y) + new Vector2(offsetX * zoom, offsetY * zoom);
    }

    private void SetLabel(Rect position, string text)
    {
        GUIStyle styleLabel = new GUIStyle();
        styleLabel.fontSize =16;
        styleLabel.richText = true;
        styleLabel.normal.textColor =Color.black;
        styleLabel.fontStyle = FontStyle.Bold;
        //shadow.alignment = TextAnchor.MiddleCenter;

        GUI.Label(position, text, styleLabel);
    }

    private void SetTextLog(string text)
    {
        txtLog.text = text;// "?" + _fieldHero;
    }

    void SetCountText()
    {
        int limitEndGame = 150;
        txtMessage.text = "Count: " + _count.ToString() + " / " + limitEndGame;

        if (_count >= limitEndGame)
        {
            txtMessage.text = "You win! :" + _count;
            StartCoroutine(EndGame());
        }
    }

}
