using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGridFields : MonoBehaviour {

    public GameObject prefabField;
    public GameObject prefabPanel;
    //public Dictionary<int, GameObject> Fields;
    public Dictionary<string, GameObject> Fields;
    public Dictionary<string, List<GameObject>> GamesObjectsActive;

    public float GridX = 5f;
    public float GridY = 5f;
    public float Spacing = 1f;

    private int _counter;

	// Use this for initialization
	void Start () {
        //Fields = new Dictionary<int, GameObject>();
        Fields = new Dictionary<string, GameObject>();
        GamesObjectsActive = new Dictionary<string, List<GameObject>>();
        //StartGenCircle();
        StartGenGrig();
    }


    void Awake()
    {
        
        //CreateFields();
        //StartCoroutine(CreateFieldsAsync());
        //StartGenGrig();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActiveGameObject(GameObject p_saveObjects)
    {
        int x = 0;
        int y = 0;
        x = (int)p_saveObjects.transform.position.x;
        y = (int)Mathf.Abs(p_saveObjects.transform.position.y);
        string p_nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
        AddNewActiveGameObject(p_nameFiled, p_saveObjects);
    }

    private void AddNewActiveGameObject(string p_nameFiled, GameObject p_saveObjects) 
    { 
        List<GameObject> gobjects;
        if (GamesObjectsActive.ContainsKey(p_nameFiled))
        {
            gobjects = GamesObjectsActive[p_nameFiled];
        }
        else
        {
            gobjects = new List<GameObject>();
            GamesObjectsActive.Add(p_nameFiled, gobjects);
            gobjects = GamesObjectsActive[p_nameFiled];
        }
        gobjects.Add(p_saveObjects);
    }

    void StartGenGrig()
    {
        int maxWidth = (int)GridY * -1;
        int maxHeight = (int)GridX;
        _counter = maxWidth * maxHeight;
        Debug.Log("counter=" + _counter.ToString());
        _counter = 0;
        string _nameFiled = "";

        for (int y = 0; y > maxWidth; y--)
        {
            for (int x = 0; x < maxHeight; x++)
            {

                Vector3 pos = new Vector3(x, y, 1) * Spacing;
                //Vector2 pos = new Vector2(x, y) * spacing;
                pos.z = 0;
                //Instantiate(prefabField, pos, Quaternion.identity);
                //Debug.Log("pos=" + pos);
                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                //newFiled.tag = "Field";
                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                //Debug.Log("Field name init : " + nameFiled);
                newFiled.name = nameFiled;
                _nameFiled = nameFiled;
                //string nameFiled = "Field";
                //string nameFiled = + x + "x" + Mathf.Abs(y);
                //Debug.Log("newFiled.tag pre : " + newFiled.tag);
                //Debug.Log("newFiled.tag init : " + nameFiled);
                //newFiled.tag = nameFiled;
                //Debug.Log("newFiled.tag=" + newFiled.tag.ToString());
                //Fields.Add(counter, newFiled);
                Fields.Add(nameFiled, newFiled);
                _counter++;
            }
        }

        Debug.Log("Field name init : " + _nameFiled);
    }

    private void DebugLog(string log)
    {
        Debug.Log(log);
    }

    private void DebugLogT(string log)
    {
        return;
        Debug.Log(log);
    }

    public void GenGrigLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    {
        int gridWidth = 100;
        int gridHeight = 100;
        //gridWidth = (int)GridX;
        //gridHeight = (int)GridY;

        //int maxVertical = (int)p_limitVerticalLook + 1;// *-1;
        //int maxHorizontal = (int)p_limitHorizontalLook + 1;

        if (Fields.Count != _counter || _counter == 0)
            return;

        if (_movement.x != 0)
        {
            int p_startPosY;
            int limitVertical;
            InitRange(p_PosHeroY, p_limitVerticalLook, gridHeight, out p_startPosY, out limitVertical);

            int LeftX = p_PosHeroX - (p_limitHorizontalLook / 2);
            int RightX = p_PosHeroX + (p_limitHorizontalLook / 2);
            int x = 0;
            int LeftRemoveX = LeftX - 1;
            int RightRemoveX = RightX + 1;
            //Validate ValidateRemoveX
            //bool isRemove = ValidateRemoveX(ref _movement, gridWidth, LeftRemoveX, RightRemoveX);
            //bool isAdded = ValidateAddedX(ref _movement, gridWidth, ref LeftX, ref RightX);
            bool isRemove = ValidateRemoveX(_movement, gridWidth, LeftRemoveX, RightRemoveX);
            bool isAdded = ValidateAddedX( _movement, gridWidth, LeftX, RightX);

            if (isRemove)
            {
                x = _movement.x > 0 ?
                    //Remove Vertical
                LeftRemoveX :
                RightRemoveX;
                
                string _nameFiled = "";
                for (int y = p_startPosY; y < limitVertical; y++)
                {
                    string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                    _nameFiled = nameFiled;
                    //Find
                    if (!Fields.ContainsKey(nameFiled))
                        continue;

                    GameObject findFiled = Fields[nameFiled];
                    //Destroy !!!
                    //Destroy(findFiled, 0.5f);
                    Destroy(findFiled);
                    Fields.Remove(nameFiled);
                    _counter--;
                }
            }

            if (isAdded)
            {
                x = _movement.x > 0 ?
                    //Added Vertical
                    RightX :
                    LeftX;

                string _nameFiled = "";
                for (int y = p_startPosY; y < limitVertical; y++)
                {
                    string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                    _nameFiled = nameFiled;

                    if (Fields.ContainsKey(nameFiled))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;
                    GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                    newFiled.name = nameFiled;
                    Fields.Add(nameFiled, newFiled);
                    _counter++;
                }
            }
        }

        if (_movement.y != 0)
        {
            int p_startPosX;
            int limitHorizontal;
            InitRange(p_PosHeroX, p_limitHorizontalLook, gridWidth, out p_startPosX, out limitHorizontal);

            int y = 0;
            int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
            int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
            int TopRemoveY = TopY - 1;
            int DownRemoveY = DownY + 1;
           
            //Validate
            //bool isRemove = ValidateRemoveY(ref _movement, gridHeight, TopRemoveY, DownRemoveY);
            //bool isAdded = ValidateAddedY(ref _movement, gridHeight, ref TopY, ref DownY);
            bool isRemove = ValidateRemoveY(_movement, gridHeight, TopRemoveY, DownRemoveY);
            bool isAdded = ValidateAddedY(_movement, gridHeight, TopY, DownY);

            if (isRemove)
            {
                y = _movement.y < 0 ?
                    //Remove Horizontal //#
                    TopRemoveY :
                    DownRemoveY; //#

                for (int x = p_startPosX; x < limitHorizontal; x++) //#
                {
                    string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                    //Find
                    if (!Fields.ContainsKey(nameFiled))
                        continue;

                    GameObject findFiled = Fields[nameFiled];
                    //Destroy !!!
                    //Destroy(findFiled, 0.5f);
                    Destroy(findFiled);
                    Fields.Remove(nameFiled);
                    _counter--;
                }
            }

            if (isAdded)
            {
                y = _movement.y < 0 ?
                    //Added Horizontal
                    DownY :
                    TopY; //#
                for (int x = p_startPosX; x < limitHorizontal; x++) //#
                {
                    string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);

                    if (Fields.ContainsKey(nameFiled))
                        continue;

                    Vector3 pos = new Vector3(x, y*(-1), 1) * Spacing;
                    pos.z = 0;
                    GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                    newFiled.name = nameFiled;
                    Fields.Add(nameFiled, newFiled);
                    _counter++;
                }
            }
        }
    }

    private static void InitRange(int p_PosHero, int p_limitLook, int gridSize, out int p_startPos, out int limit)
    {
        int maxSize = p_limitLook + 1;
        p_startPos = p_PosHero - (p_limitLook / 2);

        if (p_startPos < 0)
            p_startPos = 0;

        limit = p_startPos + maxSize;
        if (limit > gridSize)
            limit = gridSize;
    }



    //private static bool ValidateAddedY(ref Vector2 _movement, int gridHeight, ref int TopY, ref int DownY)
    private static bool ValidateAddedY(Vector2 _movement, int gridHeight,int TopY,int DownY)
    {
        if (TopY < 0 && _movement.y > 0)
            return false;
        if (DownY > gridHeight && _movement.y < 0)
            return false;

        return true;
    }

    //private static bool ValidateAddedX(ref Vector2 _movement, int gridWidth, ref int LeftX, ref int RightX)
    private static bool ValidateAddedX(Vector2 _movement, int gridWidth, int LeftX, int RightX)
    {
        if (RightX > gridWidth && _movement.x > 0)
            return false;

        if (LeftX < 0 && _movement.x < 0)
            return false;

        return true;
    }
    
    //private static bool ValidateRemoveY(ref Vector2 _movement, int gridHeight, int TopRemoveY, int DownRemoveY)
    private static bool ValidateRemoveY(Vector2 _movement, int gridHeight, int TopRemoveY, int DownRemoveY)
    {
        if (TopRemoveY < 0 && _movement.y < 0)
            return false;

        if (DownRemoveY > gridHeight && _movement.y > 0)
            return false;
        
        return true;
    }

    //private static bool ValidateRemoveX(ref Vector2 _movement, int gridWidth, int LeftRemoveX, int RightRemoveX)
    private static bool ValidateRemoveX(Vector2 _movement, int gridWidth, int LeftRemoveX, int RightRemoveX)
    {
        if (LeftRemoveX < 0 && _movement.x > 0)
            return false;

        if (RightRemoveX > gridWidth && _movement.x < 0)
            return false;

        return true;
    }

    //IEnumerator CreateFieldsAsync_()
    //{
    //    yield return null;
    //}

    public void CreateFields()
    {
        Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Field==" + sizeSpriteRendererField);

        float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
        float heightFiled = sizeSpriteRendererField.y;

        Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
        Debug.Log("size Panel==" + sizeSpriteRendererField);

        var scaleX = prefabPanel.transform.localScale.x;
        var scaleY = prefabPanel.transform.localScale.y;

        float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
        float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

        int widthLenght = (int)(widthPanel / widthFiled);
        int heightLenght = (int)(heightPanel / heightFiled);

        int maxLengthOfArray = widthLenght * heightLenght;
        Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
        int counter = 0;

        Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
        Debug.Log("panelLocation =" + panelLocation.ToString());

        prefabPanel.GetComponent<Renderer>().enabled = false;

        widthFiled = 1f;
        heightFiled = 1f;

        float offsetX = 1;
        float offsetY = 1;
        for (int heig = 0; heig < widthLenght; heig++)
        {
            for (int wid = 0; wid < heightLenght; wid++)
            {
                counter++;
                Vector2 newPos = new Vector2(offsetX, offsetY);
                GameObject newFiled = (GameObject)Instantiate(prefabField);
                newFiled.transform.position = new Vector2(offsetX, offsetY);
                Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());
                offsetX += widthFiled;

                //Fields.Add(counter, newFiled);
            }
            offsetX = 0;
            offsetY -= heightFiled;
        }
    }


    //---------------------

    public GameObject prefabCompas;
    public int numberOfObjects = 10;
    public float radius = 0.1f;

    void StartGenCircle()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            pos += new Vector3(10, -5, 0);

            Instantiate(prefabCompas, pos, Quaternion.identity);
            
        }
    }



   
   

    void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 100, 100), (int)(1.0f / Time.smoothDeltaTime));
        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());
        GUI.Label(new Rect(0, 30, 100, 100), _counter.ToString());
    }
   
}
