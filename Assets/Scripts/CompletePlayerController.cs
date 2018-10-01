using UnityEngine;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class CompletePlayerController : MonoBehaviour {

    [Header("Speed move hero")]
	public float speed;				//Floating point variable to store the player's movement speed.
	[Space]
    public Text countText;			//Store a reference to the UI Text component which will display the number of pickups collected.
	public Text winText;			//Store a reference to the UI Text component which will display the 'You win' message.
    public Text winTextLog;
    public Camera MainCamera;

	private Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.
	[SerializeField]
    private int count;				//Integer to store the number of pickups collected so far.

    int posLastX = 0;
    int posLastY = 0;

	// Use this for initialization
	void Start()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();

		//Initialize count to zero.
		count = 0;

		//Initialze winText to a blank string since we haven't won yet at beginning.
		winText.text = "";

		//Call our SetCountText function which will update the text with the current value for count.
		SetCountText ();
	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{
		//Store the current horizontal input in the float moveHorizontal.
		float moveHorizontal = Input.GetAxis ("Horizontal");

		//Store the current vertical input in the float moveVertical.
		float moveVertical = Input.GetAxis ("Vertical");

		//Use the two store floats to create a new Vector2 variable movement.
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

		//Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
		//rb2d.AddForce (movement * speed);
        rb2d.MovePosition(rb2d.position + movement * speed * Time.deltaTime);

        if (movement.x != 0 || movement.y != 0)
        {
            //GetpositionFiled();
            RestructGrid();
            
        }
	}

    private void RestructGrid()
    {
        var prefabFind = FindFieldCurrent();
        if (prefabFind != null)
        {
            //Debug.Log("Filed finded !!! " + prefabFind.name);
            winTextLog.text = prefabFind.name.ToString();
            prefabFind.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    GameObject FindFieldCurrent()
    {
        int posX = 0;
        int posY = 0;
        posX = (int)((transform.position.x / 2) + 0.5);
        posY = (int)((transform.position.y / 2) - 0.5);
        posY = (int)(Mathf.Abs(posY));
        if (posLastX == posX && posLastY == posY)
            return null;
        posLastX = posX;
        posLastY = posY;

        string nameFiled = "Filed" + posX + "x" + posY;
        winTextLog.text = "?" + nameFiled;
        
        //Debug.Log("MainCamera.GetComponent GenerateGridFields");
        var camera = MainCamera;
        if (camera == null)
        {
            Debug.Log("MainCamera null");
            return null;
        }

        GenerateGridFields scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
        var Fields = scriptGrid.Fields;
        if (!Fields.ContainsKey(nameFiled))
            return null;
        GameObject prefabFind = Fields[nameFiled];

        //GameObject prefabFind = GameObject.Find(nameFiled);
        return prefabFind;
    }

  

	//OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
	void OnTriggerEnter2D(Collider2D other) 
	{
		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag ("Ufak")) 
		{
			//... then set the other object we just collided with to inactive.
			//other.gameObject.SetActive(false);
            Destroy(other.gameObject);
			
			//Add one to the current value of our count variable.
			count = count + 1;
			
			//Update the currently displayed count by calling the SetCountText function.
			SetCountText ();
		}

        //if (other.gameObject.CompareTag("Field"))
        //{

        //    winTextLog.text = other.gameObject.transform.position.ToString();

            
        //}
        
	}

	//This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
	void SetCountText()
	{
		//Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
		countText.text = "Count: " + count.ToString ();

		//Check if we've collected all 12 pickups. If we have...
		if (count >= 12)
			//... then set the text property of our winText object to "You win!"
			winText.text = "You win!";
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
