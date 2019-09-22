using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    public GameObject PanelModelsView;
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

    private void Awake()
    {
        if (name == "PrefabPortaBase")
        {
            m_isInit = true;
            gameObject.SetActive(false);
            return;
        }
        m_dataController = gameObject.GetDataController();

        if (PanelModelsView == null)
            Debug.Log("####### PrefabPortaBase is Empty");

        m_sortingLayer = GetComponent<PositionRenderSorting>();
        if (m_sortingLayer == null)
            Debug.Log("####### m_sortingLayer is Empty");

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
        }

        if(m_isInit)
            CheckUpdateModelView();
    }

    protected ModelNPC.PortalData GetUpdateData(string callInfo = "PortalController.GetUpdateData")
    {
        var m_DataPortal = m_dataController.UpdateData(callInfo) as ModelNPC.PortalData;
        return m_DataPortal;
    }

    #region Model View
    private void LoadModelsView()
    {
        if (PanelModelsView == null)
        {
            Debug.Log("###### LoadModelsView  PanelModelsViewNPC == null");
            return;
        }

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
            }
        }
    }
#endregion
}
