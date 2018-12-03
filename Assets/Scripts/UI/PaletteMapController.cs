//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaletteMapController : MonoBehaviour {

    public GameObject FramePaletteMap;
    public Button btnClose;
    public Dropdown ListConstructsControl;
    public string SelectedConstruction { get; set; }
    public DataTile SelectedCell { get; set; }
    public GameObject PrefabCellMapPalette;

    //public Button btnOnPaint;
    public Toggle btnOnPaint;
    public Toggle btnPaste;
    public Toggle btnBrush;
    public Toggle btnClear;
    public Toggle btnCursor;
    public Toggle btnOnLayer;
    public Toggle btnTeleport;
    public Toggle checkStepGen;
    public Button btnReloadWorld;
    public Button btnRefreshMap;
    public Button btnDestroyWorld;
    public Button btnUp;
    public Button btnDown;
    public InputField btxSizeBrush;
    public InputField btxIntGenOption1;
    public InputField btxIntGenOption2;
    public InputField btxIntGenOption3;
    public InputField btxIntGenOption4;
    public InputField btxIntGenOption5;

    public GameObject ToolBarBrushPalette;

    private List<Toggle> m_listToggleMode;

    public ToolBarPaletteMapAction ModePaint = ToolBarPaletteMapAction.Paste;
    public TypesBrushGrid SelectedTypeBrush = TypesBrushGrid.Prefabs;

    public bool IsPaintsOn = false;
    //public bool IsCursorOn = false;
    private bool m_PasteOnLayer = false;

    private List<Dropdown.OptionData> m_ListConstructsOptopnsData;
    private List<GameObject> m_listCallsOnPalette;
    private List<string> m_ListNamesConstructs;
    private List<TypesBrushGrid> ListTypesBrushes;

    public int sizeCellMap = 20;
    public float ActionRate = 0.5f;
    private float DelayTimer = 0F;
    private bool isPaletteBrushOn = false;
    
    private GridLayoutGroup m_GridMap;

    private GameObject lastSelectCellPalette;
    private GameObject lastBorderCellPalette;

    public GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    public Canvas CanvasUI;

    public float DelayTimerPaletteUse = 0f;
    public float ActionRatePaletteUse = 0.1f;

    public enum TypesBrushGrid
    {
        Prefabs,
        Brushes,
        PaintBrush,
        OptionsGeneric
    }


    private int m_SizeBrush = 4;
    public int SizeBrush
    {
        get {
            return m_SizeBrush;
        }
        set {
            m_SizeBrush = value;
            CorrectOptionGenCount();
            btxSizeBrush.text = m_SizeBrush.ToString();
        }
    }

    //Count 
    private int m_OptionGenCount1 = 1;
    public int OptionGenCount {
        get {
            return m_OptionGenCount1;
        }
        set {
            m_OptionGenCount1 = value;
            btxIntGenOption1.text = m_OptionGenCount1.ToString();
        }
    }
    private void CorrectOptionGenCount()
    {
        OptionGenCount = (m_SizeBrush * m_SizeBrush) * m_OptionGenPercent2 / 100;
    }


    //Percent
    private int m_OptionGenPercent2 = 50;
    
    public int OptionGenPercent {
        get {
            if (btxIntGenOption2.text != m_OptionGenPercent2.ToString())
                btxIntGenOption2.text = m_OptionGenPercent2.ToString();
            return m_OptionGenPercent2;
        }
        set {
            m_OptionGenPercent2 = value;
            CorrectOptionGenCount();
            btxIntGenOption2.text = m_OptionGenPercent2.ToString();
        }
    }


    //Segments
    private int m_OptionGenSegments3 = 5;
    public int OptionGenSegments
    {
        get {
            if (btxIntGenOption3.text != m_OptionGenSegments3.ToString())
                btxIntGenOption3.text = m_OptionGenSegments3.ToString();
            return m_OptionGenSegments3;
        }
        set {
            m_OptionGenSegments3 = value;
            btxIntGenOption3.text = m_OptionGenSegments3.ToString();
        }
    }

    //Level
    private int m_OptionGenLevel4 = 10;
    public int OptionGenLevel {
        get {
            if (btxIntGenOption4.text != m_OptionGenLevel4.ToString())
                btxIntGenOption4.text = m_OptionGenLevel4.ToString();
            return m_OptionGenLevel4;
        }
        set { m_OptionGenLevel4 = value;
            btxIntGenOption4.text = m_OptionGenLevel4.ToString();
        }
    }

    private int m_OptionGen5 = 1;
    public int OptionGen5
    {
        get
        {
            if (btxIntGenOption5.text != m_OptionGen5.ToString())
                btxIntGenOption5.text = m_OptionGen5.ToString();
            return m_OptionGen5;
        }
        set
        {
            m_OptionGen5 = value;
            btxIntGenOption5.text = m_OptionGen5.ToString();
        }
    }

    int LayerUI;
    int LayerViewUI;
    int LayerObjects;

    


    private void Awake()
    {
        LayerUI = LayerMask.NameToLayer("LayerUI");
        LayerViewUI = LayerMask.NameToLayer("UI");
        LayerObjects = LayerMask.NameToLayer("LayerObjects");

        m_GridMap = this.gameObject.GetComponent<GridLayoutGroup>();
        m_listToggleMode = new List<Toggle>()
        {
             btnPaste,
             btnClear,
             btnTeleport,
             btnBrush
        };

        ListTypesBrushes = new List<TypesBrushGrid>
        {
            TypesBrushGrid.Prefabs,
            TypesBrushGrid.Brushes,
            TypesBrushGrid.PaintBrush,
            TypesBrushGrid.OptionsGeneric
        };

    }


    // Use this for initialization
    void Start()
    {
        //ResizeScaleGrid();

        InitEventsButtonMenu();

        m_listCallsOnPalette = new List<GameObject>();

        m_ListNamesConstructs = new List<string>();

     
        //--------- After Update
        StartCoroutine(LoadMap());

        OptionGenPercent = 100;


        if(CanvasUI==null)
        {
            Debug.Log("###### PletteMapController Start CanvasUI is empty");
            return;
        }

        //-------------- Raycast
        //Fetch the Raycaster from the GameObject (the Canvas)
        //m_Raycaster = CanvasUI.GetComponent<GraphicRaycaster>();
        //m_Raycaster = GetComponent<GraphicRaycaster>();
        m_Raycaster = FramePaletteMap.GetComponent<GraphicRaycaster>();
        if (m_Raycaster == null)
        {
            Debug.Log("###### Raycaster is empty");
        }
        //Fetch the Event System from the Scene
        //m_EventSystem = CanvasUI.GetComponent<EventSystem>();
        //m_EventSystem = GetComponent<EventSystem>();
        m_EventSystem = FramePaletteMap.GetComponent<EventSystem>();
        if (m_EventSystem == null)
        {
            Debug.Log("###### EventSystem is empty");
        }

        m_PointerEventData = new PointerEventData(m_EventSystem);
        if (m_PointerEventData == null)
        {
            Debug.Log("####### EventsUI Palette PointerEventData is empty");
        }

    }

    private void LateUpdate()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        

        //IsPaintsOn
        EventsUI();
    }

    private void FixedUpdate()
    {
        if (IsPaintsOn)
        {
            if (Input.GetMouseButtonDown(2))
            {
                DefaultModeOn();
            }

            if (!Storage.Map.IsOpen)
            {
                switch (ModePaint)
                {
                    case ToolBarPaletteMapAction.Paste:
                    case ToolBarPaletteMapAction.Brush:
                    case ToolBarPaletteMapAction.Clear:
                        ShowBorderBrush();
                        break;
                }
            }
        }
    }

    private void EventsUI()
    {
        if (!IsPaintsOn)
            return;

        //Check if the left Mouse button is clicked
        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //Set up the new Pointer Event

        //if (m_Raycaster == null || m_EventSystem == null)
        //{
        //    return;
        //}

        //m_PointerEventData = new PointerEventData(m_EventSystem);
        //if (m_PointerEventData == null)
        //{
        //    Debug.Log("####### EventsUI Palette PointerEventData==null");
        //    return;
        //}

        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;


        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        //foreach (RaycastResult result in results)
        //{
        //    //Debug.Log(">>>>>>>>>> Hit Palette " + result.gameObject.name);
        //}

        if (results.Count > 0)
        {
            OnMauseWheel();
            DelayTimerPaletteUse = Time.time + ActionRatePaletteUse;
        }
        else if(!Storage.Map.IsOpen)
            OnMauseWheel();

        //}

    }


    private void OnMauseWheel()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel != 0) // back
        {
            //Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 1);
            if (wheel > 0)
            {
                SizeBrush++;
            }
            else
            {
                SizeBrush--;
                if (SizeBrush < 1)
                    SizeBrush =1;
            }
        }
    }

    public void ShowBorderBrush()
    {

        //Storage.Events.ListLogAdd = Storage.Instance.SelectFieldCursor;
        //Storage.Events.ListLogAdd = Helper.GetNameFieldPosit(Storage.PlayerController.PosCursorToField.x, Storage.PlayerController.PosCursorToField.y);
        string filed = Helper.GetNameFieldPosit(Storage.PlayerController.PosCursorToField.x, Storage.PlayerController.PosCursorToField.y);
        Vector2 position = Helper.GetPositByField(filed);
        position *= Storage.ScaleWorld;

        position.x -= 1f;
        position.y -= 1f;

        float size = SizeBrush * Storage.ScaleWorld;
        float sizeX = position.x + size;
        float sizeY = position.y + size;

        position.y *= -1;
        sizeY *= -1;

        if (Storage.DrawGeom != null)
            Storage.DrawGeom.DrawRect(position.x, position.y , sizeX, sizeY, Color.blue, 0.05f);
    }


    IEnumerator LoadMap()
    {
        yield return new WaitForSeconds(0.3f);

        if (isPaletteBrushOn)
        {
            LoadListBrushTypesControl();
            yield return null;
            PrefabsOnPalette();
            //LoadTypeBrushOnPalette(TypesBrushGrid.Brushes);
        }
        else
        {
            LoadListConstructsControl();
        }
    }

    //private void LoadPalette()
    //{
    //    StartCoroutine(LoadMap());
    //}

    public void PaintAction()
    {
        bool isValidRefreshMap = true;

        //ToolBarPaletteMapAction ModePaint
        switch (ModePaint)
        {
            case ToolBarPaletteMapAction.Paste:
                Paste();
                break;
            case ToolBarPaletteMapAction.Clear:
                Clear();
                break;
            case ToolBarPaletteMapAction.Teleport:
                break;
            case ToolBarPaletteMapAction.Transfer:
                TeleportHero();
                break;
            case ToolBarPaletteMapAction.Brush:
                //if (SelectedTypeBrush == TypesBrushGrid.Prefabs) {
                //    Paste();
                //}
                //else {
                //    BrushCells();
                //}
                BrushCells();
                break;
            case ToolBarPaletteMapAction.Cursor:
            case ToolBarPaletteMapAction.None:
                isValidRefreshMap = false;
                break;
        }
        if(Storage.Map.IsOpen && isValidRefreshMap)
            Storage.Map.Refresh();
    }


    void DropdownValueChanged(Dropdown dpntStructurs)
    {
        //LoadConstructOnPalette(dpntStructurs.captionText.ToString());
        //LoadConstructOnPalette(dpntStructurs.value);
        //LoadConstructOnPalette(dpntStructurs.itemText.ToString() + " .2");
        //string selectStructure = 
        if(dpntStructurs.value > m_ListNamesConstructs.Count-1)
        {
            Debug.Log("######## DropdownValueChanged NOT dpntStructurs.value[" + dpntStructurs.value  + "] > m_ListNmeStructurs.Length[" + m_ListNamesConstructs.Count + "]");
            return;
        }
        if (isPaletteBrushOn)
        {
            LoadTypeBrushOnPalette(ListTypesBrushes[dpntStructurs.value]);
        }
        else
        {
            LoadConstructOnPalette(m_ListNamesConstructs[dpntStructurs.value]);
        }
    }

    public void Show(bool isClose = false)
    {
        if (!isClose)
        {
            if (!FramePaletteMap.activeSelf && m_ListNamesConstructs.Count == 0)
                LoadListConstructsControl();
            FramePaletteMap.SetActive(!FramePaletteMap.activeSelf);
        }
        else
        {
            FramePaletteMap.SetActive(false);
        }
        Storage.DrawGeom.DrawClear();
        //ContentGridPaletteMap

        //----------
        //IsPaintsOn = btnOnPaint.isOn;
        IsPaintsOn = FramePaletteMap.activeSelf;
        if (IsPaintsOn)
        {
            ModePaint = ToolBarPaletteMapAction.Cursor;
            Storage.PlayerController.CursorSelectionOn(true);
            DefaultModeOn();
        }

        
        //Debug.Log("_________IgnoreLayerCollision: " + LayerUI + " > " + LayerObjects);
        Physics.IgnoreLayerCollision(LayerViewUI, LayerObjects, IsPaintsOn);
        Physics.IgnoreLayerCollision(LayerViewUI, LayerUI, IsPaintsOn);
    }

    private void DefaultModeOn()
    {
        ModePaint = ToolBarPaletteMapAction.Cursor;
        btnCursor.isOn = true;
        btnPaste.isOn = false;
        btnClear.isOn = false;
        btnTeleport.isOn = false;
        btnBrush.isOn = false;
        Storage.DrawGeom.DrawClear();
    }

    private bool UncheckModeTool(Toggle toggleOn, ToolBarPaletteMapAction actionMode)
    {
        if (toggleOn.isOn)
        {
            ModePaint = actionMode;
            foreach (var toggleItem in m_listToggleMode)
            {
                if (!toggleItem.name.Equals(toggleOn.name))
                    toggleItem.isOn = false;
            }
        }
        else if (ModePaint == actionMode)
        {
            ModePaint = ToolBarPaletteMapAction.None;
        }
        Storage.DrawGeom.DrawClear();
        

        return toggleOn.isOn;
    }

    private void InitEventsButtonMenuOtions()
    {
        btnCursor.onValueChanged.AddListener(delegate
        {
            Storage.PlayerController.CursorSelectionOn(btnCursor.isOn);
            UncheckModeTool(btnCursor, ToolBarPaletteMapAction.Cursor);

        });

        btnPaste.onValueChanged.AddListener(delegate
        {
            UncheckModeTool(btnPaste, ToolBarPaletteMapAction.Paste);

        });
        btnClear.onValueChanged.AddListener(delegate
        {
            UncheckModeTool(btnClear, ToolBarPaletteMapAction.Clear);

        });

        btnBrush.onValueChanged.AddListener(delegate
        {
            if(UncheckModeTool(btnBrush, ToolBarPaletteMapAction.Brush))
            {
                //ToolBarBrushPalette.SetActive(true);
            }

        });

        btnTeleport.onValueChanged.AddListener(delegate
        {
            UncheckModeTool(btnTeleport, ToolBarPaletteMapAction.Teleport);

        });

    }

    private void InitEventsButtonMenu()
    {
        ListConstructsControl.onValueChanged.AddListener(delegate {
            DropdownValueChanged(ListConstructsControl);
        });


        btnClose.onClick.AddListener(delegate
        {
            Show(false);
        });
        btnOnPaint.onValueChanged.AddListener(delegate
        {
            isPaletteBrushOn = !isPaletteBrushOn;
            StartCoroutine(LoadMap());
        });

        InitEventsButtonMenuOtions();

        btnOnLayer.onValueChanged.AddListener(delegate
        {
            if (btnOnLayer.isOn)
            {
                //ModePaint = ToolBarPaletteMapAction.Clear;
            }
            m_PasteOnLayer = btnOnLayer.isOn;
        });

        //--- Generic New World ---
        btnReloadWorld.onClick.AddListener(delegate
        {
            //---------------------------
            //Storage.Events.ReloadWorld();
            //---------------------------
            Storage.Instance.CreateWorld(true);
            Storage.Map.RefreshFull();
        });

        //--- Clear full World ---
        btnDestroyWorld.onClick.AddListener(delegate
        {
            Storage.GridData.ClearWorld();
            Storage.Map.RefreshFull();
        });

        //--- Refresh map ---
        btnRefreshMap.onClick.AddListener(delegate
        {
            Storage.Map.RefreshFull();
            Storage.Map.Create(true);
        });


        btnUp.onClick.AddListener(delegate
        {
            SizeBrush+=3;
            //btxSizeBrush.text = SizeBrush.ToString();

        });
        btnDown.onClick.AddListener(delegate
        {
            SizeBrush-=3;
        });

        


        btxSizeBrush.text = SizeBrush.ToString();
        btxSizeBrush.onValueChange.AddListener(delegate
        {
            int _sizeBrush = SizeBrush;
            if(!int.TryParse(btxSizeBrush.text, out _sizeBrush))
            {
                btxSizeBrush.text = SizeBrush.ToString();
            }
            else
            {
                SizeBrush = _sizeBrush;
            }
            //SizeBrush = int.Parse(btxSizeBrush.text);
        });

        btxIntGenOption1.onValueChange.AddListener(delegate
        {
            OptionGenCount = IntPrseDef(btxIntGenOption1.text, OptionGenCount);
            if (btxIntGenOption1.text != OptionGenCount.ToString())
                btxIntGenOption1.text = OptionGenCount.ToString();
            //OptionGenCount = int.Parse(btxIntGenOption1.text);
        });
        btxIntGenOption2.onValueChange.AddListener(delegate
        {
            //OptionGenPercent = int.Parse(btxIntGenOption2.text);
            OptionGenPercent = IntPrseDef(btxIntGenOption2.text, OptionGenPercent);
            if (btxIntGenOption2.text != OptionGenPercent.ToString())
                btxIntGenOption2.text = OptionGenPercent.ToString();
        });
        btxIntGenOption3.onValueChange.AddListener(delegate
        {
            //OptionGenSegments
            //OptionGen3 = int.Parse(btxIntGenOption3.text);
            OptionGenSegments = IntPrseDef(btxIntGenOption3.text, OptionGenSegments);
            if (btxIntGenOption3.text != OptionGenSegments.ToString())
                btxIntGenOption3.text = OptionGenSegments.ToString();
        });
        btxIntGenOption4.onValueChange.AddListener(delegate
        {
            //OptionGen4 = int.Parse(btxIntGenOption4.text);
            OptionGenLevel = IntPrseDef(btxIntGenOption4.text, OptionGenLevel);
            if (btxIntGenOption4.text != OptionGenLevel.ToString())
                btxIntGenOption4.text = OptionGenLevel.ToString();

        });
        btxIntGenOption5.onValueChange.AddListener(delegate
        {
            //OptionGenLevel = int.Parse(btxIntGenOption4.text);
            OptionGen5 = IntPrseDef(btxIntGenOption5.text, OptionGenLevel);
            if (btxIntGenOption5.text != OptionGen5.ToString())
                btxIntGenOption5.text = OptionGen5.ToString();

        });
    }

    public int IntPrseDef(string value, int defValue)
    {
        int resValue = defValue;
        if (!int.TryParse(value, out resValue))
        {
            //btxSizeBrush.text = SizeBrush.ToString();
        }

        return resValue;
    }
    
    public void PrefabsOnPalette()
    {
       List<Texture2D> listTextures = Storage.TilesManager.ListTexturs.Where(p => p.name.IndexOf("Prefab") != -1).ToList();

        //int countColumnMap = listTextures[0].width;
        int countColumnMap = 6;
        SizeBrush = 1;
        //m_GridMap.startCorner = GridLayoutGroup.Corner.UpperLeft;
        //m_GridMap.startCorner = GridLayoutGroup.Corner.UpperLeft;
        m_GridMap.childAlignment = TextAnchor.UpperLeft;
        m_GridMap.constraintCount = countColumnMap;
        ResizeScaleGrid(countColumnMap,1.1f);

        GameObject[] gobjCells = GameObject.FindGameObjectsWithTag("PaletteCell");
        for (int i = 0; i < gobjCells.Length; i++)
        {
            Destroy(gobjCells[i]);
        }

        m_listCallsOnPalette.Clear();

        //m_GridMap.constraintCount = countColumnMap;

        int index = 0;
        foreach (var item in listTextures)
        {
            var cellMap = (GameObject)Instantiate(PrefabCellMapPalette);
            cellMap.transform.SetParent(this.gameObject.transform);
            Sprite spriteTile = Storage.TilesManager.CollectionSpriteTiles[item.name];

            TypesStructure typeTilePrefab = TypesStructure.Prefab;
            try
            {
                //TypePrefabs prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), namePrefab);
                if (!System.Enum.IsDefined(typeof(SaveLoadData.TypePrefabs), item.name))
                {
                    typeTilePrefab = TypesStructure.Terra;
                    Debug.Log("Not Prefab"); 
                }
            }
            catch (System.ArgumentException)
            {
                Debug.Log("############ PrefabsOnPalette ERROR PARSE typeTilePrefab");
            }
            

            cellMap.GetComponent<Image>().sprite = spriteTile;
            //cellMap.GetComponent<CellMapControl>().DataTileCell = new DataTile() { Name = item.name, X = index, Tag= TypesStructure.Prefab.ToString() };
            cellMap.GetComponent<CellMapControl>().DataTileCell = new DataTile() { Name = item.name, X = index, Tag = typeTilePrefab.ToString() };
            cellMap.SetActive(true);
            
            m_listCallsOnPalette.Add(cellMap);
            index++;
        }
    }

    private void LoadTypeBrushOnPalette(TypesBrushGrid selectTypeBrush)
    {
        ToolBarBrushPalette.SetActive(false);
        SelectedTypeBrush = selectTypeBrush;

        switch (selectTypeBrush)
        {
            case TypesBrushGrid.Prefabs:
                PrefabsOnPalette();
                break;
            case TypesBrushGrid.Brushes:
                break;
            case TypesBrushGrid.PaintBrush:
                break;
            case TypesBrushGrid.OptionsGeneric:
                ToolBarBrushPalette.SetActive(true);
                break;
        }
    }

    private void LoadConstructOnPalette(string keyStruct)
    {
        Debug.Log("Selected struct :" + keyStruct);
        SelectedConstruction = keyStruct;

        if (!Storage.TilesManager.DataMapTiles.ContainsKey(SelectedConstruction))
        {
            Debug.Log("######### LoadConstructOnPalette: TilesManager.DataMapTiles  Not find SelectedStructure: " + SelectedConstruction);
            return;
        }

        DataConstructionTiles dataTiles = Storage.TilesManager.DataMapTiles[SelectedConstruction];


        //float col = listTiles.Count;
        //int countColumnMap = (int)Mathf.Sqrt(col);
        int countColumnMap = dataTiles.Height;
        SizeBrush = countColumnMap;

        //m_GridMap.startCorner = GridLayoutGroup.Corner.LowerLeft;
        m_GridMap.childAlignment = TextAnchor.MiddleLeft;
        m_GridMap.constraintCount = countColumnMap;

        GameObject[] gobjCells = GameObject.FindGameObjectsWithTag("PaletteCell");
        for(int i=0;i< gobjCells.Length;i++)
        {
            Destroy(gobjCells[i]);
        }
        //foreach (var oldCell in m_listCallsOnPalette)
        //{
        //    if(oldCell!=null)
        //        Destroy(oldCell);
        //}
        m_listCallsOnPalette.Clear();

        ResizeScaleGrid(countColumnMap);

        if (dataTiles.ListDataTileTerra != null && dataTiles.ListDataTileTerra.Count > 0)
        {
            LoadLayerConstrOnPalette(keyStruct, dataTiles.ListDataTileTerra);
        }
        if (dataTiles.ListDataTileFloor != null && dataTiles.ListDataTileFloor.Count > 0)
        {
            LoadLayerConstrOnPalette(keyStruct, dataTiles.ListDataTileFloor);
        }
        if (dataTiles.ListDataTilePrefabs != null && dataTiles.ListDataTilePrefabs.Count > 0)
        {
            LoadLayerConstrOnPalette(keyStruct, dataTiles.ListDataTilePrefabs, countColumnMap);
        }
        if (dataTiles.ListDataTilePerson != null && dataTiles.ListDataTilePerson.Count > 0)
        {
            LoadLayerConstrOnPalette(keyStruct, dataTiles.ListDataTilePerson);
        }

    }

    private void LoadLayerConstrOnPalette(string keyStruct, List<DataTile> listTiles, int size=1)
    {
        bool isOnLayer = (m_listCallsOnPalette.Count > 0);

        int index = 0;
        foreach (DataTile itemTileData in listTiles)
        {
            string namePrefab = itemTileData.Name;
            string nameTexture = itemTileData.Name;
            bool isUpdate = (isOnLayer && m_listCallsOnPalette.Count > index);

            GameObject cellMap;
            if (isUpdate)
            {
                int newIndex = 0;
                for (int i=0; i< m_listCallsOnPalette.Count; i++)
                {
                    GameObject item = m_listCallsOnPalette[i];
                    var dataCell = item.GetComponent<CellMapControl>().DataTileCell;
                    if(dataCell.X == itemTileData.X && dataCell.Y == itemTileData.Y)
                    {
                        newIndex = i;
                        break;
                    }
                }
                cellMap = m_listCallsOnPalette[newIndex];
            }
            else
            {
                //test
                //itemTileData.Name += " " + index;
                cellMap = (GameObject)Instantiate(PrefabCellMapPalette);
                cellMap.transform.SetParent(this.gameObject.transform);
            }
            Sprite spriteTile = Storage.TilesManager.CollectionSpriteTiles[nameTexture];

            cellMap.GetComponent<Image>().sprite = spriteTile;
            cellMap.GetComponent<CellMapControl>().DataTileCell = itemTileData;
            cellMap.SetActive(true);
            
            if(isUpdate)
                m_listCallsOnPalette[index] = cellMap;
            else
                m_listCallsOnPalette.Add(cellMap);

            index++;
        }
    }

    private void TeleportHero()
    {
        int posTransferHeroX = (int)(Storage.Map.SelectPointField.x * Storage.ScaleWorld);
        int posTransferHeroY = (int)(Storage.Map.SelectPointField.y * Storage.ScaleWorld);
        posTransferHeroY *= -1;
        Storage.Player.TeleportHero(posTransferHeroX, posTransferHeroY);
    }


    private void ResizeScaleGrid(int column, float ratio = 0.9f)
    {
        float size = this.gameObject.GetComponent<RectTransform>().rect.width;
        
        float sizeCorr = (size / column) * ratio;
        Vector2 newSize = new Vector2(sizeCorr, sizeCorr);
        m_GridMap.cellSize = newSize;
    }

    private void ResizeScaleGrid()
    {
        float width;
        width = this.gameObject.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / sizeCellMap, width / sizeCellMap);
        
        m_GridMap.cellSize = newSize;
    }

    private void UpdateTiles()
    {
        //Storage.TilesManager.CreateDataTiles();
        Storage.TilesManager.UpdateGridTiles();
    }

   

    public void LoadListBrushTypesControl()
    {
        m_ListConstructsOptopnsData = new List<Dropdown.OptionData>();

       
        foreach (var item in ListTypesBrushes)
        {
            m_ListConstructsOptopnsData.Add(new Dropdown.OptionData() { text = item.ToString() });
        }

        ListConstructsControl.ClearOptions();
        ListConstructsControl.AddOptions(m_ListConstructsOptopnsData);
    }

    public void LoadListConstructsControl()
    {
        if (ListConstructsControl == null)
        {
            Debug.Log("###### LoadListConstructsContro  ListConstructsControl is Empty");
            return;
        }
        if(Storage.TilesManager == null)
        {
            Debug.Log("###### LoadListConstructsContro  TilesManager is Empty");
            return;
        }

        Storage.TilesManager.LoadTextures();// LoadGridTiles();

        if (Storage.TilesManager.DataMapTiles == null)
        {
            Debug.Log("###### LoadListConstructsContro  DataMapTales is Empty");
            return;
        }

        m_ListNamesConstructs.Clear();
        m_ListConstructsOptopnsData = new List<Dropdown.OptionData>();
        
        foreach (var itemTileData in Storage.TilesManager.DataMapTiles)
        {
            m_ListNamesConstructs.Add(itemTileData.Key);
            m_ListConstructsOptopnsData.Add(new Dropdown.OptionData() { text = itemTileData.Key });
        }

        ListConstructsControl.ClearOptions();
        ListConstructsControl.AddOptions(m_ListConstructsOptopnsData);
    }

    public void SelectedCellMap(DataTile DataTileCell, GameObject selCellPalette, GameObject borderCellPalette)
    {
        SelectedCell = DataTileCell;

        if (lastSelectCellPalette != null)
            lastSelectCellPalette.GetComponent<Image>().color = Color.white;

        if (lastBorderCellPalette != null)
            lastBorderCellPalette.SetActive(false);

        selCellPalette.GetComponent<Image>().color = "#FFD600".ToColor();

        borderCellPalette.SetActive(true);

        lastBorderCellPalette = borderCellPalette;
        lastSelectCellPalette = selCellPalette;
    }
    
    private void Clear()
    {
        string fieldStart = Storage.Instance.SelectFieldCursor;
        Vector2 posFieldClear = Helper.GetPositByField(fieldStart);
        string fieldNew = "";
        int sizeClear = SizeBrush;
        int sizeClearX = (int)posFieldClear.x + sizeClear;
        int sizeClearY = (int)posFieldClear.y + sizeClear;

        bool isClearDataGrid = Storage.Map.IsOpen;

        for (int x = (int)posFieldClear.x; x <  sizeClearX; x++)
        {
            for (int y = (int)posFieldClear.y; y < sizeClearY; y++)
            {
                fieldNew = Helper.GetNameField(x, y);
                ClearLayerForStructure(fieldNew, isClearDataGrid);
            }
        }
    }

    public void GenericOnWorld(SaveLoadData.TypePrefabs prefab)
    {
        DataTile genPrefab;
        if (prefab== SaveLoadData.TypePrefabs.PrefabField)
        {
            genPrefab = new DataTile()
            {
                Name = prefab.ToString(),
                Tag = TypesStructure.Terra.ToString()
            };
        }
        else
        {
            genPrefab = new DataTile()
            {
                Name = prefab.ToString(),
                Tag = TypesStructure.Prefab.ToString()
            };
        }

        //TypesStructure structType = (TypesStructure)Enum.Parse(typeof(TypesStructure), itemTile.Tag); ;
        //if (structType == TypesStructure.Terra)
        //{
        //    prefabName = TypePrefabs.PrefabField;
        //}
        //if (structType == TypesStructure.Person || structType == TypesStructure.Prefab)
        //{
        //    prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), itemTile.Name);
        //}
        int size = Helper.WidthLevel;
        //OptionGenCount = (m_SizeBrush * m_SizeBrush) * m_OptionGenPercent2 / 100;
        OptionGenCount = (size * size) * m_OptionGenPercent2 / 100;
        BrushCells(true, genPrefab);
    }

    private void BrushCells(bool isOnFullMap = false, DataTile sel = null)
    {
        if(sel==null)
            sel = SelectedCell;

        if (sel == null)
        {
            Debug.Log("######## BrushCells SelectedCell is empty");
            return;
        }

        //public float ActionRate = 0.5f;
        //private float DelayTimer = 0F;
        //if (Time.time < DelayTimer)
        //    return;
        //DelayTimer = Time.time + ActionRate;

        string fieldStart = Storage.Instance.SelectFieldCursor;
        if (isOnFullMap)
            fieldStart = Helper.GetNameField(0, 0);

        Vector2 posStructFieldStart = Helper.GetPositByField(fieldStart);
        Vector2 posStructFieldNew = Helper.GetPositByField(fieldStart);

        bool isClearLayer = !m_PasteOnLayer;

        if (isOnFullMap)
            isClearLayer = false;

        //if (isClearLayer)
        //    ClearLayerForStructure(fieldStart);


            //Storage.GridData.AddConstructInGridData(fieldStart, sel, isClearLayer);

            //---------- Brush zone
        int size = SizeBrush;
        if (isOnFullMap)
            size = Helper.WidthLevel;

        int sizeX = (int)posStructFieldNew.x + size;
        int sizeY = (int)posStructFieldNew.y + size;

        if (SelectedTypeBrush != TypesBrushGrid.OptionsGeneric && !isOnFullMap)
        { 
            for (int x = (int)posStructFieldNew.x; x < sizeX; x++)
            {
                for (int y = (int)posStructFieldNew.y; y < sizeY; y++)
                {
                    //posStructFieldNew = posStructFieldStart + new Vector2(sizeX, sizeY);
                    string fieldNew = Helper.GetNameField(x, y);
                    if (isClearLayer)
                        ClearLayerForStructure(fieldNew);

                    Storage.GridData.AddConstructInGridData(fieldNew, sel, isClearLayer);
                }
            }
        }
        else
        {
            //Generic
            int CountObjects = OptionGenCount;
            int Percent = OptionGenPercent;
            int SubsystemSegments = OptionGenSegments;
            int SubsystemLevel = OptionGenLevel;
            if (SubsystemLevel == 0) SubsystemLevel = 10;
            //int CountObjects = OptionGen1;

            if (OptionGenSegments < 2)
            {
                Storage.Events.ListLogAdd = "standart generation 1.";
                //------ standart 1.
                for (int i = 0; i < CountObjects; i++)
                {
                    int x = Random.Range((int)posStructFieldNew.x, sizeX);
                    int y = Random.Range((int)posStructFieldNew.y, sizeY);
                    string fieldNew = Helper.GetNameField(x, y);
                    if (isClearLayer)
                        ClearLayerForStructure(fieldNew);

                    Storage.GridData.AddConstructInGridData(fieldNew, sel, isClearLayer);
                }
                //------
            }
            else
            //------ Segment
            {
                Storage.Events.ListLogAdd = "Segments generation.";
                bool isSteps = checkStepGen.isOn;
                //bool isSteps = true;
                //Step
                if (SubsystemSegments == 0) SubsystemSegments = 1;

                int ContAll = CountObjects / SubsystemSegments;
                int startX = (int)posStructFieldNew.x;
                int startY = (int)posStructFieldNew.y;
                int minLevel = (SubsystemLevel / 2) * (-1);
                int maxLevel = (SubsystemLevel / 2);

                for (int i = 0; i < ContAll; i++)
                {
                    startX = Random.Range((int)posStructFieldNew.x, sizeX);
                    startY = Random.Range((int)posStructFieldNew.y, sizeY);

                    for (int s = 0; s < SubsystemSegments; s++)
                    {
                        int offsetX = Random.Range(minLevel, maxLevel);
                        int offsetY = Random.Range(minLevel, maxLevel);
                        int x = 0;
                        int y = 0;
                        if (isSteps)
                        {
                            startX += offsetX;
                            startY += offsetY;
                            x = startX;
                            y = startY;
                        }
                        else
                        {
                            x = startX + offsetX;
                            y = startY + offsetY;
                        }
                        string fieldNew = Helper.GetNameField(x, y);
                        if (isClearLayer)
                            ClearLayerForStructure(fieldNew);

                        Storage.GridData.AddConstructInGridData(fieldNew, sel, isClearLayer);
                    }
                }
            }
            //------
        }
        //-------------
        //int sizeX = (int)posStructFieldNew.x + size;
        //int sizeY = (int)posStructFieldNew.y + size;
        bool isZoneStart = Helper.IsValidFieldInZona(posStructFieldNew.x, posStructFieldNew.y);
        bool isZoneEnd = Helper.IsValidFieldInZona(sizeX, sizeY);
        if(isZoneStart || isZoneEnd) //#fast
        {
            //Storage.Events.ListLogAdd = "Create construct in zona";
            Storage.GenGrid.LoadObjectsNearHero();
        }
        else
        {
            //Storage.Events.ListLogAdd = "Create construct NOT in zona";
        }
    }

    private void Paste()
    {
        if (Time.time > DelayTimer)
        {
            SaveConstructTileInGridData();
            if (Storage.Map.IsOpen)
                Storage.GenGrid.ReloadGridLook();
            else
            {
                if (PoolGameObjects.IsUsePoolObjects)
                {
                    //Storage.GenGrid.ReloadGridLook();
                    Storage.GenGrid.LoadObjectsNearHero();
                }
                else
                {
                    Storage.GenGrid.LoadObjectsNearHero();
                }
            }
            //DefaultModeOn();

            DelayTimer = Time.time + ActionRate;
        }
    }

    private void SaveConstructTileInGridData()
    {
        if (string.IsNullOrEmpty(SelectedConstruction))
        {
            Storage.Events.SetTittle = "No selected construction";
            return;
        }

        if (!Storage.TilesManager.DataMapTiles.ContainsKey(SelectedConstruction))
        {
            Storage.Events.SetTittle = "Not exist : " + SelectedConstruction;
            Debug.Log("#######  SaveConstructTileInGridData : Not exist SelectedConstruction: " + SelectedConstruction);
            return;
        }

        //string fieldStart = Storage.Instance.SelectFieldPosHero;
        
        //var listTiles = Storage.TilesManager.DataMapTiles[SelectedConstruction];
        DataConstructionTiles dataTiles = Storage.TilesManager.DataMapTiles[SelectedConstruction];

        if (dataTiles.ListDataTileTerra != null && dataTiles.ListDataTileTerra.Count > 0)
        {
            SaveLayerConstrTileInGridData(SelectedConstruction, dataTiles.ListDataTileTerra, dataTiles, TypesStructure.Terra);
        }
        if (dataTiles.ListDataTileFloor != null && dataTiles.ListDataTileFloor.Count > 0)
        {
            SaveLayerConstrTileInGridData(SelectedConstruction, dataTiles.ListDataTileFloor, dataTiles);
        }
        if (dataTiles.ListDataTilePrefabs != null && dataTiles.ListDataTilePrefabs.Count > 0)
        {
            SaveLayerConstrTileInGridData(SelectedConstruction, dataTiles.ListDataTilePrefabs, dataTiles, TypesStructure.Prefab);
        }
        if (dataTiles.ListDataTilePerson != null && dataTiles.ListDataTilePerson.Count > 0)
        {
            SaveLayerConstrTileInGridData(SelectedConstruction, dataTiles.ListDataTilePerson, dataTiles);
        }
    }

    
    private void SaveLayerConstrTileInGridData(string keyStruct, List<DataTile> listTiles, DataConstructionTiles dataTiles, TypesStructure typeCell = TypesStructure.None)
    {
        string fieldStart = Storage.Instance.SelectFieldCursor;
        Vector2 posStructFieldStart = Helper.GetPositByField(fieldStart);
        Vector2 posStructFieldNew = Helper.GetPositByField(fieldStart);

        bool isClearLayer = !m_PasteOnLayer;


        posStructFieldStart.y--; 

        //int size = (int)Mathf.Sqrt(listTiles.Count) - 1;
        int size = dataTiles.Height;
        foreach (DataTile itemTile in listTiles)
        {
            if (typeCell != TypesStructure.None)
                itemTile.Tag = typeCell.ToString();

            //Correct position
            posStructFieldNew = posStructFieldStart + new Vector2(itemTile.X, size - itemTile.Y);

            string fieldNew = Helper.GetNameField(posStructFieldNew.x, posStructFieldNew.y);

            if (isClearLayer)
                ClearLayerForStructure(fieldNew);

            Storage.GridData.AddConstructInGridData(fieldNew, itemTile, isClearLayer);
        }
    }

    public void UpOptionGen1() {
        OptionGenCount++;
    }
    public void UpOptionGen2() {
        OptionGenPercent+=10;
    }
    public void UpOptionGen3() {
        OptionGenSegments++;
    }
    public void UpOptionGen4()
    {
        OptionGenLevel++;
    }
    public void UpOptionGen5()
    {
        OptionGen5++;
    }

    public void DownOptionGen1() {
        OptionGenCount--;
    }
    public void DownOptionGen2() {
        OptionGenPercent-=10;
    }
    public void DownOptionGen3()
    {
        OptionGenSegments--;
    }
    public void DownOptionGen4()
    {
        OptionGenLevel--;
    }
    public void DownOptionGen5()
    {
        OptionGen5--;
    }

    //public void OptionGenOnChanged1()
    //{
    //    OptionGen1 = int.Parse(btxIntGenOption1.text);
    //}
    //public void OptionGenOnChanged2()
    //{
    //    OptionGenPercent = int.Parse(btxIntGenOption2.text);
    //}
    //public void OptionGenOnChanged3()
    //{
    //    OptionGen3 = int.Parse(btxIntGenOption3.text);
    //}
    //public void OptionGenOnChanged4()
    //{
    //    OptionGen4 = int.Parse(btxIntGenOption4.text);
    //}

    private void ClearLayerForStructure(string field, bool isClearData = false)
    {
        //Destroy All Objects
        if (Storage.Instance.GamesObjectsReal.ContainsKey(field))
        {
            var listObjs = Storage.Instance.GamesObjectsReal[field];

            foreach (var obj in listObjs.ToArray())
            {
                if (PoolGameObjects.IsUsePoolObjects)
                {
                    obj.DisableComponents();
                    Storage.Instance.DestroyFullObject(obj);
                }
                else
                {
                    Storage.Instance.AddDestroyGameObject(obj);
                }
            }
        }

        if (isClearData)
        {
            if (Storage.Map.IsGridMap)
                Storage.Map.CheckSector(field);

            //Destroy All DATA Objects
            if (Storage.Instance.GridDataG.FieldsD.ContainsKey(field))
            {
                Storage.Instance.GridDataG.FieldsD[field].Objects.Clear();
            }
        }
    }

    //--- fixed Bug ---
    //void Start()
    //{
    //    float width;
    //    GridLayoutGroup lg;
    //    lg = GetComponent<GridLayoutGroup>();
    //    width = gameObject.GetComponent<RectTransform>().rect.width;
    //    float sizeButt = lg.cellSize.x + lg.spacing.x;
    //    int hSize = Mathf.FloorToInt(width / sizeButt);
    //    lg.constraintCount = hSize;
    //}
}

public enum ToolBarPaletteMapAction
{
    None,
    Paste,
    Clear,
    Cursor,
    Teleport,
    Transfer,
    Brush
}
