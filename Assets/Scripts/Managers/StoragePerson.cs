using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class StoragePerson : MonoBehaviour {

    public Vector3 PersonsTargetPosition { get; set; }

    [SerializeField]
    public ContainerPriorityFinder ContainerPrioritys;

    public Color ColorSelectedCursorObject = Color.cyan;
    public Color ColorFindCursorObject = Color.magenta;

    public Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> PersonPriority;
    public Dictionary<string, int> CollectionPowerAllTypes;
    public Dictionary<SaveLoadData.TypePrefabs, List<AlienJob>> CollectionAlienJob;
    public Dictionary<string, AlienJob> AlienToResourceJobsJoins;

    public static string _Ufo { get { return SaveLoadData.TypePrefabs.PrefabUfo.ToString(); } }
    public static string _Boss { get { return SaveLoadData.TypePrefabs.PrefabBoss.ToString(); } }

    public SpriteAtlas SpriteAtlasNPC;

    public TypeModifPerson ModificatorPerson = TypeModifPerson.PointPos;

    public enum TypeModifPerson
    {
        Alpha, 
        Kill,
        PointPos,
        Paint,
    }

    //#Atlas SpriteCollection
    private Dictionary<string, Sprite> m_SpriteCollection;
    public Dictionary<string, Sprite> SpriteCollection
    {
        get
        {
            if (m_SpriteCollection == null || m_SpriteCollection.Count ==0)
            {
                m_SpriteCollection = new Dictionary<string, Sprite>();

                Sprite[] spritesPrefabsAtlas = GetSpritesAtlasNPC();
                foreach (var sprt in spritesPrefabsAtlas)
                {
                    string namePrefab = sprt.name;//.GetNamePrefabByTextureMap();
                    namePrefab = namePrefab.Replace("(Clone)", "");
                    //Texture2D _texture = sprt.texture;
                    //Texture2D _texture = SpriteUtility.GetSpriteTexture(sprt, false /* getAtlasData */);
                    //_texture.Apply();
                    m_SpriteCollection.Add(namePrefab, sprt);
                }
            }
            return m_SpriteCollection;
        }
    }

    //private ModelNPC.LevelData _personsData;
    //public ModelNPC.LevelData PersonsData
    //{
    //    get { return _personsData; }
    //}

    //public void PersonsDataInit(ModelNPC.LevelData _newData = null)
    //{
    //    if (_newData == null)
    //        _personsData = new ModelNPC.LevelData();
    //    else
    //        _personsData = _newData;

        
    //}

    void Awake()
    {
        //PersonsDataInit();
    }

    // Use this for initialization
    void Start() {
        LoadSprites();
        //LoadPriorityPerson();
    }

    public void Init()
    {
        //LoadSprites();
        LoadPriorityPerson();
    }

    // Update is called once per frame
    void Update() {

    }


    public Sprite[] GetSpritesAtlasNPC()
    {
        Sprite[] spritesAtlas = new Sprite[SpriteAtlasNPC.spriteCount];
        SpriteAtlasNPC.GetSprites(spritesAtlas);
        return spritesAtlas;
    }

    private void LoadSprites()
    {
        if(SpriteAtlasNPC==null)
        {
            Debug.Log("########### SpriteAtlasNPCis empty");
            return;
        }
        var load = SpriteCollection;
    }

    public IEnumerable<GameObject> GetAllRealPersons()
    {
        //??????????????? MissingReferenceException: The object of type 'GameObject' has been destroyed but you are still trying to access it.
        //        Your script should either check if it is null or you should not destroy the object.
        //        StoragePerson.< GetAllRealPersons > m__1(UnityEngine.GameObject p)(at Assets / Scripts / Storage / StoragePerson.cs:49)
        //System.Linq.Enumerable +< CreateWhereIterator > c__Iterator1D`1[UnityEngine.GameObject].MoveNext()
        //System.Collections.Generic.List`1[UnityEngine.GameObject].AddEnumerable(IEnumerable`1 enumerable)(at / Users / builduser / buildslave / mono / build / mcs /class/corlib/System.Collections.Generic/List.cs:128)
        //System.Collections.Generic.List`1[UnityEngine.GameObject]..ctor(IEnumerable`1 collection) (at /Users/builduser/buildslave/mono/build/mcs/class/corlib/System.Collections.Generic/List.cs:65)
        //System.Linq.Enumerable.ToList[GameObject] (IEnumerable`1 source)
        //StoragePerson.GetAllRealPersons() (at Assets/Scripts/Storage/StoragePerson.cs:49)
        //GenerateGridFields+<CalculateTealsObjects>c__Iterator0.MoveNext() (at Assets/Scripts/Storage/GenerateGridFields.cs:64)
        //UnityEngine.SetupCoroutine.InvokeMoveNext(IEnumerator enumerator, IntPtr returnValueAddress) (at C:/buildslave/unity/build/Runtime/Export/Coroutines.cs:17)


        if (PoolGameObjects.IsUseTypePoolPrefabs)
        {
            return Storage.Instance.GamesObjectsReal.
                SelectMany(x => x.Value).
                Where(p => p!=null && (p.tag == PoolGameObjects.TypePoolPrefabs.PoolPerson.ToString())).ToList();
        }

        return Storage.Instance.GamesObjectsReal.
                SelectMany(x => x.Value).
                Where(p => p!=null && (p.tag == _Ufo || p.tag == _Boss)).ToList();
    }

    public IEnumerable<GameObject> GetAllRealPersons(string field, bool  isModif = false)
    {
        try
        {
            //var count1= Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).ToList().Count();
            //Debug.Log("PERSON PAIR (" + field + ")  COUNT " + count1);

            foreach (GameObject gobjItem in Storage.Instance.GamesObjectsReal.
                Where(p => p.Key == field).
                SelectMany(x => x.Value).ToList())
            {
                if (isModif)
                {
                    ModifObject(gobjItem);
                }
                Debug.Log("OBJECT(" + field + ") : " + gobjItem);
            }

            if (PoolGameObjects.IsUseTypePoolPrefabs)
            {
                Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).
                    SelectMany(x => x.Value).
                    Where(p => p != null && (p.tag == PoolGameObjects.TypePoolPrefabs.PoolPerson.ToString())).ToList();
            }

            return Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).
                    SelectMany(x => x.Value).
                    Where(p => p.tag == _Ufo || p.tag == _Boss).ToList();
        }catch(Exception x)
        {
            Debug.Log("###### GetAllRealPersons: " + x.Message);
        }
        return null;
    }

    public IEnumerable<GameObject> GetAllRealPersonsForName(string name)
    {
        if(string.IsNullOrEmpty(name))
            return new List<GameObject>();

        string id = Helper.GetID(name);

        if (string.IsNullOrEmpty(id))
            return new List<GameObject>();

        return Storage.Instance.GamesObjectsReal.
                SelectMany(x => x.Value).
                Where(p => p!=null && p.name.IndexOf(id)!=-1).ToList();
    }

    public IEnumerable<GameObject> GetAllRealPersonsForID(string id)
    {
        if (string.IsNullOrEmpty(id))
            return new List<GameObject>();

        return Storage.Instance.GamesObjectsReal.
                SelectMany(x => x.Value).
                Where(p => p != null && p.name.IndexOf(id) != -1).ToList();
    }

    public IEnumerable<ModelNPC.ObjectData> GetAllDataPersonsForID(string name)
    {
        string id = Helper.GetID(name);
        return GetAllDataPersonsForName(id);
    }

    public IEnumerable<ModelNPC.ObjectData> GetAllDataPersonsForName(string name)
    {
        return Storage.Instance.GridDataG.FieldsD.
            Select(x => x.Value).
            SelectMany(x => x.Objects).
            Where(p => p.NameObject.IndexOf(name) != -1).ToList();
    }


   

    public FindPersonData GetFindPersonsDataForName(string nameFind)
    {
        FindPersonData persData = null;
        
        foreach (var item in Storage.Instance.GridDataG.FieldsD)
        {
            string field = item.Key;
            ModelNPC.ObjectData dataObj = item.Value.Objects.Find(p => p.NameObject.IndexOf(nameFind) != -1);
            if(dataObj!=null)
            {

                int index = item.Value.Objects.FindIndex(p => p.NameObject.IndexOf(nameFind) != -1);
                persData = new FindPersonData()
                {
                    DataObj = dataObj,
                    Field = field,
                    Index = index
                };
                break;
            }
            //Storage.Instance.GridDataG.FieldsD.
            //   Select(x => x.Value).
            //   SelectMany(x => x.Objects).
            //   Where(p => p.NameObject.IndexOf(name) != -1).ToList();


        }
        return persData;
    }

    public List<SaveLoadData.TypePrefabs> TypesPersons
    {
        get
        {
            return new List<SaveLoadData.TypePrefabs>()
                {
                     SaveLoadData.TypePrefabs.PrefabUfo,
                     SaveLoadData.TypePrefabs.PrefabBoss
                };
        }
    }

    public List<string> NamesPersons
    {
        get
        {
            return new List<string>()
                {
                     SaveLoadData.TypePrefabs.PrefabUfo.ToString(),
                     SaveLoadData.TypePrefabs.PrefabBoss.ToString()
                };
        }
    }

    public void SelectedID(string gobjID)
    {

    }

    public void SelectGameObjectDataByField(string p_field)
    {
        try
        {
            if (!Storage.Instance.Fields.ContainsKey(p_field))
                return;

            //Storage.Instance.SelectFieldCursor = p_field;
            Storage.EventsUI.ListLogAdd = "SelectFieldCursor: " + Storage.Instance.SelectFieldCursor;
            //Storage.Events.ListLogClear();
            GameObject prefabFind = Storage.Instance.Fields[p_field];

            if (prefabFind != null)
            {
                prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorSelectedCursorObject;
            }


            if (Storage.Map.IsOpen)
                return;

            foreach (var gobj in Storage.Person.GetAllRealPersons(p_field, true))
            {
                //Storage.Events.ListLogAdd = "FIND (" + _fieldCursor + "): " + gobj.name;

                gobj.GetComponent<SpriteRenderer>().color = ColorFindCursorObject;

                //MovementNPC movement = gobj.GetComponent<MovementNPC>();
                GameObjecDataController dataObj = gobj.GetComponent<GameObjecDataController>();
                ModelNPC.ObjectData findData = dataObj.GetData();
                var objData = SaveLoadData.GetObjectDataByGobj(gobj);
                if (objData == null)
                    continue;
                if (findData != objData)
                {
                    Storage.EventsUI.ListLogAdd = "#### " + gobj.name + " conflict DATA";
                    Debug.Log("#### " + gobj.name + " conflict DATA");
                }

                var dataNPC = findData as ModelNPC.GameDataNPC;
                if (dataNPC != null)
                {
                    Storage.EventsUI.ListLogAdd = "VeiwCursorGameObjectData: " + gobj.name + " NPC Params: " + dataNPC.GetParamsString;
                    Debug.Log("VeiwCursorGameObjectData: " + gobj.name + " NPC Params: " + dataNPC.GetParamsString);

                    //#EXPAND
                    Storage.EventsUI.AddExpandPerson(dataNPC.NameObject,
                        dataNPC.GetParams,
                        new List<string> { "Pause", "Kill", "StartTrack" },
                        gobjObservable: gobj);
                }
                else
                {
                    Debug.Log("VeiwCursorGameObjectData: " + gobj.name + "  Not is NPC");
                    //ModifObject(gobj);
                }


                var dataBoss = findData as ModelNPC.GameDataBoss;
                if (dataBoss != null)
                {
                    Storage.EventsUI.ListLogAdd = "YES GameDataBoss " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ";
                    Debug.Log("YES GameDataBoss " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ");
                    dataBoss.ColorRender = Color.magenta;

                    //#EXPAND
                    Storage.EventsUI.AddExpandPerson(dataBoss.NameObject,
                        dataBoss.GetParams,
                        new List<string> { "Pause", "Kill", "StartTrack" },
                        gobjObservable: gobj);
                }
                else
                {
                    var dataAlien = findData as ModelNPC.GameDataAlien;
                    if (dataAlien != null)
                    {
                        Storage.EventsUI.ListLogAdd = "YES GameDataAlien " + gobj.name + "";
                        Debug.Log("YES GameDataAlien " + gobj.name);
                        dataBoss.ColorRender = Color.magenta;

                        //#EXPAND
                        Storage.EventsUI.AddExpandPerson(dataBoss.NameObject,
                            dataBoss.GetParams,
                            new List<string> { "Pause", "Kill", "StartTrack" },
                            gobjObservable: gobj);
                    }
                    else
                    {
                        if (gobj.tag == _Boss)
                        {
                            Storage.EventsUI.ListLogAdd = "#### " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ";
                            Debug.Log("#### " + gobj.name + " SaveLoadData.GameDataBoss is EMPTY ");
                        }
                    }
                }

            }
        }catch(Exception x)
        {
            Debug.Log("##############  VeiwCursorGameObjectData: " + x.Message);
        }
    }

    public void SetTartgetPositionAll(Vector2 posCursorToField)
    {
        Debug.Log("SetTartgetPositionAll : to " + posCursorToField.x + "" + posCursorToField.y);

        //foreach (var gobj in Storage.Person.GetAllRealPersons())
        //{
        //    Debug.Log("SetTartgetPositionAll : " + gobj.name + " to : " + posCursorToField.x + "" + posCursorToField.y);
        //    MovementNPC movem = gobj.GetComponent<MovementNPC>();
        //    ModelNPC.GameDataNPC dataNPC = movem.GetData();
        //    dataNPC.SetTargetPosition(posCursorToField);
        //}
        PersonsTargetPosition = posCursorToField;// new Vector3(posCursorToField.x,posCursorToField.y,0);
        TartgetPositionAll(); //<< dataNPC.SetTargetPosition(Storage.Instance.PersonsTargetPosition);
    }

    //#TARGET
    public void TartgetPositionAll()
    {
        Debug.Log("^^^^^^^^ TARGET --- TartgetPositionAll");//#TARGET

        //PersonsTargetPosition
        foreach (GameObject gobj in Storage.Person.GetAllRealPersons().ToList())
        {
            //if (Storage.Person.NamesPersons.Contains(gobj.tag.ToString()))
            //if (typeP.IsPerson())
            if (gobj.tag.ToString().IsPerson())
            {
                var movementUfo = gobj.GetMoveUfo();
                if (movementUfo != null)
                    movementUfo.SetTarget();

                var movementNPC = gobj.GetMoveNPC();
                if (movementNPC != null)
                    movementNPC.SetTarget();
            }
        }
    }

    public void ModifObject(GameObject gobj)
    {
        Storage.EventsUI.ListLogAdd = "Modif : " + ModificatorPerson + " > " + gobj.name;
        switch(ModificatorPerson)
        {
            case TypeModifPerson.Alpha:
                gobj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
                break;
            case TypeModifPerson.Kill:
                Storage.Instance.AddDestroyGameObject(gobj);
                break;

            case TypeModifPerson.PointPos:
                Storage.EventsUI.PointGO.transform.position = gobj.transform.position;
                break;
            default:
                Storage.EventsUI.ListLogAdd = "Empty modificator > " + ModificatorPerson.ToString();
                break;
        }
    }

    public ModelNPC.GameDataAlien GenTypeAlien()
    {
        int maxT = Storage.GridData.NamesPrefabNPC.Count;
        int indT = UnityEngine.Random.Range(0, maxT);
        //Level = rng.Next(1, 7);

        ModelNPC.GameDataAlien obj = null;

        //TypeAlien = Storage.Person.GenTypeAlien()
        string TypeAlien = Storage.GridData.NamesPrefabNPC[indT];
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Inspector.ToString())
            obj = new ModelNPC.GameDataAlienInspector();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Machinetool.ToString())
            obj = new ModelNPC.GameDataAlienMachinetool();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Mecha.ToString())
            obj = new ModelNPC.GameDataAlienMecha();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Dendroid.ToString())
            obj = new ModelNPC.GameDataAlienDendroid();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Gary.ToString())
            obj = new ModelNPC.GameDataAlienGarry();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Lollipop.ToString())
            obj = new ModelNPC.GameDataAlienLollipop();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Blastarr.ToString())
            obj = new ModelNPC.GameDataAlienBlastarr();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Hydragon.ToString())
            obj = new ModelNPC.GameDataAlienHydragon();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Pavuk.ToString())
            obj = new ModelNPC.GameDataAlienPavuk();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Skvid.ToString())
            obj = new ModelNPC.GameDataAlienSkvid();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Fantom.ToString())
            obj = new ModelNPC.GameDataAlienFantom();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Mask.ToString())
            obj = new ModelNPC.GameDataAlienMask();
        if (TypeAlien == SaveLoadData.TypePrefabNPC.Vhailor.ToString())
            obj = new ModelNPC.GameDataAlienVhailor();
        //if (TypeAlien == SaveLoadData.TypePrefabNPC..ToString())
        //    obj = new ModelNPC.GameDataAlien();
        //obj.TypePrefabName = TypeAlien;
        return obj;
    }

    public void UpdateGamePositionInDream(string fieldOld, string fieldNew,  ModelNPC.ObjectData dataNPC, Vector3 newPosition)
    {
        if (!ReaderScene.IsGridDataFieldExist(fieldOld))
            Storage.Data.AddNewFieldInGrid(fieldOld, "UpdateGamePositionInDream");
        if (!ReaderScene.IsGridDataFieldExist(fieldNew))
            Storage.Data.AddNewFieldInGrid(fieldNew, "UpdateGamePositionInDream");

        var objectsData = ReaderScene.GetObjectsDataFromGrid(fieldOld);
        string oldName = dataNPC.NameObject;
        int index = objectsData.FindIndex(p => p.NameObject == dataNPC.NameObject);
        if (index == -1)
        {
            Debug.Log("########### NOT FOUND IN OLD FIELD " + fieldOld + " -- " + dataNPC.NameObject);
        }

        string nameObject = Helper.CreateName(dataNPC.TypePrefabName, fieldNew, "", dataNPC.NameObject);
        dataNPC.SetNameObject(nameObject, isTestValid: false); //isTestValid - 3*

        Storage.Data.AddDataObjectInGrid(dataNPC, fieldNew, "ActionMove from: " + fieldOld);  // <<<< beforeUpdateField - 3 *
        dataNPC.SetPosition(newPosition); //FIX~~TRANSFER

        if (index != -1)
        {
            if (Storage.Instance.ReaderSceneIsValid)
            {
                Storage.ReaderWorld.RemoveObjectInfo(objectsData[index].Id);
            }
            objectsData.RemoveAt(index);
        }
        Storage.ReaderWorld.UpdateLinkData(dataNPC, true, fieldNew, -1);
    }

    public string UpdateGamePosition_Cache(string p_OldField, string p_NewField, string p_NameObject, ModelNPC.ObjectData objData, Vector3 p_newPosition, GameObject thisGameObject, bool isDestroy = false, bool NotValid = false)
    {
        if (Storage.Instance.IsLoadingWorld && !NotValid)
            return "";

        if (Storage.Data.IsUpdatingLocationPersonGlobal)
            return "";

        List<GameObject> realObjectsOldField = Storage.Instance.GamesObjectsReal[p_OldField];
        List<ModelNPC.ObjectData> dataObjectsOldField = ReaderScene.GetObjectsDataFromGrid(p_OldField);
        
        int indReal = realObjectsOldField.FindIndex(p => p.name == p_NameObject);
        int testIndData = dataObjectsOldField.FindIndex(p => p.NameObject == p_NameObject);
        GameObject gobj = realObjectsOldField[indReal];

        //add to new Field
        if (!ReaderScene.IsGridDataFieldExist(p_NewField))
            Storage.Data.AddNewFieldInGrid(p_NewField, "UpdateGamePosition");

        Storage.Data.UpdatingLocationPersonLocal++;
        string nameObject = Helper.CreateName(objData.TypePrefabName, p_NewField, "", p_NameObject);
        objData.SetNameObject(nameObject, isTestValid: false); 
        gobj.name = objData.NameObject;

        if (isDestroy)
            objData.IsReality = false;

        if (!Storage.Instance.GamesObjectsReal.ContainsKey(p_NewField))
        {
            Storage.Instance.GamesObjectsReal.Add(p_NewField, new List<GameObject>());
        }

        bool resAddData = Storage.Data.AddDataObjectInGrid(objData, p_NewField, "UpdateGamePosition from: " + p_OldField);
        if (!resAddData)
        {
            Storage.Data.UpdatingLocationPersonLocal--;
            return "";
        }
        objData.SetPosition(gobj.transform.position);
        if (!isDestroy)
        {
            bool resAddReal = Storage.Data.AddRealObject(gobj, p_NewField, "UpdateGamePosition from: " + p_OldField);
            if (!resAddReal)
            {
                Storage.Data.UpdatingLocationPersonLocal--;
                return "";
            }
        }
        //remove
        Storage.Data.RemoveObjecDataGridByIndex(ref dataObjectsOldField, testIndData);
        realObjectsOldField.RemoveAt(indReal);
        Storage.Data.UpdatingLocationPersonLocal--;

        return gobj.name;
    }

    public string UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, ModelNPC.ObjectData objData, Vector3 p_newPosition, GameObject thisGameObject, bool isDestroy = false, bool NotValid = false)
    {
        if (!ConfigDebug.IsTestDUBLICATE)
            return UpdateGamePosition_Cache(p_OldField, p_NewField, p_NameObject, objData, p_newPosition, thisGameObject, isDestroy, NotValid);

        if (Storage.Instance.IsLoadingWorld && !NotValid)
        {
            Debug.Log("_______________ LOADING WORLD ....._______________");
            return "";
        }

        if (Storage.Data.IsUpdatingLocationPersonGlobal)
        {
            Debug.Log("_______________UpdateGamePosition  RETURN IsUpdatingLocationPerson_______________");
            return "";
        }

        if (Storage.Instance.IsCorrectData && !NotValid)
        {
            Debug.Log("_______________ RETURN LoadGameObjectDataForLook ON CORRECT_______________");
            return "Error";
        }

        if (Storage.Instance.GamesObjectsReal == null || Storage.Instance.GamesObjectsReal.Count == 0)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      GamesObjectsReal is EMPTY");
            return "";
        }
        if (Storage.Instance.GridDataG == null || Storage.Instance.GridDataG.FieldsD == null || Storage.Instance.GridDataG.FieldsD.Count == 0)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      GridData is EMPTY");
            return "";
        }

        if (!Storage.Instance.GamesObjectsReal.ContainsKey(p_OldField))
        {
            Debug.Log("********** (" + p_NameObject + ") ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition      GamesObjectsReal not found OldField = " + p_OldField);
            if (p_NameObject != null)
                Storage.Instance.SelectGameObjectID = Helper.GetID(p_NameObject);

            //Storage.Log.GetHistory(p_NameObject);

            //@@CORRECT
            //Destroy(thisGameObject, 1f);
            //return "Error";
            return "";
        }
        if (!ReaderScene.IsGridDataFieldExist(p_OldField))
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition      GridData not found OldField = " + p_OldField);
            return "";
        }

        List<GameObject> realObjectsOldField = Storage.Instance.GamesObjectsReal[p_OldField];
        List<ModelNPC.ObjectData> dataObjectsOldField = ReaderScene.GetObjectsDataFromGrid(p_OldField);

        if (realObjectsOldField == null)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition     realObjectsOldField is Null !!!!");
            if (!Storage.Instance.GamesObjectsReal.ContainsKey(p_OldField))
            {
                Debug.Log("********** UpdatePosition     in GamesObjectsReal not found OldField = " + p_OldField);
                return "";
            }
            else
            {
                Storage.Instance.GamesObjectsReal[p_OldField] = new List<GameObject>();
            }
            return "";
        }

        //#TEST -----
        for (int i = realObjectsOldField.Count - 1; i >= 0; i--)
        {
            if (realObjectsOldField[i] == null)
            {
                Debug.Log("^^^^ UpfatePosition  -- remove destroy realObjects");
                realObjectsOldField.RemoveAt(i);
            }
        }
        //--------------

        int indReal = realObjectsOldField.FindIndex(p => p.name == p_NameObject);
        if (indReal == -1)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("######## UpfatePosition Not Real object (" + p_NameObject + ") in field: " + p_OldField);
            if (p_NameObject != null)
                Storage.Instance.SelectGameObjectID = Helper.GetID(p_NameObject);

            //Storage.Fix.CorrectData(p_NameObject, "UpfatePosition Not Real");


            //return "Error";
            //var DataObj = Storage.Person.GetFindPersonsDataForName(p_NameObject);
            //if (DataObj != null)
            //{
            //Debug.Log("::::::::::::::::::::::::: Find Pesron DATA: " + p_NameObject + " :::::");
            //Debug.Log("))))))))) :  [" + DataObj.Field + "][" + DataObj.Index + "] " + DataObj.DataObj);

            Debug.Log("+++++++ Add New Real Object " + thisGameObject + "   in field: " + p_NewField);
            Storage.Data.AddRealObject(thisGameObject, p_NewField, "UpdateGamePosition");
            realObjectsOldField = Storage.Instance.GamesObjectsReal[p_NewField];
            indReal = realObjectsOldField.FindIndex(p => p.name == p_NameObject);
            if (indReal == -1)
            {
                Storage.Log.GetHistory(p_NameObject);
                return "";
            }
        }
        int testIndData = dataObjectsOldField.FindIndex(p => p.NameObject == p_NameObject);
        if (testIndData == -1)
        {
            //--------------------
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            var posI = thisGameObject.transform.position;
            string info = " >>>>>> thisGameOobject : " + thisGameObject.name + "       pos = " + Helper.GetNameFieldPosit(posI.x, posI.y);

            Debug.Log("^^^^ UpfatePosition Not DATA object (" + p_NameObject + ") in field: " + p_OldField + "     " + info);
            foreach (var itemObj in dataObjectsOldField)
            {
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- object : " + itemObj.NameObject);
            }
            if (dataObjectsOldField.Count == 0)
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- objects ZERO !!!!!");
            //--------------------
            //Storage.Fix.CorrectData(p_NameObject, "UpfatePosition IN DATA");
            
            return "Error";
        }
        GameObject gobj = realObjectsOldField[indReal];
        if (gobj == null)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      gobj is Destroy");
            return "";
        }

        if (!gobj.Equals(thisGameObject))
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("################ ERROR Not Equals thisGameOobject (" + thisGameObject + ")  and RealObject (" + gobj + ")");
            Storage.Instance.SelectGameObjectID = Helper.GetID(gobj.name);
            //@CD@
            //_StorageCorrect.CorrectData(gobj, thisGameObject, "UpdateGamePosition");
            //return "Error";
            return "";
        }

        //add to new Field
        if (!ReaderScene.IsGridDataFieldExist(p_NewField))
        {
            //#!!!!  Debug.Log("SaveListObjectsToData GridData ADD new FIELD : " + posFieldReal);
            //Storage.Instance.GridDataG.FieldsD.Add(p_NewField, new ModelNPC.FieldData());
            Storage.Data.AddNewFieldInGrid(p_NewField, "UpdateGamePosition");
        }

        if (p_newPosition != gobj.transform.position)
        {
            Debug.Log("********** (" + gobj.name + ")^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("############### ERROR UpdatePosition 1.  ERROR POSITOIN : GAME OBJ NEW POS: " + p_newPosition + "       REAL OBJ POS: " + gobj.transform.position + "  REAL FIELD: " + Helper.GetNameFieldPosit(gobj.transform.position.x, gobj.transform.position.y));
            return "";
        }

        if (Storage.Data.IsUpdatingLocationPersonGlobal)
        {
            Debug.Log("_______________UpdateGamePosition  RETURN IsUpdatingLocationPerson_______________");
            return "";
        }

        //VALID ==============================================================
        string nameObjectTest = Helper.CreateName(objData.TypePrefabName, p_NewField, "", p_NameObject);
        //if (IsGridDataFieldExist(p_NewField))
        if (ReaderScene.IsGridDataFieldExist(p_NewField))
        {
            var indT1 = ReaderScene.GetObjectsDataFromGrid(p_NewField).FindIndex(p => p.NameObject == nameObjectTest);
            if (indT1 != -1)
            {
                Storage.Instance.SelectGameObjectID = Helper.GetID(nameObjectTest);
                //Debug.Log("########## UpdatePosition [" + objData.NameObject + "] DUBLICATE DATA: " + nameObjectTest + "      in " + p_NewField);
                //Storage.Log.GetHistory(objDataSave.NameObject);

                //<< fix: >>
                //Storage.Data.RemoveAllFindDataObject(nameObjectTest);
                //Storage.Data.RemoveAllFindRealObject(nameObjectTest);
                Storage.Data.RemoveDataObjectInGrid(p_NewField, indT1, "UpdatePosition");
                //return "";
            }
        }
        if (Storage.Instance.GamesObjectsReal.ContainsKey(p_NewField))
        {
            var indT2 = Storage.Instance.GamesObjectsReal[p_NewField].FindIndex(p => p != null && p.name == nameObjectTest); ;
            if (indT2 != -1) //@@DUBLICATE
            {
                GameObject findGobjDbl = Storage.Instance.GamesObjectsReal[p_NewField][indT2];
                if (!findGobjDbl.Equals(thisGameObject))
                {
                    Debug.Log("############################## find dublicate Real obj --- is NOT ME : " + findGobjDbl.name + "      ME: " + thisGameObject.name);
                    Storage.Pool.DestroyPoolGameObject(findGobjDbl);
                }
                //@@@@
                //Storage.EventsUI.ClearListExpandPersons();
                //Storage.EventsUI.AddMenuPerson(objData as ModelNPC.GameDataNPC, thisGameObject);
                //Storage.EventsUI.AddMenuPerson(dataDbl as ModelNPC.GameDataNPC, findGobjDbl);
                //Storage.GamePause = true;
                //return "Error";
                //@@@@
                //remove dublicate in real list gobj
                Storage.Data.RemoveRealObject(indT2, p_NewField, "UpdatePosition");
                Debug.Log("############# UpdateGamePosition  RETURN DUBLICATE");
                return "";
            }
        }
        Storage.Data.UpdatingLocationPersonLocal++;

        if (p_newPosition != gobj.transform.position)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** ERROR UpdatePosition 2.   ERROR POSITOIN :  GAME OBJ NEW POS: " + p_newPosition + "       REAL OBJ POS: " + gobj.transform.position);
            return "";
        }

        string nameObject = Helper.CreateName(objData.TypePrefabName, p_NewField, "", p_NameObject);
        objData.SetNameObject(nameObject, isTestValid: false); //isTestValid -- 3 *
        gobj.name = objData.NameObject;

        if (isDestroy)
            objData.IsReality = false;

        if (!Storage.Instance.GamesObjectsReal.ContainsKey(p_NewField))
        {
            Storage.Instance.GamesObjectsReal.Add(p_NewField, new List<GameObject>());
        }

        bool resAddData = Storage.Data.AddDataObjectInGrid(objData, p_NewField, "UpdateGamePosition from: " + p_OldField); //<<< beforeUpdateField -- 3*
        if (!resAddData)
        {
            Storage.Data.UpdatingLocationPersonLocal--;
            return "";
        }
        objData.SetPosition(gobj.transform.position);//-- 3*

        //add
        if (!isDestroy)
        {
            bool resAddReal = Storage.Data.AddRealObject(gobj, p_NewField, "UpdateGamePosition from: " + p_OldField);
            if (!resAddReal)
            {
                Storage.Data.UpdatingLocationPersonLocal--;
                return "";
            }
        }

        //remove
        Storage.Data.RemoveObjecDataGridByIndex(ref dataObjectsOldField, testIndData);
        realObjectsOldField.RemoveAt(indReal);
        Storage.Data.UpdatingLocationPersonLocal--;

        return gobj.name;
    }

    //================================================

    //private PriorityFinder m_prioritysGet;
    //private AlienJob temp_job;
    private int temp_distantionFind;

    public void GetAlienNextTargetObject(ref ModelNPC.ObjectData result, ref AlienJob job, ModelNPC.GameDataAlien dataAlien)
    {
        int versionSearching = 2;// 1; //@JOB@

        if (PersonPriority == null)
        {
            Storage.EventsUI.ListLogAdd = "### GetAlienNextTargetObject >> PersonPriority is null";
            LoadPriorityPerson();
            result = null;
            return;
        }
        if (dataAlien == null)
        {
            Storage.EventsUI.ListLogAdd = "### GetAlienNextTargetObject >> dataAlien is null";
            result = null;
            return;
        }

        /*
        if (!PersonPriority.ContainsKey(dataAlien.TypePrefab))
        {
            if (PersonPriority.Count == 0)
            {
                Debug.Log(Storage.EventsUI.ListLogAdd = "##### GetAlienNextTargetObject PersonPriority count = 0");
                LoadPriorityPerson();
            }
            else
                Debug.Log(Storage.EventsUI.ListLogAdd = "##### GetAlienNextTargetObject PersonPriority Not found = " + dataAlien.TypePrefab);
            result = null;
            return;
        }
        */

        //m_prioritysGet = PersonPriority[dataAlien.TypePrefab];
        //int distantionFind = UnityEngine.Random.Range(2, 15);

        //v.1
        if (versionSearching == 1)
        {
            //result = FindFromLocation(dataAlien.Position, distantionFind, prioritys, dataAlien.Id, dataAlien.TypePrefab);
            //result = Helper.FindFromLocation(dataAlien, distantionFind);
            temp_distantionFind = UnityEngine.Random.Range(2, 15);
            Helper.FindFromLocation_Cache(ref result, dataAlien, temp_distantionFind);
        }
        else //v.3
        {
            if (job != null)
            {
                //fix job
                bool isCompletedMission = job != null &&
                    dataAlien.Inventory != null &&
                    dataAlien.Inventory.NameInventopyObject == job.TargetResource.ToString();

                switch (job.JobTo)
                {
                    case TypesJobTo.ToPortal:
                        if (isCompletedMission)
                        {
                            if (dataAlien.PortalId == null)
                                Storage.PortalsManager.SetHome(dataAlien);

                            var info = ReaderScene.GetInfoID(dataAlien.PortalId);
                            if (info != null)
                                result = info.Data;
                        }
                        break;
                    default:
                        job = null;
                        break;
                }
            }
            //if (string.IsNullOrEmpty(dataAlien.TargetID))
            if (result == null)
            {
                temp_distantionFind = UnityEngine.Random.Range(2, 150);
                FindJobResouceLocation(ref result, ref job, dataAlien, temp_distantionFind);
                if (result == null)
                {
                    temp_distantionFind = UnityEngine.Random.Range(2, 25);//15
                    Helper.FindFromLocation_Cache(ref result, dataAlien, temp_distantionFind);
                }
            }

        }
        //v.2
        //if (versionSearching == 2)
        //{
        //    string fieldName = Helper.GetNameFieldPosit(dataAlien.Position.x, dataAlien.Position.y);
        //    Vector2 posField = Helper.GetPositByField(fieldName);
        //    Vector2Int posFieldInt = new Vector2Int((int)posField.x, (int)posField.y);
        //    ReaderScene.DataInfoFinder finder = ReaderScene.GetDataInfoLocationFromID((int)posFieldInt.x, (int)posFieldInt.y, distantionFind, dataAlien.TypePrefab, dataAlien.Id);
    }

    List<Vector2Int> temp_findedFileds = new List<Vector2Int>();
    List<ModelNPC.ObjectData> temp_resourcesData;

    private void FindJobResouceLocation(ref ModelNPC.ObjectData result, ref AlienJob job, ModelNPC.ObjectData dataAien, int distantionWay)
    {
        if (!CollectionAlienJob.ContainsKey(dataAien.TypePrefab))
            return;

        int x = 0;
        int y = 0;
        string nameField = string.Empty;
        string key;

        Helper.GetFieldPositByWorldPosit(ref x, ref y, dataAien.Position);
        temp_findedFileds.Clear();
        temp_resourcesData = null;
        Helper.GetSpiralFields_Cache(ref temp_findedFileds, x, y, distantionWay);
        result = null;
        foreach (Vector2Int fieldNext in temp_findedFileds)
        {
            Helper.GetNameField_Cache(ref nameField, fieldNext.x, fieldNext.y);
            //resourcesData = ReaderScene.GetObjectsDataFromGrid(nameField);
            temp_resourcesData = ReaderScene.GetObjectsDataFromGridContinue(nameField);
            if (temp_resourcesData == null)
                continue;
            foreach (ModelNPC.ObjectData resoursData in temp_resourcesData)
            {
                key = string.Format("{0}_{1}", dataAien.TypePrefabName, resoursData.TypePrefabName);
                AlienToResourceJobsJoins.TryGetValue(key, out job);
                if (job != null)
                {
                    result = resoursData;
                    return;
                }
            }
        }

        job = null;
    }

    private void LoadPriorityPerson()
    {
        string strErr = "";
        try
        {
            strErr = "1";
            //if (ContainerPrioritys==null)
            //{
            //    Storage.EventsUI.ListLogAdd = "Container ContainerPrioritys is EMPTY !!!!";
            //    Storage.EventsUI.SetMessageBox = "Container ContainerPrioritys is EMPTY !!!!";
            //    return;
            //}
            strErr = "2";
            PersonPriority = Helper.GetPrioritys(ContainerPrioritys,"NPC");
            if (PersonPriority == null)
            {
                Storage.EventsUI.ListLogAdd = "Container PersonPriority is EMPTY !!!!";
                Storage.EventsUI.SetMessageBox = "Container PersonPriority is EMPTY !!!!";
                return;
            }
            strErr = "3";
            CollectionPowerAllTypes = Helper.FillPrioritys(PersonPriority);
            if (CollectionPowerAllTypes == null)
            {
                Storage.EventsUI.ListLogAdd = "Container CollectionPowerAllTypes is EMPTY !!!!";
                Storage.EventsUI.SetMessageBox = "Container CollectionPowerAllTypes is EMPTY !!!!";
                return;
            }
            strErr = "4";
            AlienToResourceJobsJoins = new Dictionary<string, AlienJob>();
            //@JOB@
            strErr = "5";
            CollectionAlienJob = Helper.CollectionAlienJob(PersonPriority ,ref AlienToResourceJobsJoins);
        }
        catch (Exception ex)
        {
            Storage.EventsUI.ListLogAdd = "##### LoadPriorityPerson #" + strErr + " : " + ex.Message;
        }
    }

    public int GetPriorityPowerByJoin(SaveLoadData.TypePrefabs prefabNameType, SaveLoadData.TypePrefabs prefabNameTypeTarget)
    {
        string keyJoinNPC = prefabNameType + "_" + prefabNameTypeTarget;
        if (!CollectionPowerAllTypes.ContainsKey(keyJoinNPC))
        {
            //Debug.Log("########## GetPriorityPowerByJoin Not Key = " + keyJoinNPC);
            return 0;
        }
        return CollectionPowerAllTypes[keyJoinNPC];
    }


}

public static class PersonsExtensions
{
    public static bool IsPerson(this string typePrefab)
    {
        if (PoolGameObjects.IsUseTypePoolPrefabs)
        {
            return typePrefab == PoolGameObjects.TypePoolPrefabs.PoolPerson.ToString() ||
                typePrefab == PoolGameObjects.TypePoolPrefabs.PoolPersonBoss.ToString() ||
                typePrefab == PoolGameObjects.TypePoolPrefabs.PoolPersonUFO.ToString();
        }
        bool isNPC = Helper.IsTypePrefabNPC(typePrefab);
        return isNPC;
    }

    public static MovementUfo GetMoveUfo(this GameObject gobj)
    {
        var moveUfo = gobj.GetComponent<MovementUfo>();
        if (moveUfo != null)
            return moveUfo;
        return null;
    }

    public static MovementNPC GetMoveNPC(this GameObject gobj)
    {
        if (gobj == null)
            return null;
        var moveNPC = gobj.GetComponent<MovementNPC>();
        if (moveNPC != null)
            return moveNPC;
        return null;
    }

    public static EventsObject GetEvent(this GameObject gobj)
    {
        var evObj = gobj.GetComponent<EventsObject>();
        if (evObj != null)
            return evObj;
        return null;
    }

    public static GameObjecDataController GetDataController(this GameObject gobj)
    {
        if (gobj == null)
            return null;
        var dataObjControl = gobj.GetComponent<GameObjecDataController>();
        if (dataObjControl != null)
            return dataObjControl;
        return null;
    }

    public static ModelNPC.GameDataNPC GetDataNPC(this GameObject gobj)
    {
        var dataControl = gobj.GetDataController();
        if (dataControl != null)
            return dataControl.GetData() as ModelNPC.GameDataNPC;
        return null;
    }

    public static ModelNPC.ObjectData GetData(this GameObject gobj)
    {
        var dataControl = gobj.GetDataController();
        if (dataControl != null)
            return dataControl.GetData();
        return null;
    }

    public static bool IsNPC(this GameObject gobj)
    {
        bool isNPC = false;
        try
        {
            var typeT = gobj.tag.ToString();
            if (PoolGameObjects.IsUseTypePoolPrefabs)
            {
                return typeT == PoolGameObjects.TypePoolPrefabs.PoolPerson.ToString() ||
                    typeT == PoolGameObjects.TypePoolPrefabs.PoolPersonBoss.ToString() ||
                    typeT == PoolGameObjects.TypePoolPrefabs.PoolPersonUFO.ToString();
            }

            isNPC = Helper.IsTypePrefabNPC(typeT);
        }
        catch(Exception x)
        {
            Debug.Log(x.Message);
        }

        return isNPC;
    }

    public static bool IsUFO(this GameObject gobj)
    {
        var data = gobj.GetDataNPC();
        return data != null && data.TypePrefab == SaveLoadData.TypePrefabs.PrefabUfo;
        //return gobj.tag.Equals(StoragePerson._Ufo); 
    }

    public static void DisableComponents(this GameObject gobj)
    {
        var evObj = gobj.GetComponent<EventsObject>();
        if (evObj != null)
            evObj.PoolCase.IsDesrtoy = true;

        var movObj = gobj.GetComponent<MovementNPC>();
        if (movObj != null)
            movObj.Pause();

        var dataObj = gobj.GetComponent<GameObjecDataController>();
        if (dataObj != null && dataObj.GetData() != null)
            dataObj.GetData().IsReality = false;

        var actionObj = gobj.GetComponent<GameActionPersonController>();
        if (actionObj != null)
            actionObj.enabled = false;

        var portalObj = gobj.GetComponent<PortalController>();
        if (portalObj)
            portalObj.enabled = false;
    }
}

public class FindPersonData
{
    public FindPersonData() { }
    public ModelNPC.ObjectData DataObj { get; set; }
    public string Field { get; set; }
    public int Index { get; set; }
}


