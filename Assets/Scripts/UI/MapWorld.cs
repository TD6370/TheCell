using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapWorld : MonoBehaviour {

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

    private void Awake()
    {
        MapObjects = new Dictionary<string, GameObject>();
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
            isNewCreate = true;

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

        Debug.Log("---------- Change ----E:" + hero.enabled + " S:" + hero.Rb2D.IsSleeping() + " / C:"  + hero.MainCamera.enabled + "   [" + DateTime.Now);
        Storage.Events.ListLogAdd = "---------------------- Change ---- ";
        Storage.Events.ListLogAdd = "enabled = " + hero.enabled;
        Storage.Events.ListLogAdd = "Collider= " + hero.GetComponent<CapsuleCollider2D>().enabled;
        Storage.Events.ListLogAdd = "Rb2D.IsSleeping = " + hero.Rb2D.IsSleeping();
        Storage.Events.ListLogAdd = "MainCamera.enabled = " + hero.MainCamera.enabled;
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
            prefabFrameMap.GetComponent<BoxCollider2D>().size = new Vector3(textureMap.width / 100, textureMap.height / 100, 0);
        }
    }


    //public void CreateTextureMap(int scaleCell = 1, bool isRefresh = false)
    //{
    //    string indErr = "start";
    //    int sizeMap = Helper.HeightLevel;
    //    int sizeDraw = Helper.HeightLevel * scaleCell;
    //    int addSize = scaleCell - 1;



    //    //List<Color> colorsPersons = new List<Color>();
    //    //List<SaveLoadData.TypePrefabs> listPersonsTypes = new List<SaveLoadData.TypePrefabs>();
    //    //List<Texture2D> listPersonsPrefabTexture = new List<Texture2D>();
    //    //List<Texture2D> listPersonsMapTexture = new List<Texture2D>();

    //    if (!isRefresh)
    //        CreateFrameMap();

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
    //    }
    //    catch (Exception x)
    //    {
    //        Debug.Log("############# CreateTextureMap: " + indErr + "  " + x.Message);
    //        return;
    //    }

    //    texture = DrawLocationHero(texture);

    //    texture.Apply();

    //    Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //    prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    //    BoxCollider2D colliderMap = prefabFrameMap.GetComponent<BoxCollider2D>();
    //    if (colliderMap != null)
    //    {
    //        //colliderMap.size = new Vector3(texture.width, texture.height, 0);
    //        prefabFrameMap.GetComponent<BoxCollider2D>().size = new Vector3(texture.width / 100, texture.height / 100, 0);
    //    }
    //    //transform
    //}

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

    //public void UpdateMarker(Vector3 posMarker)
    //{
        
    //}
    public void UpdateMarkerPointCell()
    {
        Vector2 pos = Storage.Map.SelectPointField;
        pos.y *= -1;
        MarkerMapWorldCell.transform.position = pos * Storage.ScaleWorld;
        //Storage.Map.SelectPointField;
    }

    //public void ShowBorderBrush()
    //{

    //    Vector2 pos = Storage.Map.SelectPointField;
    //    pos.y *= -1;
    //    var position = pos * Storage.ScaleWorld;

    //    float size = Storage.PaletteMap.SizeBrush;
    //    float sizeX = position.x + size;
    //    float sizeY = position.y + size;


    //    if (Storage.DrawGeom != null)
    //        Storage.DrawGeom.DrawRect(position.x, position.y, sizeX, sizeY);
    //}

    //public void CreateTextureMap()
    //{

    //    CreateFrameMap();

    //    Texture2D texture = new Texture2D(100, 100);
    //    for (int y = 0; y < Helper.HeightLevel; y++)
    //    {
    //        for (int x = 0; x < Helper.WidthLevel; x++)
    //        {
    //            string nameField = Helper.GetNameField(x, y);
    //            SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
    //            if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                continue;

    //            foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //            {
    //                //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                {
    //                    prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                }
    //            }
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabField)
    //                continue;

    //            if (!ManagerPalette.PaletteColors.ContainsKey(prefabType.ToString()))
    //            {
    //                Debug.Log("#### Not Color in PaletteColors for type : " + prefabType.ToString());
    //            }
    //            Color colorCell = ManagerPalette.PaletteColors[prefabType.ToString()];
    //            texture.SetPixel(x, y, colorCell);
    //        }
    //    }
    //    texture.Apply();

    //    Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //    prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    //}

    //public void CreateTextureMap()
    //{
    //    //CreateTextureMapStep();
    //    //return;

    //    CreateFrameMap();


    //    Texture2D texture = new Texture2D(100, 100);
    //    //GetComponent<Renderer>().material.mainTexture = texture;
    //    //prefabFrameMap.GetComponent<Sprite>().texture = texture;

    //    //Sprite spriteMe = Storage.GridData.GetSpriteBoss(Level);
    //    //Texture2D[] _texturesBoss = Resources.LoadAll<Texture2D>(pathSprites + nameStrite);
    //    //Texture2D _texture = _texturesBoss[0];


    //    //if (spriteMe == null)
    //    //{
    //    //    //string _nameSprite = Storage.GridData.GetNameSpriteForIndexLevel(Level);
    //    //    Debug.Log("######## CreateTextureMap spriteMe == null ");
    //    //    return;
    //    //}

    //    //    for (int y = 0; y < texture.height; y++)
    //    //{
    //    //    for (int x = 0; x < texture.width; x++)
    //    //    {
    //    for (int y = 0; y < Helper.HeightLevel; y++)
    //    {
    //        for (int x = 0; x < Helper.WidthLevel; x++)
    //        {
    //            //---------
    //            //Color color = ((x & y) != 0 ? Color.white : Color.gray);
    //            //texture.SetPixel(x, y, color);
    //            //continue;
    //            //---------
    //            string nameField = Helper.GetNameField(x, y);
    //            SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
    //            if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                continue;

    //            foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //            {
    //                //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                {
    //                    prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                    //Debug.Log("Create Prefab : " + prefabName);
    //                }
    //            }
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabField)
    //                continue;

    //            if (!ManagerPalette.PaletteColors.ContainsKey(prefabType.ToString()))
    //            {
    //                Debug.Log("#### Not Color in PaletteColors for type : " + prefabType.ToString());
    //            }

    //            Color colorCell = ManagerPalette.PaletteColors[prefabType.ToString()];
    //            //if (prefabType == SaveLoadData.TypePrefabs.PrefabRock)
    //            //{
    //            //    colorCell = Color.blue;
    //            //}
    //            //if (prefabType == SaveLoadData.TypePrefabs.PrefabVood)
    //            //{
    //            //    colorCell = Color.green;
    //            //}

    //            texture.SetPixel(x, y, colorCell);
    //            //color = ((x & y) != 0 ? Color.blue : Color.green);
    //            //texture.SetPixel(x, y, color);
    //            //------------




    //        }
    //    }
    //    texture.Apply();

    //    Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //    prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    //}

    //public void CreateTextureMapStep()
    //{
    //    CreateFrameMap();
    //    CreateMapIndexColor();

    //    Texture2D texture = new Texture2D(100, 100);

    //    for (int y = 0; y < Helper.HeightLevel; y++)
    //    {
    //        for (int x = 0; x < Helper.WidthLevel; x++)
    //        {

    //            if (string.IsNullOrEmpty(colorMap[x, y]))
    //                continue;

    //            string prefabType = colorMap[x, y];

    //            //if (!ManagerPalette.PaletteColors.ContainsKey(prefabType))
    //            //{
    //            //    Debug.Log("#### Not Color in PaletteColors for type : " + prefabType);
    //            //}

    //            //Color colorCell = ManagerPalette.PaletteColors[prefabType];
    //            Color colorCell = Color.yellow;
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabRock.ToString())
    //            {
    //                colorCell = Color.blue;
    //            }
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabVood.ToString())
    //            {
    //                colorCell = Color.green;
    //            }

    //            texture.SetPixel(x, y, colorCell);
    //        }
    //    }
    //    texture.Apply();

    //    Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //    prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    //}

    //private void CreateMapIndexColor()
    //{

    //    //int[,] array2Da = new int[4, 2];
    //    int maxY = 0;
    //    int maxX = 0;

    //    for (int y = 0; y < Helper.HeightWorld; y++)
    //    {
    //        for (int x = 0; x < Helper.WidthWorld; x++)
    //        {
    //            //---------

    //            //continue;
    //            //---------
    //            string nameField = Helper.GetNameField(x, y);
    //            SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
    //            if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                continue;

    //            foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //            {
    //                //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                {
    //                    prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                    //Debug.Log("Create Prefab : " + prefabName);
    //                }
    //            }
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabField)
    //                continue;

    //            //if (!ManagerPalette.PaletteColors.ContainsKey(prefabType.ToString()))
    //            //{
    //            //    Debug.Log("#### Not Color in PaletteColors for type : " + prefabType.ToString());
    //            //}

    //            //Color colorCell = ManagerPalette.PaletteColors[prefabType.ToString()];
    //            //if (prefabType == SaveLoadData.TypePrefabs.PrefabRock)
    //            //{
    //            //    colorCell = Color.blue;
    //            //}
    //            //if (prefabType == SaveLoadData.TypePrefabs.PrefabVood)
    //            //{
    //            //    colorCell = Color.green;
    //            //}
    //            if (x > maxX)
    //                maxX = x;
    //            if (y > maxY)
    //                maxY = y;

    //            colorMap[x,y]= prefabType.ToString();
    //            //------------
    //        }
    //    }
    //    Debug.Log("---------- LIMIT X Y = " + maxX + "x" + maxY);
    //}

    //private void CreateRenderSprite()
    //{
    //    //SpriteRenderer[] loadedRenderers = GetComponentsInChildren<SpriteRenderer>(true);

    //    //Texture2D atlas = Resources.Load(activeSkin, typeof(Texture2D)) as Texture2D;

    //    //for (int i = 0; i < loadedRenderers.Length; i++)
    //    //{
    //    //    try
    //    //    {
    //    //        // replace the current sprite with the desired sprite, but using the loaded sprite as a cut out reference via 'rect'
    //    //        loadedRenderers[i].sprite = Sprite.Create(atlas, loadedRenderers[i].sprite.rect, new Vector2(0.5f, 0.5f));

    //    //        // update name, main texture and shader, these all seem to be required... even thou you'd think it already has a shader :|
    //    //        loadedRenderers[i].sprite.name = loadedRenderers[i].name + "_sprite";
    //    //        loadedRenderers[i].material.mainTexture = atlas as Texture;
    //    //        loadedRenderers[i].material.shader = Shader.Find("Sprites/Default");
    //    //    }
    //    //    catch (Exception e) { }
    //    //}
    //}

    //public void CreatePrefabsMap()
    //{
    //    string indErr = "start";
    //    int index = 0;
    //    float scaleMap = 0.2f;

    //        ClearWorld();

    //    try
    //    {
    //        indErr = "1.";
    //        MapObjects = new Dictionary<string, GameObject>();

    //        int schet = 0;

    //        GameObject newFieldBackground = BildMapObject(SaveLoadData.TypePrefabs.PrefabField);
    //        newFieldBackground.transform.position = new Vector3(10, -10, -2);
    //        newFieldBackground.transform.localScale = new Vector3(100F, 100f, 0);
    //        //newFieldBackground.GetComponent<PositionRenderSorting>()
    //        //newField.GetComponent<Sprite>().
    //        //rectTransform.sizeDelta = new Vector2(width, height);
    //        newFieldBackground.tag = "MapObject";
    //        newFieldBackground.SetActive(true);

    //        indErr = "2.";
    //        for (int y = 0; y < Helper.HeightWorld ; y++)
    //        {
    //            for (int x = 0; x < Helper.WidthWorld; x++)
    //            {
    //                //indErr = "3.";
    //                //int intRndCount = UnityEngine.Random.Range(0, 3);

    //                //indErr = "4.";
    //                //int maxObjectInField = (intRndCount == 0) ? 1 : 0;
    //                indErr = "5.";
    //                string nameField = Helper.GetNameField(x, y);

    //                indErr = "6.";
    //                List<GameObject> ListNewObjects = new List<GameObject>();
    //                //for (int i = 0; i < maxObjectInField; i++)
    //                //{
    //                indErr = "7.";
    //                int _y = y * (-1);
    //                Vector3 pos = new Vector3(x, _y, 0) * scaleMap;
    //                pos.z = -10;
    //                indErr = "8.";
    //                SaveLoadData.TypePrefabs prefabName = SaveLoadData.TypePrefabs.PrefabField;
    //                indErr = "9.";

    //                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                    continue;

    //                //Storage.Instance.GridDataG.FieldsD[nameField]
    //                if (Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                {

    //                    foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //                    {
    //                        //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                        indErr = "10.";
    //                        //if (!datObjItem.TagObject.IsPerson())
    //                        if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                        datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                        {
    //                            indErr = "11.";
    //                            prefabName = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                            //Debug.Log("Create Prefab : " + prefabName);
    //                        }
    //                    }
    //                }
    //                indErr = "12.";
    //                //Debug.Log("Bild : " + prefabName);
    //                GameObject newField = BildMapObject(prefabName);
    //                newField.transform.position = pos;
    //                newField.tag = "MapObject";
    //                newField.SetActive(true);
    //                //newField.name = nameField;
    //                indErr = "13.";
    //                newField.name = "MapPoint_" + nameField + "_" + prefabName.ToString() + index;
    //                indErr = "14.";
    //                //Debug.Log("MapObjects : " + newField.name);
    //                MapObjects.Add(nameField, newField);
    //                indErr = "15.";
    //                index++;
    //               // }
    //            }
    //            indErr = "next";
    //            schet++;
    //            if(schet>10)
    //            {
    //                schet = 0;
    //                Debug.Log("Next y=" + y);
    //            }

    //        }
    //        indErr = "end";

    //    }
    //    catch (Exception x)
    //    {
    //        Debug.Log("############# MapWorld.Create[" + indErr + "] : " + x.Message);
    //    }
    //    indErr = "ok";
    //    Debug.Log("Map Worl is loaded ))");
    //}

    //private void ClearWorld()
    //{
    //    try
    //    {

    //        if (MapObjects == null)
    //            return;

    //        foreach (var mapObjItem in MapObjects.Values)
    //        {
    //            Destroy(mapObjItem);
    //        }
    //        MapObjects.Clear();
    //    }catch(Exception x)
    //    {
    //        Debug.Log("############# MapWorld.ClearWorld : " + x.Message);
    //    }
    //}

    //private GameObject BildMapObject(SaveLoadData.TypePrefabs prefabName)
    //{
    //    GameObject newField = null;
    //    //switch (prefabName)
    //    //{
    //    //    case SaveLoadData.TypePrefabs.PrefabField:
    //    //        newField = (GameObject)Instantiate(prefabField, new Vector3(0, 0, -1), Quaternion.identity);
    //    //        break;
    //    //    case SaveLoadData.TypePrefabs.PrefabRock:
    //    //        newField = (GameObject)Instantiate(prefabRock, new Vector3(0, 0, -1), Quaternion.identity);
    //    //        break;
    //    //    case SaveLoadData.TypePrefabs.PrefabVood:
    //    //        newField = (GameObject)Instantiate(prefabVood, new Vector3(0, 0, -1), Quaternion.identity);
    //    //        break;
    //    //}

    //    newField = (GameObject)Instantiate(prefabMapCell, new Vector3(10, 10, -10), Quaternion.identity);
    //    SpriteRenderer sprtRend = newField.GetComponent<SpriteRenderer>();
    //    switch (prefabName)
    //    {
    //        case SaveLoadData.TypePrefabs.PrefabField:
    //            sprtRend.color = "#8ACA84".ToColor();
    //            break;
    //        case SaveLoadData.TypePrefabs.PrefabRock:
    //            sprtRend.color = "#77A7C2".ToColor();
    //            break;
    //        case SaveLoadData.TypePrefabs.PrefabVood:
    //            sprtRend.color = "#379200".ToColor();
    //            break;
    //    }
    //    return newField;
    //}
}


