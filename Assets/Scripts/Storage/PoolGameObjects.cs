﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//#if UNITY_EDITOR
//[MenuItem("Assets/CreateLocalizationData")]
//#endif
public class PoolGameObjects
{
    public static string TestNamePool = "";

    public PoolGameObjects()
    {
        LoadPoolGameObjects();
    }

    #region Pool
    [NonSerialized]
    public static bool IsUsePoolField = true;
    public static bool IsUsePoolObjects = true; //34e-39e   46e
    public static bool IsTestingDestroy = false;//true; // 
    public static bool IsStack = true; //false;// 

    private int limitPoolOnRemovedBoss = 550;
    private int limitPoolOnRemoved = 1500;
    private int indexPool = 0;

    public List<PoolGameObject> PoolGamesObjects;
    public Dictionary<string, Stack<PoolGameObject>> PoolGamesObjectsStack;

    //[CreateAssetMenu(fileName = "PoolConfig", menuName = "Pool Config", order = 51)]
    class PoolConfig : ScriptableObject
    {
        public int LimitField = 400;
        public int LimitBoss = 230;
        public int LimitPerson = 230;
        public int LimitVood = 100;
        
        public int LimitOthers = 100;

        public int LimitFloor = 250;
        public int LimitFlore = 100;
        public int LimitWood = 100;
        public int LimitWall = 100;
        public int LimitNPC = 250;
    }

    public enum TypePoolPrefabs
    {
        TerraFloor,
        TerraFlore,
        TerraWood,
        Wall,
        Person,
        PersonUFO
    }

   
    void LoadPoolGameObjects()
    {
        PoolConfig poolConfig = new PoolConfig();

        if (IsStack)
        {
            PoolGamesObjectsStack = new Dictionary<string, Stack<PoolGameObject>>();
        }
        else
        {
            PoolGamesObjects = new List<PoolGameObject>();
        }
        //return;
        //foreach (var i in Enumerable.Range(0, 1000))
        foreach (var i in Enumerable.Range(0, poolConfig.LimitField))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabField.ToString(), false);
        }

        //-------------
        foreach (var i in Enumerable.Range(0, poolConfig.LimitBoss))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabBoss.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitOthers))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabUfo.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitVood))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabVood.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitOthers))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabWallWood.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitOthers))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabWallRock.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitOthers))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabRock.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitOthers))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabElka.ToString(), false);
        }

        foreach (var i in Enumerable.Range(0, poolConfig.LimitOthers))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabNPC.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitOthers))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabFlore.ToString(), false);
        }
    }

    /*
    void LoadPoolGameObjects()
    {
        PoolConfig poolConfig = new PoolConfig();

        if (IsStack)
        {
            PoolGamesObjectsStack = new Dictionary<string, Stack<PoolGameObject>>();
        }
        else
        {
            PoolGamesObjects = new List<PoolGameObject>();
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitFloor))
        {
            indexPool = i;
            AddPoolNewTypeObject(TypePoolPrefabs.TerraFloor.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitFlore))
        {
            indexPool = i;
            AddPoolNewTypeObject(TypePoolPrefabs.TerraFlore.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitWood))
        {
            indexPool = i;
            AddPoolNewTypeObject(TypePoolPrefabs.TerraWood.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitWall))
        {
            indexPool = i;
            AddPoolNewTypeObject(TypePoolPrefabs.Wall.ToString(), false);
        }
        //-------------
        foreach (var i in Enumerable.Range(0, poolConfig.LimitNPC))
        {
            indexPool = i;
            AddPoolNewTypeObject(TypePoolPrefabs.Person.ToString(), false);
        }
        foreach (var i in Enumerable.Range(0, poolConfig.LimitOthers))
        {
            indexPool = i;
            AddPoolNewTypeObject(TypePoolPrefabs.PersonUFO.ToString(), false);
        }
    }
    */

    public PoolGameObject AddPoolNewTypeObject(string prefabTag, bool isLog = false)
    {
        GameObject newGO = Storage.GenGrid.FindPrefab(prefabTag, ""); // "" - new object instaint
        PoolGameObject poolObj = new PoolGameObject();
        poolObj.Name = "GameObjectPool " + indexPool++;
        poolObj.Tag = prefabTag;
        //poolObj.Tag = Storage.GridData.GetTypePool(newGO.tag);
        //poolObj.GameObjectNext = newGO;
        poolObj.Init(newGO);

        //poolObj.Deactivate("Add " + poolObj.Name, true);
        poolObj.Deactivate();

        //Fix Tile field 
        if (PoolGameObjects.IsUsePoolObjects)
        {
            var tagPrefab = Storage.GridData.GetTypePool(prefabTag);
            if (tagPrefab == SaveLoadData.TypePrefabs.PrefabField.ToString())
            {
                ModelNPC.TerraData terrD = new ModelNPC.TerraData()
                {
                    ModelView = "Tundra"
                };
                //Update texture Object pool Field default
                terrD.UpdateGameObject(newGO);
            }
        }

        //poolObj.IsLock = false;
        if (IsStack)
        {
            var stackPool = new Stack<PoolGameObject>();
            if (!PoolGamesObjectsStack.ContainsKey(prefabTag))
                PoolGamesObjectsStack.Add(prefabTag, stackPool);
            else
                stackPool = PoolGamesObjectsStack[prefabTag];

            //#test
            int countInPool = PoolGamesObjectsStack[prefabTag].Count;

            stackPool.Push(poolObj);
            //PoolGamesObjectsStack.Add(prefabTag, stackPool);
        }
        else
        {
            PoolGamesObjects.Add(poolObj);
        }

        //Debug.Log("pool +++++ " + newGO.name);
        //if (PoolGamesObjects.Count > limitPoolOnRemoved)
        //{
        //    if (isLog)
        //        Debug.Log("[" + DateTime.Now.ToShortTimeString() + "] Add Pool !!!! limitPoolOnRemoved " + limitPoolOnRemoved + " / " + PoolGamesObjects.Count);
        //    //TestFields(isLog);
        //}
        return poolObj;
    }

    public PoolGameObject AddPoolNewTypeObject2(SaveLoadData.TypePrefabs prefabType, bool isLog = false)
    {
        string prefabTag = prefabType.ToString();
        GameObject newGO = Storage.GenGrid.FindPrefab(prefabTag, "");
        PoolGameObject poolObj = new PoolGameObject();
        poolObj.Name = "GameObjectPool " + indexPool++;
        poolObj.Tag = prefabTag;
        poolObj.Init(newGO);
        //poolObj.Deactivate("Add " + poolObj.Name, true);
        poolObj.Deactivate();

        if (PoolGameObjects.IsUsePoolObjects)
        {
            var tagPrefab = Storage.GridData.GetTypePool(prefabTag);
            if (tagPrefab == SaveLoadData.TypePrefabs.PrefabField.ToString())
            {
                ModelNPC.TerraData terrD = new ModelNPC.TerraData()
                {
                    ModelView = "Tundra"
                };
                //Update texture Object pool Field default
                terrD.UpdateGameObject(newGO);
            }
        }

        if (IsStack)
        {
            var stackPool = new Stack<PoolGameObject>();
            if (!PoolGamesObjectsStack.ContainsKey(prefabTag))
                PoolGamesObjectsStack.Add(prefabTag, stackPool);
            else
                stackPool = PoolGamesObjectsStack[prefabTag];

            //#test
            int countInPool = PoolGamesObjectsStack[prefabTag].Count;

            stackPool.Push(poolObj);
        }
        else
        {
            PoolGamesObjects.Add(poolObj);
        }
        return poolObj;
    }


    public GameObject GetPoolGameObject(string nameObject, string tagPool, Vector3 pos)
    {
        GameObject findGO = null;

        //if (IsTestingDestroy)
        //{
        //    var destroyedPrefabsTest = PoolGamesObjects.Where(p => p.IsLock && p.GameObjectNext == null).ToList();
        //    if (destroyedPrefabsTest.Count > 0)
        //        Debug.Log("/////// Pool contains null object (" + destroyedPrefabsTest.Count + ")  " + destroyedPrefabsTest[0].ToString());
        //}

        //string tagPool = Storage.GridData.GetTypePool(tag);

        PoolGameObject findPoolGO = null;
        int contUnlockPools = 0;
        if (!IsStack)
        {
            //findPoolGO = PoolGamesObjects.Find(p => p.IsLock == false && p.Tag == tag);
            findPoolGO = PoolGamesObjects.Find(p => p.IsLock == false && p.Tag == tagPool);
            contUnlockPools = PoolGamesObjects.Count;
        }
        else
        {
            //var stackPool = new Stack<PoolGameObject>();
            if (PoolGamesObjectsStack.ContainsKey(tagPool))
            {
                contUnlockPools = PoolGamesObjectsStack[tagPool].Count;
                if (contUnlockPools > 0)
                {
                    PoolGameObject returnPool =  PoolGamesObjectsStack[tagPool].Peek();
                    //#FIX ELKA
                    returnPool.Tag = tagPool;

                    if (returnPool==null)
                    {
                        Debug.Log("######## returnPool==null");
                    }
                    if (returnPool.GameObjectNext == null)
                    {
                        Debug.Log("######## returnPool.GameObjectNext==null");
                    }

                    //#TEST
                    int countInPool = PoolGamesObjectsStack[tagPool].Count;
                    if(countInPool==1)
                    {
                        AddPoolNewTypeObject(tagPool, false);
                    }

                    findPoolGO = PoolGamesObjectsStack[tagPool].Pop();

                    if(findPoolGO == null)
                    {
                        Debug.Log("######## findPoolGO==null");
                    }
                    if (findPoolGO.GameObjectNext == null)
                    {
                        Debug.Log("######## findPoolGO.GameObjectNext==null");
                    }
                    
                }
            }

        }

        if (findPoolGO == null)
        {
            //---------------------------- TEST UNLOCk
            if (contUnlockPools > limitPoolOnRemoved)
            {
                if (IsTestingDestroy)
                    Debug.Log("?????? NOT POOL ME:" + nameObject);

                #region test pool limit
                //Debug.Log("%%%%%%%%%%%%%% LIMIT " + PoolGamesObjects.Count + " > " + limitPoolOnRemoved + " " + nameObject);

                //if (tag == "PrefabBoss")
                //{
                //-----------------
                //%%% CREANER %%%%

                //if (IsTestingDestroy)
                //{
                //    int countBoss = PoolGamesObjects.Where(p => p.Tag == "PrefabBoss").Count();
                //    Debug.Log("COUNT POOL BOSS: " + countBoss + @" \ " + limitPoolOnRemovedBoss + "     ME:" + nameObject);
                //    if (countBoss > limitPoolOnRemovedBoss)
                //    {
                //        //var destroyedBoss = PoolGamesObjects.Where(p => p.Tag == "PrefabBoss" && p.IsLock && p.GameObjectNext == null).ToList();
                //        //if (destroyedBoss.Count != 0)
                //        //    Debug.Log("COUNT POOL BOSS DESTROYED >>>> : " + destroyedBoss.Count());
                //        var destroyedPrefabs = PoolGamesObjects.Where(p => p.IsLock && p.GameObjectNext == null).ToList();
                //        //if (destroyedPrefabs.Count != 0 && destroyedBoss.Count != destroyedPrefabs.Count)
                //        if (destroyedPrefabs.Count != 0)
                //            Debug.Log("COUNT POOL DESTROYED >>>> : " + destroyedPrefabs.Count());

                //        //foreach (var item in destroyedPrefabs)
                //        for (int ind = 0; ind < destroyedPrefabs.Count; ind++)
                //        {
                //            PoolGamesObjects.RemoveAt(ind);
                //        }
                //    }
                //}
                //-----------------
                //}
                #endregion
            }
            //----------------------------

            findPoolGO = AddPoolNewTypeObject(tagPool);
        }

        //#FIX ELKA
        //findPoolGO.Tag = tagPool;

        findPoolGO.Activate(nameObject, tagPool, pos);
        findGO = findPoolGO.GameObjectNext;

        //---------------
        EventsObject evenObj = findGO.GetComponent<EventsObject>();
        if(evenObj!=null)
        {
            if(findPoolGO==null)
            {
                Debug.Log("#######  GetPoolGameObject   findPoolGO is null " + findGO.name);
            }
            evenObj.PoolCase = findPoolGO;
        }
        else
        {
            Debug.Log("#######  GetPoolGameObject   NOT EventsObject " + findGO.name);
        }

        if (IsTestingDestroy)
        {
            if (findGO == null)
                Debug.Log("###########///////////// Pool contrains null object");
        }

        return findGO;
    }

    public GameObject InstantiatePool(GameObject prefabField, Vector3 pos, string nameFieldNew)
    {
        nameFieldNew = string.IsNullOrEmpty(nameFieldNew) ? prefabField.name : nameFieldNew;
        GameObject findGO = GetPoolGameObject(nameFieldNew, prefabField.tag, pos);

        return findGO;
    }

    public void Restart()
    {
        if (IsStack)
        {
            PoolGamesObjectsStack.Clear();
            LoadPoolGameObjects();
        }
    }

    public bool DestroyPoolGameObject(GameObject delGO)
    {
        if (delGO == null)
            return false;
        if (string.IsNullOrEmpty(delGO.name))
            return false;

        //....

        EventsObject evenObj = delGO.GetComponent<EventsObject>();
        PoolGameObject itemPool = evenObj.PoolCase;
        if (itemPool != null)
        {
            //itemPool.Deactivate(delGO.name);
            itemPool.Deactivate();
            //return true;
        }
        else
        {
            Debug.Log("########### PoolCase IS NULL");
            return false;
        }


        if (IsStack)
        {
            if (itemPool != null)
            {
                //itemPool.Deactivate(delGO.name);
                PoolGamesObjectsStack[itemPool.Tag].Push(itemPool);
            }
        }

        return true;
    }


    private void TestRealGO()
    {
        //List<GameObject> poolsTesting = Storage.Instance.GamesObjectsReal.SelectMany(x => x.Value).Where(p=> p.tag =="Field" || p.tag == "PrefabField").ToList();
        //for (int i = poolsTesting.Count() - 1; i >= 0; i--)
        //{
        //    var pool = poolsTesting[i];
        //    bool inZona = Helper.IsValidPiontInZonaCorr(pool.GameObjectNext.transform.position.x, pool.GameObjectNext.transform.position.y);
        //    if (!inZona)
        //    {
        //        Debug.Log("TEST Pool Not in Zona: " + pool.NameObject + " " + pool.GameObjectNext.transform.position.x + "x" + pool.GameObjectNext.transform.position.y +
        //            "   hero=" + Storage.Instance.HeroPositionX + "x" + Storage.Instance.HeroPositionY +
        //            "   zona: " + Storage.Instance.ZonaReal.X + "x" + Storage.Instance.ZonaReal.Y + " - " + Storage.Instance.ZonaReal.X2 + "x" + Storage.Instance.ZonaReal.Y2);
        //    }
        //}
    }

    private void TestGameObjects()
    {
        //-----------------------
        //var gobjs = GameObject.FindGameObjectsWithTag("Field");
        //foreach(var gobj in gobjs)
        //{
        //    var dist = Vector2.Distance(gobj.transform.position, Storage.Instance.HeroObject.transform.position);
        //    //if (!inZona)
        //    if (dist > 40)
        //    {
        //        Debug.Log("TEST Pool Dist[" + dist + "]: " + gobj.name + " " + gobj.transform.position.x + "x" + gobj.transform.position.y +
        //            "   hero=" + Storage.Instance.HeroPositionX + "x" + Storage.Instance.HeroPositionY +
        //            "   zona: " + Storage.Instance.ZonaReal.X + "x" + Storage.Instance.ZonaReal.Y + " - " + Storage.Instance.ZonaReal.X2 + "x" + Storage.Instance.ZonaReal.Y2);
        //        //PoolGamesObjects.Remove(pool);
        //        //pool.Deactivate();
        //    }
        //}

        //var gobjsP = GameObject.FindGameObjectsWithTag("FieldPrefab");
        //foreach (var gobj in gobjsP)
        //{
        //    var dist = Vector2.Distance(gobj.transform.position, Storage.Instance.HeroObject.transform.position);
        //    //if (!inZona)
        //    if (dist > 40)
        //    {
        //        Debug.Log("TEST Pool Dist[" + dist + "]: " + gobj.name + " " + gobj.transform.position.x + "x" + gobj.transform.position.y +
        //            "   hero=" + Storage.Instance.HeroPositionX + "x" + Storage.Instance.HeroPositionY +
        //            "   zona: " + Storage.Instance.ZonaReal.X + "x" + Storage.Instance.ZonaReal.Y + " - " + Storage.Instance.ZonaReal.X2 + "x" + Storage.Instance.ZonaReal.Y2);
        //        //PoolGamesObjects.Remove(pool);
        //        //pool.Deactivate();
        //    }
        //}
    }
    #endregion
}

public class PoolGameObject
{
    public DateTime TimeCreate;
    public string Name = "";
    public string NameObject = "";
    public string Tag = "";
    public GameObject GameObjectNext { get; private set; }
    public bool IsLock { get; set; }
    public bool IsDesrtoy { get; set; } 

    //public PoolGameObject(bool isCreateDefault = true)
    public PoolGameObject()
    {
        IsLock = true;
        TimeCreate = DateTime.Now;
        IsDesrtoy = false;
    }

    public void Init(GameObject newGO)
    {
        //#FIX POOL
        Tag = Storage.GridData.GetTypePool(newGO.tag);
        //Tag = newGO.tag;

        GameObjectNext = newGO;
        GameObjectNext.name = Name + "_Empty" + Tag;
        GameObjectNext.transform.SetParent(Storage.GenGrid.PanelPool.transform);

        //#TEST
        //if (NameObject.IndexOf("Elka") != -1 && Tag == "PrefabVood")
        //{
        //    Debug.Log("######## Elka !");
        //}
        //if (NameObject.IndexOf("Elka") != -1 && Tag != "PrefabVood")
        //{
        //    Debug.Log("######## Elka");
        //}
    }

    public override string ToString()
    {
        string infoObj = GameObjectNext == null ? "NOT" : GameObjectNext.name + 
            " pool:" + GameObjectNext.GetComponent<EventsObject>().PoolCase.Name + " " + 
            GameObjectNext.GetComponent<EventsObject>().PoolCase.NameObject;
        string info = "[" + TimeCreate.TimeOfDay + "] " + Name + " " + NameObject + " -- " + Tag + " " + infoObj;

        //return base.ToString();
        return info;
    }

    public void Activate(string nameObj, string tag, Vector3 pos)
    {

        //#TEST
        if (PoolGameObjects.IsTestingDestroy)
        {
            if (GameObjectNext == null)
            {
                Debug.Log("##########\\\\\\\\\\\\\\\\ Pool contains null object ");
            }
        }

        IsLock = true;
        NameObject = nameObj;

        TimeCreate = DateTime.Now;
        if (GameObjectNext == null)
        {
            Debug.Log("#### Activate >> Object is null " + NameObject);
            return;
        }

        //GameObjectNext.SetActive(true);
        GameObjectNext.transform.SetParent(null);
        
        //#FIX TAG
        if(GameObjectNext.tag == "Field")
            GameObjectNext.tag = tag;

        GameObjectNext.transform.position = pos;
        GameObjectNext.name = nameObj;
        GameObjectNext.GetComponent<SpriteRenderer>().color = Color.white;

        var movement = GameObjectNext.GetComponent<MovementBoss>();
        if (movement == null)
        {
            //Debug.Log("SetActive NO NPC " + nameObj);
            GameObjectNext.SetActive(true);
        }

        //#TEST
        if (PoolGameObjects.IsTestingDestroy)
        {
            if (GameObjectNext == null)
            {
                Debug.Log("##########\\\\\\\\\\\\\\\\ Pool contains null object ");
            }
        }
    }


    //public void Deactivate(string gobjName="", bool isCreatedNew = false, GameObject gobj =null)
    public void Deactivate()
    {
        if (GameObjectNext == null)
        {
            Debug.Log("#### Deactivate >> Object is null " + NameObject);
            return;
        }


        NameObject = "";

        GameObjectNext.SetActive(false);
        GameObjectNext.transform.SetParent(Storage.GenGrid.PanelPool.transform);


        IsLock = false;

        //#TEST
        if (PoolGameObjects.IsTestingDestroy)
        {
            if (GameObjectNext == null)
            {
                Debug.Log("##########\\\\\\\\\\\\\\\\ Pool contains null object ");
            }
        }
    }

}
