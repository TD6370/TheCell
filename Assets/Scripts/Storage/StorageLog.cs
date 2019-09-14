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

        var res = Storage.Instance.KillObjectHistory.Find(p => p == findObj);
        if (res != null)
            Debug.Log("FIND KILLED : " + findObj);
        else
        {
            var res2 = Storage.Instance.KillObjectHistory.Find(p => { return p.IndexOf(id) != -1; });
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
        int i1 = 0;
        foreach (var obj in resList)
        {
            i1++;
            Debug.Log(i1 + ". " + obj.ToString());
        }
        string id = Helper.GetID(nameObj);
        var resListById = _listHistoryGameObject.Where(p => { return p.Name.IndexOf(id) != -1; }).OrderBy(p => p.TimeSave);
        if (resListById!=null && resListById.Count() > 0)
            Debug.Log("::::::::::::::::::::::::: Find hyst: " + id + " :::::");
        foreach (var obj in resListById)
        {
            
            i1++;
            Debug.Log(i1 + ". " + obj.ToString());
        }
        Debug.Log("GAME: -----------------------------------");
        var listRealObjs = Storage.Person.GetAllRealPersonsForID(nameObj);
        if (listRealObjs != null && listRealObjs.Count() > 0)
            Debug.Log("::::::::::::::::::::::::: Find Real: " + id + " :::::");
        foreach (var obj in listRealObjs)
        {
            i1++;
            Debug.Log(i1 + ".   " + obj.ToString());
        }
        var listDataObjs = Storage.Person.GetAllDataPersonsForID(nameObj);
        if (listDataObjs != null && listDataObjs.Count() > 0)
            Debug.Log("::::::::::::::::::::::::: Find DATA: " + id + " :::::");
        foreach (var obj in listDataObjs)
        {
            i1++;
            Debug.Log(i1 + ".   " + obj.ToString());
        }

        var DataObj = Storage.Person.GetFindPersonsDataForName(nameObj);
        if (DataObj != null) { 
            Debug.Log("::::::::::::::::::::::::: Find Pesron DATA: " + id + " :::::");
            Debug.Log("DP:  [" + DataObj.Field + "][" + DataObj.Index + "] " + DataObj.DataObj );
        }

        string field = Helper.GetNameFieldByName(nameObj);
        var listDataObjsInField = Storage.Person.GetAllDataPersonsForName(field);
        if (listDataObjsInField != null && listDataObjsInField.Count() > 0)
            Debug.Log("::::::::::::::::::::::::: Find in Field: " + field + " :::::");
        foreach (var obj in listDataObjsInField)
        {
            i1++;
            Debug.Log(i1 + ".   " + obj.ToString());
        }

        DebugKill(nameObj);

        Debug.Log("*******************************************************************************************");

        return findName;
    }

    public void SaveHistory(string name, string actionName, string callFunc, string field = "", string comment = "", ModelNPC.ObjectData oldDataObj = null, ModelNPC.ObjectData newDataObj = null)
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

    public int ConflictLog(GameObject gobj, string p_nameField, List<ModelNPC.ObjectData> dataObjects)
    {
        FindPersonData findPersonData = null;
        int indDataNew = -1;
        //var listDataObjsInField = Storage.Person.GetAllDataPersonsForName(p_nameField);
        //----------
        FindPersonData findPersonDataT = Storage.Person.GetFindPersonsDataForName(gobj.name.GetID());
        if (findPersonDataT != null)
        {
            Debug.Log("##..... ConflictLog (" + gobj.name + ") GetFindPersonsDataForName[" + findPersonDataT.Field + "]: " + findPersonDataT.DataObj);
            if (gobj.name == findPersonDataT.DataObj.NameObject && findPersonDataT.Field == p_nameField)
            {
                indDataNew = findPersonDataT.Index;
                if (dataObjects.Count <= indDataNew)
                {
                    Debug.Log("##..... ConflictLog indDataNew[" + indDataNew + "]  out of range " + dataObjects.Count);
                }
                else
                {
                    if (dataObjects[indDataNew].NameObject != gobj.name)
                    {
                        Debug.Log("##..... ConflictLog dataObjects[" + indDataNew + "].NameObject[" + dataObjects[indDataNew].NameObject + "]  <>  GOBJ: " + gobj.name);
                        Debug.Log("##..... Objects in Data : ");
                        foreach (var doItem in dataObjects)
                        {
                            Debug.Log("#................. Data : " + doItem.ToString());
                        }
                        return -1;
                    }
                    else
                    {
                        return indDataNew;
                    }
                }
            }
            else
            {
                Debug.Log("##..... ConflictLog [" + p_nameField + "] (" + gobj.name + ") <> [" + findPersonDataT.Field + "]: " + findPersonDataT.DataObj);
            }
        }
        else
        {
            return -1;
        }
        return -1;
    }

    public class HistoryGameObject
    {
        public string Name { get; set; }
        public string Field { get; set; }
        public string ActionName { get; set; }
        public string CallFunc { get; set; }
        public string Comment { get; set; }
        public DateTime TimeSave { get; set; }
        public ModelNPC.ObjectData OldDataObj { get; set; }
        public ModelNPC.ObjectData NewDataObj { get; set; }

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
