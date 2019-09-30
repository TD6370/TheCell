using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum TypesJobs { Free, Bathering, Build, Craft, Defense, Patrol, Scout, War, Destroy, Fix }
public enum TypesJobTo { Free, ToPortal, ToMe, ToBrother, ToEnemy, ToHero, ToRed, ToBlue, ToGreen, ToViolet }

// Job Bathering - > TargetResource
// Job Build - > TargetResource On TargetLocation
// Job Craft - > TargetResource On TargetLocation(Portal) -> ResourceResult (build, resource)

[CreateAssetMenu(menuName = "Custom Tool/Create Alien Job %&j", fileName = "AlienJob_")]
[SerializeField]
public class AlienJob : ScriptableObject
{
    [SerializeField]
    public TypesJobs Job;
    [SerializeField]
    public SaveLoadData.TypePrefabs TargetResource = SaveLoadData.TypePrefabs.PrefabField;
    [SerializeField]
    public SaveLoadData.TypePrefabs ResourceResult = SaveLoadData.TypePrefabs.PrefabField;
    [SerializeField]
    public string TargetField;
    [SerializeField]
    public string TargetId;
    [SerializeField]
    public TypesJobTo JobTo;
}


public static class AlienJobsManager
{
    //TEST
    //public static List<string> TestHistoryJobs = new List<string>();
    //public static List<string> TestHistoryJobsDelID = new List<string>();
    //public static List<string> TestHistoryJobsSpawnID = new List<string>();

    public static bool CheckJobAlien(ModelNPC.GameDataAlien p_dataNPC, GameActionPersonController controller = null)
    {
        AlienJob job = p_dataNPC.Job;
        if(job!=null)
        {
            string fieldAlien = string.Empty;
            string fieldTarget = string.Empty;
            Helper.GetNameFieldByPosit(ref fieldAlien, p_dataNPC.Position);
            SaveLoadData.TypePrefabs jobResourceTarget = job.TargetResource;
            //Target object
            ReaderScene.DataObjectInfoID targetInfo = ReaderScene.GetInfoID(p_dataNPC.TargetID);
            if (targetInfo != null)
            {
                if (!targetInfo.TestIsValud())
                {
                    p_dataNPC.TargetID = null;
                }
                else
                {
                    int distField = 0;
                    Helper.GetDistatntionFields(ref distField, targetInfo.Data.Position, p_dataNPC.Position);
                    Helper.GetNameFieldByPosit(ref fieldTarget, targetInfo.Data.Position);

                    ModelNPC.PortalData portal = targetInfo.Data as ModelNPC.PortalData;
                    if (distField < 2)
                    {

                        if (portal != null)
                        {
                            if (p_dataNPC.Inventory != null)
                            {
                                //***** Back to HOME **** (trget is Portal)
                                //p_dataNPC.InventoryObject is ModelNPC;
                                Storage.Portals.AddResourceFromAlien(portal, p_dataNPC);
                            }
                            else
                            {
                                p_dataNPC.Inventory = DataObjectInventory.EmptyInventory();
                                Debug.Log(Storage.EventsUI.ListLogAdd = "## JOB: dataNPC.Inventory is null");
                            }

                            //End job
                            p_dataNPC.Job = null;
                            p_dataNPC.TargetID = string.Empty;
                            p_dataNPC.TargetPosition = Vector3.zero;
                        }
                        else
                        {
                            //Test job on target //@JOB@
                            if (targetInfo.Data.TypePrefab == jobResourceTarget)
                            {
                                //if (!job.IsJobCompleted && !job.IsJobRun)
                                //if (p_dataNPC.CurrentAction == GameActionPersonController.NameActionsPerson.Target.ToString())
                                //p_dataNPC.CurrentAction != GameActionPersonController.NameActionsPerson.Work.ToString())
                                if (p_dataNPC.CurrentAction != GameActionPersonController.NameActionsPerson.CompletedLoot.ToString() &&
                                    p_dataNPC.CurrentAction != GameActionPersonController.NameActionsPerson.Work.ToString())
                                {
                                    //job.IsJobRun = true;
                                    //p_dataNPC.CurrentAction = GameActionPersonController.NameActionsPerson.Target.ToString();
                                    //GameActionPersonController.RequestActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Work, null);
                                    GameActionPersonController.ExecuteActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Work, controller, true);
                                }
                                //if(job.IsJobCompleted)
                                if(p_dataNPC.CurrentAction == GameActionPersonController.NameActionsPerson.CompletedLoot.ToString())
                                {
                                    //p_dataNPC.CurrentAction = GameActionPersonController.NameActionsPerson.Target.ToString();
                                    GameActionPersonController.ExecuteActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Move, controller, true);
                                    // **** FIND RESOURCE ****
                                    //---Replace object
                                    //1. Remove resource
                                    Vector3 posTarget = targetInfo.Data.Position;
                                    GenericWorldManager.ClearLayerObject(targetInfo.Data);
                                    //2. Create new resource
                                    if (job.ResourceResult != SaveLoadData.TypePrefabs.PrefabField)
                                    {
                                        //Storage.GenWorld.GenericPrefabOnWorld(job.ResourceResult, posTarget);
                                        ModelNPC.ObjectData spawnObject = Storage.GenWorld.GetCreatePrefab(job.ResourceResult, targetInfo.Field);
                                        bool isSpawned = Storage.Data.AddDataObjectInGrid(spawnObject,
                                                            targetInfo.Field, "CheckJobAlien",
                                                            p_modeDelete: PaletteMapController.SelCheckOptDel.None,
                                                            p_modeCheck: PaletteMapController.SelCheckOptDel.DelTerra);
                                        if (!isSpawned)
                                            Debug.Log(Storage.EventsUI.ListLogAdd = "### JOB: Not Spawn " + spawnObject.NameObject);
                                    }
                                    bool isZonaReal = Helper.IsValidPiontInZona(targetInfo.Data.Position.x, targetInfo.Data.Position.y);
                                    //if (!targetInfo.Data.IsReality && isZonaReal)
                                    if (isZonaReal)
                                    {
                                        Storage.GenGrid.LoadObjectToReal(targetInfo.Field);
                                    }

                                    //3. Add resource in Inventory
                                    p_dataNPC.Inventory = targetInfo.Data.LootObjectToInventory(p_dataNPC);
                                    //4. Set target to target location
                                    if (job.JobTo == TypesJobTo.ToPortal)
                                    {
                                        //GameActionPersonController.RequestActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Idle, null);
                                        //GameActionPersonController.RequestActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Target, null);
                                    }
                                }
                                //continue work...
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public static void GetNameJob(ref string result, AlienJob job)
    {
        if (job == null)
            return;
        //AlienJob job = p_dataNPC.Job;
        result = string.Format("{0}_{1}_{2}_{3}", job.Job, job.TargetResource, job.ResourceResult, job.JobTo);
    }
    public static string GetNameJob(AlienJob job)
    {
        if (job == null)
            return string.Empty;

        return string.Format("{0}_{1}_{2}_{3}", job.Job, job.TargetResource, job.ResourceResult, job.JobTo);
    }

    private static List<AlienJob> temp_listJobs;
    public static void GetJobFromName(ref AlienJob resultJob, string jobName, SaveLoadData.TypePrefabs typeAlien)
    {
        //AlienJob job = p_dataNPC.Job;
        Storage.Person.CollectionAlienJob.TryGetValue(typeAlien, out temp_listJobs);
        string nameNextJob = string.Empty;
        if(temp_listJobs != null)
        {
            foreach(var itemJob in temp_listJobs)
            {
                GetNameJob(ref nameNextJob, itemJob);
                if(nameNextJob == jobName)
                {
                    resultJob = itemJob;
                }
            }
        }
        
    }
}

