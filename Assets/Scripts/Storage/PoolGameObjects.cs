using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolGameObjects
{

    public PoolGameObjects()
    {
        LoadPoolGameObjects();
    }

    #region Pool

    [NonSerialized]
    //public bool IsUsePoolField = false;
    public static bool IsUsePoolField = true;
    public static bool IsUsePoolObjects = true;
    //public static bool IsUsePoolObjects = false;

    int limitPoolOnRemoved = 450;
    int indexPool = 0;
    public List<PoolGameObject> PoolGamesObjects;


    void LoadPoolGameObjects()
    {
        PoolGamesObjects = new List<PoolGameObject>();
        //return;
        //foreach (var i in Enumerable.Range(0, 1000))
        foreach (var i in Enumerable.Range(0, 100))
        {
            indexPool = i;
            AddPoolNewTypeObject(SaveLoadData.TypePrefabs.PrefabField.ToString(), false);
        }
    }

    public PoolGameObject AddPoolNewTypeObject(string prefabTag, bool isLog = false)
    {
        GameObject newGO = Storage.GenGrid.FindPrefab(prefabTag,"");
        PoolGameObject poolObj = new PoolGameObject();
        poolObj.Name = "GameObjectPool " + indexPool++;
        poolObj.Tag = prefabTag;
        //poolObj.GameObjectNext = newGO;
        poolObj.Init(newGO);
        poolObj.IsLock = false;
        PoolGamesObjects.Add(poolObj);

        if (PoolGamesObjects.Count > limitPoolOnRemoved)
        {
            if (isLog)
                Debug.Log("[" + DateTime.Now.ToShortTimeString() + "] Add Pool !!!! limitPoolOnRemoved " + limitPoolOnRemoved + "  / " + PoolGamesObjects.Count);
            TestFields(isLog);
        }

        return poolObj;
    }


    public GameObject GetPoolGameObject(string nameObject, string tag, Vector3 pos)
    {
        GameObject findGO = null;

        PoolGameObject findPoolGO = PoolGamesObjects.Find(p => p.IsLock == false && p.Tag == tag);
        if (findPoolGO == null)
        {
            findPoolGO = AddPoolNewTypeObject(tag);
        }
        findPoolGO.Activate(nameObject, tag, pos);
        findGO = findPoolGO.GameObjectNext;

        //---------------
        EventsObject evenObj = findGO.GetComponent<EventsObject>();
        if(evenObj!=null)
        {
            evenObj.PoolCase = findPoolGO;
        }
        //---------------

        return findGO;
    }

    //public GameObject GetPoolGameObject(string nameObject, string tag, Vector3 pos)
    //{
    //    GameObject findGO = null;
    //    PoolGameObject findPoolGO = PoolGamesObjects.Find(p => p.IsLock == false && p.Tag == tag);
    //    if (findPoolGO == null)
    //    {
    //        findPoolGO = AddPoolNewTypeObject(tag);
    //    }
    //    findPoolGO.Activate(nameObject, tag, pos);
    //    findGO = findPoolGO.GameObjectNext;

    //    return findGO;
    //}

    public GameObject InstantiatePool(GameObject prefabField, Vector3 pos, string nameFieldNew)
    {
        nameFieldNew = string.IsNullOrEmpty(nameFieldNew) ? prefabField.name : nameFieldNew;
        GameObject findGO = GetPoolGameObject(nameFieldNew, prefabField.tag, pos);

        return findGO;
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
            itemPool.Deactivate();
        }
        else
        {
            //....
            int indexLoop = PoolGamesObjects.FindIndex(p => p.NameObject == delGO.name);
            if (indexLoop != -1)
            {
                itemPool = PoolGamesObjects[indexLoop];
                //Debug.Log("~~~~~~~~ DestroyPoolGameObject yes pool (" + indexLoop + ") :" + delGO.name);
                itemPool.Deactivate();
            }
            else
            {
                Debug.Log("~~~~~~~~ DestroyPoolGameObject NOT pool !!! :" + delGO.name + "  PosHero=" + Storage.Instance.SelectFieldPosHero);
                //Destroy(delGO);
                return false;
            }
            //....
        }

        return true;
    }

    //public bool DestroyPoolGameObject(GameObject delGO)
    //{
    //    if (delGO == null)
    //        return false;
    //    if (string.IsNullOrEmpty(delGO.name))
    //        return false;

    //    //....
    //    //EventsObject


    //    int indexLoop = PoolGamesObjects.FindIndex(p => p.NameObject == delGO.name);

    //    if (indexLoop != -1)
    //    {
    //        var itemPool = PoolGamesObjects[indexLoop];
    //        //Debug.Log("~~~~~~~~ DestroyPoolGameObject yes pool (" + indexLoop + ") :" + delGO.name);
    //        itemPool.Deactivate();
    //    }
    //    else
    //    {
    //        Debug.Log("~~~~~~~~ DestroyPoolGameObject NOT pool !!! :" + delGO.name + "  PosHero=" + Storage.Instance.SelectFieldPosHero);
    //        //Destroy(delGO);
    //        return false;
    //    }
    //    return true;
    //}



    private void CleanerPool(GameObject gobj, bool isLog = false)
    {
        PoolGameObject[] poolsTesting;

        string nameField = Helper.GetNameFieldObject(gobj);
        if (Storage.Instance.Fields.ContainsKey(nameField))
        {
            if (isLog)
                Debug.Log("///// 1. CleanerPool: Fields list  --  Removed: " + nameField);
            Storage.Instance.Fields.Remove(nameField);
        }
        //if (Storage.Instance.GamesObjectsReal.ContainsKey(nameField))
        //{
        //    foreach (var item in Storage.Instance.GamesObjectsReal[nameField])
        //    {
        //        if (isLog)
        //            Debug.Log("///// 66. CleanerPool: Destroy real object: " + item.name);
        //        Storage.Instance.AddDestroyGameObject(item);
        //        //Storage.Instance.DestroyObject(item);
        //    }
        //}
        Storage.GenGrid.RemoveRealObjects(nameField);

        if (isLog)
            Debug.Log("///// 2. CleanerPool: Destroy (not pool): " + gobj.name);
        if(gobj!=null)
            Storage.Instance.DestroyObject(gobj);

        poolsTesting = PoolGamesObjects.Where(p => p.NameObject == gobj.name).ToArray();
        for (int i = poolsTesting.Count() - 1; i >= 0; i--)
        {
            var pool = poolsTesting[i];
            if (isLog)
                Debug.Log("///// 3. CleanerPool: Removed Pool null: " + pool.Name + "   " + pool.NameObject);
            PoolGamesObjects.Remove(pool);
        }
    }

    private void TestFields(bool isLog = false)
    {
        //.......
        //Single dist = 0;
        List<GameObject> poolsTesting = Storage.Instance.Fields.Select(x => x.Value).Where(p => p.tag == "Field" || p.tag == "PrefabField").ToList();
        for (int i = poolsTesting.Count() - 1; i >= 0; i--)
        {
            GameObject gobj = poolsTesting[i];
            //bool inZona = Helper.IsValidPiontInZonaCorr(gobj.transform.position.x, gobj.transform.position.y);
            bool inZona = Helper.IsValidFieldInZona(gobj.name);

            if (!inZona)
            {
                if (isLog)
                {
                    Debug.Log("TEST Pool Not in Zona: " + gobj.name + " " + gobj.transform.position.x + "x" + gobj.transform.position.y +
                        "   hero=" + Storage.Instance.HeroPositionX + "x" + Storage.Instance.HeroPositionY +
                        "   zona: " + Storage.Instance.ZonaReal.X + "x" + Storage.Instance.ZonaReal.Y + " - " + Storage.Instance.ZonaReal.X2 + "x" + Storage.Instance.ZonaReal.Y2);
                }

                gobj.GetComponent<SpriteRenderer>().color = Color.red;
                //StartCoroutine(StartCleanerPool(gobj));
                CleanerPool(gobj, isLog);
            }
        }
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

    private void TestPooling()
    {
        foreach (var pool in PoolGamesObjects.Where(p => p.IsLock && p.GameObjectNext == null))
        {
            Debug.Log("TEST Pool EMPTY and LOCKED !!!!! " + pool.Name);
        }

        try
        {
            PoolGameObject[] poolsTesting = PoolGamesObjects.Where(p => p.IsLock && p.GameObjectNext != null).ToArray();
            //foreach (var pool in PoolGamesObjects.Where(p => p.IsLock && p.GameObjectNext != null))
            for (int i = poolsTesting.Count() - 1; i >= 0; i--)
            {
                var pool = poolsTesting[i];
                //Storage.Instance.HeroObject.transform.position, pool.GameObjectNext.transform.position
                //bool inZona = Helper.IsValidPiontInZonaCorr(pool.GameObjectNext.transform.position.x, pool.GameObjectNext.transform.position.y);
                var dist = Vector2.Distance(pool.GameObjectNext.transform.position, Storage.Instance.HeroObject.transform.position);
                //if (!inZona)
                if (dist > 40)
                {
                    Debug.Log("TEST Pool Dist[" + dist + "]: " + pool.NameObject + " " + pool.GameObjectNext.transform.position.x + "x" + pool.GameObjectNext.transform.position.y +
                        "   hero=" + Storage.Instance.HeroPositionX + "x" + Storage.Instance.HeroPositionY +
                        "   zona: " + Storage.Instance.ZonaReal.X + "x" + Storage.Instance.ZonaReal.Y + " - " + Storage.Instance.ZonaReal.X2 + "x" + Storage.Instance.ZonaReal.Y2);
                    //PoolGamesObjects.Remove(pool);
                    pool.Deactivate();
                }
            }


            poolsTesting = PoolGamesObjects.Where(p => p.GameObjectNext == null).ToArray();
            for (int i = poolsTesting.Count() - 1; i >= 0; i--)
            {
                var pool = poolsTesting[i];
                Debug.Log("TEST Pool  is EMPTY  " + pool.Name + " " + pool.NameObject + "   " + pool.Tag + "    " + pool.TimeCreate);
            }

            //PoolGameObject[] poolsTesting = PoolGamesObjects.Where(p => p.IsLock && p.GameObjectNext != null).ToArray();
            ////foreach (var pool in PoolGamesObjects.Where(p => p.IsLock && p.GameObjectNext != null))
            //for (int i = poolsTesting.Count() - 1; i >= 0; i--)
            //{
            //    var pool = poolsTesting[i];
            //    //Storage.Instance.HeroObject.transform.position, pool.GameObjectNext.transform.position
            //    bool inZona = Helper.IsValidPiontInZonaCorr(pool.GameObjectNext.transform.position.x, pool.GameObjectNext.transform.position.y);
            //    if (!inZona)
            //    {
            //        Debug.Log("TEST Pool Not in Zona: " + pool.NameObject + " " + pool.GameObjectNext.transform.position.x + "x" + pool.GameObjectNext.transform.position.y +
            //            "   hero=" + Storage.Instance.HeroPositionX + "x" + Storage.Instance.HeroPositionY +
            //            "   zona: " + Storage.Instance.ZonaReal.X +"x" + Storage.Instance.ZonaReal.Y + " - " + Storage.Instance.ZonaReal.X2 + "x" + Storage.Instance.ZonaReal.Y2);
            //        //PoolGamesObjects.Remove(pool);
            //        pool.Deactivate();
            //    }
            //}

            //poolsTesting = PoolGamesObjects.OrderBy(p => p.TimeCreate).Take(5).ToArray();
            //for (int i = poolsTesting.Count() - 1; i >= 0; i--)
            //{
            //    var pool = poolsTesting[i];
            //    Debug.Log("TEST Pool old time " + pool.NameObject + " = " + pool.TimeCreate);
            //    if (pool.IsLock && pool.GameObjectNext != null)
            //    {
            //        //bool inZona = Helper.IsValidPiontInZona(pool.GameObjectNext.transform.position.x, pool.GameObjectNext.transform.position.y);
            //        bool inZona = Helper.IsValidPiontInZonaCorr(pool.GameObjectNext.transform.position.x, pool.GameObjectNext.transform.position.y);
            //        if (!inZona)
            //        {
            //            Debug.Log("TEST Pool old time " + pool.NameObject + " > " + pool.GameObjectNext.name + "    " + pool.GameObjectNext.transform.position);
            //            //PoolGamesObjects.Remove(pool);
            //            pool.Deactivate();
            //        }
            //    }
            //}
        }
        catch (Exception x)
        {
            Debug.Log("########### AddPoolNewTypeObject REMOVED " + x.Message);
        }

        //PoolGameObject removedPool = PoolGamesObjects.Where(p=>p.IsLock == false).OrderBy(p => p.TimeCreate).FirstOrDefault();
        //PoolGameObject removedPoolFirst = PoolGamesObjects.Where(p => p.IsLock == false).OrderByDescending(p => p.TimeCreate).FirstOrDefault();
        //if (removedPool!=null)
        //{
        //    Debug.Log("Removed pool: 0:" + removedPool.TimeCreate + " " + removedPool.Name + "       first: " + removedPoolFirst.TimeCreate + " " + removedPoolFirst.Name );
        //    Debug.Log("Removed pool: " + removedPool.Name + "   no lock=" + PoolGamesObjects.Where(p => p.IsLock == false).Count());
        //    PoolGamesObjects.Remove(removedPool);
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

    //public PoolGameObject(bool isCreateDefault = true)
    public PoolGameObject()
    {
        IsLock = true;
        TimeCreate = DateTime.Now;
    }

    public void Init(GameObject newGO)
    {
        Tag = newGO.tag;

        GameObjectNext = newGO;
        GameObjectNext.name = Name + "_Empty" + Tag;
        GameObjectNext.transform.SetParent(Storage.GenGrid.PanelPool.transform);
        GameObjectNext.tag = "InPool";

        //GameObjectNext.AddComponent<Transform>();
        //GameObjectNext.AddComponent<SpriteRenderer>();

        //GameObjectNext.SetActive(false);
        //GameObjectNext.transform.SetParent(PanelPoolField.transform);
    }

    public void Activate(string nameObj, string tag, Vector3 pos)
    {
        IsLock = true;
        NameObject = nameObj;

        TimeCreate = DateTime.Now;
        if (GameObjectNext == null)
        {
            Debug.Log("#### Activate >> Object is null " + NameObject);
            return;

            //GameObjectNext = Storage.GenGrid.FindPrefab(Tag);
            //if(GameObjectNext==null)
            //{
            //    Debug.Log("#### Activate >> Object is null and Not create prefab: " + Tag);
            //    return;
            //}
        }

        GameObjectNext.SetActive(true);
        GameObjectNext.transform.SetParent(null);
        GameObjectNext.tag = tag;
        GameObjectNext.transform.position = pos;
        GameObjectNext.name = nameObj;

        GameObjectNext.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Deactivate()
    {
        if (GameObjectNext == null)
        {
            Debug.Log("#### Deactivate >> Object is null " + NameObject);
            //GameObjectNext = Storage.GenGrid.FindPrefab(Tag);
            //NameObject = "";
            //IsLock = false;
            return;
        }
        NameObject = "";

        GameObjectNext.SetActive(false);
        GameObjectNext.transform.SetParent(Storage.GenGrid.PanelPool.transform);
        GameObjectNext.tag = "InPool";
        //GameObjectNext.transform.position = pos;

        IsLock = false;
    }

}
