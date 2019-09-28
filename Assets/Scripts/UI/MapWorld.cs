using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MapWorld : MonoBehaviour {

    public static bool IsReloadGridMap = false;
    public bool IsAutoAction = false;

    public bool IsGridMap = true;
    private  bool m_isGridMapLoaded = false;

    public Texture2D textureField;
    public Texture2D textureRock;
    public Texture2D textureVood;
    public Texture2D textureElka;
    public Texture2D textureWallRock;
    public Texture2D textureWallWood;
    public Texture2D textureHero;
    public Texture2D textureTest;
    public Texture2D textureParket;

    //--- Grid Map
    public GameObject PrefabGridMap;
    public GameObject PrefabSpriteCellMap;
    public Stack<string> StackSectorsUpdating;
    //public SpriteAtlas SpriteAtlasMapPrefab;

    private GridLayoutGroup m_GridMap;
    private List<GameObject> m_listCellsGridMap = new List<GameObject>();

    public GameObject gobjField;
    public GameObject MarkerMapWorldCell;

    public GameObject prefabMapCell;
    public GameObject prefabFrameMap;

    public Vector3 DistMoveCameraMap = new Vector3();
    public float DistMoveCameraMapXY = 0;
    public Vector3 StartPositFrameMap = new Vector3();

    public float ZoomMap { get; set; }
    public Vector2 SelectPointField = new Vector2(0, 0);
    public string SelectFieldMap
    {
        get
        {
            return Helper.GetNameField(SelectPointField.x, SelectPointField.y);
        }
    }

    public Vector3 FramePosition
    {
        get {
            return prefabFrameMap.transform.position;
        }
        set {
            prefabFrameMap.transform.position = value;
        }
    }

    private FrameMap m_Frame;
    public FrameMap Frame
    {
        get
        {
            if (m_Frame == null)
            {
                m_Frame = prefabFrameMap.GetComponent<FrameMap>();
                if (m_Frame == null)
                    Debug.Log("########### MapWorld.Frame is empty");
            }
            return m_Frame;
        }
    }

    public float ZOrderMap
    {
        get
        {
            return -2f;
        }
    }

    //--- TAILS ---
    public GameObject BackPalette;
    public Grid GridTails;
    public GameObject TailsMap;
    public int SizeCellMap = 25;

    //private string[,] colorMap = new string[1000, 1000];
    private bool m_IsCreatedMap = false;
    public Dictionary<string, GameObject> MapObjects;

    //List<Color> colorsPersons;
    List<SaveLoadData.TypePrefabs> listPersonsTypes;
    List<Texture2D> listPersonsPrefabTexture;
    List<Texture2D> listPersonsMapTexture;
    //Texture2D textureMap;//#fix mem
    //Sprite spriteMap; //#fix mem

 

    private void Awake()
    {
        m_GridMap = PrefabGridMap.gameObject.GetComponent<GridLayoutGroup>();

        MapObjects = new Dictionary<string, GameObject>();

        StackSectorsUpdating = new Stack<string>();
    }

    // Use this for initialization
    void Start () {

        InitImageMap();
        //if(StartPositFrameMap == new Vector3())
        //    StartPositFrameMap = prefabFrameMap.transform.position;
        prefabFrameMap.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitImageMap()
    {
        //int sizeDraw = Helper.HeightLevel * SizeCellMap;
        //listPersonsTypes = new List<SaveLoadData.TypePrefabs>();
        //listPersonsPrefabTexture = new List<Texture2D>();
        //listPersonsMapTexture = new List<Texture2D>();
    }


    private void CreateFrameMap()
    {
        Vector2 posHero = Storage.PlayerController.transform.position;
        //prefabFrameMap.transform.position = new Vector3(posHero.x -12.5f, posHero.y + 5, -2);
        //prefabFrameMap.transform.position = new Vector3(posHero.x, posHero.y, -2);
        FramePosition = new Vector3(posHero.x, posHero.y, ZOrderMap);
        //prefabFrameMap.transform.localScale = new Vector3(10F, 10f, 0);
        
        prefabFrameMap.transform.SetParent(Storage.PlayerController.transform);

        //prefabFrameMap.transform.position = new Vector3(posHero.x, posHero.y, -2);

        Frame.Show(true);
    }

   public bool IsOpen
    {
        get
        {
            return prefabFrameMap.activeSelf;
        }
    }


    //#region Fill Image map icons
    

    public void Refresh()
    {
        CreateTextureMap(SizeCellMap, true);
        //StartCoroutine(CreateTextureMap(SizeCellMap, true));
        m_IsCreatedMap = true;
        //DrawLocationHero(true);
    }

    public void Create(bool isNewCreate = false)
    {
        if (Storage.Instance.SelectFieldPosHero != saveHeroPosField)
        {
            saveHeroPosField = Storage.Instance.SelectFieldPosHero;
            //isNewCreate = true;
        }

        if (!m_IsCreatedMap || isNewCreate)
        {
            CreateTextureMap(SizeCellMap); //<--- CreateFrameMap()
            //StartCoroutine(CreateTextureMap(SizeCellMap));

            m_IsCreatedMap = true;
            //Storage.PlayerController.CameraMapOn(true);
            CameraMapOn(true);
            //DrawLocationHero(true);
        }
        else
        {
            Show();
        }

        if (IsOpen)
        {
            Storage.DrawGeom.DrawClear();
            //DrawLocationHero(true);
            float distMap = Vector2.Distance(Storage.PlayerController.transform.position, FramePosition);
            if (distMap > 50f)
            {
                Frame.Restart();
            }
        }
    }

    public bool IsValid
    {
        get
        {
            float distMap = Vector2.Distance(Storage.PlayerController.transform.position, FramePosition);
            if (distMap > 35f) //30f
            {
                return false;
            }
            return true;
        }
    }

    public void Show()
    {
        bool isShow = !prefabFrameMap.activeSelf;

        //Storage.PlayerController.CameraMapOn(isShow);
        CameraMapOn(isShow, false);

        //prefabFrameMap.SetActive(!prefabFrameMap.activeSelf);
        Frame.Show(isShow);

        Storage.Map.MarkerMapWorldCell.SetActive(isShow);

        AddRefreshCurrentSector();

        RefreshGrid(); 
    }

    Vector3 oldPosCamMap = new Vector3();

    public void CameraMapOn(bool isOpenMap, bool isRestartingLocation = true)
    {
        Camera mainCamera = Storage.Instance.MainCamera;
        Camera mapCamera = Storage.PlayerController.CameraMap;

        if (isOpenMap)
        {
            //#fix map
            //prefabFrameMap.transform.position = hero.MainCamera.transform.position;
            //prefabFrameMap.transform.SetParent(Storage.PlayerController.transform);
            //CreateFrameMap();
            Frame.ValidateStartPosition();
            if(oldPosCamMap!= mainCamera.transform.position)
            {
                Vector3 posMove = mainCamera.transform.position - oldPosCamMap;
                mapCamera.transform.position += posMove;
                oldPosCamMap = mainCamera.transform.position;
            }

            //hero.GetComponent<Rigidbody2D>().mass = 10000;
            //hero.GetComponent<CapsuleCollider2D>().enabled = !hero.GetComponent<CapsuleCollider2D>().enabled;
            //hero.Rb2D.Sleep();
            Storage.PlayerController.Disable();

            if (isRestartingLocation)
                mapCamera.transform.position = mainCamera.transform.position;
            mapCamera.enabled = true;
            mainCamera.enabled = false;

            //#TEST
            //int LayerUI = LayerMask.NameToLayer("LayerUI");
            //int LayerObjects = LayerMask.NameToLayer("LayerObjects");
            //Debug.Log("_________IgnoreLayerCollision: " + LayerUI + " > " + LayerObjects);
            //Physics.IgnoreLayerCollision(LayerUI, LayerObjects, true);

            //Physics.IgnoreLayerCollision(LayerObjects, LayerUI, true);
            //CameraMap.cullingMask = LayerObjects;
        }
        else
        {
            //hero.GetComponent<Rigidbody2D>().mass = 0;

            //hero.GetComponent<CapsuleCollider2D>().enabled = !hero.GetComponent<CapsuleCollider2D>().enabled;
            //hero.Rb2D.WakeUp();
            Storage.PlayerController.Enable();
            if (!Storage.Player.HeroExtremal)
                Storage.Player.HeroExtremal = false;

            mainCamera.enabled = true;
            mapCamera.enabled = false;

            //#TEST
            //int LayerUI = LayerMask.NameToLayer("LayerUI");
            //int LayerObjects = LayerMask.NameToLayer("LayerObjects");
            //Debug.Log("_________IgnoreLayerCollision No: " + LayerUI + " > " + LayerObjects);
            //Physics.IgnoreLayerCollision(LayerUI, LayerObjects, false);
        }

        //Debug.Log("---------- Change ----E:" + hero.enabled + " S:" + hero.Rb2D.IsSleeping() + " / C:"  + hero.MainCamera.enabled + "   [" + DateTime.Now);
        //Storage.Events.ListLogAdd = "---------------------- Change ---- ";
        //Storage.Events.ListLogAdd = "enabled = " + hero.enabled;
        //Storage.Events.ListLogAdd = "Collider= " + hero.GetComponent<CapsuleCollider2D>().enabled;
        //Storage.Events.ListLogAdd = "Rb2D.IsSleeping = " + hero.Rb2D.IsSleeping();
        //Storage.Events.ListLogAdd = "MainCamera.enabled = " + hero.MainCamera.enabled;
    }

    public void CreateTextureMap(int scaleCell = 1, bool isRefresh = false)
    {
        string indErr = "start";
        int sizeMap = Helper.HeightLevel;
        int sizeDraw = Helper.HeightLevel * scaleCell;
        int addSize = scaleCell - 1;
        //List<Color> colorsPersons = new List<Color>();
        //List<SaveLoadData.TypePrefabs> listPersonsTypes = new List<SaveLoadData.TypePrefabs>();
        //List<Texture2D> listPersonsPrefabTexture = new List<Texture2D>();
        //List<Texture2D> listPersonsMapTexture = new List<Texture2D>();

        if (!isRefresh)
            CreateFrameMap();

        //Storage.Events.ListLogAdd = "Loaded map.." + DateTime.Now.ToLongTimeString();

        bool isPerson = false;
        //Texture2D texture = new Texture2D(sizeDraw, sizeDraw);
        //textureMap = new Texture2D(sizeDraw, sizeDraw);


        //------- GRID MAP
        if (IsGridMap)
        {
            //var spriteMapC = Sprite.Create(textureMap, new Rect(0.0f, 0.0f, sizeDraw, sizeDraw), new Vector2(0.5f, 0.5f), 100.0f);
            //prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMapC;
            if (m_isGridMapLoaded)
            {
                BoxCollider2D colliderMapC = prefabFrameMap.GetComponent<BoxCollider2D>();
                if (colliderMapC != null)
                {
                    prefabFrameMap.GetComponent<BoxCollider2D>().size = new Vector3(sizeDraw / Helper.WidthLevel, sizeDraw / Helper.HeightLevel, 0);
                }
            }
            LoadGrid();
            return;
        }
        //----------------------

        //textureMap
        //textureMap = null;
        //        textureMap = new Texture2D(sizeDraw, sizeDraw);
        //# fix mem 2. 
        Texture2D textureMap = new Texture2D(sizeDraw, sizeDraw);


        try
        {
            indErr = "1";

            for (int y = 0; y < sizeMap; y++)
            {
                indErr = "2";
                for (int x = 0; x < sizeMap; x++)
                {
                    indErr = "3";
                    isPerson = false;
                    indErr = "5";
                    string nameField = Helper.GetNameField(x, y);
                    indErr = "6";
                    SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
                    indErr = "7";
                    if (!ReaderScene.IsGridDataFieldExist(nameField))
                    {
                        DrawTextureTo(scaleCell, indErr, addSize, textureMap, y, x, prefabType);
                        continue;
                    }
                    indErr = "9";
                    foreach (ModelNPC.ObjectData datObjItem in ReaderScene.GetObjectsDataFromGrid(nameField))
                    {
                        indErr = "10";
                        //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
                        if (datObjItem.TypePrefabName != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                        datObjItem.TypePrefabName != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
                        {
                            indErr = "11";
                            prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TypePrefabName);
                        }
                        else
                        {
                            indErr = "12";
                            ModelNPC.GameDataBoss bossObj = datObjItem as ModelNPC.GameDataBoss;
                            if (bossObj != null)
                            {
                                indErr = "";
                                prefabType = SaveLoadData.TypePrefabs.PrefabBoss;

                                ////+++DRAW PERSON ---------------------------------
                                //    //Texture2D personTexture = Storage.GridData.GetSpriteBoss(bossObj.Level).texture;
                                //    //listPersonsPrefabTexture.Add(personTexture);
                                ////>>>> Texture2D personMapTexture_True = Storage.GridData.GetTextuteMapBoss(bossObj.Level);
                                //Texture2D personMapTexture = TypeBoss.Instance.GetNameTextureMapForIndexLevel(bossObj.Level);
                                //listPersonsMapTexture.Add(personMapTexture);
                                //-----------------------------------------------------

                                isPerson = true;
                            }
                        }
                    }
                    indErr = "14";
                    ////----- DRAW
                    if (!isPerson)
                    {
                        DrawTextureTo(scaleCell, indErr, addSize, textureMap, y, x, prefabType);
                    }
                    //-----------------
                    //else
                    //{
                    indErr = "22";
                    //---- Draw Person

                    //+++DRAW PERSON
                    //for (int indMap2D = 0; indMap2D < listPersonsMapTexture.Count(); indMap2D++)
                    //{
                    //    Texture2D texturePerson = listPersonsMapTexture[indMap2D];
                    //    DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, texturePerson);
                    //}
                }
            }
        }
        catch (Exception x)
        {
            Debug.Log("############# CreateTextureMap: " + indErr + "  " + x.Message);
            return;
        }

        textureMap = DrawLocationHero(textureMap);

        textureMap.Apply();

        //#fix mem
        var _spriteMap = Sprite.Create(textureMap, new Rect(0.0f, 0.0f, textureMap.width, textureMap.height), new Vector2(0.5f, 0.5f), 100.0f);

        prefabFrameMap.GetComponent<SpriteRenderer>().sprite = _spriteMap;
        BoxCollider2D colliderMap = prefabFrameMap.GetComponent<BoxCollider2D>();
        if (colliderMap != null)
        {
            prefabFrameMap.GetComponent<BoxCollider2D>().size = new Vector3(textureMap.width / Helper.WidthLevel, textureMap.height / Helper.HeightLevel, 0);
        }
    }

    //Texture2D textureMap = null;

    public Sprite GetBildSpriteMap(out Texture2D textureMap, int scaleCell = 1, bool isRefresh = false, int offsetMapX =0, int offsetMapY =0)
    {
        string indErr = "start";

        int sizeMap = 25;
        int sizeDraw = sizeMap * scaleCell;

        int addSize = scaleCell - 1;

        offsetMapX *= sizeMap;
        offsetMapY *= sizeMap;

        textureMap = new Texture2D(sizeDraw, sizeDraw);
        //textureMap = new Texture2D(sizeDraw, sizeDraw, UnityEngine.Experimental.Rendering.GraphicsFormat.RGBA_DXT5_SRGB, UnityEngine.Experimental.Rendering.TextureCreationFlags.MipChain);
        //textureMap = new Texture2D(sizeDraw, sizeDraw, UnityEngine.Experimental.Rendering.GraphicsFormat.RGBA_DXT5_UNorm, UnityEngine.Experimental.Rendering.TextureCreationFlags.MipChain);

        try
        {
            indErr = "1";

            for (int y = 0; y < sizeMap; y++)
            {
                indErr = "2";
                for (int x = 0; x < sizeMap; x++)
                {
                    indErr = "3";
                    //isPerson = false;
                    indErr = "5";

                    int mapX = x + offsetMapX;
                    int mapY = y + offsetMapY;
                    string nameField = Helper.GetNameField(mapX, mapY);
                    indErr = "6";
                    SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;

                    //--------------
                    if(nameField == Storage.Instance.SelectFieldPosHero)
                    {
                        DrawTextureTo(scaleCell, indErr, addSize, textureMap, y, x, Storage.Map.textureHero);
                        continue;
                    }
                    //--------------
                    indErr = "7";
                    if (!ReaderScene.IsGridDataFieldExist(nameField))
                    {
                        DrawTextureTo(scaleCell, indErr, addSize, textureMap, y, x, prefabType);
                        continue;
                    }
                    indErr = "9";
                    foreach (ModelNPC.ObjectData datObjItem in ReaderScene.GetObjectsDataFromGrid(nameField))
                    {
                        indErr = "10";
                        //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
                        if (datObjItem.TypePrefabName != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                            datObjItem.TypePrefabName != SaveLoadData.TypePrefabs.PrefabBoss.ToString() &&
                            datObjItem.TypePoolPrefab != PoolGameObjects.TypePoolPrefabs.PoolPerson)
                        {
                            indErr = "11";
                            prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TypePrefabName);
                            if (datObjItem.TypePoolPrefab != PoolGameObjects.TypePoolPrefabs.PoolFloor)
                                break;
                        }
                    }
                    DrawTextureTo(scaleCell, indErr, addSize, textureMap, y, x, prefabType);
                    indErr = "14";
                }
            }
        }
        catch (Exception x)
        {
            Debug.Log("############# GetTextureMap: " + indErr + "  " + x.Message);
            return null;
        }

        //textureMap.Apply();
        try
        {
            textureMap.Apply();
            //textureMap.Apply(false, true);
        }
        catch (Exception x2)
        {
            Debug.Log("############# GetTextureMap:  textureMap.Apply " + indErr + "  " + x2.Message);
            return null;
        }

        Sprite _spriteMap = null;
        try
        {
            //spriteMap = Sprite.Create(textureMap, new Rect(0.0f, 0.0f, textureMap.width, textureMap.height), new Vector2(0.5f, 0.5f), 100.0f);
            _spriteMap = Sprite.Create(textureMap, new Rect(0.0f, 0.0f, textureMap.width, textureMap.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        catch (Exception x2)
        {
            Debug.Log("############# GetTextureMap:  Sprite.Create " + indErr + "  " + x2.Message);
            return null;
        }
        return _spriteMap;
    }

    //public void DestroyTexture()
    //{
    //    Destroy(textureMap);
    //    textureMap = null;
    //}

    private string saveHeroPosField = "";
    public void DrawLocationHero(bool isOpenChange = false)
    {
        if (!isOpenChange)
            return;

        if (IsOpen || isOpenChange)
        {
            Texture2D textureMap = prefabFrameMap.GetComponent<SpriteRenderer>().sprite.texture;
            Texture2D textureMapResult = textureMap;
            //Texture2D textureResult = new Texture2D(textureMap.width, textureMap.height);
            //Graphics.CopyTexture(textureMap, 0, 0, 0, 0, textureMap.width, textureMap.height, textureResult, 0, 0, 0, 0);

            //-- Restore ---
            if (Storage.Instance.SelectFieldPosHero != saveHeroPosField)
            {
                if (!string.IsNullOrEmpty(saveHeroPosField))
                {
                    SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
                    string nameField = saveHeroPosField;
                    if (ReaderScene.IsGridDataFieldExist(nameField))
                    {
                        foreach (ModelNPC.ObjectData datObjItem in ReaderScene.GetObjectsDataFromGrid(nameField))
                        {
                            if (datObjItem.TypePrefabName != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                            datObjItem.TypePrefabName != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
                            {
                                prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TypePrefabName);
                                break; //@@@+
                            }
                        }
                    }
                    Vector2 posF = Helper.GetPositByField(nameField);
                    int x = (int)posF.x;
                    int y = (int)posF.y;
                    DrawTextureTo(SizeCellMap, "Restore", SizeCellMap - 1, textureMapResult, y, x, prefabType);
                }
                saveHeroPosField = Storage.Instance.SelectFieldPosHero;
            }
            //------

            //@@@+
            textureMapResult = AddTextureHeroOnMap(textureMapResult);

            //@@@-
            //Vector2 posHero = Helper.GetPositByField(Storage.Instance.SelectFieldPosHero);
            //int heroX = (int)posHero.x;
            //int heroY = (int)posHero.y;

            //DrawTextureTo(SizeCellMap, "Hero", SizeCellMap - 1, textureResult, heroY, heroX, Storage.Map.textureHero);

            textureMapResult.Apply();
            Sprite spriteMe = Sprite.Create(textureMapResult, new Rect(0.0f, 0.0f, textureMapResult.width, textureMapResult.height), new Vector2(0.5f, 0.5f), 100.0f);
            prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
        }
    }

    public Texture2D DrawLocationHero(Texture2D textureResult)
    {
        //Texture2D textureResult = new Texture2D(textureMap.width, textureMap.height);
        //Graphics.CopyTexture(textureMap, 0, 0, 0, 0, textureMap.width, textureMap.height, textureResult, 0, 0, 0, 0);

        //-- Restore ---
        //if (Storage.Instance.SelectFieldPosHero != saveHeroPosField)
        //{
            //Clear old position Hero
            if (!string.IsNullOrEmpty(saveHeroPosField))
            {
                SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
                string nameField = saveHeroPosField;
                if (ReaderScene.IsGridDataFieldExist(nameField))
                {
                    foreach (ModelNPC.ObjectData datObjItem in ReaderScene.GetObjectsDataFromGrid(nameField))
                    {
                        if (datObjItem.TypePrefabName != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                        datObjItem.TypePrefabName != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
                        {
                            prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TypePrefabName);
                        }
                    }
                }
                Vector2 posF = Helper.GetPositByField(nameField);
                int x = (int)posF.x;
                int y = (int)posF.y;
                DrawTextureTo(SizeCellMap, "Restore", SizeCellMap - 1, textureResult, y, x, prefabType);
            }
            saveHeroPosField = Storage.Instance.SelectFieldPosHero;
        //}
        //------

        //@@@+

        textureResult = AddTextureHeroOnMap(textureResult);
        //@@@-
        //Vector2 posHero = Helper.GetPositByField(Storage.Instance.SelectFieldPosHero);
        //int heroX = (int)posHero.x;
        //int heroY = (int)posHero.y;

        //DrawTextureTo(SizeCellMap, "Hero", SizeCellMap - 1, textureResult, heroY, heroX, Storage.Map.textureHero);


        return textureResult;
    }

    public Texture2D AddTextureHeroOnMap(Texture2D textureMapResult)
    {
        Vector2 posHero = Helper.GetPositByField(Storage.Instance.SelectFieldPosHero);
        int heroX = (int)posHero.x;
        int heroY = (int)posHero.y;
        DrawTextureTo(SizeCellMap, "Hero", SizeCellMap - 1, textureMapResult, heroY, heroX, Storage.Map.textureHero);
        return textureMapResult;
    }

    //@@@?
    /*
    public void DrawMapCell(int y, int x, SaveLoadData.TypePrefabs prefabType)
    {
        Texture2D textureMap = prefabFrameMap.GetComponent<SpriteRenderer>().sprite.texture;
        Texture2D textureResult = textureMap;

        DrawTextureTo(SizeCellMap, "?", SizeCellMap - 1, textureResult, y, x, prefabType);

        textureResult.Apply();
        Sprite spriteMe = Sprite.Create(textureResult, new Rect(0.0f, 0.0f, textureResult.width, textureResult.height), new Vector2(0.5f, 0.5f), 100.0f);
        prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    }
    */

    public void DrawMapCell(int y, int x, Texture2D texturePrefab)
    {
        //@@@ Celagcy code , not drawing on FrameMap, true drawing on m_listCellsGridMap
        return;

        Texture2D textureMap = prefabFrameMap.GetComponent<SpriteRenderer>().sprite.texture;
        Texture2D textureResult = textureMap;

        int startX1 = x * SizeCellMap;
        int startY1 = y * SizeCellMap;
        int addSize = SizeCellMap - 1;

        // Correct .............
        startY1 = textureResult.height - startY1 - addSize;
        //.................

        try
        {
            //textureResult.alphaIsTransparency = true;
            Graphics.CopyTexture(texturePrefab, 0, 0, 0, 0, addSize, addSize, textureResult, 0, 0, (int)startX1, (int)startY1);
            textureResult.Apply();
        }
        catch (Exception ex)
        {
            Debug.Log("######### DrawMapCell [" + texturePrefab.name + "] :" + ex.Message);
            return;
        }

        Sprite spriteMe = Sprite.Create(textureResult, new Rect(0.0f, 0.0f, textureResult.width, textureResult.height), new Vector2(0.5f, 0.5f), 100.0f);
        prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;

    }

    private Dictionary<string, string> m_NamesTexturesMaps = null;
    Dictionary<string, string> NamesTexturesMaps
    {
        get
        {
            if (m_NamesTexturesMaps == null)
            {
                m_NamesTexturesMaps = new Dictionary<string, string>
                {
                    {"PrefabVood", "VoodMap" },
                    {"PrefabElka", "ElkaMap" },
                    {"PrefabRock", "RockMap" },
                    {"PrefabWallRock", "WallRockMap" },
                    {"PrefabWallWood","WallWoodMap" },
                    {"PrefabField", "FieldMap" },
                    {"PrefabHero", "HeroMap" },
                };

            }
            return m_NamesTexturesMaps;
        }

    }

    //DrawTextureTo(SizeCellMap, "Restore", SizeCellMap - 1, textureResult, y, x, prefabType);
    public void DrawTextureTo(int scaleCell, string indErr, int addSize, Texture2D texture, int y, int x, SaveLoadData.TypePrefabs prefabType)
    {
        //------------------
        Texture2D texturePrefab = GetPrefabTexture(prefabType);
        //------------------
        //string nameTexture = NamesTexturesMaps[prefabType.ToString()];
        //Texture2D texturePrefab = TexturesMaps[nameTexture];
        //------------------
        //Texture2D texturePrefab = GetSpriteAtlasPrefab(prefabType.ToString()).texture;
        //------------------

        if (texturePrefab == null)
        {
            Debug.Log("###### CreateTextureMap.ManagerPalette: " + indErr + "   prefabType:" + prefabType + " texturePrefab Is NULL ");
            //return;
        }
       
        int startX1 = x * scaleCell;
        int startY1 = y * scaleCell;

        // Correct .............
        startY1 = texture.height - startY1 - addSize;
        //.................

        if (texturePrefab != null && texture.format.ToString() != texturePrefab.format.ToString())
        {
            //Debug.Log(".......... Start CopyTexture   prefabType:" + prefabType + " : " + startX1 + "x" + startY1 + " Size=" + addSize);
            //Debug.Log(".......... Start CopyTexture   Formats source:" + texture.format.ToString());
            //Debug.Log(".......... Start CopyTexture " + prefabType + "  Formats texturePrefab:" + texturePrefab.format.ToString());
            Debug.Log(".......... CopyTexture Type:" + prefabType + " Formats source:" + texture.format.ToString() + " Formats texturePrefab: " + texturePrefab.format.ToString());
            return;
        }

        try
        {
            Graphics.CopyTexture(texturePrefab, 0, 0, 0, 0, addSize, addSize, texture, 0, 0, (int)startX1, (int)startY1);
        }catch(Exception ex)
        {
            Debug.Log("############## DrawTextureTo " + ex.Message);
        }
    }

    private void DrawTextureTo(int scaleCell, string indErr, int addSize, Texture2D texture, int y, int x, Texture2D texturePrefab)
    {
        int startX1 = x * scaleCell;
        int startY1 = y * scaleCell;

        // Correct .............
        startY1 = texture.height - startY1 - addSize;
        //.................
        try
        {

            Graphics.CopyTexture(texturePrefab, 0, 0, 0, 0, addSize, addSize, texture, 0, 0, (int)startX1, (int)startY1);
        }catch(Exception ex)
        {
            Debug.Log("############## DrawTextureTo texturePrefab " + ex.Message);
        }
    }

    private string test_textureType = "";

    public Texture2D GetPrefabTexture(SaveLoadData.TypePrefabs typePredab)
    {
        //switch(typePredab)
        //{
        //    case SaveLoadData.TypePrefabs.PrefabElka:
        //        break;
        //    case SaveLoadData.TypePrefabs.PrefabRock:
        //        break;
        //    case SaveLoadData.TypePrefabs.PrefabVood:
        //        break;
        //    case SaveLoadData.TypePrefabs.PrefabWallRock:
        //        break;
        //    case SaveLoadData.TypePrefabs.PrefabWallWood:
        //        break;
        //}
        string strTypePref = typePredab.ToString();

        //if (Storage.Palette == null || Storage.Palette.TexturesPrefabs == null)
        if (Storage.Palette == null)
        {
            Debug.Log("############# GetPrefabTexture  Palette Or TexturesPrefabs is Empty !!!");
            return null;
        }

        //Sprite[] _sprites = GetSpritesAtlasMapPrefab();
        
        if (!Storage.Palette.SpritesMaps.ContainsKey(strTypePref))
        //if (!Storage.Palette.TexturesMaps.ContainsKey(strTypePref))
        {
            //Debug.Log("############# GetPrefabTexture   TexturesPrefabs not found type: " + typePredab.ToString());
            Debug.Log("############# GetPrefabTexture   Palette.SpritesMaps not found type: " + typePredab.ToString());
            return Storage.Palette.TexturesMaps["PrefabField"]; //fix 
        }

        //------- test
        List<string> strFilter = new List<string>() {
                    "PrefabVood",
                    "PrefabElka",
                    "PrefabRock",
                    "PrefabWallRock",
                    "PrefabWallWood",
                    "PrefabField",
                    "PrefabHero",
        };

        //------------------------------
        //Texture2D textureRes = Storage.Palette.TexturesMaps[strTypePref];
        Texture2D textureRes = Storage.Palette.SpritesMaps[strTypePref].texture;

        if (!strFilter.Contains(strTypePref))
        {
            if(test_textureType != strTypePref)
            {
                test_textureType = strTypePref;
                Debug.Log("---- DRAW ICON >> " + strTypePref);
            }
            //textureRes = Storage.Palette.SpritesMaps[strTypePref].texture;

            //var spriteRes = Storage.Palette.SpriteAtlasMapPrefab.GetSprite(strTypePref + "Map");
            //if (spriteRes == null)
            //    Debug.Log("---- DRAW ICON >> spriteRes = null");
            //else
            //    textureRes = spriteRes.texture;
        }

        //Texture2D textureRes = Storage.GridData.PrefabElka.GetComponent<SpriteRenderer>().sprite.texture;
        return textureRes;
    }


    public void UpdateMarkerPointCell()
    {
        Vector2 pos = Storage.Map.SelectPointField;
        pos.y *= -1;
        //if (Helper.IsBigWorld)
        //{
        //    //int offSetSizeWorld = Helper.SizeWorldOffSet;
        //    pos.y += Helper.SizeWorldOffSet;
        //    pos.x += Helper.SizeWorldOffSet;
        //}
        MarkerMapWorldCell.transform.position = pos * Storage.ScaleWorld;
        

        //Storage.Map.SelectPointField;

        //Storage.Events.ListLogAdd = "-------------------------------- SizeWorldOffSet: "  + Helper.SizeWorldOffSet;
        //Storage.Events.ListLogAdd = "SelectPointField: " + Storage.Map.SelectPointField;
        //Storage.Events.ListLogAdd = "MarkerMapWorldCell: " + MarkerMapWorldCell.transform.position;
    }

    

    public void LoadGrid()
    {
        if (m_isGridMapLoaded)
        {
            RefreshGrid();
            return;
        }
        m_isGridMapLoaded = true;
        m_lastSectorHeroVisit = FieldToSector(saveHeroPosField);

        //List<Texture2D> listTextures = Storage.TilesManager.ListTexturs.Where(p => p.name.IndexOf("Prefab") != -1).ToList();

        //int countColumnMap = listTextures[0].width;
        int SizeSectror = 4;
        int countColumnMap = Helper.SpeedWorld * SizeSectror;// 12;
        //var SizeBrush = 1;
        //m_GridMap.startCorner = GridLayoutGroup.Corner.UpperLeft;
        //m_GridMap.startCorner = GridLayoutGroup.Corner.UpperLeft;
        //m_GridMap.childAlignment = TextAnchor.UpperLeft;
        //m_GridMap.constraintCount = countColumnMap;
        //ResizeScaleGrid(countColumnMap, 1.1f);

        //string field = Helper.GetNameField(1, 1);
        GameObject[] gobjCells = GameObject.FindGameObjectsWithTag("MapGridCell");
        for (int i = 0; i < gobjCells.Length; i++)
        {
            Destroy(gobjCells[i]);
        }

        m_listCellsGridMap.Clear();

        //m_GridMap.constraintCount = countColumnMap;

        int index = 0;
        int maxCellGridMap = countColumnMap;// (Helper.HeightLevel / 100) / 5;

        for (int x = 1; x < maxCellGridMap + 1; x++)
        {
            for (int y = 1; y < maxCellGridMap + 1; y++)
            {

                CreateCell(x, y);

                index++;
            }
        }

        if(!IsAutoAction)
        {
            //foreach(var cell in m_listCellsGridMap)
            //{
            //    CellGridMapController sector = cell.GetSector();
            //    RefreshGrid(sector.X, sector.Y);
            //}
            StartCoroutine(StartLoadGrid());
        }
     }

    private void ReloadCellGrid(int x, int y)
    {
        DestroyCell(x,y);
        CreateCell(x, y);
    }

    private void DestroyCell(int x, int y)
    {
        string field = Helper.GetNameField(y, x); //FIX
        string name = "MapGridCell" + field;
        //cellMap.name = "MapGridCell" + field;
        //GameObject[] gobjCells = GameObject.FindGameObjectsWithTag("MapGridCell");
        GameObject gobjCell = GameObject.Find(name);
        if(gobjCell!=null)
            Destroy(gobjCell);
   
        int destroyObjsIndex = m_listCellsGridMap.FindIndex(p => p == null);
        if(destroyObjsIndex!=-1)
            m_listCellsGridMap.RemoveAt(destroyObjsIndex);
    }

    private void CreateCell(int x, int y)
    {
        GameObject cellMap = (GameObject)Instantiate(PrefabSpriteCellMap);
        //cellMap.transform.SetParent(this.gameObject.transform);
        cellMap.transform.SetParent(m_GridMap.transform);
        //Sprite spriteTile = Storage.TilesManager.CollectionSpriteTiles[item.name];
        //Sprite spriteTile = cellMap.GetComponent<SpriteRenderer>().sprite;

        //field = Helper.GetNameField(x, y);
        var field = Helper.GetNameField(y, x); //FIX

        cellMap.name = "MapGridCell" + field;
        cellMap.tag = "MapGridCell";

        //cellMap.GetComponent<SpriteRenderer>().sprite = spriteTile;
        //cellMap.GetComponent<CellMapControl>().DataTileCell = new DataTile() { Name = item.name, X = index, Tag= TypesStructure.Prefab.ToString() };
        //cellMap.GetComponent<CellMapControl>().DataTileCell = new DataTile() { Name = item.name, X = index, Tag = typeTilePrefab.ToString() };
        cellMap.SetActive(true);

        m_listCellsGridMap.Add(cellMap);
    }

    IEnumerator StartLoadGrid()
    {
        yield return null;

        //while (StackSectorsUpdating.Count > 0)
        //{
        //    yield return null;

        //    if (IsReloadGridMap)
        //        yield return null;

        //    string sector = StackSectorsUpdating.Pop();
        //    Debug.Log("************ RefreshGrid " + sector);

        //    IsReloadGridMap = true;
        //    RefreshGrid(sector);

        //    yield return null;
        //}
        foreach (var cell in m_listCellsGridMap)
        {
            if (IsReloadGridMap)
                yield return null;

            CellGridMapController sector = cell.GetSector();

            IsReloadGridMap = true;
            RefreshGrid(sector.X, sector.Y);
            yield return null;
        }

    }

        private void ResizeScaleGrid(int column, float ratio = 0.9f)
    {
        //float size = prefabFrameMap.GetComponent<RectTransform>().rect.width;
        float size = Helper.HeightLevel;

        float sizeCorr = (size / column) * ratio;
        Vector2 newSize = new Vector2(sizeCorr, sizeCorr);
        m_GridMap.cellSize = newSize;
    }

    #region Refresh Sectors

    public void AddUpdatingField(string fieldSectror)
    {
        if (!StackSectorsUpdating.Contains(fieldSectror))
        {
            //Debug.Log("+++++++++++ AddUpdatingField  " + fieldSectror);
            StackSectorsUpdating.Push(fieldSectror);
        }
    }

    public void RefreshGrid()
    {
        //#fix mem
        //string sector1 = StackSectorsUpdating.Peek();
        //foreach (string itemSector in StackSectorsUpdating)
        //{
        //    Vector2 posSector = Helper.GetPositByField(itemSector);
        //    ReloadCellGrid((int)posSector.x, (int)posSector.y);
        //}

        StartCoroutine(StartRefresh());

        return;

        CheckPosHero();

        while (StackSectorsUpdating.Count>0)
        {
            string sector = StackSectorsUpdating.Pop();
            Debug.Log("************ RefreshGrid " + sector);
            RefreshGrid(sector);
        }
    }

    //UpdateSprite()

    public void RefreshGrid(string sector)
    {
        if (Storage.Instance.IsLoadingWorld)
            return;

        ////#fix mem
        //Vector2 posSector = Helper.GetPositByField(sector);
        //ReloadCellGrid((int)posSector.x, (int)posSector.y);
        //for (int i = m_listCellsGridMap.Count - 1; i >= 0; i--)
        //{
        //    if (m_listCellsGridMap[i] == null)
        //    {
        //        m_listCellsGridMap.RemoveAt(i);
        //    }
        //}

        GameObject cellMap = m_listCellsGridMap.Find(p => p.name == "MapGridCell" + sector);
        if (cellMap == null)
        {
            Debug.Log("###### RefreafGrid cellMap==null");
            IsReloadGridMap = false;
            return;
        }
        CellGridMapController cellMapController = cellMap.GetComponent<CellGridMapController>();
        if (cellMapController == null)
        {
            Debug.Log("###### RefreafGrid cellMapController == null");
            IsReloadGridMap = false;
            return;
        }
        cellMapController.Refresh();
    }

    

    IEnumerator StartRefresh()
    {
        CheckPosHero();

        yield return null;


        ////#fix mem
        ////string sector1 = StackSectorsUpdating.Peek();
        //foreach (string itemSector in StackSectorsUpdating)
        //{
        //    Vector2 posSector = Helper.GetPositByField(itemSector);
        //    ReloadCellGrid((int)posSector.x, (int)posSector.y);
        //}

        while (StackSectorsUpdating.Count > 0)
        {
            if (Storage.Instance.IsLoadingWorld)
                yield return null;

            yield return null;

            if(IsReloadGridMap)
                yield return null;

            //#?????
            if(StackSectorsUpdating.Count == 0)
                yield return null;

            string sector = StackSectorsUpdating.Pop();
            Debug.Log(Storage.EventsUI.ListLogAdd = "************ Refresh MAP Grid " + sector);


            IsReloadGridMap = true;
            RefreshGrid(sector);

            yield return null;
        }

        //-- add start sector
        AddRefreshCurrentSector();
    }

    private void AddRefreshCurrentSector()
    {
        string newSector = FieldToSector(saveHeroPosField);
        AddUpdatingField(newSector);
    }


    public void RefreshGrid(int cellX, int cellY)
    {
        string field = Helper.GetNameField(cellX, cellY);
        RefreshGrid(field);
    }

    private string m_lastSectorHeroVisit = "";
    public void CheckPosHero()
    {
        if (saveHeroPosField != Storage.Instance.SelectFieldPosHero)
        {
            saveHeroPosField = Storage.Instance.SelectFieldPosHero;
            string newSector = FieldToSector(saveHeroPosField);
            if (m_lastSectorHeroVisit != newSector)
            {
                m_lastSectorHeroVisit = newSector;
                AddUpdatingField(m_lastSectorHeroVisit);
            }
        }

    }

    public void CheckSector(string checkField)
    {
        string newSector = FieldToSector(checkField);
        AddUpdatingField(newSector);
    }

    public void RefreshFull()
    {
        int sizeWorldSector = Helper.WidthLevel / SizeCellMap; //12
        for (int x=0; x< sizeWorldSector; x++) 
        {
            for (int y = 0; y < sizeWorldSector; y++)
            {
                string sector = Helper.GetNameField(x + 1, y + 1);
                AddUpdatingField(sector);
            }
        }
    }


    public string FieldToSector(string p_field)
    {
        string sector = "";
        Vector2 posF = Helper.GetPositByField(p_field);
        float posSectorX = posF.x / SizeCellMap;
        float posSectorY = posF.y / SizeCellMap;
        sector = Helper.GetNameField(posSectorX + 1, posSectorY + 1);
        //Storage.Events.ListLogAdd = "   }}}}  Sector: " + sector;
        return sector;
    }

    #endregion

}

public static class MapExtension
{

    public static CellGridMapController GetSector(this GameObject cellMap)
    {
        CellGridMapController cellMapController = cellMap.GetComponent<CellGridMapController>();
        return cellMapController;
    }

}



