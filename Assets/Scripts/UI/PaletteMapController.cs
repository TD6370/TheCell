//using System;
//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
//UnityEditor
//using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaletteMapController : MonoBehaviour {

    public bool IsLogGenericWorld = true;
    public bool IsGenericContruct = true;
    //public bool IsTestFilledFieldGen = true;
    //public bool IsTestExistMeTypeGen = true;

    public GameObject FramePaletteMap;
    public GameObject ToolBarBrushPalette;
    public Button btnClose;
    public Button btnCloseToolBarBrushPalette;//ToolBarBrushPalette.SetActive(false);
    public Dropdown dpdListConstructsControl;

    public string SelectedConstruction { get; set; }
    public DataTile SelectedCell { get; set; }
    //private DataTile selectGeneticObject { get; set; }
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

    bool IsSteps
    {
        get
        {
            return checkStepGen.isOn;
        }
        set
        {
            checkStepGen.isOn = value;
        }
    }

    // # Segmnt Next Point Gen
    public Toggle checkOptStartSegmentMarginLimit;
    public Toggle checkOptStartSegmentRange;
    public Toggle checkOptStartSegmentIsFirstLast;
    public Toggle checkOptRepeatFind;
    bool IsRepeatFind {
        get
        {
           return checkOptRepeatFind.isOn;
        }
        set
        {
            checkOptRepeatFind.isOn = value;
        }
    }

    public InputField tbxOptStartSegment;
    public InputField tbxOptEndSegment;

    // --- Save version generic option
    //private bool m_IsListModeIsContainerGeneticOtions = false;
    //public Toggle checkOptVersionGeneric;
    //public Toggle checkOptVersionContainerGenerics;


    public Dropdown dpnListGenOptionsVersion;
    public InputField tbxVersionNameGenOption; //ListGenOptionsVersion
    public Button btnAddVersionOptGen;
    public Button bntDeleteVersionOptGen;
    private List<Dropdown.OptionData> m_ListVersionNameGenOptionData;
    private List<GenericOptionsWorld> m_ListGenOptionsVersion;

    //Version Gen World
    public Dropdown dpnListVersionGenWorld;
    public InputField tbxVersionNameGenWorld; //ListGenOptionsVersion
    public Button btnAddVersionGenWorld;
    public Button bntDeleteVersionGenWorld;
    public Button bntAddInContainer;
    public Button bntGenVersionSaveUpdate;
    public GameObject ContentListVersionsGenericWorld;
    private List<Dropdown.OptionData> m_ListVersionNameGenWorldData;
    private List<ContainerOptionsWorld> m_ListGenWorldVersion;
    private List<GenericOptionsWorld> m_ListGenOptionsVersionForContainer;
    private GenericOptionsWorld SelectedGenericOptionsWorld;
    private ContainerOptionsWorld SelectedVersionWorld;

    // --- options Delete cell prefabs generic option
    private SelCheckOptDel m_TypeModeOptStartDelete = SelCheckOptDel.All;
    private SelCheckOptDel TypeModeOptStartDelete
    {
        get
        {
            if (m_ModeOptStartDeleteFull == ModeDelete.Clear)
                return SelCheckOptDel.DelFull;
            if (m_ModeOptStartDeleteTypePrefab == ModeDelete.Clear)
                return SelCheckOptDel.DelType;
            if (m_ModeOptStartDeletePrefab == ModeDelete.Clear)
                return SelCheckOptDel.DelPrefab;
            if (m_ModeOptStartDeleteTerra == ModeDelete.Clear)
                return SelCheckOptDel.DelTerra;
            //return SelCheckOptDel.DelType;
            return SelCheckOptDel.None;
        }
    }
    private SelCheckOptDel m_TypeModeOptStartCheck = SelCheckOptDel.All;
    private SelCheckOptDel TypeModeOptStartCheck
    {
        get
        {
            if (m_ModeOptStartDeleteFull == ModeDelete.Check)
                return SelCheckOptDel.DelFull;
            if (m_ModeOptStartDeleteTypePrefab == ModeDelete.Check)
                return SelCheckOptDel.DelType;
            if (m_ModeOptStartDeletePrefab == ModeDelete.Check)
                return SelCheckOptDel.DelPrefab;
            if (m_ModeOptStartDeleteTerra == ModeDelete.Check)
                return SelCheckOptDel.DelTerra;
            return SelCheckOptDel.None;
        }
    }
    private ModeDelete m_ModeOptStartDeleteFull = ModeDelete.Off;
    private ModeDelete m_ModeOptStartDeleteTypePrefab = ModeDelete.Off;
    private ModeDelete m_ModeOptStartDeletePrefab = ModeDelete.Off;
    private ModeDelete m_ModeOptStartDeleteTerra = ModeDelete.Off;
    public Toggle checkOptStartDeleteFull;
    public Toggle checkOptStartDeleteTypePrefab;
    public Toggle checkOptStartDeletePrefab;
    public Toggle checkOptStartDeleteTerra;

    public enum SelCheckOptDel
    {
        DelFull,
        DelType,
        DelPrefab,
        DelTerra,
        All,
        None
    }

    public enum ModeDelete
    {
        Off,
        Check,
        Clear
    }

    public enum ModeStartSegmentGen
    {
        Margin,
        Range,
        None
    }

    public ModeStartSegmentGen ModeSegmentMarginLimit
    {
        get
        {
            if (checkOptStartSegmentMarginLimit.isOn)
            {
                return ModeStartSegmentGen.Margin;
            }
            if (checkOptStartSegmentRange.isOn)
            {
                return ModeStartSegmentGen.Range;
            }
            return ModeStartSegmentGen.None;
        }
    }

    public bool IsFirstStartSegment
    {
        get
        {
            return !checkOptStartSegmentIsFirstLast.isOn;
        }
        set
        {
            checkOptStartSegmentIsFirstLast.isOn = !value;
        }
    }

    public float FirstLimitMargin = 0f;
    public float FirstLimitRange = 0f;
    public float FirstLimit
    {
        get
        {
            float resF = 0f;
            if (float.TryParse(tbxOptStartSegment.text, out resF))
            {
                if (ModeSegmentMarginLimit == ModeStartSegmentGen.Margin)
                    FirstLimitMargin = resF;
                else
                    FirstLimitRange = resF;
            }
            else
            {
                if (ModeSegmentMarginLimit == ModeStartSegmentGen.Margin)
                    resF = FirstLimitMargin;
                else
                    resF = FirstLimitRange;
            }
            return resF;
        }
        set
        {
            float resF = value;
            if (ModeSegmentMarginLimit == ModeStartSegmentGen.Margin)
                FirstLimitMargin = resF;
            else
                FirstLimitRange = resF;

        }
    }

    //IsFirstStartSegment
    //ModeSegmentMarginLimit
    //public float FirstLimitMargin = 0f;
    //public float FirstLimitRange = 0f;
    //public float FirstLimit
    //public float LastLimitMargin = 0f;
    //public float LastLimitRange = 0f;
    //public float LastLimit

    public float LastLimitMargin = 0f;
    public float LastLimitRange = 0f;
    public float LastLimit
    {
        get
        {
            float resF = 0f;
            if (float.TryParse(tbxOptEndSegment.text, out resF))
            {
                if (ModeSegmentMarginLimit == ModeStartSegmentGen.Margin)
                    LastLimitMargin = resF;
                else
                    LastLimitRange = resF;
            }
            else
            {
                if (ModeSegmentMarginLimit == ModeStartSegmentGen.Margin)
                    resF = LastLimitMargin;
                else
                    resF = LastLimitRange;
            }
            return resF;
        }
        set
        {
            float resF = value;
            if (ModeSegmentMarginLimit == ModeStartSegmentGen.Margin)
                LastLimitMargin = resF;
            else
                LastLimitRange = resF;

        }
    }


    public bool IsClearLayer
    {
        get
        {
            return !m_PasteOnLayer;
        }
        set
        {
            m_PasteOnLayer = !value;
        }
    }

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
    //[NonSerialized]
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

        if (CanvasUI == null)
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

        //LoadOptionGen();

    }

    public void Init()
    {
        LoadOptionGen();
    }

    private void LateUpdate()
    {

    }

    // Update is called once per frame
    void Update()
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
        else if (!Storage.Map.IsOpen)
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
                    SizeBrush = 1;
            }
        }
    }

    private void LoadOptionGen()
    {
        // --- Save version generic option
        //public Dropdown dpdListGenOptionsVersion;
        //public InputField tbxVersionNameGenOption;
        //public Button btnAddVersionOptGen;
        //private List<Dropdown.OptionData> m_ListVersionNameGenOptionData;
        //private List<GenericOprionsWorld> m_ListGenOptionsVersion;

        OptionGenPercent = 20;

        LoadVersionOptons();
        //LoadListVersionGeneticOptons();


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
            Storage.DrawGeom.DrawRect(position.x, position.y, sizeX, sizeY, Color.blue, 0.05f);
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
            //LoadListVersionGeneticOptons();
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
        if (Storage.Map.IsOpen && isValidRefreshMap)
            Storage.Map.Refresh();
    }



    void ConstructsValueChanged(Dropdown dpntStructurs)
    {
        //LoadConstructOnPalette(dpntStructurs.captionText.ToString());
        //LoadConstructOnPalette(dpntStructurs.value);
        //LoadConstructOnPalette(dpntStructurs.itemText.ToString() + " .2");
        //string selectStructure = 
        if (dpntStructurs.value > m_ListNamesConstructs.Count - 1)
        {
            Debug.Log("######## DropdownValueChanged NOT dpntStructurs.value[" + dpntStructurs.value + "] > m_ListNmeStructurs.Length[" + m_ListNamesConstructs.Count + "]");
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
            if (UncheckModeTool(btnBrush, ToolBarPaletteMapAction.Brush))
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
        dpdListConstructsControl.onValueChanged.AddListener(delegate {
            ConstructsValueChanged(dpdListConstructsControl);
        });

        dpnListGenOptionsVersion.onValueChanged.AddListener(delegate {
            VersionGenOptionValueChanged(dpnListGenOptionsVersion);
        });

        dpnListVersionGenWorld.onValueChanged.AddListener(delegate {
            VersionGenWorldValueChanged(dpnListVersionGenWorld);
        });



        btnClose.onClick.AddListener(delegate
        {
            Show(false);
        });
        btnCloseToolBarBrushPalette.onClick.AddListener(delegate
        {
            ToolBarBrushPalette.SetActive(!ToolBarBrushPalette.activeSelf); 
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

        //public Toggle checkOptStartSegmentMarginLimit;
        //public Toggle checkOptStartSegmentRange;
        //public Toggle checkOptStartSegmentIsFirstLast;
        //------------
        //public float FirstLimitMargin = 0f;
        //public float FirstLimitRange = 0f;
        //public float FirstLimit
        //public float LastLimitMargin = 0f;
        //public float LastLimitRange = 0f;
        //public float LastLimit
        //public InputField tbxOptStartSegment;
        //public InputField tbxOptEndSegment;
        //---------------

        checkOptStartSegmentMarginLimit.onValueChanged.AddListener(delegate
        {
            if (checkOptStartSegmentMarginLimit.isOn)
            {
                checkOptStartSegmentRange.isOn = false;

                tbxOptStartSegment.text = FirstLimitMargin.ToString();
                tbxOptEndSegment.text = LastLimitMargin.ToString();
            }
        });
        checkOptStartSegmentRange.onValueChanged.AddListener(delegate
        {
            if (checkOptStartSegmentRange.isOn)
            {
                checkOptStartSegmentMarginLimit.isOn = false;

                tbxOptStartSegment.text = FirstLimitRange.ToString();
                tbxOptEndSegment.text = LastLimitRange.ToString();
            }
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
            SizeBrush += 3;
            //btxSizeBrush.text = SizeBrush.ToString();

        });
        btnDown.onClick.AddListener(delegate
        {
            SizeBrush -= 3;
        });




        btxSizeBrush.text = SizeBrush.ToString();
        btxSizeBrush.onValueChange.AddListener(delegate
        {
            int _sizeBrush = SizeBrush;
            if (!int.TryParse(btxSizeBrush.text, out _sizeBrush))
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

        //public InputField tbxOptStartSegment;
        //public InputField tbxOptEndSegment;
        tbxOptStartSegment.onValueChange.AddListener(delegate
        {
            //OptionGenLevel = int.Parse(btxIntGenOption4.text);
            float resF = 0f;
            if (float.TryParse(tbxOptStartSegment.text, out resF))
            {
                FirstLimit = resF;
            }
        });
        tbxOptEndSegment.onValueChange.AddListener(delegate
        {
            //OptionGenLevel = int.Parse(btxIntGenOption4.text);
            float resF = 0f;
            if (float.TryParse(tbxOptEndSegment.text, out resF))
            {
                LastLimit = resF;
            }
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
        //#fix terra
        //List<Texture2D> listTextures = Storage.TilesManager.ListTexturs.Where(p => p.name.IndexOf("Prefab") != -1).ToList();
        //foreach (var item in listTextures)
        //{
        //    Debug.Log("TileName :: " + item.name);
        //}
        //foreach (Sprite item in Storage.Palette.SpritesPrefabs.Values)
        //{
        //    Debug.Log("SpriteName :: " + item.name); 
        //}

        //int countColumnMap = listTextures[0].width;
        int countColumnMap = 6;
        SizeBrush = 1;
        //m_GridMap.startCorner = GridLayoutGroup.Corner.UpperLeft;
        //m_GridMap.startCorner = GridLayoutGroup.Corner.UpperLeft;
        m_GridMap.childAlignment = TextAnchor.UpperLeft;
        m_GridMap.constraintCount = countColumnMap;
        ResizeScaleGrid(countColumnMap, 1.1f);

        GameObject[] gobjCells = GameObject.FindGameObjectsWithTag("PaletteCell");
        for (int i = 0; i < gobjCells.Length; i++)
        {
            Destroy(gobjCells[i]);
        }

        m_listCallsOnPalette.Clear();

        int index = 0;

        //foreach (var item in listTextures)
        //@FiX terra
        foreach (Sprite item in Storage.Palette.SpritesPrefabs.Values)
        {
            //item.name = item.name.ClearClone();
            //if (item.name.IndexOf("Wall") != -1)
            //{
            //    item.name = "Prefab" + item.name;
            //}

            string spriteName = item.name;
            

            var cellMap = (GameObject)Instantiate(PrefabCellMapPalette);
            cellMap.transform.SetParent(this.gameObject.transform);

            //@FiX terra
            //----------------
            //Sprite spriteTile = Storage.TilesManager.CollectionSpriteTiles[item.name];
            //----------------
            TypesStructure typeTilePrefab = TypesStructure.Prefab;
            try
            {
                //TypePrefabs prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), namePrefab);
                if (!System.Enum.IsDefined(typeof(SaveLoadData.TypePrefabs), spriteName))
                {
                    typeTilePrefab = TypesStructure.Terra;
                    Debug.Log("Not Prefab");
                }
            }
            catch (System.ArgumentException)
            {
                Debug.Log("############ PrefabsOnPalette ERROR PARSE typeTilePrefab");
            }


            cellMap.GetComponent<Image>().sprite = item;
            //cellMap.GetComponent<CellMapControl>().DataTileCell = new DataTile() { Name = item.name, X = index, Tag= TypesStructure.Prefab.ToString() };
            cellMap.GetComponent<CellMapControl>().DataTileCell = new DataTile() { Name = spriteName, X = index, Tag = typeTilePrefab.ToString() };
            cellMap.SetActive(true);

            m_listCallsOnPalette.Add(cellMap);
            index++;
        }
    }

    public void CreateCellPalette(Sprite spriteTile)
    {
        var cellMap = (GameObject)Instantiate(PrefabCellMapPalette);
        cellMap.transform.SetParent(this.gameObject.transform);
        cellMap.GetComponent<Image>().sprite = spriteTile;
        cellMap.SetActive(true);
    }

    public void CreateCellPalette2(Sprite spriteTile)
    {
        //Rect rectTexture = spriteTile.textureRect;
        //Vector2 textureRectOffset = spriteTile.textureRectOffset;

        Texture2D textureAtlas; // = spriteTile.texture;
        string nameS = "";
        //Packer.GetAtlasDataForSprite(spriteTile, out nameS, out textureAtlas);
        UnityEngine.U2D.SpriteAtlas atlas = Storage.Map.SpriteAtlasMapPrefab;
        textureAtlas = spriteTile.texture;
        //textureAtlas = SpriteUtility.GetSpriteTexture(spriteTile, false /* getAtlasData */);//#UnityEditor
        //--------------
        //textureAtlas = new Texture2D((int)spriteTile.rect.width, (int)spriteTile.rect.height);
        //var pixels = spriteTile.texture.GetPixels((int)spriteTile.textureRect.x,
        //                                        (int)spriteTile.textureRect.y,
        //                                        (int)spriteTile.textureRect.width,
        //                                        (int)spriteTile.textureRect.height);
        //textureAtlas.SetPixels(pixels);
        //textureAtlas.Apply();
        //-----------------
        Sprite spriteTileRes = Sprite.Create(textureAtlas, new Rect(0.0f, 0.0f, textureAtlas.width, textureAtlas.height), new Vector2(0.5f, 0.5f), 100.0f);
        var cellMap = (GameObject)Instantiate(PrefabCellMapPalette);
        cellMap.transform.SetParent(this.gameObject.transform);
        cellMap.GetComponent<Image>().sprite = spriteTileRes;
        cellMap.SetActive(true);
    }

    public void CreateCellPalette(Texture2D textureTile)
    {
        Sprite spriteTile = Sprite.Create(textureTile, new Rect(0.0f, 0.0f, textureTile.width, textureTile.height), new Vector2(0.5f, 0.5f), 100.0f);
        var cellMap = (GameObject)Instantiate(PrefabCellMapPalette);
        cellMap.transform.SetParent(this.gameObject.transform);
        cellMap.GetComponent<Image>().sprite = spriteTile;
        cellMap.SetActive(true);
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
        for (int i = 0; i < gobjCells.Length; i++)
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

    private void LoadLayerConstrOnPalette(string keyStruct, List<DataTile> listTiles, int size = 1)
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
                for (int i = 0; i < m_listCallsOnPalette.Count; i++)
                {
                    GameObject item = m_listCallsOnPalette[i];
                    var dataCell = item.GetComponent<CellMapControl>().DataTileCell;
                    if (dataCell.X == itemTileData.X && dataCell.Y == itemTileData.Y)
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

            if (isUpdate)
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

        dpdListConstructsControl.ClearOptions();
        dpdListConstructsControl.AddOptions(m_ListConstructsOptopnsData);
    }

    public void LoadListConstructsControl()
    {
        if (dpdListConstructsControl == null)
        {
            Debug.Log("###### LoadListConstructsContro  ListConstructsControl is Empty");
            return;
        }
        if (Storage.TilesManager == null)
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

        dpdListConstructsControl.ClearOptions();
        dpdListConstructsControl.AddOptions(m_ListConstructsOptopnsData);
    }



    public void SelectedCellMap(DataTile DataTileCell, GameObject selCellPalette, GameObject borderCellPalette)
    {
        SelectedCell = DataTileCell;
        //if (selectGeneticObject == null)
        //    selectGeneticObject = DataTileCell;

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

        for (int x = (int)posFieldClear.x; x < sizeClearX; x++)
        {
            for (int y = (int)posFieldClear.y; y < sizeClearY; y++)
            {
                fieldNew = Helper.GetNameField(x, y);
                ClearLayerForStructure(fieldNew, isClearDataGrid);
            }
        }
    }

    public void GenericOnWorld(bool isSelected = false, SaveLoadData.TypePrefabs prefab = SaveLoadData.TypePrefabs.PrefabElka)
    {
        DataTile genPrefab = null;
        if (isSelected)
        {
            genPrefab = SelectedCell;//  selectGeneticObject;
            if(genPrefab==null)
                genPrefab = SelectedCell;
        }

        if (genPrefab == null)
        {
            if (prefab == SaveLoadData.TypePrefabs.PrefabField)
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
        }

        IsGenericContruct = ModePaint == ToolBarPaletteMapAction.Paste && !isPaletteBrushOn;

        if (IsGenericContruct && string.IsNullOrEmpty(SelectedConstruction))
        {
            Storage.Events.ListLogAdd = "#### Not selected construction !!!";
            return;
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

    //private void BrushCells(bool isOnFullMap = false, DataTile selDataTile = null, bool isTestFilledField = false, bool isTestExistMeType = false)
    private void BrushCells(bool isOnFullMap = false, DataTile selDataTile = null)
    {
        bool isLog = false;

        if (selDataTile == null)
            selDataTile = SelectedCell;


        if (selDataTile == null)
        {
            Debug.Log("######## BrushCells SelectedCell is empty");
            return;
        }

        //selectGeneticObject = selDataTile;
        SelectedCell = selDataTile;
        Storage.Events.ListLogAdd = "Generic prefab: " + SelectedCell.Name + @" \ " + SelectedCell.Tag;


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

        bool _isClearLayer = IsClearLayer;//!m_PasteOnLayer;
        string info = "";



        //if (isClearLayer)
        //    ClearLayerForStructure(fieldStart);


        //Storage.GridData.AddConstructInGridData(fieldStart, sel, isClearLayer);

        //---------- Brush zone
        int size = SizeBrush;
        if (isOnFullMap)
            size = Helper.WidthLevel;

        int sizeX = (int)posStructFieldNew.x + size;
        int sizeY = (int)posStructFieldNew.y + size;


        bool isValidResult = true;
        
        int limitRepeat = 500;
        int indexRepeat = 0;

        int countCellInConstruct =1;
        if(IsGenericContruct)
        {
            DataConstructionTiles dtTiles = Storage.TilesManager.DataMapTiles[SelectedConstruction];
            countCellInConstruct = dtTiles.Height * dtTiles.Height;
        }

        if (SelectedTypeBrush != TypesBrushGrid.OptionsGeneric && !isOnFullMap)
        {
            for (int x = (int)posStructFieldNew.x; x < sizeX; x++)
            {
                for (int y = (int)posStructFieldNew.y; y < sizeY; y++)
                {
                    //posStructFieldNew = posStructFieldStart + new Vector2(sizeX, sizeY);
                    string fieldNew = Helper.GetNameField(x, y);

                    if (_isClearLayer)
                        ClearLayerForStructure(fieldNew);

                    if (!IsGenericContruct)
                    {
                        isValidResult = Storage.GridData.AddConstructInGridData(fieldNew, selDataTile, _isClearLayer);
                    }
                    else
                    {
                        Storage.Instance.SelectFieldCursor = fieldNew;
                        GenericContructInGridData();
                    }

                    if(!isValidResult && IsRepeatFind && indexRepeat<limitRepeat)
                    {
                        //#repeat
                        y--;
                        indexRepeat++;
                    }
                }
                indexRepeat = 0;
            }
        }
        else
        {
            //m_ModeOptStartDeleteFull = ModeDelete.Clear;
            //m_ModeOptStartDeleteTypePrefab = ModeDelete.Clear;
            //m_ModeOptStartDeletePrefab = ModeDelete.Clear;
            //m_ModeOptStartDeleteTerra = ModeDelete.Clear;

            //---------------------
            if (m_ModeOptStartDeleteFull == ModeDelete.Clear)
                _isClearLayer = true;
            else
                _isClearLayer = false;
            //---------------------

            //Generic
            int CountObjects = OptionGenCount / countCellInConstruct;
            if (CountObjects < 1)
                CountObjects = 1;
            int Percent = OptionGenPercent;
            int SubsystemSegments = OptionGenSegments;
            int SubsystemLevel = OptionGenLevel;
            if (SubsystemLevel == 0) SubsystemLevel = 10;
            //int CountObjects = OptionGen1;

            if (OptionGenSegments < 2)
            {
                if (isLog)
                    Storage.Events.ListLogAdd = "standart generation 1.";
                //------ standart 1.
                for (int i = 0; i < CountObjects; i++)
                {
                    int x = Random.Range((int)posStructFieldNew.x, sizeX);
                    int y = Random.Range((int)posStructFieldNew.y, sizeY);
                    string fieldNew = Helper.GetNameField(x, y);
                    if (_isClearLayer)
                        ClearLayerForStructure(fieldNew);

                    if (!IsGenericContruct) {
                        isValidResult = Storage.GridData.AddConstructInGridData(fieldNew, selDataTile, TypeModeOptStartDelete, TypeModeOptStartCheck);
                    }
                    else {
                        Storage.Instance.SelectFieldCursor = fieldNew;
                        GenericContructInGridData();
                    }

                    if (!isValidResult && IsRepeatFind && indexRepeat < limitRepeat)
                    {
                        //#repeat
                        i--;
                        indexRepeat++;
                    }
                }
                //------
            }
            else if (ModeSegmentMarginLimit == ModeStartSegmentGen.None)
            //------ Segment
            {
                if (isLog)
                    Storage.Events.ListLogAdd = "Segments generation.";
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
                        if (IsSteps)
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
                        if (_isClearLayer)
                            ClearLayerForStructure(fieldNew);

                        if (!IsGenericContruct)
                        {
                            isValidResult = Storage.GridData.AddConstructInGridData(fieldNew, selDataTile, TypeModeOptStartDelete, TypeModeOptStartCheck);
                        }
                        else
                        {
                            Storage.Instance.SelectFieldCursor = fieldNew;
                            GenericContructInGridData();
                        }

                        if (!isValidResult && IsRepeatFind && indexRepeat < limitRepeat)
                        {
                            //#repeat
                            s--;
                            indexRepeat++;
                        }
                    }
                    indexRepeat = 0;
                }
            } else if (ModeSegmentMarginLimit != ModeStartSegmentGen.None) {

                //------------- # Segmnt Next Point Gen

                if (isLog)
                    Storage.Events.ListLogAdd = "Segments next Point generation.";
                //bool isSteps = checkStepGen.isOn;
                //bool isSteps = true;
                //Step
                if (SubsystemSegments == 0) SubsystemSegments = 1;

                int ContAll = CountObjects / SubsystemSegments;

                int startX = (int)posStructFieldNew.x;
                int startY = (int)posStructFieldNew.y;
                int minLevel = (SubsystemLevel / 2) * (-1);
                int maxLevel = (SubsystemLevel / 2);

                int startSegmentX = 0;
                int startSegmentY = 0;
                List<Vector2Int> stepsPointsSart = new List<Vector2Int>();

                for (int i = 0; i < ContAll; i++)
                {
                    startX = Random.Range((int)posStructFieldNew.x, sizeX);
                    startY = Random.Range((int)posStructFieldNew.y, sizeY);

                    if (!IsSteps)
                        stepsPointsSart = new List<Vector2Int>();

                    stepsPointsSart.Add(new Vector2Int(startX, startY));

                    for (int s = 0; s < SubsystemSegments; s++)
                    {
                        if (!IsSteps)
                        {
                            startSegmentX = 0;
                            startSegmentY = 0;
                        }
                        if (s < 3)
                        {
                            startSegmentX = startX;
                            startSegmentY = startY;
                        }
                        else
                        {
                            //-------------------- Select Next start Point segment
                            if (ModeSegmentMarginLimit == ModeStartSegmentGen.Margin) //------------ number limit
                            {
                                int max = stepsPointsSart.Count - 1;

                                Vector2Int corrRange = GetValidMargin((int)FirstLimit, (int)LastLimit, max);
                                int firstCorr = corrRange.x;
                                int lastCorr = corrRange.y;

                                int SelectedPintindex = 0;
                                if (IsFirstStartSegment)
                                {
                                    SelectedPintindex = Random.Range(0, firstCorr);

                                    if (IsLogGenericWorld)
                                        info = "margin (0--" + firstCorr + ")   max = " + max + "  I=" + SelectedPintindex + "  [" + FirstLimit + @"\" + LastLimit + "]";
                                }
                                else
                                {
                                    SelectedPintindex = Random.Range(max - lastCorr, max);

                                    if (IsLogGenericWorld)
                                        info = "margin (" + (max - lastCorr) + "--" + max + ")    max = " + max + "  I=" + SelectedPintindex + "  [" + FirstLimit + @"\" + LastLimit + "]";
                                }

                                startSegmentX = stepsPointsSart[SelectedPintindex].x;
                                startSegmentY = stepsPointsSart[SelectedPintindex].y;

                                if (IsLogGenericWorld)
                                {
                                    info += " < " + startSegmentX + "x" + startSegmentY + " >";
                                    if (isLog)
                                        Storage.Events.ListLogAdd = info;
                                    //Debug.Log(info);
                                }
                            }
                            if (ModeSegmentMarginLimit == ModeStartSegmentGen.Range) //------------ percent limit
                            {
                                int max = stepsPointsSart.Count - 1;

                                float firstCorr = FirstLimit;
                                if (firstCorr < 0f || firstCorr >= 1f)
                                    firstCorr = 0.5f;
                                float lastCorr = LastLimit;
                                if (lastCorr < 0f || firstCorr >= 1f)
                                    firstCorr = 0.5f;

                                int SelectedPintindex = 0;
                                if (IsFirstStartSegment)
                                {
                                    firstCorr = max * FirstLimit;
                                    firstCorr = (int)firstCorr;
                                    if (firstCorr < 1)
                                        firstCorr = 1;

                                    SelectedPintindex = Random.Range(0, (int)firstCorr);

                                    if (IsLogGenericWorld)
                                        info = "percent 0 -- " + firstCorr + "  max = " + max + "   index = " + SelectedPintindex;
                                }
                                else
                                {

                                    lastCorr = max - (max * LastLimit);
                                    lastCorr = (int)lastCorr;
                                    SelectedPintindex = Random.Range((int)lastCorr, max);

                                    if (IsLogGenericWorld)
                                        info = "percent  " + lastCorr + " -- " + max + "    max = " + max + "   index = " + SelectedPintindex;
                                }

                                startSegmentX = stepsPointsSart[SelectedPintindex].x;
                                startSegmentY = stepsPointsSart[SelectedPintindex].y;

                                if (IsLogGenericWorld)
                                {
                                    info += " < " + startSegmentX + "x" + startSegmentY + " >";
                                    if (isLog)
                                        Storage.Events.ListLogAdd = info;
                                    //Debug.Log(info);
                                }
                            }
                            //--------------------
                        }

                        //---- #fix start gen
                        int offsetX = Random.Range(minLevel, maxLevel);
                        int offsetY = Random.Range(minLevel, maxLevel);
                        int x = 0;
                        int y = 0;
                        bool isLeft = 1 == Random.Range(1, 3);
                        bool isTop = 1 == Random.Range(1, 3);
                        if (isLeft)
                            offsetX *= -1;
                        if (isTop)
                            offsetY *= -1;
                        x = startSegmentX + offsetX;
                        y = startSegmentY + offsetY;
                        //------------

                        stepsPointsSart.Add(new Vector2Int(x, y));

                        string fieldNew = Helper.GetNameField(x, y);

                        if (_isClearLayer || TypeModeOptStartDelete == SelCheckOptDel.DelFull)
                            ClearLayerForStructure(fieldNew);

                        if (!IsGenericContruct)
                        {
                            isValidResult = Storage.GridData.AddConstructInGridData(fieldNew, selDataTile, TypeModeOptStartDelete, TypeModeOptStartCheck);
                        }
                        else
                        {
                            Storage.Instance.SelectFieldCursor = fieldNew;
                            GenericContructInGridData();
                        }

                        if (!isValidResult && IsRepeatFind && indexRepeat < limitRepeat)
                        {
                            //#repeat
                            s--;
                            indexRepeat++;
                        }
                    }
                    indexRepeat=0;
                }
                //------
            }

        }

        //int sizeX = (int)posStructFieldNew.x + size;
        //int sizeY = (int)posStructFieldNew.y + size;
        bool isZoneStart = Helper.IsValidFieldInZona(posStructFieldNew.x, posStructFieldNew.y);
        bool isZoneEnd = Helper.IsValidFieldInZona(sizeX, sizeY);
        if (isZoneStart || isZoneEnd) //#fast
        {
            //Storage.Events.ListLogAdd = "Create construct in zona";
            Storage.GenGrid.LoadObjectsNearHero();
        }
        else
        {
            //Storage.Events.ListLogAdd = "Create construct NOT in zona";
        }
    }

    private void GenericContructInGridData()
    {
        if (string.IsNullOrEmpty(SelectedConstruction))
            return;
        //Storage.Instance.SelectFieldCursor = ""; // if (!IsGenericContruct)

        SaveConstructTileInGridData();
    }

    private bool IsTestFieldFilled(string nameField, DataTile itemTile, bool isTestFilledField = false, bool isTestExistMeType = false)
    {
        if (Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
        {
            if (isTestFilledField)
                return true;

            if (isTestExistMeType)
            {
                var indTM = Storage.Instance.GridDataG.FieldsD[nameField].Objects.FindIndex(p => p.TagObject == itemTile.Tag);
                if (indTM != -1)
                    return true;
            }
        }
        return false;
    }

    private Vector2Int GetValidMargin(int FirstLimit, int LastLimit, int max)
    {
        int firstCorr =
            (int)FirstLimit > max - 1 ?
            max - 1 :
            (int)FirstLimit;
        int lastCorr =
            (int)LastLimit > max ?
            max - 1 :
            (int)LastLimit;
        if (firstCorr < 1) firstCorr = 1;
        if (lastCorr < 1) lastCorr = 1;

        return new Vector2Int(firstCorr, lastCorr);
    }

    //private Vector2Int GetValidMargin(int FirstLimit, int LastLimit, int max)
    //{
    //    int firstCorr =
    //            (int)FirstLimit > max - 1 ?
    //            max - 1 :
    //            (int)FirstLimit;
    //    int lastCorr =
    //        (int)LastLimit > max ?
    //        max :
    //        max - (int)LastLimit;
    //    if (firstCorr < 0) firstCorr = 0;
    //    if (lastCorr < 1) lastCorr = max;
    //    if (firstCorr > lastCorr)
    //        firstCorr = lastCorr - 1;
    //    return new Vector2Int(firstCorr, lastCorr);
    //}

    private Vector2Int GetValidRange(int FirstLimit, int LastLimit, int max)
    {
        int firstCorr =
                (int)FirstLimit > max - 1 ?
                max - 1 :
                (int)FirstLimit;
        int lastCorr =
            (int)LastLimit > max ?
            max :
            (int)LastLimit;
        if (firstCorr < 0) firstCorr = 0;
        if (lastCorr < 1) lastCorr = max;
        if (firstCorr > lastCorr)
            firstCorr = lastCorr - 1;
        return new Vector2Int(firstCorr, lastCorr);
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

        bool _isClearLayer = IsClearLayer;// !m_PasteOnLayer;


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

            if (_isClearLayer)
                ClearLayerForStructure(fieldNew);

            Storage.GridData.AddConstructInGridData(fieldNew, itemTile, _isClearLayer);
        }
    }

    public void UpOptionGen1() {
        OptionGenCount++;
    }
    public void UpOptionGen2() {
        OptionGenPercent += 10;
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
        OptionGenPercent -= 10;
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

    #region Save generic options

    System.Type[] extraTypesOtions = {
            typeof(GenericOptionsWorld),
            typeof(ContainerOptionsWorld)
    };


    private void LoadVersionOptons()
    {

        //LoadVersionsGenericOptionsXML();
        LoadVersionsGenericXML();

        UpdateListVersions();

        btnAddVersionOptGen.onClick.AddListener(() =>
        {
            bool isEdit = dpnListGenOptionsVersion.gameObject.activeSelf;
            dpnListGenOptionsVersion.gameObject.SetActive(!isEdit);
            tbxVersionNameGenOption.gameObject.SetActive(isEdit);
            if (!isEdit)
            {
                string nameVersion = tbxVersionNameGenOption.text;
                if (!string.IsNullOrEmpty(nameVersion))
                    AddListVersions(nameVersion);
            }
        });


        btnAddVersionGenWorld.onClick.AddListener(() =>
        {
            bool isEdit = dpnListVersionGenWorld.gameObject.activeSelf;
            dpnListVersionGenWorld.gameObject.SetActive(!isEdit);
            tbxVersionNameGenWorld.gameObject.SetActive(isEdit);
            if (!isEdit)
            {
                string nameVersion = tbxVersionNameGenWorld.text;
                if (!string.IsNullOrEmpty(nameVersion))
                    AddListVersionsWorld(nameVersion);
            }
        });

        bntDeleteVersionOptGen.onClick.AddListener(() =>
        {
            string nameVersion = tbxVersionNameGenOption.text;// tbxVersionNameGenOption.text;
            if (SelectedGenericOptionsWorld != null)
                nameVersion = SelectedGenericOptionsWorld.NameVersion;
            DeleteVersionsOption(nameVersion);
        });

        bntDeleteVersionGenWorld.onClick.AddListener(() =>
        {
            string nameVersion = tbxVersionNameGenWorld.text;
            if (SelectedVersionWorld != null)
                nameVersion = SelectedVersionWorld.NameVersion;
            DeleteVersionsWorld(nameVersion);
        });

        bntAddInContainer.onClick.AddListener(() =>
        {
            AddOptionsInContainer();
        });

        bntGenVersionSaveUpdate.onClick.AddListener(() =>
        {
            SaveUpdateVersionGenOptions();
            //SaveVersionsGeneric();
        });
        //-------- new

        CheckStateOpionsDelete(SelCheckOptDel.All);

        checkOptStartDeleteFull.onValueChanged.AddListener(delegate
        {
            CheckStateOpionsDelete(SelCheckOptDel.DelFull);
        });
        checkOptStartDeleteTypePrefab.onValueChanged.AddListener(delegate
        {
            CheckStateOpionsDelete(SelCheckOptDel.DelType);
        });
        checkOptStartDeletePrefab.onValueChanged.AddListener(delegate
        {
            CheckStateOpionsDelete(SelCheckOptDel.DelPrefab);
        });
        checkOptStartDeleteTerra.onValueChanged.AddListener(delegate
        {
            CheckStateOpionsDelete(SelCheckOptDel.DelTerra);
        });

        //checkOptVersionGeneric.onValueChanged.AddListener(delegate
        //{
        //    if (checkOptVersionGeneric.isOn)
        //    {
        //        checkOptVersionContainerGenerics.isOn = false;
        //        m_IsListModeIsContainerGeneticOtions = false;
        //        UpdateListVersions();
        //    }
        //});
        //checkOptVersionContainerGenerics.onValueChanged.AddListener(delegate
        //{
        //    if (checkOptVersionContainerGenerics.isOn)
        //    {
        //        checkOptVersionGeneric.isOn = false;
        //        m_IsListModeIsContainerGeneticOtions = true;
        //        UpdateListVersions();
        //    }
        //});

        //bntDeleteVersionOptGen
    }



    private bool m_isNotCheckindOptDelete = false;

    private ModeDelete GetNextModeDel(ModeDelete mode)
    {
        if (mode == ModeDelete.Off)
        {
            return ModeDelete.Check;
        }
        else if (mode == ModeDelete.Check)
        {
            return ModeDelete.Clear;
        }
        else if (mode == ModeDelete.Clear)
        {
            return ModeDelete.Off;
        }
        return ModeDelete.Off;
    }

    private void CheckStateOpionsDelete(SelCheckOptDel checkType)
    {
        if (m_isNotCheckindOptDelete)
            return;

        if (checkType == SelCheckOptDel.All)
        {
            m_ModeOptStartDeleteFull = ModeDelete.Clear;
            m_ModeOptStartDeleteTypePrefab = ModeDelete.Clear;
            m_ModeOptStartDeletePrefab = ModeDelete.Clear;
            m_ModeOptStartDeleteTerra = ModeDelete.Clear;
        }
        // --- options Delete cell prefabs generic option
        //checkOptStartDeleteFull;
        //checkOptStartDeleteTypePrefab;
        //checkOptStartDeletePrefab;
        //checkOptStartDeleteTerra;

        m_isNotCheckindOptDelete = true;

        if (checkType == SelCheckOptDel.DelFull || checkType == SelCheckOptDel.All)
        {
            m_ModeOptStartDeleteFull = GetNextModeDel(m_ModeOptStartDeleteFull);

            checkOptStartDeleteFull.isOn = m_ModeOptStartDeleteFull == ModeDelete.Clear;
            SetStateDefuault(checkOptStartDeleteFull, m_ModeOptStartDeleteFull == ModeDelete.Off);
        }
        if (checkType == SelCheckOptDel.DelType || checkType == SelCheckOptDel.All)
        {
            m_ModeOptStartDeleteTypePrefab = GetNextModeDel(m_ModeOptStartDeleteTypePrefab);

            checkOptStartDeleteTypePrefab.isOn = m_ModeOptStartDeleteTypePrefab == ModeDelete.Clear;
            SetStateDefuault(checkOptStartDeleteTypePrefab, m_ModeOptStartDeleteTypePrefab == ModeDelete.Off);
        }
        if (checkType == SelCheckOptDel.DelPrefab || checkType == SelCheckOptDel.All)
        {
            m_ModeOptStartDeletePrefab = GetNextModeDel(m_ModeOptStartDeletePrefab);

            checkOptStartDeletePrefab.isOn = m_ModeOptStartDeletePrefab == ModeDelete.Clear;
            SetStateDefuault(checkOptStartDeletePrefab, m_ModeOptStartDeletePrefab == ModeDelete.Off);
        }
        if (checkType == SelCheckOptDel.DelTerra || checkType == SelCheckOptDel.All)
        {
            m_ModeOptStartDeleteTerra = GetNextModeDel(m_ModeOptStartDeleteTerra);

            checkOptStartDeleteTerra.isOn = m_ModeOptStartDeleteTerra == ModeDelete.Clear;
            SetStateDefuault(checkOptStartDeleteTerra, m_ModeOptStartDeleteTerra == ModeDelete.Off);
        }

        m_isNotCheckindOptDelete = false;
    }

    private void UpdateStateOptDelUI()
    {
        //SetStateDefuault(checkOptStartDeleteFull, false);
        //SetStateDefuault(checkOptStartDeleteTypePrefab, false);
        //SetStateDefuault(checkOptStartDeletePrefab, false);
        //SetStateDefuault(checkOptStartDeleteTerra, false);

        //checkOptStartDeleteFull.isOn = m_ModeOptStartDeleteFull == ModeDelete.Clear;
        //SetStateDefuault(checkOptStartDeleteFull, checkOptStartDeleteFull.isOn);

        //checkOptStartDeleteTypePrefab.isOn = m_ModeOptStartDeleteTypePrefab == ModeDelete.Clear;
        //SetStateDefuault(checkOptStartDeleteTypePrefab, checkOptStartDeleteTypePrefab.isOn);

        //checkOptStartDeletePrefab.isOn = m_ModeOptStartDeletePrefab == ModeDelete.Clear;
        //SetStateDefuault(checkOptStartDeletePrefab, checkOptStartDeletePrefab.isOn);

        //checkOptStartDeleteTerra.isOn = m_ModeOptStartDeleteTerra == ModeDelete.Clear;
        //SetStateDefuault(checkOptStartDeleteTerra, checkOptStartDeleteTerra.isOn);

        //CheckStateOpionsDelete(SelCheckOptDel.DelFull);
        //CheckStateOpionsDelete(SelCheckOptDel.DelType);
        //CheckStateOpionsDelete(SelCheckOptDel.DelPrefab);
        //CheckStateOpionsDelete(SelCheckOptDel.DelTerra);

        //----------------
        m_isNotCheckindOptDelete = true;

        checkOptStartDeleteFull.isOn = m_ModeOptStartDeleteFull == ModeDelete.Clear;
        SetStateDefuault(checkOptStartDeleteFull, m_ModeOptStartDeleteFull == ModeDelete.Off);

        checkOptStartDeleteTypePrefab.isOn = m_ModeOptStartDeleteTypePrefab == ModeDelete.Clear;
        SetStateDefuault(checkOptStartDeleteTypePrefab, m_ModeOptStartDeleteTypePrefab == ModeDelete.Off);

        checkOptStartDeletePrefab.isOn = m_ModeOptStartDeletePrefab == ModeDelete.Clear;
        SetStateDefuault(checkOptStartDeletePrefab, m_ModeOptStartDeletePrefab == ModeDelete.Off);

        checkOptStartDeleteTerra.isOn = m_ModeOptStartDeleteTerra == ModeDelete.Clear;
        SetStateDefuault(checkOptStartDeleteTerra, m_ModeOptStartDeleteTerra == ModeDelete.Off);

        m_isNotCheckindOptDelete = false;
    }

    private void SetStateDefuault(Toggle tgl, bool isOn)
    {
        //Image[] lisImages = tgl.GetComponents<Image>();
        Image[] lisImages = tgl.GetComponentsInChildren<Image>();
        foreach (var img in lisImages)
        {
            if (img.name == "ImgDef")
                img.enabled = isOn;
            if (img.name == "Background")
                img.enabled = !isOn;

        }
    }

    //private void UncheckedOptionsDel(string name)
    //{
    //    foreach (var opt in ListCheckGenOptDel)
    //    {
    //        if(opt.name != name)
    //        {
    //            opt.isOn = false;
    //        }
    //    }
    //}

    private void UpdateListVersions()
    {
        //if (!m_IsListModeIsContainerGeneticOtions)
        //{
        LoadListVersionGeneticOptons();
        //}
        //else
        //{
        LoadListContainersVersionsGeneticOptons();
        //}
    }


    private void AddOptionsInContainer()
    {
        if (SelectedVersionWorld == null)
        {
            Storage.Events.ListLogAdd = "### Not selected World version";
            return;
        }

        if (SelectedGenericOptionsWorld != null)
        {
            if (m_ListGenOptionsVersionForContainer == null)
                m_ListGenOptionsVersionForContainer = new List<GenericOptionsWorld>();

            int ind = m_ListGenOptionsVersionForContainer.FindIndex(p => p.NameVersion == SelectedGenericOptionsWorld.NameVersion);
            if (ind == -1)
            {
                m_ListGenOptionsVersionForContainer.Add(SelectedGenericOptionsWorld);
                SelectedVersionWorld.ListGenericOprionsWorld = m_ListGenOptionsVersionForContainer;
                UpdateListOptionsInContainer();
                SaveVersionsGeneric();
                UpdateListVersions();
            }
            else
            {
                Storage.Events.ListLogAdd = ">> Exist World version name: " + SelectedGenericOptionsWorld.NameVersion + " !!!!";
            }
        }
    }

    private void SaveUpdateVersionGenOptions()
    {
        string nameVerion = SelectedGenericOptionsWorld.NameVersion;
        AddVersionGenericOptions(nameVerion, true); //New verion
        //LoadVersionsGenericXML();
        UpdateListVersions();
    }

    private void AddListVersions(string value)
    {
        AddVersionGenericOptions(value); //New verion
        //LoadVersionsGenericXML();
        UpdateListVersions();
    }

    private void AddListVersionsWorld(string value)
    {
        if (m_ListGenOptionsVersionForContainer == null)
            m_ListGenOptionsVersionForContainer = new List<GenericOptionsWorld>();
        m_ListGenOptionsVersionForContainer.Clear();

        AddContainerVersionGeneric(value, m_ListGenOptionsVersionForContainer); //New container
        //LoadVersionsGenericXML();
        UpdateListVersions();
    }

    private void DeleteVersionsOption(string nameVersion)
    {
        var delVers = m_ListGenOptionsVersion.Find(p => p.NameVersion == nameVersion);
        if (delVers != null)
        {
            m_ListGenOptionsVersion.Remove(delVers);
            SaveVersionsGeneric();
            UpdateListVersions();
        }
    }

    private void DeleteVersionsWorld(string nameVersion)
    {
        var delVers = m_ListGenWorldVersion.Find(p => p.NameVersion == nameVersion);
        if (delVers != null)
        {
            m_ListGenWorldVersion.Remove(delVers);
            SaveVersionsGeneric();
            UpdateListVersions();
        }
    }

    private void LoadListVersionGeneticOptons()
    {

        if (m_ListGenOptionsVersion == null)
            m_ListGenOptionsVersion = new List<GenericOptionsWorld>();

        if (m_ListVersionNameGenOptionData != null)
            m_ListVersionNameGenOptionData.Clear();
        m_ListVersionNameGenOptionData = new List<Dropdown.OptionData>();

        foreach (var itemVersion in m_ListGenOptionsVersion)
        {
            m_ListVersionNameGenOptionData.Add(new Dropdown.OptionData() { text = itemVersion.NameVersion });
        }

        dpnListGenOptionsVersion.ClearOptions();
        if (m_ListVersionNameGenOptionData.Count > 0)
        {
            dpnListGenOptionsVersion.AddOptions(m_ListVersionNameGenOptionData);
            dpnListGenOptionsVersion.value = 0;
            SelectVersionGenOption(0);
        }
    }

    private void LoadListContainersVersionsGeneticOptons()
    {
        if (m_ListGenWorldVersion == null)
            m_ListGenWorldVersion = new List<ContainerOptionsWorld>();

        if (m_ListVersionNameGenWorldData != null)
            m_ListVersionNameGenWorldData.Clear();
        m_ListVersionNameGenWorldData = new List<Dropdown.OptionData>();

        foreach (var itemVersion in m_ListGenWorldVersion)
        {
            m_ListVersionNameGenWorldData.Add(new Dropdown.OptionData() { text = itemVersion.NameVersion });
        }

        dpnListVersionGenWorld.ClearOptions();
        if (m_ListVersionNameGenWorldData.Count > 0)
        {
            dpnListVersionGenWorld.AddOptions(m_ListVersionNameGenWorldData);
            dpnListVersionGenWorld.value = 0;
            SelectedVersionWorldChange(0);
            //SelectionVersionOptInContainer(0);
            //SelectionVersionOptInContainer(m_ListGenWorldVersion[0].NameVersion);
        }
    }

    private void AddContainerVersionGeneric(string value, List<GenericOptionsWorld> p_listGenOptionsVersion)
    {
        int ind = m_ListGenWorldVersion.FindIndex(p => p.NameVersion == value);
        if (ind != -1)
        {
            return;
        }

        ContainerOptionsWorld saveOptions = new ContainerOptionsWorld()
        {
            NameVersion = value,
            ListGenericOprionsWorld = p_listGenOptionsVersion
        };
        m_ListGenWorldVersion.Add(saveOptions);
        SaveVersionsGeneric();
    }


    // --- Save version generic option
    //public InputField tbxVersionNameGenOption; //ListGenOptionsVersion
    //private List<Dropdown.OptionData> m_ListVersionNameGenOptionData;
    //private List<string> m_ListVersionNameGenOption;
    //ListGenOptionsVersion.onValueChanged.AddListener(delegate {
    //        VersionGenOptionValueChanged(ListGenOptionsVersion);
    //});
    void VersionGenOptionValueChanged(Dropdown dpntGenOptionsVersion)
    {
        if (dpntGenOptionsVersion.value > m_ListVersionNameGenOptionData.Count - 1)
        {
            Debug.Log("######## VersionGenOptionValueChanged NOT dpntGenOptionsVersion.value[" + dpntGenOptionsVersion.value + "] > m_ListVersionNameGenOptionData.Length[" + m_ListVersionNameGenOptionData.Count + "]");
            return;
        }

        SelectVersionGenOption(dpntGenOptionsVersion.value);
    }

    private void SelectVersionGenOption(int index)
    {
        GenericOptionsWorld selVers = m_ListGenOptionsVersion[index];
        SelectVersionGenOption(selVers);
    }

    private void SelectVersionGenOption(GenericOptionsWorld selVers)
    {

        SelectedGenericOptionsWorld = selVers;
        //Load Options

        if (selVers.IsConstruction)
        {
            ModePaint = ToolBarPaletteMapAction.Paste;
            isPaletteBrushOn = false;
            SelectedConstruction = selVers.NameObject;
            IsGenericContruct = true;
        }
        else
        {
            SelectedCell = new DataTile()
            {
                Name = selVers.NameObject,
                Tag = selVers.TagObject
            };
        }
        Storage.Events.ListLogAdd = "Secelted Gen prefab: " + selVers.NameObject + @" \ " + selVers.TagObject;
        //}
        //selectGeneticObject.Name = selVers.NameObject;
        //selectGeneticObject.Tag = selVers.TagObject;

        //NameVersion = nameVerion,
        //CountObjects = OptionGenCount,
        OptionGenCount = selVers.CountObjects;
        OptionGenSegments = selVers.Segment;
        OptionGenLevel = selVers.Level;

        IsClearLayer = selVers.IsClear;
        IsSteps = selVers.IsSteps;
        //IsTestFilledFieldGen = selVers.IsTestField;
        //IsTestExistMeTypeGen = selVers.IsTestType;

        m_ModeOptStartDeleteFull = ModeDelete.Off;
        m_ModeOptStartDeleteTypePrefab = ModeDelete.Off;
        m_ModeOptStartDeletePrefab = ModeDelete.Off;
        m_ModeOptStartDeleteTerra = ModeDelete.Off;

        switch (selVers.TypeModeOptStartDelete)
        {
            case "DelFull":
                m_ModeOptStartDeleteFull = ModeDelete.Clear;
                break;
            case "DelType":
                m_ModeOptStartDeleteTypePrefab = ModeDelete.Clear;
                break;
            case "DelPrefab":
                m_ModeOptStartDeletePrefab = ModeDelete.Clear;
                break;
            case "DelTerra":
                m_ModeOptStartDeleteTerra = ModeDelete.Clear;
                break;
        }
        switch (selVers.TypeModeOptStartCheck)
        {
            case "DelFull":
                m_ModeOptStartDeleteFull = ModeDelete.Check;
                break;
            case "DelType":
                m_ModeOptStartDeleteTypePrefab = ModeDelete.Check;
                break;
            case "DelPrefab":
                m_ModeOptStartDeletePrefab = ModeDelete.Check;
                break;
            case "DelTerra":
                m_ModeOptStartDeleteTerra = ModeDelete.Check;
                break;
        }
        UpdateStateOptDelUI();

        // 1.
        checkOptStartSegmentMarginLimit.isOn = selVers.IsSegmentNextPointMargin;
        checkOptStartSegmentRange.isOn = selVers.IsSegmentNextPointRange;

        // 3. 
        LastLimit = selVers.LastLimit;
        FirstLimit = selVers.FirstLimit;

        //-------
        tbxOptStartSegment.text = selVers.FirstLimit.ToString();
        tbxOptEndSegment.text = selVers.LastLimit.ToString();

        // 2.
        IsFirstStartSegment = selVers.IsSegmentNextPointFirst;

        IsRepeatFind = selVers.IsRepeatFind;
    }

    void VersionGenWorldValueChanged(Dropdown dpntGenWorldVersion)
    {
        if (dpntGenWorldVersion.value > m_ListVersionNameGenWorldData.Count - 1)
        {
            //Debug.Log("######## VersionGenOptionValueChanged NOT dpntGenOptionsVersion.value[" + dpntGenOptionsVersion.value + "] > m_ListVersionNameGenOptionData.Length[" + m_ListVersionNameGenOptionData.Count + "]");
            return;
        }

        SelectedVersionWorldChange(dpntGenWorldVersion.value);
    }

    private void SelectedVersionWorldChange(int index)
    {
        SelectedVersionWorld = m_ListGenWorldVersion[index];
        if (SelectedVersionWorld.ListGenericOprionsWorld == null)
            SelectedVersionWorld.ListGenericOprionsWorld = new List<GenericOptionsWorld>();

        List<GenericOptionsWorld> listOptGen = SelectedVersionWorld.ListGenericOprionsWorld;
        //#LIST IN WORLD
        m_ListGenOptionsVersionForContainer = SelectedVersionWorld.ListGenericOprionsWorld;
        UpdateListOptionsInContainer();
    }

    private void UpdateListOptionsInContainer()
    {
        if (m_ListGenOptionsVersionForContainer == null)
            return;

        //var itemsDestroy = ContentListVersionsGenericWorld.GetComponentsInChildren<GameObject>();
        foreach (Transform itemsDestroy in ContentListVersionsGenericWorld.transform)
        {
            //child is your child transform
            if (itemsDestroy != null)
            {
                Destroy(itemsDestroy.gameObject);
            }
        }
        //if (itemsDestroy != null)
        //{
        //    foreach(var itemDel in itemsDestroy)
        //        Destroy(itemDel);
        //}

        foreach (var itemOpt in m_ListGenOptionsVersionForContainer)
        {
            //Storage.Events.CreateCommandLogText(itemOpt.NameVersion, Color.white, ContentListVersionsGenericWorld.transform);
            Button buttonVersOpt;
            Button btnSubCommand;
            Storage.Events.CreateListButtton(itemOpt.NameVersion, ContentListVersionsGenericWorld.transform, out buttonVersOpt, out btnSubCommand);
            buttonVersOpt.onClick.AddListener(delegate () {
                //Selected item version in World
                
                //string nameVersOpt = buttonVersOpt.GetComponent<Text>().text;
                string nameVersOpt = buttonVersOpt.GetComponentInChildren<Text>().text;
                Storage.Events.ListLogAdd = ">> Selected " + nameVersOpt + " in " + SelectedVersionWorld.NameVersion;
                SelectionVersionOptInContainer(nameVersOpt);
            });

            //#TEST
            if(btnSubCommand.name!= "btnSubCommand")
            {
                Storage.Events.ListLogAdd = "#######  btnSubCommand.name!= btnSubCommand    " + btnSubCommand.name ;
            }

            btnSubCommand.onClick.AddListener(delegate () {
                //Remove version in World
                string nameVersOpt = itemOpt.NameVersion;// buttonVersOpt.GetComponentInChildren<Text>().text;
                GenericOptionsWorld genOpt = m_ListGenOptionsVersionForContainer.Find(p => p.NameVersion == nameVersOpt);
                if (genOpt != null)
                {
                    Storage.Events.ListLogAdd = ">> Remove " + genOpt + " in " + SelectedVersionWorld.NameVersion;
                    m_ListGenOptionsVersionForContainer.Remove(genOpt);
                    ///----
                    if (dpnListGenOptionsVersion.options.Count > 0)
                        dpnListGenOptionsVersion.value = 0;
                    SaveVersionsGeneric();
                    SelectVersionGenOption(0);
                    UpdateListOptionsInContainer();
                    ///---
                }
            });
        }
    }

    private void SelectionVersionOptInContainer(int index)
    {
        GenericOptionsWorld genOpt = m_ListGenOptionsVersionForContainer[index];
        SelectionVersionOptInContainer(genOpt);
    }

    private void SelectionVersionOptInContainer(string nameVersOpt)
    {
        GenericOptionsWorld genOpt = m_ListGenOptionsVersionForContainer.Find(p => p.NameVersion == nameVersOpt);
        SelectionVersionOptInContainer(genOpt);
    }

    private void SelectionVersionOptInContainer(GenericOptionsWorld genOpt) //string nameVersOpt)
    {
        //GenericOptionsWorld genOpt = m_ListGenOptionsVersionForContainer.Find(p => p.NameVersion == nameVersOpt);
        if (genOpt != null)
        {
            //int index = m_ListGenOptionsVersion.FindIndex(p => p.NameVersion == nameVersOpt);
            int index = m_ListGenOptionsVersion.FindIndex(p => p.NameVersion == genOpt.NameVersion);
            
            if (index != -1)
            {
                dpnListGenOptionsVersion.value = index;
            }
            else
            {
                m_ListGenOptionsVersion.Add(genOpt);
                if (dpnListGenOptionsVersion.options.Count>0)
                    dpnListGenOptionsVersion.value = dpnListGenOptionsVersion.options.Count-1;
                SaveVersionsGeneric();
            }

            SelectVersionGenOption(genOpt);
        }
        else
        {
            Storage.Events.ListLogAdd = "#### Not find version in ListGenOptionsVersionForContainer: " + genOpt.NameVersion;
        }
    }

    public void AddVersionGenericOptions(string nameVerion, bool isUpdate = false)
    {
        if(SelectedCell == null)
        {
            Storage.Events.ListLogAdd = "##### Not Selected Genetic Object !!!!!!";
            Debug.Log("###### Not Selected Genetic Object");
            return;
        }

        int ind = m_ListGenOptionsVersion.FindIndex(p => p.NameVersion == nameVerion);
        if (ind != -1)
        {
            return;
        }

        string nameObject = IsGenericContruct ? SelectedConstruction : SelectedCell.Name;
        string tagObject = IsGenericContruct ? "" : SelectedCell.Tag;

        //Create Options
        GenericOptionsWorld saveOptions = new GenericOptionsWorld()
        {
            NameVersion = nameVerion,
            NameObject = nameObject,
            TagObject = tagObject,

            CountObjects = OptionGenCount,
            Segment = OptionGenSegments,
            Level = OptionGenLevel,

            IsClear = IsClearLayer,
            IsSteps = IsSteps,
            //IsTestField = IsTestFilledFieldGen,
            //IsTestType = IsTestExistMeTypeGen,
            TypeModeOptStartCheck = TypeModeOptStartCheck.ToString(),
            TypeModeOptStartDelete = TypeModeOptStartDelete.ToString(),

            LastLimit = LastLimit,
            FirstLimit = FirstLimit,
            IsSegmentNextPointFirst = IsFirstStartSegment,
            IsSegmentNextPointMargin = ModeSegmentMarginLimit == ModeStartSegmentGen.Margin,
            IsSegmentNextPointRange = ModeSegmentMarginLimit == ModeStartSegmentGen.Range,

            IsConstruction = IsGenericContruct,
            IsRepeatFind = IsRepeatFind
        };

        //-----
        //if (string.IsNullOrEmpty(SelectedConstruction))
        //    return;
        //Storage.Instance.SelectFieldCursor = ""; // if (!IsGenericContruct)
        //if (selVers.IsConstruction)
        //{
        //    ModePaint = ToolBarPaletteMapAction.Paste;
        //    isPaletteBrushOn = false;
        //    SelectedConstruction = selVers.NameObject;
        //    IsGenericContruct = true;
        //----
        if (!isUpdate)
        {
            m_ListGenOptionsVersion.Add(saveOptions);
        }
        else
        {
            int indUpdate = m_ListGenOptionsVersion.FindIndex(p => p.NameVersion == nameVerion);
            if (indUpdate != -1)
                m_ListGenOptionsVersion[indUpdate] = saveOptions;
            else
                Storage.Events.ListLogAdd = "#### Not find option gen : " + saveOptions;
            //SelectedGenericOptionsWorld
        }
        //SaveVersionsGenericOptions();
        SaveVersionsGeneric();
    }


    //public void SaveVersionsGenericWorld()
    //{
    //    string nameXML = Storage.Instance.DataPathVersion;
    //    ContainerOptionsWorld state = new ContainerOptionsWorld()
    //    {
    //        NameVersion = nameXML,
    //        ListGenericOprionsWorld = m_ListGenOptionsVersion
    //    };
    //    string pathVersOptions = "";
    //    Serializator.SaveXml(state, pathVersOptions, false, extraTypesOtions);
    //}

    public void SaveVersionsGeneric()
    {

        string pathVersOptions = Storage.Instance.DataPathVersion;
        GeneticWorld state = new GeneticWorld()
        {
              ListGenericWorlds = m_ListGenWorldVersion,
              GenericOptions = new ContainerOptionsWorld()
              {
                   NameVersion = "Options",
                   ListGenericOprionsWorld = m_ListGenOptionsVersion
              }
        };
        Serializator.SaveXml(state, pathVersOptions, false, extraTypesOtions);
    }

    public void LoadVersionsGenericXML()
    {
        string pathVersOptions = Storage.Instance.DataPathVersion;
        GeneticWorld state = Serializator.LoadXml<GeneticWorld>(pathVersOptions, extraTypesOtions);
        if (state != null)
        {
            m_ListGenOptionsVersion = state.GenericOptions.ListGenericOprionsWorld;
            m_ListGenWorldVersion = state.ListGenericWorlds;
        }
    }

    //public void SaveVersionsGenericOptions()
    //{
    //    string nameXML = Storage.Instance.DataPathVersion;
    //    ContainerOptionsWorld state = new ContainerOptionsWorld()
    //    {
    //        NameVersion = nameXML,
    //        ListGenericOprionsWorld = m_ListGenOptionsVersion
    //    };
    //    string pathVersOptions = "";

        

    //    Serializator.SaveXml(state, pathVersOptions, false, extraTypesOtions);
    //    //m_ListGenOptionsVersion;
    //}
    //public void LoadVersionsGenericOptionsXML()
    //{
    //    string pathVersOptions = Storage.Instance.DataPathVersion;
    //    ContainerOptionsWorld state =  Serializator.LoadXml<ContainerOptionsWorld>(pathVersOptions, extraTypesOtions);
    //    if(state==null)
    //    {
    //        m_ListGenOptionsVersion = new List<GenericOptionsWorld>();
    //    }
    //    else
    //    {
    //        m_ListGenOptionsVersion = state.ListGenericOprionsWorld;
    //    }
    //}

    #endregion
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

//[XmlRoot("Grid")]
//[XmlInclude(typeof(FieldData))]
//[XmlType("Field")] 

[XmlRoot("GenWorld")]
[XmlInclude(typeof(ContainerOptionsWorld))]
public class GeneticWorld
{
    public List<ContainerOptionsWorld> ListGenericWorlds;
    public ContainerOptionsWorld GenericOptions;
}

[XmlRoot("OptionsWorld")]
[XmlInclude(typeof(GenericOptionsWorld))]
public class ContainerOptionsWorld
{
    public string NameVersion;
    public List<GenericOptionsWorld> ListGenericOprionsWorld;
}

[XmlType("Option")]
public class GenericOptionsWorld
{
    public string NameVersion { get; set; }

    public int CountObjects;
    public int Segment;
    public int Level;

    //public int OptStartSegment;
    //public int OptEndSegment;
    public float LastLimit;
    public float FirstLimit;

    public string TagObject;
    public string NameObject;

    public bool IsClear;
    //public bool IsTestField;
    //public bool IsTestType;

    public string TypeModeOptStartDelete;
    public string TypeModeOptStartCheck;

    public bool IsSteps;
    public bool IsSegmentNextPointMargin;
    public bool IsSegmentNextPointRange;
    public bool IsSegmentNextPointFirst;

    public bool IsConstruction;
    public bool IsRepeatFind;
}
