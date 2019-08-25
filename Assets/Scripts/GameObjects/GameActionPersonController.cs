using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameActionPersonController : MonoBehaviour
{

    public enum NameActionsPerson
    {
        None, Idle, Move, Dead, Work, Attack, Completed //, Completion
    };

    public static bool IsGameActionPersons = true;
    public bool IsStartInit = false;
    private bool m_stateInit = false;
    private bool m_isMeHero = false;

    public GameObject PanelModelsViewNPC;
    public PlayerAnimation m_MeAnimation;

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
    //public static float MinDistEndMove = 1f;
    public static float MinDistEndMove = 0.8f;
    public NameActionsPerson ActionPerson = NameActionsPerson.None;

    private SpriteRenderer m_MeRender;
    private Vector2 m_MeMovement;
    private NameActionsPerson temp_ActionPerson = NameActionsPerson.None;
    private ModelNPC.PersonData m_dataNPC;
    private MovementBoss m_meMovement;
    private Dictionary<SaveLoadData.TypePrefabs, GameObject> m_ListViewModels;
    private ModelNPC.PersonData temp_dataNPC;
    private SaveLoadData.TypePrefabs temp_TypePrefab = SaveLoadData.TypePrefabs.PrefabField;
    private PositionRenderSorting m_sortiongLayer;

    private void Awake()
    {
        m_sortiongLayer = GetComponent<PositionRenderSorting>();

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

    public void ChangeActions()
    {
        if (m_meMovement == null)
            return;

        CheckUpdateModelView();
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
    public static NameActionsPerson GetCurrentAction(ModelNPC.PersonData dataNPC)
    {
        if(string.IsNullOrEmpty(dataNPC.CurrentAction))
        {
            Debug.Log("####### GetCurrentAction dataNPC.CurrentAction is null");
            dataNPC.CurrentAction = NameActionsPerson.Idle.ToString();
        }
        return (NameActionsPerson)Enum.Parse(typeof(NameActionsPerson), dataNPC.CurrentAction); ;
    }
    #endregion

    public static void CheckNextAction(ModelNPC.PersonData dataNPC, NameActionsPerson p_actionPerson, GameActionPersonController controller)
    {
        CheckCompletionActions(dataNPC, p_actionPerson, controller);

        var listPersonActions = GetActions(dataNPC);
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

    public static void CheckCompletionActions(ModelNPC.PersonData dataNPC, NameActionsPerson actionPerson, GameActionPersonController controller)
    {
        switch (actionPerson)
        {
            case NameActionsPerson.Idle:
                CheckComplitionIdle(dataNPC, actionPerson, controller);
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
            case NameActionsPerson.Attack:
            case NameActionsPerson.Dead:
            case NameActionsPerson.Completed:
            case NameActionsPerson.Work:
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
                break;
        }
    }

    public static void RequestActionNPC(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction, GameActionPersonController controller, bool isForce = false)
    {
        if (isForce)
            controller.ResetAction();

        AddActionNPC(dataNPC, p_nameAction);
        
        if (controller != null)
            controller.ActionPerson = NameActionsPerson.Completed;

        dataNPC.CurrentAction = NameActionsPerson.Completed.ToString();
    }
    

    public static void AddActionNPC(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction = NameActionsPerson.None)
    {
        var m_ListPersonActions = GetActions(dataNPC);
        if (m_ListPersonActions.Count > 0)
        {
            var lastAction = m_ListPersonActions[m_ListPersonActions.Count - 1];
            if (lastAction == p_nameAction) // repeat
                return;
        }
        m_ListPersonActions.Add(p_nameAction);
        dataNPC.PersonActions = m_ListPersonActions.Select(p => p.ToString()).ToArray();
    }

    public void SetAction(NameActionsPerson p_nameAction)
    {
        ActionPerson = p_nameAction;
    }

    #region Checked and run actions

    public static void CheckComplitionIdle(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction, GameActionPersonController controller)
    {
        float timeWait = (dataNPC as ModelNPC.GameDataAlien).TimeEndCurrentAction;
        if (Time.time > timeWait && p_nameAction == NameActionsPerson.Idle)
        {
            RequestActionNPC(dataNPC, NameActionsPerson.Completed, controller);
        }
    }

    public static void ActionTarget(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        dataNPC.SetTargetPosition();
        RequestActionNPC(dataNPC, NameActionsPerson.Move, controller);
    }

    public static bool ActionIdle(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        float tilme = GetAlienData(dataNPC).TimeEndCurrentAction;
        if(tilme == -1)
            GetAlienData(dataNPC).TimeEndCurrentAction = Time.time + TimeWaitIdle;
        return false;
    }

    private void ActionDead()
    {
    }

    #endregion

    #region Action Move

    private int stepTest = 0;
    private int stepLimitTest = 10;
    private string lastFieldForLock = "";
    private float limitLockInField = 3f;
    private float TimeInField;
    private float minDistLck = 0.0005f;
    private Vector3 lastPositionForLock;

    public static void CheckComplitionMoveInDream(ModelNPC.PersonData dataNPC)
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
            RequestActionNPC(dataNPC, NameActionsPerson.Idle, null);
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
                m_MeAnimation.PersonLook(isRight);
            }
        }

        bool isCompletedMoving = false;
        isCompletedMoving = TestMoveTargetLock();
        if (!isCompletedMoving)
        {
            float dist = Vector3.Distance(targetPosition, transform.position);
            if (dist < MinDistEndMove)
            {
                RequestActionNPC(m_dataNPC, NameActionsPerson.Idle, this);
                isCompletedMoving = true;
            }
        }

        bool isAnimateMove = (ActionPerson == NameActionsPerson.Move) && isCompletedMoving == false;

        if (m_MeAnimation != null)
            m_MeAnimation.PersonMove(isAnimateMove);
    }

    private bool TestMoveTargetLock()
    {
        bool isLock = false;

        float distLock = Vector3.Distance(lastPositionForLock, transform.position);
        if (distLock < minDistLck)
        {
            stepTest++;
            if (stepTest > stepLimitTest)
            {
                RequestActionNPC(m_dataNPC, NameActionsPerson.Idle, this);
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
                        RequestActionNPC(m_dataNPC, NameActionsPerson.Idle, this);
                        isLock = true;
                        TimeInField = -1f;
                    }
                }
            }else
                TimeInField = -1f;

            lastFieldForLock = currentField;
            //TimeInField = Time.time + limitLockInField;
        }
        return isLock;
    }

    public static void ActionMove(ModelNPC.PersonData dataNPC)
    {
        Vector3 oldPosition = dataNPC.Position;

        float step = dataNPC.Speed + Storage.Person.SpeedMovePersonInDream;// / 3; // * Time.deltaTime;
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

        if (fieldOld != fieldNew)
        {
            Storage.Person.UpdateGamePositionInDream(fieldOld, fieldNew, dataNPC, newPosition);
        }
        else
        {
            dataNPC.SetPosition(newPosition);
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
        bool isThisPrefab = gameObject.name == "PrefabBoss" || gameObject.name == "PrefabPerson";

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
            if (isThisPrefab)
            {
                CreateViewNodelsPrefabs();
            }
            else
            {
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
                        Debug.Log("########## Error ModelView " + nameModelView + " " + x.Message);
                        continue;
                    }
                    m_ListViewModels.Add(typeModel, modelView);
                }
            }
        }
    }


    private void CheckUpdateModelView()
    {
        if (m_stateInit != IsStartInit)
        {
            m_stateInit = IsStartInit;
            m_dataNPC = m_meMovement.GetData("GameActionPersonController.Start") as ModelNPC.PersonData;
            InitCurrentAction();
            bool isNewModel = temp_TypePrefab != m_dataNPC.TypePrefab;
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
                m_sortiongLayer.UpdateOrderingLayer(m_MeModelView.GetComponent<Renderer>());
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

        m_MeRender = m_MeModelView.GetComponent<SpriteRenderer>();
        if (m_MeRender == null)
        {
            Debug.Log("###### GameActionPersonController.UpdateMeModelView  m_MeRender == null");
            return;
        }

        var meAnimator = m_MeModelView.GetComponent<Animator>();
        m_MeAnimation = new PlayerAnimation(meAnimator, m_MeRender);
    }
    #endregion
}
