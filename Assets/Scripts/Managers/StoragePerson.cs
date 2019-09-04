using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class StoragePerson : MonoBehaviour {

    public Vector3 PersonsTargetPosition { get; set; }
    public ContainerPriorityFinder ContainerPriority;

    public Color ColorSelectedCursorObject = Color.cyan;
    public Color ColorFindCursorObject = Color.magenta;

    public Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> PersonPriority;
    public Dictionary<string, int> CollectionPowerAllTypes;

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

    private ModelNPC.LevelData _personsData;
    public ModelNPC.LevelData PersonsData
    {
        get { return _personsData; }
    }

    public void PersonsDataInit(ModelNPC.LevelData _newData = null)
    {
        if (_newData == null)
            _personsData = new ModelNPC.LevelData();
        else
            _personsData = _newData;

        
    }

    void Awake()
    {
        PersonsDataInit();
    }

    // Use this for initialization
    void Start() {
        LoadSprites();
        LoadPriorityPerson();
    }

    // Update is called once per frame
    void Update() {

    }

    private void LoadPriorityPerson()
    {
        PersonPriority = new Dictionary<SaveLoadData.TypePrefabs, PriorityFinder>();
        foreach (var prior in ContainerPriority.CollectionPriorityFinder)
        {
            PersonPriority.Add(prior.TypeObserver, prior);
        }
        FillAllPriority();
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

    public IEnumerable<GameObject> GetAllRealPersonsForID(string name)
    {
        string id = Helper.GetID(name);

        return Storage.Instance.GamesObjectsReal.
                SelectMany(x => x.Value).
                Where(p => p.name.IndexOf(id)!=-1).ToList();
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

    //public void TartgetPositionAll()
    //{
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

                //var movementNPC = gobj.GetMoveBoss();
                //if (movementNPC != null)
                //    movementNPC.SetTarget();
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
            //SaveLoadData
            Storage.Data.AddNewFieldInGrid(fieldOld, "UpdateGamePositionInDream");
        //Storage.Data.AddDataObjectInGrid
        if (!ReaderScene.IsGridDataFieldExist(fieldNew))
            //SaveLoadData
            Storage.Data.AddNewFieldInGrid(fieldNew, "UpdateGamePositionInDream");


        var objectsData = ReaderScene.GetObjectsDataFromGrid(fieldOld);
        string oldName = dataNPC.NameObject;
        //int index = objectsData.Find(p => p.NameObject == dataNPC.NameObject);
        int index = objectsData.FindIndex(p => p.NameObject == dataNPC.NameObject);
        if (index == -1)
        {
            Debug.Log("########### NOT FOUND IN OLD FIELD " + fieldOld + " -- " + dataNPC.NameObject);
        }

        string nameObject = Helper.CreateName(dataNPC.TypePrefabName, fieldNew, "", dataNPC.NameObject);
        dataNPC.SetNameObject(nameObject);
        dataNPC.SetPosition(newPosition);

        Storage.Data.AddDataObjectInGrid(dataNPC, fieldNew, "ActionMove from: " + fieldOld);

        if (index != -1)
            objectsData.RemoveAt(index);

        Storage.ReaderWorld.UpdateField(dataNPC, fieldNew);
    }

    public string UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, ModelNPC.ObjectData objData, Vector3 p_newPosition, GameObject thisGameObject, bool isDestroy = false, bool NotValid = false)
    {
        
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

            Storage.Log.GetHistory(p_NameObject);

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
                Debug.Log("UGP: (" + p_NameObject + ") ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
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
            }
        }
        //==============================================================

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        Storage.Data.UpdatingLocationPersonLocal++;

        //objData.NameObject = Helper.CreateName(objData.TypePrefabName, p_NewField, "", p_NameObject);
        string nameObject = Helper.CreateName(objData.TypePrefabName, p_NewField, "", p_NameObject);
        objData.SetNameObject(nameObject);
        gobj.name = objData.NameObject;

        if (p_newPosition != gobj.transform.position)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** ERROR UpdatePosition 2.   ERROR POSITOIN :  GAME OBJ NEW POS: " + p_newPosition + "       REAL OBJ POS: " + gobj.transform.position);
            return "";
        }

        //objData.Position = gobj.transform.position;
        objData.SetPosition(gobj.transform.position);//###ERR

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

        //add
        if (!isDestroy)
        {
            //_GamesObjectsReal[p_NewField].Add(gobj);
            bool resAddReal = Storage.Data.AddRealObject(gobj, p_NewField, "UpdateGamePosition from: " + p_OldField);
            if (!resAddReal)
            {
                Storage.Data.UpdatingLocationPersonLocal--;
                return "";
            }
        }

        //remove
        dataObjectsOldField.RemoveAt(testIndData);
        realObjectsOldField.RemoveAt(indReal);

        Storage.Data.UpdatingLocationPersonLocal--;

        return gobj.name;
    }

    //================================================
    

    public ModelNPC.ObjectData GetAlienNextTargetObject(ModelNPC.GameDataAlien dataAlien)
    {
        int versionSearching = 1;

        if (!PersonPriority.ContainsKey(dataAlien.TypePrefab))
        {
            Debug.Log("######### GetAlienNextTargetObject PersonPriority Not found = " + dataAlien.TypePrefab);
            return null;
        }

        //string typeAlien = dataAlien.TypePrefabName;
        //Storage.ReaderWorld
        PriorityFinder prioritys = PersonPriority[dataAlien.TypePrefab];
        int distantionFind = UnityEngine.Random.Range(2, 15);

        ModelNPC.ObjectData result = new ModelNPC.ObjectData();

        //v.1
        if (versionSearching == 1)
            //result = FindFromLocation(dataAlien.Position, distantionFind, prioritys, dataAlien.Id, dataAlien.TypePrefab);
            result = FindFromLocation(dataAlien, distantionFind);
        //v.2
        if (versionSearching == 2)
        {
            string fieldName = Helper.GetNameFieldPosit(dataAlien.Position.x, dataAlien.Position.y);
            Vector2 posField = Helper.GetPositByField(fieldName);
            Vector2Int posFieldInt = new Vector2Int((int)posField.x, (int)posField.y);
            ReaderScene.DataInfoFinder finder = ReaderScene.GetDataInfoLocationFromID(posFieldInt, distantionFind, dataAlien.TypePrefab, dataAlien.Id); ;
            result = finder.ResultData;
        }

        return result;
    }




    //public Vector3 GetAlienNextTarget(ModelNPC.GameDataAlien dataAlien)
    //{
    //    //string typeAlien = dataAlien.TypePrefabName;
    //    //Storage.ReaderWorld
    //    PriorityFinder prioritys = PersonPriority[dataAlien.TypePrefab];
    //    int distantionFind = UnityEngine.Random.Range(2, 15);

    //    ModelNPC.ObjectData result = FindFromLocation(dataAlien.Position, distantionFind, prioritys, dataAlien.Id);

    //    if (result == null)
    //        return Vector3.zero;

    //    return result.Position;
    //}
    public ModelNPC.ObjectData FindFromLocation(ModelNPC.GameDataAlien dataAlien, int distantion)
    {
        Vector2 Position = dataAlien.Position;
        string id_Observer = dataAlien.Id;
        string id_PrevousTarget = dataAlien.PrevousTargetID;
        SaveLoadData.TypePrefabs typeObserver = dataAlien.TypePrefab;

        //string fieldName = Helper.GetNameField(Position);
        string fieldName = Helper.GetNameFieldPosit(Position.x, Position.y);
        Vector2 posField = Helper.GetPositByField(fieldName);
        Vector2Int posFieldInt = new Vector2Int((int)posField.x, (int)posField.y);

        //ReaderScene.DataInfoFinder finder = ReaderScene.GetDataInfoLocation(posFieldInt, distantion, prioritys, id_Observer, typeObserver, id_PrevousTarget);
        ReaderScene.DataInfoFinder finder = ReaderScene.GetDataInfoLocation(posFieldInt, distantion, id_Observer, typeObserver, id_PrevousTarget);
        return finder.ResultData;
    }

    //public  ModelNPC.ObjectData FindFromLocation(Vector2 Position, int distantion, PriorityFinder prioritys, string id_Observer, SaveLoadData.TypePrefabs typeObserver)
    //{

    //    //string fieldName = Helper.GetNameField(Position);
    //    string fieldName = Helper.GetNameFieldPosit(Position.x, Position.y);
    //    Vector2 posField = Helper.GetPositByField(fieldName);
    //    Vector2Int posFieldInt = new Vector2Int((int)posField.x, (int)posField.y);

    //    ReaderScene.DataInfoFinder finder = ReaderScene.GetDataInfoLocation(posFieldInt, distantion, prioritys, id_Observer, typeObserver);
    //    return finder.ResultData;
    //}

    //public ModelNPC.ObjectData FindFromLocation(Vector2Int fieldPosit, int distantion, PriorityFinder prioritys, string id_Observer, SaveLoadData.TypePrefabs typeObserver)
    //{
    //    ReaderScene.DataInfoFinder finder = ReaderScene.GetDataInfoLocation(fieldPosit, distantion, prioritys, id_Observer, typeObserver);
    //    return finder.ResultData;
    //}
    
    [Header("Count Prioritys Join ID")]
    public int CountPrioritysJoinID = 0;

    public void FillAllPriority()
    {
        var max = Enum.GetValues(typeof(SaveLoadData.TypePrefabs)).Length - 1;
        //var ind = UnityEngine.Random.Range(12, max);
        //var prefabNameGet = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), ind.ToString());

        //prefabNameGet = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), ind.ToString());

        CollectionPowerAllTypes = new Dictionary<string, int>();

        SaveLoadData.TypePrefabs prefabNameType;
        SaveLoadData.TypePrefabs prefabNameTypeTarget;
        for (int ind = 0; ind < max; ind++)
        {
            prefabNameType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), ind.ToString());
            //if (Helper.IsTypePrefabNPC(prefabNameType.ToString()))
            if(PersonPriority.ContainsKey(prefabNameType))
            {
                ModelNPC.ObjectData objData = BilderGameDataObjects.BildObjectData(prefabNameType.ToString());
                PriorityFinder prioritys = PersonPriority[prefabNameType];
                for (int indT = 0; indT < max; indT++)
                {
                    prefabNameTypeTarget = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), indT.ToString());
                    ModelNPC.ObjectData objDataTarget = BilderGameDataObjects.BildObjectData(prefabNameTypeTarget.ToString());
                    int power = GetPriorityPower(objDataTarget, prioritys);
                    string keyJoinNPC = prefabNameType + "_" + prefabNameTypeTarget;
                    CollectionPowerAllTypes.Add(keyJoinNPC, power);
                }
            }
        }
        CountPrioritysJoinID = CollectionPowerAllTypes.Count(); 
        //Storage.EventsUI.ListLogAdd = "COUNT PRIORITYS JOIN ID : " + CollectionPowerAllTypes.Count();
    }

    public static int GetPriorityPower(string id, PriorityFinder priority)
    {
        if (Storage.ReaderWorld.CollectionInfoID.ContainsKey(id))
        {
            Debug.Log("######### Error GetPrioriryPower=" + id);
        }

        int power = 0;

        var objData = Storage.ReaderWorld.CollectionInfoID[id].Data;
        return Storage.Person.GetPriorityPower(objData, priority);

        //return GetPriorityPowerByJoin(objData, priority);
    }

    public int GetPriorityPowerByJoin(SaveLoadData.TypePrefabs prefabNameType, SaveLoadData.TypePrefabs prefabNameTypeTarget)
    {
        string keyJoinNPC = prefabNameType + "_" + prefabNameTypeTarget;
        if(!Storage.Person.CollectionPowerAllTypes.ContainsKey(keyJoinNPC))
        {
            Debug.Log("########## GetPriorityPowerByJoin Not Key = " + keyJoinNPC);
            return 0;
        }
        return Storage.Person.CollectionPowerAllTypes[keyJoinNPC];
    }

    public int GetPriorityPower(ModelNPC.ObjectData objData, PriorityFinder priority)
    {
        int power = 0;
        SaveLoadData.TypePrefabs typeModel = objData.TypePrefab;
        PoolGameObjects.TypePoolPrefabs typePool = objData.TypePoolPrefab;
        TypesBiomNPC biomNPC = GetBiomByTypeModel(typeModel);

        int slotPower = 3;
        int maxtPrioprity = 10;
        maxtPrioprity = priority.GetPrioritysTypeModel().Count() * slotPower;
        foreach (SaveLoadData.TypePrefabs itemModel in priority.GetPrioritysTypeModel())
        {
            if (itemModel == typeModel)
            {
                power += maxtPrioprity;
                break;
            }
            maxtPrioprity -= slotPower;
        }
        maxtPrioprity = priority.PrioritysTypeBiomNPC.Count() * slotPower;
        foreach (TypesBiomNPC itemBiom in priority.PrioritysTypeBiomNPC)
        {
            if (itemBiom == biomNPC)
            {
                power += maxtPrioprity;
                break;
            }
            maxtPrioprity -= slotPower;
        }
        maxtPrioprity = priority.PrioritysTypeBiomNPC.Count() * slotPower;
        foreach (PoolGameObjects.TypePoolPrefabs itemPool in priority.PrioritysTypePool)
        {
            if (itemPool == typePool)
            {
                power += maxtPrioprity;
                break;
            }
            maxtPrioprity -= slotPower;
        }

        return power;
    }

    public static TypesBiomNPC GetBiomByTypeModel(SaveLoadData.TypePrefabs typeModel)
    {
        TypesBiomNPC resType = TypesBiomNPC.None;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomBlue), typeModel.ToString()))
            return TypesBiomNPC.Blue;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomGreen), typeModel.ToString()))
            return TypesBiomNPC.Green;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomRed), typeModel.ToString()))
            return TypesBiomNPC.Red;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomViolet), typeModel.ToString()))
            return TypesBiomNPC.Violet;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomGray), typeModel.ToString()))
            return TypesBiomNPC.Gray;
        return resType;
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
    }
}

public class FindPersonData
{
    public FindPersonData() { }
    public ModelNPC.ObjectData DataObj { get; set; }
    public string Field { get; set; }
    public int Index { get; set; }
}


