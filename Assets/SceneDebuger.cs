using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using static GameActionPersonController;

public class SceneDebuger : MonoBehaviour {

    public static bool IsDebugOn = false;
    public GameObject DialogPersonPrefab;
    public GameObject PanelDialogPrefabs;

    public bool IsAutoRefresh = false;
    [Range(0.3f,10)]
    public float TimeRefresh = 1;
    private float m_waitRefresh = 0;

    private List<CaseSceneDialogPerson> m_poolDialogPersonPrefabs;
    private int m_maxPoolDialogs = 200;
    private Queue<SceneDialogPerson> m_collectionPersonData;

    void Start () {
        LoadDialogs();
	}
	
	void Update () {
		if(IsAutoRefresh)
        {
            if(Time.time > m_waitRefresh)
            {
                FillDialogsFromData();
                m_waitRefresh += TimeRefresh;
            }
        }
	}

    private void LoadDialogs()
    {
        m_collectionPersonData = new Queue<SceneDialogPerson>();
        if(DialogPersonPrefab == null)
        {
            Debug.Log("########## LoadDialogs NOT set DialogPersonPrefab");
            return;
        }

        m_poolDialogPersonPrefabs = new List<CaseSceneDialogPerson>();
        foreach (var i in Enumerable.Range(0, m_maxPoolDialogs))
        {
            GameObject dialog = Instantiate(DialogPersonPrefab);
            dialog.transform.position = new Vector3(0, 0, 0);
            dialog.transform.SetParent(null);
            dialog.SetActive(false);

            CaseSceneDialogPerson caseDialog = new CaseSceneDialogPerson()
            {
                Dialog = dialog
            };
            m_poolDialogPersonPrefabs.Add(caseDialog);
        };
    }

    public void ViewPerson(ModelNPC.PersonData dataNPC, GameActionPersonController.NameActionsPerson p_nameAction)
    {
        if (dataNPC.IsReality)
            return;

        //TEST
        //if(dataNPC.ModelView==null)
        //    Debug.Log(Storage.EventsUI.ListLogAdd = "#### ViewPerson dataNPC.ModelView is Null >> " + dataNPC.NameObject);

        m_collectionPersonData.Enqueue(new SceneDialogPerson()
        {
            NameAction = p_nameAction,
            Position = dataNPC.Position,
            TargetPosition = dataNPC.TargetPosition,
            ID = dataNPC.Id != null ? dataNPC.Id : Helper.GetID(dataNPC.NameObject)

            //Data = dataNPC
        });
        if(m_collectionPersonData.Count > m_maxPoolDialogs)
        {
            m_collectionPersonData.Dequeue();
        }
    }

    public void FillDialogsFromData()
    {
        if (!IsAutoRefresh)
            Storage.EventsUI.ListLogAdd = "IN DEBUG COUNT >>  "  + m_collectionPersonData.Count;

        while(m_collectionPersonData.Count>0)
        {
            var data = m_collectionPersonData.Dequeue();
            CaseSceneDialogPerson caseDialog = GetFreeDialog(data);
            if(caseDialog == null)
                caseDialog = GetFreeDialog(isForce: true);
            if (caseDialog == null)
            {
                Storage.EventsUI.ListLogAdd = "#### CASE DEBUG -- EMPTY";
                Debug.Log("######### FillDialogsFromData m_poolDialogPersonPrefabs is empty");
                break;
            }
            caseDialog.Activate(data);

            if (!IsAutoRefresh)
                Storage.EventsUI.ListLogAdd = "DEBUG ++ >> " + data.Data.NameObject;
        }
    }

    public void DialogsClear()
    {
        if (m_poolDialogPersonPrefabs == null || m_poolDialogPersonPrefabs.Count == 0)
            return;

        foreach (var itemCase in m_poolDialogPersonPrefabs)
            itemCase.Deactivate();
    }

    public void RefreshDialogs()
    { 
        if (m_poolDialogPersonPrefabs == null || m_poolDialogPersonPrefabs.Count == 0)
            return;

        DialogsClear();
        FillDialogsFromData();
    }

    private void ClearTemplate()
    {
        if (m_poolDialogPersonPrefabs == null || m_poolDialogPersonPrefabs.Count == 0)
            return;

        foreach (var caseDlg in m_poolDialogPersonPrefabs.Where(p => p.TimeCreate > Time.time - 10f))
        {
            caseDlg.Deactivate();
        }
    }

    private CaseSceneDialogPerson GetFreeDialog(SceneDialogPerson p_Data = null, bool isForce = false)
    {
        if (m_poolDialogPersonPrefabs == null || m_poolDialogPersonPrefabs.Count == 0)
            return null;

        CaseSceneDialogPerson findCase = null;
        if (isForce) {
            findCase = m_poolDialogPersonPrefabs.OrderBy(p => p.TimeCreate).FirstOrDefault();
        }
        else {
            if (p_Data != null)
            {
                //findCase = m_poolDialogPersonPrefabs.Find(
                //    p => p.Person != null &&
                //    p.Person.Data != null &&
                //    p.Person.Data.NameObject == p_Data.Data.NameObject);
                findCase = m_poolDialogPersonPrefabs.Find(
                        p => p.Person != null &&
                        p.Person.Data != null &&
                        p.ID == p_Data.Data.Id);
            }
            if (findCase == null)
                findCase = m_poolDialogPersonPrefabs.Where(p => !p.IsLock).FirstOrDefault();
        }
        return findCase;
    }
   
    #region CaseSceneDialogPerson
    public class CaseSceneDialogPerson
    {
        public string ID;
        public GameObject Dialog;
        public SceneDialogPerson Person;
        public bool IsLock;
        public float TimeCreate;
        public CaseSceneDialogPerson() { }

        private DialogSceneInfo m_dialogView;

        public void Activate(SceneDialogPerson p_Data)
        {
            Person = p_Data;
            //ID = Helper.GetID(p_Data.Data.NameObject);
            ID = p_Data.Data.Id;
            if (Dialog != null)
            {
                Dialog.transform.position = p_Data.Position;


                Dialog.SetActive(true);
                if(m_dialogView == null)
                    m_dialogView = Dialog.GetComponent<DialogSceneInfo>();

                m_dialogView.InitDialogView(this);
            }
            TimeCreate = Time.time;
        }
        public void Deactivate()
        {
            if (Dialog != null)
                Dialog.transform.position = Vector3.zero;
                Dialog.SetActive(false);
        }
    }
    #endregion

    #region SceneDialogPerson
    public class SceneDialogPerson
    {
        public Vector2 Position;
        public Vector2 TargetPosition;
        public GameActionPersonController.NameActionsPerson NameAction;
        public string ID;
        //public ModelNPC.PersonData Data;
        private string m_MessageInfo;

        public ModelNPC.PersonData Data {
            get
            {
                //TEST
                if(!Storage.ReaderWorld.CollectionInfoID.ContainsKey(ID))
                {
                    Storage.EventsUI.ListLogAdd = "IN DEBUG Data  ID not found  ";
                    return null;
                }
                var data = Storage.ReaderWorld.CollectionInfoID[ID];
                return data.Data as ModelNPC.PersonData;
            }
        }

        public SceneDialogPerson() { }
    }
    #endregion
}
