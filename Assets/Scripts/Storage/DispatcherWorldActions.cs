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

    bool isInit = false;

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
                    //Storage.ReaderWorld.CollectionInfoID;
                    foreach (string id in Storage.ReaderWorld.CollectionInfoID.Where(p => p.Value.Data.IsNPC()).Select(p => p.Key))
                    {
                        colectionLivePerson.Enqueue(id);
                    }
                    m_IsRunSearching = false;
                    m_IsFilledSearchingCollection = true;
                }
                yield return null;

                string nextPersonLiveID = colectionLivePerson.Dequeue();
                ReaderScene.DataObjectInfoID infoNPC = Storage.ReaderWorld.CollectionInfoID[nextPersonLiveID];

                yield return null;

                PersonWork(infoNPC);

                //ModelNPC.ObjectData dataNPC = infoNPC.Data;
                //Vector2 observerFieldPos = Helper.GetPositByField(infoNPC.Field);

                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private void PersonWork(ReaderScene.DataObjectInfoID infoNPC)
    {
        var persData = infoNPC.Data as ModelNPC.PersonData;

        if (persData.IsReality)
            return;

        //persData.Id;
        List<GameActionPersonController.NameActionsPerson> actonsNPC = GameActionPersonController.GetActions(persData);
        GameActionPersonController.NameActionsPerson actionCurrent = GameActionPersonController.GetCurrentAction(persData);

        //GameActionPersonController.CheckComplitionActions(persData, actionCurrent, null); //$$$

        actonsNPC = GameActionPersonController.GetActions(persData);
        actionCurrent = GameActionPersonController.GetCurrentAction(persData);

        GameActionPersonController.CheckNextAction(persData, actionCurrent, null);
    }
}
