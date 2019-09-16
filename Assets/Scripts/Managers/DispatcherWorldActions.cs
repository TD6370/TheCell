using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
//using UnityEditor;

public class DispatcherWorldActions : MonoBehaviour
{
    private bool m_IsRunSearching = false;
    private bool m_IsFilledSearchingCollection= false;
    public bool m_isInit = false;
    public bool m_isStop = false;

    private void Awake()
    {
    }

    private void LateUpdate()
    {
        Init();
    }
    private void Init()
    {
        if (m_isInit)
            return;


        m_isInit = true;
        m_isStop = false;

        StopCoroutine(NavigatorWorldScene());
        StartCoroutine(NavigatorWorldScene());

        //m_actionController = new GameActionPersonController();

        //isInit = true;
    }

    void Start () {
        
    }
	
	void Update () {
		
	}

    //private void LoadPriorityPerson()
    //{
    //    PersonPriority = new Dictionary<SaveLoadData.TypePrefabs, PriorityFinder>();
    //    foreach(var prior in Storage.Person.ContainerPriority.CollectionPriorityFinder)
    //    {
    //        PersonPriority.Add(prior.TypeObserver, prior);
    //    }
    //}

    public void StopDispatcher()
    {
        m_isStop = true;
    }

    public void Resume()
    {
        m_isInit = false; 
    }

    public void ResetDispatcher()
    {
        try
        {
            //isInit = false;
            //StopCoroutine(NavigatorWorldScene());
            //StartCoroutine(NavigatorWorldScene());

            //isStop = false;
            //isInit = false;

        }
        catch (Exception ex)
        {
            Debug.Log("Error ResetDispatcher : " + ex.Message);
        }
    }

    public class CaseDreamWorker
    {
        //private float timePause = 2f;
        private float _timeWorkAction  {
            get {
                return Storage.SceneDebug.SettingsScene.TimeWorkAction;
            }
        }

        private string _id;
        public string ID { get { return _id; } }

        private float _timeStartDreamWork;
        public float TimeStartDreamWork { get { return _timeStartDreamWork; } }

        public CaseDreamWorker(string p_id, float p_timeCreate) {
            _timeStartDreamWork = p_timeCreate;
            _id = p_id;
         }
        public CaseDreamWorker(string p_id)
        {
            _timeStartDreamWork = Time.time + _timeWorkAction;
            _id = p_id;
        }
        public void NextTimeWorker()
        {
            _timeStartDreamWork = Time.time + _timeWorkAction;
        }
    }


    IEnumerator NavigatorWorldScene()
    {
        bool isTimeOfClear = false;
        float timeLimitResetNavigator = 10f;
        float timeLive = Time.time + timeLimitResetNavigator;
        int nextIndexID = 0;

        Queue<CaseDreamWorker> colectionLivePerson = new Queue<CaseDreamWorker>();
        Queue<CaseDreamWorker> colectionLivePersonVIP = new Queue<CaseDreamWorker>();
        List<string> listNPC;

        while (true)
        {
            //yield break;//TEST TEMP Close

            if (m_isStop)
            {
                m_isInit = false;
                yield break;
            }

            timeLimitResetNavigator = Storage.SceneDebug.SettingsScene.TimeLimitResetNavigator;

            if (Storage.Instance.ReaderSceneIsValid)
            {
                //---Init---
                if (colectionLivePerson.Count == 0)
                    m_IsFilledSearchingCollection = false;

                if (!m_IsFilledSearchingCollection)
                {
                    if (Storage.ReaderWorld.CollectionInfoID.Count == 0)
                    {
                        yield return new WaitForSeconds(5f);
                        continue;
                    }
                    string message = "Search dreamworkers...";

                    float timeStartSearch = Time.time;
                    Storage.EventsUI.ListLogAdd = "~~~~~~~~~~" + message;
                    //Storage.EventsUI.SetTittle = message;
                    m_IsRunSearching = true;
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< SPEED
                    //listNPC = Storage.ReaderWorld.CollectionInfoID.Where(p => p.Value.Data.IsNPC()).Select(p => p.Key).ToList();
                    listNPC = new List<string>();
                    int indexWait = 0;
                    var arrayID = Storage.ReaderWorld.CollectionInfoID.Values.ToArray();
                    yield return null;
                    int count = arrayID.Count();
                    nextIndexID = 0;
                    while(nextIndexID < count)
                    {
                        var item = arrayID[nextIndexID];
                        if (item.Data.IsNPC())
                            listNPC.Add(item.ID);
                        if(indexWait > 1000)
                        {
                            indexWait = 0;
                            yield return null;
                        }
                        indexWait++;
                        nextIndexID++;
                    }
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    if (listNPC.Count == 0)
                    {
                        //UnityEngine.Profiling.Profiler.BeginSample("Sample LOCK dreamworkers");
                        Storage.EventsUI.ListLogAdd = "...Search dreamworkers No NPC... all: " + count;
                        yield return new WaitForSeconds(8f);

                        //UnityEngine.Profiling.Profiler.EndSample();
                        //yield return null;
                        continue;
                        
                    }
                    yield return null;
                    List<Shuffle> listNPC_Rnd = new List<Shuffle>();
                    foreach (string id in listNPC)
                    {
                        int indRnd = Random.Range(1, listNPC.Count());
                        listNPC_Rnd.Add( new Shuffle() { ID = id, Index = indRnd } );
                    }
                    yield return null;
                    //Randomize list
                    listNPC = listNPC_Rnd.OrderBy(p => p.Index).Select(p => p.ID).ToList();
                    yield return null;
                    foreach (string id in listNPC)
                    {
                        //colectionLivePerson.Enqueue(new CaseDreamWorker(id));
                        //var item = new CaseDreamWorker(id);
                        colectionLivePerson.Enqueue(new CaseDreamWorker(id));
                    }
                    yield return null;
                    m_IsRunSearching = false;
                    m_IsFilledSearchingCollection = true;
                    //Next time reset
                    timeLive = Time.time + timeLimitResetNavigator;
                    isTimeOfClear = false;
                    //if(Storage.EventsUI.SetTittle == message)
                        //Storage.EventsUI.SetTittle = "";
                    Storage.EventsUI.ListLogAdd = "...Search dreamworkers end : " + (Time.time - timeStartSearch)  ;
                }
                //----
                //---Init VIP---
                //----
                //End time 
                if(timeLive < Time.time && !isTimeOfClear) //timeLimitResetNavigator;
                {
                    //Start clear
                    Storage.EventsUI.ListLogAdd = "~~~~~~~~~~ DREAMWORKER: TimeOfClear";
                    isTimeOfClear = true;
                }

                foreach (int nextI in Enumerable.Range(0, 1))
                {
                    if (colectionLivePerson.Count == 0)
                        break;

                    if (!Storage.Instance.ReaderSceneIsValid)
                        break;

                    //CaseDreamWorker dreamworker = colectionLivePerson.Peek();
                    CaseDreamWorker dreamworker = colectionLivePerson.Dequeue();

                    //Continue on time work ...
                    if (dreamworker.TimeStartDreamWork >= Time.time)
                    {
                        //Storage.EventsUI.ListLogAdd = "~~~~~~~Continue on time work ..." + dreamworker.ID;

                        //Back to Live collection
                        colectionLivePerson.Enqueue(dreamworker);
                        continue;
                    }

                    //Remove on Live collection
                    //if (isTimeOfClear)
                    //    dreamworker = colectionLivePerson.Dequeue();
                    //Back to Live collection
                    if (isTimeOfClear == false)
                    {
                        colectionLivePerson.Enqueue(dreamworker);
                    }

                    string nextPersonLiveID = dreamworker.ID;

                    if (!Storage.ReaderWorld.CollectionInfoID.ContainsKey(nextPersonLiveID))
                    {
                        //Debug.Log("############## ReaderWorld.CollectionInfoID.ContainsKey Not found nextPersonLiveID ");
                        continue;
                    }
                    ReaderScene.DataObjectInfoID infoNPC = Storage.ReaderWorld.CollectionInfoID[nextPersonLiveID];
                    if(infoNPC.Data == null)
                    {
                        Storage.EventsUI.ListLogAdd = "### NavigatorWorldScene InfoNPC.Data is EMPTY";
                        continue;
                    }

                    PersonWork(infoNPC, colectionLivePerson.Count);

                    dreamworker.NextTimeWorker();

                    if (colectionLivePerson.Count == 0)
                        break;
                }

#if UNITY_EDITOR
                Storage.SceneDebug.LivePersonsCount = colectionLivePerson.Count;
#endif

                float timeNext = Storage.SceneDebug.SettingsScene.TimeRelax; //Storage.Person.TestSpeed
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
        if (persData == null || persData.IsReality)
            return;

        List<GameActionPersonController.NameActionsPerson> actonsNPC = GameActionPersonController.GetActions(persData);
        GameActionPersonController.NameActionsPerson actionCurrent = GameActionPersonController.GetCurrentAction(persData);
        actionCurrent = GameActionPersonController.GetCurrentAction(persData);
        GameActionPersonController.CheckNextAction(persData, actionCurrent, null);

        //TEST -----------------------------
        if (Storage.SceneDebug.SettingsScene.IsLog)
            Storage.EventsUI.ListLogAdd = "WORK: " + persData.NameObject + " >> " + actionCurrent.ToString();

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

    public class Shuffle
    {
        public string ID;
        public int Index;
        public Shuffle() { }
    }
}
