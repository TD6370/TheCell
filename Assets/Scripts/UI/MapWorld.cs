using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapWorld : MonoBehaviour {

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
    Texture2D textureMap;
    Sprite spriteMap;

    //--- Grid Map
    public GameObject PrefabGridMap;
    public GameObject PrefabSpriteCellMap;
    public Stack<string> StackSectorsUpdating;

    private GridLayoutGroup m_GridMap;
    private List<GameObject> m_listCellsGridMap = new List<GameObject>();

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

    private void CreateFrameMap()
    {
        Vector2 posHero = Storage.PlayerController.transform.position;
        //prefabFrameMap.transform.position = new Vector3(posHero.x -12.5f, posHero.y + 5, -2);
        //prefabFrameMap.transform.position = new Vector3(posHero.x, posHero.y, -2);
        prefabFrameMap.transform.position = new Vector3(posHero.x, posHero.y, 0);
        //prefabFrameMap.transform.localScale = new Vector3(10F, 10f, 0);
        
        prefabFrameMap.transform.SetParent(Storage.PlayerController.transform);
        Frame.Show(true);
    }

   public bool IsOpen
    {
        get
        {
            return prefabFrameMap.activeSelf;
        }
    }

    private void InitImageMap()
    {
        //colorsPersons = new List<Color>();
        //texture = new Texture2D(sizeDraw, sizeDraw);
        int sizeDraw = Helper.HeightLevel * SizeCellMap;
        textureMap = new Texture2D(sizeDraw, sizeDraw);
        listPersonsTypes = new List<SaveLoadData.TypePrefabs>();
        listPersonsPrefabTexture = new List<Texture2D>();
        listPersonsMapTexture = new List<Texture2D>();
    }

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
            isNewCreate = true;
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
            float distMap = Vector2.Distance(Storage.PlayerController.transform.position, prefabFrameMap.transform.position);
            if (distMap > 30f)
            {
                Frame.Restart();
            }
        }
    }

    public bool IsValid
    {
        get
        {
            float distMap = Vector2.Distance(Storage.PlayerController.transform.position, prefabFrameMap.transform.position);
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
        CameraMapOn(isShow);

        //prefabFrameMap.SetActive(!prefabFrameMap.activeSelf);
        Frame.Show(isShow);

        Storage.Map.MarkerMapWorldCell.SetActive(isShow);
        //DrawLocationHero(true);
        //if (isShow)
        //{
        //    Storage.DrawGeom.DrawClear();
        //    DrawLocationHero(true);
        //    float distMap = Vector2.Distance(Storage.PlayerController.transform.position, prefabFrameMap.transform.position);
        //    if (distMap > 30f)
        //    {
        //        Frame.Restart();
        //    }
        //}
    }

    public void CameraMapOn(bool isOpenMap)
    {
        var hero = Storage.PlayerController;

        if (isOpenMap)
        {
            //hero.GetComponent<Rigidbody2D>().mass = 10000;
            hero.GetComponent<CapsuleCollider2D>().enabled = !hero.GetComponent<CapsuleCollider2D>().enabled;
            hero.Rb2D.Sleep();
            hero.CameraMap.transform.position = hero.MainCamera.transform.position;
            hero.CameraMap.enabled = true;
            hero.MainCamera.enabled = false;
            int LayerUI = LayerMask.NameToLayer("LayerUI");
            int LayerObjects = LayerMask.NameToLayer("LayerObjects");
            Debug.Log("_________IgnoreLayerCollision: " + LayerUI + " > " + LayerObjects);
            Physics.IgnoreLayerCollision(LayerUI, LayerObjects, true);

            //Physics.IgnoreLayerCollision(LayerObjects, LayerUI, true);
            //CameraMap.cullingMask = LayerObjects;
        }
        else
        {
            //hero.GetComponent<Rigidbody2D>().mass = 0;
            hero.GetComponent<CapsuleCollider2D>().enabled = !hero.GetComponent<CapsuleCollider2D>().enabled;
            hero.Rb2D.WakeUp();
            hero.MainCamera.enabled = true;
            hero.CameraMap.enabled = false;
            int LayerUI = LayerMask.NameToLayer("LayerUI");
            int LayerObjects = LayerMask.NameToLayer("LayerObjects");
            Debug.Log("_________IgnoreLayerCollision No: " + LayerUI + " > " + LayerObjects);
            Physics.IgnoreLayerCollision(LayerUI, LayerObjects, false);
        }

        //Debug.Log("---------- Change ----E:" + hero.enabled + " S:" + hero.Rb2D.IsSleeping() + " / C:"  + hero.MainCamera.enabled + "   [" + DateTime.Now);
        //Storage.Events.ListLogAdd = "---------------------- Change ---- ";
        //Storage.Events.ListLogAdd = "enabled = " + hero.enabled;
        //Storage.Events.ListLogAdd = "Collider= " + hero.GetComponent<CapsuleCollider2D>().enabled;
        //Storage.Events.ListLogAdd = "Rb2D.IsSleeping = " + hero.Rb2D.IsSleeping();
        //Storage.Events.ListLogAdd = "MainCamera.enabled = " + hero.MainCamera.enabled;
    }

    //public void CreateTextureMap1(int scaleCell = 1)
    //{
    //    string indErr = "start";
    //    int sizeMap = Helper.HeightLevel;
    //    int sizeDraw = Helper.HeightLevel * scaleCell;
    //    int addSize = scaleCell - 1;

    //    CreateFrameMap();

    //    //---------------
    //    //Texture2D textureCopy = textureField;
    //    //Texture2D textureSource = new Texture2D(textureCopy.width + 50, textureCopy.height + 50);
    //    //if (textureSource.format.ToString() != textureCopy.format.ToString())
    //    //{
    //    //    Debug.Log(".......... Start CopyTexture   Formats source:" + textureSource.format.ToString());
    //    //    Debug.Log(".......... Start CopyTexture " + textureSource.format + "  Formats texturePrefab:" + textureCopy.format.ToString());
    //    //    return;
    //    //}
    //    //Graphics.CopyTexture(textureCopy, 0, 0, 0, 0, textureCopy.width, textureCopy.height, textureSource, 0, 0, 20, 20);
    //    //Sprite spriteNew = Sprite.Create(textureSource, new Rect(0.0f, 0.0f, textureSource.width, textureSource.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    //prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteNew;
    //    //return;
    //    //-----------------

    //    List<Color> colorsPersons = new List<Color>();
    //    bool isPerson = false;
    //    Texture2D texture = new Texture2D(sizeDraw, sizeDraw);

    //    try
    //    {
    //        indErr = "1";

    //        for (int y = 0; y < sizeMap; y++)
    //        {
    //            indErr = "2";
    //            for (int x = 0; x < sizeMap; x++)
    //            {
    //                indErr = "3";
    //                isPerson = false;
    //                indErr = "4";
    //                colorsPersons.Clear();
    //                indErr = "5";
    //                string nameField = Helper.GetNameField(x, y);
    //                indErr = "6";
    //                SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
    //                indErr = "7";
    //                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                {
    //                    DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, prefabType);
    //                    continue;
    //                }
    //                indErr = "8";
    //                Color colorCell = Color.clear;
    //                indErr = "9";
    //                foreach (ModelNPC.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //                {
    //                    indErr = "10";
    //                    //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                    if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                    datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                    {
    //                        indErr = "11";
    //                        prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                    }
    //                    else
    //                    {
    //                        indErr = "12";
    //                        ModelNPC.GameDataBoss bossObj = datObjItem as ModelNPC.GameDataBoss;
    //                        if (bossObj != null)
    //                        {
    //                            //Storage //bossObj.Level;
    //                            colorCell = TypeBoss.TypesBoss.Find(p => p.Level == bossObj.Level).ColorTrack;
    //                            if (colorCell == null)
    //                            {
    //                                Debug.Log("############# colorCell Point Map Person is null");
    //                                continue;
    //                            }
    //                            indErr = "13";
    //                            colorsPersons.Add(colorCell);
    //                            //prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                            indErr = "";
    //                            prefabType = SaveLoadData.TypePrefabs.PrefabBoss;
    //                            isPerson = true;
    //                        }
    //                    }
    //                }
    //                indErr = "14";

    //                if (prefabType == SaveLoadData.TypePrefabs.PrefabField)
    //                {
    //                    DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, prefabType);
    //                    //continue;
    //                }
    //                //==========================
    //                //DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, prefabType);
    //                //=======================

    //                indErr = "15";
    //                //if (colorCell == Color.clear)
    //                //{
    //                //    indErr = "16";
    //                //    if (!ManagerPalette.PaletteColors.ContainsKey(prefabType.ToString()))
    //                //    {
    //                //        Debug.Log("#### Not Color in PaletteColors for type : " + prefabType.ToString());
    //                //    }
    //                //    indErr = "17";
    //                //    try
    //                //    {
    //                //        colorCell = ManagerPalette.PaletteColors[prefabType.ToString()];
    //                //    }
    //                //    catch (Exception x1)
    //                //    {
    //                //        Debug.Log("###### CreateTextureMap.ManagerPalette: " + indErr + "   prefabType:" + prefabType + "  " + x1.Message);
    //                //        return;
    //                //    }
    //                //}

    //                //----- DRAW
    //                if (!isPerson)
    //                {
    //                    //+++
    //                        //DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, prefabType);
    //                    //++++

    //                    indErr = "18";
    //                    int startX = x * scaleCell;
    //                    int startY = y * scaleCell;
    //                    indErr = "19";

    //                    //--------------------------------
    //                    Texture2D texturePrefab = GetPrefabTexture(prefabType);//SaveLoadData.TypePrefabs
    //                    if (texturePrefab == null)
    //                    {
    //                        Debug.Log("###### CreateTextureMap.ManagerPalette: " + indErr + "   prefabType:" + prefabType + " texturePrefab Is NULL ");
    //                        continue;
    //                    }

    //                    try
    //                    {
    //                        if (texture.format.ToString() != texturePrefab.format.ToString())
    //                        {
    //                            Debug.Log(".......... Start CopyTexture   prefabType:" + prefabType + " : " + startY + "x" + startX + " Size=" + addSize);
    //                            Debug.Log(".......... Start CopyTexture   Formats source:" + texture.format.ToString());
    //                            Debug.Log(".......... Start CopyTexture " + prefabType + "  Formats texturePrefab:" + texturePrefab.format.ToString());
    //                            continue;
    //                        }
    //                        //Debug.Log(".......... Start CopyTexture   prefabType:" + prefabType + " : " + startY + "x" + startX + " Size=" + addSize);
    //                        //.... I was able to find a work around using the Graphics API 
    //                        //Graphics.CopyTexture(texture, 0, 0, (int)startX, (int)startY, addSize, addSize, texturePrefab, 0, 0, 0, 0);

    //                        //Graphics.CopyTexture(texturePrefab, 0, 0, (int)startX, (int)startY, addSize, addSize, texture, 0, 0, 0, 0);
    //                        Graphics.CopyTexture(texturePrefab, 0, 0, 0, 0, addSize, addSize, texture, 0, 0, (int)startX, (int)startY);

    //                        //Graphics.CopyTexture(tileTexture, 0, 0, 0, 0, tileTexture.width, tileTexture.height, TileMap.textures[level], 0, 0, x, y);
    //                        //texture.Apply();
    //                        //--------------------------------
    //                    }
    //                    catch (Exception exT)
    //                    {
    //                        Debug.Log("###### CreateTextureMap.ManagerPalette: CopyTexture " + indErr + "   prefabType:" + prefabType + " ERROR: " + exT);
    //                        continue;
    //                    }

    //                    //texture.DrawPixeles(startX, startY, addSize, sizeDraw, colorCell);
    //                    //_________________
    //                    //for (int x2 = startX; x2 < startX + addSize; x2++)
    //                    //{
    //                    //    indErr = "20";
    //                    //    for (int y2 = startY; y2 < startY + addSize; y2++)
    //                    //    {
    //                    //        indErr = "21";
    //                    //        //texture.SetPixel(x2, y2, colorCell);
    //                    //        texture.SetPixel(x2, sizeDraw - y2, colorCell);
    //                    //    }
    //                    //}
    //                    //_________________
    //                }
    //                //else
    //                //{
    //                indErr = "22";
    //                //---- Draw Person
    //                //foreach (Color colorPerson in colorsPersons)
    //                for (int indColor = 0; indColor < colorsPersons.Count(); indColor++)
    //                {
    //                    Color colorPerson = colorsPersons[indColor];

    //                    indErr = "23";
    //                    int startX2 = x * scaleCell;
    //                    int startY2 = y * scaleCell;
    //                    int column = 5;
    //                    int row = 3;
    //                    indErr = "24";
    //                    for (int x2 = startX2 + addSize - column; x2 < startX2 + addSize; x2++)
    //                    {
    //                        for (int y2 = startY2 + (indColor * row); y2 < startY2 + (indColor * row) + 2; y2++)
    //                        {
    //                            indErr = "25";
    //                            //texture.SetPixel(x2, y2, colorCell);
    //                            texture.SetPixel(x2, sizeDraw - y2, colorPerson);
    //                        }
    //                    }
    //                }
    //                //}
    //            }
    //        }
    //    }catch(Exception x)
    //    {
    //        Debug.Log("############# CreateTextureMap: " + indErr + "  " + x.Message);
    //        return;
    //    }

    //    texture.Apply();

    //    Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //    prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    //}

    //IEnumerator CreateTextureMap(int scaleCell = 1, bool isRefresh = false)
    //{
    //    string indErr = "start";
    //    int sizeMap = Helper.HeightLevel;
    //    int sizeDraw = Helper.HeightLevel * scaleCell;
    //    int addSize = scaleCell - 1;

    //    if (!isRefresh)
    //        CreateFrameMap();

    //    List<Color> colorsPersons = new List<Color>();
    //    List<SaveLoadData.TypePrefabs> listPersonsTypes = new List<SaveLoadData.TypePrefabs>();
    //    List<Texture2D> listPersonsPrefabTexture = new List<Texture2D>();
    //    List<Texture2D> listPersonsMapTexture = new List<Texture2D>();

    //    bool isPerson = false;
    //    Texture2D texture = new Texture2D(sizeDraw, sizeDraw);

    //    //yield return null;

    //    bool isLoaded = true;

    //    while (isLoaded)
    //    {

    //        //try
    //        //{
    //        indErr = "1";

    //        for (int y = 0; y < sizeMap; y++)
    //        {
    //            indErr = "2";
    //            for (int x = 0; x < sizeMap; x++)
    //            {
    //                yield return null;

    //                indErr = "3";
    //                isPerson = false;
    //                indErr = "4";
    //                colorsPersons.Clear();
    //                indErr = "5";
    //                string nameField = Helper.GetNameField(x, y);
    //                indErr = "6";
    //                SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
    //                indErr = "7";
    //                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                {
    //                    DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, prefabType);
    //                    continue;
    //                }
    //                indErr = "8";
    //                Color colorCell = Color.clear;
    //                indErr = "9";
    //                foreach (ModelNPC.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //                {
    //                    indErr = "10";
    //                    //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                    if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                    datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                    {
    //                        indErr = "11";
    //                        prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                    }
    //                    else
    //                    {
    //                        indErr = "12";
    //                        ModelNPC.GameDataBoss bossObj = datObjItem as ModelNPC.GameDataBoss;
    //                        if (bossObj != null)
    //                        {
    //                            //Storage //bossObj.Level;
    //                            colorCell = TypeBoss.TypesBoss.Find(p => p.Level == bossObj.Level).ColorTrack;
    //                            if (colorCell == null)
    //                            {
    //                                Debug.Log("############# colorCell Point Map Person is null");
    //                                continue;
    //                            }
    //                            indErr = "13";
    //                            colorsPersons.Add(colorCell);

    //                            //prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                            indErr = "";
    //                            prefabType = SaveLoadData.TypePrefabs.PrefabBoss;

    //                            ////+++DRAW PERSON ---------------------------------
    //                            //    //Texture2D personTexture = Storage.GridData.GetSpriteBoss(bossObj.Level).texture;
    //                            //    //listPersonsPrefabTexture.Add(personTexture);
    //                            ////>>>> Texture2D personMapTexture_True = Storage.GridData.GetTextuteMapBoss(bossObj.Level);
    //                            //Texture2D personMapTexture = TypeBoss.Instance.GetNameTextureMapForIndexLevel(bossObj.Level);
    //                            //listPersonsMapTexture.Add(personMapTexture);
    //                            //-----------------------------------------------------

    //                            isPerson = true;
    //                        }
    //                    }
    //                }
    //                indErr = "14";
    //                ////----- DRAW
    //                if (!isPerson)
    //                {
    //                    DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, prefabType);
    //                }
    //                //-----------------
    //                //else
    //                //{
    //                indErr = "22";
    //                //---- Draw Person

    //                //+++DRAW PERSON
    //                //for (int indMap2D = 0; indMap2D < listPersonsMapTexture.Count(); indMap2D++)
    //                //{
    //                //    Texture2D texturePerson = listPersonsMapTexture[indMap2D];
    //                //    DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, texturePerson);
    //                //}

    //                //foreach (Color colorPerson in colorsPersons)
    //                //----- Color Person
    //                for (int indColor = 0; indColor < colorsPersons.Count(); indColor++)
    //                {
    //                    Color colorPerson = colorsPersons[indColor];

    //                    indErr = "23";
    //                    int startX2 = x * scaleCell;
    //                    int startY2 = y * scaleCell;
    //                    int column = 5;
    //                    int row = 3;
    //                    indErr = "24";
    //                    for (int x2 = startX2 + addSize - column; x2 < startX2 + addSize; x2++)
    //                    {
    //                        for (int y2 = startY2 + (indColor * row); y2 < startY2 + (indColor * row) + 2; y2++)
    //                        {
    //                            indErr = "25";
    //                            //texture.SetPixel(x2, y2, colorCell);
    //                            texture.SetPixel(x2, sizeDraw - y2, colorPerson);
    //                        }
    //                    }
    //                }
    //                //------------------------
    //                //}
    //            }
    //        }
    //        isLoaded = true;
    //    }
    //    //}
    //    //catch (Exception x)
    //    //{
    //    //    Debug.Log("############# CreateTextureMap: " + indErr + "  " + x.Message);
    //    //    yield break;
    //    //}

    //    //yield return null;

    //    texture = DrawLocationHero(texture);

    //    texture.Apply();

    //    Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //    prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    //    BoxCollider2D colliderMap = prefabFrameMap.GetComponent<BoxCollider2D>();
    //    if (colliderMap != null)
    //    {
    //        prefabFrameMap.GetComponent<BoxCollider2D>().size = new Vector3(texture.width / 100, texture.height / 100, 0);
    //    }

    //    yield break;
    //}

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

        Storage.Events.ListLogAdd = "Loaded map.." + DateTime.Now.ToLongTimeString();

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

        textureMap = new Texture2D(sizeDraw, sizeDraw);


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
                    if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                    {
                        DrawTextureTo(scaleCell, indErr, addSize, textureMap, y, x, prefabType);
                        continue;
                    }
                    indErr = "9";
                    foreach (ModelNPC.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
                    {
                        indErr = "10";
                        //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
                        if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                        datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
                        {
                            indErr = "11";
                            prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
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

        spriteMap = Sprite.Create(textureMap, new Rect(0.0f, 0.0f, textureMap.width, textureMap.height), new Vector2(0.5f, 0.5f), 100.0f);

        prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMap;
        BoxCollider2D colliderMap = prefabFrameMap.GetComponent<BoxCollider2D>();
        if (colliderMap != null)
        {
            prefabFrameMap.GetComponent<BoxCollider2D>().size = new Vector3(textureMap.width / Helper.WidthLevel, textureMap.height / Helper.HeightLevel, 0);
        }
    }

    public Sprite GetSpriteMap(int scaleCell = 1, bool isRefresh = false, int offsetMapX =0, int offsetMapY =0)
    {
        string indErr = "start";
        //int sizeMap = Helper.HeightLevel;
        //int sizeDraw = Helper.HeightLevel * scaleCell;
        //int sizeCellImage = 25;
        //int CellsInSectror = 25;

        //int sizeMap = sizeCellImage * CellsInSectror;
        int sizeMap = 25;
        int sizeDraw = sizeMap * scaleCell;

        int addSize = scaleCell - 1;

        offsetMapX *= sizeMap;
        offsetMapY *= sizeMap;

        //List<Color> colorsPersons = new List<Color>();
        //List<SaveLoadData.TypePrefabs> listPersonsTypes = new List<SaveLoadData.TypePrefabs>();
        //List<Texture2D> listPersonsPrefabTexture = new List<Texture2D>();
        //List<Texture2D> listPersonsMapTexture = new List<Texture2D>();

        //if (!isRefresh)
        //    CreateFrameMap();

        Storage.Events.ListLogAdd = "Loaded map.." + DateTime.Now.ToLongTimeString();

        //bool isPerson = false;
        //Texture2D texture = new Texture2D(sizeDraw, sizeDraw);
        textureMap = new Texture2D(sizeDraw, sizeDraw);

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
                    if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                    {
                        DrawTextureTo(scaleCell, indErr, addSize, textureMap, y, x, prefabType);
                        continue;
                    }
                    indErr = "9";
                    foreach (ModelNPC.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
                    {
                        indErr = "10";
                        //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
                        if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                        datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
                        {
                            indErr = "11";
                            prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
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


        //-----------------
        //textureMap = DrawLocationHero(textureMap);
        //saveHeroPosField


        //textureMap = DrawLocationHero(textureMap);

        textureMap.Apply();

        spriteMap = Sprite.Create(textureMap, new Rect(0.0f, 0.0f, textureMap.width, textureMap.height), new Vector2(0.5f, 0.5f), 100.0f);


        return spriteMap;
    }
   

    private string saveHeroPosField = "";
    public void DrawLocationHero(bool isOpenChange = false)
    {
        if (!isOpenChange)
            return;

        if (IsOpen || isOpenChange)
        {
            Texture2D textureMap = prefabFrameMap.GetComponent<SpriteRenderer>().sprite.texture;
            Texture2D textureResult = textureMap;
            //Texture2D textureResult = new Texture2D(textureMap.width, textureMap.height);
            //Graphics.CopyTexture(textureMap, 0, 0, 0, 0, textureMap.width, textureMap.height, textureResult, 0, 0, 0, 0);

            //-- Restore ---
            if (Storage.Instance.SelectFieldPosHero != saveHeroPosField)
            {
                if (!string.IsNullOrEmpty(saveHeroPosField))
                {
                    SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
                    string nameField = saveHeroPosField;
                    if (Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                    {
                        foreach (ModelNPC.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
                        {
                            if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                            datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
                            {
                                prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
                            }
                        }
                    }
                    Vector2 posF = Helper.GetPositByField(nameField);
                    int x = (int)posF.x;
                    int y = (int)posF.y;
                    DrawTextureTo(SizeCellMap, "Restore", SizeCellMap - 1, textureResult, y, x, prefabType);
                }
                saveHeroPosField = Storage.Instance.SelectFieldPosHero;
            }
            //------

            Vector2 posHero = Helper.GetPositByField(Storage.Instance.SelectFieldPosHero);
            int heroX = (int)posHero.x;
            int heroY = (int)posHero.y;

            DrawTextureTo(SizeCellMap, "Hero", SizeCellMap - 1, textureResult, heroY, heroX, Storage.Map.textureHero);

            textureResult.Apply();
            Sprite spriteMe = Sprite.Create(textureResult, new Rect(0.0f, 0.0f, textureResult.width, textureResult.height), new Vector2(0.5f, 0.5f), 100.0f);
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
            if (!string.IsNullOrEmpty(saveHeroPosField))
            {
                SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
                string nameField = saveHeroPosField;
                if (Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                {
                    foreach (ModelNPC.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
                    {
                        if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                        datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
                        {
                            prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
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

        Vector2 posHero = Helper.GetPositByField(Storage.Instance.SelectFieldPosHero);
        int heroX = (int)posHero.x;
        int heroY = (int)posHero.y;

        DrawTextureTo(SizeCellMap, "Hero", SizeCellMap - 1, textureResult, heroY, heroX, Storage.Map.textureHero);
        

        return textureResult;
    }

    public Texture2D GetTextureHero(Texture2D textureResult)
    {
        Vector2 posHero = Helper.GetPositByField(Storage.Instance.SelectFieldPosHero);
        int heroX = (int)posHero.x;
        int heroY = (int)posHero.y;
        DrawTextureTo(SizeCellMap, "Hero", SizeCellMap - 1, textureResult, heroY, heroX, Storage.Map.textureHero);
        return textureResult;
    }



    public void DrawMapCell(int y, int x, SaveLoadData.TypePrefabs prefabType)
    {
        Texture2D textureMap = prefabFrameMap.GetComponent<SpriteRenderer>().sprite.texture;
        Texture2D textureResult = textureMap;

        DrawTextureTo(SizeCellMap, "?", SizeCellMap - 1, textureResult, y, x, prefabType);

        textureResult.Apply();
        Sprite spriteMe = Sprite.Create(textureResult, new Rect(0.0f, 0.0f, textureResult.width, textureResult.height), new Vector2(0.5f, 0.5f), 100.0f);
        prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    }

    public void DrawMapCell(int y, int x, Texture2D texturePrefab)
    {
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
        }
        catch (Exception ex)
        {
            Debug.Log("######### DrawMapCell [" + texturePrefab.name + "] :" + ex.Message );
            return;
        }

        textureResult.Apply();
        Sprite spriteMe = Sprite.Create(textureResult, new Rect(0.0f, 0.0f, textureResult.width, textureResult.height), new Vector2(0.5f, 0.5f), 100.0f);
        prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;

    }

    public void DrawTextureTo(int scaleCell, string indErr, int addSize, Texture2D texture, int y, int x, SaveLoadData.TypePrefabs prefabType)
    {
        Texture2D texturePrefab = GetPrefabTexture(prefabType);
        if (texturePrefab == null)
        {
            Debug.Log("###### CreateTextureMap.ManagerPalette: " + indErr + "   prefabType:" + prefabType + " texturePrefab Is NULL ");
            //continue;
        }
       
        int startX1 = x * scaleCell;
        int startY1 = y * scaleCell;

        // Correct .............
        startY1 = texture.height - startY1 - addSize;
        //.................

        if (texture.format.ToString() != texturePrefab.format.ToString())
        {
            Debug.Log(".......... Start CopyTexture   prefabType:" + prefabType + " : " + startX1 + "x" + startY1 + " Size=" + addSize);
            Debug.Log(".......... Start CopyTexture   Formats source:" + texture.format.ToString());
            Debug.Log(".......... Start CopyTexture " + prefabType + "  Formats texturePrefab:" + texturePrefab.format.ToString());
            return;
        }

        Graphics.CopyTexture(texturePrefab, 0, 0, 0, 0, addSize, addSize, texture, 0, 0, (int)startX1, (int)startY1);
    }

    private void DrawTextureTo(int scaleCell, string indErr, int addSize, Texture2D texture, int y, int x, Texture2D texturePrefab)
    {
        int startX1 = x * scaleCell;
        int startY1 = y * scaleCell;

        // Correct .............
        startY1 = texture.height - startY1 - addSize;
        //.................

        Graphics.CopyTexture(texturePrefab, 0, 0, 0, 0, addSize, addSize, texture, 0, 0, (int)startX1, (int)startY1);
    }


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

        if (Storage.Palette == null || Storage.Palette.TexturesPrefabs == null)
        {
            Debug.Log("############# GetPrefabTexture  Palette Or TexturesPrefabs is Empty !!!");
            return null;
        }

        if (!Storage.Palette.TexturesMaps.ContainsKey(strTypePref))
        {
            Debug.Log("############# GetPrefabTexture   TexturesPrefabs not found type: " + typePredab.ToString());
            return null;
        }
        

        Texture2D textureRes = Storage.Palette.TexturesMaps[strTypePref];
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

        Storage.Events.ListLogAdd = "-------------------------------- SizeWorldOffSet: "  + Helper.SizeWorldOffSet;
        Storage.Events.ListLogAdd = "SelectPointField: " + Storage.Map.SelectPointField;
        Storage.Events.ListLogAdd = "MarkerMapWorldCell: " + MarkerMapWorldCell.transform.position;
    }

    

    public void LoadGrid()
    {
        if (m_isGridMapLoaded)
            return;
        m_isGridMapLoaded = true;

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

        string field = Helper.GetNameField(1, 1);
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
                var cellMap = (GameObject)Instantiate(PrefabSpriteCellMap);
                //cellMap.transform.SetParent(this.gameObject.transform);
                cellMap.transform.SetParent(m_GridMap.transform);
                //Sprite spriteTile = Storage.TilesManager.CollectionSpriteTiles[item.name];
                //Sprite spriteTile = cellMap.GetComponent<SpriteRenderer>().sprite;

                //field = Helper.GetNameField(x, y);
                field = Helper.GetNameField(y, x); //FIX

                cellMap.name = "MapGridCell" + field;
                cellMap.tag = "MapGridCell";

                //cellMap.GetComponent<SpriteRenderer>().sprite = spriteTile;
                //cellMap.GetComponent<CellMapControl>().DataTileCell = new DataTile() { Name = item.name, X = index, Tag= TypesStructure.Prefab.ToString() };
                //cellMap.GetComponent<CellMapControl>().DataTileCell = new DataTile() { Name = item.name, X = index, Tag = typeTilePrefab.ToString() };
                cellMap.SetActive(true);

                m_listCellsGridMap.Add(cellMap);
                index++;
            }
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

    public void AddUpdatingField(string field)
    {
        string fieldSectror = field;

        if (!StackSectorsUpdating.Contains(fieldSectror))
            StackSectorsUpdating.Push(fieldSectror);
    }

    public void RefreshGrid()
    {
        while(StackSectorsUpdating.Count>0)
        {
            string field = StackSectorsUpdating.Pop();
            RefreshGrid(field);
        }
    }

    public void RefreshGrid(string field)
    {
        GameObject cellMap = m_listCellsGridMap.Find(p => p.name == "MapGridCell" + field);
        if (cellMap == null)
        {
            Debug.Log("###### RefreafGrid cellMap==null");
            return;
        }
        CellGridMapController cellMapController = cellMap.GetComponent<CellGridMapController>();
        if (cellMapController == null)
        {
            Debug.Log("###### RefreafGrid cellMapController == null");
            return;
        }
        cellMapController.Refresh();
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

        }

    }

    #endregion

}



