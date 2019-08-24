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
    }

    public void InitCollectionID()
    {
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
                    continue;
                }

                CollectionInfoID.Add(id, dataInfo);
            }
        }
        IsLoaded = true;
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
        if (newData == null)
        {
            //Debug.Log("### UpdateLinkGobject.newData is null");
            return;
        }
        string id = GetDataID(newData);
        if (false == CheckCollectionInfoID(id))
            return;
        CollectionInfoID[id].Data = newData;
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

        if (newField == null)
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
        //if (!Storage.Instance.IsLoadingWorldThread)
        //{
        //if (!IsGridDataFieldExist(nameField))
        //    Storage.Data.AddNewFieldInGrid(nameField, "GetObjecsDataFromGrid");
        //}
        return Storage.Instance.GridDataG.FieldsD[nameField].Objects;
    }

    public static ModelNPC.ObjectData GetObjectDataFromGrid(string nameField, int index)
    {
        //if (!Storage.Instance.IsLoadingWorldThread)
        //{
        //if (!IsGridDataFieldExist(nameField))
        //    Storage.Data.AddNewFieldInGrid(nameField, "GetObjecDataFromGrid");
        //}
        return GetObjectsDataFromGrid(nameField)[index];
    }

    public static ModelNPC.ObjectData FindFromLocation(Vector2Int fieldPosit, int distantion, PriorityFinder prioritys)
    {
        DataInfoFinder finder = GetDataInfoLocation(fieldPosit, distantion,  prioritys);
        return finder.ResultData;
    }

    public static DataInfoFinder GetDataInfoLocation(Vector2Int fieldPosit, int distantion, PriorityFinder priority)
    {
        DataInfoFinder finder = new DataInfoFinder();
        int startX = fieldPosit.x - distantion;
        int startY = fieldPosit.y - distantion;
        int endX = fieldPosit.x + distantion;
        int endY = fieldPosit.y + distantion;
        

        if (startX < 1)  startX = 1;
        if (startY < 1)  startY = 1;

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                string field = Helper.GetNameField(x, y);
                List<ModelNPC.ObjectData> objects = GetObjectsDataFromGrid(field);
                foreach (ModelNPC.ObjectData objData in objects)
                {
                    string id = Helper.GetID(objData.NameObject);
                    //finder.ListObjData.Add(id, objData);
                   
                    int power = GetPriorityPower(objData, priority);
                    int powerDist = (distantion - Math.Max(Math.Abs(fieldPosit.x - x), Math.Abs(fieldPosit.y - y))) * 3;

                    if (finder.ResultPowerData.ContainsKey(id))
                        Debug.Log("######### Error finder.ResultPowerData.ContainsKey(id)");
                        //finder.ResultPowerData[id] += power;
                    else
                        finder.ResultPowerData.Add(id, power);
                }
            }
        }

        //foreach (var item in finder.ListObjData)
        //{
        //    var objData = item.Value;
        //    string id = item.Key;
        //    int power = GetPriorityPower(objData, priority);
        //    if (finder.ResultPowerData.ContainsKey(id))
        //        Debug.Log("######### Error finder.ResultPowerData.ContainsKey(id)");
        //        //finder.ResultPowerData[id] += power;
        //    else
        //        finder.ResultPowerData.Add(id, power);
        //}

        //return Storage.Instance.GridDataG.FieldsD.
        //    Select(x => x.Value).
        //    SelectMany(x => x.Objects).
        //    .ToList();

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
        if (string.IsNullOrEmpty(selId))
        {
            finder.ResultData = finder.ListObjData[selId];
        }
        return finder;
    }

    public static int GetPriorityPower(string id, PriorityFinder priority)
    {
        if (Storage.ReaderWorld.CollectionInfoID.ContainsKey(id))
        {
            Debug.Log("######### Error GetPrioriryPower=" + id);
        }

        int power = 0;

        var objData = Storage.ReaderWorld.CollectionInfoID[id].Data;
        return GetPriorityPower(objData, priority);
    }

    //public static int GetPrioriryPower(DataInfoFinder finder, string id, PriorityPerson priority)
    public static int GetPriorityPower(ModelNPC.ObjectData objData, PriorityFinder priority)
    {
        int power = 0;
        SaveLoadData.TypePrefabs typeModel = objData.TypePrefab;
        PoolGameObjects.TypePoolPrefabs typePool = objData.TypePoolPrefab;
        TypesBiomNPC biomNPC = GetBiomByTypeModel(typeModel);

        int slotPower = 3;
        int maxtPrioprity = 10;
        maxtPrioprity = priority.GetPrioritysTypeModel().Count() * slotPower;
        foreach (SaveLoadData.TypePrefabs itemModel in priority.GetPrioritysTypeModel())
        {
            if (itemModel == typeModel)
            {
                power += maxtPrioprity;
                break;
            }
            maxtPrioprity -= slotPower;
        }
        maxtPrioprity = priority.PrioritysTypeBiomNPC.Count() * slotPower;
        foreach (TypesBiomNPC itemBiom in priority.PrioritysTypeBiomNPC)
        {
            if (itemBiom == biomNPC)
            {
                power += maxtPrioprity;
                break;
            }
            maxtPrioprity -= slotPower;
        }
        maxtPrioprity = priority.PrioritysTypeBiomNPC.Count() * slotPower;
        foreach (PoolGameObjects.TypePoolPrefabs itemPool in priority.PrioritysTypePool)
        {
            if (itemPool == typePool)
            {
                power += maxtPrioprity;
                break;
            }
            maxtPrioprity -= slotPower;
        }

        return power;
    }
    //public List<PriorityPerson> GetPrioritys()
    //{
    //    return new List<PriorityPerson>();
    //}

    public static TypesBiomNPC GetBiomByTypeModel(SaveLoadData.TypePrefabs typeModel)
    {
        TypesBiomNPC resType = TypesBiomNPC.None;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomBlue), typeModel.ToString()))
            return TypesBiomNPC.Blue;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomGreen), typeModel.ToString()))
            return TypesBiomNPC.Green;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomRed), typeModel.ToString()))
            return TypesBiomNPC.Red;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomViolet), typeModel.ToString()))
            return TypesBiomNPC.Violet;

        return resType;
    }

    public class DataInfoFinder
    {
        //public Queue<string> StackAll = new Queue<string>();
        //public Dictionary<string, ModelNPC.ObjectData> listData = new Dictionary<string, ModelNPC.ObjectData>();
        //public List<ModelNPC.ObjectData> listAll = new List<ModelNPC.ObjectData>();

        //public Dictionary<TypesBiomNPC, Dictionary<string, string>> ListBiomNPC = new Dictionary<TypesBiomNPC, Dictionary<string, string>>();
        //public Dictionary<PoolGameObjects.TypePoolPrefabs, Dictionary<string, string>> ListTypesPrefabs = new Dictionary<PoolGameObjects.TypePoolPrefabs, Dictionary<string, string>>();
        //public Dictionary<SaveLoadData.TypePrefabs, Dictionary<string, string>> ListTypesModels = new Dictionary<SaveLoadData.TypePrefabs, Dictionary<string, string>>();

        //public List<string> ListAll = new List<string>();
        public Dictionary<string, int> ResultPowerData = new Dictionary<string, int>();
        public Dictionary<string, ModelNPC.ObjectData> ListObjData = new Dictionary<string, ModelNPC.ObjectData>();
        public ModelNPC.ObjectData ResultData;

        public DataInfoFinder()
        {
        }
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