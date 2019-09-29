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
    public static List<string> TestHistoryJobs = new List<string>();
    public static List<string> TestHistoryJobsDelID = new List<string>();
    public static List<string> TestHistoryJobsSpawnID = new List<string>();


    public static bool CheckJobAlien(ModelNPC.GameDataAlien p_dataNPC)
    {
        //AlienJob job = p_dataNPC.Job;
        //string jobName = p_dataNPC.JobName;
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
                if (!targetInfo.TestIsValud()) //FIX**DELETE
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
                                //------ test ----------
                                if (string.IsNullOrEmpty(targetInfo.ID))
                                {
                                    Debug.Log(Storage.EventsUI.ListLogAdd = "## JOB: ReaderScene.DataObjectInfoID targetInfo is null");
                                }
                                else
                                {
                                    var objsTest = ReaderScene.GetFieldsByID(targetInfo.ID);
                                    if (objsTest.Count == 0)
                                    {
                                        targetInfo.TestIsValud();
                                        return false;
                                    }
                                }
                                //--------------

                                // **** FIND RESOURCE ****
                                //---Replace object
                                //1. Remove resource
                                //Storage.Data.RemoveDataObjectInGrid(fieldTarget, -1, "CheckJobAlien", dataObjDel: targetInfo.Data);
                                Vector3 posTarget = targetInfo.Data.Position;
                                //Storage.Data.RemoveDataObjectInGrid(targetInfo.Data);
                                //GenericWorldManager.ClearLayerForStructure(targetInfo.Field, true);
                                GenericWorldManager.ClearLayerObject(targetInfo.Data);
                                //---TEST
                                TestHistoryJobs.Add("ClearLayerObject : " + targetInfo.Data.NameObject);

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
                                    else
                                    {
                                        //-- TEST JOB
                                        TestHistoryJobs.Add(spawnObject.NameObject + " >> " + targetInfo.Field);
                                        TestHistoryJobsSpawnID.Add(spawnObject.Id);
                                    }
                                }
                                bool isZonaReal = Helper.IsValidPiontInZona(targetInfo.Data.Position.x, targetInfo.Data.Position.y);
                                //if (!targetInfo.Data.IsReality && isZonaReal)
                                if (isZonaReal)
                                {
                                    Storage.GenGrid.LoadObjectToReal(targetInfo.Field);
                                    //--TEST
                                    TestHistoryJobs.Add(" LoadObjectToReal >> " + targetInfo.Field);
                                }

                                //3. Add resource in Inventory
                                p_dataNPC.Inventory = targetInfo.Data.LootObjectToInventory(p_dataNPC);
                                //4. Set target to target location
                                if (job.JobTo == TypesJobTo.ToPortal)
                                {
                                    //GameActionPersonController.RequestActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Idle, null);
                                    //GameActionPersonController.RequestActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Target, null);
                                }
                                //continue work...
                                return true;
                            }
                        }
                    }
                }
            }
            //Test all object from location
            //List<ModelNPC.ObjectData> objectsOnField = ReaderScene.GetObjectsDataFromGrid(fieldAlien);
            //foreach(ModelNPC.ObjectData nextObject in objectsOnField)
            //{
            //    if(nextObject.Id == p_dataNPC.TargetID)
            //    {
            //    }
            //}
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

