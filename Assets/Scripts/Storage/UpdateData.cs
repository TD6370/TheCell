using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;
using System.Threading;

public class UpdateData {

    bool isTestSlow = false;// true;

    public bool IsUpdatingLocationPersonGlobal { get; set; }
    public int UpdatingLocationPersonLocal { get; set; }

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

        if(Storage.Map.IsGridMap)
            Storage.Map.CheckSector(newField);

        return fieldData;
    }

    public bool AddDataObjectInGrid(ModelNPC.ObjectData objDataSave, string nameField, string callFunc)
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

        if (isTestSlow)
        {
            var ind = fieldData.Objects.FindIndex(p => p.NameObject == objDataSave.NameObject);
            if (ind != -1)
            {

                Debug.Log("########## AddDataObjectInGrid [" + objDataSave.NameObject + "] DUBLICATE:   in " + nameField + "    " + callFunc);

                //var gobj = fieldData.Objects.Find(p => p.NameObject == objDataSave.NameObject);
                //if (gobj != null)
                //Storage.Instance.AddDestroyGameObject(gobj.NameObject);
                Storage.Log.GetHistory(objDataSave.NameObject);
                return false;
            }
        }

        fieldData.Objects.Add(objDataSave);

        if (Storage.Map.IsGridMap)
            Storage.Map.CheckSector(nameField);

        Storage.Log.SaveHistory(objDataSave.NameObject, "AddDataObjectInGrid", callFunc, nameField, "", null, objDataSave);

        return true;
    }


    //public bool AddDataObjectInGrid(ModelNPC.ObjectData objDataSave, string nameField, string callFunc, bool isClaerField = false, bool isTestFilledField = false, bool isTestExistMeType = false,
    public bool AddDataObjectInGrid(ModelNPC.ObjectData objDataSave, string nameField, string callFunc,
         PaletteMapController.SelCheckOptDel p_TypeModeOptStartDelete = PaletteMapController.SelCheckOptDel.None,
        PaletteMapController.SelCheckOptDel p_TypeModeOptStartCheck = PaletteMapController.SelCheckOptDel.None)
    {
        bool isLog = true;

        //bool isDel = p_TypeModeOptStartDelete != PaletteMapController.SelCheckOptDel.None;

        if (isLog)
        {
            Storage.EventsUI.ListLogAdd = "Add IN GRID: d: " + p_TypeModeOptStartDelete + " c: " + p_TypeModeOptStartCheck;  
        }

        ModelNPC.FieldData fieldData;
        if (!_GridDataG.FieldsD.ContainsKey(nameField))
        {
            fieldData = AddNewFieldInGrid(nameField, callFunc);
        }
        //else
        else 
        {
            fieldData = _GridDataG.FieldsD[nameField];

            if (p_TypeModeOptStartDelete != PaletteMapController.SelCheckOptDel.DelFull)
            { 
                //if (!isDel)
                //{
                //if (isTestFilledField)
                if (p_TypeModeOptStartDelete != PaletteMapController.SelCheckOptDel.DelFull && p_TypeModeOptStartCheck == PaletteMapController.SelCheckOptDel.DelFull)
                    return false;

                //if (isTestExistMeType)
                if (p_TypeModeOptStartDelete != PaletteMapController.SelCheckOptDel.DelType && 
                    p_TypeModeOptStartCheck == PaletteMapController.SelCheckOptDel.DelType)
                {
                    var indTM = fieldData.Objects.FindIndex(p => p.TypePrefabName == objDataSave.TypePrefabName);
                    if (indTM != -1)
                    {
                        if(isLog)
                            Storage.EventsUI.ListLogAdd = "Add IN GRID: " + "Check Type";
                        return false;
                    }
                }
                if (p_TypeModeOptStartDelete != PaletteMapController.SelCheckOptDel.DelPrefab && 
                    p_TypeModeOptStartCheck == PaletteMapController.SelCheckOptDel.DelPrefab)
                {
                    //var indTM = fieldData.Objects.FindIndex(p => p.TagObject.IsTerra());
                    var indTM = fieldData.Objects.FindIndex(p => !p.TypePrefabName.IsField());
                    if (indTM != -1)
                    {
                        if (isLog)
                            Storage.EventsUI.ListLogAdd = "Add IN GRID: " + "Check Prefab";
                        return false;
                    }
                }
                if (p_TypeModeOptStartDelete != PaletteMapController.SelCheckOptDel.DelTerra && 
                    p_TypeModeOptStartCheck == PaletteMapController.SelCheckOptDel.DelTerra)
                {
                    //var indTM = fieldData.Objects.FindIndex(p => !p.TagObject.IsTerra());
                    var indTM = fieldData.Objects.FindIndex(p => !p.TypePrefabName.IsField());
                    if (indTM != -1)
                    {
                        if (isLog)
                            Storage.EventsUI.ListLogAdd = "Add IN GRID: " + "Check Terra";
                        return false;
                    }
                }
                //}
                //else
                //{
                if (p_TypeModeOptStartDelete == PaletteMapController.SelCheckOptDel.DelType)
                {
                    var ListRemove = fieldData.Objects.Where(p => p.TypePrefabName == objDataSave.TypePrefabName).ToList();
                    for(int i=ListRemove.Count -1; i>=0;i--)
                    {
                        if (isLog)
                            Storage.EventsUI.ListLogAdd = "Add IN GRID: " + "CLEAR type";
                        //ListRemove.RemoveAt(i);
                        fieldData.Objects.Remove(ListRemove[i]);
                    }
                }
                else if (p_TypeModeOptStartDelete == PaletteMapController.SelCheckOptDel.DelPrefab)
                {
                    var ListRemove = fieldData.Objects.Where(p => !p.TypePrefabName.IsField()).ToList();
                    for (int i = ListRemove.Count - 1; i >= 0; i--)
                    {
                        if (isLog)
                            Storage.EventsUI.ListLogAdd = "Add IN GRID: " + "CLEAR prefab";
                        //ListRemove.RemoveAt(i);
                        fieldData.Objects.Remove(ListRemove[i]);
                    }
                }
                else if (p_TypeModeOptStartDelete == PaletteMapController.SelCheckOptDel.DelTerra)
                {
                    var ListRemove = fieldData.Objects.Where(p => p.TypePrefabName.IsField()).ToList();
                    for (int i = ListRemove.Count - 1; i >= 0; i--)
                    {
                        if (isLog)
                            Storage.EventsUI.ListLogAdd = "Add IN GRID: " + "CLEAR terra";
                        //ListRemove.RemoveAt(i);
                        fieldData.Objects.Remove(ListRemove[i]);
                    }
                }

            }
        }

        if (p_TypeModeOptStartDelete == PaletteMapController.SelCheckOptDel.DelFull)
        {
            if (isLog)
                Storage.EventsUI.ListLogAdd = "Add IN GRID: " + "CLEAR FULL ";
            fieldData.Objects.Clear();
        }

        //if (isClaerField)
        //{
        //    fieldData.Objects.Clear();
        //}

        if (isTestSlow)
        {
            var ind = fieldData.Objects.FindIndex(p => p.NameObject == objDataSave.NameObject);
            if (ind != -1)
            {

                Debug.Log("########## AddDataObjectInGrid [" + objDataSave.NameObject + "] DUBLICATE:   in " + nameField + "    " + callFunc);

                //var gobj = fieldData.Objects.Find(p => p.NameObject == objDataSave.NameObject);
                //if (gobj != null)
                    //Storage.Instance.AddDestroyGameObject(gobj.NameObject);
                Storage.Log.GetHistory(objDataSave.NameObject);
                return false;
            }
        }

        fieldData.Objects.Add(objDataSave);

        if (Storage.Map.IsGridMap)
            Storage.Map.CheckSector(nameField);

        Storage.Log.SaveHistory(objDataSave.NameObject, "AddDataObjectInGrid", callFunc, nameField, "", null, objDataSave);

        return true;
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

        if (Storage.Map.IsGridMap)
            Storage.Map.CheckSector(nameField);
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

        if (Storage.Map.IsGridMap)
            Storage.Map.CheckSector(nameField);
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

    public bool AddRealObject(GameObject p_saveObject, string nameField, string callFunc)
    {
        try
        {
            if (p_saveObject == null || p_saveObject.name == null)
            {
                Debug.Log("######## AddRealObject is DESTROYED: in " + nameField + "    " + callFunc);
                return false;
            }

            if (isTestSlow)
            {
                int ind = _GamesObjectsReal[nameField].FindIndex(p => p != null && p.name == p_saveObject.name);
                if (ind != -1)
                {
                    Debug.Log("######## AddRealObject is DUBLICATE: " + p_saveObject.name + "   in " + nameField + "    " + callFunc);
                    Storage.Log.GetHistory(p_saveObject.name);
                    return false;
                }
                //@@@@+ Test Dublicate  
                /*
                ModelNPC.GameDataNPC dataObjSave = p_saveObject.GetDataNPC();
                string posFieldOld = Helper.GetNameFieldPosit(dataObjSave.Position.x, dataObjSave.Position.y);
                var indT2 = Storage.Instance.GamesObjectsReal[posFieldOld].FindIndex(p => p != null && p.name == p_saveObject.name);
                if (indT2 != -1)
                {
                    
                    var gobjDbl = Storage.Instance.GamesObjectsReal[posFieldOld][indT2];
                    Debug.Log("############################## find dublicate Real obj --- is NOT ME : " + gobjDbl.name + "      ME: " + p_saveObject.name);
                    ModelNPC.GameDataNPC dataDbl = gobjDbl.GetDataNPC();
                    Storage.EventsUI.ClearListExpandPersons();
                    Storage.EventsUI.AddMenuPerson(dataObjSave as ModelNPC.GameDataNPC, p_saveObject);
                    Storage.EventsUI.AddMenuPerson(dataDbl as ModelNPC.GameDataNPC, gobjDbl);
                    Storage.GamePause = true;
                }
                */
                //@@@@
            }
            _GamesObjectsReal[nameField].Add(p_saveObject);

            Storage.Log.SaveHistory(p_saveObject.name, "AddRealObject", callFunc, nameField);
            return true;
        }catch(Exception x)
        {
            Debug.Log("########### AddRealObject " + nameField + " " + p_saveObject.name + "    "  + x.Message);
        }
        return false;
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

    //async Task TestReader(System.IO.Stream stream)
    //{
    //    XmlReaderSettings settings = new XmlReaderSettings();
    //    settings.Async = true;

    //    using (XmlReader reader = XmlReader.Create(stream, settings))
    //    {
    //        while (await reader.ReadAsync())
    //        {
    //            switch (reader.NodeType)
    //            {
    //                case XmlNodeType.Element:
    //                    Console.WriteLine("Start Element {0}", reader.Name);
    //                    break;
    //                case XmlNodeType.Text:
    //                    Console.WriteLine("Text Node: {0}",
    //                             await reader.GetValueAsync());
    //                    break;
    //                case XmlNodeType.EndElement:
    //                    Console.WriteLine("End Element {0}", reader.Name);
    //                    break;
    //                default:
    //                    Console.WriteLine("Other node {0} with value {1}",
    //                                    reader.NodeType, reader.Value);
    //                    break;
    //            }
    //        }
    //    }
    //}

    Thread threadLoadWorld = null;
    public string DataPathBigPart = Application.dataPath + "/Levels/LevelDataPart1x2.xml";

    public void LoadBigWorldThread(bool isFull = false)
    {
        if (threadLoadWorld != null && threadLoadWorld.IsAlive)
        {
            Debug.Log("threadLoadWorld is run");
            return;
        }

        fieldsD_Temp = new Dictionary<string, ModelNPC.FieldData>();


        if (threadLoadWorld == null)
        {
            threadLoadWorld = new Thread(() =>
            {
                //if(!isFull)
                //    BackgroundLoadDataBigXML();
                //else
                    ThreadLoadDataBigXML();
            });
        }
        threadLoadWorld.Start();
    }

    public bool IsThreadLoadWorldCompleted
    {
        get
        {
            if (threadLoadWorld == null)
                return true;
            if (threadLoadWorld.IsAlive)
                return false;

            return true;
        }
    }

    public void CompletedLoadWorld()
    {
        //v.1
        Storage.Instance.GridDataG.FieldsD = Storage.Instance.GridDataG.FieldsD.Concat(fieldsD_Temp)
                                .ToDictionary(x => x.Key, x => x.Value);

        //return;

        //v.2
        //foreach (var itemObj in fieldsD_Temp)
        //{
        //    if(IsGridDataFieldExist(itemObj.Key))
        //    {
        //        Storage.Instance.GridDataG.FieldsD[itemObj.Key].Objects.AddRange(itemObj.Value.Objects);
        //    }
        //    else
        //    {
        //        Storage.Instance.GridDataG.FieldsD.Add(itemObj.Key, new ModelNPC.FieldData()
        //        {
        //            NameField = itemObj.Key,
        //            Objects = itemObj.Value.Objects
        //        });
        //    }
        //}
        

        Storage.EventsUI.SetTittle = "World is loaded";
        Storage.EventsUI.ListLogAdd = "************** World is loaded **************";
    }

    Dictionary<string, ModelNPC.FieldData> fieldsD_Temp = new Dictionary<string, ModelNPC.FieldData>();

    public Dictionary<string, ModelNPC.FieldData> SetGridDatatBig
    {
        set
        {
            fieldsD_Temp = value;
        }
    }

    public void ThreadLoadDataBigXML()
    {
        string stepErr = "start";

        if (File.Exists(DataPathBigPart))
        {
            var tempGridData = Serializator.LoadGridXml(DataPathBigPart);
            fieldsD_Temp = tempGridData.FieldsD;
        }
    }

    /*
    public void BackgroundLoadDataBigXML()
    {
        string stepErr = "start";
        //Debug.Log("Loaded Xml GridData start...");

        //fieldsD_Temp = new Dictionary<string, ModelNPC.FieldData>();

        string saveField = "";

        if (File.Exists(DataPathBigPart))
        {
            string nameField = "";

            using (XmlReader xml = XmlReader.Create(DataPathBigPart))
            {
                while (xml.Read())
                {
                    switch (xml.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xml.Name == "Key")
                            {
                                XElement el = XElement.ReadFrom(xml) as XElement;
                                nameField = el.Value;
                                
                                //nameField = xml.Value;
                                break;
                            }
                            //if (xml.Name == "Objects")
                            if (xml.Name == "ObjectData") //WWW
                            {
                                XElement el = XElement.ReadFrom(xml) as XElement;
                                string inputString = el.ToString();

                                XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.ObjectData), Serializator.extraTypes);
                                StringReader stringReader = new StringReader(inputString);
                                //--------------

                                ModelNPC.ObjectData dataResult;
                                try
                                {
                                    dataResult = (ModelNPC.ObjectData)serializer.Deserialize(stringReader);
                                }
                                catch (Exception x)
                                {
                                    Debug.Log("############# " + x.Message);
                                    return;
                                }
                                //-------------------------
                                if(saveField==nameField)
                                {
                                    if(_GridDataG.FieldsD.ContainsKey(nameField))
                                    {
                                        fieldsD_Temp[nameField].Objects.Add(dataResult);
                                    }
                                    else
                                    {
                                        fieldsD_Temp.Add(nameField, new ModelNPC.FieldData()
                                        {
                                            NameField = nameField,
                                            Objects = new List<ModelNPC.ObjectData>() { dataResult }
                                        });
                                    }
                                }
                                else
                                {
                                    saveField = nameField;
                                    fieldsD_Temp.Add(nameField, new ModelNPC.FieldData()
                                    {
                                        NameField = nameField,
                                        Objects = new List<ModelNPC.ObjectData>() { dataResult }
                                    });
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
        */
        #endregion

    }
    
