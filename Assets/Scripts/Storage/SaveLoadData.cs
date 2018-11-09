using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;

public class SaveLoadData : MonoBehaviour {

    //@NEWPREFAB@
    public GameObject PrefabVood;
    public GameObject PrefabRock;
    public GameObject PrefabUfo;
    public GameObject PrefabBoss;
    public GameObject PrefabElka;
    public GameObject PrefabWallRock;
    public GameObject PrefabWallWood;

    ////--- TAILS ---
    //public GameObject BackPalette;
    //public Grid GridTails;
    //public GameObject TailsMap;

    //public GameObject 
    public static float Spacing = 2f;

    private GenerateGridFields _scriptGrid;
   

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
                    if (prefabName == TypePrefabs.PrefabField)
                        continue;
                    //-----------

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * Spacing;
                    pos.z = -1;
                    if (prefabName == TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    

                    //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ModelNPC.ObjectData objDataSave = BildObjectData(prefabName);
                    objDataSave.NameObject = nameObject;
                    objDataSave.TagObject = prefabName.ToString();
                    objDataSave.Position = pos;

                    if(objDataSave.TagObject=="8")
                    {
                        Debug.Log("@@@@@@@@");
                    }

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
        }

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);
    }


    //GEN -----
    private TypePrefabs GenObjectWorld()
    {
        //Type prefab
        //++ Elka, WallRock, WallWood
        int intTypePrefab = UnityEngine.Random.Range(1, 8);


        //#TT YES BOSS
        //int intTypePrefab = UnityEngine.Random.Range(1, 5);
        //#TT YES UFO
        //int intTypePrefab = UnityEngine.Random.Range(1, 4);
        //#TT NOT UFO
        //int intTypePrefab = UnityEngine.Random.Range(1, 3);

        TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;
        int rnd1 = UnityEngine.Random.Range(1, 3);
        if(rnd1!=1)
        {
            prefabName = TypePrefabs.PrefabField;
        }
        //TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;


        //prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
        return prefabName;
    }


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
                newObject = new ModelNPC.GameDataBoss();
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

                if (newObject==null)
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
                break;
        }

        //Debug.Log("FindObjectData -------- newObject: " + newObject);
        //Debug.Log("FindObjectData -------- newObject: " + newObject.NameObject);

        return newObject;
    }

    public GameObject FindPrefab(string namePrefab)
    {
        //#TEST #PREFABF.1
        //return (GameObject)Resources.Load("Prefabs/" + namePrefab, typeof(GameObject));
        //#TEST #PREFABF.2
        return FindPrefabHieracly(namePrefab);
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

    public Sprite GetSpriteBoss(int index)
    {
        
        try
        {
            string spriteName = TypeBoss.Instance.GetNameSpriteForIndexLevel(index);
            Sprite spriteBoss = Storage.Person.SpriteCollection[spriteName];
            
            return spriteBoss;
        }
        catch (Exception x)
        {
            Debug.Log("################# GetSpriteBoss [" + index + "] : " + x.Message);
        }

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

    public static ModelNPC.ObjectData BildObjectData(TypePrefabs prefabType)
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
        _scriptGrid.SaveAllRealGameObjects();
        if (Storage.Instance.GridDataG == null)
        {
            Debug.Log("Error SaveLevel gridData is null !!!");
            return;
        }

        Serializator.SaveGridXml(Storage.Instance.GridDataG, Storage.Instance.DataPathLevel, true);
    }

    public void AddConstructInGridData(string nameField, DataTile itemTile)
    {
        TypePrefabs prefabName = TypePrefabs.PrefabField;

        //public enum TypesStructure
        //{
        //    Terra,
        //    Floor,
        //    Wall,
        //    Person
        //}

        TypesStructure structType = (TypesStructure)Enum.Parse(typeof(TypesStructure), itemTile.Tag); ;
        if (structType == TypesStructure.Terra)
        {
            prefabName = TypePrefabs.PrefabField; 
        }
        if (structType == TypesStructure.Person || structType == TypesStructure.Wall)
        {
            prefabName = GetPrefabByTile(itemTile.Name);
        }

        int y = 0;
        int x = 0;
        int _y = y * (-1);
        Vector3 pos = new Vector3(x, _y, 0) * Spacing;
        pos.z = -1;
        if (prefabName == TypePrefabs.PrefabUfo)
            pos.z = -2;

        string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
        ModelNPC.ObjectData objDataSave = BildObjectData(prefabName);
        objDataSave.NameObject = nameObject;
        objDataSave.TagObject = prefabName.ToString();
        objDataSave.Position = pos;

        Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
    }

    private TypePrefabs GetPrefabByTile(string TileName)
    {

        return TypePrefabs.PrefabField;
    }


    //public void CreateStructDataTile(string NameStructMap, BoundsInt boundsStruct, TileBase[] allTiles, BoundsInt boundsMap)
    //{
    //    int countFindTiles = 0;
    //    List<DataTile> listDataTiles = new List<DataTile>();

    //    int startX = boundsStruct.x + Math.Abs(boundsMap.x);
    //    int startY = boundsStruct.y + Math.Abs(boundsMap.y);
    //    int boundsSizeX = startX + boundsStruct.size.x;
    //    int boundsSizeY = startY + boundsStruct.size.y;



    //    for (int x = startX; x < boundsSizeX; x++)
    //    {
    //        for (int y = startY; y < boundsSizeY; y++)
    //        {
    //            TileBase tile = allTiles[x + y * boundsMap.size.x];


    //            if (tile != null)
    //            {
    //                int cellX = x + -startX;
    //                int cellY = y + -startY;

    //                DataTile dataTiles = new DataTile()
    //                {
    //                    X = cellX,
    //                    Y = cellY,
    //                    NameTales = tile.name
    //                };

    //                listDataTiles.Add(dataTiles);
    //                countFindTiles++;
    //            }
    //        }
    //    }

    //    CollectionDataMapTales.Add(NameStructMap, listDataTiles);

    //}




}





