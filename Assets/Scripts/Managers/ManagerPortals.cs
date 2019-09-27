using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ManagerPortals : MonoBehaviour
{
    [SerializeField]
    public ContainerPortalFabrication ContainerFabrications;

    [Space]
    [Header("INFO PORTAL:")]
    public string InfoPortal;
    public string CurrentAnimationState;
    [Space]
    public string CurrentID;
    public int CurrentIndex;
    public ModelNPC.PortalData CurrentPortal;

    public List<ModelNPC.PortalData> Portals;
    public Dictionary<TypesBiomNPC, List<PortalResourceFabrication>> ResourcesFabrications;
    public SettingsPortals SettingPortals;

    private List<PortalResourceFabrication> m_listResourceWork;
    private DataObjectInventory m_resourceNext;
    private PortalResourceFabrication m_workResource;
    private bool isInitPortals = false;

    public enum TypeResourceProcess { Incubation, IncubationLight, IncubationMedium, IncubationHight,
        SpawnFlore, WpawnFloor, None };

    public ManagerPortals()
    {
        Portals = new List<ModelNPC.PortalData>();
    }

    private static Dictionary<TypesBiomNPC, GenericWorldManager.GenObjectWorldMode> GetGenericModeNPC;

    private void Awake()
    {
        GetGenericModeNPC = new Dictionary<TypesBiomNPC, GenericWorldManager.GenObjectWorldMode>()
        {
            { TypesBiomNPC.Blue, GenericWorldManager.GenObjectWorldMode.BlueNPC},
            { TypesBiomNPC.Red, GenericWorldManager.GenObjectWorldMode.RedNPC},
            { TypesBiomNPC.Green, GenericWorldManager.GenObjectWorldMode.BlueNPC},
            { TypesBiomNPC.Violet, GenericWorldManager.GenObjectWorldMode.VioletNPC},
        };
    }

    private void LateUpdate()
    {
        if (!isInitPortals && Storage.Instance.ReaderSceneIsValid)
        {

            isInitPortals = true;
            LoadPortals();
        }
    }

    private void LoadPortals()
    {
        if (false == LoadResourceFabrications())
            return;

        if (SettingPortals == null)
        {
            Debug.Log(Storage.EventsUI.ListLogAdd = "Setting portals is Empty !!!");
            return;
        }

        Storage.EventsUI.ListLogAdd = "LoadPortals....";
        Storage.EventsUI.SetMessageBox = "LoadPortals....";

        StartCoroutine(StartLoadingPortals());
    }

    private bool LoadResourceFabrications()
    {
        if (ContainerFabrications == null || ContainerFabrications.CollectionPriorityFinder == null)
        {
            Storage.EventsUI.ListLogAdd = "Container Prioritys is EMPTY !!!!";
            Storage.EventsUI.SetMessageBox = "Container Prioritys is EMPTY !!!!";
            return false;
        }

        Storage.EventsUI.ListLogAdd = "Load Resource Fabrications....";
        Storage.EventsUI.SetMessageBox = "Load Resource Fabrications...."; ;

        TypesBiomNPC typeBiom = TypesBiomNPC.None;
        List<PortalResourceFabrication> listResourcesBiom = null; // = new List<PortalResourceFabrication>();
        ResourcesFabrications = new Dictionary<TypesBiomNPC, List<PortalResourceFabrication>>();
        foreach (PortalResourceFabrication resourceNext in ContainerFabrications.CollectionPriorityFinder.OrderBy(p => p.TypeBiom))
        {
            if (typeBiom != resourceNext.TypeBiom)
            {
                typeBiom = resourceNext.TypeBiom;
                ResourcesFabrications.TryGetValue(resourceNext.TypeBiom, out listResourcesBiom);
            }
            if (listResourcesBiom == null)
            {
                listResourcesBiom = new List<PortalResourceFabrication>();
                ResourcesFabrications.Add(typeBiom, listResourcesBiom);
            }
            listResourcesBiom.Add(resourceNext);
        }
        if (ResourcesFabrications == null || ResourcesFabrications.Count == 0)
        {
            Storage.EventsUI.ListLogAdd = "LoadPortals....";
            Storage.EventsUI.SetMessageBox = "LoadPortals....";
            return false;
        }

        isInitPortals = true;
        return true;
    }

    IEnumerator StartLoadingPortals()
    {
        Portals.Clear();
        var arrayID = Storage.ReaderWorld.CollectionInfoID.Values.ToArray();
        yield return null;
        int count = arrayID.Count();
        for (int i = 0; i < count; i++)
        {
            var item = arrayID[i];
            if (item.Data.IsPortal())
            {
                //Portals.TryGetValue(item.ID, out portal);
                //if(portal == null)
                //    Portals.Add(item.ID, item.Data as ModelNPC.PortalData);
                Portals.Add(item.Data as ModelNPC.PortalData);

            }
            if (i % 1000 == 0)
                yield return null;

        }
        yield return null;

        if (Portals.Count == 0)
            CreatePortal();

        StartCoroutine(DispatcherPortals());
    }


    IEnumerator DispatcherPortals()
    {
        float timeWait = 10f; // 5f;
        bool isCompleted = false;
        yield return null;
        while (!isCompleted)
        {
            if (!Storage.Instance.ReaderSceneIsValid)
            {
                yield return new WaitForSeconds(timeWait * 3);
                continue;
            }
            yield return null;

            CurrentPortal = Portals[CurrentIndex];

            if (CurrentPortal == null)
            {
                Debug.Log("######### DispatcherPortals.CurrentPortal == null");
                yield return new WaitForSeconds(timeWait * 2);
                continue;
            }

            CurrentID = CurrentPortal.Id;
            InfoPortal = CurrentPortal.GetInfo();
            PortalProcess(CurrentPortal);

            yield return new WaitForSeconds(timeWait);

            CurrentIndex++;
            if (CurrentIndex >= Portals.Count)
                CurrentIndex = 0;

        }
        yield return null;
    }


    private void PortalProcess(ModelNPC.PortalData p_portal)
    {
        if (p_portal.ChildrensId == null)
            p_portal.ChildrensId = new List<string>();

        int preCountChild = p_portal.ChildrensId.Count;
        //bool isIncubationValid = p_portal.ChildrenPreparationIncubation();
        //bool isParkingLock = p_portal.ChildrenPreparationIncubation();
        bool isNotCreated;
        bool isTimeValid = p_portal.LastTimeIncubation + SettingPortals.PeriodIncubation < Time.time;

        //foreach(DataObjectInventory resource in p_portal.Resources)
        if (isTimeValid && p_portal.Resources != null && p_portal.Resources.Count > 0)
        {
            Storage.EventsUI.ListLogAdd = "PortalWork....";
            Storage.EventsUI.SetMessageBox = "PortalWork....";

            for (int i = p_portal.Resources.Count() - 1; i > 0; i--)  //!!!!!!!!!!!!!!!!
            {
                m_resourceNext = p_portal.Resources[i];
                m_listResourceWork = ResourcesFabrications[p_portal.TypeBiom];
                m_workResource = m_listResourceWork.Find(p => p.ResouceInventory == m_resourceNext.TypeInventoryObject);
                if (m_workResource != null)
                {
                    if (m_resourceNext.Count >= m_workResource.LimitToBeginProcess)
                    {
                        isNotCreated = preCountChild == p_portal.ChildrensId.Count && p_portal.CurrentProcess == TypeResourceProcess.None;
                        if (isNotCreated)
                        {
                            //Begin portal process incubation on Full resources
                            StartPortalProcess(m_workResource.BeginProcess, p_portal, i, m_workResource);
                            //isIncubationValid = false;
                        }
                    }
                }
            }
        }
        isNotCreated = preCountChild == p_portal.ChildrensId.Count;// && isIncubationValid;
        if (isNotCreated)
        {
            //if (p_portal.CurrentProcess != TypeResourceProcess.None)
            //    StartPortalProcess(p_portal.CurrentProcess, p_portal); //FIX
            //else
            if (p_portal.CurrentProcess == TypeResourceProcess.None)
            {
                bool isNotFullLimit = p_portal.ChildrensId.Count < SettingPortals.StartLimitNPC;
                
                //Begin portal process on Time
                if (isNotFullLimit && isTimeValid)
                {
                    StartPortalProcess(TypeResourceProcess.Incubation, p_portal);
                }
            }
        }
    }

    //Start Portal process
    private void StartPortalProcess(TypeResourceProcess workProcess, ModelNPC.PortalData p_portal, int index = -1, PortalResourceFabrication fabrication=null)
    {
        switch (workProcess)
        {
            case TypeResourceProcess.Incubation:
                // *  Start Action
                IncubationProcess(p_portal);
                // *  Remove resource
                if (index != -1)
                {
                    p_portal.Resources[index].Count -= fabrication.LimitToBeginProcess;
                    if (p_portal.Resources[index].Count < 0)
                        p_portal.Resources.RemoveAt(index);

                }
                break;
        }
    }

    public void CreatePortal()
    {
        string[] portalsId = Storage.GenWorld.GenericPortal(1, SaveLoadData.TypePrefabs.PortalBlue);
        foreach (string idPortal in portalsId)
        {
            if (ReaderScene.ExistID(idPortal))
            {
                var info = ReaderScene.GetInfoID(idPortal);
                ModelNPC.PortalData portalNext = info.Data as ModelNPC.PortalData;
                if (portalNext != null)
                {
                    Portals.Add(portalNext);

                    //... Check on Real
                    string fieldName = string.Empty;
                    Helper.GetNameFieldByPosit(ref fieldName, portalNext.Position);
                    bool isZonaReal = Helper.IsValidPiontInZona(portalNext.Position.x, portalNext.Position.y);
                    if(!isZonaReal)
                        portalNext.IsReality = false;
                    if (!portalNext.IsReality && isZonaReal)
                        Storage.GenGrid.LoadObjectToReal(fieldName);
                    //else
                   //     portalNext.IsReality = false;
                }
            }
        }
    }

    public void AddResourceFromAlien(ModelNPC.PortalData portal, ModelNPC.GameDataAlien alien)
    {
        DataObjectInventory existRes = DataObjectInventory.EmptyInventory(); 
        //>INV>
        try
        {
            existRes = portal.Resources.Where(p => p.TypeInventoryObject == alien.Inventory.TypeInventoryObject).FirstOrDefault();
        }catch(System.Exception ex)
        {
            Debug.Log(Storage.EventsUI.ListLogAdd = string.Format("###### AddResourceFromAlien error: {0}", ex));
        }
        if (existRes == null)
            portal.Resources.Add(new DataObjectInventory(alien.Inventory));
        else
            existRes.Count += alien.Inventory.Count;
        alien.Inventory.Clear();
    }

    public void AddResource(ModelNPC.PortalData portal, DataObjectInventory inventory)
    {
        //>INV>
        DataObjectInventory existRes =  portal.Resources.Where(p => p.TypeInventoryObject == inventory.TypeInventoryObject).FirstOrDefault();
        if (existRes == null)
            portal.Resources.Add(inventory);
        else
            existRes.Count += inventory.Count;

    }

    #region Process 

    public static void IncubationProcess(ModelNPC.PortalData p_portal, bool isCallFromReality = false)
    {
        p_portal.CurrentProcess = TypeResourceProcess.Incubation;
        bool isDreamworker = !p_portal.IsReality;
        if (isDreamworker || isCallFromReality)
        {
            GenericWorldManager.GenObjectWorldMode nodeGen = GenericWorldManager.GenObjectWorldMode.NPC;
            Vector3 posGen = p_portal.Position;

            GetGenericModeNPC.TryGetValue(p_portal.TypeBiom, out nodeGen);
            if(nodeGen.Equals(null))
            {
                Debug.Log("######### GetGenericModeNPC not found = " + p_portal.TypeBiom.ToString());
                return;
            }

            string fieldName = string.Empty;
            Vector3 positGen = new Vector3();
            bool isParkingFree = p_portal.ChildrenPreparationIncubation();
            if (isParkingFree)
                positGen = p_portal.Position;
            else
                positGen = p_portal.SearchParking(ref fieldName);

            if (positGen == Vector3.zero)
                return;

            if (fieldName == string.Empty)
                Helper.GetNameFieldByPosit(ref fieldName, positGen);

            SaveLoadData.TypePrefabs genNPC = Storage.GenWorld.GenObjectWorld(nodeGen);
            //TEST
            genNPC = SaveLoadData.TypePrefabs.Inspector;

            ModelNPC.ObjectData objDataNPC = Storage.GenWorld.GenericPrefabOnWorld(genNPC, positGen);
            if (objDataNPC != null)
            {
                (objDataNPC as ModelNPC.PersonData).PortalId = p_portal.Id;
                p_portal.ChildrensId.Add(objDataNPC.Id);
                bool isZonaReal = Helper.IsValidPiontInZona(objDataNPC.Position.x, objDataNPC.Position.y);
                if (!isZonaReal)
                    objDataNPC.IsReality = false;
                if (!objDataNPC.IsReality && isZonaReal)
                {
                    Storage.GenGrid.LoadObjectToReal(fieldName);
                    Storage.EventsUI.ListLogAdd = ">>>>> IncubationProcess >>> LoadObjectToReal...";
                }
            }
            p_portal.CurrentProcess = TypeResourceProcess.None;
            p_portal.LastTimeIncubation = Time.time;
        }
    }
    #endregion

    public void Stop()
    {
        Portals.Clear();
    }
}


