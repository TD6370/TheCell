﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Profiling;

public class GenerateGridFields : MonoBehaviour {

    [NonSerialized]
    public bool IsLoadOnlyField = false;// true;// 

    public GameObject PanelPool;
    public GameObject prefabField;
    private SaveLoadData _sctiptData;

    public float GridX = 5f;
    public float GridY = 5f;
    public float Spacing = 2f;
    //Profiler Profiller;

    private int _CounterRealObj;

    private int _counter;
    public int Counter
    {
        get { return _counter; }
        set { _counter = value; }
    }

    // Use this for initialization
    void Start() {

        _sctiptData = GetComponent<SaveLoadData>();
        if (_sctiptData == null)
            Debug.Log("GenerateGridFields.Start : sctiptData not load !!!");

        //LoadPoolGameObjects();


        //StartCoroutine(CalculateTilesObjects());
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update() {

    }

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
            startY = Storage.Instance.ZonaField.Y * (-1);
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

                GameObject newField;

                string nameFieldNew = Helper.GetNameField(x, y);
                if(PoolGameObjects.IsUseTypePoolPrefabs) //$$$
                {
                    prefabField.tag = PoolGameObjects.TypePoolPrefabs.PoolFloor.ToString();
                }
                newField = Storage.Pool.InstantiatePool(prefabField, pos, nameFieldNew);

                newField.tag = "Field";
                string nameField = Helper.GetNameField(x, y);
                newField.name = nameField;
                _nameField = nameField;

                //fix alpha field
                newField.GetComponent<SpriteRenderer>().sortingLayerName = Helper.LayerTerraName;

                Storage.Instance.Fields.Add(nameField, newField);
                Counter++;
            }
        }
        Debug.Log("Pole Field name init : " + _nameField);
    }

    private bool m_onLoadFields = false;

    public void GenGridLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0, bool isOnlyField = false)
    {
        int gridWidth = Helper.WidthLevel; //100 Big
        int gridHeight = Helper.HeightLevel;//100; Big

        int countField = (int)GridX * (int)GridY;
        Storage.Data.IsUpdatingLocationPersonGlobal = true;

        if (!m_onLoadFields && (Storage.Instance.Fields.Count < countField || countField == 0))
        {
            Debug.Log("!!!!! Fields.Count =" + Storage.Instance.Fields.Count + "   Grid Field Limit =" + countField);
            Storage.Data.IsUpdatingLocationPersonGlobal = false;
            return;
        }
        m_onLoadFields = true;

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
                //for (int y = p_startPosY; y < limitVertical; y++)
                //#TEST FIX : NOT REMOVED NOT IN ZONA FIELDS
                for (int y = p_startPosY - 1; y < limitVertical + 1; y++)
                {

                    string nameField = Helper.GetNameField(x, y);
                    _nameField = nameField;
                    //Find
                    if (!Storage.Instance.Fields.ContainsKey(nameField))
                        continue;

                    GameObject findField = Storage.Instance.Fields[nameField];
                    //Destroy !!!
                    DestroyField(findField);
                    Storage.Instance.Fields.Remove(nameField);
                    if (!isOnlyField)
                    {
                        RemoveRealObjects(nameField);
                        Counter--;
                    }
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

                    if (Storage.Instance.Fields.ContainsKey(nameField))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;

                    GameObject newField = GetPrefabFieldFloor(pos, nameField);
                    newField.name = nameField;
                    Storage.Instance.Fields.Add(nameField, newField);
                    if (!isOnlyField)
                    {
                        Counter++;
                        LoadObjectToReal(nameField);
                    }
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

                //for (int x = p_startPosX; x < limitHorizontal; x++) //#
                //#TEST FIX : NOT REMOVED NOT IN ZONA FIELDS
                for (int x = p_startPosX-1; x < limitHorizontal +1 ; x++) //#
                {

                    string nameField = Helper.GetNameField(x, y);

                    //Find
                    if (!Storage.Instance.Fields.ContainsKey(nameField))
                        continue;

                    GameObject findField = Storage.Instance.Fields[nameField];
                    //Destroy !!!
                    DestroyField(findField);
                    Storage.Instance.Fields.Remove(nameField);
                    if (!isOnlyField)
                    {
                        RemoveRealObjects(nameField);
                        Counter--;
                    }
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

                    if (Storage.Instance.Fields.ContainsKey(nameField))
                        continue;

                    Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
                    pos.z = 0;

                    GameObject newField = GetPrefabFieldFloor(pos, nameField);
                    newField.name = nameField;
                    Storage.Instance.Fields.Add(nameField, newField);
                    if (!isOnlyField)
                    {
                        Counter++;
                        LoadObjectToReal(nameField);
                    }
                }
            }
        }

        Storage.Data.IsUpdatingLocationPersonGlobal = false;
        //Profiler.EndSample();
    }

    public void LoadObjectToReal(string nameField)
    {

        LoadGameObjectDataForLook_2(nameField);
    }

    //загрузка из данныx объектов из памяти и создание их на поле  ADDED FOR LOOK - DATA 2
    private void LoadGameObjectDataForLook_2(string p_nameField)
    {
        var _gridData = Storage.Instance.GridDataG;
        var _gamesObjectsReal = Storage.Instance.GamesObjectsReal;

        if (Storage.Instance.IsCorrectData)
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
            List<ModelNPC.ObjectData> listDataObjectInField = ReaderScene.GetObjectsDataFromGrid(p_nameField);
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
                    listDataObjectInField.RemoveAt(i); ////!@#$
                    continue;
                }
                //--------------
                indErr = "13.";
                //TEST -------
                if (dataObj.IsReality)
                {
                    indErr = "13.1";
                    //Debug.Log("LoadGameObjectDataForLook ********************** " + dataObj + " already IsReality !!!!");
                    var realObj = listGameObjectReal.Find(p => p != null && p.name == dataObj.NameObject);
                    indErr = "13.2";
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
                indErr = "15. dataObj = " + dataObj.NameObject + " " + dataObj.ToString();

                GameObject gobj = CreateGameObjectByData(dataObj);
                indErr = "16.";

                listGameObjectReal.Add(gobj);

                //---------- ACTIVATE
                if (PoolGameObjects.IsUsePoolObjects)
                {
                    var movement = gobj.GetComponent<MovementBoss>();
                    //var movement = newField.GetComponent<MovementNPC>();
                    if (movement != null)
                    {
                        //Debug.Log("~~~~~~~~~~~~~~~ GenGrid Activate UpdateData " + newField.name);
                        movement.UpdateData("Activate");
                        //@@@<
                        gobj.SetActive(true);
                        movement.InitNPC();
                    }
                    else
                    {
                        var movementNPC = gobj.GetComponent<MovementNPC>();
                        if (movementNPC != null)
                        {
                            movementNPC.UpdateData("Activate");
                            gobj.SetActive(true);
                            movementNPC.InitNPC();
                        }
                    }
                    
                    //#fix color
                    //  newField.GetComponent<SpriteRenderer>().color = Color.white;
                    //newField.SetActive(true);

                    //@@@>
                    /*
                    if (movement != null)
                    {
                        //Debug.Log("~~~~~~~~~~~~~~~ GenGrid Activate InitNPC " + newField.name);
                        newField.SetActive(true);
                        movement.InitNPC();
                    }
                    */
                }
                //------------

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
    public void RemoveRealObjects(string p_nameField)
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
                //if (obj != null && obj.name !=null)
                //{
                //    Storage.Log.SaveHistory(obj.name, "DESTROY GOBJ", "RemoveRealObjects");
                //Destroy(obj);
                DestroyObject(obj);
                //}
            }
            //@<<@ Storage.Instance.GamesObjectsReal.Remove(p_nameField);
            Storage.Data.RemoveFieldRealObject(p_nameField, "RemoveRealObjects");
        }
    }

    private int ConflictLog(GameObject gobj, string p_nameField, List<ModelNPC.ObjectData> dataObjects)
    {
        FindPersonData findPersonData = null;
        int indDataNew = -1;
        //var listDataObjsInField = Storage.Person.GetAllDataPersonsForName(p_nameField);
        //----------
        FindPersonData findPersonDataT = Storage.Person.GetFindPersonsDataForName(gobj.name.GetID());
        if (findPersonDataT != null)
        {
            Debug.Log("##..... ConflictLog (" + gobj.name + ") GetFindPersonsDataForName[" + findPersonDataT.Field + "]: " + findPersonDataT.DataObj);
            if (gobj.name == findPersonDataT.DataObj.NameObject && findPersonDataT.Field == p_nameField)
            {
                indDataNew = findPersonDataT.Index;
                if (dataObjects.Count <= indDataNew)
                {
                    Debug.Log("##..... ConflictLog indDataNew[" + indDataNew + "]  out of range " + dataObjects.Count);
                }
                else
                {
                    if (dataObjects[indDataNew].NameObject != gobj.name)
                    {
                        Debug.Log("##..... ConflictLog dataObjects[" + indDataNew + "].NameObject[" + dataObjects[indDataNew].NameObject + "]  <>  GOBJ: " + gobj.name);
                        Debug.Log("##..... Objects in Data : ");
                        foreach (var doItem in dataObjects)
                        {
                            Debug.Log("#................. Data : " + doItem.ToString());
                        }
                        return -1;
                    }
                    else
                    {
                        return indDataNew;
                    }
                }
            }
            else
            {
                Debug.Log("##..... ConflictLog [" + p_nameField + "] (" + gobj.name + ") <> [" + findPersonDataT.Field + "]: " + findPersonDataT.DataObj);
            }
        }
        else
        {
            return -1;
        }
        return -1;
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

        List<ModelNPC.ObjectData> dataObjects = ReaderScene.GetObjectsDataFromGrid(p_nameField);
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
                    //Debug.Log("################# SaveListObjectsToData 1.  DataObject (" + gobj.name + ") not Find in DATA     field: " + p_nameField);
                    indData = 0;
                    bool isFind = false;
                    foreach (var itemObj in ReaderScene.GetObjectsDataFromGrid(p_nameField))
                    {
                        Debug.Log("[" + p_nameField + "] :: " + itemObj.NameObject);
                        if (itemObj.NameObject == gobj.name)
                        {
                            Debug.Log("))))))))))))))))))))  [" + p_nameField + "] :: " + itemObj.NameObject + "    index: " + indData);
                            isFind = true;
                            break;
                        }
                        indData++;
                    }
                    if (!isFind)
                    {
                        Debug.Log("################# SaveListObjectsToData 1.  DataObject (" + gobj.name + ") not Find in DATA     field: " + p_nameField);

                        int newIndex = ConflictLog(gobj, p_nameField, dataObjects);
                        if (newIndex == -1)
                        {
                            Debug.Log("################# SaveListObjectsToData 2.  DataObject (" + gobj.name + ") not Find in DATA     field: " + p_nameField);
                            //Storage.Log.GetHistory(gobj.name);
                            continue;
                        }
                        else
                        {
                            indData = newIndex;
                        }
                    }
                }

                ModelNPC.ObjectData dataObj = dataObjects[indData];
                if (dataObj == null)
                {
                    Debug.Log("################# SaveListObjectsToData 2.  DataObject (" + gobj.name + ") not Find in DATA     field: " + p_nameField);
                    Storage.Log.GetHistory(gobj.name);
                    continue;
                }
                if (dataObj.NameObject != gobj.name)
                {
                    Debug.Log("################# SaveListObjectsToData 2.  DataObject (" + gobj.name + ") <> (" + gobj.name + ") Gobj)");
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
                    Debug.Log("###### SaveListObjectsToData [" + gobj.name + "] posFieldOld(" + posFieldOld + ") != (" + p_nameField + ")p_nameField ");
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

                    indErr = "10. posFieldReal=" + posFieldReal + " <> posFieldOld=" + posFieldOld + "  " + gobj.name;


                    if (!ReaderScene.IsGridDataFieldExist(posFieldReal))
                    {
                        //Debug.Log("######## ?????? Add field " + posFieldReal + "   " + indErr);
                        //$$$LC.1
                        //Storage.Data.AddNewFieldInGrid(posFieldReal, "SaveListObjectsToData");
                    }
                    else
                    {
                        int indValid = ReaderScene.GetObjectsDataFromGrid(posFieldReal).FindIndex(p => p.NameObject == gobj.name);
                        if (indValid != -1)
                        {
                            Debug.Log("################# SaveListObjectsToData ))))))))))))))   Find " + gobj.name + "  in " + posFieldReal);
                            Storage.Data.UpdateDataObect(p_nameField, indData, dataObj, "SaveListObjectsToData", posR); //@<<@ 
                            continue;
                        }
                    }

                    //Debug.Log("___________________ SaveListObjectsToData destroy(" + isDestroy + ")______GO:" + gobj.name + "  DO: " + dataObj.ToString() + "      " + posFieldOld + " >> " + posFieldReal);
                    indErr = "11.";

                    //!!!!!!!!!!!!!!!!!!
                    ModelNPC.ObjectData dataObjNew = (ModelNPC.ObjectData)dataObj.Clone();
                    var name = Helper.CreateName(dataObj.TypePrefabName, posFieldReal, "", gobj.name);

                    indErr = "12.";

                    // #????# ????????????????
                    //Storage.Data.RemoveDataObjectInGrid(p_nameField, i, "SaveListObjectsToData", false,  dataObj); ////@<<@ 
                    if (ReaderScene.IsGridDataFieldExist(posFieldOld))
                    {
                        int indLast = ReaderScene.GetObjectsDataFromGrid(posFieldOld).FindIndex(p => p.NameObject == gobj.name);
                        if (indLast == -1)
                            Storage.Data.RemoveDataObjectInGrid(posFieldOld, indLast, "SaveListObjectsToData", false, dataObj); ////@<<@ 
                    }
                    //--------------------
                    indErr = "13.";

                    //$$$LC.1
                    //if (!_gridData.FieldsD.ContainsKey(posFieldReal))
                    //{
                    //    //$$$LC Storage.Data.AddNewFieldInGrid(posFieldReal, "SaveListObjectsToData"); //@<<@ 
                    //}

                    indErr = "14.";

                    dataObj = dataObjNew;
                    indErr = "15.";
                    //dataObj = dataObjects[indData];
                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    dataObj.NameObject = name;
                    //Debug.Log("___ RENAME : DATA OBJECT: " + testDO + "  >>  " + dataObj.NameObject + "         GO:" + gobj.name);

                    //if (isDestroy)
                    //    dataObj.IsReality = false;
                    indErr = "16.";
                   
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
                        //dataObj.Position = gobj.transform.position;
                        dataObj.SetPosition(gobj.transform.position);//###ERR
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
                        Debug.Log("SaveListObjectsToData -- AddRealObject -- " + gobj.name + " : " + posFieldReal);
                        Storage.Data.AddRealObject(gobj, posFieldReal, "SaveListObjectsToData");
                    }

                    indErr = "24.";
                    //Debug.Log("SaveListObjectsToData -- RemoveRealObject -- " + gobj.name + " : " + p_nameField);
                    Storage.Data.RemoveRealObject(i, p_nameField, "SaveListObjectsToData");
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++

                    indErr = "25.";
                    //@SAVE@
                    if (!isDestroy)
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
        if (_movementUfo != null)
            _movementUfo.UpdateData("GameObjectUpdatePersonData");
    }

    public GameObject CreateGameObjectByData(ModelNPC.ObjectData objData)
    {
        var newGO = Storage.Pool.GetPoolGameObject("new", objData.TypePoolPrefabName, new Vector3(0, 0, 0));
        objData.UpdateGameObject(newGO);
        newGO.transform.position = objData.Position;
        newGO.name = objData.NameObject;

        return newGO;
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
        Debug.Log("SSSSSSSSSSSSS SaveAllRealGameObjects END ^^^^^^^^^^^^^^^^^^^^^: T1 : " + (DateTime.Now - startTestTime).TotalMilliseconds);

        Storage.Instance.IsLoadingWorld = false;
    }

    public void LoadObjectsNearHero()
    {
        try
        {
            if (Storage.GenGrid.IsLoadOnlyField)
                return;

            var keys = Storage.Instance.Fields.Select(p => p.Key).ToArray();
            //Debug.Log("______________________LoadObjectsNearHero__________________");
            //foreach (var nameField in Storage.Instance.Fields.Select(p => p.Key))
            foreach (var nameField in keys)
            {
                //string nameField = field.Key;
                LoadObjectToReal(nameField);
            }

        }catch(Exception x)
        {
            Debug.Log("########### LoadObjectsNearHero !!!!!!!!!!! " + x.Message);
        }
    }

    public void ReloadGridLook()
    {
        foreach (var fieldItem in Storage.Instance.Fields.Select(p => p.Key))
        {
            //Debug.Log("remove   " + fieldItem + " ....");
            Storage.GenGrid.RemoveRealObjects(fieldItem);
        }
        LoadObjectsNearHero();
    }

    //static Type CompType;
    private GameObject GetPrefabFieldFloor(Vector3 pos, string nameFieldNew)
    {
        GameObject resGO;
        resGO = Storage.Pool.InstantiatePool(prefabField, pos, nameFieldNew);
        ModelNPC.TerraData terrD = new ModelNPC.TerraData()
        {
            ModelView = "Weed"
        };
        terrD.UpdateGameObject(resGO);
        //fix alpha field
        resGO.GetComponent<SpriteRenderer>().sortingLayerName = Helper.LayerTerraName;
        return resGO;
    }

    private bool DestroyField(GameObject findGobjField)
    {
        //For Pool
        Storage.Pool.DestroyPoolGameObject(findGobjField);
        return true;
    }

    private bool DestroyObject(GameObject findGobj)
    {
        //For Pool
        Storage.Pool.DestroyPoolGameObject(findGobj);
        return true;
    }

}
