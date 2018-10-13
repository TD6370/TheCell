using UnityEngine;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class CompletePlayerController : MonoBehaviour {

    public Camera MainCamera;

    [Header("Speed move hero")]
	public float Speed;				//Floating point variable to store the player's movement speed.
	[Space]
	public Text txtMessage;			//Store a reference to the UI Text component which will display the 'You win' message.
    public Text txtLog;
    public Color ColorCurrentField = Color.yellow;

	private Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.
	//[SerializeField]
    private int _count;				//Integer to store the number of pickups collected so far.
    private int _posLastX = 0;
    private int _posLastY = 0;
    private GenerateGridFields m_scriptGrid;
    Vector2 _movement;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
        
        InitData();
		_count = 0;
		txtMessage.text = "";

		SetCountText ();
	}

    void Awake()
    {
    }

  	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

        _movement = new Vector2 (moveHorizontal, moveVertical);

        rb2d.MovePosition(rb2d.position + _movement * Speed * Time.deltaTime);

        if (_movement.x != 0 || _movement.y != 0)
        {
            RestructGrid();
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
	}

    void Update()
    {
        
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
        string nameFiled = Storage.GetNameField(posX, posY);

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

        scriptGrid.GenGrigLook(_movement, posX, Storage.Instance.LimitHorizontalLook, posY, Storage.Instance.LimitVerticalLook);

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
        Storage.Instance.DestroyRealObject(gObj);

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
    
}
