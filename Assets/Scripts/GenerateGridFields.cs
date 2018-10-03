using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGridFields : MonoBehaviour {

    public GameObject prefabField;
    public GameObject prefabPanel;
    //public Dictionary<int, GameObject> Fields;
    public Dictionary<string, GameObject> Fields;
    public Dictionary<string, List<GameObject>> GamesObjectsActive;
    public Dictionary<string, List<GameObject>> GamesObjectsReal;

    public float GridX = 5f;
    public float GridY = 5f;
    public float Spacing = 2f;

    private int _counter;

	// Use this for initialization
	void Start () {
        //Fields = new Dictionary<int, GameObject>();
        Fields = new Dictionary<string, GameObject>();
        GamesObjectsActive = new Dictionary<string, List<GameObject>>();
        GamesObjectsReal = new Dictionary<string, List<GameObject>>();
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

    public void ActiveGameObject(GameObject p_saveObject)
    {
        DebugLog("# ActiveGameObject");

        int x = 0;
        int y = 0;
        x = (int)p_saveObject.transform.position.x;
        y = (int)Mathf.Abs(p_saveObject.transform.position.y);
        string p_nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
        AddNewActiveGameObject(p_nameFiled, p_saveObject);
    }

    private void AddNewActiveGameObject(string p_nameFiled, GameObject p_saveObject) 
    {
        DebugLog("# AddNewActiveGameObject " + p_saveObject.name + "  " + p_saveObject.tag);

        int index = 0;

        List<GameObject> gobjects = new List<GameObject>();
        //List<GameObject> gobjects;
        if (GamesObjectsActive.ContainsKey(p_nameFiled))
        {
            gobjects = GamesObjectsActive[p_nameFiled];
            index = gobjects.Count + 1; //.Find(p => p.tag == p_saveObject.tag);
            //var index = gobjects.Where(p => p.tag == p_saveObject.tag).Count;
        }
        else
        {
            gobjects = new List<GameObject>();
            GamesObjectsActive.Add(p_nameFiled, gobjects);
            gobjects = GamesObjectsActive[p_nameFiled]; //???
            index = 1;
        }
        
        p_saveObject.name = p_saveObject.tag + "_" + p_nameFiled + index;

        gobjects.Add(p_saveObject);
        DebugLog("# AddNewActiveGameObject Init +++ " + p_saveObject.name);
    }


    //Загрузка объектов из стека памяти на поле
    //GamesObjectsActive -> listGameObjectReal
    private void LoadGameObjectActiveForLook(string p_nameFiled)
    {
        //DebugLog("# LoadGameObjectActiveForLook");

        if (!GamesObjectsActive.ContainsKey(p_nameFiled))
        {
            //Debug.Log("LoadGameObjectActiveForLook Not in field : " + p_nameFiled);
            return;
        }

        //# Debug.Log("# LoadGameObjectActiveForLook : " + p_nameFiled);

        List<GameObject> listGameObjectInField = GamesObjectsActive[p_nameFiled];
        List<GameObject> listGameObjectReal = new List<GameObject>();

        bool isExistFieldReal = false;
        if (!GamesObjectsReal.ContainsKey(p_nameFiled))
        {
            //# Debug.Log("LoadGameObjectActiveForLook GamesObjectsReal add field - " + p_nameFiled);
            GamesObjectsReal.Add(p_nameFiled, listGameObjectReal);
        }else
        {
            listGameObjectReal = GamesObjectsReal[p_nameFiled];
        }

        foreach (var gameObj in listGameObjectInField)
        {
            //Debug.Log("# LoadGameObjectActiveForLook REAL ++++++++ " + gameObj.name + " " + gameObj.tag + "  in  " + p_nameFiled );

            //gameObj
            GameObject newFiled = (GameObject)Instantiate(gameObj, gameObj.transform.position, Quaternion.identity);
            newFiled.SetActive(true);
            //newFiled.name = nameFiled;
            //Fields.Add(nameFiled, newFiled);
            listGameObjectReal.Add(newFiled);
            _counter++;
            //Debug.Log("# LoadGameObjectActiveForLook " + newFiled.name + " " + newFiled.tag + "  in  " + p_nameFiled + "  pos=" + gameObj.transform.position);
        }
    }

    private void RemoveRealObject(string p_nameFiled)
    {
        if (!GamesObjectsReal.ContainsKey(p_nameFiled))
        {
            //Debug.Log("RemoveRealObject Not in field : " + p_nameFiled);
            return;
        }
        else
        {
            
            List<GameObject> activeObjects = GamesObjectsActive[p_nameFiled];
            foreach (var obj in activeObjects) 
            {
                obj.SetActive(false);
            }
            List<GameObject> realObjects = GamesObjectsReal[p_nameFiled];
            foreach (var obj in realObjects)
            {
                Destroy(obj);
                //obj.SetActive(false);
            }
            GamesObjectsReal.Remove(p_nameFiled);

            DebugLogT("RemoveRealObject objects in field ++++ " + p_nameFiled);
        }
    }

    //private void DeactivateGameObjectForLook(string p_nameFiled)
    //{
    //    //DebugLog("# DeactivateGameObjectForLook");

    //    if (!GamesObjectsActive.ContainsKey(p_nameFiled))
    //    {
    //        //DebugLog("DeactivateGameObjectForLook Not in field : " + p_nameFiled);
    //        return;
    //    }

    //    Debug.Log("# CreateGameObjectActiveForLook : " + p_nameFiled);

    //    List<GameObject> listGameObjectInField = GamesObjectsActive[p_nameFiled];
    //    foreach (var gameObj in listGameObjectInField)
    //    {
    //        //Destroy();// gameObj.SetActive(false);
    //        _counter--;
    //        DebugLog("# CreateGameObjectActiveForLook " + newFiled.name + " " + newFiled.tag + "  in  " + p_nameFiled);
    //    }
    //}

    //private void CreateGameObjectActive(string p_nameFiled)
    //{
   
    //}

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
                //Debug.Log("Gen field pos=" + pos + "   Spacing=" + Spacing);
                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                newFiled.tag = "Field";
                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                //Debug.Log("Field name init : " + nameFiled);
                newFiled.name = nameFiled;
                _nameFiled = nameFiled;
                //Debug.Log("newFiled.tag init : " + nameFiled);
                Fields.Add(nameFiled, newFiled);
                _counter++;
            }
        }

        Debug.Log("Field name init : " + _nameFiled);
    }


    private bool m_onLoadFields = false;
    public void GenGrigLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    {
        int gridWidth = 100;
        int gridHeight = 100;
        //gridWidth = (int)GridX;
        //gridHeight = (int)GridY;
        int countFiled = (int)GridX * (int)GridY;

        //if (Fields.Count != _counter || _counter == 0)
        if (!m_onLoadFields && (Fields.Count < countFiled || countFiled == 0))
        {
            //Debug.Log("!!!!! Fields.Count =" + Fields.Count + "   _counter =" + _counter);
            Debug.Log("!!!!! Fields.Count =" + Fields.Count + "   countFiled =" + countFiled);
            return;
        }
        {
            m_onLoadFields = true;
        }

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
            bool isRemove = ValidateRemoveX(_movement, gridWidth, LeftRemoveX, RightRemoveX);
            bool isAdded = ValidateAddedX(_movement, gridWidth, LeftX, RightX);

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
                    RemoveRealObject(nameFiled);
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

                    LoadGameObjectActiveForLook(nameFiled);
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
                    Destroy(findFiled);
                    Fields.Remove(nameFiled);
                    RemoveRealObject(nameFiled);
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

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;
                    GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                    newFiled.name = nameFiled;
                    Fields.Add(nameFiled, newFiled);
                    _counter++;

                    LoadGameObjectActiveForLook(nameFiled);

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


    private void DebugLog(string log)
    {
        Debug.Log(log);
    }

    private void DebugLogT(string log)
    {
        return;
        Debug.Log(log);
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 100, 100), (int)(1.0f / Time.smoothDeltaTime));
        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());
        GUI.Label(new Rect(0, 30, 100, 100), _counter.ToString());
    }
   
}
