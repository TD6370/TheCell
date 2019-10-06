using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

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
        SpawnFlore, WpawnFloor, None, ResourceProduction };

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
        //TEST
        //var test = ScriptableObjectUtility.LoadContainerPriorityFinderByTag("Terra");
        //var test2 = ScriptableObjectUtility.LoadResource<ContainerPortalFabrication>("Job");

        if (ContainerFabrications == null)
        {
            ContainerFabrications = ScriptableObjectUtility.LoadResource<ContainerPortalFabrication>("Job");
            if (ContainerFabrications == null)
            {
                Storage.EventsUI.ListLogAdd = "Container LoadResourceFabrications is EMPTY !!!!";
                Storage.EventsUI.SetMessageBox = "Container LoadResourceFabrications is EMPTY !!!!";
                return false;
            }
        }
        if (ContainerFabrications.CollectionResourceFabrication == null)
        {


            Storage.EventsUI.ListLogAdd = "Container CollectionPriorityFinder is EMPTY !!!!";
            Storage.EventsUI.SetMessageBox = "Container CollectionPriorityFinder is EMPTY !!!!";
            return false;
        }

        Storage.EventsUI.ListLogAdd = "Load Resource Fabrications....";
        Storage.EventsUI.SetMessageBox = "Load Resource Fabrications...."; ;

        TypesBiomNPC typeBiom = TypesBiomNPC.None;
        List<PortalResourceFabrication> listResourcesBiom = null; // = new List<PortalResourceFabrication>();
        ResourcesFabrications = new Dictionary<TypesBiomNPC, List<PortalResourceFabrication>>();
        foreach (PortalResourceFabrication resourceNext in ContainerFabrications.CollectionResourceFabrication.OrderBy(p => p.TypeBiom))
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
            if(Portals == null || Portals.Count == 0)
            {
                yield return new WaitForSeconds(timeWait * 3);
                continue;
            }
            if (CurrentIndex >= Portals.Count)
                CurrentIndex = 0;

            CurrentPortal = Portals[CurrentIndex];
            if (CurrentPortal == null)
            {
                Debug.Log("######### DispatcherPortals.CurrentPortal == null");
                yield return new WaitForSeconds(timeWait * 2);
                continue;
            }
            if (CurrentPortal.Id == null)
            {
                Debug.Log("######### DispatcherPortals.CurrentPortal ID is null");
                yield return new WaitForSeconds(timeWait * 2);
                continue;
            }

            CurrentID = CurrentPortal.Id;
            InfoPortal = CurrentPortal.GetInfo();
            PortalProcess(CurrentPortal);

            yield return new WaitForSeconds(timeWait);
            CurrentIndex++;
        }
        yield return null;
    }


    private void PortalProcess(ModelNPC.PortalData p_portal)
    {
        if (p_portal.ChildrensId == null)
            p_portal.ChildrensId = new List<string>();

        int preCountChild = p_portal.ChildrensId.Count;
        bool isProgress;
        bool isTimeValid = p_portal.LastTimeFabrication + SettingPortals.PeriodIncubation < Time.time;
        bool isNotAllLimit = p_portal.ChildrensId.Count < SettingPortals.AllLimitNPC;
        if (SettingPortals.AllLimitNPC == 0)
            SettingPortals.AllLimitNPC = 1;
        if (SettingPortals.AllLimitNPC < SettingPortals.StartLimitNPC)
            SettingPortals.AllLimitNPC = SettingPortals.StartLimitNPC;

        //foreach(DataObjectInventory resource in p_portal.Resources)
        if (isNotAllLimit && isTimeValid && p_portal.Resources != null && p_portal.Resources.Count > 0)
        {
            Storage.EventsUI.ListLogAdd = "PortalWork....";
            Storage.EventsUI.SetMessageBox = "PortalWork....";

            for (int i = p_portal.Resources.Count() - 1; i >= 0; i--) 
            {
                m_resourceNext = p_portal.Resources[i];
                m_listResourceWork = ResourcesFabrications[p_portal.TypeBiom];
                m_workResource = m_listResourceWork.Find(p => p.ResouceInventory == m_resourceNext.TypeInventoryObject);
                if (m_workResource != null)
                {
                    if (m_resourceNext.Count >= m_workResource.LimitToBeginProcess)
                    {
                        //isNotProgress = preCountChild == p_portal.ChildrensId.Count && p_portal.CurrentProcess == TypeResourceProcess.None;
                        isProgress = preCountChild != p_portal.ChildrensId.Count && p_portal.CurrentProcess != TypeResourceProcess.None;
                        if (!isProgress)
                        {
                            //Begin portal process incubation on Full resources
                            StartPortalProcess(m_workResource.BeginProcess, p_portal, i, m_workResource);
                            //isIncubationValid = false;
                        }
                    }
                }
            }
        }
        isProgress = preCountChild != p_portal.ChildrensId.Count;// && isIncubationValid;
        if (isProgress == false)
        {
            //if (p_portal.CurrentProcess == TypeResourceProcess.None)
            if (p_portal.CurrentProcess == TypeResourceProcess.None || 
                (!p_portal.IsReality && p_portal.CurrentProcess == TypeResourceProcess.Incubation)) //FIX<<>>INCUBATION
            {
                bool isNotStartLimit = p_portal.ChildrensId.Count < SettingPortals.StartLimitNPC;
                //Begin portal process on Time
                if (isNotStartLimit && isTimeValid)
                {
                    StartPortalProcess(TypeResourceProcess.Incubation, p_portal);
                    isProgress = true;
                }
            }
        }
        // - Resources Production
        if (!isProgress && 
            p_portal.CurrentProcess == TypeResourceProcess.None && 
            isTimeValid &&
            p_portal.Resources != null && 
            p_portal.Resources.Count > 0 )
        {
            isProgress = FabricationProduction(p_portal);
        }

        if(isProgress)
            p_portal.EndFabrication();
    }

    private void GetFabricsByResource(ref List<PortalResourceFabrication> resultFabrics, ModelNPC.PortalData p_portal, string p_nameResourceInv)
    {
        resultFabrics = ResourcesFabrications[p_portal.TypeBiom].Where(f => f.ResouceInventory.ToString() == p_nameResourceInv).ToList();
    }

    //private void  GetFabricaByResource(ref PortalResourceFabrication resultFabrica,  ModelNPC.PortalData p_portal, string p_nameResourceInv)
    //{
    //    resultFabrica = ResourcesFabrications[p_portal.TypeBiom].Find(f => f.ResouceInventory.ToString() == p_nameResourceInv);
    //}

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

    public bool CheckStorageResourceForAlien(ModelNPC.PortalData portal, ModelNPC.GameDataAlien alien)
    {
        string strErr = "0";
;
        if(portal.Resources == null || portal.Resources.Count == 0)
            return false; //pessimistic job
        if (!alien.Inventory.IsEmpty)
            return true; //optimistic job

        DataObjectInventory resNext = null;
        List<AlienJob> temp_listJobs;
        SaveLoadData.TypePrefabs typeAlien = alien.TypePrefab;
        int limitRes = 0;
        Storage.Person.CollectionAlienJob.TryGetValue(typeAlien, out temp_listJobs);

        //>INV>
        try
        {
            //strErr = "1";
            for (int indRes = portal.Resources.Count - 1; indRes >= 0; indRes--)
            {
                //strErr = "2";
                resNext = portal.Resources[indRes];
                //strErr = "3";
                if (temp_listJobs != null && resNext != null)
                {
                    //strErr = "4";
                    foreach (var itemJob in temp_listJobs)
                    {
                        //if(itemJob == null)
                        //{
                        //    Debug.Log(Storage.EventsUI.ListLogAdd = "### CheckStorageResourceForAlien itemJob is null");
                        //    continue;
                        //}
                        //trErr = "5";
                        if (itemJob.ResourceResult.ToString() == resNext.NameInventopyObject)
                        {
                            //strErr = "6";
                            limitRes = itemJob.LimitResourceCount == 0 ? 1 : itemJob.LimitResourceCount;
                            //strErr = "7";
                            alien.AddToInventory(portal, indRes, limitRes);
                            //strErr = "8";
                            alien.Job = itemJob;
                            //strErr = "9";
                            alien.CurrentAction = GameActionPersonController.NameActionsPerson.Target.ToString();
                            //strErr = "10";
                            Storage.EventsUI.ListLogAdd = "Storage To Alien >> " + resNext.NameInventopyObject + " >> " + itemJob.TargetResource;
                            return true;
                        }
                    }
                
                }
            }
        }catch (System.Exception ex)
        {
            Debug.Log(Storage.EventsUI.ListLogAdd = string.Format("###### CheckStorageResourceForAlien #{1} error: {0}", ex, strErr));
        }
        //alien.Inventory.Clear();
        return false;
    }

    public void AddResourceFromAlien(ModelNPC.PortalData portal, ModelNPC.GameDataAlien alien)
    {
        DataObjectInventory existRes = DataObjectInventory.EmptyInventory(); 
        //>INV>
        try
        {
            if (portal.Resources == null) //fix null
                portal.Resources = new List<DataObjectInventory>();

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

    public void AddResource(ModelNPC.PortalData portal, SaveLoadData.TypeInventoryObjects invObj, int Count = 1)
    {
        DataObjectInventory inventory = new DataObjectInventory(invObj.ToString(), Count);
        DataObjectInventory existRes = DataObjectInventory.EmptyInventory();
        //>INV>
        try
        {
            if (portal.Resources == null) //fix null
                portal.Resources = new List<DataObjectInventory>();

            existRes = portal.Resources.Where(p => p.TypeInventoryObject == inventory.TypeInventoryObject).FirstOrDefault();
        }
        catch (System.Exception ex)
        {
            Debug.Log(Storage.EventsUI.ListLogAdd = string.Format("###### AddResource error: {0}", ex));
        }
        if (existRes == null)
            portal.Resources.Add(new DataObjectInventory(inventory));
        else
            existRes.Count += inventory.Count;
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

    public void SetHome(ModelNPC.GameDataAlien dataAlien)
    {
        float dist_min = 10000;
        float dist = 0;
        foreach (ModelNPC.PortalData portal in Portals)
        {
            dist = Vector3.Distance(portal.Position, dataAlien.Position);
            if(dist < dist_min)
            {
                dist_min = dist;
                dataAlien.PortalId = portal.Id;
            }
        }
    }

    #region Process 

    private bool FabricationProduction(ModelNPC.PortalData p_portal)
    {
        DataObjectInventory resInventory;
        List<PortalResourceFabrication> listFabrics = null;
        bool isStartProgress = false;
        int caseBuild = 7;
        int totalResources = p_portal.Resources.Sum(p => p.Count);
        int limirProcess = totalResources / caseBuild;
        if (limirProcess < 0)
            limirProcess = 1;
        if (limirProcess > 7)
            limirProcess = 7;

        Storage.EventsUI.ListLogAdd = "PortalWork fabrication.... RES: "  + totalResources + "  pass:" + limirProcess;

        for (int proc = 0; proc < limirProcess; proc++)
        {
            for (int i = p_portal.Resources.Count - 1; i >= 0; i--)
            {
                resInventory = p_portal.Resources[i];
                if (resInventory == null || resInventory.Count == 0)
                    continue;
                //GetFabricaByResource(ref fabrica, p_portal, resInventory.NameInventopyObject);
                GetFabricsByResource(ref listFabrics, p_portal, resInventory.NameInventopyObject);
                foreach (PortalResourceFabrication fabrica in listFabrics)
                {
                    if (fabrica == null)
                        continue;

                    switch (fabrica.BeginProcess)
                    {
                        case TypeResourceProcess.ResourceProduction:
                            if (fabrica.LimitStorage == 0) //TEST
                                Debug.Log(Storage.EventsUI.ListLogAdd = "## NOT Setting LimitStorage " + fabrica.SpawnResourceName);

                            //Check total storage resources
                            int totalStorageRes = 0; 
                            var storageRes = p_portal.Resources.Where(p => p.NameInventopyObject == fabrica.SpawnResourceName.ToString()).FirstOrDefault();
                            if(storageRes != null)
                                totalStorageRes = storageRes.Count;

                            //Check production
                            if (resInventory.Count >= fabrica.LimitToBeginProcess &&
                                totalStorageRes <= fabrica.LimitStorage)
                            {
                                //Begin progress
                                p_portal.Resources[i].Count -= fabrica.LimitToBeginProcess;
                                if (p_portal.Resources[i].Count <= 0)
                                    p_portal.Resources.RemoveAt(i);
                                Storage.PortalsManager.AddResource(p_portal, fabrica.SpawnResourceName, 1);
                                Storage.EventsUI.ListLogAdd = "Fabrication >> " + resInventory.NameInventopyObject + " => " + fabrica.SpawnResourceName;
                                isStartProgress = true;
                            }
                            break;
                    }
                    //if (isStartProgress)
                    //    break;
                }
            }
        }
        return isStartProgress;
    }

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
            //TEST BILD
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
            
        }
    }
    #endregion

    public void Stop()
    {
        Portals.Clear();
    }

    //public static void GetJobJoin(ref string jeyJoin, SaveLoadData.TypePrefabs alienType, AlienJob job)
    //{
    //    jeyJoin = string.Format("{0}_{1}_{2}", alienType, job.TargetResource.ToString(), job.ResourceResult.ToString());
    //}

    public static void GetJobJoin(ref string jeyJoin, SaveLoadData.TypePrefabs alienType, string TargetResource)
    {
        jeyJoin = string.Format("{0}_{1}", alienType, TargetResource);
    }

    //public static Dictionary<SaveLoadData.TypePrefabs, List<AlienJob>> CollectionAlienJob(Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> pioritys, ref Dictionary<string, AlienJob> jobsJoin)
    public static Dictionary<SaveLoadData.TypePrefabs, List<AlienJob>> CollectionAlienJob(Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> pioritys)
    {
        string strErr = "1";
        Dictionary<SaveLoadData.TypePrefabs, List<AlienJob>> jobs = new Dictionary<SaveLoadData.TypePrefabs, List<AlienJob>>();
        SaveLoadData.TypePrefabs prefabNameType;
        SaveLoadData.TypePrefabNPC prefabNameTypeNPC;
        strErr = "2";
        try
        {
            string key = string.Empty;
            PriorityFinder prioritys;
            strErr = "3";
            var max = Enum.GetValues(typeof(SaveLoadData.TypePrefabNPC)).Length - 1;
            strErr = "4";
            for (int ind = 0; ind < max; ind++)
            {
                strErr = "5";
                prefabNameTypeNPC = (SaveLoadData.TypePrefabNPC)Enum.Parse(typeof(SaveLoadData.TypePrefabNPC), ind.ToString());
                strErr = "6";
                prefabNameType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), prefabNameTypeNPC.ToString());
                if (pioritys.ContainsKey(prefabNameType))
                {
                    strErr = "7";
                    prioritys = pioritys[prefabNameType];
                    strErr = "8";
                    if (prioritys.ListJobs != null)
                    {
                         /*
                        foreach (AlienJob job in prioritys.ListJobs)
                        {
                            if (job == null)
                            {
                                Storage.EventsUI.ListLogAdd = "##### CollectionAlienJob.job is null";
                                continue;
                            }
                            //key = string.Format("{0}_{1}", prefabNameType, job.TargetResource.ToString());
                            GetJobJoin(ref key, prefabNameType, job.TargetResource.ToString());
                            if (!jobsJoin.ContainsKey(key))
                                jobsJoin.Add(key, job);
                            else
                                Debug.Log(Storage.EventsUI.ListLogAdd = "### jobsJoin dublicte!! == " + key);
                        }
                        */
                        strErr = "9";
                        if (!jobs.ContainsKey(prefabNameType) && prioritys.ListJobs != null && prioritys.ListJobs.Count() > 0)
                            jobs.Add(prefabNameType, prioritys.ListJobs.ToList());
                    }
                    else
                    {
                        Storage.EventsUI.ListLogAdd = "##### CollectionAlienJob.prioritys.ListJobs is null";
                    }
                }
            }
            strErr = "10";
        }
        catch (Exception ex)
        {
            Storage.EventsUI.ListLogAdd = "##### CollectionAlienJob #" + strErr + " : " + ex.Message;
        }

        return jobs;
    }
}


