using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageCorrect : MonoBehaviour {

    //public StorageCorrect()
    //{

    //}


    //private static StorageCorrect _instance;
    //public static StorageCorrect Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new StorageCorrect();
    //        }
    //        return _instance;
    //    }
    //}

    // Use this for initialization
    void Start()
    {

    }

    //public static StorageCorrect Instance { get; private set; }
    ////   //public static Storage Instance { get; private set; }
    //public void Awake()
    //{
    //    Instance = this;
    //}

    //   // Update is called once per frame
    void Update()
    {

    }

    #region Correct

    //@CD@
    public void CorrectData(string nameObj, string callFunc)
    {
        bool isCorrect = false;

        if (Storage.Instance.IsCorrectData)
        {
            Debug.Log("_______________ RETURN CorrectData ON CORRECT_______________");
            return;
        }
        Storage.Instance.IsCorrectData = true;

        Debug.Log("<<<<<<<<<<<<<<<<<<<<<<<1.>>>>>>>>>>>>>>>>>>>>>> CorrectData BY NAME : " + nameObj + "       call:" + callFunc);

        //string idObj = GetID(nameObj);
        //string typeObj = GetTag(nameObj);
        //Vector2 posObj = GetPositByField(nameObj);
        bool isRemReal = Storage.Data.RemoveAllFindRealObject(nameObj);
        bool isRemData = Storage.Data.RemoveAllFindDataObject(nameObj);
        bool isRemGameObj = false;


        GameObject findGO = GameObject.Find(nameObj);
        if (findGO != null)
        {
            Debug.Log("--- CorrectData : yes find GameObject by Name: " + findGO.name);
            //Debug.Log("--- CorrectData : DESTROY FGO:");
            Destroy(findGO);
            //Debug.Log("--- CorrectData : DESTROY REAL FGO:");
            Storage.Instance.DestroyFullObject(findGO, true);
            isRemGameObj = Storage.Data.RemoveAllFindDataObject(nameObj);
            //@@CORRECT !!!
            //isRemGameObj = true;
        }
        else
        {
            Debug.Log("--- CorrectData : NOT find GameObject by Name: " + nameObj);
        }
        Storage.Instance.IsCorrectData = false;
        if (isRemGameObj || isRemReal || isRemData)
        {
            string strErr = "1.";
            if (nameObj == null)
            {
                Debug.Log("################## CorrectData StartCreateNewCorrectObject: name: is null");
                Storage.Instance.CorrectCreateName = "";
                return;
            }

            try
            {
                strErr = "2.";
                //CreateNewCorrectObject(nameObj, "CorrectData 2.     GO=" + isRemGameObj + " RO=" + isRemReal + " DO:" + isRemData);
                string _info = "CorrectData 2.     GO=" + isRemGameObj + " RO=" + isRemReal + " DO:" + isRemData + " name: " + nameObj;

                strErr = "3.";
                if (_info==null)
                {

                    _info = "CorrectData 2.";
                }
                strErr = "4.";
                //StartCoroutine(StartCreateNewCorrectObject(nameObj, _info));
                //@TEST@
                //CreateNewCorrectObject(nameObj, _info); 
                string corrName = Storage.Instance.CorrectCreateName;
                if (!String.IsNullOrEmpty(corrName) && Helper.GetID(corrName) == Helper.GetID(nameObj))
                {
                    Debug.Log("----- 2.CorrectData  WAIT Create : " + corrName);
                    return;
                }
                Storage.Instance.CorrectCreateName = nameObj;
                StartCoroutine(StartCreateNewCorrectObject(nameObj, _info));
                //Storage.Instance.StartCor(nameObj, _info);

            } catch (Exception x)
            {
                Debug.Log("################# ERROR CorrectData EX (" + strErr + ") :" + x.ToString());
                Storage.Instance.CorrectCreateName = "";
            }

        }
        else
        {
            //Storage.Instance.CorrectCreateName = "";
        }
    }
    //@CD@ //--- CORRECT -----
    public void CorrectData(GameObject realGO, GameObject thisGO, string callFunc)
    {
        if (Storage.Instance.IsCorrectData)
        {
            Debug.Log("_______________ RETURN CorrectData ON CORRECT_______________");
            return;
        }

        Storage.Instance.IsCorrectData = true;
#if UNITY_EDITOR
        Debug.Log("UNITY_EDITOR ++++++++++++++++++++ CorrectData : " + callFunc);
#else
    Debug.Log("Booo..");
#endif
        Debug.Log("<<<<<<<<<<<<<<<<2.>>>>>>>>>>>>>>>>>>> CorrectData : " + callFunc);

        bool isExistRealObj = false;
        if (realGO != null)
            isExistRealObj = true;

        //@DESTROY@
        //string nameT = GetID(thisGO.name);
        string nameObjT = thisGO.name;
        string nameObj = nameObjT;

        if (isExistRealObj)
            nameObj = realGO.name;

        bool isRemovedThis = false;
        bool isRemovedThis2 = false;
        bool isRemovedReal = false;
        bool isRemovedGObj = false;

        //string idObj = GetID(name);

        if (isExistRealObj && nameObj != nameObjT)
        {
            Debug.Log("--- CorrectData : names not equals: " + nameObj + "     <>  " + nameObjT);
            GameObject findGO1 = GameObject.Find(nameObjT);
            if (findGO1 != null)
            {
                Debug.Log("--- CorrectData : yes find GameObject by Name: " + findGO1.name);
                Destroy(findGO1);
                isRemovedThis2 = Storage.Instance.DestroyFullObject(findGO1, true);
            }
            else
            {
                Debug.Log("--- CorrectData : NOT find GameObject by Name: " + findGO1.name);
            }
        }

        GameObject findGO = GameObject.Find(nameObj);
        if (findGO != null)
        {
            //Debug.Log("--- CorrectData : yes find GameObject by Name: " + findGO.name);
            //Debug.Log("--- CorrectData : DESTROY FGO:");
            Destroy(findGO);
            //Debug.Log("--- CorrectData : DESTROY REAL FGO:");
            isRemovedGObj = Storage.Instance.DestroyFullObject(findGO, true);
            Debug.Log("--- CorrectData : DESTROY DATA FGO:");
        }
        else
        {
            Debug.Log("--- CorrectData : NOT find GameObject by Name: " + nameObj);
        }

        isRemovedThis = Storage.Instance.DestroyFullObject(thisGO, true);
        if (isExistRealObj)
        {
            isRemovedReal = Storage.Instance.DestroyFullObject(realGO, true);
        }

        Storage.Instance.IsCorrectData = false;

        //CREATE NEW CORERECT
        //CreateNewCorrectObject(name);
        bool isRemoved = (isRemovedThis || isRemovedThis2 || isRemovedReal || isRemovedGObj);
        if (isRemoved)
        {
            if (nameObj == null)
            {
                Debug.Log("############# CorrectData  nameObj==null");
                Storage.Instance.CorrectCreateName = "";
                return;
            }
            try
            {
                Debug.Log("--- CorrectData 1.  ---- start Coroutine CreateNewCorrectObject .......");
                string _info = "  FGO=" + isRemovedGObj + " RO=" + isRemovedReal + " TGO:" + isRemovedThis + " TGO1:" + isRemovedThis2;

                string corrName = Storage.Instance.CorrectCreateName;
                if (!String.IsNullOrEmpty(corrName) && Helper.GetID(corrName) == Helper.GetID(nameObj))
                {
                    Debug.Log("----- 1.CorrectData  WAIT Create : " + corrName);
                    return;
                }
                Storage.Instance.CorrectCreateName = nameObj;
                StartCoroutine(StartCreateNewCorrectObject(nameObj, _info));
                //Storage.Instance.StartCor(nameObj, _info);
            }
            catch (Exception x)
            {
                Debug.Log("############# ERROR CorrectData EX: +" + x.Message);
                Storage.Instance.CorrectCreateName = "";
            }
        }
        else
        {
            Debug.Log("Not Start CreateNewCorrectObject...");
            //Storage.Instance.CorrectCreateName = "";
        }
    }

    //@CD@ //--- CORRECT NEW -----
    private void CreateNewCorrectObject(string p_name, string callFunc)
    {
        try
        {

            Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Debug.Log("+++++++ 1. CreateNewCorrectObject   start ++++ " + p_name + "      " + callFunc);

            string idObj = Helper.GetID(p_name);
            string typeObj = Helper.GetTag(p_name);
            string fieldName = Helper.GetNameFieldByName(p_name);
            Vector2 posObj = Helper.GetPositByField(fieldName);
            CreateNewCorrectObject(idObj, typeObj, (int)posObj.x, (int)posObj.y);
        }catch(Exception x)
        {
            Debug.Log("################# ERROR CreateNewCorrectObject EX:" + x.ToString());
        }
    }

    public IEnumerator StartCreateNewCorrectObject(string p_name, string callFunc)
    {
        
        Debug.Log("--- Coroutine StartCreateNewCorrectObject wait.......");

        //yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1f);

        try
        {
            string corrName = Storage.Instance.CorrectCreateName;
            if (!String.IsNullOrEmpty(corrName) && Helper.GetID(corrName) == Helper.GetID(p_name))
            {
                Debug.Log("----- COROUTINE  StartCreateNewCorrectObject  WAIT Create : " + corrName);
                yield break;
            }

            Debug.Log("--- Coroutine StartCreateNewCorrectObject start....... " + callFunc);

            CreateNewCorrectObject(p_name, callFunc); ;
        }
        catch (Exception x)
        {
            Debug.Log("################# ERROR StartCreateNewCorrectObject EX:" + x.ToString());
            //Storage.Instance.IsCorrectData = false;
            Storage.Instance.CorrectCreateName = "";
        }

        //Storage.Instance.IsCorrectData = false;
        //Storage.Instance.CorrectCreateName = "";

        yield break;
    }

    //@CD@ //--- CORRECT NEW -----
    private void CreateNewCorrectObject(string idObj, string prefabName, int x, int y)//, string nameField)
    {
        Debug.Log("+++++++ 2. CreateNewCorrectObject   start ++++ " + idObj + "    " + prefabName + "      " + x + "x" + y);

        SaveLoadData.TypePrefabs prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), prefabName);

        int _y = y * (-1);
        Vector3 pos = new Vector3(x, _y, 0) * SaveLoadData.Spacing;
        pos.z = -2;

        //string nameField = GetNameFieldPosit(x,y);
        string nameField = Storage.FieldKey + x + "x" + y;
        string nameObject = Helper.CreateName(prefabName.ToString(), nameField, idObj);// prefabName.ToString() + "_" + nameFiled + "_" + i;

        Debug.Log("+++++++ CreateNewCorrectObject  create Name Object : " + nameObject);

        //CREATE DATA
        SaveLoadData.ObjectData objDataSave = SaveLoadData.BildObjectData(prefabType);
        objDataSave.NameObject = nameObject;
        objDataSave.TagObject = prefabName.ToString();
        objDataSave.Position = pos;
        Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateNewCorrectObject");

        Debug.Log("+++++++ CreateNewCorrectObject  Data +: " + objDataSave);

        List<GameObject> listGameObjectReal = new List<GameObject>();
        if (!Storage.Instance.GamesObjectsReal.ContainsKey(nameField))
        {
            Storage.Instance.GamesObjectsReal.Add(nameField, listGameObjectReal);
        }
        else
        {
            listGameObjectReal = Storage.Instance.GamesObjectsReal[nameField];
        }

        //ADD IN REAL
        GameObject newField = Storage.Instance.CreatePrefab(objDataSave);
        //GameObject newField = CreatePrefabByName(objDataSave);
        listGameObjectReal.Add(newField);

        Debug.Log("+++++++ CreateNewCorrectObject  Real +: " + newField.name);
        Storage.Instance.SelectGameObjectID = idObj;
        Debug.Log("EEEEEEEEEEEEEEEEEEEEEEEE  Real +: " + newField.name);
    }

    #endregion

}
