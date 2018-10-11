using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenerateGridFields : MonoBehaviour {

    public GameObject prefabField;

    //@ST@
    //public Dictionary<string, GameObject> Fields;
    //public Dictionary<string, List<GameObject>> GamesObjectsReal;
    //public Dictionary<string, List<SaveLoadData.ObjectData>> GamesObjectsPersonalData;

    //public List<GameObject> PoolGamesObjects;
    //@ST@ public SaveLoadData.GridData GridData;
    
    private SaveLoadData _sctiptData;
    

    public float GridX = 5f;
    public float GridY = 5f;
    public float Spacing = 2f;

    private int _counter;
    public int Counter
    {
        get { return _counter; }
        set { _counter = value; }
    }

	// Use this for initialization
	void Start () {
        //@ST@
        //Fields = new Dictionary<string, GameObject>();
        //GamesObjectsReal = new Dictionary<string, List<GameObject>>();
        //GamesObjectsPersonalData = new Dictionary<string, List<SaveLoadData.ObjectData>>();

        _sctiptData = GetComponent<SaveLoadData>();
        if (_sctiptData == null)
            Debug.Log("GenerateGridFields.Start : sctiptData not load !!!");

        //@ST@ StartGenGrigField();

        //LoadPoolGameObjects();

        //@ST@ Storage.SetGamesObjectsReal(GamesObjectsReal);
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

    //@ST@
    public void StartGenGrigField()
    {
        int maxWidth = (int)GridY * -1;
        int maxHeight = (int)GridX;
        Counter = maxWidth * maxHeight;
        Debug.Log("counter=" + Counter.ToString());
        Counter = 0;
        string _nameField = "";

        for (int y = 0; y > maxWidth; y--)
        {
            for (int x = 0; x < maxHeight; x++)
            {

                Vector3 pos = new Vector3(x, y, 1) * Spacing;
                //Vector2 pos = new Vector2(x, y) * spacing;
                pos.z = 0;
                //Debug.Log("Gen field pos=" + pos + "   Spacing=" + Spacing);
                GameObject newField = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                newField.tag = "Field";
                string nameField = Storage.GetNameField(x, y);
                newField.name = nameField;
                _nameField = nameField;
                Storage.Instance.Fields.Add(nameField, newField);
                Counter++;
            }
        }

        Debug.Log("Pole Field name init : " + _nameField);
    }


    private bool m_onLoadFields = false;
    public void GenGrigLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    {
        var _fields = Storage.Instance.Fields;

        int gridWidth = 100;
        int gridHeight = 100;
        //gridWidth = (int)GridX;
        //gridHeight = (int)GridY;
        int countField = (int)GridX * (int)GridY;

        //if (Fields.Count != _counter || _counter == 0)
        if (!m_onLoadFields && (Storage.Instance.Fields.Count < countField || countField == 0))
        {
            //Debug.Log("!!!!! Fields.Count =" + Fields.Count + "   _counter =" + _counter);
            Debug.Log("!!!!! Fields.Count =" + _fields.Count + "   countField =" + countField);
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

                string _nameField = "";
                for (int y = p_startPosY; y < limitVertical; y++)
                {
                    //string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
                    string nameField = Storage.GetNameField(x, y);
                    _nameField = nameField;
                    //Find
                    if (!_fields.ContainsKey(nameField))
                        continue;

                    GameObject findField = _fields[nameField];
                    //Destroy !!!
                    DestroyField(findField);
                    _fields.Remove(nameField);
                    RemoveRealObjects(nameField);
                    Counter--;
                }
            }

            if (isAdded)
            {
                x = _movement.x > 0 ?
                    //Added Vertical
                    RightX :
                    LeftX;

                string _nameField = "";
                for (int y = p_startPosY; y < limitVertical; y++)
                {
                    string nameField = Storage.GetNameField(x, y);
                    _nameField = nameField;

                    if (_fields.ContainsKey(nameField))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;
                    //GameObject newField = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                    GameObject newField = GetPrefabField(pos);
                    newField.name = nameField;
                    _fields.Add(nameField, newField);
                    Counter++;

                    LoadObjectToReal(nameField);
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
                    string nameField = Storage.GetNameField(x, y);

                    //Find
                    if (!_fields.ContainsKey(nameField))
                        continue;

                    GameObject findField = _fields[nameField];
                    //Destroy !!!
                    DestroyField(findField);
                    _fields.Remove(nameField);
                    RemoveRealObjects(nameField);
                    Counter--;
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
                    string nameField = Storage.GetNameField(x, y);

                    if (_fields.ContainsKey(nameField))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;
                    //GameObject newField = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                    GameObject newField = GetPrefabField(pos);
                    newField.name = nameField;
                    _fields.Add(nameField, newField);
                    Counter++;

                    LoadObjectToReal(nameField);
                }
            }
        }
    }

    private void LoadObjectToReal(string nameField)
    {
        //LoadGameObjectActiveForLook(nameFiled);
        //#.D  
        //LoadGameObjectDataForLook(nameFiled);
        LoadGameObjectDataForLook_2(nameField);
    }

    //загрузка из данныx объектов из памяти и создание их на поле  ADDED FOR LOOK - DATA 2
    private void LoadGameObjectDataForLook_2(string p_nameField)
    {
        var _gridData = Storage.Instance.GridDataG;
        var _gamesObjectsReal = Storage.Instance.GamesObjectsReal;

        //#TEST
        string indErr = "";
        try
        {
            indErr = "1.";
            //GridData
            if (_gridData == null || _gridData.FieldsD == null)
            {
                Debug.Log(" LoadGameObjectDataForLook_2 GridData IS EMPTY !!!");
                return;
            }
            indErr = "2.";
            if (!_gridData.FieldsD.ContainsKey(p_nameField))
                return;

            indErr = "3.";
            List<SaveLoadData.ObjectData> listDataObjectInField = _gridData.FieldsD[p_nameField].Objects;
            indErr = "4.";
            List<GameObject> listGameObjectReal = new List<GameObject>();

            indErr = "5.";
            if (!_gamesObjectsReal.ContainsKey(p_nameField))
            {
                indErr = "6.";
                _gamesObjectsReal.Add(p_nameField, listGameObjectReal);
            }
            else
            {
                indErr = "7.";
                listGameObjectReal = _gamesObjectsReal[p_nameField];
            }

            //#TEST
            string saveNextName = "";
            

            indErr = "8.";

            if (listDataObjectInField.Count == 0)
            {
                //Debug.Log("LoadGameObjectDataForLook ----- listDataObjectInField (" + p_nameField + ") count==0");
                return;
            }
            //Debug.Log("LoadGameObjectDataForLook ----- listDataObjectInField (" + p_nameField + ") count==" + listDataObjectInField.Count);

           
            //foreach (var dataObj in listDataObjectInField)
            for (int i = listDataObjectInField.Count - 1; i >= 0; i--)
            {
                indErr = "9. ind=" + i;
                var dataObj = listDataObjectInField[i];
                indErr = "10.";
                //TEST -------
                if (saveNextName != dataObj.NameObject)
                {
                    indErr = "11.";
                    saveNextName = dataObj.NameObject;
                }
                else
                {
                    indErr = "12.";
                    Debug.Log("LoadGameObjectDataForLook ********************** " + saveNextName + " is dublicate !!!!");
                    foreach (var obj in listDataObjectInField)
                    {
                        Debug.Log("listDataObjectInField: --------------------------------------------- " + obj.ToString());
                    }
                    //fix @KOSTIL@
                    listDataObjectInField.RemoveAt(i);
                    continue;
                }
                //--------------
                indErr = "13.";
                //TEST -------
                if (dataObj.IsReality)
                {
                    Debug.Log("LoadGameObjectDataForLook ********************** " + dataObj + " already IsReality !!!!");

                    int indTest = listGameObjectReal.FindIndex(p => p.name == dataObj.NameObject);
                    if (indTest != -1)
                    {
                        Debug.Log("LoadGameObjectDataForLook ********************** " + dataObj + " already EXIST !!!!");
                    }
                    else
                    {
                        Debug.Log("LoadGameObjectDataForLook ********************** " + dataObj + " not EXIST %)");
                    }
                    Storage.Instance.GetHistory(dataObj.NameObject);
                    continue;
                }
                //--------------

                indErr = "14.";
                dataObj.IsReality = true;

                //#PPP TEST
                //Debug.Log("TEST ########### LoadGameObjectDataForLook_2 added +++ : " + gameObj.NameObject);

                indErr = "15.";

                //GameObject newField = CreatePrefabByName(gameObj.TagObject, gameObj.NameObject, gameObj.Position);
                GameObject newField = CreatePrefabByName(dataObj);

                indErr = "16.";
                //Debug.Log(" LoadGameObjectDataForLook_2 added +++ : " + gameObj.NameObject);
                listGameObjectReal.Add(newField);
                Counter++;
            }
            indErr = "17.end.";
        }
        catch (Exception x)
        {
            Debug.Log("################ ERROR LoadGameObjectDataForLook_2 : " + x.Message + "   #" + indErr);
        }
    }

    public void DestroyRealObject(GameObject gObj)
    {
        if (gObj == null)
            return;

        //Debug.Log("DestroyRealObject... ");
        string nameField = Storage.GetNameFieldByName(gObj.name);
        if (nameField == null)
            return;

        List<GameObject> listObjInField = Storage.Instance.GamesObjectsReal[nameField];

        for (int i = listObjInField.Count - 1; i >= 0; i--)
        {
            if (listObjInField[i] == null)
            {
                listObjInField.RemoveAt(i);
            }
        }
        if (listObjInField.Count > 0)
        {
            int indRealData = listObjInField.FindIndex(p => p.name == gObj.name);
            if (indRealData == -1)
            {
                Debug.Log("Hero destroy >>> Not find GamesObjectsReal : " + gObj.name);
            }
            else
            {
                //Debug.Log(" KILL 1. GOR: " + GamesObjectsReal[nameField][indRealData].name);

                Storage.Instance.GamesObjectsReal[nameField].RemoveAt(indRealData);
            }
        }

        //Debug.Log(" KILL 2. GO: " + gObj.name);
        Destroy(gObj);

        Storage.Instance.KillObject.Add(gObj.name);
        //-----------------------------------------------

        //Destrot to Data
        if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
        {
            Debug.Log("!!!! DestroyRealObject !GridData.FieldsD not field=" + nameField);
            return;
        }
        List<SaveLoadData.ObjectData> dataObjects = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
        int indObj = dataObjects.FindIndex(p => p.NameObject == gObj.name);

        //@KOSTIL
        //if (indObj == -1)
        //{
        //    int len1 = gObj.name.Length;
        //    string leftNameFind = gObj.name.Substring(0, len1 - 2);
        //    for (int i = 0; i < dataObjects.Count; i++ )
        //    {
        //        Debug.Log("!!>> DestroyRealObject FIND...");
        //        int len = dataObjects[i].NameObject.Length;
        //        string leftNameItem = dataObjects[i].NameObject.Substring(0, len - 2);
        //        Debug.Log("!!>> DestroyRealObject FIND: " + leftNameFind + " <> " + leftNameItem);
        //        if (leftNameFind == leftNameItem)
        //        {
        //            indObj = i;
        //            break;
        //        }
        //    }
        //}

        if (indObj == -1)
        {
            Debug.Log("!!!! DestroyRealObject GridData not object=" + gObj.name);
            //foreach (var item in dataObjects) 
            //{
            //    Debug.Log(":::: DestroyRealObject GridData Exist objects : " + item.NameObject);
            //}
        }
        else
        {
            //Debug.Log(" KILL 3. DO:" + dataObjects[indObj].ToString());
            dataObjects.RemoveAt(indObj);
            //Debug.Log("DestroyRealObject +++ " + gObj.name);
        }
    }

    //private void RemoveRealObjects(string p_nameField)
    //{ 
    //    //RemoveRealObject_Active(p_nameFiled);
    //    RemoveRealObjects_Data(p_nameField);
    //}

    //REMOVE FOR LOOK
    //private void RemoveRealObjects_Data(string p_nameField)
    private void RemoveRealObjects(string p_nameField)
    {
        if (!Storage.Instance.GamesObjectsReal.ContainsKey(p_nameField))
        {
            //Debug.Log("RemoveRealObject Not in field : " + p_nameFiled);
            return;
        }
        else
        {
             //#.D_2
            SaveListObjectsToData(p_nameField, true);

            List<GameObject> realObjects = Storage.Instance.GamesObjectsReal[p_nameField];
            foreach (var obj in realObjects)
            {
                Counter--;
                Destroy(obj);
            }
            //@<<@ Storage.Instance.GamesObjectsReal.Remove(p_nameField);
            Storage.Instance.RemoveFieldRealObject(p_nameField, "RemoveRealObjects");
        }
    }

    //#.D //UPDATE FOR LOOK - DATA_2
    //private void SaveListObjectsToData(string p_nameField)
    private void SaveListObjectsToData(string p_nameField, bool isDestroy = false)
    {
        var _gamesObjectsReal = Storage.Instance.GamesObjectsReal;
        var _gridData = Storage.Instance.GridDataG;

        string indErr = "start";

        if (!_gamesObjectsReal.ContainsKey(p_nameField))
        {
            Debug.Log("################# SaveListObjectsToData GamesObjectsReal Not field=" + p_nameField);
            return;
        }

        indErr = "start";
        List<GameObject> realObjects = _gamesObjectsReal[p_nameField];

        if (_gridData == null)
        {
            Debug.Log("SaveListObjectsToData GridData is EMPTY");
            return;
        }

        if (!_gridData.FieldsD.ContainsKey(p_nameField))
        {
            Debug.Log("SaveListObjectsToData !GridData.FieldsD not field=" + p_nameField);
            return;
        }

        //@<<@ 
        List<SaveLoadData.ObjectData> dataObjects = _gridData.FieldsD[p_nameField].Objects;
        try
        {
            indErr = "1.";

            for (int i = realObjects.Count - 1; i >= 0; i--)  //+FIX
            {
                indErr = "2.";
                GameObject gobj = realObjects[i];
                if (gobj == null)
                {
                    Debug.Log("################# SaveListObjectsToData   REMOVE  GameObject   field:" + p_nameField + "  ind:" + i);
                    continue;
                }

                indErr = "3.";
                //SaveLoadData.ObjectData dataObj = dataObjects.Find(p => p.NameObject == gobj.name);
                int indData = dataObjects.FindIndex(p => p.NameObject == gobj.name);

                if (indData == -1)
                {
                    Debug.Log("################# SaveListObjectsToData 1.  DataObject (" + gobj.name + ") not Find in DATA     field: " + p_nameField);
                    Storage.Instance.GetHistory(gobj.name);
                    continue;
                }
                
                SaveLoadData.ObjectData dataObj = dataObjects[indData];
                if (dataObj == null)
                {
                    Debug.Log("################# SaveListObjectsToData 2.  DataObject (" + gobj.name + ") not Find in DATA     field: " + p_nameField);
                    Storage.Instance.GetHistory(gobj.name);
                    continue;
                }

                indErr = "3.7.";
                var posD = dataObj.Position;
                var posR = realObjects[i].transform.position; //!!!!
                
                indErr = "6.";
                string posFieldOld = Storage.GetNameFieldPosit(posD.x, posD.y);
                indErr = "7.";
                string posFieldReal = Storage.GetNameFieldPosit(posR.x, posR.y);

                if (posFieldOld != p_nameField)
                {
                    Debug.Log("################# SaveListObjectsToData !!!!!!!!!!!! posFieldOld != p_nameField");
                }
                indErr = "8.";
                //---------------------------------------------
                if (posFieldOld != posFieldReal)
                {
                    indErr = "10.";

                    //@<<@ dataObjects.RemoveAt(i);
                    Storage.Instance.RemoveDataObjectInGrid(p_nameField, i, "SaveListObjectsToData"); ////@<<@ 

                    indErr = "11.";
                    //add to new Field
                    if (!_gridData.FieldsD.ContainsKey(posFieldReal))
                    {
                        indErr = "12.";
                        //@<<@ _gridData.FieldsD.Add(posFieldReal, new SaveLoadData.FieldData());
                        Storage.Instance.AddNewFieldInGrid(posFieldReal, "SaveListObjectsToData"); //@<<@ 
                    }

                    indErr = "13.";
                    Debug.Log("______________________________________CALL CreateObjectData 1. @@NEW_________________________");

                    indErr = "14.";
                    dataObj.NameObject = Storage.CreateName(dataObj.TagObject, posFieldReal, gobj.name);

                    //Debug.Log(info);
                    indErr = "15.";
                    if (isDestroy)
                        dataObj.IsReality = false;

                    indErr = "16.";
                    //#TEST -----------------
                    var objTest = _gridData.FieldsD[posFieldReal].Objects.Find(p => p.NameObject == dataObj.NameObject);
                    if (objTest != null)
                    {
                        Debug.Log("################# Error SaveListObjectsToData GridData.FieldsD[" + posFieldReal + "].Objects already Exist : " + objTest);
                        continue;
                    }
                    //-----------------------------------

                    indErr = "17.";
                    Storage.Instance.AddDataObjectInGrid(dataObj, posFieldReal, "SaveListObjectsToData"); //@<<@ 
                }
                else
                {

                    //------------------------------------
                    /*
                    indErr = "18.";
                    //Create on Game Object
                    //Debug.Log("______________________________________CALL CreateObjectData 2. @@NEW_________________________");
                    SaveLoadData.ObjectData objData = SaveLoadData.CreateObjectData(gobj);

                    indErr = "18.5";
                    if (objData == null)
                    {
                        //Debug.Log("################# Error SaveListObjectsToData objData is NULL for : " + gobj.name);
                        continue;
                    }

                    indErr = "19.";
                    //update
                    if (isDestroy)
                    {

                        objData.IsReality = false;
                    }

                    indErr = "20.";

                    //@<<@ dataObjects[i] = objData;
                    Storage.Instance.UpdateDataObect(p_nameField, i, objData); //@<<@ 
                     */
                    //------------------------------------
                    //update
                    if (isDestroy)
                        dataObj.IsReality = false;
                   
                    //@@@TEST Storage.Instance.UpdateDataObect(p_nameField, indData, dataObj, "SaveListObjectsToData"); //@<<@ 
                    //------------------------------------
                }
               
            }

        }
        catch (Exception x)
        {
            Debug.Log("ERROR SaveListObjectsToData : " + x.Message + "  #" + indErr);
        }
    }

    //ADD NEW GEN GAME OBJECT 
    public void ActiveGameObject(GameObject p_saveObject)
    {
        int x = 0;
        int y = 0;
        //int i = UnityEngine.Random.Range(0, 100);
        //string id = Guid.NewGuid().ToString();
        //id = id.Substring(1, 4);

        x = (int)p_saveObject.transform.position.x;
        y = (int)Mathf.Abs(p_saveObject.transform.position.y);
        //string p_nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
        string p_nameField = Storage.GetNameFieldPosit(x, y);

        //p_saveObject.name = SaveLoadData.CreateName(p_saveObject.tag, p_nameField, id);
        p_saveObject.name = Storage.CreateName(p_saveObject.tag, p_nameField, "", p_saveObject.name);
        SaveNewGameObjectToData(p_nameField, p_saveObject);
    }

    private void SaveNewGameObjectToData(string p_nameField, GameObject p_saveObject)
    {
        var _gamesObjectsReal = Storage.Instance.GamesObjectsReal;
        var _gridData = Storage.Instance.GridDataG;

        if (_gridData == null)
        {
            Debug.Log("SaveNewGameObjectToData_2 GridData is EMPTY");
            return;
        }

        //Debug.Log("ActiveGameObject 9." + p_saveObject.name );

        if (!_gridData.FieldsD.ContainsKey(p_nameField))
        {
            //@<<@ _gridData.FieldsD.Add(p_nameField, new SaveLoadData.FieldData());
            Storage.Instance.AddNewFieldInGrid(p_nameField, "SaveNewGameObjectToData");
        }

        List<SaveLoadData.ObjectData> listDataObjectInField = _gridData.FieldsD[p_nameField].Objects;
        if (listDataObjectInField == null)
        {
            Debug.Log("******************* Error SaveNewGameObjectToDat for field: " + p_nameField + " ---  Data Objects  is NULL");
            return;
        }

        //Debug.Log("______________________________________CALL CreateObjectData 3. @@Create_________________________");
        SaveLoadData.ObjectData dataObjectSave = SaveLoadData.CreateObjectData(p_saveObject);

        if (dataObjectSave == null)
        {
            Debug.Log("******************* Error SaveNewGameObjectToDat  --- CreateObjectData NEW is NULL ---- for gobj: " + p_saveObject);
            return;
        }

        //dataObjectSave.NameObject = SaveLoadData.CreateName(dataObjectSave.TagObject, p_nameField, listDataObjectInField.Count);
        //p_saveObject.name = dataObjectSave.NameObject;  //+FIX
        //dataObjectSave.UpdateGameObject(p_saveObject);

        SaveLoadData.ObjectData objDataOld = null;

        int indexObjectData = -1;

        if (listDataObjectInField.Count > 0)
        {
            indexObjectData = listDataObjectInField.FindIndex(p => p.NameObject == dataObjectSave.NameObject);
            if (indexObjectData != -1)
                objDataOld = listDataObjectInField[indexObjectData];
        }

        if (objDataOld == null)
        {
            //Debug.Log("ActiveGameObject PRED lost count=" + GridData.FieldsD[p_nameField].Objects.Count);
            //-------------------------- Add object in Data
            
            //@<<@ listDataObjectInField.Add(dataObjectSave);
            Storage.Instance.AddDataObjectInGrid(dataObjectSave, p_nameField, "SaveNewGameObjectToData"); //@<<@


            //SaveLoadData.ObjectData objData = dataObjectSave;
            //Debug.Log("ActiveGameObject POST lost count=" + GridData.FieldsD[p_nameField].Objects.Count);
        }
        else
        {
            //dataObjectSave.NameObject = SaveLoadData.CreateName(dataObjectSave.TagObject, p_nameField, listDataObjectInField.Count + 1);
            //+FIX
            dataObjectSave.NameObject = Storage.CreateName(dataObjectSave.TagObject, p_nameField, "", p_saveObject.name);

            //update object
           
            //@<<@ listDataObjectInField[indexObjectData] = dataObjectSave;
            Storage.Instance.UpdateDataObect(p_nameField, indexObjectData, dataObjectSave, "SaveNewGameObjectToData"); //@<<@ 

            Debug.Log("# SaveNewGameObjectToData_2  ADD NEW GEN OBJECT -- SAVE TO OLD: " + dataObjectSave.NameObject);
        }

        //-------------------------- Add object in Real list
        if (!_gamesObjectsReal.ContainsKey(p_nameField))
        {
            //@<<@  _gamesObjectsReal.Add(p_nameField, new List<GameObject>());
            Storage.Instance.AddNewFieldInRealObject(p_nameField, "SaveNewGameObjectToData"); //@<<@ 
        }

        //@<<@ _gamesObjectsReal[p_nameField].Add(p_saveObject);
        Storage.Instance.AddRealObject(p_saveObject, p_nameField, "SaveNewGameObjectToData"); //@<<@ 


        //@TEST
        //List<GameObject> objectsReal = _gamesObjectsReal[p_nameField];
        //var objG = objectsReal[objectsReal.Count() -1 ];
        //objG.name = "NAME_TEST_TEST";
        //@POS@ Debug.Log("ActiveGameObjec. =================== TEST SAVE REAL OBJ        objectsReal: " + objG.name + "       GameObject: " + p_saveObject.name);

        Counter++;

        //Debug.Log("ActiveGameObject 9." + p_saveObject.name + "   [" + dataObjectSave.NameObject + "]   FIELD: " + p_nameField);
    }
    //private void SavePersonalData()
    //{

    //}
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


    //private void DebugLog(string log)
    //{
    //    Debug.Log(log);
    //}

    //private void DebugLogT(string log)
    //{
    //    return;
    //    Debug.Log(log);
    //}

    

    private GameObject FindPrefab(string namePrefab)
    {
        //return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
        //return sctiptData.FindPrefabHieracly(namePrefab);
        if (_sctiptData == null)
            return null; 

        return _sctiptData.FindPrefab(namePrefab);
    }

    //const string FieldKey = "Field";

    //public static string GetNameField(int x, int y)
    //{
    //    return FieldKey + x + "x" + Mathf.Abs(y);
    //}

    //public static string GetNameField(System.Single x, System.Single y)
    //{
    //    return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    //}

    //public static string GetNameFieldPosit(int x, int y)
    //{
    //    x = (int)(x / 2);
    //    y = (int)(y / 2);
    //    return FieldKey + x + "x" + Mathf.Abs(y);
    //}

    //public static string GetNameFieldPosit(System.Single x, System.Single y)
    //{
    //    x = (int)(x / 2);
    //    y = (int)(y / 2);
    //    return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    //}

    //+++ CreateObjectData +++ LoadObjectForLook
    private GameObject CreatePrefabByName(SaveLoadData.ObjectData objData)
    {
        string typePrefab = objData.TagObject;
        string namePrefab = objData.NameObject;
        Vector3 pos = objData.Position;

        //#TEST #PREFABF
        //---------------- 1.
        //GameObject newPrefab = FindPrefab(typePrefab);
        //GameObject newObjGame = (GameObject)Instantiate(newPrefab, pos, Quaternion.identity);
        //----------------2.
        GameObject newObjGame = FindPrefab(typePrefab);

        //var rb = newObjGame.GetComponent<Rigidbody2D>();
        //if (rb != null)
        //{
        //    Debug.Log("CreatePrefabByName CreatePrefabByName Set position 1........");
        //    rb.MovePosition(pos);
        //}
        //else
        //{
            newObjGame.transform.position = pos; //@!@.1
        //}
        //----------------
        SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;

        if (!String.IsNullOrEmpty(newObjGame.tag))
            prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), newObjGame.tag.ToString()); ;

        switch (prefabType)
        {
            case SaveLoadData.TypePrefabs.PrefabUfo:
                var objUfo = objData as SaveLoadData.GameDataUfo;
                if (objUfo != null)
                {
                    //LoadObjectForLook:  DATA -->> PERSONA #P#
                    //@POS@ Debug.Log("CreatePrefab ______________________LOAD DATA __________________" + newObjGame.name + "       DATA:" + objUfo.ToString());
                    objUfo.UpdateGameObject(newObjGame);
                }
                else
                {
                    Debug.Log("CreatePrefabByName... (" +  objData.NameObject + ")  objData not is ObjectDataUfo !!!!");
                }
                break;
            default:
                break;
        }
        //.............

        newObjGame.name = namePrefab;
        //newObjGame.SetActive(false);
        return newObjGame;
    }

    public void SaveAllRealGameObjects()
    {
        foreach (var realObjGame in Storage.Instance.GamesObjectsReal)
        {
            string nameField = realObjGame.Key;
            //RemoveRealObjects(nameField);
            SaveListObjectsToData(nameField);
        }
    }

    public void LoadObjectsNearHero()
    {
        Debug.Log("______________________LoadObjectsNearHero__________________");

        foreach (var field in Storage.Instance.Fields)
        {
            string nameField = field.Key;
            LoadObjectToReal(nameField);
        }
    }


    //static Type CompType;
    private GameObject GetPrefabField(Vector3 pos)
    {
        //#PRED 
        GameObject resGO = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);

        //---------For Pool ------------------------------------------------------------------
        /*
        List<string> componentWork = new List<string> { "SpriteRenderer", "", "" };
        GameObject prefabGO = prefabField;
        GameObject resGO = GetPoolGameObject();
        if (resGO = null)
        {
            Debug.Log("~~~~~~~~ GetPrefabField  NOT pool !!!");
            GameObject newGO = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
        }
        else
        {
            //Debug.Log("~~~~~~~~ GetPrefabField yes pool");
            //resGO = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);

            //var components = prefabGO.GetComponents<Component>();
            //for (int i = 0; i < components.Length; ++i) {
            //    if(components[i]!=null)
            //    {
            //        //Component CompType = components[i].GetType();
            //        //Component comp = components[i];
            //        //var t = typeof(comp);
            //        //resGO.AddComponent<comp>();

            //        Type compType = components[i].GetType();
            //        string compTypeName = compType.Name;
            //        if (componentWork.Contains(compTypeName))
            //        {
            //            if (compTypeName == "SpriteRenderer")
            //            {
            //                Debug.Log("~~~~~~~~~ Clone component type:" + compTypeName);
            //                var compNew = resGO.GetComponent<SpriteRenderer>();
            //                if (compNew == null)
            //                    compNew = resGO.AddComponent<SpriteRenderer>();
            //                compNew = (SpriteRenderer)components[i];
            //                //print (CompType);
            //            }
            //        }
            //    }
            //}

            //resGO = prefabGO;
            var countComp = resGO.GetComponents<Component>().Length;
            Debug.Log("~~~~~~~~~ countComp :" + countComp);
            Transform transComp = resGO.GetComponent<Transform>();
            if (transComp == null)
                resGO.AddComponent<Transform>();
            resGO.transform.position = pos;
            resGO.SetActive(true);
        }
        */
        //---------------------------------------------------------------------------

        return resGO;
    }
    private void DestroyField(GameObject findField)
    {
        //#PRED 
        Destroy(findField);
        //For Pool
        //DestroyPoolGameObject(findField);
    }

    

#region Pool
    
    /*
    void LoadPoolGameObjects()
    {
        return;
        foreach (var i in Enumerable.Range(0, 1000))
        {
            GameObject nexGO = new GameObject();
            nexGO.name = "GameObjectPool " + i;

            //nexGO.AddComponent<Transform>();
            nexGO.AddComponent<SpriteRenderer>(); 

            nexGO.SetActive(false);
            //nexGO = Instantiate(nexGO, new Vector3(-20, 20, 2), Quaternion.identity);
            PoolGamesObjects.Add(nexGO);
        }
    }

    public GameObject GetPoolGameObject()
    {
        GameObject findGO = null;
        //PoolGameObject findPoolGO = PoolGamesObjects.Find(p => p.IsLock = false);
        //findPoolGO.IsLock = true;
        //findGO = findPoolGO.GameObjectNext;
        findGO = PoolGamesObjects.Find(p => p.activeSelf == false);
        if (findGO != null)
        {
            findGO.SetActive(true);
        }
        return findGO;
    }

    public void DestroyPoolGameObject(GameObject delGO)
    {
        //bool isFindPool = false;
        //int indexLoop = 0;
        //foreach(var itemObj in PoolGamesObjects)
        //{
        //    if(itemObj.Equals(delGO))
        //    {
        //        isFindPool = true;
        //        break;
        //    }
        //    indexLoop++;
        //}
        //if(isFindPool)
        //{
        //    Debug.Log("~~~~~~~~ DestroyPoolGameObject yes pool (" + indexLoop + ") :" + delGO.name);
            delGO.SetActive(false);
        //}
        //else
        //{
        //    Debug.Log("~~~~~~~~ DestroyPoolGameObject NOT pool !!! :" + delGO.name);
        //    Destroy(delGO);
        //}
    }
    */
#endregion

    //public class PoolGameObject
    //{
    //    public GameObject GameObjectNext { get; set; }
    //    public bool IsLock { get; set; }

    //    public PoolGameObject()
    //    {
    //        GameObjectNext = new GameObject();
    //        IsLock = false;
    //    }
    //}

    ////-----------temp----------------------

    void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 100, 100), (int)(1.0f / Time.smoothDeltaTime));
        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());
        GUI.Label(new Rect(0, 30, 100, 100), Counter.ToString());
    }

}
