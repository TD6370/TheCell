using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    public enum AnimationState { None, Run , Completed };

    private GameObjecDataController m_dataController;
    private ModelNPC.PortalData m_DataPortal;
    private bool m_isInit = false;

    private Dictionary<SaveLoadData.TypePrefabs, GameObject> m_ListViewModels;
    private PositionRenderSorting m_sortingLayer;

    private GameObject m_MeModelView;
    public GameObject ModelView
    {
        get { return m_MeModelView; }
    }

    private string temp_name = "";
    private SaveLoadData.TypePrefabs temp_TypePrefab;
    private Animator m_animator;
    private AnimationState m_stateAnimation = AnimationState.None;

    private void Awake()
    {
        if (name == "PrefabPortalBase")
        {
            m_isInit = true;
            gameObject.SetActive(false);
            return;
        }
        m_dataController = gameObject.GetDataController();

        m_sortingLayer = GetComponent<PositionRenderSorting>();
        if (m_sortingLayer == null)
            Debug.Log("####### PortalController.m_sortingLayer is Empty");

        
        
        
        LoadModelsView();
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
    
    private void LateUpdate()
    {
        if(!m_isInit)
        {
            m_DataPortal = GetUpdateData();
            m_isInit = true;
        }

        if (m_isInit)
        {
            CheckUpdateModelView();
            CheckUpdateStatusProcess();
            //if (m_stateAnimation == AnimationState.Run)
            //{
            //    //AnimationClip moveClip = m_animator.runtimeAnimatorController.animationClips[1];
            //    AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(0);
                
            //    if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("PortalBlueEnterAnimation"))
            //        m_stateAnimation = AnimationState.Completed;
            //    if(state.speed==0)
            //        m_stateAnimation = AnimationState.Completed;
            //    //PlayIncubation(true);
            //}
        }
    }

    void OnGUI()
    {
        if (Storage.SceneDebug.SettingsScene.IsShowTittleInfoPerson)
        {
            if (m_DataPortal == null)
                return;

            string objID = m_DataPortal.Id; // ModelView
            if (objID == null)
                return;

            Color tittleColor = "#FFE881".ToColor();

            string animationInfo = "\n Play : " + m_stateAnimation.ToString();
            string resourcesList = "";
            foreach (var item in m_DataPortal.Resources)
            {
                resourcesList += "\n" + item;
            }
            if (m_DataPortal.Resources.Count > 0)
                resourcesList = "\n  Resources : " + resourcesList;

            Color colorGreen = "#9FFF00".ToColor();
            Color colorBlue = "#8BD6FF".ToColor();
            Color colorOrange = "#FFD600".ToColor();
            string messageInfo =
                "BIOM  :" + m_DataPortal.TypeBiom
                + animationInfo 
                + resourcesList;

            GUIStyle style = new GUIStyle();
            style.fontSize = 16;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = tittleColor; // Color.yellow;

            if (Camera.main == null)
                return;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            Rect positionRect = new Rect(screenPosition.x - 10, Screen.height - screenPosition.y - 200, 400, 100);
            GUI.Label(positionRect, messageInfo, style);
        }
    }

    public void IncubationCompleted()
    {
        if (m_stateAnimation == AnimationState.Run)
        {
            m_stateAnimation = AnimationState.Completed;
        }
    }

    protected ModelNPC.PortalData GetUpdateData(string callInfo = "PortalController.GetUpdateData")
    {
        var m_DataPortal = m_dataController.UpdateData(callInfo) as ModelNPC.PortalData;
        return m_DataPortal;
    }

    #region Model View
    private void LoadModelsView()
    {
        if (m_ListViewModels == null)
        {
            m_ListViewModels = new Dictionary<SaveLoadData.TypePrefabs, GameObject>();

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
        
    private void CheckUpdateModelView()
    {
        if (temp_name != name)
        {
            temp_name = name;
            m_DataPortal = GetUpdateData();
            if (m_DataPortal == null)
            {
                Debug.Log("########### ERROR CheckUpdateModelView  m_DataPortal is null");
                return;
            }
            bool isNewModel = temp_TypePrefab != m_DataPortal.TypePrefab;
            if (isNewModel)
            {
                temp_TypePrefab = m_DataPortal.TypePrefab;
                UpdateMeModelView();
            }
        }
               
    }

    private void CheckUpdateStatusProcess()
    {
        Storage.Portals.CurrentAnimationState = m_MeModelView.name + " : " + m_stateAnimation.ToString();
        //Check on process
        switch (m_DataPortal.CurrentProcess)
        {
            case ManagerPortals.TypeResourceProcess.Incubation:
                if (m_stateAnimation == AnimationState.None)
                {
                    PlayIncubation(true);
                    m_stateAnimation = AnimationState.Run;
                }
                else if (m_stateAnimation == AnimationState.Completed)
                {
                    ManagerPortals.IncubationProcess(m_DataPortal, isCallFromReality: true);
                    m_stateAnimation = AnimationState.None;
                }
                break;
        }
    }

    public void UpdateMeModelView()
    {
        if (m_ListViewModels == null)
            return;

        SaveLoadData.TypePrefabs typeMePrefab = m_DataPortal.TypePrefab;
        foreach (var itemModel in m_ListViewModels)
        {
            itemModel.Value.SetActive(itemModel.Key == typeMePrefab);
            if (itemModel.Key == typeMePrefab)
            {
                m_MeModelView = itemModel.Value;
                m_sortingLayer.UpdateOrderingLayer(m_MeModelView.GetComponent<Renderer>());
                m_animator = m_MeModelView.GetComponent<Animator>();
                if (m_animator == null)
                    Debug.Log("####### PortalController.m_animator is Empty");
            }
        }
               
    }
    #endregion

    #region Animation
    public void PlayIncubation(bool isPlay)
    {
        m_animator.SetBool("TriggerIncubation", isPlay);
    }
    #endregion

}
