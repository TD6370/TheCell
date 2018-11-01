using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Linq;

public class SaveLoadData : MonoBehaviour {

    //@NEWPREFAB@
    public GameObject PrefabVood;
    public GameObject PrefabRock;
    public GameObject PrefabUfo;
    public GameObject PrefabBoss;

    //public GameObject 
    public static float Spacing = 2f;

    private GenerateGridFields _scriptGrid;
   

    //#################################################################################################
    //>>> ObjectData -> GameDataNPC -> PersonData -> 
    //>>> ObjectData -> GameDataNPC -> PersonData -> PersonDataBoss -> GameDataBoss
    //>>> ObjectData -> GameDataUfo
    //>>> ObjectData -> GameDataNPC -> GameDataOther
    //#################################################################################################

    static Type[] extraTypes = {
            typeof(ModelNPC.FieldData),
            typeof(ModelNPC.ObjectData),

            typeof(ModelNPC.GameDataUfo),

            typeof(ModelNPC.GameDataNPC),
            typeof(ModelNPC.PersonData),

            typeof(ModelNPC.PersonDataBoss),
            typeof(ModelNPC.GameDataBoss) };  

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
        PrefabBoss
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

                    //Type prefab
                    
                    //#TT YES BOSS
                    int intTypePrefab = UnityEngine.Random.Range(1, 5);
                    //#TT YES UFO
                    //int intTypePrefab = UnityEngine.Random.Range(1, 4);
                    //#TT NOT UFO
                    //int intTypePrefab = UnityEngine.Random.Range(1, 3);

                    TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;

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

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
        }

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);
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

    

    //---------------------------------------------------------------------------------------------------------------------

    public class Serializator {

        
        static public void SaveGridXml(ModelNPC.GridData state, string datapath, bool isNewWorld = false)
        {

            if (isNewWorld)
            {
                if (File.Exists(datapath))
                {
                    try
                    {
                        File.Delete(datapath);
                    }catch(Exception x)
                    {
                        Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapath + " : " + x.Message);
                    }
                }
            }

            //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
            //## 
            state.FieldsXML = state.FieldsD.ToList();

            //## 
            Debug.Log("SaveXml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);

            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);

		    FileStream fs = new FileStream(datapath, FileMode.Create);

		    serializer.Serialize(fs, state); 
		    fs.Close();

            state.FieldsXML = null;
            //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
	    }
	
	    static public ModelNPC.GridData LoadGridXml(string datapath){
            string stepErr = "start";
            ModelNPC.GridData state = null;
            try
            {
                Debug.Log("Loaded Xml GridData start...");

                stepErr = ".1";
                //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
                stepErr = ".2";
                XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);
                stepErr = ".3";
                FileStream fs = new FileStream(datapath, FileMode.Open);
                stepErr = ".4";
                state = (ModelNPC.GridData)serializer.Deserialize(fs);
                stepErr = ".5";
                fs.Close();

                stepErr = ".6";
                state.FieldsD = state.FieldsXML.ToDictionary(x => x.Key, x => x.Value);
                stepErr = ".7";
                Debug.Log("Loaded Xml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
                //## 
                state.FieldsXML = null;
            }
            catch (Exception x)
            {
                state = null;
                Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
            }

		    return state;
	    }

        static public ModelNPC.LevelData LoadPersonXml(string datapath)
        {
            string stepErr = "start";
            ModelNPC.LevelData state = null;
            try
            {
                Debug.Log("Loaded Xml GridData start...");

                stepErr = ".1";
                stepErr = ".2";
                XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.LevelData), extraTypes);
                stepErr = ".3";
                FileStream fs = new FileStream(datapath, FileMode.Open);
                stepErr = ".4";
                state = (ModelNPC.LevelData)serializer.Deserialize(fs);
                stepErr = ".5";
                fs.Close();
                stepErr = ".6";
                state.Persons = state.PersonsXML.ToDictionary(x => x.Key, x => x.Value);
                stepErr = ".7";
                Debug.Log("Loaded Xml CasePersonData :" + state.Persons.Count() + "   XML:" + state.PersonsXML.Count() + "     datapath=" + datapath);
                state.PersonsXML = null;
            }
            catch (Exception x)
            {
                state = null;
                Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
            }

            return state;
        }

        //static public CommandStore LoadCommandsStore(string datapath)
        //{
        //    string stepErr = "start";
        //    CommandStore state = null;
        //    try
        //    {
        //        Debug.Log("Loaded Xml GridData start...");

        //        stepErr = ".1";
        //        stepErr = ".2";
        //        XmlSerializer serializer = new XmlSerializer(typeof(CommandStore), extraTypes);
        //        stepErr = ".3";
        //        FileStream fs = new FileStream(datapath, FileMode.Open);
        //        stepErr = ".4";
        //        state = (CommandStore)serializer.Deserialize(fs);
        //        stepErr = ".5";
        //        fs.Close();
        //        stepErr = ".6";
        //        stepErr = ".7";
        //    }
        //    catch (Exception x)
        //    {
        //        state = null;
        //        Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        //    }

        //    return state;
        //}

        static public T LoadXml<T>(string datapath) where T : class
        {
            string stepErr = "start";
            T state = null;
            try
            {
                //Debug.Log("Loaded Xml GridData start...");

                stepErr = ".1";
                stepErr = ".2";
                XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);
                stepErr = ".3";
                FileStream fs = new FileStream(datapath, FileMode.Open);
                stepErr = ".4";
                state = (T)serializer.Deserialize(fs);
                stepErr = ".5";
                fs.Close();
                stepErr = ".6";
                stepErr = ".7";
            }
            catch (Exception x)
            {
                state = null;
                Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
            }

            return state;
        }

        static public void SaveXml<T>(T state, string datapath, bool isResave = false) where T : class
        {

            if (isResave)
            {
                if (File.Exists(datapath))
                {
                    try
                    {
                        File.Delete(datapath);
                    }
                    catch (Exception x)
                    {
                        Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapath + " : " + x.Message);
                    }
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);

            FileStream fs = new FileStream(datapath, FileMode.Create);

            serializer.Serialize(fs, state);
            fs.Close();

        }
    }

    
    //------------------------------------------------------------------------------
    //------------------------------------------------------------------------------

    

   

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

    


}


