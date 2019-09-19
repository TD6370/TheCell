using UnityEngine;
//using UnityEditor;

using System.Collections.Generic;
using System.Linq;
using System;

public class ReaderScene //: UpdateData
{
    public static bool IsReaderOn = true; //false; //

    //: UpdateData.
    //[MenuItem("Tools/MyTool/Do It in C#")]
    //static void DoIt()
    //{
    //    EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    //}

    public class DataObjectInfoID
    {
        DataObjectInfoID m_dataInfo;
        public string Field { get; set; }
        public ModelNPC.ObjectData Data { get; set; }
        public GameObject Gobject { get; set; }

        public Vector2 FiledPos
        {
            get
            {
                return Helper.GetPositByField(Field);
            }
        }

        //public float TimeStartWork { get; set; }
        //public float TimeStartDreamWork { get; set; }

        private string _id;
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                {
                    if (Data != null)
                    {
                        _id = (Data.Id == null) ? Helper.GetID(Data.NameObject) : Data.Id;
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
                    if (Data != null)
                    {
                        _ModelView = (Data.ModelView == null) ? Data.TypePrefabName : Data.ModelView;
                    }
                }
                return _ModelView;
            }
        }

        public DataObjectInfoID()
        { }
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

    public void InitCollectionID()
    {
        string message = "   Init CollectionID...";
        Storage.EventsUI.SetTittle += message;

        string field = string.Empty;
        CollectionInfoID = new Dictionary<string, DataObjectInfoID>();
        foreach (var item in Storage.Instance.GridDataG.FieldsD)
        {
            field = item.Value.NameField;
            foreach (ModelNPC.ObjectData objData in item.Value.Objects)
            {
                string id = Helper.GetID(objData.NameObject);
                DataObjectInfoID dataInfo = new DataObjectInfoID()
                {
                    Field = field,
                    Data = objData,
                    Gobject = null
                };
                if (CollectionInfoID.ContainsKey(id))
                {
                    var oldObj = CollectionInfoID[id];
                    string strOld = oldObj == null ? " none " : oldObj.Data.NameObject;
                    string strNew = (dataInfo == null || dataInfo.Data == null) ? " none " : dataInfo.Data.NameObject;
                    Debug.Log("##### Error ID : " + id + " old =" + strOld ?? "null " + "  New obj=" + strNew);
                    CollectionInfoID.Remove(id);
                    //continue;
                }

                CollectionInfoID.Add(id, dataInfo);
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

    public void UpdateLinkData(ModelNPC.ObjectData newData)
    {
        if (newData == null || newData.NameObject == null)
        {
            //Debug.Log("### UpdateLinkGobject.newData is null");
            return;
        }
        string id = GetDataID(newData);
        if (false == CheckCollectionInfoID(id))
            return;

        //newData = UpdateFix(newData);
        CollectionInfoID[id].Data = newData;
        
        //TEST
        //UpdateFix(id, newData);
    }

    private void UpdateFix(string id, ModelNPC.ObjectData newData)
    {
        //TEST
        //if (newData.ModelView != null)
            //CollectionInfoID[id].Data.ModelView = newData.ModelView;
        //Debug.Log(Storage.EventsUI.ListLogAdd = "#### UpdateFix newData.ModelView is Null >> " + newData.NameObject);
        //if (newData.Id != null)
        //    CollectionInfoID[id].Data.Id = newData.Id;
        //else
        //    CollectionInfoID[id].Data.CreateID();
        //Debug.Log(Storage.EventsUI.ListLogAdd = "#### UpdateFix newData.ID is Null >> " + newData.NameObject);
    }


    private string GetDataID(ModelNPC.ObjectData newData)
    {
        string id = Helper.GetID(newData.NameObject);
        var persData = newData as ModelNPC.PersonData;
        //if (persData != null)
        //    id = persData.Id;
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

        //TEST
        var t1 = CollectionInfoID[id].Data;
        var t2 = CollectionInfoID[id].Gobject;

        CollectionInfoID[id].Field = newField;
    }

    public void RemoveGobject(string p_id)
    {
        if (false == CheckCollectionInfoID(p_id))
        {
            Debug.Log("RemoveGobject  CollectionInfo  ID not found = " + p_id);
            return;
        }
        CollectionInfoID.Remove(p_id);
    }


    private bool CheckCollectionInfoID(string id)
    {
        if (!CollectionInfoID.ContainsKey(id))
        {
            //Debug.Log("############ NEW ID");
            //CollectionInfoID.Add(id, new DataObjectInfoID());
            CollectionInfoID.Add(id, new DataObjectInfoID());
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
        //bool isFindReaderWorld = Storage.ReaderWorld.CollectionInfoID.Count > 0;
        //bool isFindReaderWorld = false;

        //if (!Storage.Instance.ReaderSceneIsValid)
        //    return finder;

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
                    powerDist = (distantion - Math.Max(Math.Abs(xPos - x), Math.Abs(yPos - y))) * 3;
                    power += powerDist;
                    //if (finder.ResultPowerData.ContainsKey(id))
                    //Debug.Log("######### Error finder.ResultPowerData.ContainsKey(id)");
                    //else
                    if (!finder.ResultPowerData.ContainsKey(id))
                    {
                        finder.ResultPowerData.Add(id, power);
                        //if (!isFindReaderWorld)
                        //{
                            if(!locationObjects.ContainsKey(id))
                                locationObjects.Add(id, objData);
                        //}
                    }
                }
            }
        }
      
        //int priorityIndex = 0;
        string selId = string.Empty;
        //------------------------
        //foreach (var item in finder.ResultPowerData)
        //{
        //    if (priorityIndex < item.Value) 
        //    {
                
        //        priorityIndex = item.Value; //only star
        //        selId = item.Key;

        //    }
        //}
        //------------------------
        int top = 10;
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
            //if (isFindReaderWorld)
            //{
            //    if (Storage.ReaderWorld.CollectionInfoID.ContainsKey(selId))
            //        finder.ResultData = Storage.ReaderWorld.CollectionInfoID[selId].Data;
            //}else
            //{
                finder.ResultData = locationObjects[selId];
            //}
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

    


   


    //public static int GetPrioriryPower(DataInfoFinder finder, string id, PriorityPerson priority)

    //public List<PriorityPerson> GetPrioritys()
    //{
    //    return new List<PriorityPerson>();
    //}

    public class DataInfoFinder
    {
        //public Queue<string> StackAll = new Queue<string>();
        //public Dictionary<string, ModelNPC.ObjectData> listData = new Dictionary<string, ModelNPC.ObjectData>();
        //public List<ModelNPC.ObjectData> listAll = new List<ModelNPC.ObjectData>();

        //public Dictionary<TypesBiomNPC, Dictionary<string, string>> ListBiomNPC = new Dictionary<TypesBiomNPC, Dictionary<string, string>>();
        //public Dictionary<PoolGameObjects.TypePoolPrefabs, Dictionary<string, string>> ListTypesPrefabs = new Dictionary<PoolGameObjects.TypePoolPrefabs, Dictionary<string, string>>();
        //public Dictionary<SaveLoadData.TypePrefabs, Dictionary<string, string>> ListTypesModels = new Dictionary<SaveLoadData.TypePrefabs, Dictionary<string, string>>();

        //public List<string> ListAll = new List<string>();
        //public Dictionary<string, ModelNPC.ObjectData> ListObjData = new Dictionary<string, ModelNPC.ObjectData>();

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