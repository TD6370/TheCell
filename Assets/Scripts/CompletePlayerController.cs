using UnityEngine;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider2D))]
public class CompletePlayerController : MonoBehaviour {

    public Camera MainCamera;

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
    private GenerateGridFields m_scriptGrid;
    private Vector2 _movement;
    private Vector2 _MousePositionClick;

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

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        GetMousePositionOnScene2();
    }


    void Update()
    {
         //GetMousePositionOnScene2()
    }

    private void GetMousePositionOnScene2()
    {
        //GetMousePositionOnScene();
        //var t2 = Input.GetButtonDown("Q");
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("&&&&&& GetMousePositionOnScene.....Input.GetMouseButtonDown  &  Input.mousePosition");
            _MousePositionClick = Input.mousePosition;
            Debug.Log("&&&&&& GetMousePositionOnScene.....Input.GetMouseButtonDown  &  Input.mousePosition " + _MousePositionClick);
        }
        
        //Vector3 screenPosition = Camera.main.WorldToScreenPoint(mousePos);
        //Rect position1 = new Rect(screenPosition.x - 10, Screen.height - screenPosition.y - 30, 300, 100);

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, hit))
        //{
        //    Debug.Log(hit.collider.name);
        //}
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


    void OnMouseDown()
    {
        float nextSize = 8.0f;
        float size = MainCamera.orthographicSize;
        if(size==8.0f)
            nextSize = 22.0f;
        else if(size==22.0f)
            nextSize = 35.0f;
        else 
            nextSize = 8.0f;

        MainCamera.orthographicSize = nextSize;
    }

    private void RestructGrid()
    {
      

        var prefabFind = FindFieldCurrent();
        if (prefabFind != null)
        {
            txtLog.text = prefabFind.name.ToString();
            prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorCurrentField;
        }
    }

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

        //# string nameFiled = "Filed" + posX + "x" + posY;
        string nameFiled = Helper.GetNameField(posX, posY);

        txtLog.text = "?" + nameFiled;
        
        //Debug.Log("MainCamera.GetComponent GenerateGridFields");
        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("MainCamera null");
            return null;
        }
        
        GenerateGridFields scriptGrid = MainCamera.GetComponent<GenerateGridFields>();

        if (Storage.Instance == null)
        {
            Debug.Log("scriptStorage null");
            return null;
        }
        Storage.Instance.SetHeroPosition(posX, posY, transform.position.x, transform.position.y); 

        scriptGrid.GenGridLook(_movement, posX, Storage.Instance.LimitHorizontalLook, posY, Storage.Instance.LimitVerticalLook);

        if (! Storage.Instance.Fields.ContainsKey(nameFiled))
            return null;
        GameObject prefabFind = Storage.Instance.Fields[nameFiled];

        return prefabFind;
    }
   

	void OnTriggerEnter2D(Collider2D other) 
	{
        //Debug.Log("OnTriggerEnter2D.............................................");

		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag ("PrefabUfo")) 
		{
            //DestroyObject(other.gameObject);  //        var gObj = other.gameObject;
            StartCoroutine(DestroyObjectC(other.gameObject));
		}
	}


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter.............................................");
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

    private void BeforeDestroyUfo()
    {
        //Add one to the current value of our count variable.
        _count = _count + 1;

        //Update the currently displayed count by calling the SetCountText function.
        SetCountText();
    }

	//This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
	void SetCountText()
    {
        int limit = 150;
        txtMessage.text = "Count: " + _count.ToString() + " / " + limit;

        if (_count >= limit)
        {
            txtMessage.text = "You win! :" + _count;
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        while(true)
        {
            yield return new WaitForSeconds(5);
            txtMessage.text = "END GAME";
            Application.Quit();
        }
    }

    private void GetMousePositionOnScene()
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

    void OnGUI()
    {
        //Debug.Log("Current detected event: " + Event.current);
        //GetMousePositionOnScene();
        GetMouseCursorClick();
    }

    private Rect _positionLastTarget = new Rect(0,0, 100, 100);

    private void GetMouseCursorClick()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_MousePositionClick);
        //Debug.Log("^^^^^^^^ GetMouseCursorClick _MousePositionClick: " + screenPosition);
        if (screenPosition != new Vector3(0, 0, 0))
        {
            Rect positionM = new Rect(_MousePositionClick.x, Screen.height - _MousePositionClick.y, 100, 100);
            if (_positionLastTarget != positionM)
            {
                //Debug.Log("^^^^^^^^ GetMouseCursorClick _MousePositionClick: " + positionM);

                _positionLastTarget = positionM;
                float positionMx = positionM.x / 25;
                float positionMy = positionM.y / 25;

                string field = Helper.GetNameFieldPosit(positionMx, positionMy);

                //string field2 = Helper.GetNameFieldPosit(_MousePositionClick.x, _MousePositionClick.y);
                //string field3 = Helper.GetNameFieldPosit(screenPosition.x, screenPosition.y);
                //GUI.Label(positionM, field + " " + field2 + " " + field3);
                GUI.Label(positionM, field);
                foreach (var gobj in Storage.Person.GetAllRealPersons(field))
                {
                    Debug.Log("^^^^^^^^ GetMouseCursorClick _MousePositionClick: GOBJ: " + gobj.name);
                    gobj.GetComponent<SpriteRenderer>().color = Color.black;
                }
            }

        }

    }
}
