using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

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

    private void InitCollectionID()
    {
        string field = string.Empty;
        CollectionInfoID = new Dictionary<string, DataObjectInfoID>();
        foreach (var item in Storage.Instance.GridDataG.FieldsD)
        {
            field = item.Value.NameField;
            foreach(ModelNPC.ObjectData objData in item.Value.Objects)
            {
                string id = Helper.GetID(objData.NameObject);
                DataObjectInfoID dataInfo = new DataObjectInfoID()
                {
                    Field = field,
                    Data = objData,
                    Gobject = null
                };
                if(CollectionInfoID.ContainsKey(id))
                {
                    var oldObj = CollectionInfoID[id];
                    string strOld = oldObj == null ? " none " : oldObj.Data.NameObject;
                    string strNew = dataInfo == null ? " none " : dataInfo.Data.NameObject;
                    Debug.Log("##### Error ID : " + id + " old =" + strOld ?? "null " + "  New obj=" + strNew);
                    continue;
                }

                CollectionInfoID.Add(id, dataInfo);
            }
        }
        IsLoaded = true;
    }

    public void UpdateLinkGobject( GameObject newGobject)
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
        if (persData != null)
            id = persData.Id;
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
        CollectionInfoID[id].Field = newField;
    }

  
    private bool CheckCollectionInfoID(string id)
    {
        if(!CollectionInfoID.ContainsKey(id))
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

    public static List<ModelNPC.ObjectData> GetObjecsDataFromGrid(string nameField)
    {
        //if (!Storage.Instance.IsLoadingWorldThread)
        //{
            //if (!IsGridDataFieldExist(nameField))
            //    Storage.Data.AddNewFieldInGrid(nameField, "GetObjecsDataFromGrid");
        //}
        return Storage.Instance.GridDataG.FieldsD[nameField].Objects;
    }

    public static ModelNPC.ObjectData GetObjecDataFromGrid(string nameField, int index)
    {
        //if (!Storage.Instance.IsLoadingWorldThread)
        //{
            //if (!IsGridDataFieldExist(nameField))
            //    Storage.Data.AddNewFieldInGrid(nameField, "GetObjecDataFromGrid");
        //}
        return GetObjecsDataFromGrid(nameField)[index];
    }

    
}