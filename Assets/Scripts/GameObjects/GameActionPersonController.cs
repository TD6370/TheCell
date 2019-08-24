using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameActionPersonController : MonoBehaviour
{

    public enum NameActionsPerson
    {
        None, Idle, Move, Dead, Work, Attack, Completion
    };

    public static bool IsGameActionPersons = true;
    public bool IsStartInit = false;
    private bool m_stateInit = false;

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

    public static float TimeWaitIdle = 5000f;
    public static float MinDistEndMove = 1f;
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

    //private void LateUpdate()
    //{
    //    if (IsStartInit)
    //        ChangeActions();
    //}

    public void ChangeActions()
    {
        if (m_meMovement == null)
            return;

        

        CheckUpdateModelView();
        

        //CheckComplitionActions(m_dataNPC, ActionPerson, this);

        CheckNextAction(m_dataNPC, ActionPerson, this);
    }

    public void StartInit()
    {
        m_stateInit = false;
        IsStartInit = true;
        TimeInField = Time.time + limitLockInField;
    }

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
            } else
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
            return NameActionsPerson.None;
        }
        return (NameActionsPerson)Enum.Parse(typeof(NameActionsPerson), dataNPC.CurrentAction); ;
    }
        
    public static void CheckNextAction(ModelNPC.PersonData dataNPC, NameActionsPerson p_actionPerson, GameActionPersonController controller)
    {
        CheckComplitionActions(dataNPC, p_actionPerson, controller);

        var listPersonActions = GetActions(dataNPC);

        if (listPersonActions.Count == 0)
            AddActionNPC(dataNPC, NameActionsPerson.Move);

        listPersonActions = GetActions(dataNPC);

        if (listPersonActions.Count > 0)
        {
            var actionPerson = listPersonActions[0];
            listPersonActions.RemoveAt(0);
            dataNPC.PersonActions = listPersonActions.Select(p => p.ToString()).ToArray();// (p=>p.).ToArray();
            StartActionNPC(dataNPC, actionPerson, controller);
        }
    }

    public static void CheckComplitionActions(ModelNPC.PersonData dataNPC, NameActionsPerson actionPerson, GameActionPersonController controller)
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
            case NameActionsPerson.Attack:
            case NameActionsPerson.Dead:
            case NameActionsPerson.Completion:
            case NameActionsPerson.Work:
                break;
        }
        
    }

    public static void RequestActionNPC(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction, GameActionPersonController controller, bool isForce = false)
    {
        if (isForce)
            controller.ResetAction();
        AddActionNPC(dataNPC, p_nameAction);
        if(controller!=null)
            controller.ActionPerson = p_nameAction;
        else
            dataNPC.CurrentAction = p_nameAction.ToString();
    }

    public void ResetAction()
    {
        temp_ActionPerson = NameActionsPerson.None;
    }

    public  static void AddActionNPC(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction = NameActionsPerson.None)
    {
        var m_ListPersonActions = GetActions(dataNPC);
        if (m_ListPersonActions.Count > 0)
        {
            var lastAction = m_ListPersonActions[m_ListPersonActions.Count - 1];
            if (lastAction == p_nameAction)
            {
                // repeat
                return;
            }
        }
        m_ListPersonActions.Add(p_nameAction);
        dataNPC.PersonActions = m_ListPersonActions.Select(p => p.ToString()).ToArray();
    }

    public void SetAction(NameActionsPerson p_nameAction)
    {
        ActionPerson = p_nameAction;
    }

    public static void StartActionNPC(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction, GameActionPersonController controller)
    {
        if (p_nameAction != NameActionsPerson.None)
        {
            if (controller != null)
                controller.SetAction(p_nameAction);
        }
        //m_ActionPerson = p_nameAction;

        if (controller != null)
            dataNPC.CurrentAction = controller.ActionPerson.ToString();
        else
            dataNPC.CurrentAction = p_nameAction.ToString();
        //else
        //    dataNPC.CurrentAction = controller.ActionPerson.ToString();

        switch (p_nameAction)
        {
            case NameActionsPerson.Idle:
                ActionIdle(dataNPC);
                break;
            case NameActionsPerson.Move:
                if (controller == null)
                    ActionMove(dataNPC);
                break;
            case NameActionsPerson.Completion:
                ActionNewTargetMove(dataNPC, controller);
                break;
            case NameActionsPerson.Dead:
                break;
            case NameActionsPerson.Attack:
                break;
            case NameActionsPerson.Work:
                break;
        }
    }

    public static void CheckComplitionIdle(ModelNPC.PersonData dataNPC, NameActionsPerson p_nameAction, GameActionPersonController controller)
    {
        if (Time.time > (dataNPC as ModelNPC.GameDataAlien).TimeEndCurrentAction && p_nameAction == NameActionsPerson.Idle)
        {
            RequestActionNPC(dataNPC, NameActionsPerson.Move, controller);
        }
    }

    private int stepTest=0;
    private int stepLimitTest = 10;
    private string lastFieldForLock = "";
    private float limitLockInField = 3f;
    private float TimeInField;// = Time.time + limitLockInField;
    private float minDistLck = 0.005f;
    private Vector3 lastPositionForLock;

    public static void CheckComplitionMoveInDream(ModelNPC.PersonData dataNPC)
    {
        Vector3 newCurrentPosition = GetAlienData(dataNPC).MovePosition;
        float dist = Vector3.Distance(dataNPC.TargetPosition, newCurrentPosition);
        string nameFiledTarget = Helper.GetNameField(dataNPC.TargetPosition);
        string nameFiledCurrent = Helper.GetNameField(newCurrentPosition);

        //End move to Target
        bool trueDist = dist < MinDistEndMove;
        bool trueField = dist < MinDistEndMove;

        if (trueDist || trueField)  
        {
            //TEST
            //Storage.EventsUI.ListLogAdd = "..work INFO: " + dataNPC.NameObject + " is Move END DIST:" + trueDist + " FIELD:" + trueField;
            RequestActionNPC(dataNPC, NameActionsPerson.Completion, null);
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

        TestMoveTargetLock();

        float dist = Vector3.Distance(targetPosition, transform.position);
        if (dist < MinDistEndMove)
        {
            RequestActionNPC(m_dataNPC, NameActionsPerson.Completion, this);
        }

        if (m_MeAnimation!=null)
            m_MeAnimation.PersonMove(ActionPerson == NameActionsPerson.Move);
    }

    private void TestMoveTargetLock()
    {
        stepTest++;
        if (stepTest > stepLimitTest)
        {
            float distLock = Vector3.Distance(lastPositionForLock, transform.position);
            if (distLock < minDistLck)
            {
                RequestActionNPC(m_dataNPC, NameActionsPerson.Completion, this);
                //Debug.Log("~~~ New Taget 1 " + this.gameObject.name);
            }
            lastPositionForLock = transform.position;
            stepTest = 0;
        }
        if (Time.time > TimeInField && lastFieldForLock != Storage.Instance.SelectFieldPosHero)
        {
            //Debug.Log("......... I AM LOCK IN FIELD : " + lastFieldForLock + "  " + this.name);
            if (!string.IsNullOrEmpty(lastFieldForLock))
            {
                RequestActionNPC(m_dataNPC, NameActionsPerson.Completion, this);
                //Debug.Log("~~~ New Taget 2 " + this.gameObject.name);
            }
            lastFieldForLock = Storage.Instance.SelectFieldPosHero;
            TimeInField = Time.time + limitLockInField;
        }

    }

    public static void ActionNewTargetMove(ModelNPC.PersonData dataNPC, GameActionPersonController controller)
    {
        dataNPC.SetTargetPosition();
        RequestActionNPC(dataNPC, NameActionsPerson.Move, controller);
    }

    public static ModelNPC.GameDataAlien GetAlienData(ModelNPC.PersonData dataNPC)
    {
        return dataNPC as ModelNPC.GameDataAlien;
    }

    public static void ActionIdle(ModelNPC.PersonData dataNPC)
    {
        GetAlienData(dataNPC).TimeEndCurrentAction = Time.time + TimeWaitIdle;
    }

    public static void ActionMove(ModelNPC.PersonData dataNPC)
    {
        Vector3 oldPosition = dataNPC.Position;
        
        //(dataNPC.Position, dataNPC.TargetPosition);
        //float dist = Vector3.Distance(dataNPC.TargetPosition, newCurrentPosition);
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
        if(fieldOld_Name != fieldOld)
        {
            Debug.Log("###### ActionMove = Field not correct " + fieldOld_Name + " <> " + fieldOld + " >>> " + dataNPC.NameObject);
            fieldOld = fieldOld_Name;
        }

        if (fieldOld != fieldNew)
        {
            Storage.Person.UpdateGamePositionInDream(fieldOld, fieldNew, dataNPC, newPosition);
        }else
        {
            dataNPC.SetPosition(newPosition);
        }
        //GetAlienData(dataNPC).MovePosition = newPosition;
    }

    private void ActionDead()
    {
    }

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
}
