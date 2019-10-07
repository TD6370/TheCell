using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum TypesJobs { Free, Bathering, Build, Craft, Defense, Patrol, Scout, War, Destroy, Fix }
public enum TypesJobTo { Free, ToPortal, ToMe, ToBrother, ToEnemy, ToHero, ToRed, ToBlue, ToGreen, ToViolet }

// Job Bathering - > TargetResource
// Job Build - > TargetResource On TargetLocation
// Job Craft - > TargetResource On TargetLocation(Portal) -> ResourceResult (build, resource)


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
                        bool isExitTargetResource = false;
                        if (p_dataNPC.Inventory == null)
                        {
                            p_dataNPC.Inventory = DataObjectInventory.EmptyInventory();
                            //Debug.Log(Storage.EventsUI.ListLogAdd = "## JOB: dataNPC.Inventory is null");
                        }else
                            isExitTargetResource = job.TargetResource.ToString() == p_dataNPC.Inventory.TypeInventoryObject.ToString();

                        // --- TO PORTAL
                        if (portal != null )
                        {
                            if(isExitTargetResource)
                            {
                                //***** Back to HOME **** (trget is Portal)
                                //p_dataNPC.InventoryObject is ModelNPC;
                                Storage.PortalsManager.AddResourceFromAlien(portal, p_dataNPC);
                            }
                            // --- TAKE RESOURCE
                            bool checkStorageResource = Storage.PortalsManager.CheckStorageResourceForAlien(portal, p_dataNPC);
                            if (!checkStorageResource && isExitTargetResource)
                            {
                                //End job
                                p_dataNPC.Job = null;
                                p_dataNPC.TargetID = string.Empty;
                                p_dataNPC.TargetPosition = Vector3.zero;
                            }
                            //Continue job
                            if (p_dataNPC.Job != null && p_dataNPC.Job.Job != TypesJobs.Bathering)
                                return true; 
                        }
                        // --- TO LOOT && BUILD
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
                                    if(job.Job == TypesJobs.Build)
                                    {
                                        if(p_dataNPC.Inventory == null ||  p_dataNPC.Inventory.IsEmpty ||  p_dataNPC.Inventory.TypeInventoryObject.ToString() != job.ResourceResult.ToString())
                                        {
                                            Debug.Log(Storage.EventsUI.ListLogAdd = "### JOB BUILD: Inventory is Empty >> " + job.Job.ToString() + " " + job.TargetResource + " R:" + job.ResourceResult);
                                            //p_dataNPC.Inventory = DataObjectInventory.EmptyInventory();
                                            return false;
                                        }
                                    }
                                    //p_dataNPC.CurrentAction = GameActionPersonController.NameActionsPerson.Target.ToString();
                                    GameActionPersonController.ExecuteActionNPC(p_dataNPC, GameActionPersonController.NameActionsPerson.Move, controller, true);
                                    // **** FIND RESOURCE ****
                                    //---Replace object
                                    //1. Remove resource
                                    Vector3 posTarget = targetInfo.Data.Position;

                                    bool isTargetTypeTrue = false;
                                    PoolGameObjects.TypePoolPrefabs typePoolResource = CheckFieldJobValid(ref isTargetTypeTrue, job, targetInfo.Data);
                                    if (job.Job == TypesJobs.Build)
                                    {
                                        if (typePoolResource == PoolGameObjects.TypePoolPrefabs.PoolFloor)
                                            GenericWorldManager.ClearLayerObject(targetInfo.Data);
                                        //---- TEST
                                        //else
                                        //    Debug.Log(Storage.EventsUI.ListLogAdd = "TypesJobs.Build .. Not Remove resource: " + job.ResourceResult.ToString() + " >> " + targetInfo.Data.NameObject);
                                    }
                                    else
                                        GenericWorldManager.ClearLayerObject(targetInfo.Data);
                                    //2. Create new resource
                                    if (job.ResourceResult != SaveLoadData.TypePrefabs.PrefabField)
                                    {
                                        bool isTestMe = false;
                                        PaletteMapController.SelCheckOptDel deleteOpt = PaletteMapController.SelCheckOptDel.None;
                                        PaletteMapController.SelCheckOptDel checkOpt = PaletteMapController.SelCheckOptDel.DelTerra;
                                        if (typePoolResource != PoolGameObjects.TypePoolPrefabs.PoolFloor &&
                                            typePoolResource != PoolGameObjects.TypePoolPrefabs.PoolPerson)
                                        {
                                            checkOpt = PaletteMapController.SelCheckOptDel.DelPrefab;
                                            isTestMe = true;
                                        }

                                        //Storage.GenWorld.GenericPrefabOnWorld(job.ResourceResult, posTarget);
                                        ModelNPC.ObjectData spawnObject = Storage.GenWorld.GetCreatePrefab(job.ResourceResult, targetInfo.Field);
                                        bool isSpawned = Storage.Data.AddDataObjectInGrid(spawnObject,
                                                            targetInfo.Field, "CheckJobAlien",
                                                            p_modeDelete: deleteOpt,
                                                            p_modeCheck: checkOpt, 
                                                            p_dataNPC: p_dataNPC);
                                        if (!isSpawned)
                                            Debug.Log(Storage.EventsUI.ListLogAdd = "### JOB [" + job .Job.ToString() + "]: Not Spawn " + spawnObject.NameObject);
                                    }
                                    bool isZonaReal = Helper.IsValidPiontInZona(targetInfo.Data.Position.x, targetInfo.Data.Position.y);
                                    //if (!targetInfo.Data.IsReality && isZonaReal)
                                    if (isZonaReal)
                                    {
                                        Storage.GenGrid.LoadObjectToReal(targetInfo.Field);
                                    }
                                  
                                    //3. Add resource in Inventory (where not Ground)
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

    public static bool ResourceRsultIsFloor(AlienJob job)
    {
        return Storage.GridData.NamesPrefabFloors.Contains(job.ResourceResult.ToString());
    }
    public static bool ResourceRsultIsFlore(AlienJob job)
    {
        return Storage.GridData.NamesPrefabFlore.Contains(job.ResourceResult.ToString());
    }
    public static bool ResourceRsultIsPrefab(AlienJob job)
    {
        return Storage.GridData.NamesPrefabObjects.Contains(job.ResourceResult.ToString());
    }

    public static void CheckFieldsJobValid(ref bool resultIsValid, AlienJob job,  List<ModelNPC.ObjectData> resourcesData)
    {
        foreach (ModelNPC.ObjectData fieldItem in resourcesData)
        {
            CheckFieldJobValid(ref resultIsValid, job, fieldItem);
            if (!resultIsValid)
                break;
        }
    }

    public static PoolGameObjects.TypePoolPrefabs CheckFieldJobValid(ref bool resultIsValid, AlienJob job, ModelNPC.ObjectData objectField)
    {
        bool isResourcePrefabs = Storage.GridData.NamesPrefabObjects.Contains(job.ResourceResult.ToString());
        if(isResourcePrefabs)
        {
            //resultIsValid = !(objectField is ModelNPC.WoodData);
            resultIsValid = !(objectField is ModelNPC.WoodData) &&
                            !(objectField is ModelNPC.PersonData);
            return PoolGameObjects.TypePoolPrefabs.PoolWood;
        }
        bool isResourceFloor = Storage.GridData.NamesPrefabFloors.Contains(job.ResourceResult.ToString());
        if(isResourceFloor)
        {
            //resultIsValid = !(objectField is ModelNPC.FloorData);
            resultIsValid = true; //FIX__FLOOR_BILD
            return PoolGameObjects.TypePoolPrefabs.PoolFloor;
        }
        bool isResourceFlores = Storage.GridData.NamesPrefabFlore.Contains(job.ResourceResult.ToString());
        if (isResourceFlores)
        {
            //resultIsValid = !(objectField is ModelNPC.FloreData);
            resultIsValid = !(objectField is ModelNPC.FloreData) &&
                !(objectField is ModelNPC.PersonData);
            return PoolGameObjects.TypePoolPrefabs.PoolFlore;
        }
        //return Storage.GridData.NamesPrefabObjects.Contains(job.ResourceResult.ToString());

        return PoolGameObjects.TypePoolPrefabs.PoolPerson;
    }

    //bool isResourceFloor = Storage.GridData.NamesPrefabFloors.Contains(job.ResourceResult.ToString());
    //bool isResourceFlores = Storage.GridData.NamesPrefabFlore.Contains(job.ResourceResult.ToString());
    //bool isResourcePrefabs = Storage.GridData.NamesPrefabObjects.Contains(job.ResourceResult.ToString());
    //bool isReplaceTypeFloors = targetInfo.Data is ModelNPC.FloorData && isResourceFloor;
    //bool isReplaceTypeFlores = targetInfo.Data is ModelNPC.FloreData && isResourceFlores;
    //bool isReplaceTypePrefabs = targetInfo.Data is ModelNPC.WoodData && isResourcePrefabs;
    //bool isTargetTypeTrue = isReplaceTypePrefabs || isReplaceTypeFlores || isReplaceTypePrefabs;

    public struct CacheParamSpiralFields
    {
        public int indexSingle;
        public int _single;
        public int limitInvertSingle;
        public int indexFileds;
        public bool isValid;
        public int index;
        public int nextF;
        public int Order;
        public Vector2Int testPosit;
        // public bool isTestPortal;
    }
    public static CacheParamSpiralFields temp_spatal;
    //List<FieldRnd>

    public static void GetSpiralFields_Cache(ref List<FieldRnd> findedFileds, int x, int y, int lenSnake = 8, bool isTestZonaWorls = false, ModelNPC.PortalData portal = null, bool isRandomOrder = false)
    {
        //cacheSpiralFields = new CacheParamSpiralFields();

        temp_spatal.indexSingle = 0;
        temp_spatal._single = 1;
        temp_spatal.limitInvertSingle = 1;
        temp_spatal.indexFileds = 0;
        temp_spatal.isValid = true; // !isTestZonaWorls;
        //temp_spatal.isTestPortal = portal != null && !isTestZonaWorls;
        temp_spatal.index = 0;
        temp_spatal.nextF = 0;
        temp_spatal.Order = 0;


        for (temp_spatal.index = 0; temp_spatal.index < 20; temp_spatal.index++)
        {
            //X
            for (temp_spatal.nextF = 0; temp_spatal.nextF < temp_spatal.index; temp_spatal.nextF++)
            {
                x += temp_spatal._single;
                temp_spatal.isValid = true;
                if (isTestZonaWorls)
                    temp_spatal.isValid = Helper.IsValidFieldInZonaWorld(x, y);
                //FIX__FLOOR_BILD
                //if(portal !=null && temp_spatal.isValid)
                //    temp_spatal.isValid = IsValidFieldInZonaPortal(x, y, portal);

                if (temp_spatal.isValid)
                {
                    if (isRandomOrder)
                        temp_spatal.Order = UnityEngine.Random.Range(0, lenSnake);

                    findedFileds.Add(new FieldRnd(x, y, temp_spatal.Order));
                    temp_spatal.indexFileds++;
                }
                temp_spatal.indexSingle++;
                if (temp_spatal.indexFileds > lenSnake)
                    return;
                if (temp_spatal.indexSingle >= temp_spatal.limitInvertSingle)
                {
                    temp_spatal.limitInvertSingle += 2;
                    temp_spatal.indexSingle = 0;
                    temp_spatal._single *= -1;
                }
            }
            //Y
            for (temp_spatal.nextF = 0; temp_spatal.nextF < temp_spatal.index; temp_spatal.nextF++)
            {
                y += temp_spatal._single;
                temp_spatal.isValid = true;
                if (isTestZonaWorls)
                    temp_spatal.isValid = Helper.IsValidFieldInZonaWorld(x, y);
                //FIX__FLOOR_BILD
                //if (temp_spatal.isValid)
                //    temp_spatal.isValid = IsValidFieldInZonaPortal(x, y, portal);

                if (temp_spatal.isValid)
                {
                    if (isRandomOrder)
                        temp_spatal.Order = UnityEngine.Random.Range(0, lenSnake);

                    findedFileds.Add(new FieldRnd(x, y, temp_spatal.Order));
                    temp_spatal.indexFileds++;
                }
                temp_spatal.indexSingle++;
                if (temp_spatal.indexFileds > lenSnake)
                    return;
                if (temp_spatal.indexSingle >= temp_spatal.limitInvertSingle)
                {
                    temp_spatal.limitInvertSingle += 2;
                    temp_spatal.indexSingle = 0;
                    temp_spatal._single *= -1;
                }
            }
        }
    }

    //public static bool IsFilledLocationConstruction(int x, int y)
    //{

    //    return false;
    //}

    //List<PoolGameObjects.TypePoolPrefabs> filretsIgnorType = null)
    public static bool IsFreeLocationConstruction(
        ref Dictionary<Vector2Int, bool> excludedFreeFileds,
        int x, int y,
        int lenSnake,
        TypesBiomNPC biomType) 

    {
        temp_spatal.indexSingle = 0;
        temp_spatal._single = 1;
        temp_spatal.limitInvertSingle = 1;
        temp_spatal.indexFileds = 0;
        temp_spatal.isValid = true; // !isTestZonaWorls;
        temp_spatal.index = 0;
        temp_spatal.nextF = 0;
        temp_spatal.Order = 0;

        for (temp_spatal.index = 0; temp_spatal.index < 20; temp_spatal.index++)
        {
            //X
            for (temp_spatal.nextF = 0; temp_spatal.nextF < temp_spatal.index; temp_spatal.nextF++)
            {
                x += temp_spatal._single;
                temp_spatal.isValid = true;
                if (temp_spatal.isValid)
                {
                    temp_spatal.testPosit = new Vector2Int(x, y);
                    //already tested on free
                    if (IsNotInListFree(temp_spatal.testPosit, excludedFreeFileds))
                    {
                        //test on free
                        if (IsFreeFieldConstructBiom(x, y, biomType) == false)
                            return false;

                        AddExcludeFreeFiled(temp_spatal.testPosit, excludedFreeFileds);
                    }
                    temp_spatal.indexFileds++;
                }
                temp_spatal.indexSingle++;
                if (temp_spatal.indexFileds > lenSnake)
                    return true;
                if (temp_spatal.indexSingle >= temp_spatal.limitInvertSingle)
                {
                    temp_spatal.limitInvertSingle += 2;
                    temp_spatal.indexSingle = 0;
                    temp_spatal._single *= -1;
                }
            }
            //Y
            for (temp_spatal.nextF = 0; temp_spatal.nextF < temp_spatal.index; temp_spatal.nextF++)
            {
                y += temp_spatal._single;
                temp_spatal.isValid = true;

                if (temp_spatal.isValid)
                {
                    temp_spatal.testPosit = new Vector2Int(x, y);

                    //already tested on free
                    if (IsNotInListFree(temp_spatal.testPosit, excludedFreeFileds))
                    {
                        //test on free
                        if (IsFreeFieldConstructBiom(x, y, biomType) == false)
                            return false;

                        AddExcludeFreeFiled(temp_spatal.testPosit, excludedFreeFileds);
                    }
                    temp_spatal.indexFileds++;
                }
                temp_spatal.indexSingle++;
                if (temp_spatal.indexFileds > lenSnake)
                    return true;
                if (temp_spatal.indexSingle >= temp_spatal.limitInvertSingle)
                {
                    temp_spatal.limitInvertSingle += 2;
                    temp_spatal.indexSingle = 0;
                    temp_spatal._single *= -1;
                }
            }
        }

        return true;
    }

    public static bool FilterData(
        ref ModelNPC.ObjectData findedData,
        List<PoolGameObjects.TypePoolPrefabs> filretsIgnorType = null,
        SaveLoadData.TypePrefabs findType = SaveLoadData.TypePrefabs.PrefabField,
        int x=0, int y=0)
    {
        string nameField = Helper.GetNameField(x, y);
        List<ModelNPC.ObjectData> dataObjs = ReaderScene.GetObjectsDataFromGridContinue(nameField);
        foreach(ModelNPC.ObjectData itemData in dataObjs)
        {
            if (filretsIgnorType.Contains(itemData.TypePoolPrefab))
            {
                findedData = null;
                break;
            }
            else if (itemData.TypePrefab == findType)
            {
                findedData = itemData;
            }
        }
        return true;
    }

    //public static bool IsFieldFree(
    //   List<PoolGameObjects.TypePoolPrefabs> filretsIgnorType = null,
    //   int x = 0, int y = 0)
    //{
    //    string nameField = Helper.GetNameField(x, y);
    //    List<ModelNPC.ObjectData> dataObjs = ReaderScene.GetObjectsDataFromGridContinue(nameField);
    //    foreach (ModelNPC.ObjectData itemData in dataObjs)
    //    {
    //        if (filretsIgnorType.Contains(itemData.TypePoolPrefab))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}



    public static bool IsFreeFieldConstructBiom(int x, int y, TypesBiomNPC biomType)
    {
        string nameField = Helper.GetNameField(x, y);
        List<ModelNPC.ObjectData> dataObjs = ReaderScene.GetObjectsDataFromGridContinue(nameField);
        if (dataObjs == null)
            return true;
        foreach (ModelNPC.ObjectData itemData in dataObjs)
        {
            if(Storage.GridData.GetPrefabsBioms[biomType].Contains(itemData.TypePrefab))
                return false;
        }
        return true;
    }

    public static bool IsFreeFieldConstruct(int x = 0, int y = 0)
    {
        string nameField = Helper.GetNameField(x, y);
        List<ModelNPC.ObjectData> dataObjs = ReaderScene.GetObjectsDataFromGridContinue(nameField);
        if (dataObjs == null)
            return true;
        foreach (ModelNPC.ObjectData itemData in dataObjs)
        {
            if (itemData.TypePoolPrefab != PoolGameObjects.TypePoolPrefabs.PoolFloor)
                return false;
        }
        return true;
    }

    public static void AddExcludeFreeFiled(Vector2Int key, Dictionary<Vector2Int, bool> excludedFreeFileds)
    {
        //if (!excludedFreeFileds.ContainsKey(key))
        excludedFreeFileds.Add(key, true);
    }

    public static bool IsNotInListFree(Vector2Int key, Dictionary<Vector2Int, bool> excludedFreeFileds)
    {
        return excludedFreeFileds.ContainsKey(key) == false;
    }

    public static bool IsFree(Vector2Int key, Dictionary<Vector2Int, bool> excludedFreeFileds)
    {
        return excludedFreeFileds.ContainsKey(key);
    }

    public static void RemoveExcludeFree(Vector2Int key, Dictionary<Vector2Int, bool> excludedFreeFileds)
    {
        if(excludedFreeFileds.ContainsKey(key))
            excludedFreeFileds.Remove(key);
    }
    

}

