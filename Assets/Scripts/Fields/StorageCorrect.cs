using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageCorrect : MonoBehaviour {

    public static StorageCorrect Instance { get; private set; }

    // Use this for initialization
    void Start () {
		
	}

    //public static Storage Instance { get; private set; }
    public void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}

    #region Correct

    //@CD@
    public void CorrectData(string nameObj)
    {
        bool isCorrect = false;

        if (Storage.Instance.IsCorrectData)
        {
            Debug.Log("_______________ RETURN CorrectData ON CORRECT_______________");
            return;
        }
        Storage.Instance.IsCorrectData = true;

        Debug.Log("++++++++++++++++++++ CorrectData BY NAME : " + nameObj);

        //string idObj = GetID(nameObj);
        //string typeObj = GetTag(nameObj);
        //Vector2 posObj = GetPositByField(nameObj);
        bool isRemReal = Storage.Instance.RemoveAllFindRealObject(nameObj);
        bool isRemData = Storage.Instance.RemoveAllFindDataObject(nameObj);
        bool isRemGameObj = false;


        GameObject findGO = GameObject.Find(nameObj);
        if (findGO != null)
        {
            Debug.Log("--- CorrectData : yes find GameObject by Name: " + findGO.name);
            //Debug.Log("--- CorrectData : DESTROY FGO:");
            Destroy(findGO);
            //Debug.Log("--- CorrectData : DESTROY REAL FGO:");
            Storage.Instance.DestroyFullObject(findGO, true);
            isRemGameObj = Storage.Instance.RemoveAllFindDataObject(nameObj);
        }
        else
        {
            Debug.Log("--- CorrectData : NOT find GameObject by Name: " + nameObj);
        }
        Storage.Instance.IsCorrectData = false;
        if (isRemGameObj || isRemReal || isRemData)
        {
            //CreateNewCorrectObject(nameObj, "CorrectData 2.     GO=" + isRemGameObj + " RO=" + isRemReal + " DO:" + isRemData);
            string _info = "CorrectData 2.     GO=" + isRemGameObj + " RO=" + isRemReal + " DO:" + isRemData;
            StartCoroutine(StartCreateNewCorrectObject(name, _info));
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
        Debug.Log("++++++++++++++++++++ CorrectData : " + callFunc);

        bool isExistRealObj = false;
        if (realGO != null)
            isExistRealObj = true;

        //@DESTROY@
        //string nameT = GetID(thisGO.name);
        string nameT = thisGO.name;
        string name = nameT;

        if (isExistRealObj)
            name = realGO.name;

        bool isRemovedThis = false;
        bool isRemovedThis2 = false;
        bool isRemovedReal = false;
        bool isRemovedGObj = false;

        //string idObj = GetID(name);

        if (isExistRealObj && name != nameT)
        {
            Debug.Log("--- CorrectData : names not equals: " + name + "     <>  " + nameT);
            GameObject findGO1 = GameObject.Find(nameT);
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

        GameObject findGO = GameObject.Find(name);
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
            Debug.Log("--- CorrectData : NOT find GameObject by Name: " + name);
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
            Debug.Log("--- CorrectData  ---- start Coroutine CreateNewCorrectObject .......");

            string _info = "  FGO=" + isRemovedGObj + " RO=" + isRemovedReal + " TGO:" + isRemovedThis + " TGO1:" + isRemovedThis2;
            StartCoroutine(StartCreateNewCorrectObject(name, "CorrectData 1.    " + _info));
        }
        else
            Debug.Log("Not Start CreateNewCorrectObject...");
    }

    //@CD@ //--- CORRECT NEW -----
    private void CreateNewCorrectObject(string name, string callFunc)
    {
        Debug.Log("**************************************************************");
        Debug.Log("+++++++ 1. CreateNewCorrectObject   start ++++ " + name + "      " + callFunc);

        string idObj = Storage.GetID(name);
        string typeObj = Storage.GetTag(name);
        string fieldName = Storage.GetNameFieldByName(name);
        Vector2 posObj = Storage.Instance.GetPositByField(fieldName);
        CreateNewCorrectObject(idObj, typeObj, (int)posObj.x, (int)posObj.y);
    }

    IEnumerator StartCreateNewCorrectObject(string name, string callFunc)
    {
        Debug.Log("--- Coroutine StartCreateNewCorrectObject wait.......");

        //yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1f);

        Debug.Log("--- Coroutine StartCreateNewCorrectObject start.......");

        CreateNewCorrectObject(name, callFunc); ;

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
        string nameObject = Storage.CreateName(prefabName.ToString(), nameField, idObj);// prefabName.ToString() + "_" + nameFiled + "_" + i;

        Debug.Log("+++++++ CreateNewCorrectObject  create Name Object : " + nameObject);

        //CREATE DATA
        SaveLoadData.ObjectData objDataSave = SaveLoadData.BildObjectData(prefabType);
        objDataSave.NameObject = nameObject;
        objDataSave.TagObject = prefabName.ToString();
        objDataSave.Position = pos;
        Storage.Instance.AddDataObjectInGrid(objDataSave, nameField, "CreateNewCorrectObject");

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
        listGameObjectReal.Add(newField);

        Debug.Log("+++++++ CreateNewCorrectObject  Real +: " + newField.name);
        Storage.Instance.SelectGameObjectID = idObj;
        Debug.Log("EEEEEEEEEEEEEEEEEEEEEEEE  Real +: " + newField.name);
    }

    #endregion

}
