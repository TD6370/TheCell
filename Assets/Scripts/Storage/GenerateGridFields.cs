﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenerateGridFields : MonoBehaviour {

    public GameObject prefabField;
    private SaveLoadData _sctiptData;

    public float GridX = 5f;
    public float GridY = 5f;
    public float Spacing = 2f;

    private int _CounterRealObj;

    private int _counter;
    public int Counter
    {
        get { return _counter; }
        set { _counter = value; }
    }

	// Use this for initialization
	void Start () {
        

        _sctiptData = GetComponent<SaveLoadData>();
        if (_sctiptData == null)
            Debug.Log("GenerateGridFields.Start : sctiptData not load !!!");

        //LoadPoolGameObjects();
        StartCoroutine(CalculateTealsObjects());
    }

    void Awake()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator CalculateTealsObjects()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.3f);

            if (Storage.Instance.GamesObjectsReal == null)
                yield return null;

            //string prUfo = SaveLoadData.TypePrefabs.PrefabUfo.ToString();
            //string prBoss = SaveLoadData.TypePrefabs.PrefabBoss.ToString();

            //List<GameObject> gobjsList = Storage.Instance.GamesObjectsReal.
            //    SelectMany(x => x.Value).
            //    Where( p=> p.tag == prUfo || p.tag == prBoss).ToList();
            //List<GameObject> gobjsList = Storage.Person.GetAllRealPersons().ToList();

            _CounterRealObj = Storage.Person.GetAllRealPersons().ToList().Count(); //gobjsList.Count();
        }

    }


    //Add start position
    public void StartGenGrigField(bool isOffsetHero = false)
    {
        int maxWidth = (int)GridY * -1;
        int maxHeight = (int)GridX;
        Counter = maxWidth * maxHeight;
        Debug.Log("counter=" + Counter.ToString());
        Counter = 0;
        string _nameField = "";

        int startX = 0;
        int startY = 0;
        int maxWidthOffset = maxWidth;
        int maxHeightOffset = maxHeight;
        if (isOffsetHero)
        {
            startX = Storage.Instance.ZonaField.X;
            startY = Storage.Instance.ZonaField.Y*(-1);
            //Debug.Log("-------- StartGenGrigField ---- " + startX + "x" + startY + "    h:" + maxHeightOffset + " w:" + maxWidthOffset);
            maxHeightOffset += startX + 1;
            maxWidthOffset += startY - 1;
        }

        Storage.Instance.Fields.Clear();

        for (int y = startY; y > maxWidthOffset; y--)
        {
            for (int x = startX; x < maxHeightOffset; x++)
            {
                Vector3 pos = new Vector3(x, y, 1) * Spacing;
                pos.z = 0;
                GameObject newField = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
                newField.tag = "Field";
                string nameField = Helper.GetNameField(x, y);
                newField.name = nameField;
                _nameField = nameField;
                Storage.Instance.Fields.Add(nameField, newField);
                Counter++;
            }
        }

        Debug.Log("Pole Field name init : " + _nameField);
    }


    private bool m_onLoadFields = false;
    public void GenGridLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    {
        var _fields = Storage.Instance.Fields;

        int gridWidth = 100;
        int gridHeight = 100;
        
        int countField = (int)GridX * (int)GridY;

        
        if (!m_onLoadFields && (Storage.Instance.Fields.Count < countField || countField == 0))
        {
            
            Debug.Log("!!!!! Fields.Count =" + _fields.Count + "   Grid Field Limit =" + countField);
            return;
        }
        {
            m_onLoadFields = true;
        }

        if (_movement.x != 0)
        {
            int p_startPosY;
            int limitVertical;
            Helper.InitRange(p_PosHeroY, p_limitVerticalLook, gridHeight, out p_startPosY, out limitVertical);

            int LeftX = p_PosHeroX - (p_limitHorizontalLook / 2);
            int RightX = p_PosHeroX + (p_limitHorizontalLook / 2);
            int x = 0;
            int LeftRemoveX = LeftX - 1;
            int RightRemoveX = RightX + 1;
            //Validate ValidateRemoveX
            bool isRemove = Helper.ValidateRemoveX(_movement, gridWidth, LeftRemoveX, RightRemoveX);
            bool isAdded = Helper.ValidateAddedX(_movement, gridWidth, LeftX, RightX);

            if (isRemove)
            {
                x = _movement.x > 0 ?
                    //Remove Vertical
                LeftRemoveX :
                RightRemoveX;

                string _nameField = "";
                for (int y = p_startPosY; y < limitVertical; y++)
                {
                    
                    string nameField = Helper.GetNameField(x, y);
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
                    string nameField = Helper.GetNameField(x, y);
                    _nameField = nameField;

                    if (_fields.ContainsKey(nameField))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;
                    
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
            Helper.InitRange(p_PosHeroX, p_limitHorizontalLook, gridWidth, out p_startPosX, out limitHorizontal);

            int y = 0;
            int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
            int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
            int TopRemoveY = TopY - 1;
            int DownRemoveY = DownY + 1;

            //Validate
            bool isRemove = Helper.ValidateRemoveY(_movement, gridHeight, TopRemoveY, DownRemoveY);
            bool isAdded = Helper.ValidateAddedY(_movement, gridHeight, TopY, DownY);

            if (isRemove)
            {
                y = _movement.y < 0 ?
                    //Remove Horizontal //#
                    TopRemoveY :
                    DownRemoveY; //#

                for (int x = p_startPosX; x < limitHorizontal; x++) //#
                {
                    
                    string nameField = Helper.GetNameField(x, y);

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
                    
                    string nameField = Helper.GetNameField(x, y);

                    if (_fields.ContainsKey(nameField))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;
                    
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
        
        LoadGameObjectDataForLook_2(nameField);
    }

    //загрузка из данныx объектов из памяти и создание их на поле  ADDED FOR LOOK - DATA 2
    private void LoadGameObjectDataForLook_2(string p_nameField)
    {
        var _gridData = Storage.Instance.GridDataG;
        var _gamesObjectsReal = Storage.Instance.GamesObjectsReal;

        if(Storage.Instance.IsCorrectData)
        {
            Debug.Log("_______________ RETURN LoadGameObjectDataForLook ON CORRECT_______________");
            return;
        }

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
                return;
            }
           
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

                    //Debug.Log("LoadGameObjectDataForLook ********************** " + dataObj + " already IsReality !!!!");
                    var realObj = listGameObjectReal.Find(p => p.name == dataObj.NameObject);
                    if (realObj != null) //@??@
                    {
                        //Debug.Log("????????????????? LoadGameObjectDataForLook ****** IsReality DO:" + dataObj + " EXIST in Real == " + realObj.name);
                        //Storage.Log.GetHistory(dataObj.NameObject);
                        //return;
                        continue;
                    }
                }
                //--------------

                indErr = "14.";
                dataObj.IsReality = true;
                indErr = "15.";
                
                GameObject newField = CreatePrefabByName(dataObj);
                indErr = "16.";
                
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
                Storage.Log.SaveHistory(obj.name, "DESTROY GOBJ", "RemoveRealObjects");
                Destroy(obj);
            }
            //@<<@ Storage.Instance.GamesObjectsReal.Remove(p_nameField);
            Storage.Data.RemoveFieldRealObject(p_nameField, "RemoveRealObjects");
        }
    }

    //#.D //UPDATE FOR LOOK - DATA_2
    
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
            Debug.Log("################# SaveListObjectsToData GridData is EMPTY");
            return;
        }

        if (!_gridData.FieldsD.ContainsKey(p_nameField))
        {
            Debug.Log("################# SaveListObjectsToData !GridData.FieldsD not field=" + p_nameField);
            return;
        }

        //@<<@ 
        List<SaveLoadData.ObjectData> dataObjects = _gridData.FieldsD[p_nameField].Objects;
        try
        {
            indErr = "1.";

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            for (int i = realObjects.Count - 1; i >= 0; i--)  //+FIX
            {
                indErr = "2.";
                GameObject gobj = realObjects[i];
                if (gobj == null || !gobj.activeSelf)
                {
                    //Debug.Log("################# SaveListObjectsToData   REMOVE  GameObject   field:" + p_nameField + "  ind:" + i);
                    Debug.Log("***************** SaveListObjectsToData DESTROY GameObject field:" + p_nameField + "  ind:" + i);
                    continue;
                }

                indErr = "3.";
                //SaveLoadData.ObjectData dataObj = dataObjects.Find(p => p.NameObject == gobj.name);
                int indData = dataObjects.FindIndex(p => p.NameObject == gobj.name);

                if (indData == -1)
                {
                    Debug.Log("################# SaveListObjectsToData 1.  DataObject (" + gobj.name + ") not Find in DATA     field: " + p_nameField);
                    Storage.Log.GetHistory(gobj.name);
                    continue;
                }
                
                SaveLoadData.ObjectData dataObj = dataObjects[indData];
                if (dataObj == null)
                {
                    Debug.Log("################# SaveListObjectsToData 2.  DataObject (" + gobj.name + ") not Find in DATA     field: " + p_nameField);
                    Storage.Log.GetHistory(gobj.name);
                    continue;
                }

                indErr = "3.7.";
                var posD = dataObj.Position;
                //@SAVE@ var posR = realObjects[i].transform.position; //!!!!
                var posR = gobj.transform.position; //!!!!
                
                indErr = "6.";
                string posFieldOld = Helper.GetNameFieldPosit(posD.x, posD.y);
                indErr = "7.";
                string posFieldReal = Helper.GetNameFieldPosit(posR.x, posR.y);

                if (posFieldOld != p_nameField)
                {
                    Debug.Log("################# SaveListObjectsToData !!!!!!!!!!!! posFieldOld != p_nameField");
                }
                indErr = "8.";
                //---------------------------------------------
                if (posFieldOld != posFieldReal)
                {
                    if (isDestroy)
                    {
                        dataObj.IsReality = false;
                        //Debug.Log("SaveListObjectsToData DEACTIVATE : " + gobj.name);
                        gobj.SetActive(false);
                    }

                    indErr = "10.";

                    //Debug.Log("___________________ SaveListObjectsToData destroy(" + isDestroy + ")______GO:" + gobj.name + "  DO: " + dataObj.ToString() + "      " + posFieldOld + " >> " + posFieldReal);

                    //!!!!!!!!!!!!!!!!!!
                    SaveLoadData.ObjectData dataObjNew = (SaveLoadData.ObjectData)dataObj.Clone();
                    var name = Helper.CreateName(dataObj.TagObject, posFieldReal, "", gobj.name);


                    // #????# ????????????????
                    Storage.Data.RemoveDataObjectInGrid(p_nameField, i, "SaveListObjectsToData", false,  dataObj); ////@<<@ 

                    if (!_gridData.FieldsD.ContainsKey(posFieldReal))
                    {
                        Storage.Data.AddNewFieldInGrid(posFieldReal, "SaveListObjectsToData"); //@<<@ 
                    }

                    //var testDO = dataObj.NameObject;
                    //!!!!!!!!!!!!!!!!!!!!!
                    //SaveLoadData.ObjectData dataObjNew = new SaveLoadData.ObjectData()
                    //{

                    //};
                    
                    dataObjects = _gridData.FieldsD[posFieldReal].Objects;
                    indData = dataObjects.FindIndex(p => p.NameObject == name);
                    if (indData != -1)
                    {
                        dataObj = dataObjects[indData];
                        Debug.Log("################# SaveListObjectsToData 3.  DataObject (" + gobj.name + ") not Find in DATA     field: " + p_nameField);
                        Storage.Log.GetHistory(gobj.name);
                        continue;
                    }
                    dataObj = dataObjNew;

                    //dataObj = dataObjects[indData];
                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    dataObj.NameObject = name;
                    //Debug.Log("___ RENAME : DATA OBJECT: " + testDO + "  >>  " + dataObj.NameObject + "         GO:" + gobj.name);

                    //if (isDestroy)
                    //    dataObj.IsReality = false;

                    //#TEST -----------------
                    var objTest = _gridData.FieldsD[posFieldReal].Objects.Find(p => p.NameObject == dataObj.NameObject);
                    if (objTest != null)
                    {
                        Debug.Log("################# Error SaveListObjectsToData GridData.FieldsD[" + posFieldReal + "].Objects already Exist : " + objTest);
                        continue;
                    }
                    //-----------------------------------

                    indErr = "17.";
                    //@SAVE@ ------ RESAVE PROPERTIES
                    if (dataObj.NameObject != gobj.name)
                    {
                        //!!!!!!!!!!!!!!!!!!!!!!
                        if (!isDestroy)
                            dataObj.UpdateGameObject(gobj);

                        indErr = "18.";
                        //Debug.Log("___ RENAME : GO: " + gobj.name + "  >>  " + dataObj.NameObject);
                        //gobj.name = dataObj.NameObject;
                        indErr = "19.";
                        //Debug.Log("___ RESAVE POS : GO: " + dataObj.Position + "  >>  " + gobj.transform.position);
                        dataObj.Position = gobj.transform.position;
                    }
                    //dataObj.IsReality = true;
                    dataObj.IsReality = false;

                    indErr = "20.";

                    //Debug.Log("_________DATA SAVE: " + dataObj  + " " + dataObj.NameObject + "    " + posFieldReal + "       pos filed:" + Helper.GetNameFieldPosit(dataObj.Position.x, dataObj.Position.y) + "  pos:" + dataObj.Position.x + "x" + dataObj.Position.y);
                    Storage.Data.AddDataObjectInGrid(dataObj, posFieldReal, "SaveListObjectsToData"); //@<<@ 

                    indErr = "21.";

                    //RESAVE REAL +++++++++++++++++++++++++++++++++++++++++++++++++++++
                    if (!Storage.Instance.GamesObjectsReal.ContainsKey(posFieldReal))
                    {
                        indErr = "22.";
                        Storage.Data.AddNewFieldInRealObject(posFieldReal, "SaveListObjectsToData");
                    }

                    

                    indErr = "23.";
                    if (!isDestroy)
                    {
                        Debug.Log("SaveListObjectsToData -- AddRealObject -- " +  gobj.name + " : " + posFieldReal);
                        Storage.Data.AddRealObject(gobj, posFieldReal, "SaveListObjectsToData");
                    }

                    indErr = "24.";
                    //Debug.Log("SaveListObjectsToData -- RemoveRealObject -- " + gobj.name + " : " + p_nameField);
                    Storage.Data.RemoveRealObject(i, p_nameField, "SaveListObjectsToData");
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++

                    indErr = "25.";
                    //@SAVE@
                    if(!isDestroy)
                        GameObjectUpdatePersonData(gobj);
                }
                else
                {
                    //update
                    if (isDestroy)
                        dataObj.IsReality = false;

                    //@@@TEST 
                    Storage.Data.UpdateDataObect(p_nameField, indData, dataObj, "SaveListObjectsToData", posR); //@<<@ 
                    //------------------------------------
                }
               
            }

        }
        catch (Exception x)
        {
            Debug.Log("################# ERROR SaveListObjectsToData : " + x.Message + "  #" + indErr);
        }
    }

    //@SAVE@
    private void GameObjectUpdatePersonData(GameObject gobj)
    {
        string tag = gobj.tag.ToString();

        if (String.IsNullOrEmpty(gobj.tag))
        {
            Debug.Log("############ GameObjectRealUpdateData gobj.tag = null");
            return;
        }

        MovementUfo _movementUfo = gobj.GetComponent<MovementUfo>();
        if(_movementUfo!=null)
            _movementUfo.UpdateData("GameObjectUpdatePersonData");

        //Debug.Log("___________________________GameObjectRealUpdateData....__________________" + gobj.name);

        //SaveLoadData.TypePrefabs prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), gobj.tag.ToString()); ;

        //switch (prefabType)
        //{
        //    case SaveLoadData.TypePrefabs.PrefabUfo:
        //        MovementUfo _movementUfo = gobj.GetComponent<MovementUfo>();
        //        _movementUfo.UpdateData("GameObjectUpdatePersonData");
        //        break;
        //    default:
        //        break;
        //}
    }
    

    //ADD NEW GEN GAME OBJECT 
    public void ActiveGameObject(GameObject p_saveObject)
    {
        int x = 0;
        int y = 0;
        
        x = (int)p_saveObject.transform.position.x;
        y = (int)Mathf.Abs(p_saveObject.transform.position.y);
        
        string p_nameField = Helper.GetNameFieldPosit(x, y);

        p_saveObject.name = Helper.CreateName(p_saveObject.tag, p_nameField, "", p_saveObject.name);
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

        if (!_gridData.FieldsD.ContainsKey(p_nameField))
        {

            Storage.Data.AddNewFieldInGrid(p_nameField, "SaveNewGameObjectToData");
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
            Storage.Data.AddDataObjectInGrid(dataObjectSave, p_nameField, "SaveNewGameObjectToData"); //@<<@
        }
        else
        {
            //+FIX
            dataObjectSave.NameObject = Helper.CreateName(dataObjectSave.TagObject, p_nameField, "", p_saveObject.name);

            //update object
            Storage.Data.UpdateDataObect(p_nameField, indexObjectData, dataObjectSave, "SaveNewGameObjectToData"); //@<<@ 
            Debug.Log("# SaveNewGameObjectToData_2  ADD NEW GEN OBJECT -- SAVE TO OLD: " + dataObjectSave.NameObject);
        }

        //-------------------------- Add object in Real list
        if (!_gamesObjectsReal.ContainsKey(p_nameField))
        {
            Storage.Data.AddNewFieldInRealObject(p_nameField, "SaveNewGameObjectToData"); //@<<@ 
        }
        Storage.Data.AddRealObject(p_saveObject, p_nameField, "SaveNewGameObjectToData"); //@<<@ 

        Counter++;
    }
    

    private GameObject FindPrefab(string namePrefab)
    {
        //return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
        if (_sctiptData == null)
            return null;

        return _sctiptData.FindPrefab(namePrefab);
    }

    //+++ CreateObjectData +++ LoadObjectForLook
    public GameObject CreatePrefabByName(SaveLoadData.ObjectData objData)
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
        newObjGame.transform.position = pos; //@!@.1

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
                    Debug.Log("CreatePrefabByName... (" + objData.NameObject + ")  objData not is ObjectDataUfo !!!!");
                }
                break;
            case SaveLoadData.TypePrefabs.PrefabBoss: //$$

                //Debug.Log("################ CreatePrefabByName.. BOSS: " + prefabType);
                var objBoss = objData as SaveLoadData.GameDataBoss;
                if (objBoss != null)
                {
                    //LoadObjectForLook:  DATA -->> PERSONA #P#
                    objBoss.UpdateGameObject(newObjGame);
                }
                else
                {
                    Debug.Log("CreatePrefabByName... (" + objData.NameObject + ")  objData not is ObjectDataUfo !!!!");
                }
                break;
            default:
                //Debug.Log("################ CreatePrefabByName.. default Type: " + prefabType);
                break;
        }
        //.............

        newObjGame.name = namePrefab;
        return newObjGame;
    }

    
    public void SaveAllRealGameObjects()
    {
        if (Storage.Instance.IsLoadingWorld)
        {
            Debug.Log("_______________ LOADING WORLD ....._______________");
            return;
        }

        Storage.Instance.IsLoadingWorld = true;

        
        Debug.Log("SSSSSSSSSSSSS SaveAllRealGameObjects ...........");

        DateTime startTestTime = DateTime.Now;

        //------------
        var listKeyR = Storage.Instance.GamesObjectsReal.Select(p => p.Key).ToList();
        foreach (var itemKey in listKeyR)
        {
            string nameField = itemKey;
            //SaveListObjectsToData(nameField, true);
            SaveListObjectsToData(nameField);
        }
        //------------

        Debug.Log("SSSSSSSSSSSSS SaveAllRealGameObjects END ^^^^^^^^^^^^^^^^^^^^^: T1 : " +  (DateTime.Now - startTestTime).TotalMilliseconds);
        //startTestTime = DateTime.Now;

        //-------------
        //var listKey = Storage.Instance.GamesObjectsReal.Select(p => p.Key).ToList();
        //foreach (var itemKey in listKey)
        //{
        //    List<GameObject> objects = Storage.Instance.GamesObjectsReal[itemKey];

        //    //foreach (var gobj in objects)
        //    for (int i = objects.Count() - 1; i >= 0; i--)
        //    {
        //        var gobj = objects[i];

        //        var moveUfo = gobj.GetComponent<MovementUfo>();
        //        if (moveUfo != null)
        //        {
        //            moveUfo.SaveData();
        //        }
        //        //var moveNPC = gobj.GetComponent<MovementNPC>();
        //        //if (moveNPC != null)
        //        //    moveNPC.SaveData();
        //        //var moveBoss = gobj.GetComponent<MovementBoss>();
        //        //if (moveBoss != null)
        //        //    moveBoss.SaveData();
        //    }
        //}
        //------------
        //Debug.Log("SSSSSSSSSSSSS SaveAllRealGameObjects END ^^^^^^^^^^^^^^^^^^^^^: T2 : " + (DateTime.Now - startTestTime).TotalMilliseconds);

        //Storage.Data.UpdateDataObect(p_nameField, indData, dataObj, "SaveListObjectsToData", posR);
        //_dataUfo.NextPosition(this.gameObject);
        //Storage.Instance.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, this, _newPosition, gobj, !isInZona);

        Storage.Instance.IsLoadingWorld = false;
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
     
    */
#endregion

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());
        GUI.Label(new Rect(0, 30, 100, 100), Counter.ToString());

        GUI.Label(new Rect(0, 550, 100, 100), "REAL GOBJ: " + _CounterRealObj.ToString());
    }

}
