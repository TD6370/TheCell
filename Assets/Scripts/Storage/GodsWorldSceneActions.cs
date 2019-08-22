using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class GodsWorldSceneActions : MonoBehaviour {

    public ContainerPriorityFinder ContainerPriority;

    private void Awake()
    {
    }

    void Start () {
        //StartCoroutine(NavigateWorldScne());
	}
	
	void Update () {
		
	}

    //float m_realtimeMoving = Time.time + 0.5f;
    private bool m_IsRunSearching = false;
    private bool m_IsFilledSearchingCollection= false;

    IEnumerator NavigateWorldScne()
    {
        Queue<string> colectionLivePerson = new Queue<string>();

       

        while (true)
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
            //ctor2 observerFieldPos = Helper.GetPositByField(infoNPC.Field);


            yield return new WaitForSeconds(2f);
        }
    }

    private void PersonWork(ReaderScene.DataObjectInfoID infoNPC)
    {

        //public class PersonData : GameDataNPC
        //public override Vector3 TargetPosition { get; set; }

        //public string Id { get; set; }

        //public string[] PersonActions { get; set; } //$$$

        var persData = infoNPC.Data as ModelNPC.PersonData;
        //persData.Id;
        List<GameActionPersonController.NameActionsPerson> actonsNPC = GameActionPersonController.GetActions(persData);
        var t = persData.CurrentAction;
        //string[] actonsNPC = persData.PersonActions;
    }
}
