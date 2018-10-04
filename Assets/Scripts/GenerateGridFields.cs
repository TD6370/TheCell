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
    public SaveLoadData.GridData GridData;

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
                //string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                string nameFiled = GetNameFiled(x, y);
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
                    //string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                    string nameFiled = GetNameFiled(x, y);
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
                    //string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                    string nameFiled = GetNameFiled(x, y);
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
                    //#.D  LoadGameObjectDataForLook(nameFiled);
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
                    //string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                    string nameFiled = GetNameFiled(x, y);
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
                    //string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                    string nameFiled = GetNameFiled(x, y);

                    if (Fields.ContainsKey(nameFiled))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;
                    GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                    newFiled.name = nameFiled;
                    Fields.Add(nameFiled, newFiled);
                    _counter++;

                    LoadGameObjectActiveForLook(nameFiled);
                    //#.D  LoadGameObjectDataForLook(nameFiled);
                }
            }
        }
    }

    //Загрузка объектов из стека памяти на поле ADDED FOR LOOK
    //GamesObjectsActive -> listGameObjectReal
    private void LoadGameObjectActiveForLook(string p_nameFiled)
    {
        //return;

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
        }
        else
        {
            listGameObjectReal = GamesObjectsReal[p_nameFiled];
        }

        foreach (var gameObj in listGameObjectInField)
        {
            //Debug.Log("# LoadGameObjectActiveForLook REAL ++++++++ " + gameObj.name + " " + gameObj.tag + "  in  " + p_nameFiled );

            //# TYPE.1
            //GameObject newFiled = (GameObject)Instantiate(gameObj, gameObj.transform.position, Quaternion.identity);
            //newFiled.SetActive(true);

            //# TYPE.3
            //gameObj.SetActive(true);
            //GameObject newFiled = gameObj;

            //# TYPE.2
            GameObject newFiled = CreatePrefabByName(gameObj.tag, gameObj.name, gameObj.transform.position);

            //Fields.Add(nameFiled, newFiled);
            //# rem TYPE.3 
            listGameObjectReal.Add(newFiled);
            _counter++;
            //Debug.Log("# LoadGameObjectActiveForLook " + newFiled.name + " " + newFiled.tag + "  in  " + p_nameFiled + "  pos=" + gameObj.transform.position);
        }
    }

    //#.D загрузка из данныx объектов из памяти и создание их на поле  ADDED FOR LOOK - DATA
    private void LoadGameObjectDataForLook(string p_nameFiled)
    {
        //GridData
        if (GridData != null)
        {
            return;
        }

        if (GridData.Fields.Find(p => p.NameField == p_nameFiled) == null)
            return;

        List<SaveLoadData.ObjectData> listGameObjectInField = GridData.Fields.Find(p => p.NameField == p_nameFiled).Objects;
        List<GameObject> listGameObjectReal = new List<GameObject>();

        bool isExistFieldReal = false;
        if (!GamesObjectsReal.ContainsKey(p_nameFiled))
        {
            GamesObjectsReal.Add(p_nameFiled, listGameObjectReal);
        }
        else
        {
            listGameObjectReal = GamesObjectsReal[p_nameFiled];
        }

        foreach (var gameObj in listGameObjectInField)
        {
            GameObject newFiled = CreatePrefabByName(gameObj.TagObject, gameObj.NameObject, gameObj.Position);

            listGameObjectReal.Add(newFiled);
            _counter++;
        }
    }

    //загрузка из данныx объектов из памяти и создание их на поле  ADDED FOR LOOK - DATA 2
    private void LoadGameObjectDataForLook_2(string p_nameFiled)
    {
        //GridData
        if (GridData != null)
        {
            return;
        }

        if (!GridData.FieldsD.ContainsKey(p_nameFiled))
            return;

        List<SaveLoadData.ObjectData> listGameObjectInField = GridData.FieldsD[p_nameFiled].Objects;
        List<GameObject> listGameObjectReal = new List<GameObject>();

        bool isExistFieldReal = false;
        if (!GamesObjectsReal.ContainsKey(p_nameFiled))
        {
            GamesObjectsReal.Add(p_nameFiled, listGameObjectReal);
        }
        else
        {
            listGameObjectReal = GamesObjectsReal[p_nameFiled];
        }

        foreach (var gameObj in listGameObjectInField)
        {
            GameObject newFiled = CreatePrefabByName(gameObj.TagObject, gameObj.NameObject, gameObj.Position);

            listGameObjectReal.Add(newFiled);
            _counter++;
        }
    }

    //REMOVE FOR LOOK
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
            //foreach (var obj in activeObjects) 
            //{
            //    obj.SetActive(false);

            //    //# TYPE.3
            //    //_counter--;
            //}

            //# TYPE.3
            //return;

            List<GameObject> realObjects = GamesObjectsReal[p_nameFiled];

            //Debug.Log("RemoveRealObject TYPE.2 Save new position");
            //# TYPE.2 Save new position
            //for (int i = 0; i < activeObjects.Count; i++) 
            for (int i = activeObjects.Count - 1; i >= 0; i--)
            {
                //Debug.Log("RemoveRealObject TYPE.2 Save new position .2");

                activeObjects[i].SetActive(false); //#

                if (realObjects.Count <= i)
                    continue;
                if (realObjects[i] == null)
                    continue;

                //Debug.Log("RemoveRealObject TYPE.2 Save new position .3");

                var pos1 = activeObjects[i].transform.position;
                var pos2 = realObjects[i].transform.position;
                //if (pos1 != pos2)
                //{
                //    Debug.Log("RemoveRealObject pos1(" + pos1 + ") != pos2(" + pos2 + ") ");
                //    activeObjects[i].transform.position = realObjects[i].transform.position;
                //}
                //string posFieldOld = "Field" + (int)pos1.x + "x" + (int)Mathf.Abs(pos1.y);
                //string posFieldReal = "Field" + (int)pos2.x + "x" + (int)Mathf.Abs(pos2.y);
                var f = pos1.y;
                string posFieldOld = GetNameFiled(pos1.x, pos1.y);
                string posFieldReal = GetNameFiled(pos2.x, pos2.y);

                //---------------------------------------------
                if (posFieldOld != posFieldReal)
                {
                    Debug.Log("RemoveRealObject posFieldOld(" + posFieldOld + ") != posFieldReal(" + posFieldReal + ")      " + activeObjects[i].name + "    " + realObjects[i].name);
                    //activeObjects[i].transform.position = realObjects[i].transform.position;

                    //Debug.Log("RemoveRealObject ........... Remove in old Filed");
                    //Remove in old Filed
                    //activeObjects[i].SetActive(false);
                    activeObjects.RemoveAt(i);
                    //if (GamesObjectsActive[posFieldReal])
                    if (!GamesObjectsActive.ContainsKey(posFieldReal))
                    {
                        Debug.Log("RemoveRealObject Not new posFieldReal =" + posFieldReal);
                    }
                    else
                    {
                        //Debug.Log("RemoveRealObject ........... Add in new Filed");

                        //Add in new Filed
                        List<GameObject> activeObjectsNew = GamesObjectsActive[posFieldReal];

                        //Debug.Log("RemoveRealObject ........... Add in new Filed  pred=" + GamesObjectsActive[posFieldReal].Count);

                        //realObjects[i].SetActive(false);
                        //activeObjectsNew.Add(Instantiate(realObjects[i]));

                        //## var coyObj = Instantiate(realObjects[i]);
                        var realObj = realObjects[i];
                        var coyObj = CreatePrefabByName(realObj.tag, realObj.name, realObj.transform.position);

                        activeObjectsNew.Add(coyObj);
                        //coyObj.SetActive(false);

                        //Debug.Log("RemoveRealObject ........... Add in new Filed  post=" + GamesObjectsActive[posFieldReal].Count);
                        //activeObjectsNew[activeObjectsNew.Count-1].SetActive(false);
                    }
                }
                else
                {
                    //---------------------------------------------
                    //Save Real value in memory
                    activeObjects[i] = Instantiate(realObjects[i]); //#
                    activeObjects[i].SetActive(false); //#
                }
            }

            //#.D
            SaveListObjectsToData(p_nameFiled);

            foreach (var obj in realObjects)
            {
                _counter--;
                //# rem TYPE.3 
                Destroy(obj);
                //obj.SetActive(false);
            }
            GamesObjectsReal.Remove(p_nameFiled);

            //DebugLogT("RemoveRealObject objects in field ++++ " + p_nameFiled);


        }
    }

    //#.D //REMOVE FOR LOOK - DATA
    private void SaveListObjectsToData(string p_nameFiled)
    {
        //#timeclose
        return;

        List<GameObject> realObjects = GamesObjectsReal[p_nameFiled];
        //GridData
        if (GridData != null)
        {
            return;
        }

        if (!GridData.FieldsD.ContainsKey(p_nameFiled))
            return;

        List<SaveLoadData.ObjectData> dataObjects = GridData.FieldsD[p_nameFiled].Objects;

        for (int i = 0; i < realObjects.Count; i++)
        //for (int i = realObjects.Count - 1; i >= 0; i--) 
        {
            GameObject gobj = realObjects[i];
            SaveLoadData.ObjectData objData = SaveLoadData.CreateObjectData(gobj);
            if (dataObjects.Count >= i)
            {
                var pos1 = dataObjects[i].Position;
                var pos2 = realObjects[i].transform.position;
                var f = pos1.y;
                string posFieldOld = GetNameFiled(pos1.x, pos1.y);
                string posFieldReal = GetNameFiled(pos2.x, pos2.y);

                //---------------------------------------------
                if (posFieldOld != posFieldReal)
                {
                    Debug.Log("SaveListObjectsToData posFieldOld(" + posFieldOld + ") != posFieldReal(" + posFieldReal + ")      " + dataObjects[i].NameObject + "    " + realObjects[i].name);

                    //remove in Old Field
                    dataObjects.RemoveAt(i);

                    //add to new Field
                    if (!GridData.FieldsD.ContainsKey(posFieldReal))
                    {
                        Debug.Log("SaveListObjectsToData ADD new DIELD : " + posFieldReal);

                        GridData.FieldsD.Add(posFieldReal, new SaveLoadData.FieldData());
                    }
                    var otherObjects = GridData.FieldsD[posFieldReal].Objects;
                    otherObjects.Add(objData);
                }
                else
                {
                    //update
                    dataObjects[i] = objData;
                }
            }
        }
        //--------------------
        ////# List<SaveLoadData.ObjectData> listGameObjectInField2 = GridData.Fields.Find(p => p.NameField == p_nameFiled).Objects; 
        //int index = GridData.Fields.FindIndex(p => p.NameField == p_nameFiled);
        //if (index != 0)
        //    return;
        //List<SaveLoadData.ObjectData> listGameObjectInField2 = GridData.Fields[index].Objects;

        //for (int i = 0; i < realObjects.Count; i++)
        //{
        //    GameObject gobj = realObjects[i];
        //    SaveLoadData.ObjectData objData = SaveLoadData.CreateObjectData(gobj);
        //    if (listGameObjectInField2.Count >= i)
        //         listGameObjectInField2[i] = objData;
        //    //if (GridData.Fields[index].Objects.Count >=i)
        //    //    GridData.Fields[index].Objects[i] = objData;
        //}

    }

    //#.D ADD NEW GEN GAME OBJECT -- DATA
    private void SaveNewGameObjectToData(string p_nameFiled, GameObject p_saveObject)
    {
        DebugLog("# SaveNewAGameObjectInData " + p_saveObject.name + "  " + p_saveObject.tag);

        if (GridData != null)
        {
            return;
        }

        if (GridData.Fields.Find(p => p.NameField == p_nameFiled) == null)
            return;

        int indexFieldData = GridData.Fields.FindIndex(p => p.NameField == p_nameFiled);
        List<SaveLoadData.ObjectData> listDataObjectInField = GridData.Fields[indexFieldData].Objects;
        //List<SaveLoadData.ObjectData> listDataObjectInField = GridData.Fields.Find(p => p.NameField == p_nameFiled).Objects;


        List<GameObject> listGameObjectReal = new List<GameObject>();

        int indexObjectData = listDataObjectInField.FindIndex(p => p.NameObject == p_saveObject.name);
        //SaveLoadData.ObjectData objData = listDataObjectInField.Find(p => p.NameObject == p_saveObject.name);
        SaveLoadData.ObjectData objDataOld = listDataObjectInField[indexObjectData];


        var dataObjectSave = SaveLoadData.CreateObjectData(p_saveObject);
        int indexNN = listDataObjectInField.Count + 1;

        if (objDataOld == null)
        {
            //new object
            //Name prefab in design game
            // p_saveObject.tag + "_" + p_saveObject.name + "_" + p_nameFiled + indexNN;
            dataObjectSave.NameObject += indexNN;
            listDataObjectInField.Add(dataObjectSave);
        }
        else
        {
            //update object
            objDataOld = dataObjectSave;
        }
        _counter++;


        //---------------------------------
        //if (!GamesObjectsReal.ContainsKey(p_nameFiled))
        //{
        //    GamesObjectsReal.Add(p_nameFiled, listGameObjectReal);
        //}
        //else
        //{
        //    listGameObjectReal = GamesObjectsReal[p_nameFiled];
        //}

        //foreach (var gameObj in listDataObjectInField)
        //{
        //    GameObject newFiled = CreatePrefabByName(gameObj.TagObject, gameObj.NameObject, gameObj.Position);

        //    listGameObjectReal.Add(newFiled);
        //    _counter++;
        //}
    }

    //#.D ADD NEW GEN GAME OBJECT
    public void ActiveGameObject(GameObject p_saveObject)
    {
        DebugLog("# ActiveGameObject");

        int x = 0;
        int y = 0;
        x = (int)p_saveObject.transform.position.x;
        y = (int)Mathf.Abs(p_saveObject.transform.position.y);
        //string p_nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
        string p_nameFiled = GetNameFiled(x, y);

        AddNewActiveGameObject(p_nameFiled, p_saveObject);
        //#.D SaveNewGameObjectToData(p_nameFiled, p_saveObject);
    }

    //#.D ADD NEW GEN GAME OBJECT -- ACTIVE
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

    //-------------------------- 

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

    private GameObject FindPrefab(string namePrefab)
    {
        return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
    }

    public static string GetNameFiled(int x, int y)
    {
        return "Filed" + x + "x" + Mathf.Abs(y);
    }

    public static string GetNameFiled(System.Single x, System.Single y)
    {
        return "Filed" + (int)x + "x" + Mathf.Abs((int)y);
    }


    private GameObject CreatePrefabByName(string typePrefab, string namePrefab, Vector3 pos = new Vector3())
    {
        //Debug.Log("# CreatePrefabByName REAL ++++++++ " + namePrefab + " " + typePrefab + "  in  pos=" + pos);

        GameObject newPrefab = FindPrefab(typePrefab);
        GameObject newObjGame = (GameObject)Instantiate(newPrefab, pos, Quaternion.identity);
        newObjGame.name = namePrefab;
        //newObjGame.SetActive(false);
        return newObjGame;
    }

}
