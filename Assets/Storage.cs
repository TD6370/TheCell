using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

public class Storage : MonoBehaviour {

    const string FieldKey = "Field";

    public GameObject HeroObject;
    public ZonaFieldLook ZonaField { get; set; }
    public ZonaRealLook ZonaReal { get; set; }
    public List<string> KillObject = new List<string>();
    public List<GameObject> DestroyObjectList;

    public static Storage Instance { get; private set; }

    private int _limitHorizontalLook = 22;
    public int LimitHorizontalLook
    {
        get { return _limitHorizontalLook; }
    }
    private int _limitVerticalLook = 18;
    public int LimitVerticalLook
    {
        get { return _limitVerticalLook; }
    }
    
    private int _heroPositionX = 0;
    public int HeroPositionX
    {
        get { return _heroPositionX; }
    }
    private int _heroPositionY = 0;
    public int HeroPositionY
    {
        get { return _heroPositionY; }
    }

    private SaveLoadData.LevelData _personsData;
    public SaveLoadData.LevelData PersonsData
    {
        get { return _personsData; }
    }

    public Dictionary<string, GameObject> Fields;

    //private Dictionary<string, List<SaveLoadData.ObjectData>> _GamesObjectsPersonalData;
    //public Dictionary<string, List<SaveLoadData.ObjectData>> GamesObjectsPersonalData
    //{
    //    get
    //    {
    //        return _GamesObjectsPersonalData;
    //    }
    //}

    private Dictionary<string, List<GameObject>> _GamesObjectsReal;
    public Dictionary<string, List<GameObject>> GamesObjectsReal
    {
        get { return _GamesObjectsReal; }
    }

    private SaveLoadData.GridData _GridDataG;
    public SaveLoadData.GridData GridDataG
    {
        get { return _GridDataG; }
    }

    private string _datapathLevel;
    public string DataPathLevel
    {
        get { return _datapathLevel; }
    }

    private string _datapathPerson;
    public string DataPathPerson
    {
        get { return _datapathPerson; }
    }

    public Camera MainCamera;
    public string SelectGameObjectID="?";

    private SaveLoadData _scriptData;
    private GenerateGridFields _scriptGrid;
    private CompletePlayerController _screiptHero;
    private CreateNPC _scriptNPC;
    private List<HistoryGameObject> _listHistoryGameObject;
    private bool _isSaveHistory = true;

    public void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        ZonaField = null;
        ZonaReal = null;
        
        Fields = new Dictionary<string, GameObject>();
        _GamesObjectsReal = new Dictionary<string, List<GameObject>>();
        _GridDataG = new SaveLoadData.GridData();
        _personsData = new SaveLoadData.LevelData();
        _listHistoryGameObject = new List<HistoryGameObject>();
        DestroyObjectList = new List<GameObject>();
        //_GamesObjectsPersonalData = new Dictionary<string, List<SaveLoadData.ObjectData>>();

        //var camera = MainCamera;
        if (MainCamera == null)
        {
            Debug.Log("MainCamera null");
            return;
        }

        _scriptData = MainCamera.GetComponent<SaveLoadData>();
        if (_scriptData == null)
            Debug.Log("Storage.Start : sctiptData not load !!!");

        _scriptGrid = MainCamera.GetComponent<GenerateGridFields>();
        if (_scriptGrid == null)
            Debug.Log("Storage.Start : scriptGrid not load !!!");

        _scriptNPC = MainCamera.GetComponent<CreateNPC>();
        if (_scriptNPC == null)
        {
            Debug.Log("Storage.Start scriptNPC not load !!!!!");
        }
        _screiptHero = HeroObject.GetComponent<CompletePlayerController>();
        if (_screiptHero == null)
        {
            Debug.Log("Storage.Start scriptHero not load !!!!!");
        }

        //StartGenGrigField();

        LoadData();

        LoadGameObjects();
	}

    void Update()
    {
        DestroyRealObjectInList();
    }
    
    private void LoadGameObjects()
    {
        //GenerateGridFields
        _scriptGrid.StartGenGrigField();

        //SaveLoadData:
        _scriptData.CreateDataGamesObjectsWorld();

        Debug.Log("....Init Position HERO......");
        _screiptHero.FindFieldCurrent();

        _scriptGrid.LoadObjectsNearHero();

        Debug.Log("....Sart Crate NPC......");
        _scriptNPC.SartCrateNPC();
    }
	
	// Update is called once per frame
	
    //public static void SetGridData(SaveLoadData.GridData p_GridData)
    //{
    //    _GridData = p_GridData;
    //}
    //public static void SetGamesObjectsReal(Dictionary<string, List<GameObject>> p_GamesObjectsReal)
    //{
    //    _GamesObjectsReal = p_GamesObjectsReal;
    //}

    private void LoadData()
    {
        //_datapath = Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";
        //_datapath = Application.dataPath + "/SavedData" + Application.loadedLevel + ".xml";
        _datapathLevel = Application.dataPath + "/Levels/LevelData" + Application.loadedLevel + ".xml";
        Debug.Log("# LoadPathData... " + _datapathLevel);

        if (File.Exists(_datapathLevel))
        {
            //@ST@ _gridData = Serializator.LoadGridXml(_datapathLevel);
            _GridDataG = SaveLoadData.Serializator.LoadGridXml(_datapathLevel);
        }
        else
        {
            Debug.Log("# LoadPathData not exist: " + _datapathLevel);
        }

        _datapathPerson = Application.dataPath + "/Levels/PersonData" + Application.loadedLevel + ".xml";
        if (File.Exists(_datapathPerson))
        {
            _personsData = SaveLoadData.Serializator.LoadPersonXml(_datapathPerson);
        }
        else
        {
            Debug.Log("# LoadPathData not exist: " + _datapathPerson);
        }
    }

    public void SetHeroPosition(int x, int y, float xH, float yH)
    {
        //Debug.Log("SetHeroPosition...");

        int scale = 2;
        _heroPositionX = x;
        _heroPositionY = y;

        int _limitX = _limitHorizontalLook / 2;
        int _limitY = _limitVerticalLook / 2;
        {
            int fX = x - _limitX;
            int fY = y - _limitY;
            
            if (fX < 0) fX = 0;
            if (fY < 0) fY = 0;
            int fX2 = x + _limitX;
            int fY2 = y + _limitY;

            ZonaField = new ZonaFieldLook()
            {
                X = fX,
                Y = fY,
                X2 = fX2,
                Y2 = fY2
            };
            //Debug.Log("ZonaField: X:" + ZonaField.X + " Y:" + ZonaField.Y + " X2:" + ZonaField.X2 + " Y2:" + ZonaField.Y2);
        }
        {
            float limitH = _limitHorizontalLook / 2;
            float limitV = _limitVerticalLook / 2;

            float rX = xH - (_limitX * scale);
            float rY = yH + (_limitY * scale);
            float margin = 0.1f;
            if (rX < 0)
            {
                rX = 0.1f;
                limitH -= margin;
            }
            if (rY > 0)
            {
                rY = -0.1f;
                limitV -= margin;
            }
            int LevelX = WidthLevel * scale;
            int LevelY = HeightLevel * scale;

            float rX2 = xH + (limitH * scale);
            float rY2 = yH - (limitV * scale);
            if (rX2 > LevelX) rX2 = LevelX;
            if (rY2 > LevelY) rY2 = LevelY;

            ZonaReal = new ZonaRealLook()
            {
                X = rX,
                Y = rY,
                X2 = rX2,
                Y2 = rY2
            };
            //Debug.Log("ZonaReal: X:" + ZonaReal.X + " Y:" + ZonaReal.Y + " X2:" + ZonaReal.X2 + " Y2:" + ZonaReal.Y2);
            //Draw result
            //DrawRect(rX,rY,rX2,rY2);
        }
    }

    private void DrawRect(float x,float y, float x2, float y2)
    {
        //return;
        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.Log("LineRenderer is null !!!!");
            return;
        }

        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //lineRenderer.SetColors(c1, c2);
        lineRenderer.SetColors(Color.green, Color.green);
        lineRenderer.SetWidth(0.2F, 0.2F);
        int size = 5;
        lineRenderer.SetVertexCount(size);
        
        Vector3 pos1 = new Vector3(x, y, -2);
        lineRenderer.SetPosition(0, pos1);
        Vector3 pos2 = new Vector3(x2, y, -2);
        lineRenderer.SetPosition(1, pos2);
        Vector3 pos3 = new Vector3(x2, y2, -2);
        lineRenderer.SetPosition(2, pos3);
        Vector3 pos4 = new Vector3(x, y2, -2);
        lineRenderer.SetPosition(3, pos4);
        Vector3 pos5 = new Vector3(x, y, -2);
        lineRenderer.SetPosition(4, pos5);
    }

    public Vector2 ValidPiontInZona(ref float x,ref float y, float offset=0)
    {
        offset = Mathf.Abs(offset);

        if (x < ZonaReal.X)
            x = ZonaReal.X + offset;
        if (y > ZonaReal.Y) //*-1
            y = ZonaReal.Y - offset;
        if (x > ZonaReal.X2)
            x = ZonaReal.X2 - offset;
        if (y < ZonaReal.Y2) //*-1
            y = ZonaReal.Y + offset;
        Vector2 result = new Vector2(x, y);
        return result;
    }

    public bool IsValidPiontInZona(float x,float y)
    {
        bool result = true;

        if (x < ZonaReal.X)
            return false;
        if (y > ZonaReal.Y) //*-1
            return false;
        if (x > ZonaReal.X2)
            return false;
        if (y < ZonaReal.Y2) //*-1
            return false;
        return result;
    }



    
    //public static void UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, SaveLoadData.ObjectData objData, Vector3 p_newPosition)
    //public static void UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, SaveLoadData.ObjectData objData)
    //public static string UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, SaveLoadData.ObjectData objData)
    public string UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, SaveLoadData.ObjectData objData, Vector3 p_newPosition, bool isDestroy = false)
    {
        if (_GamesObjectsReal == null || _GamesObjectsReal.Count == 0)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      GamesObjectsReal is EMPTY");
            return "";
        }
        if (_GridDataG == null || _GridDataG.FieldsD == null || _GridDataG.FieldsD.Count == 0)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      GridData is EMPTY");
            return "";
        }

        if(!_GamesObjectsReal.ContainsKey(p_OldField))
        {
            Debug.Log("********** (" + p_NameObject + ") ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition      GamesObjectsReal not found OldField = " + p_OldField);
            return "";
        }
        if (!_GridDataG.FieldsD.ContainsKey(p_OldField))
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition      GridData not found OldField = " + p_OldField);
            return "";
        }

        List<GameObject> realObjectsOldField = _GamesObjectsReal[p_OldField];
        List<SaveLoadData.ObjectData> dataObjectsOldField = _GridDataG.FieldsD[p_OldField].Objects;

        if (realObjectsOldField == null)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition     realObjectsOldField is Null !!!!");
            if (!_GamesObjectsReal.ContainsKey(p_OldField))
            {
                Debug.Log("********** UpdatePosition     in GamesObjectsReal not found OldField = " + p_OldField);
                return "";
            }
            else
            {
                _GamesObjectsReal[p_OldField] = new List<GameObject>();
            }
            return "";
        }

        //#TEST -----
        for (int i = realObjectsOldField.Count - 1; i >= 0;i--)
        {
            if (realObjectsOldField[i] == null)
            {
                Debug.Log("UGP: (" +  p_NameObject + ") ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
                Debug.Log("^^^^ UpfatePosition  -- remove destroy realObjects");
                realObjectsOldField.RemoveAt(i);
            }
        }
        //--------------

        int indReal = realObjectsOldField.FindIndex(p => p.name == p_NameObject);
        if (indReal == -1)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpfatePosition Not Real object (" + p_NameObject + ") in field: " + p_OldField);
            return "";
        }
        int indData = dataObjectsOldField.FindIndex(p => p.NameObject == p_NameObject);
        if (indData == -1)
        {
            //--------------------
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpfatePosition Not DATA object (" + p_NameObject + ") in field: " + p_OldField);
            foreach (var itemObj in dataObjectsOldField)
            {
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- object : " + itemObj.NameObject);
            }
            if (dataObjectsOldField.Count == 0)
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- objects ZERO !!!!!");
            //--------------------
            return "";
        }
        GameObject gobj = realObjectsOldField[indReal];
        if (gobj == null)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      gobj is Destroy");
            return "";
        }

        //add to new Field
        if (!_GridDataG.FieldsD.ContainsKey(p_NewField))
        {
            //#!!!!  Debug.Log("SaveListObjectsToData GridData ADD new FIELD : " + posFieldReal);
            _GridDataG.FieldsD.Add(p_NewField, new SaveLoadData.FieldData());
        }

        if (p_newPosition != gobj.transform.position)
        {
            Debug.Log("********** (" + gobj.name + ")^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** ERROR UpdatePosition 1.  ERROR POSITOIN : GAME OBJ NEW POS: " + p_newPosition + "       REAL OBJ POS: " + gobj.transform.position + "  REAL FIELD: " + Storage.GetNameFieldPosit(gobj.transform.position.x, gobj.transform.position.y));
            return "";
        }

        //Debug.Log("--------------------PRED NAME :" + objDataNow.NameObject);
        objData.NameObject = CreateName(objData.TagObject, p_NewField, "", p_NameObject);
        gobj.name = objData.NameObject;
        //Debug.Log("--------------------POST NAME :" + objDataNow.NameObject);

        //Debug.Log("UpdateGamePosition TEST POSITION GameObj ref: " + p_newPosition + "     GameObj realObjects: " + gobj.transform.position);

        //@POS@ Debug.Log("---- SET POS --- GO:" + gobj.name + "    DO:" + objData.NameObject);
        if (p_newPosition != gobj.transform.position)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** ERROR UpdatePosition 2.   ERROR POSITOIN :  GAME OBJ NEW POS: " + p_newPosition + "       REAL OBJ POS: " + gobj.transform.position);
            return "";
        }
        objData.Position = gobj.transform.position;
        
        if (isDestroy)
            objData.IsReality = false;

        if (!_GamesObjectsReal.ContainsKey(p_NewField))
        {
            _GamesObjectsReal.Add(p_NewField, new List<GameObject>());
        }

        //add
        if (!isDestroy)
            _GamesObjectsReal[p_NewField].Add(gobj);
        _GridDataG.FieldsD[p_NewField].Objects.Add(objData);
        
        //remove
        dataObjectsOldField.RemoveAt(indData);
        realObjectsOldField.RemoveAt(indReal);

        //-------------------
        //int p_x = (int)objData.Position.x;
        //int p_y = (int)objData.Position.y;
        //int x = (int)(p_x / 2);
        //int y = (int)(p_y / 2);
        //string fieldR = FieldKey + (int)x + "x" + Mathf.Abs((int)y);
        //string strX = "  x: " + objData.Position.x + " -> " + p_x + " -> " + (p_x / 2) + " -> " + x;
        //string strY = "  y: " + objData.Position.y + " -> " + p_y + " -> " + (p_y / 2) + " -> " + y;
        //------------------

        //@POS@ Debug.Log("^^^^ UpfatePosition UPDATED...... : go:" + gobj.transform.position + "  DO:" + objData.Position + "  NPC:" + ((SaveLoadData.GameDataNPC)objData).Position + "  UFO:" + ((SaveLoadData.GameDataUfo)objData).Position + "     " + gobj.name);
        //Debug.Log("^^^^ UpfatePosition UPDATED......  >" + fieldR);

        return gobj.name;
    }

    public void AddDestroyRealObject(GameObject gObj)
    {
        DestroyObjectList.Add(gObj);
    }

    private void DestroyRealObjectInList()
    {
        if (DestroyObjectList.Count == 0)
            return;

        for (int i = DestroyObjectList.Count - 1; i >= 0; i--)
        {
            GameObject gObj = DestroyObjectList[i];
            if (gObj != null)
            {
                Debug.Log("*** POOL DESTROY GAME OBJECt INCORRECT : " + gObj.name);
                DestroyRealObject(gObj);
            }
        }
    }

    //@DESTROY@
    public void DestroyRealObject(GameObject gObj)
    {
        if (gObj == null)
            return;


        string nameField = GetNameFieldByName(gObj.name);
        if (nameField == null)
            return;

        List<GameObject> listObjInField = _GamesObjectsReal[nameField];

        for (int i = listObjInField.Count - 1; i >= 0; i--)
        {
            if (listObjInField[i] == null)
            {
                RemoveRealObject(i, nameField, "DestroyRealObject");
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
                RemoveRealObject(indRealData, nameField, "DestroyRealObject");
            }
        }

        Destroy(gObj);

        KillObject.Add(gObj.name);
        //-----------------------------------------------

        //Destrot to Data
        if (!_GridDataG.FieldsD.ContainsKey(nameField))
        {
            Debug.Log("!!!! DestroyRealObject !GridData.FieldsD not field=" + nameField);
            return;
        }
        List<SaveLoadData.ObjectData> dataObjects = _GridDataG.FieldsD[nameField].Objects;
        int indObj = dataObjects.FindIndex(p => p.NameObject == gObj.name);
        if (indObj == -1)
        {
            Debug.Log("!!!! DestroyRealObject GridData not object=" + gObj.name);
            //RemoveAllFindRealObject(gObj.name);
            RemoveAllFindDataObject(gObj.name);
        }
        else
        {
            //@DD@ dataObjects.RemoveAt(indObj);
            RemoveDataObjectInGrid(nameField, indObj, "DestroyRealObject");
        }
    }

    public void RemoveAllFindRealObject(string nameObj)
    {
        string idObj = GetID(nameObj);
        //-----------------------FIXED Correct
        foreach (var item in _GamesObjectsReal)
        {
            string nameField = item.Key;
            List<GameObject> resListData = _GamesObjectsReal[nameField].Where(p => { return p.name.IndexOf(idObj) != -1; }).ToList();
            if (resListData != null)
            {
                for (int i = 0; i < resListData.Count(); i++)
                {
                    var obj = resListData[i];
                    Debug.Log("+++++ CORRECT ++++  DELETE (" + idObj + ") >>>> in Real Object Fields: " + nameField + "     obj=" + obj);
                    Storage.Instance.RemoveDataObjectInGrid(nameField, i, "NextPosition", true);
                }
            }
        }
        //---------------------

    }

    public void RemoveAllFindDataObject(string nameObj)
    {
        string idObj = GetID(nameObj);
        //-----------------------FIXED Correct
        foreach (var item in GridDataG.FieldsD)
        {
            string nameField = item.Key;
            List<SaveLoadData.ObjectData> resListData = Storage.Instance.GridDataG.FieldsD[nameField].Objects.Where(p => { return p.NameObject.IndexOf(idObj) != -1; }).ToList();
            if (resListData != null)
            {
                //foreach (var obj in resListData)
                for (int i = 0; i < resListData.Count(); i++)
                {
                    var obj = resListData[i];
                    //Debug.Log("----------- Exist " + idObj + " in Data Field: " + nameField + " --- " + obj.NameObject);
                    
                    Debug.Log("+++++ CORRECT ++++  DELETE (" + idObj + ") >>>> in DATA Object Fields: " + nameField + "     obj=" + obj);
                    Storage.Instance.RemoveDataObjectInGrid(nameField, i, "NextPosition");
                }
            }
        }

    }

    //----- Data Object
    public void ClearGridData()
    {
        _GridDataG = new SaveLoadData.GridData();
    }

    public void SaveGridGameObjectsXml(bool isNewWorld = false)
    {
        SaveLoadData.Serializator.SaveGridXml(_GridDataG, DataPathLevel, isNewWorld);
    }

    public SaveLoadData.FieldData AddNewFieldInGrid(string newField, string callFunc)
    {
        SaveLoadData.FieldData fieldData = new SaveLoadData.FieldData() { NameField = newField };
        _GridDataG.FieldsD.Add(newField, fieldData);

        //! SaveHistory("", "AddNewFieldInGrid", callFunc, newField);

        return fieldData;
    }

    public void AddDataObjectInGrid(SaveLoadData.ObjectData objDataSave, string nameField, string callFunc)
    {
        SaveLoadData.FieldData fieldData;
        if (!_GridDataG.FieldsD.ContainsKey(nameField))
        {
            fieldData = AddNewFieldInGrid(nameField, callFunc);
        }
        else
        {
            fieldData = _GridDataG.FieldsD[nameField];
        }
        fieldData.Objects.Add(objDataSave);

        SaveHistory(objDataSave.NameObject, "AddDataObjectInGrid", callFunc, nameField, "", null, objDataSave);
    }

    public void RemoveDataObjectInGrid(string nameField, int index, string callFunc, bool isDebug = false)
    {
        SaveLoadData.ObjectData histData = null;
        if (_isSaveHistory)
            histData = _GridDataG.FieldsD[nameField].Objects[index];
        if(isDebug)
            Debug.Log("****** RemoveDataObjectInGrid : " + histData);

        _GridDataG.FieldsD[nameField].Objects.RemoveAt(index);

        if (_isSaveHistory)
        {
            if (histData == null)
            {
                Debug.Log("##################### Error RemoveDataObjectInGrid save SaveHistory  histData == null ");
                return;
            }
            SaveHistory(histData.NameObject, "RemoveDataObjectInGrid", callFunc, nameField, "", histData);
        }
    }

    public void UpdateDataObect(string nameField, int index, SaveLoadData.ObjectData setObject, string callFunc)
    {
        if (_isSaveHistory)
        {
            SaveLoadData.ObjectData oldObj = _GridDataG.FieldsD[nameField].Objects[index];
            SaveHistory(setObject.NameObject, "UpdateDataObect", callFunc, nameField, "", oldObj, setObject);
            SaveHistory(oldObj.NameObject, "UpdateDataObect", callFunc, nameField, "RESAVE", oldObj, setObject);
        }

        //List<SaveLoadData.ObjectData> dataObjects = _gridData.FieldsD[p_nameField].Objects;
        _GridDataG.FieldsD[nameField].Objects[index] = setObject;
    }
    //----- Real Object

    public List<GameObject> AddNewFieldInRealObject(string newField, string callFunc)
    {
        List<GameObject> listRealObjects = new List<GameObject>();

        _GamesObjectsReal.Add(newField, listRealObjects);

        SaveHistory("", "AddNewFieldInRealObject", callFunc, newField);

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

        SaveHistory(p_saveObject.name, "AddRealObject", callFunc, nameField);
    }

    public void RemoveRealObject(int indexDel, string nameField, string callFunc)
    {
        //List<GameObject> objects = _GamesObjectsReal[nameField];
        if (!_GamesObjectsReal.ContainsKey(nameField))
        {
            Debug.Log("################ RemoveRealObject    Not Filed: " + nameField);
            return;
        }
        if (_GamesObjectsReal[nameField].Count-1 < indexDel)
        {
            Debug.Log("################ RemoveRealObject  " + nameField + "  Not indexDel: " + indexDel);
            return;
        }
        if(_GamesObjectsReal[nameField][indexDel]==null)
            SaveHistory("Destroy real obj", "RemoveRealObject", callFunc, nameField);
        else
            SaveHistory(_GamesObjectsReal[nameField][indexDel].name, "RemoveRealObject", callFunc, nameField);
        
        _GamesObjectsReal[nameField].RemoveAt(indexDel);
    }

 #region Log

    public void DebugKill(string findObj)
    {
        if (!_isSaveHistory)
            return;


        string id = GetID(findObj);
        
        var res = KillObject.Find(p => p == findObj);
        if (res != null)
            Debug.Log("FIND KILLED : " + findObj);
        else
        {
            var res2 = KillObject.Find(p => { return p.IndexOf(id) != -1; });
            if (res2 != null)
                Debug.Log("FIND KILLED : " + findObj + "    -- " + res2);
        }

        //{
        //    Debug.Log("All killed --------------------------------:");
        //    foreach (var obj in KillObject)
        //    {
        //        Debug.Log("killed --------------------------------:" + obj);
        //    }
        //}
    }

    //--------------- History

    public void GetHistory(string nameObj)
    {
        Debug.Log("******** History (" + _listHistoryGameObject.Count + ") --------------------------------------------FIND: " + nameObj);
        var resList = _listHistoryGameObject.Where(p => p.Name == nameObj || p.Name=="").OrderBy(p => p.TimeSave);
        int i=0;
        foreach (var obj in resList)
        {
            i++;
            Debug.Log(i + ". " + obj.ToString());
        }
        if(resList == null || resList.Count() == 0)
        {
            string id = GetID(nameObj);
            Debug.Log("--- Find hyst: " + id + " ---------------------");

            //resList = _listHistoryGameObject.Where(p => p.Name.StartsWith(id)).OrderBy(p => p.TimeSave);
            resList = _listHistoryGameObject.Where(p => { return p.Name.IndexOf(id)!=-1; }).OrderBy(p => p.TimeSave);
            foreach (var obj in resList)
            {
                i++;
                Debug.Log(i + ". " + obj.ToString());
            }
            if (resList == null || resList.Count() == 0)
            {
                //var listKey = GridDataG.FieldsD.Select(p => p.Key).ToList();
                foreach (var item in GridDataG.FieldsD)
                {
                    string nameField = item.Key;
                    var resListData = GridDataG.FieldsD[nameField].Objects.Where(p => { return p.NameObject.IndexOf(id) != -1; });
                    if(resListData!=null)
                    {
                        foreach (var obj in resListData)
                        {
                            Debug.Log("Exist " + id + " in Data Field: " + nameField + " --- " + obj.NameObject);
                        }
                    }
                }
            }
        }

        DebugKill(nameObj);

        Debug.Log("*******************************************************************************************"); 
    }

    public void SaveHistory(string name, string actionName, string callFunc, string field = "", string comment = "", SaveLoadData.ObjectData oldDataObj = null, SaveLoadData.ObjectData newDataObj = null)
    {
        if (!_isSaveHistory)
            return;

        _listHistoryGameObject.Add(new HistoryGameObject()
        {
            Name = name,
            ActionName = actionName,
            CallFunc = callFunc,
            Comment = comment, 
            Field = field, 
            NewDataObj = newDataObj, 
            OldDataObj = oldDataObj, 
            TimeSave = DateTime.Now
        });
    }
 #endregion

    #region Helper



    public static string CreateName(string tag, string nameFiled, string id = "", string nameObjOld = "")
    {
        if (string.IsNullOrEmpty(id))
        {
            if (string.IsNullOrEmpty(nameObjOld))
            {
                Debug.Log("!!!!!! Error create name !!!!!!!!!!");
            }
            else
            {

                int i = nameObjOld.LastIndexOf("_");
                //int i2 = nameObjOld.IndexOf("_");
                if (i != -1)
                {
                    //123456789
                    //Debug.Log("_______________________CREATE NAME i_l=" + i + "     i=" + i2 + "     len=" + nameObjOld.Length + "      :" + nameObjOld);
                    id = nameObjOld.Substring(i + 1, nameObjOld.Length - i - 1);
                    //id = nameObjOld.Substring(nameObjOld.Length - i, i);
                    //Debug.Log("_______________________CREATE NAME  ID:" + id + "       nameObjOld: " + nameObjOld);
                }
                else
                    Debug.Log("!!!!!! Error create name prefix !!!!!!!!!!");
            }
        }
        else
        {
            //Debug.Log("__CreateName old id=" + id);
        }

        if (id == "-1")
        {
            id = Guid.NewGuid().ToString().Substring(1, 4);
        }

        return tag + "_" + nameFiled + "_" + id;
    }

    public static string GetGameObjectID(GameObject gobj)
    {
        string nameObj = gobj.name;
        return GetID(nameObj);
    }

    public static string GetID(string nameObj)
    {
        string id = "";
        int i = nameObj.LastIndexOf("_");
        //int i2 = nameObjOld.IndexOf("_");
        if (i != -1)
        {
            //123456789
            //Debug.Log("_______________________CREATE NAME i_l=" + i + "     i=" + i2 + "     len=" + nameObjOld.Length + "      :" + nameObjOld);
            id = nameObj.Substring(i + 1, nameObj.Length - i - 1);
            //Debug.Log("_______________________GetGameObjectID  ID:" + id);
        }
        else
            Debug.Log("!!!!!! GetGameObjectID Error create name prefix !!!!!!!!!!");

        return id;
    }

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

    public static string GetNameFieldByName(string nameGameObject)
    {

        int start = nameGameObject.IndexOf(FieldKey);
        string result = "";
        string resultInfo = "";
        int valid = 0;
        if (start == -1)
        {
            Debug.Log("# GetNameFieldByName " + nameGameObject + " key 'Field' not found!");
            //return nameGameObject;
            return null;
        }
        start += "Field".Length;

        //int i = nameGameObject.IndexOf("Field");
        for (int i = start; i < nameGameObject.Length - 1; i++)
        {
            var symb = nameGameObject[i];
            resultInfo += symb;
            if (symb == 'x')
            {
                result += symb.ToString();
                continue;
            }
            if (Int32.TryParse(symb.ToString(), out valid))
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

    public static Vector3 ConvVector3(Vector3 copyPos)
    {
        return new Vector3(copyPos.x, copyPos.y, copyPos.z);
    }

    public static bool IsDataInit(GameObject gameObject)
    {
        int start = gameObject.name.IndexOf(FieldKey);
        if (start == -1)
        {
            //Debug.Log("# IsDataInit " + gameObject.name + " key 'Field' not found!");
            //yield return null;
            return false;
        }
        return true;
    }

   

    public static int WidthLevel
    {
        get { return 100; }
    }
    public static int HeightLevel
    {
        get { return 100; }
    }
    public static int WidthWorld
    {
        get { return 1000; }
    }
    public static int HeightWorld
    {
        get { return 1000; }
    }

#endregion

#region class

    public class ZonaFieldLook
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public ZonaFieldLook() { }
    }

    public class ZonaRealLook
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public ZonaRealLook() { }
    }

    public class HistoryGameObject
    {
        public string Name { get; set; }
        public string Field { get; set; }
        public string ActionName { get; set; }
        public string CallFunc { get; set; }
        public string Comment { get; set; }
        public DateTime TimeSave { get; set; }
        public SaveLoadData.ObjectData OldDataObj { get; set; }
        public SaveLoadData.ObjectData NewDataObj { get; set; }

        public HistoryGameObject() {}

        public override string ToString()
        {
            string oldData = (OldDataObj==null) ? "" : " [" + OldDataObj.ToString() + "]";
            string newData = (NewDataObj == null) ? "" : " [" + NewDataObj.ToString() + "]";

            //return TimeSave + " " + ActionName + " " +  Name + "  " + OldDataObj + NewDataObj + "  >>" + CallFunc + "  " + Field  + "  '" + Comment + "'";
            return ActionName + " " + Name + "  " + oldData + newData + "  >>" + CallFunc + "  " + Field + "  '" + Comment + "'";
            //return base.ToString();
        }
    }

#endregion

}
