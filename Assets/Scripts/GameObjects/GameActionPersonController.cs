using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameActionPersonController : MonoBehaviour
{
    public static float TimeIdleLock = 1f;
    public static float TimeWork = 3f;
    public static int LimitListCommandActions = 3;

    public enum NameActionsPerson
    {
        //None, Idle, Move, Dead, Work, Attack, Completed //, Target //, Completion
        None, Idle, IdleLock, Move, Target, TargetLocal, TargetBackToBase, Dead, Work, Attack, Completed, CompletedLoot //, Target //, Completion
    };

    public static bool IsGameActionPersons = true;
    public bool IsStartInit = false;
    private bool m_stateInit = false;
    private bool m_isMeHero = false;


    public GameObject PanelModelsViewNPC;
    private PlayerAnimation m_MeAnimation;

    private GameObject m_MeModelView;
    public GameObject ModelView
    {
        get { return m_MeModelView; }
    }

    private ModelNPC.GameDataAlien DataAlien
    {
        get { return m_dataNPC as ModelNPC.GameDataAlien; }
    }

    public static float TimeWaitIdle = 5f;
    public static float MinDistEndMove = 1.2f;
    public NameActionsPerson ActionPerson = NameActionsPerson.None;

    private LineRenderer m_lineRenderer;

    private SpriteRenderer m_MeRender;
    private SpriteRenderer renderBack = null;
    private SpriteRenderer renderFront = null;

    private Vector2 m_MeMovement;
    private NameActionsPerson temp_ActionPerson = NameActionsPerson.None;
    private ModelNPC.GameDataAlien m_dataNPC;
    private ModelNPC.GameDataAlien temp_dataNPC;
    private MovementNPC m_meMovement; //@@$$
    
    private Dictionary<SaveLoadData.TypePrefabs, GameObject> m_ListViewModels;
    private SaveLoadData.TypePrefabs temp_TypePrefab = SaveLoadData.TypePrefabs.PrefabField;
    private PositionRenderSorting m_sortingLayer;

    private void Awake()
    {
        m_sortingLayer = GetComponent<PositionRenderSorting>();
        m_lineRenderer = GetComponent<LineRenderer>();

        LoadModelsView();
    }

    void Start()
    {
        m_meMovement = gameObject.GetComponent<MovementBoss>();
        if (m_meMovement == null)
            Debug.Log("###### GameActionPersonController.Start  m_meMovement == null");
    }

    void Update()
    {
        if (m_meMovement == null)
            return;

        if (IsStartInit)
            ChangeActions();
    }


    private void OnDrawGizmos()
    {
        //if (Storage.SceneDebug.SettingsScene.IsShowTittleInfoPerson)
        //{
        //    if (m_dataNPC != null && m_dataNPC.TargetPosition != Vector3.zero)
        //    {
        //        DrawGizmosLine(transform.position, m_dataNPC.TargetPosition, "#8533ff".ToColor());
        //    }
        //}
    }


    private void DrawGizmosLine(Vector3 pos1, Vector3 pos2, Color color)
    {
#if UNITY_EDITOR
        Gizmos.color = color;
        Gizmos.DrawLine(pos1, pos2);
#endif
    }

    void OnGUI()
    {
        if (Storage.SceneDebug.SettingsScene.IsShowTittleInfoPerson)
        {
            if (DataAlien == null)
                return;

            string objID = DataAlien.GetId; // ModelView
            if (objID == null)
                return;

            Color tittleColor = "#FFE881".ToColor();
            float distan = Vector2.Distance(DataAlien.Position, DataAlien.TargetPosition);

//#if UNITY_EDITOR

//            UnityEditor.Handles.DrawLine(transform.position, m_dataNPC.TargetPosition);
//#endif

            string animationInfo = "";
            if (m_MeAnimation != null)
            {
                animationInfo = "\n Play : " + m_MeAnimation.CurrentAnimationPlay;
            }

            string inventory = "";
            if (DataAlien.Inventory != null)
            {
                inventory = "\n Inventory: " + DataAlien.Inventory.NameInventopyObject + "(" + DataAlien.Inventory.Count + ")";
            }

            string actionList = "";
            foreach (var item in DataAlien.PersonActions)
            {
                actionList += "\n" + item;
            }
            if (DataAlien.PersonActions.Length > 0)
                actionList = "\n Commands : " + actionList;

            Color colorGreen = "#9FFF00".ToColor();
            Color colorBlue = "#8BD6FF".ToColor();
            Color colorOrange = "#FFD600".ToColor();

            if (DataAlien.CurrentAction == NameActionsPerson.Idle.ToString())
                tittleColor = colorGreen;

            if (DataAlien.CurrentAction == NameActionsPerson.Move.ToString())
                tittleColor = colorBlue;

            string TargetName = "";
            if (Storage.Instance.ReaderSceneIsValid)
            {
                if (DataAlien.TargetID != null && Storage.ReaderWorld.CollectionInfoID.ContainsKey(DataAlien.TargetID))
                {
                    tittleColor = colorOrange;
                    TargetName += "\nTarget Name: " + Storage.ReaderWorld.CollectionInfoID[DataAlien.TargetID].Data.NameObject;
                }
            }

            string jobInfo = string.Empty;
            if (!string.IsNullOrEmpty(DataAlien.JobName))
            {
                jobInfo = string.Format("\nJOB: {0}\nTo: {1}\nResource: {2}\nResult: {3}",DataAlien.Job.Job.ToString(), DataAlien.Job.JobTo, DataAlien.Job.TargetResource, DataAlien.Job.ResourceResult);
            }

            string messageInfo =
                "Action  :" + DataAlien.CurrentAction + "  (" + DataAlien.PersonActions.Length + ")"
                + "\nTimeWork  : " + (Time.time - DataAlien.TimeEndCurrentAction)
                + animationInfo
                + "\nDistan : "
                + distan
                + "\nTarget : " + Helper.GetNameField(DataAlien.TargetPosition)
                + TargetName
                + inventory
                + jobInfo;
            
            //string messageInfo = "TEST";

           GUIStyle style = new GUIStyle();
            style.fontSize = 16;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = tittleColor; // Color.yellow;


            if (Camera.main == null)
                return;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            Rect positionRect = new Rect(screenPosition.x - 10, Screen.height - screenPosition.y - 200, 400, 100);
            GUI.Label(positionRect, messageInfo, style);
            //GUI.TextField(positionRect, messageInfo, 25, style);
        }
    }

    public void ChangeActions()
    {
        if (m_meMovement == null)
            return;

        CheckUpdateModelView();

        //TEST
        if (m_dataNPC == null)
        {
            Debug.Log(Storage.EventsUI.ListLogAdd = "#### ChangeActions dataNPC  is Null >> " + gameObject.name);
            return;
        }
        if (m_dataNPC != null && m_dataNPC.ModelView == null)
            Debug.Log(Storage.EventsUI.ListLogAdd = "#### ChangeActions dataNPC.ModelView is Null >> " + m_dataNPC.NameObject);

        CheckNextAction(m_dataNPC, ActionPerson, this);
    }

    #region Fill data
    public void StartInit()
    {
        m_stateInit = false;
        IsStartInit = true;
        TimeInField = Time.time + limitLockInField;
    }

    public void PlayAnimationIdle()
    {
        if (m_MeAnimation != null)
            m_MeAnimation.PersonIdle();
    }

    public void PlayAnimationWork()
    {
        if (m_MeAnimation != null)
            m_MeAnimation.PersonWork();
    }

    public static ModelNPC.GameDataAlien GetAlienData(ModelNPC.PersonData dataNPC)
    {
        return dataNPC as ModelNPC.GameDataAlien;
    }

    public void ResetAction()
    {
        temp_ActionPerson = NameActionsPerson.None;
    }

    private void InitCurrentAction()
    {
        if (temp_ActionPerson == NameActionsPerson.None && temp_ActionPerson == ActionPerson && !string.IsNullOrEmpty(m_dataNPC.CurrentAction))
        {
            temp_ActionPerson = ActionPerson = (NameActionsPerson)Enum.Parse(typeof(NameActionsPerson), m_dataNPC.CurrentAction.ToString());
            StartActionNPC(m_dataNPC, ActionPerson, this);
        }
    }

    public static List<NameActionsPerson> GetActions(ModelNPC.PersonData dataNPC)
    {
        var ListPersonActions = new List<NameActionsPerson>();
        if (dataNPC.PersonActions == null)
            dataNPC.PersonActions = new string[] { };

        for (int i = 0; i < dataNPC.PersonActions.Length; i++)
        {
            NameActionsPerson nextActon = (NameActionsPerson)Enum.Parse(typeof(NameActionsPerson), dataNPC.PersonActions[i].ToString()); ;
            ListPersonActions.Add(nextActon);
        }
        return ListPersonActions;
    }

    public static NameActionsPerson GetCurrentAction(ModelNPC.GameDataAlien dataNPC)
    {
        if(string.IsNullOrEmpty(dataNPC.CurrentAction))
        {
            //Debug.Log("####### GetCurrentAction dataNPC.CurrentAction is null");
            dataNPC.CurrentAction = NameActionsPerson.Idle.ToString();
        }
        return (NameActionsPerson)Enum.Parse(typeof(NameActionsPerson), dataNPC.CurrentAction); ;
    }

    public static void CheckCurrentAction(ModelNPC.GameDataAlien dataNPC, ref NameActionsPerson result)
    {
        if (string.IsNullOrEmpty(dataNPC.CurrentAction))
        {
            //Debug.Log("####### GetCurrentAction dataNPC.CurrentAction is null");
            dataNPC.CurrentAction = NameActionsPerson.Idle.ToString();
        }
        if(dataNPC.CurrentAction != NameActionsPerson.Completed.ToString())
            result = (NameActionsPerson)Enum.Parse(typeof(NameActionsPerson), dataNPC.CurrentAction); ;
    }

    public static void GetCurrentAction_Cache(ref NameActionsPerson result, ModelNPC.GameDataAlien dataNPC)
    {
        if (string.IsNullOrEmpty(dataNPC.CurrentAction))
        {
            dataNPC.CurrentAction = NameActionsPerson.Idle.ToString();
        }
        result = (NameActionsPerson)Enum.Parse(typeof(NameActionsPerson), dataNPC.CurrentAction); ;
    }
    #endregion

    public static void CheckNextAction(ModelNPC.GameDataAlien dataNPC, NameActionsPerson p_actionPerson, GameActionPersonController controller)
    {
        CheckCompletionActions(dataNPC, p_actionPerson, controller);

        var listPersonActions = GetActions(dataNPC);
        //fix WORK
        CheckCurrentAction(dataNPC, ref p_actionPerson);
        //p_actionPerson = GetCurrentAction(dataNPC);

        if (dataNPC.CurrentAction == NameActionsPerson.Completed.ToString())
        {
            if (listPersonActions.Count == 0)
                AddActionNPC(dataNPC, NameActionsPerson.Completed);

            listPersonActions = GetActions(dataNPC);

            if (listPersonActions.Count > 0)
            {
                NameActionsPerson actionPerson = listPersonActions[0];
                listPersonActions.RemoveAt(0);
                dataNPC.PersonActions = listPersonActions.Select(p => p.ToString()).ToArray();// (p=>p.).ToArray();
                StartActionNPC(dataNPC, actionPerson, controller);
            }
        }
        else
        {
            StartActionNPC(dataNPC, p_actionPerson, controller);
        }
    }

    public static void CheckCompletionActions(ModelNPC.GameDataAlien dataNPC, NameActionsPerson actionPerson, GameActionPersonController controller)
    {
        //dleLock, Move, Target, TargetLocal, TargetBackToBase
        switch (actionPerson)
        {
            case NameActionsPerson.Idle:
                CheckComplitionIdle(dataNPC, controller);
                break;
            case NameActionsPerson.IdleLock:
                CheckComplitionIdleLock(dataNPC, controller);
                break;
            case NameActionsPerson.Move:
                if (controller != null)
                    controller.CheckComplitionMove();
                else
                    CheckComplitionMoveInDream(dataNPC);
                break;
            case NameActionsPerson.None:
                if (controller != null)
                    controller.ActionPerson = NameActionsPerson.Idle;
                dataNPC.CurrentAction = NameActionsPerson.Idle.ToString();
                break;
            case NameActionsPerson.CompletedLoot:
                CheckComplitionLoot(dataNPC, controller);
                break;
            case NameActionsPerson.Work:
                CheckComplitionWork(dataNPC, controller);
                break;
            case NameActionsPerson.Target:
            case NameActionsPerson.TargetLocal:
            case NameActionsPerson.TargetBackToBase:
                break;
            case NameActionsPerson.Attack:
            case NameActionsPerson.Dead:
            case NameActionsPerson.Completed:
            
                break;
        }

    }

    public static void StartActionNPC(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction, GameActionPersonController controller)
    {
        if (p_nameAction != NameActionsPerson.None)
        {
            if (controller != null)
                controller.SetAction(p_nameAction);
        }

        if (controller != null)
            dataNPC.CurrentAction = controller.ActionPerson.ToString();
        else
            dataNPC.CurrentAction = p_nameAction.ToString();

        switch (p_nameAction)
        {
            case NameActionsPerson.Idle:
                ActionIdle(dataNPC, controller);
                break;
            case NameActionsPerson.IdleLock:
                ActionIdleLock(dataNPC, controller);
                break;
            case NameActionsPerson.Move:
                if (controller == null)
                    ActionMove(dataNPC);
                break;
            case NameActionsPerson.Completed:
                ActionTarget(dataNPC, controller);
                break;
            case NameActionsPerson.Dead:
                break;
            case NameActionsPerson.Attack:
                break;
            case NameActionsPerson.Work:
                ActionWork(dataNPC, controller);
                break;
            case NameActionsPerson.Target:
                ActionTarget(dataNPC, controller);
                break;
            case NameActionsPerson.TargetLocal:
                ActionTargetLocal(dataNPC, controller);
                break;
            case NameActionsPerson.TargetBackToBase:
                ActionTargetBackToBase(dataNPC, controller);
                break;
        }

        //Debug
        if(controller == null || Storage.SceneDebug.SettingsScene.RealDebugOn || Storage.SceneDebug.VipID == dataNPC.Id)
            Storage.SceneDebug.ViewPerson(dataNPC, p_nameAction);
    }

    public static void RequestActionNPC(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction, GameActionPersonController controller, bool isForce = false)
    {
        if (isForce)
            controller.ResetAction();

        if(p_nameAction != NameActionsPerson.Completed)
            AddActionNPC(dataNPC, p_nameAction);
        
        if (controller != null)
            controller.ActionPerson = NameActionsPerson.Completed;

        dataNPC.CurrentAction = NameActionsPerson.Completed.ToString();
    }

    public static void ExecuteActionNPC(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction, GameActionPersonController controller, bool isForce = false)
    {
        if (isForce && controller!=null)
            controller.ResetAction();

        if (controller != null)
            controller.ActionPerson = p_nameAction;

        dataNPC.CurrentAction = p_nameAction.ToString();
    }

    public static void EndAction(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        if (controller != null)
            controller.ActionPerson = NameActionsPerson.Completed;
        dataNPC.CurrentAction = NameActionsPerson.Completed.ToString();
    }


    public static void AddActionNPC(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction = NameActionsPerson.None)
    {
        var listPersonActions = GetActions(dataNPC);

        if (listPersonActions.Count() > LimitListCommandActions)
            return;
        
        if (listPersonActions.Count > 0)
        {
            var lastAction = listPersonActions[listPersonActions.Count - 1];
            if (lastAction == p_nameAction) // repeat
                return;
        }
        listPersonActions.Add(p_nameAction);
        dataNPC.PersonActions = listPersonActions.Select(p => p.ToString()).ToArray();
    }

    public void SetAction(NameActionsPerson p_nameAction)
    {
        ActionPerson = p_nameAction;
    }

    #region Checked and run actions

    

    private static ModelNPC.ObjectData m_TargetObject = null;
    private static AlienJob temp_job = null;
    private static bool ActionTargetIsLock = false;

    public static void ActionTarget(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        if(ActionTargetIsLock) {
            Debug.Log(Storage.EventsUI.ListLogAdd = ".....ActionTarget is LOCK");
            return;
        }
        ActionTargetIsLock = true;

        //Storage.EventsUI.ListLogAdd = "ActionTarget .... ReaderSceneIsValid=" + Storage.Instance.ReaderSceneIsValid;
        temp_job = null;

        if (!Storage.Instance.ReaderSceneIsValid)// && TimeEndCurrentAction < Time.time)
        {
            ActionTargetLocal(dataNPC, controller);
            ActionTargetIsLock = false;
            return;
        }

        Storage.EventsUI.ListLogAdd = "ActionTarget ....!!!";

        //string tempID = dataNPC.TargetID;
        string indErr = "0";
        
        try {
            indErr = "1";
            GetAlienData(dataNPC).OnTargetCompleted();
            indErr = "2";
            //m_TargetObject = Storage.Person.GetAlienNextTargetObject(GetAlienData(dataNPC));
            m_TargetObject = null;
            //@JOB@
            temp_job = dataNPC.Job;
            //Storage.Person.GetAlienNextTargetObject(ref m_TargetObject, ref temp_job, GetAlienData(dataNPC));
            AlienJobsManager.GetAlienNextTargetObject(ref m_TargetObject, ref temp_job, GetAlienData(dataNPC));
        }
        catch (Exception ex)
        {
            Storage.EventsUI.ListLogAdd = "##### ActionTarget " + ex.Message;
            ActionTargetIsLock = false;
            return;
        }

        //test
        if (m_TargetObject == null)
            Storage.EventsUI.ListLogAdd = "*** " + dataNPC.NameObject + " ==> empty";
        else
            Storage.EventsUI.ListLogAdd = "*** " + dataNPC.NameObject + " ==> " + m_TargetObject.NameObject;
            
        if (m_TargetObject == null)
            GetAlienData(dataNPC).SetTargetPosition();
        else
        {
            var targetPosition = m_TargetObject.Position;
            dataNPC.TargetID = m_TargetObject.Id;
            //FIX base>>ToPortal
            (dataNPC as ModelNPC.GameDataAlien).BaseLockedTargetID = dataNPC.TargetID;
            dataNPC.SetTargetPosition(targetPosition);
            dataNPC.Job = temp_job;//@JOB@
        }
    
        ExecuteActionNPC(dataNPC, NameActionsPerson.Move, controller);

        if(controller!=null)
            controller.DrawRayTarget(); //TEST

        ActionTargetIsLock = false;
    }

    public static void ActionTargetLocal(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        string tempID = dataNPC.TargetID;

        GetAlienData(dataNPC).OnTargetCompleted();

        string id = GetAlienData(dataNPC).BaseLockedTargetID;
        bool isNotBaseLocked = string.IsNullOrEmpty(id);
        //if (controller != null && string.IsNullOrEmpty(controller.TempLockedTargetID))
        if (controller != null && isNotBaseLocked)
        {
            //Save ID
            //controller.TempLockedTargetID = tempID; // dataNPC.TargetID;
            GetAlienData(dataNPC).BaseLockedTargetID = tempID;
        }

        dataNPC.SetTargetPosition();
        ExecuteActionNPC(dataNPC, NameActionsPerson.Move, controller);

        if (controller != null)
            controller.DrawRayTarget(); //TEST
    }

    public static void ActionTargetBackToBase(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        GetAlienData(dataNPC).OnTargetCompleted();

        if (controller != null)
        {
            //dataNPC.TargetID = controller.TempLockedTargetID;
            GetAlienData(dataNPC).ReturnBaseTarget();
            //GetAlienData(dataNPC).BaseLockedTargetID
            GetAlienData(dataNPC).BaseLockedTargetID = string.Empty;
        }
        RequestActionNPC(dataNPC, NameActionsPerson.Move, controller);

        if (controller != null)
            controller.DrawRayTarget(); //TEST
    }
 

    public static bool ActionIdle(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        float tilme = GetAlienData(dataNPC).TimeEndCurrentAction;
        if(tilme == -1)
            GetAlienData(dataNPC).TimeEndCurrentAction = Time.time + TimeWaitIdle;
        return false;
    }

    public static bool ActionIdleLock(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        float tilme = GetAlienData(dataNPC).TimeEndCurrentAction;
        if (tilme == -1)
            GetAlienData(dataNPC).TimeEndCurrentAction = Time.time + TimeIdleLock;
        return false;
    }

    public static bool ActionWork(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        float tilme = GetAlienData(dataNPC).TimeEndCurrentAction;
        if (tilme == -1)
            GetAlienData(dataNPC).TimeEndCurrentAction = Time.time + TimeWork;
        return false;
    }

    private void ActionDead()
    {
    }

    //**************************   CHECK   *************************************

    public static void CheckComplitionIdle(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        if (dataNPC == null)
        {
            string strErr = "########## CheckComplitionIdle dataNPC == null ";
            if (controller != null)
                strErr += controller.gameObject.name;
            Debug.Log(strErr);
            return;
        }

        float timeWait = (dataNPC as ModelNPC.GameDataAlien).TimeEndCurrentAction;
        if (Time.time > timeWait)
        {
            GetAlienData(dataNPC).TimeEndCurrentAction = -1;
            RequestActionNPC(dataNPC, NameActionsPerson.Completed, controller);
            //IdleLock
        }

        //FIXANIM
        if (controller != null)
            controller.PlayAnimationIdle();
    }

    public static void CheckComplitionIdleLock(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        if (controller == null)
        {
            //TEST
            RequestActionNPC(dataNPC, NameActionsPerson.Completed, controller);
            return;
        }

        float timeWait = (dataNPC as ModelNPC.GameDataAlien).TimeEndCurrentAction;
        //if (Time.time > controller.TimeIdleLock && p_nameAction == NameActionsPerson.Idle)
        if (Time.time > timeWait)
        {
            GetAlienData(dataNPC).TimeEndCurrentAction = -1;
            RequestActionNPC(dataNPC, NameActionsPerson.Completed, controller);
            //IdleLock
        }

        //FIXANIM
        if (controller != null)
            controller.PlayAnimationIdle();
    }

    public static void CheckComplitionWork(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        if (controller != null)
            controller.PlayAnimationWork();

        float timeWait = (dataNPC as ModelNPC.GameDataAlien).TimeEndCurrentAction;
        if (Time.time > timeWait) //time end animation and completed work
        {
            GetAlienData(dataNPC).TimeEndCurrentAction = -1;
            if (dataNPC.Job != null)
            {
                //dataNPC.Job.IsJobRun = false;
                //dataNPC.Job.IsJobCompleted = true;
                //dataNPC.CurrentAction = NameActionsPerson.CompletedLoot.ToString();
                //if (p_dataNPC.CurrentAction == GameActionPersonController.NameActionsPerson.CompletedLoot.ToString())
                ExecuteActionNPC(dataNPC, NameActionsPerson.CompletedLoot, controller);
            }
            else
            {
                RequestActionNPC(dataNPC, NameActionsPerson.Completed, controller);
            }
        }
    }

    public static void CheckComplitionLoot(ModelNPC.GameDataAlien dataNPC, GameActionPersonController controller)
    {
        if (AlienJobsManager.CheckJobAlien(dataNPC) == false)
        {
            RequestActionNPC(dataNPC, NameActionsPerson.Target, controller);
        }
    }

    #endregion

    #region Action Move

    private int stepTest = 0;
    private int stepLimitTest = 10;
    private string lastFieldForLock = "";
    private float limitLockInField = 5f; //3f;
    private float TimeInField;
    private float minDistLck = 0.0005f;
    private Vector3 lastPositionForLock;
    //public bool IsLocked = false;
    //public string TempLockedTargetID;

    public static void CheckComplitionMoveInDream(ModelNPC.GameDataAlien dataNPC)
    {
        Vector3 newCurrentPosition = GetAlienData(dataNPC).Position;// .MovePosition;
        float dist = Vector3.Distance(dataNPC.TargetPosition, newCurrentPosition);
        string nameFiledTarget = Helper.GetNameField(dataNPC.TargetPosition);
        string nameFiledCurrent = Helper.GetNameField(newCurrentPosition);

        //End move to Target
        bool trueDist = dist < MinDistEndMove;
        bool trueField = nameFiledTarget == nameFiledCurrent;

        if (trueDist || trueField)
        {
            //@JOB@
            if (AlienJobsManager.CheckJobAlien(dataNPC) == false)
            {
                RequestActionNPC(dataNPC, NameActionsPerson.Idle, null);
            }
        }
    }

    private void CheckComplitionMove()
    {

        if (m_dataNPC == null || m_MeMovement == null || m_MeAnimation == null)
            return;

        Vector3 targetPosition = m_dataNPC.TargetPosition;
        m_MeMovement = targetPosition - transform.position;
        if (m_MeMovement != new Vector2())
        {
            if (m_MeMovement.x != 0)
            {
                bool isRight = m_MeMovement.x > 0;
                //<<ANIMATION>>
                m_MeAnimation.PersonLook(isRight);
            }
        }

        bool isCompletedMoving = false;
        isCompletedMoving = TestMoveTargetLock();
        if(isCompletedMoving) //LOCK
        {
            RequestActionNPC(m_dataNPC, NameActionsPerson.IdleLock, this);
            int colWay = UnityEngine.Random.Range(1,4);
            foreach(int i in Enumerable.Range(0,colWay))
            {
                RequestActionNPC(m_dataNPC, NameActionsPerson.TargetLocal, this);
            }
            RequestActionNPC(m_dataNPC, NameActionsPerson.TargetBackToBase, this);
        }
        else
        {
            float dist = Vector3.Distance(targetPosition, transform.position);
            isCompletedMoving = dist < MinDistEndMove;
            if (isCompletedMoving) //END WAY TO BASE TARGET
            {
                //@JOB@
                if (AlienJobsManager.CheckJobAlien(m_dataNPC, this) == false)
                {
                    RequestActionNPC(m_dataNPC, NameActionsPerson.Idle, this);
                    RequestActionNPC(m_dataNPC, NameActionsPerson.Target, this);
                }
                isCompletedMoving = true;
            }
        }

        bool isAnimateMove = (ActionPerson == NameActionsPerson.Move) && isCompletedMoving == false;

        //<<ANIMATION>>
        if (m_MeAnimation != null)
            m_MeAnimation.PersonMove(isAnimateMove);
    }

    
    private bool TestMoveTargetLock()
    {
        ////TEST#
        //return false;

        bool isLock = false;

        float distLock = Vector3.Distance(lastPositionForLock, transform.position);
        if (distLock < minDistLck)
        {
            stepTest++;
            if (stepTest > stepLimitTest)
            {
                isLock = true;
                stepTest = 0;
            }
        }
        else
        {
            stepTest = 0;
        }
        lastPositionForLock = transform.position;

        if (!isLock)
        {
            string currentField = Helper.GetNameFieldPosit(transform.position.x, transform.position.y);
            if (lastFieldForLock == currentField)
            {
                if(TimeInField == -1f)
                    TimeInField = Time.time + limitLockInField;
                if (Time.time > TimeInField)
                {
                    if (!string.IsNullOrEmpty(lastFieldForLock))
                    {
                        isLock = true;
                        TimeInField = -1f;
                    }
                }
            }else
                TimeInField = -1f;

            lastFieldForLock = currentField;
        }
        return isLock;
    }

    public static void ActionMove(ModelNPC.PersonData dataNPC)
    {
        Vector3 oldPosition = dataNPC.Position;

        float step = dataNPC.Speed + Storage.SceneDebug.SettingsScene.SpeedMovePersonInDream;// / 3; // * Time.deltaTime;
        if (step < 0.5f)
            step = 0.5f;
        if (step > 10f)
            step = 10f;

        Vector3 targetPosition = dataNPC.TargetPosition;
        Vector3 newPosition = Vector3.MoveTowards(oldPosition, dataNPC.TargetPosition, step);
        newPosition = new Vector3(newPosition.x, newPosition.y, oldPosition.z);

        string fieldOld_Name = Helper.GetNameFieldByName(dataNPC.NameObject);
        string fieldOld = Helper.GetNameFieldPosit(oldPosition.x, oldPosition.y);
        string fieldNew = Helper.GetNameFieldPosit(newPosition.x, newPosition.y);
        if (fieldOld_Name != fieldOld)
        {
            Debug.Log("###### ActionMove = Field not correct " + fieldOld_Name + " <> " + fieldOld + " >>> " + dataNPC.NameObject);
            fieldOld = fieldOld_Name;
        }

        if (!dataNPC.IsReality)
        {
            if (dataNPC.IsMoveValid())//FIX~~TRANSFER
            {
                dataNPC.StartTransfer("ActionMove");
                if (fieldOld != fieldNew) // 2* - test
                {
                    Storage.Person.UpdateGamePositionInDream(fieldOld, fieldNew, dataNPC, newPosition);
                }
                else
                {
                    //dataNPC.SetPosition(newPosition, isTestValid: false); // 2*
                    dataNPC.SetPosition(newPosition);
                }
                dataNPC.StopTransfer();
            }
        }
    }
    #endregion

    public void Kill()
    {
        Storage.Instance.AddDestroyGameObject(this.gameObject);
    }

    private void CreateViewNodelsPrefabs()
    {
        //foreach (Transform child in PanelModelsViewNPC.transform)
        //{
        //    GameObject model = child.gameObject;
        //    SaveLoadData.TypePrefabs typeModel = SaveLoadData.TypePrefabs.PrefabField;
        //    string nameModelView = child.gameObject.name.Replace("ModelView", "");
        //    try
        //    {
        //        typeModel = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), nameModelView);
        //    }
        //    catch (Exception x)
        //    {
        //        Debug.Log("########## Error ModelView " + nameModelView + " " + x.Message);
        //        continue;
        //    }
        //    Vector3 pos = new Vector3(0, 0, -1);
        //    GameObject modelView = (GameObject)Instantiate(model, pos, Quaternion.identity);
        //    modelView.name = typeModel.ToString();
        //    modelView.transform.SetParent(this.gameObject.transform);
        //    modelView.transform.position = pos;
        //    modelView.SetActive(false);
        //    m_ListViewModels.Add(typeModel, modelView);
        //}
    }

    #region Model View
    private void LoadModelsView()
    {
        //Skip prefab original
        //bool isThisPrefab = gameObject.name == "PrefabBoss" || gameObject.name == "PrefabPerson";

        //if (!isThisPrefab)
        //    return;

        if (PanelModelsViewNPC == null)
        {
            Debug.Log("###### LoadModelsView  PanelModelsViewNPC == null");
            return;
        }

        if (m_ListViewModels == null)
        {
            m_ListViewModels = new Dictionary<SaveLoadData.TypePrefabs, GameObject>();
            //if (isThisPrefab)
            //{
            //    CreateViewNodelsPrefabs();
            //}
            //else
            //{
            foreach (Transform child in transform)
            {
                GameObject modelView = child.gameObject;
                SaveLoadData.TypePrefabs typeModel = SaveLoadData.TypePrefabs.PrefabField;
                string nameModelView = modelView.name.Replace("ModelView", "");
                try
                {
                    typeModel = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), nameModelView);
                }
                catch (Exception x)
                {
                    //Debug.Log("########## Error ModelView " + nameModelView + " " + x.Message);
                    continue;
                }
                m_ListViewModels.Add(typeModel, modelView);
            }
            //}
        }
    }


    private void CheckUpdateModelView()
    {
        if (m_stateInit != IsStartInit)
        {
            m_stateInit = IsStartInit;
            m_dataNPC = m_meMovement.GetData("GameActionPersonController.Start") as ModelNPC.GameDataAlien;
            if(m_dataNPC== null)
            {
                Debug.Log("########### ERROR CheckUpdateModelView  m_dataNPC is null");
                return;
            }
            InitCurrentAction();
            bool isNewModel = temp_TypePrefab != m_dataNPC.TypePrefab; //??? Error NullReferenceException
            if (isNewModel)
            {
                //temp_dataNPC = m_dataNPC;
                temp_TypePrefab = m_dataNPC.TypePrefab;
                UpdateMeModelView();
            }
        }
    }

    public void UpdateMeModelView()
    {
        if (m_ListViewModels == null)
            return;

        SaveLoadData.TypePrefabs typeMePrefabNPC = m_dataNPC.TypePrefab;

        foreach (var itemModel in m_ListViewModels)
        {
            itemModel.Value.SetActive(itemModel.Key == typeMePrefabNPC);
            if (itemModel.Key == typeMePrefabNPC)
            {
                m_MeModelView = itemModel.Value;
                m_sortingLayer.UpdateOrderingLayer(m_MeModelView.GetComponent<Renderer>());
            }
        }

        //$$$ TEST MODEL VIEW 
        if (m_MeModelView == null)
        {
            //TEST
            var render = GetComponent<SpriteRenderer>();
            render.enabled = true;
            string modelView = m_dataNPC.ModelView;
            Sprite spriteNew = null;

            if (Storage.Person.SpriteCollection.ContainsKey(modelView))
            {
                spriteNew = Storage.Person.SpriteCollection[modelView];
            }
            else
            {
                spriteNew = Storage.Palette.SpritesPrefabs[modelView];
            }
            if (spriteNew == null)
            {
                Debug.Log("############ Not find sprite in Atlas : modelView=" + modelView);
            }
            render.sprite = spriteNew;
            return;
        }
        else
        {
            //TEST
            var render = GetComponent<SpriteRenderer>();
            render.enabled = false;
        }
        InitAnimator();
    }

    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    private void InitAnimator()
    {
        var meAnimator = m_MeModelView.GetComponent<Animator>();
        if (meAnimator.enabled)
        {
            m_MeRender = m_MeModelView.GetComponent<SpriteRenderer>();
            //if (m_MeRender == null)
            //{
            //    Debug.Log("###### GameActionPersonController.UpdateMeModelView  m_MeRender == null");
            //    return;
            //}
            if (m_MeRender != null && m_MeRender.enabled)
                m_MeAnimation = new PlayerAnimation(meAnimator, m_MeRender);
            else
                m_MeAnimation = new PlayerAnimation(meAnimator, m_MeModelView);
        }
        else
        {
            Animator backAnimator = null;
            Animator frontAnimator = null;
            foreach (Transform child in m_MeModelView.transform)
            {
                GameObject modelAnimation = child.gameObject;
                if (modelAnimation.tag == "BackAnimationModel")
                {
                    backAnimator = modelAnimation.GetComponent<Animator>();
                    renderBack = modelAnimation.GetComponent<SpriteRenderer>();
                }
                if (modelAnimation.tag == "FrontAnimationModel")
                {
                    frontAnimator = modelAnimation.GetComponent<Animator>();
                    renderFront = modelAnimation.GetComponent<SpriteRenderer>();
                }
            }
            if (backAnimator != null && frontAnimator != null && renderBack != null && renderFront != null)
            {
                m_MeAnimation = new PlayerAnimation(backAnimator, frontAnimator, renderBack, renderFront);
                m_sortingLayer.UpdateOrderingLayer(renderBack, renderFront);
            }
        }
    }
    #endregion

    public void RayTargetClear()
    {
        m_lineRenderer.positionCount = 0;
    }

    private void DrawRayTarget()
    {
        if (Storage.SceneDebug.SettingsScene.IsShowTittleInfoPerson)
        {
            if (m_dataNPC != null && m_dataNPC.TargetPosition != Vector3.zero)
            {
                RayTargetClear();
                var pointsRay = new Vector2[]{
                    transform.position,
                    m_dataNPC.TargetPosition
                };
                //DrawPolyline(pointsRay, "#0000ff".ToColor(), 1f);
                DrawPolyline(pointsRay, Storage.Palette.DrawPolylineColor, 0.1f);
            }
        }
    }

    public void DrawPolyline(Vector2[] points, Color color, float width = 0.2f)
    {
        //return;

        if (m_lineRenderer == null)
        {
            Debug.Log("LineRenderer is null !!!!");
            return;
        }

        var colorKeys = new GradientColorKey[] { new GradientColorKey() { color = color } };
        //m_lineRenderer.SetColors(color, color);
        m_lineRenderer.colorGradient = new Gradient() { colorKeys = colorKeys };
        m_lineRenderer.SetWidth(width, width);
        int size = points.Length;
        m_lineRenderer.SetVertexCount(size);
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 posPoint = new Vector3(points[i].x, points[i].y, -2);
            m_lineRenderer.SetPosition(i, posPoint);
        }
    }
}
