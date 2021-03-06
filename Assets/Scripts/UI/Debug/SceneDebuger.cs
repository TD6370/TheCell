﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneDebuger : MonoBehaviour {

    [SerializeField, Tooltip("Settings debug scene")]
    public SettingsSceneDebug SettingsScene;
    [SerializeField, Tooltip("LivePersonsCount")]
    public int LivePersonsCount;

    public int LivePersonsStartCount;
    public int InfoCount;
    public GameObject DialogPersonPrefab;
    public GameObject PanelDialogPrefabs;

    private float m_waitClearTemplate = 0;
    private float m_waitRefresh = 0;

    private List<CaseSceneDialogPerson> m_poolDialogPersonPrefabs;
    private int m_maxPoolDialogs = 200;
    private Queue<SceneDialogPerson> m_collectionPersonData;

    public string VipID
    {
        get
        {
            return Storage.Instance.SelectGameObjectID;
        }
    }

    void Start () {
        LoadDialogs();
	}
	
	void Update () {
		if(SettingsScene.AutoRefreshOn)
        {
            if(Time.time > m_waitRefresh)
            {
                FillDialogsFromData();
                m_waitRefresh += SettingsScene.TimeRefreshDebugScene;
            }
            if(SettingsScene.IsClearTemplate && Time.time > m_waitRefresh)
            {
                ClearTemplate();
                m_waitRefresh += SettingsScene.TimeClearTemplate;
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
            dialog.name = "DialogPerson_" + i;

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

        m_collectionPersonData.Enqueue(new SceneDialogPerson()
        {
            NameAction = p_nameAction,
            Position = dataNPC.Position,
            TargetPosition = dataNPC.TargetPosition,
            NameObject = dataNPC.NameObject
        });
        if(m_collectionPersonData.Count > m_maxPoolDialogs)
        {
            m_collectionPersonData.Dequeue();
        }
    }

    public void FillDialogsFromData()
    {
        if (!SettingsScene.AutoRefreshOn)
            Storage.EventsUI.ListLogAdd = "IN DEBUG COUNT >>  "  + m_collectionPersonData.Count;

        while(m_collectionPersonData.Count>0)
        {
            SceneDialogPerson data = m_collectionPersonData.Dequeue();
            if(data == null)
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "#### FillDialogsFromData -- data is EMPTY");
                break;
            }
            if (data.Data == null)
                return; //fix null:Data

            CaseSceneDialogPerson caseDialog = GetFreeDialog(data);
            if(caseDialog == null)
                caseDialog = GetFreeDialog(isForce: true);
            if (caseDialog == null)
            {
                Storage.EventsUI.ListLogAdd = "#### CASE DEBUG -- EMPTY";
                Debug.Log("######### FillDialogsFromData caseDialog is empty");
                break;
            }
            caseDialog.Activate(data);

            if (!SettingsScene.AutoRefreshOn)
                Storage.EventsUI.ListLogAdd = "DEBUG ++ >> " + data.Data.NameObject;
        }
    }

    public CaseSceneDialogPerson CreateTargetDialog(SceneDialogPerson data, string modelViewTarget)
    {
        //CaseSceneDialogPerson caseDialog = GetFreeDialog(new SceneDialogPerson());
        CaseSceneDialogPerson caseDialog = GetFreeDialog(data, isTarget: true);
        if (caseDialog == null)
            caseDialog = GetFreeDialog(isForce: true);
        if (caseDialog == null)
        {
            Storage.EventsUI.ListLogAdd = "#### CASE DEBUG -- EMPTY";
            Debug.Log("######### FillDialogsFromData caseDialog is empty");
            return null;
        }
        caseDialog.ModelViewTarget = modelViewTarget;
        caseDialog.Activate(data, true);
        return caseDialog;
    }

    public void UpdateTargetDialog(CaseSceneDialogPerson caseDialog, SceneDialogPerson data, string modelViewTarget)
    {
        if (data.Data == null)
            return; //fix null:Data

        //Storage.EventsUI.ListLogAdd = "UpdateTargetDialog...........";
        caseDialog.ModelViewTarget = modelViewTarget;
        caseDialog.Activate(data, true);
    }


    public void DialogsClear()
    {
        if (m_poolDialogPersonPrefabs == null || m_poolDialogPersonPrefabs.Count == 0)
            return;

        if (!Storage.Instance.ReaderSceneIsValid)
            return;

        foreach (var itemCase in m_poolDialogPersonPrefabs)
            itemCase.Deactivate();
    }

    public void RefreshDialogs()
    { 
        if (m_poolDialogPersonPrefabs == null || m_poolDialogPersonPrefabs.Count == 0)
            return;

        //DialogsClear();
        FillDialogsFromData();
    }

    private void ClearTemplate()
    {
        if (m_poolDialogPersonPrefabs == null || m_poolDialogPersonPrefabs.Count == 0)
            return;
        float timeLive = Time.time - SettingsScene.TimeClearTemplate;
        foreach (var caseDlg in m_poolDialogPersonPrefabs.Where(p => p.IsLock && p.TimeCreate < timeLive))
        {
            caseDlg.Deactivate();
        }
    }

    private CaseSceneDialogPerson GetFreeDialog(SceneDialogPerson p_Data = null, bool isForce = false, bool thisIsTarget = false, DialogSceneInfo.ModeInfo filerNot = DialogSceneInfo.ModeInfo.Target, bool isTarget = false)
    {
        bool isFindedMe = false;
        if (m_poolDialogPersonPrefabs == null || m_poolDialogPersonPrefabs.Count == 0)
            return null;

        string _vip = VipID ?? "?";

        CaseSceneDialogPerson findCase = null;
        if (isForce)
        {
            Debug.Log(Storage.EventsUI.ListLogAdd = "#### GetFreeDialog isForce");
            findCase = m_poolDialogPersonPrefabs.OrderBy(p => p.TimeCreate).Where(p => p.Person.ID != _vip).FirstOrDefault();
        }
        else
        {
            if (p_Data != null)
            {
                //Find Exist
                if (!isTarget)
                {
                    findCase = m_poolDialogPersonPrefabs.Find(
                            p => p.Person != null &&
                            p.Person.Data != null &&
                            p.Person.ID == p_Data.ID &&
                            p.ModeInfo != filerNot);
                }
                else
                {
                    //find last view case 
                    findCase = m_poolDialogPersonPrefabs.Find(
                            p => p.Person != null &&
                            p.Person.Data != null &&
                            p.Person.ID == p_Data.ID &&
                            p.ModeInfo != DialogSceneInfo.ModeInfo.Person);
                }
            }

            if (findCase != null)
                isFindedMe = true;
            else
            {
                //findCase = m_poolDialogPersonPrefabs.Where(p => !p.IsLock).FirstOrDefault();
                findCase = m_poolDialogPersonPrefabs.Where(p => !p.IsLock &&
                                                            p.ModeInfo != filerNot).FirstOrDefault();
            }

            if (findCase != null && findCase.Person != null && findCase.Person.ID == _vip)
            {
                try
                {
                    //!!!! NullReferenceException: Object reference not set to an instance of an object
                    return m_poolDialogPersonPrefabs.Where(p => !p.IsLock && p.Person.ID != _vip).FirstOrDefault();
                }
                catch(System.Exception ex)
                {
                    Debug.Log(Storage.EventsUI.ListLogAdd = "### GetFreeDialog : " + ex.Message);
                    ClearTemplate();
                    DialogsClear();
                    RefreshDialogs();
                    return null;
                }
            }
            if (findCase == null)//force
            {
                //Debug.Log(Storage.EventsUI.ListLogAdd = "#### GetFreeDialog isForce on time");
                //Storage.EventsUI.ListLogAdd = "#### GetFreeDialog isForce on time";
                //findCase = m_poolDialogPersonPrefabs.OrderBy(p => p.TimeCreate).Where(p => p.Person.ID != _vip).FirstOrDefault(); ;
                findCase = m_poolDialogPersonPrefabs.OrderBy(p => p.TimeCreate).Where(p => p.Person.ID != _vip &&
                                                                p.ModeInfo != filerNot).FirstOrDefault();
            }
        }

        if (findCase != null && findCase.IsLock && isFindedMe == false)
            findCase.Deactivate();

        return findCase;
    }

    #region CaseSceneDialogPerson
    public class CaseSceneDialogPerson
    {
        //public string ID;
        public string ModelViewTarget;
        public GameObject Dialog;
        public SceneDialogPerson Person;
        public bool IsLock;
        public float TimeCreate;
        public CaseSceneDialogPerson() { }
        public DialogSceneInfo.ModeInfo ModeInfo;

        private DialogSceneInfo m_dialogView;

        public void Activate(SceneDialogPerson p_Data, bool isTarget = false)
        {
            IsLock = true;
            Person = p_Data;
            if (Dialog != null)
            {
                if (Person.Data != null)
                {
                    if (isTarget)
                        Dialog.transform.position = p_Data.TargetPosition;
                    else
                        Dialog.transform.position = p_Data.Position;


                    Dialog.SetActive(true);
                    if (isTarget)
                        Dialog.name = "Dialog_Target_" + ModelViewTarget + "_" + p_Data.NameObject;
                    else
                        Dialog.name = "Dialog_" + p_Data.NameObject;
                
                    //if (m_dialogView == null)
                    m_dialogView = Dialog.GetComponent<DialogSceneInfo>();

                    ModeInfo = isTarget ? DialogSceneInfo.ModeInfo.Target : DialogSceneInfo.ModeInfo.Person;
                    m_dialogView.DialogModelViewTarget = ModelViewTarget;
                    //IsLock = true;
                    m_dialogView.InitDialogView(this, ModeInfo);
                }
            }
            else{
                Debug.Log("######### CaseSceneDialogPerson Dialog is NULL"); 
            }
            TimeCreate = Time.time;
            //IsLock = true;
        }

        public void Deactivate()
        {
            if (!IsLock)
                return;
            //if (m_dialogView == null)
            //    return;

            if (ModeInfo == DialogSceneInfo.ModeInfo.Person)
            {
                //m_dialogView = Dialog.GetComponent<DialogSceneInfo>();
                var caseTarget = m_dialogView.CaseDialogTarget;
                if (caseTarget != null && caseTarget.IsLock)
                {
                    //Debug.Log(Storage.EventsUI.ListLogAdd = "+++ Deactivate ...." + caseTarget.ModelViewTarget);

                    caseTarget.ModeInfo = DialogSceneInfo.ModeInfo.Target;
                    caseTarget.Deactivate();
                    
                }
                //m_dialogView.Deactivate();

                //TEST
                //if (caseTarget == null)
                //{
                //    Debug.Log(Storage.EventsUI.ListLogAdd = "#### Deactivate() CaseDialogTarget -- EMPTY");
                //}
            }
            m_dialogView.Deactivate();

            if (Dialog != null)
            {
                Dialog.name = "DialogPerson_empty";
                Dialog.transform.position = Vector3.zero;
                Dialog.SetActive(false);
            }
            else
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "#### Deactivate Dialog -- EMPTY");
            }
            

            ModeInfo = DialogSceneInfo.ModeInfo.Person;
            IsLock = false;
        }
    }
    #endregion

    #region SceneDialogPerson
    public class SceneDialogPerson
    {
        public string NameObject = "";
        public Vector2 Position;
        public Vector2 TargetPosition;
        public GameActionPersonController.NameActionsPerson NameAction;
        private string m_MessageInfo;

        private string _id;
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                {
                    _id = Helper.GetID(NameObject);
                }
                if (string.IsNullOrEmpty(_id))
                    return "?";
                return _id;
            }
        }
        private string _ModelView;
        public string ModelView
        {
            get
            {
                if (string.IsNullOrEmpty(_ModelView))
                {
                    if (Data != null)
                    {
                        _ModelView = (Data.ModelView == null) ? Data.TypePrefabName : Data.ModelView;
                    }
                }
                return _ModelView;
            }
        }

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
