using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameActionPersonController : MonoBehaviour
{
    public static bool IsGameActionPersons = true;

    public bool IsStartInit = false;


    public GameObject PanelModelsViewNPC;
    private GameObject m_MeModelView;
    public GameObject ModelView
    {
        get { return m_MeModelView; }
    }

    public PlayerAnimation m_MeAnimation;
    private SpriteRenderer m_MeRender;
    private Vector2 m_MeMovement;
    private NameActionsPerson m_ActionPerson = NameActionsPerson.Move;
    private NameActionsPerson temp_ActionPerson = NameActionsPerson.None;
    private ModelNPC.PersonData m_dataNPC;
    //private List<NameActionsPerson> m_ListPersonActions;
    private MovementBoss m_meMovement;
    private Dictionary<SaveLoadData.TypePrefabs, GameObject> m_ListViewModels;
    private ModelNPC.PersonData temp_dataNPC;
    private SaveLoadData.TypePrefabs temp_TypePrefab = SaveLoadData.TypePrefabs.PrefabField;

    public enum NameActionsPerson
    {
        None, Idle, Move, Dead, Work, Attack, MoveEnd
    };

    private void Awake()
    {
        LoadModelsView();
    }

    // Use this for initialization
    void Start()
    {
        //LoadModelsView();
        //m_MeRender = gameObject.GetComponent<SpriteRenderer>();
        //if (m_MeRender == null)
        //    Debug.Log("###### GameActionPersonController.Start  m_MeRender == null");

        m_meMovement = gameObject.GetComponent<MovementBoss>();
        if (m_meMovement == null)
            Debug.Log("###### GameActionPersonController.Start  m_meMovement == null");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_meMovement == null)
            return;

        if(IsStartInit)
            ChangeActions();
    }

    public void ChangeActions()
    {
        //TEST $$$
        //if (m_MeModelView != null)
        //    return;

        if (m_meMovement == null)
            return;

        m_dataNPC = m_meMovement.GetData("GameActionPersonController.Start") as ModelNPC.PersonData;
        //bool isNewModel = temp_dataNPC == null || temp_dataNPC.Equals(m_dataNPC);
        bool isNewModel = temp_TypePrefab != m_dataNPC.TypePrefab;
        if (isNewModel)
        {
            //temp_dataNPC = m_dataNPC;
            temp_TypePrefab = m_dataNPC.TypePrefab;
            UpdateMeModelView();
        }
        //ActivateMeModelView();
        //TestNextAction(); //$$$
        AnimatorMove();

        //FixPositModelView();
    }

    private void FixPositModelView()
    {
        //var posDef = new Vector3(0, 0, -1);
        //if (m_MeModelView != null)
        //{
        //    if (m_MeModelView.transform.position != posDef)
        //    {
        //        m_MeModelView.transform.position = posDef;
        //    }
        //}
    }

    private void LoadModelsView()
    {
        //bool isThisPrefab = gameObject.name == "PrefabBoss" || gameObject.tag.ToString() == "PoolPerson";
        bool isThisPrefab = gameObject.name == "PrefabBoss" || gameObject.tag.ToString() == "PoolPerson";
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

    private void UpdateMeModelView()
    {
        if (m_ListViewModels == null)
            return;

        SaveLoadData.TypePrefabs typeMePrefabNPC = m_dataNPC.TypePrefab;
        foreach(var itemModel in m_ListViewModels)
        {
            itemModel.Value.SetActive(itemModel.Key == typeMePrefabNPC);
            if (itemModel.Key == typeMePrefabNPC)
                m_MeModelView = itemModel.Value;
        }

        //$$$ TEST MODEL VIEW 
        if (m_MeModelView == null)
        {
            //m_MeModelView = m_ListViewModels[SaveLoadData.TypePrefabs.Lollipop];
            foreach (GameObject itemModel in m_ListViewModels.Values)
            {
                m_MeModelView = itemModel;
                break;
            }
            m_MeModelView.SetActive(true);
            var render = GetComponent<SpriteRenderer>();
            render.enabled = false;
        }

        m_MeRender = m_MeModelView.GetComponent<SpriteRenderer>();
        if (m_MeRender == null)
            Debug.Log("###### GameActionPersonController.UpdateMeModelView  m_MeRender == null");

        var meAnimator = m_MeModelView.GetComponent<Animator>();
        m_MeAnimation = new PlayerAnimation(meAnimator, m_MeRender);

        //m_MeModelView.transform.position = new Vector3(0, 0, -1);
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

    private void TestNextAction()
    {
        if (temp_ActionPerson != m_ActionPerson)
        {
            var m_ListPersonActions = GetActions();
            if (m_ListPersonActions.Count > 0)
            {
                //if (m_ActionPerson == m_ListPersonActions[0])
                //{

                m_ActionPerson = m_ListPersonActions[0];
                m_ListPersonActions.RemoveAt(0);
                m_dataNPC.PersonActions = m_ListPersonActions.Select(p => p.ToString()).ToArray();// (p=>p.).ToArray();
                //}
                StartActionNPC(m_ActionPerson);
                temp_ActionPerson = m_ActionPerson;
            }
        }
    }

    public void ExecuteActionNPC(NameActionsPerson p_nameAction = NameActionsPerson.None, bool isForce = false)
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

        switch (m_ActionPerson)
        {
            case NameActionsPerson.Idle:
                break;
            case NameActionsPerson.Move:
                break;
            case NameActionsPerson.MoveEnd:
                SetActionNewTargetMove();
                break;
            case NameActionsPerson.Dead:
                break;
            case NameActionsPerson.Attack:
                break;
            case NameActionsPerson.Work:
                break;
        }
    }
       

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
        //float minDist = 1f;
        //float dist = Vector3.Distance(targetPosition, transform.position);
        //if (dist < minDist)
        //{
        //    ExecuteActionNPC(NameActionsPerson.MoveEnd);
        //}
        //---------------

        if (m_MeAnimation!=null)
            m_MeAnimation.PersonMove(m_ActionPerson == NameActionsPerson.Move);
    }

    
    private void SetActionNewTargetMove()
    {
        m_dataNPC.SetTargetPosition();
    }

    private void SetActionIdle()
    {
        //m_dataNPC.SetTargetPosition();
    }

    private void SetActionMove()
    {
        //m_dataNPC.SetTargetPosition();
    }

    private void SetActionDead()
    {
        //m_dataNPC.SetTargetPosition();
    }
}
