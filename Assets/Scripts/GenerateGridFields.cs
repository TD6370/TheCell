using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGridFields : MonoBehaviour {

    public GameObject prefabField;
    
    //public Dictionary<int, GameObject> Fields;
    public Dictionary<string, GameObject> Fields;
    //public Dictionary<string, List<GameObject>> GamesObjectsActive;
    public Dictionary<string, List<GameObject>> GamesObjectsReal;
    public Dictionary<string, List<SaveLoadData.ObjectData>> GamesObjectsPersonalData;
    public SaveLoadData.GridData GridData;
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
        //Fields = new Dictionary<int, GameObject>();
        Fields = new Dictionary<string, GameObject>();
        //GamesObjectsActive = new Dictionary<string, List<GameObject>>();
        GamesObjectsReal = new Dictionary<string, List<GameObject>>();
        GamesObjectsPersonalData = new Dictionary<string, List<SaveLoadData.ObjectData>>();

        _sctiptData = GetComponent<SaveLoadData>();
        if (_sctiptData == null)
            Debug.Log("GenerateGridFields.Start : sctiptData not load !!!");

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
                string nameField = GetNameField(x, y);
                newField.name = nameField;
                _nameField = nameField;
                Fields.Add(nameField, newField);
                Counter++;
            }
        }

        Debug.Log("Pole Field name init : " + _nameField);
    }


    private bool m_onLoadFields = false;
    public void GenGrigLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    {
        int gridWidth = 100;
        int gridHeight = 100;
        //gridWidth = (int)GridX;
        //gridHeight = (int)GridY;
        int countField = (int)GridX * (int)GridY;

        //if (Fields.Count != _counter || _counter == 0)
        if (!m_onLoadFields && (Fields.Count < countField || countField == 0))
        {
            //Debug.Log("!!!!! Fields.Count =" + Fields.Count + "   _counter =" + _counter);
            Debug.Log("!!!!! Fields.Count =" + Fields.Count + "   countField =" + countField);
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
                    string nameField = GetNameField(x, y);
                    _nameField = nameField;
                    //Find
                    if (!Fields.ContainsKey(nameField))
                        continue;

                    GameObject findField = Fields[nameField];
                    //Destroy !!!
                    //Destroy(findField, 0.5f);
                    Destroy(findField);
                    Fields.Remove(nameField);
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
                    string nameField = GetNameField(x, y);
                    _nameField = nameField;

                    if (Fields.ContainsKey(nameField))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;
                    GameObject newField = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                    newField.name = nameField;
                    Fields.Add(nameField, newField);
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
                    string nameField = GetNameField(x, y);

                    //Find
                    if (!Fields.ContainsKey(nameField))
                        continue;

                    GameObject findField = Fields[nameField];
                    //Destroy !!!
                    Destroy(findField);
                    Fields.Remove(nameField);
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
                    string nameField = GetNameField(x, y);

                    if (Fields.ContainsKey(nameField))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;
                    GameObject newField = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                    newField.name = nameField;
                    Fields.Add(nameField, newField);
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
        //GridData
        if (GridData == null || GridData.FieldsD == null)
        {
            Debug.Log(" LoadGameObjectDataForLook_2 GridData IS EMPTY !!!");
            return;
        }

        if (!GridData.FieldsD.ContainsKey(p_nameField))
            return;

        List<SaveLoadData.ObjectData> listGameObjectInField = GridData.FieldsD[p_nameField].Objects;
        List<GameObject> listGameObjectReal = new List<GameObject>();

        if (!GamesObjectsReal.ContainsKey(p_nameField))
        {
            GamesObjectsReal.Add(p_nameField, listGameObjectReal);
        }
        else
        {
            listGameObjectReal = GamesObjectsReal[p_nameField];
        }

        foreach (var gameObj in listGameObjectInField)
        {
            //#PPP TEST
            //Debug.Log("TEST ########### LoadGameObjectDataForLook_2 added +++ : " + gameObj.NameObject);

            //GameObject newField = CreatePrefabByName(gameObj.TagObject, gameObj.NameObject, gameObj.Position);
            GameObject newField = CreatePrefabByName(gameObj);

            //Debug.Log(" LoadGameObjectDataForLook_2 added +++ : " + gameObj.NameObject);
            listGameObjectReal.Add(newField);
            Counter++;
        }
    }

    public void DestroyRealObject(GameObject gObj)
    {
        if (gObj == null)
            return;

        //Debug.Log("DestroyRealObject... ");
        string nameField = GenerateGridFields.GetNameFieldByName(gObj.name);
        if (nameField == null)
            return;

        List<GameObject> listObjInField = GamesObjectsReal[nameField];

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
                GamesObjectsReal[nameField].RemoveAt(indRealData);
            }
        }
        Destroy(gObj);
        //-----------------------------------------------

        //Destrot to Data
        if (!GridData.FieldsD.ContainsKey(nameField))
        {
            Debug.Log("!!!! DestroyRealObject !GridData.FieldsD not field=" + nameField);
            return;
        }
        List<SaveLoadData.ObjectData> dataObjects = GridData.FieldsD[nameField].Objects;
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
        if (!GamesObjectsReal.ContainsKey(p_nameField))
        {
            //Debug.Log("RemoveRealObject Not in field : " + p_nameFiled);
            return;
        }
        else
        {
            List<GameObject> realObjects = GamesObjectsReal[p_nameField];
             //#.D_2
            SaveListObjectsToData(p_nameField);
            foreach (var obj in realObjects)
            {
                Counter--;
                Destroy(obj);
            }
            GamesObjectsReal.Remove(p_nameField);
        }
    }

    //#.D //UPDATE FOR LOOK - DATA_2
    private void SaveListObjectsToData(string p_nameField)
    {
        //#timeclose
        //return;

        List<GameObject> realObjects = GamesObjectsReal[p_nameField];
        //GridData
        if (GridData == null)
        {
            Debug.Log("SaveListObjectsToData GridData is EMPTY");
            return;
        }

        if (!GridData.FieldsD.ContainsKey(p_nameField))
        {
            Debug.Log("SaveListObjectsToData !GridData.FieldsD not field=" + p_nameField);
            return;
        }

        List<SaveLoadData.ObjectData> dataObjects = GridData.FieldsD[p_nameField].Objects;

        //Debug.Log("SaveListObjectsToData realObjects.Count=" + realObjects.Count);
        try
        {

            for (int i = 0; i < realObjects.Count; i++)
            //for (int i = realObjects.Count - 1; i >= 0; i--) 
            {
                GameObject gobj = realObjects[i];
                //SaveLoadData.ObjectData objData = SaveLoadData.CreateObjectData(gobj);
                //if (dataObjects.Count >= i)
                if (dataObjects.Count > i)
                {
                    
                    var pos1 = dataObjects[i].Position;
                    if (dataObjects[i] == null)
                    {
                        Debug.Log("SaveListObjectsToData REMOVE dataObjects[i] == null");
                        continue;
                    }

                    if (realObjects[i] == null)
                    {
                        //Debug.Log("SaveListObjectsToData REMOVE realObjects[i] == null");
                        //realObjects.RemoveAt(i);
                        continue;
                    }

                    var pos2 = realObjects[i].transform.position; //!!!!
                    var f = pos1.y;
                    string posFieldOld = GetNameFieldPosit(pos1.x, pos1.y);
                    string posFieldReal = GetNameFieldPosit(pos2.x, pos2.y);

                    //---------------------------------------------
                    if (posFieldOld != posFieldReal)
                    {
                        string info = "SaveListObjectsToData posFieldOld(" + posFieldOld + ") != posFieldReal(" + posFieldReal + ")      " + dataObjects[i].NameObject + "    " + realObjects[i].name;

                        //remove in Old Field
                        //Debug.Log("SaveListObjectsToData REMOVE :" + dataObjects[i].NameObject);
                        dataObjects.RemoveAt(i);

                        //add to new Field
                        if (!GridData.FieldsD.ContainsKey(posFieldReal))
                        {
                            //#!!!!  Debug.Log("SaveListObjectsToData GridData ADD new FIELD : " + posFieldReal);
                            GridData.FieldsD.Add(posFieldReal, new SaveLoadData.FieldData());
                        }

                        SaveLoadData.ObjectData objDataNow = SaveLoadData.CreateObjectData(gobj);

                        int pp = GridData.FieldsD[posFieldReal].Objects.Count;
                        objDataNow.NameObject = SaveLoadData.CreateName(objDataNow.TagObject, posFieldReal, pp);
                        info += "  New Name: " + objDataNow.NameObject;

                        //Debug.Log(info);

                        //var otherObjects = GridData.FieldsD[posFieldReal].Objects;
                        //otherObjects.Add(objData);
                        GridData.FieldsD[posFieldReal].Objects.Add(objDataNow);
                    }
                    else
                    {
                        //Create on Game Object
                        SaveLoadData.ObjectData objData = SaveLoadData.CreateObjectData(gobj);
                        //update
                        dataObjects[i] = objData;
                    }
                }
            }

        }
        catch(Exception x)
        {
            Debug.Log("ERROR SaveListObjectsToData : " + x.Message);
        }
     }

    //ADD NEW GEN GAME OBJECT 
    public void ActiveGameObject(GameObject p_saveObject)
    {
        int x = 0;
        int y = 0;
        x = (int)p_saveObject.transform.position.x;
        y = (int)Mathf.Abs(p_saveObject.transform.position.y);
        //string p_nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
        string p_nameField = GetNameFieldPosit(x, y);

        p_saveObject.name = SaveLoadData.CreateName(p_saveObject.tag, p_nameField);
        SaveNewGameObjectToData(p_nameField, p_saveObject);
    }

    private void SaveNewGameObjectToData(string p_nameField, GameObject p_saveObject)
    {
        if (GridData == null)
        {
            Debug.Log("SaveNewGameObjectToData_2 GridData is EMPTY");
            return;
        }

        if (!GridData.FieldsD.ContainsKey(p_nameField))
        {
            GridData.FieldsD.Add(p_nameField, new SaveLoadData.FieldData());
        }

        List<SaveLoadData.ObjectData> listDataObjectInField = GridData.FieldsD[p_nameField].Objects;

        SaveLoadData.ObjectData dataObjectSave = SaveLoadData.CreateObjectData(p_saveObject, true);

        dataObjectSave.NameObject = SaveLoadData.CreateName(dataObjectSave.TagObject, p_nameField, listDataObjectInField.Count);

        SaveLoadData.ObjectData objDataOld = null;
        int indexObjectData = listDataObjectInField.FindIndex(p => p.NameObject == dataObjectSave.NameObject);
        if (indexObjectData!=-1)
            objDataOld = listDataObjectInField[indexObjectData];

        if (objDataOld == null)
        {
            //-------------------------- Add object in Data
            listDataObjectInField.Add(dataObjectSave);
            SaveLoadData.ObjectData objData = dataObjectSave;
        }
        else
        {
            dataObjectSave.NameObject = SaveLoadData.CreateName(dataObjectSave.TagObject, p_nameField, listDataObjectInField.Count + 1);
            //update object
            objDataOld = dataObjectSave;
            Debug.Log("# SaveNewGameObjectToData_2  ADD NEW GEN OBJECT -- SAVE TO OLD: " + dataObjectSave.NameObject);
        }

        //-------------------------- Add object in Real list
        if (!GamesObjectsReal.ContainsKey(p_nameField))
        {
            GamesObjectsReal.Add(p_nameField, new List<GameObject>());
        }
        List<GameObject> objectsReal = GamesObjectsReal[p_nameField];
        objectsReal.Add(p_saveObject);
        Counter++;

        //Debug.Log("ActiveGameObject 9." + p_saveObject.name);
    }
    private void SavePersonalData()
    {

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
        GUI.Label(new Rect(0, 30, 100, 100), Counter.ToString());
    }

    private GameObject FindPrefab(string namePrefab)
    {
        //return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
        //return sctiptData.FindPrefabHieracly(namePrefab);
        if (_sctiptData == null)
            return null; 

        return _sctiptData.FindPrefab(namePrefab);
    }

    const string FieldKey = "Field";

    public static string GetNameField(int x, int y)
    {
        return FieldKey + x + "x" + Mathf.Abs(y);
    }

    public static string GetNameField(System.Single x, System.Single y)
    {
        return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    }

    public static string GetNameFieldPosit(int x, int y)
    {
        x = (int)(x / 2);
        y = (int)(y / 2);
        return FieldKey + x + "x" + Mathf.Abs(y);
    }

    public static string GetNameFieldPosit(System.Single x, System.Single y)
    {
        x = (int)(x / 2);
        y = (int)(y / 2);
        return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    }

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
        newObjGame.transform.position = pos;
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

    public static string GetNameFieldByName(string nameGameObject){

        int start = nameGameObject.IndexOf(FieldKey);
        string result = "";
        string resultInfo = "";
        int valid=0;
        if (start == -1)
        {
            Debug.Log("# GetNameFieldByName " + nameGameObject + " key 'Field' not found!");
            return null;
        }
        start += "Field".Length;

        //int i = nameGameObject.IndexOf("Field");
        for (int i = start; i < nameGameObject.Length -1 ; i++)
        {
            var symb = nameGameObject[i];
            resultInfo += symb;
            if (symb == 'x')
            {
                result += symb.ToString();
                continue;
            }
            if(Int32.TryParse(symb.ToString(),out valid))
            {
                result += valid.ToString();
                continue;
            }
            break;
        }
        result = FieldKey + result;
        //Debug.Log("# GetNameFieldByName " + nameGameObject + " >> " + result + "     text: " + resultInfo + "   start=" + start);
        return result;
    }

    public void SaveAllRealGameObjects()
    {
        foreach (var realObjGame in GamesObjectsReal)
        {
            string nameField = realObjGame.Key;
            //RemoveRealObjects(nameField);
            SaveListObjectsToData(nameField);
        }
    }

    public void LoadObjectsNearHero()
    {
        foreach (var field in Fields)
        {
            string nameField = field.Key;
            LoadObjectToReal(nameField);
        }
    }

    ////-----------temp----------------------

    

}
