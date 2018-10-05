using UnityEngine;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class CompletePlayerController : MonoBehaviour {

    [Header("Speed move hero")]
	public float Speed;				//Floating point variable to store the player's movement speed.
	[Space]
    public Text txtCount;			//Store a reference to the UI Text component which will display the number of pickups collected.
	public Text txtMessage;			//Store a reference to the UI Text component which will display the 'You win' message.
    public Text txtLog;
    public Button btnExit;

    public Color ColorCurrentField = Color.yellow;
    public Camera MainCamera;

	private Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.
	[SerializeField]
    private int _count;				//Integer to store the number of pickups collected so far.
    private int _posLastX = 0;
    private int _posLastY = 0;
    private int _limitHorizontalLook = 22;
    private int _limitVerticalLook = 18;
    private GenerateGridFields m_scriptGrid; 

    Vector2 _movement;
    //private HorizontalCompas _moveX = HorizontalCompas.Center;
    //private VerticalCompas _moveY = VerticalCompas.Center;

	// Use this for initialization
	void Start()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();

		//Initialize count to zero.
		_count = 0;

		//Initialze winText to a blank string since we haven't won yet at beginning.
		txtMessage.text = "";

		//Call our SetCountText function which will update the text with the current value for count.
		SetCountText ();

        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("MainCamera null");
            return;
        }
        m_scriptGrid = MainCamera.GetComponent<GenerateGridFields>();

        //btnExit.onClick.AddListener(TaskOnClick);
        btnExit.onClick.AddListener(delegate { Application.Quit(); }); 
	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{
		//Store the current horizontal input in the float moveHorizontal.
		float moveHorizontal = Input.GetAxis ("Horizontal");

		//Store the current vertical input in the float moveVertical.
		float moveVertical = Input.GetAxis ("Vertical");

		//Use the two store floats to create a new Vector2 variable movement.
		//Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
        _movement = new Vector2 (moveHorizontal, moveVertical);

		//Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
		//rb2d.AddForce (movement * speed);
        rb2d.MovePosition(rb2d.position + _movement * Speed * Time.deltaTime);

        if (_movement.x != 0 || _movement.y != 0)
        {
            //GetpositionFiled();
            RestructGrid();
            
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        //OnMouseButton();
	}

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        MainCamera.orthographicSize = MainCamera.orthographicSize == 8.0f ? 22.0f : 8.0f;
    }


    private void OnMouseButton()
    {
        if(Input.GetMouseButton(1))
        {
            MainCamera.orthographicSize = MainCamera.orthographicSize == 8.0f ? 22.0f : 8.0f;
        }
     }

    private void RestructGrid()
    {
        var prefabFind = FindFieldCurrent();
        if (prefabFind != null)
        {
            //Debug.Log("Filed finded !!! " + prefabFind.name);
            txtLog.text = prefabFind.name.ToString();
            //prefabFind.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorCurrentField;
            
        }
    }

    //enum HorizontalCompas
    //{
    //    Left, Right, Center
    //}
    //enum VerticalCompas
    //{
    //    Up, Down, Center
    //}

    GameObject FindFieldCurrent()
    {
        int posX = 0;
        int posY = 0;
        posX = (int)((transform.position.x / 2) + 0.5);
        posY = (int)((transform.position.y / 2) - 0.5);
        posY = (int)(Mathf.Abs(posY));

        if (_posLastX == posX && _posLastY == posY)
            return null;

        _posLastX = posX;
        _posLastY = posY;

        //# string nameFiled = "Filed" + posX + "x" + posY;
        string nameFiled = GenerateGridFields.GetNameField(posX, posY);

        txtLog.text = "?" + nameFiled;
        
        //Debug.Log("MainCamera.GetComponent GenerateGridFields");
        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("MainCamera null");
            return null;
        }

        GenerateGridFields scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
        var Fields = scriptGrid.Fields;

         //angle move
        //CalculateCompasMoveHero(posX, posY);
        //_movement
        //Debug.Log("FindFieldCurrent .scriptGrid.GenGrigLook...");
        
        scriptGrid.GenGrigLook(_movement, posX, _limitHorizontalLook,  posY, _limitVerticalLook);
        //StartCoroutine(scriptGrid.GenGrigLookAsync(_movement, posX, _limitHorizontalLook, posY, _limitVerticalLook));

        if (!Fields.ContainsKey(nameFiled))
            return null;
        GameObject prefabFind = Fields[nameFiled];

        //GameObject prefabFind = GameObject.Find(nameFiled);
        return prefabFind;
    }

    //void CalculateCompasMoveHero(int posX, int posY)
    //{
        
    //    int moveXv = 0;
    //    int moveYv = 0;
    //    if (_posLastX > posX)
    //    {
    //        _moveX = HorizontalCompas.Left;
    //        moveXv = -1;
    //    }
    //    else if (_posLastX < posX)
    //    {
    //        _moveX = HorizontalCompas.Right;
    //        moveXv = 1;
    //    }
    //    if (_posLastY > posY)
    //    {
    //        _moveY = VerticalCompas.Up;
    //        moveYv = 1;
    //    }
    //    else if (_posLastY < posY)
    //    {
    //        _moveY = VerticalCompas.Down;
    //        moveYv = -1;
    //    }
    //}

	//OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
	void OnTriggerEnter2D(Collider2D other) 
	{
		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag ("PrefabUfo")) 
		{
            DestroyObject(other.gameObject);  //        var gObj = other.gameObject;

            //Add one to the current value of our count variable.
			_count = _count + 1;
			
			//Update the currently displayed count by calling the SetCountText function.
			SetCountText ();
		}
	}

    private void DestroyObject(GameObject gObj)
    {
        m_scriptGrid.DestroyRealObject(gObj);
    }

	//This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
	void SetCountText()
    {
        int limit = 150;
        //int limit = 10;

        txtCount.text = "Count: " + _count.ToString() + " / " + limit;

        if (_count >= limit)
        {
            txtMessage.text = "You win! :" + _count;
            //Application.Quit();
            StartCoroutine(EndGame());
            //_count = 0;
        }
    }

    IEnumerator EndGame()
    {
        while(true)
        {
            //Application.Quit();
            yield return new WaitForSeconds(5);
            txtMessage.text = "END GAME";
            Application.Quit();
        }
    }

    //void GetpositionFiled()
    //{
    //    int posX = 0;
    //    int posY = 0;
    //    //posX = (int)transform.position.x;
    //    //posY = (int)Mathf.Abs(transform.position.y);
    //    posX = (int)((transform.position.x / 2) + 0.5);
    //    posY = (int)((transform.position.y / 2) - 0.5);
    //    posY = (int)(Mathf.Abs(posY));
    //    if (posLastX == posX && posLastY == posY)
    //        return;
    //    posLastX = posX;
    //    posLastY = posY;

    //    string nameFiled = "Filed" + posX + "x" + posY;
    //    //string nameFiled = posX + "x" + posY;
    //    //Debug.Log("start Filed find==" + nameFiled);
    //    winTextLog.text = "?" + nameFiled;
    //    //GameObject[] listFindFields = GameObject.FindGameObjectsWithTag(nameFiled);
    //    //var coutReal = listFindFields.Length;
    //    //if (coutReal > 0)
    //    //{
    //    //    string positionStr = "";
    //    //    foreach (var filed in listFindFields)
    //    //    {
    //    //        positionStr += "  " + filed.gameObject.transform.position.ToString();
    //    //        var r = filed.gameObject.GetComponent<SpriteRenderer>().sprite;
    //    //        filed.gameObject.GetComponent<SpriteRenderer>().color = Color.green;

    //    //    }
    //    //    winTextLog.text = positionStr;
    //    //    Debug.Log("Filed finded !!! " + coutReal);
    //    //}
    //    //else
    //    //{
    //    //    Debug.Log("NO Filed find");
    //    //}


    //    //GenerateGridFields
    //    //MainCamera.GetComponent(GenerateGridFields)
    //    Debug.Log("MainCamera.GetComponent GenerateGridFields");
    //    //var camera = MainCamera.GetComponent("GenerateGridFields");
    //    var camera = MainCamera;
    //    if (camera == null)
    //    {
    //        Debug.Log("MainCamera null");
    //        return;
    //    }

    //    GenerateGridFields scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
    //    //if (scriptGrid == null)
    //    //{
    //    //    Debug.Log("GenerateGridFields null");
    //    //    return;
    //    //}

    //    var Fields = scriptGrid.Fields;
    //    //if (Fields != null)        {
    //    //    Debug.Log("Fields OK");
    //    //}
    //    //else{
    //    //    Debug.Log("Fields NO");
    //    //    return;
    //    //}

    //    //GameObject prefabFind = GameObject.Find(nameFiled);
    //    if (!Fields.ContainsKey(nameFiled))
    //        return;

    //    var prefabFind = Fields[nameFiled];
    //    if (prefabFind != null)
    //    {
    //        //Debug.Log("Filed finded !!! " + prefabFind.name);
    //        winTextLog.text = prefabFind.name.ToString();
    //        //prefabFind.GetComponent(MoveSphere).DoSomething();
    //        prefabFind.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    //    }
    //}
}
