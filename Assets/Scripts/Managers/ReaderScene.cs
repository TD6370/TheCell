using UnityEngine;
//using UnityEditor;

using System.Collections.Generic;
using System.Linq;
using System;

public class ReaderScene
{
    public static bool IsReaderOn = true; //false; //

    public class DataObjectInfoID
    {
        DataObjectInfoID m_dataInfo;

        private string m_Field;
        public string Field {
            get {
                if (string.IsNullOrEmpty(m_Field) && m_Data != null)
                {
                    Helper.GetNameFieldByPosit(ref m_Field, m_Data.Position);
                }
                return m_Field;
            }
            set {
                m_Field = value;
                TestIsValud();
            }
        }

        private int m_IndexField;
        public int IndexField
        {
            get
            {
                return m_IndexField;
            }
            set
            {
                m_IndexField = value;
            }
        }

        private ModelNPC.ObjectData m_Data;
        public ModelNPC.ObjectData Data {
            get {
                //TestIsValud();
                return m_Data;
            }
        }

        public void UpdateData(ModelNPC.ObjectData value, string p_field, int p_index = -1, bool isTestValid = false)
        {
            m_Data = value;
            if(isTestValid)
                TestIsValud();

            m_Field = p_field;

            if (p_index != -1)
                IndexField = p_index;

            if (isTestValid)
                TestIsValud();
        }

        public void StartSetData(ModelNPC.ObjectData value)
        {
            if (m_Data != null)
                Debug.Log(Storage.EventsUI.ListLogAdd = "######## StartSetData m_Data is fill ???");
            m_Data = value;
        }
        
        public GameObject Gobject { get; set; }

        public Vector2 FiledPos
        {
            get
            {
                return Helper.GetPositByField(Field);
            }
        }

        private string _id;
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                {
                    if (m_Data != null)
                    {
                        _id = (m_Data.Id == null) ? Helper.GetID(m_Data.NameObject) : m_Data.Id;
                    }
                }
                return _id;
            }
        }
        private string _ModelView;
        public string ModelView
        {
            get
            {
                if (string.IsNullOrEmpty(_ModelView))
                {
                    if (m_Data != null)
                    {
                        _ModelView = (m_Data.ModelView == null) ? m_Data.TypePrefabName : m_Data.ModelView;
                    }
                }
                return _ModelView;
            }
        }

        public DataObjectInfoID()
        { }

        public bool TestIsValudField()
        {
            if (m_Data == null)
                return false;
            if (string.IsNullOrEmpty(Field))
                return false;
            if (IndexField == -1)
                return false;
            ModelNPC.ObjectData findData = GetObjectDataFromGridContinue(Field, IndexField);
            if (findData == null)
                return false;
            bool test = findData.NameObject == m_Data.NameObject;
            bool result = findData.Id == m_Data.Id;
            return result;
        }

        List<ModelNPC.ObjectData> temp_objsTest;
        public bool TestIsValud()
        {
            if (m_Data == null)
                return false;
            if (string.IsNullOrEmpty(ID))
                return false;

            temp_objsTest = GetObjectsDataFromGridContinue(Field); 
            if (temp_objsTest == null)
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "##### DataObjectInfoID  NOT.TestIsValud: objsTest is null " + m_Data.NameObject);
            }

            int count = temp_objsTest.Where(p => p.NameObject == m_Data.NameObject).Count();
            if(count == 0)
            {
                DataObjectInfoID targetInfo = GetInfoID(ID);
                Debug.Log(Storage.EventsUI.ListLogAdd = "##### DataObjectInfoID NOT.TestIsValud: " + m_Data.NameObject);
            }
            return count > 0;
        }
    }

    public Dictionary<string, DataObjectInfoID> CollectionInfoID;
    public bool IsLoaded = false;

    public ReaderScene()
    {
        InitCollectionID();
    }

    public void Clear()
    {
        if(CollectionInfoID!=null)
            CollectionInfoID.Clear();
        IsLoaded = false;
    }

    public static bool ExistID(string id)
    {
        if (id == null || 
            false == Storage.Instance.ReaderSceneIsValid || 
            Storage.ReaderWorld.CollectionInfoID == null && 
            Storage.ReaderWorld.CollectionInfoID.Count == 0)
                return false;

        return Storage.ReaderWorld.CollectionInfoID.ContainsKey(id);
    }


    public static DataObjectInfoID GetInfoID(string id)
    {
        if (id == null ||
            false == Storage.Instance.ReaderSceneIsValid ||
            Storage.ReaderWorld.CollectionInfoID == null &&
            Storage.ReaderWorld.CollectionInfoID.Count == 0)
            return null;
        DataObjectInfoID result;
        Storage.ReaderWorld.CollectionInfoID.TryGetValue(id, out result);
        return result;
    }
       

    public static List<ModelNPC.ObjectData> GetFieldsByID(string id)
    {
        if (id == null ||
            false == Storage.Instance.ReaderSceneIsValid ||
            Storage.ReaderWorld.CollectionInfoID == null &&
            Storage.ReaderWorld.CollectionInfoID.Count == 0)
            return null;
        DataObjectInfoID result;
        Storage.ReaderWorld.CollectionInfoID.TryGetValue(id, out result);
        if (result != null)
            return GetObjectsDataFromGridTest(result.Field);

        return new List<ModelNPC.ObjectData>();
    }


    public void InitCollectionID()
    {
        string message = "   Init CollectionID...";
        Storage.EventsUI.SetTittle += message;

        string field = string.Empty;
        string newName = string.Empty;
        string id = string.Empty;

        string strOld;
        string strNew;
        bool isExistID = false;
        Single dist;

        ModelNPC.ObjectData objData;
        ModelNPC.ObjectData objDataOld;

        DataObjectInfoID dataInfo;

        //int indDublicae = 0;
        CollectionInfoID = new Dictionary<string, DataObjectInfoID>();
        foreach (var item in Storage.Instance.GridDataG.FieldsD)
        {
            field = item.Value.NameField;
            for(int indDublicae = item.Value.Objects.Count - 1; indDublicae >= 0; indDublicae--)
            {
                objData = item.Value.Objects[indDublicae];
                id = Helper.GetID(objData.NameObject);

                isExistID = true;
                while (isExistID)
                {
                    isExistID = CollectionInfoID.ContainsKey(id);
                    if (isExistID)
                    {
                        objDataOld = CollectionInfoID[id].Data;
                        strOld = objDataOld == null ? " none " : objDataOld.NameObject;
                        strNew = objData == null ? " none " : objData.NameObject;
                        dist = Vector3.Distance(objData.Position, objDataOld.Position);
                        if (dist < 2f)
                        {
                            Debug.Log("##### Error ID : " + id + " old =" + strOld ?? "null " + "  New obj=" + strNew + " [REMOVE DUBLICATE]");
                            Storage.Instance.GridDataG.FieldsD[field].Objects.RemoveAt(indDublicae);
                            id = string.Empty;
                            break;
                        }
                        else
                        {
                            Debug.Log("##### Error ID : " + id + " old =" + strOld ?? "null " + "  New obj=" + strNew + " [FIX ID]");
                            newName = string.Empty;
                            objData.Id = null;
                            Helper.CreateName_Cache(ref newName, objData.TypePrefabName, field, "-1");
                            objData.SetNameObject(newName, true);
                            id = objData.Id;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(id))
                {
                    dataInfo = new DataObjectInfoID()
                    {
                        Gobject = null
                    };
                    dataInfo.UpdateData(objData, field, indDublicae);
                    CollectionInfoID.Add(id, dataInfo);
                }
            }
        }
        IsLoaded = true;
        //if(Storage.EventsUI.SetTittle == message)
        //    Storage.EventsUI.SetTittle = "";
        //string message = "   Init CollectionID...";
        //Storage.EventsUI.SetTittle += message;

    }

    public void UpdateLinkGobject(GameObject newGobject)
    {
        if (newGobject == null)
        {
            //Debug.Log("### UpdateLinkGobject.newGobject is null");
            return;
        }
        string id = Helper.GetID(newGobject.name);
        if (false == CheckCollectionInfoID(id))
            return;
        CollectionInfoID[id].Gobject = newGobject;
    }

    public void UpdateLinkDataFormModel(ModelNPC.ObjectData newData)
    {
        if (false == CheckCollectionInfoID(newData.Id, false))
            return;

        string field = string.Empty;
        Helper.GetNameFieldByPosit(ref field, newData.Position);
        List<ModelNPC.ObjectData> result = null;
        GetObjectsDataFromGridTo(ref result, field);
        int index = -1;
        if(result!=null)
            index = result.FindIndex(p => p.Id == newData.Id || p.NameObject == newData.NameObject);
        if (index == -1)
        {
            CollectionInfoID[newData.Id].UpdateData(newData, string.Empty, -1);
        }
        else
        {
            CollectionInfoID[newData.Id].UpdateData(newData, field, index);
        }
    }

    public void UpdateLinkData(ModelNPC.ObjectData newData, bool isGeneric = false, string field = "", int index = -1)
    {
        if (newData == null || newData.NameObject == null) // || isGeneric)
        {
            //Debug.Log("### UpdateLinkGobject.newData is null");
            return;
        }
        string id = GetDataID(newData);
        if (false == CheckCollectionInfoID(id, isGeneric))
            return;

        if (isGeneric)
            CollectionInfoID[id].StartSetData(newData);
        else
        {
            CollectionInfoID[id].UpdateData(newData, field, index);
        }
    }

  public void UpdateLinkData(ModelNPC.ObjectData newData, string newField, int index)
    {
        if (newData == null || newData.NameObject == null)
        {
            //Debug.Log("### UpdateLinkGobject.newData is null");
            return;
        }
        string id = GetDataID(newData);
        if (false == CheckCollectionInfoID(id, true)) 
            return;

        CollectionInfoID[id].UpdateData(newData, newField, index);
    }

    private string GetDataID(ModelNPC.ObjectData newData)
    {
        string id = Helper.GetID(newData.NameObject);
        var persData = newData as ModelNPC.PersonData;
        return id;
    }

    public void UpdateField(ModelNPC.ObjectData newData, string newField)
    {

        if (newField == null || newData == null || newData.NameObject == null)
        {
            //Debug.Log("### UpdateField.newField is null");
            return;
        }
        string id = GetDataID(newData);
        if (false == CheckCollectionInfoID(id))
            return;

        CollectionInfoID[id].Field = newField;
    }

    public void RemoveObjectInfo(string p_id)
    {
        if (false == CheckCollectionInfoID(p_id))
        {
            Debug.Log("RemoveGobject  CollectionInfo  ID not found = " + p_id);
            return;
        }
        CollectionInfoID.Remove(p_id);
    }

    private bool CheckCollectionInfoID(string id, bool isGeneric = false)
    {
        if (!CollectionInfoID.ContainsKey(id))
        {
            if (isGeneric)
                CollectionInfoID.Add(id, new DataObjectInfoID());
            else
            {
                return false;
            }
        }
        return true;
    }

    public DataObjectInfoID GetInfo(string id)
    {
        if (false == CheckCollectionInfoID(id))
            return new DataObjectInfoID();
        return CollectionInfoID[id];
    }

    public static bool IsGridDataFieldExist(string field)
    {
        return Storage.Instance.GridDataG.FieldsD.ContainsKey(field);
    }

    public static bool IsGridDataFieldFilled(string field)
    {
        if(!Storage.Instance.GridDataG.FieldsD.ContainsKey(field))
            return false;
        return Storage.Instance.GridDataG.FieldsD[field].Objects.Count > 0;
    }

    public static List<ModelNPC.ObjectData> GetObjectsDataFromGridContinue(string nameField)
    {
        if (!IsGridDataFieldExist(nameField))
            return null;

        return Storage.Instance.GridDataG.FieldsD[nameField].Objects;
    }

    public static List<ModelNPC.ObjectData> GetObjectsDataFromGridTest(string nameField)
    {
        //if (!Storage.Instance.IsLoadingWorldThread)
        //{
        if (!IsGridDataFieldExist(nameField))
            Storage.Data.AddNewFieldInGrid(nameField, "GetObjecsDataFromGrid");
        //}
        return Storage.Instance.GridDataG.FieldsD[nameField].Objects;
    }

    public static void GetObjectsDataFromGridTo(ref List<ModelNPC.ObjectData> result, string nameField)
    {

        if (!IsGridDataFieldExist(nameField))
            result = null;
        else
            result = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
    }

    public static bool IsFieldFree(string nameField)
    {
        foreach (ModelNPC.ObjectData dataObjNext in GetObjectsDataFromGrid(nameField))
        {
            if (!dataObjNext.IsFloor() && !dataObjNext.IsFlore())
                return false;
        }
        return true;
    }

    public static List<ModelNPC.ObjectData> GetObjectsDataFromGrid(string nameField)
    {
        return Storage.Instance.GridDataG.FieldsD[nameField].Objects;
    }

    public static ModelNPC.ObjectData GetObjectDataFromGridContinue(string nameField, int index)
    {
        if (!IsGridDataFieldExist(nameField))
            return null;
        if (!IsGridDataFieldFilled(nameField))
            return null;

        return GetObjectsDataFromGrid(nameField)[index];
    }

    public static ModelNPC.ObjectData GetObjectDataFromGrid(string nameField, int index)
    {
        return GetObjectsDataFromGrid(nameField)[index];
    }

    //===============================================================================
    //public static DataInfoFinder GetDataInfoLocation(Vector2Int fieldPosit, int distantion, PriorityFinder priority, string id_Observer, SaveLoadData.TypePrefabs typeObserver, string id_PrevousTarget)
    //public static DataInfoFinder GetDataInfoLocation(Vector2Int fieldPosit, int distantion, string id_Observer, SaveLoadData.TypePrefabs typeObserver, string id_PrevousTarget, bool isFoor)
    public static DataInfoFinder GetDataInfoLocation(int xPos, int yPos, int distantion, string id_Observer, SaveLoadData.TypePrefabs typeObserver, string id_PrevousTarget, bool isFoor)
    {
        DataInfoFinder finder = new DataInfoFinder();
        Dictionary<string, ModelNPC.ObjectData> locationObjects = new Dictionary<string, ModelNPC.ObjectData>();

        int startX = xPos - distantion;
        int startY = yPos - distantion;
        int endX = xPos + distantion;
        int endY = yPos + distantion;

        if (startX < 1)  startX = 1;
        if (startY < 1)  startY = 1;
        string id = "";
        int power = 0;
        int powerDist = 0;

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                string field = Helper.GetNameField(x, y);
                List<ModelNPC.ObjectData> objects = GetObjectsDataFromGridTest(field); //TEST
                foreach (ModelNPC.ObjectData objData in objects)
                {
                    id = Helper.GetID(objData.NameObject);
                    if (id == id_Observer || id == id_PrevousTarget)
                        continue;

                    //int power = Storage.Person.GetPriorityPower(objData, priority);
                    if (isFoor)
                        power = Storage.GenWorld.GetPriorityPowerByJoin(typeObserver, objData.TypePrefab);
                    else
                        power = Storage.Person.GetPriorityPowerByJoin(typeObserver, objData.TypePrefab);

                    //~TEST PRIORITY~
                    //powerDist = (distantion - Math.Max(Math.Abs(xPos - x), Math.Abs(yPos - y))) * 3;
                    //power += powerDist;

                    if (!finder.ResultPowerData.ContainsKey(id))
                    {
                        finder.ResultPowerData.Add(id, power);
                        if(!locationObjects.ContainsKey(id))
                            locationObjects.Add(id, objData);
                    }
                }
            }
        }
      
        string selId = string.Empty;
        int top = 20; // 10; //~TEST PRIORITY~
        List<string> listTopId = new List<string>();
        foreach (var item in finder.ResultPowerData.OrderByDescending(p=>p.Value))
        {
            listTopId.Add(item.Key);
            if (listTopId.Count > top) //top
                break;
        }
        if (listTopId.Count() > 0)
        {
            int indRnd = UnityEngine.Random.Range(0, listTopId.Count() - 1);
            selId = listTopId[indRnd];
        }
        //--------------------------
        if (!string.IsNullOrEmpty(selId))
        {
            finder.ResultData = locationObjects[selId];
        }
        return finder;
    }

    

    public static bool IsUseCashFinderPriorityNPC = false;

    public static DataInfoFinder GetDataInfoLocationFromID(int x, int y, int distantion, SaveLoadData.TypePrefabs typeObserver, string id_Observer)
    {
        int distantionLocalObjects = 50;
        DataInfoFinder finder = new DataInfoFinder();

        //---------------------------- CASH
        if (IsUseCashFinderPriorityNPC)
        {
            var listFindedLocalObjectsID = Storage.ReaderWorld.CollectionInfoID
               .Where(p =>
               {
                   Vector2 posT = p.Value.Data.Position;
                   int distToTatget = Math.Max(Math.Abs(x - (int)posT.x), Math.Abs(y - (int)posT.y));
                   return distToTatget <= distantionLocalObjects;
               })
               .Select(p => p.Key)
               .ToList();
        }
        //-----------------------------------

        var listPowers = Storage.ReaderWorld.CollectionInfoID
            .Where(p => {
                //Vector2 posT = p.Value.Data.Position;
                Vector2 posT = p.Value.FiledPos;
                int distToTatget = Math.Max(Math.Abs(x - (int)posT.x), Math.Abs(y - (int)posT.y));
                return distToTatget <= distantion;
            })
            .Select(p => {
                Vector2 posT = p.Value.FiledPos;
                DataInfoFinder resFinder = new DataInfoFinder()
                {
                    DistantionToTarget = Math.Max(Math.Abs(x - (int)posT.x), Math.Abs(y - (int)posT.y)),
                    ResultData = p.Value.Data,
                    PowerTarget = Storage.Person.GetPriorityPowerByJoin(typeObserver, p.Value.Data.TypePrefab)
            };
                return resFinder;
            })
            .ToList();
        string id;
        int power = 0;
        int powerDist = 0;
        foreach (DataInfoFinder objDataPower in listPowers)
        {
            id = Helper.GetID(objDataPower.ResultData.NameObject);
            //string id = objDataPower.ResultData;
            if (id == id_Observer)
                continue;

            power = objDataPower.PowerTarget;
            powerDist = (distantion - objDataPower.DistantionToTarget) * 3;
            power += powerDist;

            if (finder.ResultPowerData.ContainsKey(id))
                Debug.Log("######### Error finder.ResultPowerData.ContainsKey(id)");
            else
                finder.ResultPowerData.Add(id, power);
        }

        int priorityIndex = 0;
        string selId = string.Empty;
        foreach (var item in finder.ResultPowerData)
        {
            if (priorityIndex < item.Value)
            {
                priorityIndex = item.Value;
                selId = item.Key;
            }
        }
        if (!string.IsNullOrEmpty(selId))
        {
            if (Storage.ReaderWorld.CollectionInfoID.ContainsKey(selId))
                finder.ResultData = Storage.ReaderWorld.CollectionInfoID[selId].Data;
        }


        return finder;
    }

    public class DataInfoFinder
    {
        public Dictionary<string, int> ResultPowerData = new Dictionary<string, int>();
        public ModelNPC.ObjectData ResultData;
        public int DistantionToTarget = 0;
        public int PowerTarget = 0;

        public DataInfoFinder(){}
    }

    //public class PriorityFinder
    //{
    //    public List<SaveLoadData.TypePrefabs> PrioritysTypeModel;
    //    public List<PoolGameObjects.TypePoolPrefabs> PrioritysTypePool;
    //    public List<TypesBiomNPC> PrioritysTypeBiomNPC;

    //    public SaveLoadData.TypePrefabs CurrentTypeModel;
    //    public PoolGameObjects.TypePoolPrefabs CurrentTypePool;
    //    public TypesBiomNPC CurrentTypeNPC;
    //    public PriorityFinder() { }
    //}

}