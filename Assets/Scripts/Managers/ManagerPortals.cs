using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ManagerPortals : MonoBehaviour
{
  
    [SerializeField]
    public ContainerPortalFabrication ContainerFabrications;
    public List<ModelNPC.PortalData> Portals;
    public Dictionary<TypesBiomNPC, List<PortalResourceFabrication>> ResourcesFabrications;

    public string CurrentID;
    public int CurrentIndex;
    public ModelNPC.PortalData CurrentPortal;

    private List<PortalResourceFabrication> m_listResourceWork;
    private DataObjectInventory m_resourceNext;
    private PortalResourceFabrication m_workResource;
    private bool isInitPortals = false;
        
    public enum TypeResourceProcess { Incubation, IncubationLight, IncubationMedium, IncubationHight,
        SpawnFlore, WpawnFloor, None};

    public ManagerPortals()
    {
        Portals = new List<ModelNPC.PortalData>();
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
        foreach (PortalResourceFabrication resourceNext in ContainerFabrications.CollectionPriorityFinder.OrderBy(p=>p.TypeBiom))
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
            //if (Portals.Count == 0)
            //{
            //    yield return new WaitForSeconds(timeWait * 2);
            //    //yield return new WaitForSeconds(timeWait);
            //    continue;
            //}
            yield return null;

            CurrentPortal = Portals[CurrentIndex];

            if (CurrentPortal == null)
            {
                Debug.Log("######### DispatcherPortals.CurrentPortal == null");
                yield return new WaitForSeconds(timeWait * 2);
                continue;
            }

            CurrentID = CurrentPortal.Id;
            PortalProcess(CurrentPortal);

            yield return new WaitForSeconds(timeWait);

            CurrentIndex++;
            if (CurrentIndex >= Portals.Count)
                CurrentIndex = 0;

        }
        yield return null;
    }

    public void CreatePortal()
    {
        string[] portalsId = Storage.GenWorld.GenericPortal(1, SaveLoadData.TypePrefabs.PortalBlue);
        foreach (string idPortal in portalsId)
        {
            if (ReaderScene.ExistID(idPortal))
            {
                ModelNPC.PortalData portalNext = Storage.ReaderWorld.CollectionInfoID[idPortal].Data as ModelNPC.PortalData;
                if (portalNext != null)
                    Portals.Add(portalNext);
            }
        }
    }
       

    private void PortalProcess(ModelNPC.PortalData p_portal)
    {
        //foreach(DataObjectInventory resource in p_portal.Resources)
        if (p_portal.Resources == null || p_portal.Resources.Count == 0)
            return;

        Storage.EventsUI.ListLogAdd = "PortalWork....";
        Storage.EventsUI.SetMessageBox = "PortalWork....";

        for (int i = p_portal.Resources.Count() - 1; i > 0; i--)  //!!!!!!!!!!!!!!!!
        {
            m_resourceNext = p_portal.Resources[i];
            m_listResourceWork = ResourcesFabrications[p_portal.TypeBiom];
            m_workResource = m_listResourceWork.Find(p => p.ResouceInventory == m_resourceNext.TypeInventoryObject);
            if(m_workResource != null)
            {
                if(m_resourceNext.Count >= m_workResource.LimitToBeginProcess)
                {
                    //Begin portal process
                    StartPortalProcess(m_workResource.BeginProcess, p_portal, i);
                }
            }
        }
    }

    //Start Portal process
    private void StartPortalProcess(TypeResourceProcess workProcess, ModelNPC.PortalData p_portal, int index)
    {
        switch (workProcess)
        {
            case TypeResourceProcess.Incubation:
                // *  Start Action
                StartIncubationProcess(p_portal);
                // *  Remove resource
                p_portal.Resources.RemoveAt(index);
                break;
        }
    }

    private void StartIncubationProcess(ModelNPC.PortalData p_portal)
    {
        p_portal.CurrentProcess = TypeResourceProcess.Incubation;

        bool isDreamworker = !p_portal.IsReality;
        if (isDreamworker)
        {
            GenericWorldManager.GenObjectWorldMode nodeGen = GenericWorldManager.GenObjectWorldMode.NPC;
            Vector3 posGen = p_portal.Position;
            if(p_portal.TypeBiom == TypesBiomNPC.Blue)
            {
                nodeGen = GenericWorldManager.GenObjectWorldMode.BlueNPC;
                SaveLoadData.TypePrefabs genNPC = Storage.GenWorld.GenObjectWorld(nodeGen);
                Storage.GenWorld.GenericPrefab(genNPC, p_portal.Position);
                p_portal.CurrentProcess = TypeResourceProcess.None;
                //GetBiomByTypeModel
            }

        }


        //GetBiomByTypeModel
        //public enum TypeBioms { Blue, Red, Green, Violet, Grey }
        //if (!Storage.Instance.ReaderSceneIsValid)
        //    return;
        //ReaderScene.DataObjectInfoID infoPortal =  ReaderScene.GetInfoID(p_portal.Id);
        //if(infoPortal!=null && 
        //    infoPortal.Data.IsReality &&
        //    infoPortal.Gobject !=null )
        //{
        //    //In Reality
        //    //var gobject = infoPortal.Gobject;
        //    //gobject.Get

        //}
    }

}

