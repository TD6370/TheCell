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
        Queue<string> colectionLivePerson = new Queue<string>();

        while (true)
        {

            if (Storage.Instance.ReaderSceneIsValid)
            {
                if (colectionLivePerson.Count == 0)
                    m_IsFilledSearchingCollection = false;

                if (!m_IsFilledSearchingCollection)
                {
                    m_IsRunSearching = true;
                    foreach (string id in Storage.ReaderWorld.CollectionInfoID.Where(p => p.Value.Data.IsNPC()).Select(p => p.Key))
                    {
                        colectionLivePerson.Enqueue(id);
                    }
                    m_IsRunSearching = false;
                    m_IsFilledSearchingCollection = true;
                }

                foreach (int nextI in Enumerable.Range(0, 10))
                {
                    if (colectionLivePerson.Count == 0)
                        break;

                    if (!Storage.Instance.ReaderSceneIsValid)
                        break;

                    string nextPersonLiveID = colectionLivePerson.Dequeue();
                    if (!Storage.ReaderWorld.CollectionInfoID.ContainsKey(nextPersonLiveID))
                    {
                        Debug.Log("############## ReaderWorld.CollectionInfoID.ContainsKey Not found nextPersonLiveID ");
                        continue;
                    }
                    ReaderScene.DataObjectInfoID infoNPC = Storage.ReaderWorld.CollectionInfoID[nextPersonLiveID];

                    PersonWork(infoNPC, colectionLivePerson.Count);

                    if (colectionLivePerson.Count == 0)
                        break;
                }

                float timeNext = Storage.Person.WaitTimeReaderScene; //Storage.Person.TestSpeed
                yield return new WaitForSeconds(timeNext);
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
            return;

        List<GameActionPersonController.NameActionsPerson> actonsNPC = GameActionPersonController.GetActions(persData);
        GameActionPersonController.NameActionsPerson actionCurrent = GameActionPersonController.GetCurrentAction(persData);
        actionCurrent = GameActionPersonController.GetCurrentAction(persData);
        GameActionPersonController.CheckNextAction(persData, actionCurrent, null);

        bool isZonaReal = Helper.IsValidPiontInZona(persData.Position.x, persData.Position.y);
        if(!persData.IsReality && isZonaReal)
        {
            //TEST ------------------------
            Storage.EventsUI.ListLogAdd = "GOTO IN REAL WORLD: " + persData.NameObject;
            string fieldInfo = infoNPC.Field;
            string fieldPos = Helper.GetNameFieldPosit(persData.Position.x, persData.Position.y);
            string fieldName = Helper.GetNameFieldByName(persData.NameObject);
            if (fieldInfo != fieldPos || fieldInfo != fieldName || fieldPos != fieldName)
            {
                string strErr = "??? PersonWork name Field I: " + fieldInfo + " P:" + fieldPos + " DN:" + fieldName;
                Debug.Log(strErr);
                Storage.EventsUI.ListLogAdd = strErr;
            }
            //-----------------------------

            Storage.GenGrid.LoadObjectToReal(fieldName);

            //--- Debag
            //Storage.Instance.SelectGameObjectID = Helper.GetID(persData.NameObject);
            //Storage.EventsUI.ClearListExpandPersons();
            //GameObject gobject = Storage.Instance.GamesObjectsReal[fieldPos].Find(p => p.name == persData.NameObject);
            //if (gobject != null)
            //{
            //    Storage.EventsUI.AddMenuPerson(persData, gobject);
            //    Storage.GamePause = true;
            //}
            //else
            //{
            //    string strErr = "####### Not n REAL : " + persData.NameObject;
            //    Debug.Log(strErr);
            //    Storage.EventsUI.ListLogAdd = strErr;
            //}
            //-----------------
        }
        else { }
    }
}
