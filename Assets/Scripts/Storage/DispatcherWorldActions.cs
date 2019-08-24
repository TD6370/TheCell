using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEditor;

public class DispatcherWorldActions : MonoBehaviour
{
    private bool m_IsRunSearching = false;
    private bool m_IsFilledSearchingCollection= false;
    public Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> PersonPriority;

    private void Awake()
    {
        
    }

    public bool isInit = false;

    private void LateUpdate()
    {
        Init();
    }
    private void Init()
    {
        if (isInit)
            return;

        LoadPriorityPerson();
        StartCoroutine(NavigatorWorldScene());

        //m_actionController = new GameActionPersonController();

        isInit = true;
    }

    void Start () {
        
    }
	
	void Update () {
		
	}

    private void LoadPriorityPerson()
    {
        PersonPriority = new Dictionary<SaveLoadData.TypePrefabs, PriorityFinder>();
        foreach(var prior in Storage.Person.ContainerPriority.CollectionPriorityFinder)
        {
            PersonPriority.Add(prior.TypeObserver, prior);
        }
    }

    public void ResetDispatcher()
    {
        //isInit = false;
        StopCoroutine(NavigatorWorldScene());
        StartCoroutine(NavigatorWorldScene());
    }

    IEnumerator NavigatorWorldScene()
    {
        var countP = 0;

        Queue<string> colectionLivePerson = new Queue<string>();

        while (true)
        {

            if (Storage.Instance.ReaderSceneIsValid)
            {
                if (colectionLivePerson.Count == 0)
                    m_IsFilledSearchingCollection = false;

                if (!m_IsFilledSearchingCollection)
                {
                    //TEST
                    Storage.EventsUI.ListLogAdd = "<<< Is Filled Searching Collection >>>";
                    m_IsRunSearching = true;
                    //Storage.ReaderWorld.CollectionInfoID;
                    foreach (string id in Storage.ReaderWorld.CollectionInfoID.Where(p => p.Value.Data.IsNPC()).Select(p => p.Key))
                    {
                        colectionLivePerson.Enqueue(id);
                    }
                    m_IsRunSearching = false;
                    m_IsFilledSearchingCollection = true;
                }
                //yield return null;

                foreach (int nextI in Enumerable.Range(0, 100))
                {
                    if (colectionLivePerson.Count == 0)
                        break;

                    if (!Storage.Instance.ReaderSceneIsValid)
                        break;

                    string nextPersonLiveID = colectionLivePerson.Dequeue();
                    ReaderScene.DataObjectInfoID infoNPC = Storage.ReaderWorld.CollectionInfoID[nextPersonLiveID];

                    //yield return null;

                    PersonWork(infoNPC, colectionLivePerson.Count);

                    //if (countP != colectionLivePerson.Count())
                    //{
                    //    countP = colectionLivePerson.Count();
                    //    Storage.EventsUI.ListLogAdd = "PESONS: " + colectionLivePerson.Count();
                    //}
                    if (colectionLivePerson.Count == 0)
                        break;
                }

                //ModelNPC.ObjectData dataNPC = infoNPC.Data;
                //Vector2 observerFieldPos = Helper.GetPositByField(infoNPC.Field);

                //float timeNext = Storage.Person.WaitTimeReaderScene; //Storage.Person.TestSpeed
                //yield return new WaitForSeconds(timeNext);
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private void PersonWork(ReaderScene.DataObjectInfoID infoNPC, int count)
    {
        var persData = infoNPC.Data as ModelNPC.PersonData;

        if (persData.IsReality)
        {
            //Storage.EventsUI.ListLogAdd = "NO WORK: " + persData.NameObject + " on REAL";
            return;
        }

        //persData.Id;
        List<GameActionPersonController.NameActionsPerson> actonsNPC = GameActionPersonController.GetActions(persData);
        GameActionPersonController.NameActionsPerson actionCurrent = GameActionPersonController.GetCurrentAction(persData);

        //GameActionPersonController.CheckComplitionActions(persData, actionCurrent, null); //$$$

        //actonsNPC = GameActionPersonController.GetActions(persData);
        actionCurrent = GameActionPersonController.GetCurrentAction(persData);

        GameActionPersonController.CheckNextAction(persData, actionCurrent, null);

        //TEST
        bool isLog = Storage.Person.IsLog;
        if (isLog)
            Storage.EventsUI.ListLogAdd =  count + " WORK: " + persData.NameObject + " >> " + actionCurrent.ToString();

        //-------------------
        string fieldInfo1 = infoNPC.Field;
        string fieldPos1 = Helper.GetNameFieldPosit(persData.Position.x, persData.Position.y);
        string fieldName1 = Helper.GetNameFieldByName(persData.NameObject);
        if (fieldInfo1 != fieldPos1 || fieldInfo1 != fieldName1 || fieldPos1 != fieldName1)// || fieldInfo != fieldGO)
        {
            string strErr = "??? PersonWork name Field I: " + fieldInfo1 + " P:" + fieldPos1 + " DN:" + fieldName1;// + " GO:" + fieldGO;
            Storage.EventsUI.ListLogAdd = strErr;
        }
        //-------------------

        bool isZonaReal = Helper.IsValidPiontInZona(persData.Position.x, persData.Position.y);
        if(!persData.IsReality && isZonaReal)
        {
            //TEST
            Storage.EventsUI.ListLogAdd = "GOTO IN REAL WORLD: " + persData.NameObject;

            string fieldInfo = infoNPC.Field;
            string fieldPos = Helper.GetNameFieldPosit(persData.Position.x, persData.Position.y);
            string fieldName = Helper.GetNameFieldByName(persData.NameObject);
            
            //string fieldGO = (infoNPC.Gobject == null) ? fieldInfo : Helper.GetNameFieldObject(infoNPC.Gobject);
            //if(fieldInfo != fieldPos || fieldInfo != fieldName || fieldInfo != fieldGO)
            if (fieldInfo != fieldPos || fieldInfo != fieldName || fieldPos != fieldName)// || fieldInfo != fieldGO)
            {
                string strErr = "??? PersonWork name Field I: " + fieldInfo + " P:" + fieldPos + " DN:" + fieldName;// + " GO:" + fieldGO;
                Debug.Log(strErr);
                //Storage.SetMessageAlert = strErr;
                Storage.EventsUI.ListLogAdd = strErr;
            }
            Storage.GenGrid.LoadObjectToReal(fieldName);
        }
        else { }
    }
}
