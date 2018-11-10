using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdateData { //: MonoBehaviour {

    public UpdateData()
    {

    }

    //private static UpdateData _instance;
    //public static UpdateData Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new UpdateData();
    //        }
    //        return _instance;
    //    }
    //}


    private Dictionary<string, List<GameObject>> _GamesObjectsReal
    {
        get { return Storage.Instance.GamesObjectsReal; }
    }

    private string _datapathLevel
    {
        get { return Storage.Instance.DataPathLevel; }
    }

    private string _datapathPerson
    {
        get { return Storage.Instance.DataPathPerson; }
    }

    private ModelNPC.GridData _GridDataG
    {
        get
        {
            return Storage.Instance.GridDataG;
        }
    }

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    #region Add Remove Update Real and Data


    public bool RemoveAllFindRealObject(string nameObj)
    {
        bool isRemoved = false;
        string idObj = Helper.GetID(nameObj);
        //-----------------------FIXED Correct
        foreach (var item in Storage.Instance.GamesObjectsReal)
        {


            string nameField = item.Key;
            List<GameObject> resListData = _GamesObjectsReal[nameField].Where(p => { return p.name.IndexOf(idObj) != -1; }).ToList();
            if (resListData != null)
            {
                for (int i = 0; i < resListData.Count(); i++)
                {
                    var obj = resListData[i];
                    if (obj == null)
                    {
                        Debug.Log("+++++ CORRECT ++++  DELETE (" + idObj + ") >>>> in Real Object Fields: " + nameField + "     obj is null");
                        continue;
                    }
                    Debug.Log("+++++ CORRECT ++++  DELETE (" + idObj + ") >>>> in Real Object Fields: " + nameField + "     obj=" + obj);
                    //Storage.Instance.RemoveDataObjectInGrid(nameField, i, "NextPosition", true);
                    RemoveRealObject(i, nameField, "NextPosition");
                    isRemoved = true;
                }
            }
        }
        //---------------------
        return isRemoved;
    }

    public bool RemoveAllFindDataObject(string nameObj)
    {
        //Debug.Log("+++++ RemoveAllFindDataObject +++ start :" + nameObj);

        bool isRemoved = false;
        string idObj = Helper.GetID(nameObj);

        //Debug.Log("+++++ RemoveAllFindDataObject +++ start :" + idObj);
        //-----------------------FIXED Correct
        foreach (var item in _GridDataG.FieldsD)
        {
            string nameField = item.Key;
            List<ModelNPC.ObjectData> resListData = _GridDataG.FieldsD[nameField].Objects.Where(p => { return p.NameObject.IndexOf(idObj) != -1; }).ToList();
            if (resListData != null)
            {
                var resListDataTest = Storage.Instance.GridDataG.FieldsD[nameField].Objects.Where(p => { return p.NameObject.IndexOf(idObj) != -1; });
                if (resListDataTest.Count() != resListData.Count())
                {
                    Debug.Log("+++++ RemoveAllFindDataObject: resListDataTest.Count(" + resListDataTest.Count() + ") != resListData.Count(" + resListData.Count() + ")");
                }
                //foreach (var obj in resListData)
                for (int i = 0; i < resListData.Count(); i++)
                {
                    var obj = resListData[i];
                    if (obj == null)
                    {
                        Debug.Log("+++++ CORRECT ++++  DELETE (" + idObj + ") >>>> in DATA Object Fields: " + nameField + "     obj is null");
                        continue;
                    }
                    Debug.Log("+++++ CORRECT ++++  DELETE (" + idObj + ") >>>> in DATA Object Fields: " + nameField + "     obj=" + obj);
                    RemoveDataObjectInGrid(nameField, i, "NextPosition");
                    isRemoved = true;
                }
            }
        }
        return isRemoved;
    }

    //----- Data Object
    //public void ClearGridData()
    //{
    //    _GridDataG = new SaveLoadData.GridData();
    //}

    public void SaveGridGameObjectsXml(bool isNewWorld = false)
    {
        Serializator.SaveGridXml(_GridDataG, _datapathLevel, isNewWorld);
    }

    public ModelNPC.FieldData AddNewFieldInGrid(string newField, string callFunc)
    {
        ModelNPC.FieldData fieldData = new ModelNPC.FieldData() { NameField = newField };
        _GridDataG.FieldsD.Add(newField, fieldData);

        //! SaveHistory("", "AddNewFieldInGrid", callFunc, newField);

        return fieldData;
    }

    public void AddDataObjectInGrid(ModelNPC.ObjectData objDataSave, string nameField, string callFunc, bool isClaerField = false)
    {
        ModelNPC.FieldData fieldData;
        if (!_GridDataG.FieldsD.ContainsKey(nameField))
        {
            fieldData = AddNewFieldInGrid(nameField, callFunc);
        }
        else
        {
            fieldData = _GridDataG.FieldsD[nameField];
        }

        if (isClaerField)
            fieldData.Objects.Clear();

        fieldData.Objects.Add(objDataSave);

        Storage.Log.SaveHistory(objDataSave.NameObject, "AddDataObjectInGrid", callFunc, nameField, "", null, objDataSave);
    }

    public void RemoveDataObjectInGrid(string nameField, int index, string callFunc, bool isDebug = false, ModelNPC.ObjectData dataObjDel = null)
    {
        ModelNPC.ObjectData histData = null;
        if (Storage.Log.IsSaveHistory)
            histData = _GridDataG.FieldsD[nameField].Objects[index];
        if (isDebug)
            Debug.Log("****** RemoveDataObjectInGrid : start " + histData);

        if(dataObjDel!=null && dataObjDel.NameObject != histData.NameObject)
        {
            index = _GridDataG.FieldsD[nameField].Objects.FindIndex(p => p.NameObject == dataObjDel.NameObject);
            if (index == -1)
            {
                Debug.Log("###################### RemoveDataObjectInGrid    Data Del: " + dataObjDel.NameObject + "     Data Find: " + histData.NameObject + "  ... NOT find in Field: " + nameField);
                Storage.Log.SaveHistory(histData.NameObject, "ERROR RemoveDataObjectInGrid", callFunc, nameField, "Conflict Name", dataObjDel, histData);
                return;
            }
            histData = _GridDataG.FieldsD[nameField].Objects[index];
            if(dataObjDel.NameObject != histData.NameObject)
            {
                Debug.Log("###################### RemoveDataObjectInGrid    Data Del: " + dataObjDel.NameObject + "     Data Find: " + histData.NameObject + "  ... NOT find in Field: " + nameField);
                Storage.Log.SaveHistory(histData.NameObject, "ERROR RemoveDataObjectInGrid", callFunc, nameField, "Conflict Name", dataObjDel, histData);
                return;
            }
        }
        _GridDataG.FieldsD[nameField].Objects.RemoveAt(index);

        if (Storage.Log.IsSaveHistory)
        {
            if (histData == null)
            {
                Debug.Log("##################### Error RemoveDataObjectInGrid save SaveHistory  histData == null ");
                return;
            }
            Storage.Log.SaveHistory(histData.NameObject, "RemoveDataObjectInGrid", callFunc, nameField, "", histData, dataObjDel);
        }
    }

    public void UpdateDataObect(string nameField, int index, ModelNPC.ObjectData setObject, string callFunc, Vector3 newPos = new Vector3())
    {
        if (Storage.Log.IsSaveHistory)
        {
            ModelNPC.ObjectData oldObj = _GridDataG.FieldsD[nameField].Objects[index];
            Storage.Log.SaveHistory(setObject.NameObject, "UpdateDataObect", callFunc, nameField, "", oldObj, setObject);
            Storage.Log.SaveHistory(oldObj.NameObject, "UpdateDataObect", callFunc, nameField, "RESAVE", oldObj, setObject);
        }

        var testPos = new Vector3();
        if (testPos != newPos)
        {
            //Debug.Log("------------------------ UpdateDataObect NEW Pos");
            setObject.Position = newPos;
        }
        //List<SaveLoadData.ObjectData> dataObjects = _gridData.FieldsD[p_nameField].Objects;
        _GridDataG.FieldsD[nameField].Objects[index] = setObject;
    }

    //public void UpdateDataObect(string nameField, string nameObj, SaveLoadData.ObjectData setObject, string callFunc, Vector3 newPos = new Vector3())
    //{
    //    if(!_GridDataG.FieldsD.ContainsKey(nameField))
    //    {
    //        Debug.Log("################### UpdateDataObect not in DATA Field:" + nameField);
    //        return;
    //    }

    //    int index = _GridDataG.FieldsD[nameField].Objects.FindIndex(p => p.NameObject == nameObj);
    //    if(index==-1)
    //    {
    //        Debug.Log("################### UpdateDataObect not DATA (" + nameField + ") Find : " + nameObj);
    //        return;
    //    }
    //    UpdateDataObect(nameField, index, setObject, callFunc, newPos);

    //}

    //----- Real Object

    public List<GameObject> AddNewFieldInRealObject(string newField, string callFunc)
    {
        List<GameObject> listRealObjects = new List<GameObject>();

        _GamesObjectsReal.Add(newField, listRealObjects);

        Storage.Log.SaveHistory("", "AddNewFieldInRealObject", callFunc, newField);

        return listRealObjects;
    }

    public void RemoveFieldRealObject(string nameField, string callFunc)
    {
        _GamesObjectsReal.Remove(nameField);

        //! SaveHistory("", "RemoveFieldRealObject", callFunc, nameField);
    }

    public void AddRealObject(GameObject p_saveObject, string nameField, string callFunc)
    {
        _GamesObjectsReal[nameField].Add(p_saveObject);

        Storage.Log.SaveHistory(p_saveObject.name, "AddRealObject", callFunc, nameField);
    }

    public void RemoveRealObject(int indexDel, string nameField, string callFunc)
    {
        //List<GameObject> objects = _GamesObjectsReal[nameField];
        if (!_GamesObjectsReal.ContainsKey(nameField))
        {
            Debug.Log("################ RemoveRealObject    Not Filed: " + nameField);
            return;
        }
        if (_GamesObjectsReal[nameField].Count - 1 < indexDel)
        {
            Debug.Log("################ RemoveRealObject  " + nameField + "  Not indexDel: " + indexDel);
            return;
        }
        if (_GamesObjectsReal[nameField][indexDel] == null)
            Storage.Log.SaveHistory("Destroy real obj", "RemoveRealObject", callFunc, nameField);
        else
            Storage.Log.SaveHistory(_GamesObjectsReal[nameField][indexDel].name, "RemoveRealObject", callFunc, nameField);

        _GamesObjectsReal[nameField].RemoveAt(indexDel);
    }

    #endregion

}
