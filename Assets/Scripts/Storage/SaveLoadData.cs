using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;
using System.Xml;
using System.Xml.Linq;

public class SaveLoadData : MonoBehaviour {

    //@NEWPREFAB@
    public GameObject PrefabVood;
    public GameObject PrefabRock;
    public GameObject PrefabUfo;
    public GameObject PrefabBoss;
    public GameObject PrefabElka;
    public GameObject PrefabWallRock;
    public GameObject PrefabWallWood;
    public GameObject PrefabField;

    ////--- TAILS ---
    //public GameObject BackPalette;
    //public Grid GridTails;
    //public GameObject TailsMap;

    //public GameObject 
    public static float Spacing = 2f;

    private GenerateGridFields _scriptGrid;
    private float LoadingWordlTimer = 0f;

    //#################################################################################################
    //>>> ObjectData -> GameDataNPC -> PersonData -> 
    //>>> ObjectData -> GameDataNPC -> PersonData -> PersonDataBoss -> GameDataBoss
    //>>> ObjectData -> GameDataUfo
    //>>> ObjectData -> GameDataNPC -> GameDataOther
    //#################################################################################################



    private IEnumerable<string> _namesPrefabs
    {   get
        {
            var list = new List<string>();
            foreach (var nextType in Enum.GetValues(typeof(TypePrefabs)))
            {
                list.Add(nextType.ToString());
            }
            return list;
        }
    }

    public enum TypePrefabs
    {
        PrefabField,
        PrefabRock,
        PrefabVood,
        PrefabUfo,
        PrefabBoss,
        PrefabElka,
        PrefabWallRock,
        PrefabWallWood,
    }
    

    void Start()
    {
        InitData();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void InitData()
    {
        _scriptGrid = GetComponent<GenerateGridFields>();
    }

    //#.D 
    public void CreateDataGamesObjectsWorld(bool isAlwaysCreate = false)
    {
        //# On/Off
        //isAlwaysCreate = true;

        if (Storage.Instance.GridDataG != null && !isAlwaysCreate)
        {
            Debug.Log("# CreateDataGamesObjectsWorld... Game is loaded              Storage.Instance.GridDataG:    " + Storage.Instance.GridDataG);
            return;
        }

        int coutCreateObjects = 0;
        TypePrefabs prefabName = TypePrefabs.PrefabField;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        int countAll = Helper.HeightLevel * 2;
        int index =1;

        for (int y = 0; y < Helper.HeightLevel; y++)
        {
            for (int x = 0; x < Helper.WidthLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 3);

                int maxObjectInField = (intRndCount == 0) ? 1 : 0;
                string nameField = Helper.GetNameField(x, y);

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {
                    //GEN -----
                    prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
                    //if (prefabName == TypePrefabs.PrefabField)
                    //    continue;
                    //-----------

                    index++;
                    Storage.Events.SetTittle = String.Format("Loading {0} %", (countAll / index).ToString());

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ModelNPC.ObjectData objDataSave = BildObjectData(prefabName, true);
                    objDataSave.NameObject = nameObject;
                    objDataSave.TagObject = prefabName.ToString();
                    objDataSave.Position = pos;

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
        }

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        Storage.Events.SetTittle = String.Format("World is Loaded");
    }


    //GEN -----
    private TypePrefabs GenObjectWorld()
    {
        int intGen = UnityEngine.Random.Range(1, 4);
        int intTypePrefab = 0;
        TypePrefabs prefabName = TypePrefabs.PrefabField;

        if (intGen == 1)
            prefabName = TypePrefabs.PrefabBoss;
        else
        {
            intTypePrefab = UnityEngine.Random.Range(1, 8);
            prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;
        }

        //Type prefab
        //++ Elka, WallRock, WallWood
        //int intTypePrefab = UnityEngine.Random.Range(1, 8);
        //#TT YES BOSS
        //int intTypePrefab = UnityEngine.Random.Range(1, 5);
        //#TT YES UFO
        //int intTypePrefab = UnityEngine.Random.Range(1, 4);
        //#TT NOT UFO
        //int intTypePrefab = UnityEngine.Random.Range(1, 3);

        //int rnd1 = UnityEngine.Random.Range(1, 3);
        //int rnd1 = UnityEngine.Random.Range(1, 2);
        int rnd1 = UnityEngine.Random.Range(1, 3);
        if (rnd1!=1)
        {
            prefabName = TypePrefabs.PrefabField;
        }
        //TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;


        //prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
        return prefabName;
    }

    IEnumerator CreateDataGamesObjectsExtremalWorldProgress()
    {
        int coutCreateObjects = 0;
        TypePrefabs prefabName = TypePrefabs.PrefabField;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        int countAll = Helper.HeightLevel * 2;
        int index = 1;

        for (int y = 0; y < Helper.HeightLevel; y++)
        {
            for (int x = 0; x < Helper.WidthLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 5);

                int maxObjectInField = 1;

                if (intRndCount == 0)
                    maxObjectInField = 2;
                else
                    maxObjectInField = 1;

                intRndCount = UnityEngine.Random.Range(0, 3);

                string nameField = Helper.GetNameField(x, y);

                index++;

                
                Storage.Events.SetTittle = String.Format("{0} % [{1}]", (countAll / index).ToString(), index.ToString());

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {
                    //GEN -----
                    //prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
                    //if (prefabName == TypePrefabs.PrefabField)
                    //continue;
                    //-----------
                    if (i == 0)
                        prefabName = TypePrefabs.PrefabField;
                    else
                        prefabName = TypePrefabs.PrefabBoss;

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ModelNPC.ObjectData objDataSave = BildObjectData(prefabName, true);
                    objDataSave.NameObject = nameObject;
                    objDataSave.TagObject = prefabName.ToString();
                    objDataSave.Position = pos;

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
            yield return null;
        }

        Storage.Events.SetTittle = String.Format("World Loaded");

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        yield break;
    }

    IEnumerator CreateDataGamesObjectsExtremalTerraWorldProgress()
    {
        int coutCreateObjects = 0;
        TypePrefabs prefabName = TypePrefabs.PrefabField;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        int countAll = Helper.HeightLevel * 2;
        int index = 1;

        for (int y = 0; y < Helper.HeightLevel; y++)
        {
            for (int x = 0; x < Helper.WidthLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 5);

                int maxObjectInField = 1;

                if (intRndCount == 0)
                    maxObjectInField = 2;
                else
                    maxObjectInField = 1;

                intRndCount = UnityEngine.Random.Range(0, 3);

                string nameField = Helper.GetNameField(x, y);

                index++;


                Storage.Events.SetTittle = String.Format("{0} % [{1}]", (countAll / index).ToString(), index.ToString());

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {
                    //GEN -----
                    //prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
                    //if (prefabName == TypePrefabs.PrefabField)
                    //continue;
                    //-----------
                    if (i == 0)
                        prefabName = TypePrefabs.PrefabField;
                    else if (i == 1)
                    {
                        if(intRndCount==1)
                            prefabName = TypePrefabs.PrefabVood;
                        else
                            prefabName = TypePrefabs.PrefabWallWood;
                    }

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ModelNPC.ObjectData objDataSave = BildObjectData(prefabName, true);
                    objDataSave.NameObject = nameObject;
                    objDataSave.TagObject = prefabName.ToString();
                    objDataSave.Position = pos;

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
            yield return null;
        }

        Storage.Events.SetTittle = String.Format("World Loaded");

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        yield break;
    }

    public void CreateDataGamesObjectsExtremalWorld()
    {
        //Storage.GamePause = true;
        //Storage.Instance.StopGame();
        //Storage.Instance.DestroyAllGamesObjects();

        StartCoroutine(CreateDataGamesObjectsExtremalWorldProgress());
        //StartCoroutine(CreateDataGamesObjectsExtremalTerraWorldProgress());

        //Storage.GamePause = false;
    }

    public void CreateDataGamesObjectsExtremalWorld__()
    {
        int coutCreateObjects = 0;
        TypePrefabs prefabName = TypePrefabs.PrefabField;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        int countAll = Helper.HeightLevel * 2;
        int index = 1;

        for (int y = 0; y < Helper.HeightLevel; y++)
        {
            for (int x = 0; x < Helper.WidthLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 5);

                int maxObjectInField = 1;

                if (intRndCount == 0)
                    maxObjectInField = 2;
                else
                    maxObjectInField = 1;

                intRndCount = UnityEngine.Random.Range(0, 3);

                string nameField = Helper.GetNameField(x, y);

                index++;
                Storage.Events.SetTittle = String.Format("Loading {0} %", (countAll / index).ToString());

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {
                    //GEN -----
                    //prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
                    //if (prefabName == TypePrefabs.PrefabField)
                    //continue;
                    //-----------
                    if (i == 0)
                        prefabName = TypePrefabs.PrefabField;
                    else
                        prefabName = TypePrefabs.PrefabBoss;

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ModelNPC.ObjectData objDataSave = BildObjectData(prefabName, true);
                    objDataSave.NameObject = nameObject;
                    objDataSave.TagObject = prefabName.ToString();
                    objDataSave.Position = pos;

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
        }

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        Storage.Events.SetTittle = String.Format("World is Loaded");

        
    }


    //--------------- LINK: public GameObject CreatePrefabByName(ModelNPC.ObjectData objData)
    public static ModelNPC.ObjectData CreateObjectData(GameObject p_gobject)
    {
        ModelNPC.ObjectData newObject;
        //#PPPP
        TypePrefabs prefabType = TypePrefabs.PrefabField;

        if (!String.IsNullOrEmpty(p_gobject.tag))
            prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), p_gobject.tag.ToString()); ;

        switch (prefabType)
        {
            case TypePrefabs.PrefabUfo:
                //var newObject1 = new GameDataUfo()
                newObject = new ModelNPC.GameDataUfo()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position
                };
                //Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "   newObject=" + newObject + "             ~~~~~ DO: pos=" + newObject.Position + "  GO:  pos=" + p_gobject.transform.position);
                //newObject1.UpdateGameObject(p_gobject);
                newObject.UpdateGameObject(p_gobject);
                break;
            case TypePrefabs.PrefabBoss:
                //var newObject2 = new GameDataBoss()
                newObject = new ModelNPC.GameDataBoss()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position
                };
                Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "   newObject=" + newObject + "             ~~~~~ DO: pos=" + newObject.Position + "  GO:  pos=" + p_gobject.transform.position);
                Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "  TYPE: " + newObject.GetType());
                //newObject2.UpdateGameObject(p_gobject);
                newObject.UpdateGameObject(p_gobject);
                break;
            case TypePrefabs.PrefabField:
                //newObject = new ModelNPC.TerraData(isGen: true)
                newObject = new ModelNPC.TerraData()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position,
                    Index = 0,
                    BlockResources = 100,
                    //TileName = Storage.TilesManager.GenNameTileTerra(),
                    IsGen = false
                };
                Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "   newObject=" + newObject + "             ~~~~~ DO: pos=" + newObject.Position + "  GO:  pos=" + p_gobject.transform.position);
                Debug.Log("CREATE NEW DATA OBJECT: " + p_gobject.name + "  TYPE: " + newObject.GetType());
                //newObject2.UpdateGameObject(p_gobject);
                newObject.UpdateGameObject(p_gobject);
                break;

            default:
                //Debug.Log("_______________________CreateObjectData_____________default " + prefabType);
                //var newObject3 = new ObjectData()
                newObject = new ModelNPC.ObjectData()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position
                };
                //newObject3.UpdateGameObject(p_gobject);
                newObject.UpdateGameObject(p_gobject);
                break;
        }
        return newObject;
    }

    public static ModelNPC.ObjectData FindObjectData(GameObject p_gobject)
    {
        ModelNPC.ObjectData newObject;
        //#PPPP
        TypePrefabs prefabType = TypePrefabs.PrefabField;

        if (!String.IsNullOrEmpty(p_gobject.tag))
            prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), p_gobject.tag.ToString()); ;

        string nameField = "";
        int index = -1;
        List<ModelNPC.ObjectData> objects;

        //Debug.Log("FindObjectData -------- prefabType: " + prefabType);

        switch (prefabType)
        {
            case TypePrefabs.PrefabUfo:
                newObject = new ModelNPC.GameDataUfo();
                nameField = Helper.GetNameFieldByName(p_gobject.name);
                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                {
                    Debug.Log("################# Error FindObjectData FIELD NOT FOUND :" + nameField);
                    return null;
                }
                objects = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
                index = objects.FindIndex(p => p.NameObject == p_gobject.name);
                if (index == -1)
                {
                    Debug.Log("################# Error FindObjectData DATA OBJECT NOT FOUND : " + p_gobject.name + "   in Field: " + nameField);
                    //Storage.Instance.DebugKill(p_gobject.name);
                    //Storage.Log.GetHistory(p_gobject.name);
                    //Storage.Fix.CorrectData(null, p_gobject, "FindObjectData");
                    return null;
                }
                newObject = objects[index] as ModelNPC.GameDataUfo;
                break;
            case TypePrefabs.PrefabBoss:
                nameField = Helper.GetNameFieldByName(p_gobject.name);
                //Debug.Log("FindObjectData ------------ nameField :" + nameField);

                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                {
                    Debug.Log("################# Error FindObjectData FIELD NOT FOUND :" + nameField);
                    return null;
                }
                objects = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
                //Debug.Log("FindObjectData ------------ objects in " + nameField + ":" + objects.Count());
                index = objects.FindIndex(p => p.NameObject == p_gobject.name);
                if (index == -1)
                {
                    Debug.Log("################# Error FindObjectData DATA OBJECT NOT FOUND : " + p_gobject.name + "   in Field: " + nameField);
                    //Storage.Log.GetHistory(p_gobject.name);
                    //Storage.Fix.CorrectData(null, p_gobject, "FindObjectData");
                    return null;
                }
                //Debug.Log("FindObjectData ------------ objects[" + index + "] " + objects[index] + " type: " + objects[index].GetType());

                newObject = objects[index] as ModelNPC.GameDataBoss;
                //var t2 = (GameDataBoss)objects[index];
                //if(t2==null)
                //    Debug.Log("t2=null ");
                //else Debug.Log("t2 type=" + t2.GetType());

                if (newObject == null)
                {
                    Debug.Log("FindObjectData ------------newObject is null ");
                    Debug.Log("FindObjectData ------------newObject is null , Type:" + objects[index].GetType());
                }

                break;
            default:
                newObject = new ModelNPC.ObjectData()
                {
                    NameObject = p_gobject.name,
                    TagObject = p_gobject.tag,
                    Position = p_gobject.transform.position
                };
                nameField = Helper.GetNameFieldByName(p_gobject.name);
                if (Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                {
                    objects = Storage.Instance.GridDataG.FieldsD[nameField].Objects;
                    index = objects.FindIndex(p => p.NameObject == p_gobject.name);
                    if (index == -1)
                    {
                        Debug.Log("################# Error FindObjectData DATA OBJECT NOT FOUND : " + p_gobject.name + "   in Field: " + nameField);
                        return null;
                    }
                    newObject = objects[index] as ModelNPC.GameDataBoss;
                    if (newObject == null)
                    {
                        Debug.Log("FindObjectData ------------newObject is null ");
                        Debug.Log("FindObjectData ------------newObject is null , Type:" + objects[index].GetType());
                    }
                }
                break;
            //default:
            //    newObject = new ModelNPC.ObjectData()
            //    {
            //        NameObject = p_gobject.name,
            //        TagObject = p_gobject.tag,
            //        Position = p_gobject.transform.position
            //    };
            //    break;
        }

        //Debug.Log("FindObjectData -------- newObject: " + newObject);
        //Debug.Log("FindObjectData -------- newObject: " + newObject.NameObject);

        return newObject;
    }

    //public GameObject FindPrefab(string namePrefab)
    public GameObject FindPrefab(string namePrefab, string nameObject)
    {
        if (PoolGameObjects.IsUsePoolObjects && !string.IsNullOrEmpty(nameObject))
        {
            //GameObject gobj = Storage.Pool.InstantiatePool(prefabField, new Vector3(0, 0, 0), "namePrefab");
            //GameObject gobj = Storage.Pool.GetPoolGameObject(nameObject, namePrefab, new Vector3(0, 0, 0));
            //string typePrefab = GetTypeByName(namePrefab);

            if(namePrefab=="PrefabVood")
            {
                var t = "OK";
            }

            //#FIX POOL
            string tagPool = GetTypePool(namePrefab);

            //string tagPool = GetTypeByName(namePrefab);

            //#TEST
            //Storage.Events.ListLogAdd = "+ pool:";
            //Storage.Events.ListLogAdd = "--- namePrefab: " + namePrefab;
            //Storage.Events.ListLogAdd = "--- typePrefab: " + typePrefab;
            //Storage.Events.ListLogAdd = "--- nameObject: " + nameObject;

            //GameObject gobj = Storage.Pool.GetPoolGameObject(nameObject, typePrefab, new Vector3(0, 0, 0));
            GameObject gobj = Storage.Pool.GetPoolGameObject(nameObject, tagPool, new Vector3(0, 0, 0));
            return gobj;
        }
        else
        {

            //#TEST #PREFABF.1
            //return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
            //#TEST #PREFABF.2
            return FindPrefabHieracly(namePrefab);
        }
    }

    private string GetTypeByName(string namePrefab)
    {
        string resType="";

        TypePrefabs prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), namePrefab);

        switch (prefabType)
        {
            case TypePrefabs.PrefabRock:
                resType = PrefabRock.tag;
                break;
            case TypePrefabs.PrefabVood:
                resType = PrefabVood.tag;
                break;
            case TypePrefabs.PrefabUfo:
                resType = PrefabUfo.tag;
                break;
            case TypePrefabs.PrefabBoss:
                resType = PrefabBoss.tag;
                break;
            case TypePrefabs.PrefabWallRock:
                resType = PrefabWallRock.tag;
                break;
            case TypePrefabs.PrefabWallWood:
                resType = PrefabWallWood.tag;
                break;
            case TypePrefabs.PrefabElka:
                resType = PrefabElka.tag;
                //TypePrefabs.PrefabElka.ToString();
                break;
            case TypePrefabs.PrefabField:
                resType = PrefabField.tag;
                break;
            default:
                Debug.Log("!!! FindPrefabHieracly no type : " + prefabType.ToString());
                break;
        }
        return resType;
    }

    public string GetTypePool(string namePrefab)
    {
        string resType = "";
        if (namePrefab == TypePrefabs.PrefabElka.ToString())
        {
            return namePrefab;
        }
        else if (namePrefab == "Field")
        {
            return TypePrefabs.PrefabField.ToString();
        }
        else
        {
            return GetTypeByName(namePrefab);
        }
        return resType;
    }

    private GameObject FindPrefabHieracly(string namePrefab)
    {
        try
        {
            if(namePrefab=="8")
            {
                Debug.Log("@@@@@@@@@@@@@@@@@@@@");
            }

            TypePrefabs prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), namePrefab);
            GameObject resPrefab = null;

            switch (prefabType)
            {
                case TypePrefabs.PrefabRock:
                    resPrefab = Instantiate(PrefabRock);
                    break;
                case TypePrefabs.PrefabVood:
                    resPrefab = Instantiate(PrefabVood);
                    break;
                case TypePrefabs.PrefabUfo:
                    resPrefab = Instantiate(PrefabUfo);
                    break;
                case TypePrefabs.PrefabBoss:
                    resPrefab = Instantiate(PrefabBoss);
                    break;
                case TypePrefabs.PrefabWallRock:
                    resPrefab = Instantiate(PrefabWallRock);
                    break;
                case TypePrefabs.PrefabWallWood:
                    resPrefab = Instantiate(PrefabWallWood);
                    break;
                case TypePrefabs.PrefabElka:
                    resPrefab = Instantiate(PrefabElka);
                    break;
                case TypePrefabs.PrefabField:
                    resPrefab = Instantiate(PrefabField);
                    break;

                default:
                    Debug.Log("!!! FindPrefabHieracly no type : " + prefabType.ToString());
                    break;

            }



            //Debug.Log("FindPrefabHieracly: " + prefabType.ToString());
            return resPrefab;
        }
        catch (Exception x)
        {
            Debug.Log("Error FindPrefabHieracly: " + x.Message);
        }
        Debug.Log("Error FindPrefabHieracly: object is null");

        return null;
    }

    public Sprite GetSpriteBossTrue(int index)
    {
        string indErr = "";
        try
        {
            indErr = "start";
            //if (NemesSpritesBoss.Length <= index)
            //{
            //    Debug.Log("############ NemeSpriteBoss int on range index = " + index + "   sprites count= " + NemesSpritesBoss.Length);
            //    return null;
            //}

            indErr = "2";
            //Case 1.
            //string spriteName = NemesSpritesBoss[index];
            string spriteName = 

            indErr = "3";
            if (!Storage.Person.SpriteCollection.ContainsKey(spriteName))
            {
                Debug.Log("############ NOT in SpriteCollection name: " + spriteName);
                return null;
            }
            indErr = "4";
            Sprite spriteBoss = Storage.Person.SpriteCollection[spriteName];
            if (spriteBoss == null)
            {
                Debug.Log("############ spritesBoss is null");
                return null;
            }
            else
            {
                return spriteBoss;
            }

        }catch(Exception x)
        {
            Debug.Log("################# GetSpriteBoss #" + indErr + " [" + index + "] : " + x.Message);
            return GetSpriteBossTrue(index);
        }

        return null;
    }

    //public Sprite GetSpriteBoss(int index)
    public Sprite GetSpriteBoss(int index, out string spriteName)
    {
        
        try
        {
            //string spriteName = TypeBoss.Instance.GetNameSpriteForIndexLevel(index);
            spriteName = TypeBoss.Instance.GetNameSpriteForIndexLevel(index);
            Sprite spriteBoss = Storage.Person.SpriteCollection[spriteName];
            
            return spriteBoss;
        }
        catch (Exception x)
        {
            Debug.Log("################# GetSpriteBoss [" + index + "] : " + x.Message);
        }
        spriteName = "error";

        return null;
    }

    public Texture2D GetTextuteMapBoss(int index)
    {

        try
        {
            //string _textureName = NemesTextureBoss[index];
            //Texture2D _texture = Storage.Person.SpriteCollection[_textureName];
            //return _texture;
            //-----
        }
        catch (Exception x)
        {
            Debug.Log("################# GetSpriteBoss [" + index + "] : " + x.Message);
        }

        return null;
    }

    //

    public static ModelNPC.ObjectData BildObjectData(TypePrefabs prefabType, bool isTerraGen = false)
    {
        ModelNPC.ObjectData objGameBild;

        switch (prefabType)
        {
            case SaveLoadData.TypePrefabs.PrefabUfo:
                objGameBild = new ModelNPC.GameDataUfo();
                break;
            case SaveLoadData.TypePrefabs.PrefabBoss:
                objGameBild = new ModelNPC.GameDataBoss(); //$$
                break;
            case SaveLoadData.TypePrefabs.PrefabField:
                //objGameBild = new ModelNPC.TerraData(isTerraGen); //$$
                objGameBild = new ModelNPC.TerraData(); //$$
                break;

            default:
                objGameBild = new ModelNPC.ObjectData();
                break;
        }
        return objGameBild;
    }

    public void SaveLevel()
    {
        SaveLevelCash();
        return;

        _scriptGrid.SaveAllRealGameObjects();
        if (Storage.Instance.GridDataG == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }

        //Serializator.SaveGridXml(Storage.Instance.GridDataG, Storage.Instance.DataPathLevel, true);
        //
    }

    public void SaveLevelParts()
    {
        _scriptGrid.SaveAllRealGameObjects();
        if (Storage.Instance.GridDataG == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }

        Serializator.SaveGridPartsXml(Storage.Instance.GridDataG, Storage.Instance.DataPathLevel, true);
    }

    public void SaveLevelCash()
    {
        _scriptGrid.SaveAllRealGameObjects();
        if (Storage.Instance.GridDataG == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }

        Serializator.SaveGridCashXml(Storage.Instance.GridDataG, Storage.Instance.DataPathLevel, true);
    }


    public void AddConstructInGridData(string nameField, DataTile itemTile, bool isClaerField = false)
    {
        TypePrefabs prefabName = TypePrefabs.PrefabField;

        //public enum TypesStructure
        //{
        //    Terra,
        //    Floor,
        //    Wall,
        //    Person
        //}

        if (itemTile == null)
        {
            Debug.Log("####### AddConstructInGridData  itemTile == null");
            return;
        }

        TypesStructure structType = (TypesStructure)Enum.Parse(typeof(TypesStructure), itemTile.Tag); ;
        if (structType == TypesStructure.Terra)
        {
            prefabName = TypePrefabs.PrefabField; 
        }
        if (structType == TypesStructure.Person || structType == TypesStructure.Prefab)
        {
            prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), itemTile.Name);
            //prefabName = GetPrefabByTile(itemTile.Name);
            //GameObject prefab = Storage.GridData.FindPrefab(itemTile.Name);
        }

        Vector2 posStruct = Helper.GetPositByField(nameField);
        int x = (int)posStruct.x;
        int y = (int)posStruct.y;
        int _y = y * (-1);
        Vector3 pos = new Vector3(x, _y, 0) * Spacing;
        pos.z = -1;
        if (prefabName == TypePrefabs.PrefabUfo)
            pos.z = -2;

        string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
        ModelNPC.ObjectData objDataSave = BildObjectData(prefabName, false);
        objDataSave.NameObject = nameObject;
        objDataSave.TagObject = prefabName.ToString();
        objDataSave.Position = pos;
        if(structType == TypesStructure.Terra)
        {
            var objTerra = objDataSave as ModelNPC.TerraData;
            if(objTerra==null)
            {
                Debug.Log("####### AddConstructInGridData: structType is TypesStructure.Terra   objDataSave Not is ModelNPC.TerraData !!!!");
                return;
            }
            objTerra.TileName = itemTile.Name;
        }

        Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld", isClaerField);
    }

    private TypePrefabs GetPrefabByTile(string TileName)
    {

        return TypePrefabs.PrefabField;
    }

    public void LoadDataBigXML()
    {
        //--load parts 
        //StartCoroutine(StartLoadDataPartsXML());
        //--load asinc
        //StartCoroutine(StartLoadDataBigXML());

        //-- Load in thread
        //StartCoroutine(StartBackgroundLoadDataBigXML());

        //-- load old style
        StartCoroutine(StartInGameLoadDataBigXML());
    }

    IEnumerator StartInGameLoadDataBigXML()
    {
        yield return new WaitForSeconds(2f);

        Storage.Events.SetMessage("Подожди говнюк...");

        yield return null;

        LoadingWordlTimer = Time.time;
        var fieldsD_Temp = Serializator.LoadGridXml(Storage.Data.DataPathBigPart);
        //yield return null;
        Storage.Data.SetGridDatatBig = fieldsD_Temp.FieldsD;

        //yield return null;
        Storage.Data.CompletedLoadWorld();//fieldsD_Temp

        float loadingTime = Time.time - LoadingWordlTimer;
        Storage.Events.SetMessage("Ты ждал: " + loadingTime);

        yield return new WaitForSeconds(0.5f);

        Storage.Events.HideMessage();

        yield break;
    }

    IEnumerator StartBackgroundLoadDataBigXML()
    {
        yield return null;

        LoadingWordlTimer = Time.time;

        Storage.Data.LoadBigWorldThread();

        yield return new WaitForSeconds(1f);

        while (!Storage.Data.IsThreadLoadWorldCompleted)
        {

            yield return new WaitForSeconds(2f);
        }

        Storage.Data.CompletedLoadWorld();

        float loadingTime = Time.time - LoadingWordlTimer;
        Storage.Events.SetTittle = "Loaded:" + loadingTime;
        Storage.Events.ListLogAdd = "Loaded:" + loadingTime;
        Debug.Log("*********************** Time loding World: " + loadingTime);

        yield break;
    }

    IEnumerator StartLoadDataBigXML()
    {
        string stepErr = "start";
        Debug.Log("Loaded Xml GridData start...");

        Dictionary<string, ModelNPC.FieldData> fieldsD_Test = new Dictionary<string, ModelNPC.FieldData>();

        yield return null;

        LoadingWordlTimer = Time.time;

        string nameField = "";

        stepErr = "c.1";
        string datapathPart = Application.dataPath + "/Levels/LevelDataPart1x2.xml";
        if (File.Exists(datapathPart))
        {
            
            int indProgress = 0;
            int limitUpdate = 20;

            //using (XmlReader xml = XmlReader.Create(stReader))
            using (XmlReader xml = XmlReader.Create(datapathPart))
            //using (XmlReader xml = XmlReader.Create(new StreamReader(datapathPart, System.Text.Encoding.UTF8)))
            {
                while (xml.Read())
                {
                    switch (xml.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xml.Name == "Key")
                            {
                                XElement el = XElement.ReadFrom(xml) as XElement;
                                nameField = el.Value;
                                //nameField = xml.Value.Clone().ToString();
                                break;
                            }
                            //if (xml.Name == "Objects")
                            if (xml.Name == "ObjectData") //WWW
                            {
                                indProgress++;
                                if (indProgress > limitUpdate)
                                {
                                    indProgress = 0;
                                    yield return null;
                                }

                                XElement el = XElement.ReadFrom(xml) as XElement;
                                string inputString = el.ToString();

                                //---------------
                                //XmlSerializer serializer = new XmlSerializer(typeof(List<ModelNPC.ObjectData>), Serializator.extraTypes);
                                //WWW
                                XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.ObjectData), Serializator.extraTypes);
                                //--------------
                                StringReader stringReader = new StringReader(inputString);
                                //stringReader.Read(); // skip BOM
                                //--------------

                                //List<KeyValuePair<string, ModelNPC.FieldData>> dataResult = (List<KeyValuePair<string, ModelNPC.FieldData>>)serializer.Deserialize(rdr);
                                //Debug.Log("! " + inputString);
                                //List<ModelNPC.ObjectData> dataResult;
                                ModelNPC.ObjectData dataResult;
                                try
                                {
                                    dataResult = (ModelNPC.ObjectData)serializer.Deserialize(stringReader);
                                }
                                catch (Exception x)
                                {
                                    Debug.Log("############# " + x.Message);
                                    yield break;
                                }
                                //-------------------------
                                if (Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                                {
                                    //_GridDataG.FieldsD[nameField].Objects.Add(dataResult);
                                    fieldsD_Test[nameField].Objects.Add(dataResult);
                                }
                                else
                                {
                                    //_GridDataG.FieldsD.Add(nameField, new ModelNPC.FieldData()
                                    //{
                                    //    NameField = nameField,
                                    //    Objects = new List<ModelNPC.ObjectData>() { dataResult }
                                    //});
                                    fieldsD_Test.Add(nameField, new ModelNPC.FieldData()
                                    {
                                        NameField = nameField,
                                        Objects = new List<ModelNPC.ObjectData>() { dataResult }
                                    });
                                }
                            }
                            break;
                    }
                }
            }

            //xml.Close();
            //stReader.Close();
        }

        //------------
        Storage.Data.SetGridDatatBig = fieldsD_Test;

        yield return null;
        Storage.Data.CompletedLoadWorld();//fieldsD_Temp
        //--------------

        float loadingTime = Time.time - LoadingWordlTimer;
        Storage.Events.SetMessage("Ты ждал: " + loadingTime);

        yield return new WaitForSeconds(4f);

        Storage.Events.HideMessage();
    }



    byte[] StringToUTF8ByteArray(string pXmlString)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    IEnumerator StartLoadDataPartsXML()
    {
        string datapath;
        string stepErr = "start";
        Debug.Log("Loaded Xml GridData start...");

        yield return null;

        for (int partX = 0; partX < Helper.SizePart; partX++)
        {
            for (int partY = 0; partY < Helper.SizePart; partY++)
            {
                //--- skip first part
                if (partX == 0 && partY == 0)
                    continue;

                //yield return null;

                stepErr = "c.1";
                string nameFileXML = +(partX + 1) + "x" + (partY + 1);
                string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
                datapath = datapathPart;
                if (File.Exists(datapathPart))
                {
                    //yield return new WaitForSeconds(0.3f);
                    yield return null;
                    yield return StartCoroutine(StartLoadPartXML(datapathPart));
                }

            }
        }
    }

    IEnumerator StartLoadPartXML(string datapathPart)
    {
        string stepErr = "start";

        //yield return null;
        ModelNPC.GridData itemGridData = null;
        stepErr = ".1";
        stepErr = ".2";
        XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), Serializator.extraTypes);
        stepErr = ".3";
        using (FileStream fs = new FileStream(datapathPart, FileMode.Open))
        {
            stepErr = ".4";
            itemGridData = (ModelNPC.GridData)serializer.Deserialize(fs);
            stepErr = ".5";
            fs.Close();
        }
        yield return null;
        stepErr = ".6";
        itemGridData.FieldsD = itemGridData.FieldsXML.ToDictionary(x => x.Key, x => x.Value);
        stepErr = ".7";
        Debug.Log("Loaded Xml GridData D:" + itemGridData.FieldsD.Count() + "     datapath=" + datapathPart);
        //## 
        itemGridData.FieldsXML = null;
        stepErr = ".8";
        //yield return null;
        Storage.Instance.GridDataG.FieldsD = Storage.Instance.GridDataG.FieldsD.Concat(itemGridData.FieldsD)
            .ToDictionary(x => x.Key, x => x.Value);

    }


    public void ClearWorld(bool isReload = true)
    {
        Storage.Instance.StopGame();
        Storage.Pool.Restart();
        Storage.Instance.DestroyAllGamesObjects();
        Storage.Pool.Restart();
        Storage.Instance.GridDataG.FieldsD.Clear();
        if(isReload)
            Storage.Player.LoadPositionHero();
        //Storage.Instance.GridDataG.FieldsD.Clear();
    }

    public void GenWorld(bool isReload = true)
    {
        //Storage.Instance.StopGame();
        //Storage.Pool.Restart();
        //Storage.Instance.DestroyAllGamesObjects();
        //DataConstructionTiles dataTiles = Storage.TilesManager.DataMapTiles[SelectedConstruction];
        //------------------------------


        Storage.PaletteMap.GenericOnWorld(SaveLoadData.TypePrefabs.PrefabElka); // BrushCells(bool isOnFullMap = false)



        //------------------------------
        //Storage.Pool.Restart();
        //Storage.Instance.GridDataG.FieldsD.Clear();
        //if (isReload)
        //    Storage.Player.LoadPositionHero();
        //Storage.Instance.GridDataG.FieldsD.Clear();
    }






}





