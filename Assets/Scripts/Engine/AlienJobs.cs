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
    public static bool CheckJobAlien(ModelNPC.PersonData p_dataNPC)
    {
        //AlienJob job = p_dataNPC.Job;
        //string jobName = p_dataNPC.JobName;
        AlienJob job = p_dataNPC.Job;

        if(job!=null)
        {
            string fieldAlien = string.Empty;
            string fieldTarget = string.Empty;
            Helper.GetNameFieldByPosit(ref fieldAlien, p_dataNPC.Position);
            SaveLoadData.TypePrefabs invObj = job.ResourceResult;
            //Target object
            ReaderScene.DataObjectInfoID targetInfo = ReaderScene.GetInfoID(p_dataNPC.TargetID);
            if (targetInfo != null)
            {
                
                int distField = 0;
                Helper.GetDistatntionFields(ref distField, targetInfo.Data.Position, p_dataNPC.Position);
                Helper.GetNameFieldByPosit(ref fieldTarget, targetInfo.Data.Position);
                
                
                if (distField<2)
                {
                    //Test job on target //@JOB@
                    if (targetInfo.Data.TypePrefab == invObj)
                    {
                        //---Replace object
                        //1. Remove resource
                        //Storage.Data.RemoveDataObjectInGrid(fieldTarget, -1, "CheckJobAlien", dataObjDel: targetInfo.Data);
                        Vector3 posTarget = targetInfo.Data.Position;
                        Storage.Data.RemoveDataObjectInGrid(targetInfo.Data);
                        //2. Create new resource
                        if(job.ResourceResult != SaveLoadData.TypePrefabs.PrefabField)
                        {
                            Storage.GenWorld.GenericPrefabOnWorld(job.ResourceResult, posTarget);
                        }
                        //3. Add resource in Inventory

                        //4. Set target to target location
                        if(job.JobTo == TypesJobTo.ToPortal)
                        {
                            //GameActionPersonController.RequestActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Idle, null);
                            //GameActionPersonController.RequestActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Target, null);
                        }
                    }
                }
            }
            //Test all object from location
            List<ModelNPC.ObjectData> objectsOnField = ReaderScene.GetObjectsDataFromGrid(fieldAlien);
            foreach(ModelNPC.ObjectData nextObject in objectsOnField)
            {
                if(nextObject.Id == p_dataNPC.TargetID)
                {
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

