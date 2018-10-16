using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//public class StorageLog : MonoBehaviour {
public class StorageLog //: MonoBehaviour
{
    public StorageLog()
    {

    }


    //private static StorageLog _instance;
    //public static StorageLog Instance
    //{
    //    get
    //    {
    //        if(_instance==null)
    //        {
    //            _instance = new StorageLog();
    //        }
    //        return _instance;
    //    }
    //}

    private bool _isSaveHistory = true;
    public bool IsSaveHistory { get { return _isSaveHistory; } }


    private List<HistoryGameObject> _listHistoryGameObject;
    public List<HistoryGameObject> ListHistoryGameObject {
        get{
            return _listHistoryGameObject;
        }
        //set
        //{
        //    _listHistoryGameObject = value;
        //}
    }

    // Use this for initialization
 //   void Start()
 //   {

 //   }

 //   //public static Storage Instance { get; private set; }
 //   public void Awake()
 //   {
 //       //Instance = this;
 //   }

 //   // Update is called once per frame
 //   void Update () {
		
	//}

    public void Init()
    {
        _listHistoryGameObject = new List<HistoryGameObject>();
    }

    #region Log

    public void DebugKill(string findObj)
    {
        if (!_isSaveHistory)
            return;


        string id = Helper.GetID(findObj);

        var res = Storage.Instance.KillObject.Find(p => p == findObj);
        if (res != null)
            Debug.Log("FIND KILLED : " + findObj);
        else
        {
            var res2 = Storage.Instance.KillObject.Find(p => { return p.IndexOf(id) != -1; });
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

    public string GetHistory(string nameObj)
    {
        string findName = "";

        Debug.Log("******** History (" + _listHistoryGameObject.Count + ") --------------------------------------------FIND: " + nameObj);
        var resList = _listHistoryGameObject.Where(p => p.Name == nameObj || p.Name == "").OrderBy(p => p.TimeSave);
        int i = 0;
        foreach (var obj in resList)
        {
            i++;
            Debug.Log(i + ". " + obj.ToString());
        }
        string id1 = Helper.GetID(nameObj);
        var resList1 = _listHistoryGameObject.Where(p => { return p.Name.IndexOf(id1) != -1; }).OrderBy(p => p.TimeSave);
        int i1 = 0;
        foreach (var obj in resList1)
        {
            i1++;
            Debug.Log(i1 + ". " + obj.ToString());
        }

        if (resList == null || resList.Count() == 0)
        {
            string id = Helper.GetID(nameObj);
            Debug.Log("--- Find hyst: " + id + " ---------------------");

            //resList = _listHistoryGameObject.Where(p => p.Name.StartsWith(id)).OrderBy(p => p.TimeSave);
            resList = _listHistoryGameObject.Where(p => { return p.Name.IndexOf(id) != -1; }).OrderBy(p => p.TimeSave);
            foreach (var obj in resList)
            {
                i++;
                Debug.Log(i + ". " + obj.ToString());
            }
            if (resList == null || resList.Count() == 0)
            {
                //var listKey = GridDataG.FieldsD.Select(p => p.Key).ToList();
                foreach (var item in Storage.Instance.GridDataG.FieldsD)
                {
                    string nameField = item.Key;
                    var resListData = Storage.Instance.GridDataG.FieldsD[nameField].Objects.Where(p => { return p.NameObject.IndexOf(id) != -1; });
                    if (resListData != null)
                    {
                        foreach (var obj in resListData)
                        {
                            Debug.Log("Exist " + id + " in Data Field: " + nameField + " --- " + obj.NameObject);
                            findName = obj.NameObject;
                        }
                    }
                }
            }
        }

        DebugKill(nameObj);

        Debug.Log("*******************************************************************************************");

        return findName;
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

        public HistoryGameObject() { }

        public override string ToString()
        {
            string oldData = (OldDataObj == null) ? "" : " [" + OldDataObj.ToString() + "]";
            string newData = (NewDataObj == null) ? "" : " [" + NewDataObj.ToString() + "]";

            //return TimeSave + " " + ActionName + " " +  Name + "  " + OldDataObj + NewDataObj + "  >>" + CallFunc + "  " + Field  + "  '" + Comment + "'";
            return ActionName + " " + Name + "  " + oldData + newData + "  >>" + CallFunc + "  " + Field + "  '" + Comment + "'";
            //return base.ToString();
        }
    }
}
