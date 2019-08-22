using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameActionPersonController : MonoBehaviour
{

    public enum NameActionsPerson
    {
        None, Idle, Move, Dead, Work, Attack, MoveEnd
    };

    public static bool IsGameActionPersons = true;
    public bool IsStartInit = false;
    private bool m_stateInit = false;

    public GameObject PanelModelsViewNPC;

    private GameObject m_MeModelView;
    public GameObject ModelView
    {
        get { return m_MeModelView; }
    }

    public PlayerAnimation m_MeAnimation;
    private SpriteRenderer m_MeRender;
    private Vector2 m_MeMovement;
    private NameActionsPerson m_ActionPerson = NameActionsPerson.None;
    private NameActionsPerson temp_ActionPerson = NameActionsPerson.None;
    private ModelNPC.PersonData m_dataNPC;
    private MovementBoss m_meMovement;
    private Dictionary<SaveLoadData.TypePrefabs, GameObject> m_ListViewModels;
    private ModelNPC.PersonData temp_dataNPC;
    private SaveLoadData.TypePrefabs temp_TypePrefab = SaveLoadData.TypePrefabs.PrefabField;
    private PositionRenderSorting m_sortiongLayer;
    private float m_timeWaitIdle = 5000f;
    private float m_timeEndIdle;

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

        if(IsStartInit)
            ChangeActions();
    }

    public void ChangeActions()
    {
        if (m_meMovement == null)
            return;

        CheckUpdateModelView();
        
        AnimatorMove();

        CheckNextAction();
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
            if(spriteNew == null)
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

    private List<NameActionsPerson> GetActions()
    {
        var ListPersonActions = new List<NameActionsPerson>();
        if (m_dataNPC.PersonActions == null)
            m_dataNPC.PersonActions = new string[] { };

        for (int i=0; i< m_dataNPC.PersonActions.Length; i++)
        {
            NameActionsPerson nextActon = (NameActionsPerson)Enum.Parse(typeof(NameActionsPerson), m_dataNPC.PersonActions[i].ToString());;
            ListPersonActions.Add(nextActon);
        }
        return ListPersonActions;
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

    private void InitCurrentAction()
    {
        if (temp_ActionPerson == NameActionsPerson.None && temp_ActionPerson == m_ActionPerson && !string.IsNullOrEmpty(m_dataNPC.CurrentAction))
        {
            temp_ActionPerson = m_ActionPerson = (NameActionsPerson)Enum.Parse(typeof(NameActionsPerson), m_dataNPC.CurrentAction.ToString());
            StartActionNPC(m_ActionPerson);
        }
    }


    private void CheckNextAction()
    {
        InitCurrentAction();

        if (Time.time > m_timeEndIdle && m_ActionPerson == NameActionsPerson.Idle)
            RequestActionNPC(NameActionsPerson.Move);

        if (temp_ActionPerson != m_ActionPerson)
        {
            var m_ListPersonActions = GetActions();

            if (m_ListPersonActions.Count == 0)
                RequestActionNPC(NameActionsPerson.Move);

            if (m_ListPersonActions.Count > 0)
            {
                m_ActionPerson = m_ListPersonActions[0];
                m_ListPersonActions.RemoveAt(0);
                m_dataNPC.PersonActions = m_ListPersonActions.Select(p => p.ToString()).ToArray();// (p=>p.).ToArray();
                StartActionNPC(m_ActionPerson);
                temp_ActionPerson = m_ActionPerson;
            }
        }
    }

    public void RequestActionNPC(NameActionsPerson p_nameAction = NameActionsPerson.None, bool isForce = false)
    {
        if (isForce)
            temp_ActionPerson = NameActionsPerson.None;
        AddActionNPC(p_nameAction);
        m_ActionPerson = p_nameAction;
    }

    public void AddActionNPC(NameActionsPerson p_nameAction = NameActionsPerson.None)
    {
        var m_ListPersonActions = GetActions();
        if (m_ListPersonActions.Count > 0 )
        {
            var lastAction = m_ListPersonActions[m_ListPersonActions.Count - 1];
            if (lastAction == p_nameAction)
            {
                // repeat
                return;
            }
        }
        m_ListPersonActions.Add(p_nameAction);
        m_dataNPC.PersonActions = m_ListPersonActions.Select(p => p.ToString()).ToArray();
    }
       

    private void StartActionNPC(NameActionsPerson p_nameAction = NameActionsPerson.None)
    {
        if (p_nameAction != NameActionsPerson.None)
            m_ActionPerson = p_nameAction;
        m_dataNPC.CurrentAction = m_ActionPerson.ToString();

        switch (m_ActionPerson)
        {
            case NameActionsPerson.Idle:
                ActionIdle();
                break;
            case NameActionsPerson.Move:
                break;
            case NameActionsPerson.MoveEnd:
                ActionNewTargetMove();
                break;
            case NameActionsPerson.Dead:
                break;
            case NameActionsPerson.Attack:
                break;
            case NameActionsPerson.Work:
                break;
        }
    }
       

    private int stepTest=0;
    private int stepLimitTest = 10;
    private string lastFieldForLock = "";
    private float limitLockInField = 3f;
    private float TimeInField;// = Time.time + limitLockInField;
    private float minDistLck = 0.005f;
    private Vector3 lastPositionForLock;

    private void AnimatorMove()
    {
        //$$$$
        //return;

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

        //$$$ ---------
        TestMoveTargetLock();


        float minDist = 1f;
        float dist = Vector3.Distance(targetPosition, transform.position);
        if (dist < minDist)
        {
            RequestActionNPC(NameActionsPerson.MoveEnd);
        }
        //---------------

        if (m_MeAnimation!=null)
            m_MeAnimation.PersonMove(m_ActionPerson == NameActionsPerson.Move);
    }

    private void TestMoveTargetLock()
    {
        stepTest++;
        if (stepTest > stepLimitTest)
        {
            float distLock = Vector3.Distance(lastPositionForLock, transform.position);
            if (distLock < minDistLck)
            {
                RequestActionNPC(NameActionsPerson.MoveEnd);
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
                RequestActionNPC(NameActionsPerson.MoveEnd);
                //Debug.Log("~~~ New Taget 2 " + this.gameObject.name);
            }
            lastFieldForLock = Storage.Instance.SelectFieldPosHero;
            TimeInField = Time.time + limitLockInField;
        }

    }


    private void ActionNewTargetMove()
    {
        m_dataNPC.SetTargetPosition();
        RequestActionNPC(NameActionsPerson.Move);
    }

    private void ActionIdle()
    {
        m_timeEndIdle = Time.time + m_timeWaitIdle;
        //m_dataNPC.SetTargetPosition();
    }

    private void ActionMove()
    {
        //m_dataNPC.SetTargetPosition();
    }

    private void ActionDead()
    {
        //m_dataNPC.SetTargetPosition();
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
