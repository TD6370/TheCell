using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteMapController : MonoBehaviour {

    public GameObject FramePaletteMap;
    public Button btnClose;
    public Dropdown ListConstructsControl;
    public string SelectedConstruction { get; set; }
    public DataTile SelectedCell { get; set; }
    public GameObject PrefabCellMapPalette;

    private int m_SizeBrush = 4;
    public int SizeBrush
    {
        get
        {
            return m_SizeBrush;
        }
        set
        {
            m_SizeBrush = value;
            btxSizeBrush.text = m_SizeBrush.ToString();
        }

    }

    //public Button btnOnPaint;
    public Toggle btnOnPaint;
    public Toggle btnPaste;
    public Toggle btnBrush;
    public Toggle btnClear;
    public Toggle btnCursor;
    public Toggle btnOnLayer;
    public Toggle btnTeleport;
    public Button btnReloadWorld;
    public Button btnRefreshMap;
    public Button btnUp;
    public Button btnDown;
    public InputField btxSizeBrush;

    private List<Toggle> m_listToggleMode;

    public ToolBarPaletteMapAction ModePaint = ToolBarPaletteMapAction.Paste;

    public bool IsPaintsOn = false;
    //public bool IsCursorOn = false;
    private bool m_PasteOnLayer = false;

    private List<Dropdown.OptionData> m_ListConstructsOptopnsData;
    private List<GameObject> m_listCallsOnPalette;
    private List<string> m_ListNamesConstructs;

    public int sizeCellMap = 20;
    public float ActionRate = 0.5f;
    private float DelayTimer = 0F;
    

    private GridLayoutGroup m_GridMap;

    private void Awake()
    {
        m_GridMap = this.gameObject.GetComponent<GridLayoutGroup>();
        m_listToggleMode = new List<Toggle>()
        {
             btnPaste,
             btnClear,
             btnTeleport
        };
    }

    // Use this for initialization
    void Start()
    {
        //ResizeScaleGrid();

        InitEventsButtonMenu();

        

        m_listCallsOnPalette = new List<GameObject>();

        m_ListNamesConstructs = new List<string>();

        ListConstructsControl.onValueChanged.AddListener(delegate {
            DropdownValueChanged(ListConstructsControl);
        });

        //--------- After Update
        StartCoroutine(LoadMap());
    }

    private void LateUpdate()
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}


    private void FixedUpdate()
    {
        if (IsPaintsOn && !Storage.Map.IsOpen)
        {
            switch (ModePaint)
            {
                case ToolBarPaletteMapAction.Paste:
                case ToolBarPaletteMapAction.Clear:
                    ShowBorderBrush();
                    break;
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

        LoadListConstructsControl();
    }

    public void PaintAction()
    {
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

        }
        if(Storage.Map.IsOpen)
            Storage.Map.Refresh();
    }

    private void TeleportHero()
    {
        int posTransferHeroX = (int)(Storage.Map.SelectPointField.x * Storage.ScaleWorld);
        int posTransferHeroY = (int)(Storage.Map.SelectPointField.y * Storage.ScaleWorld);
        posTransferHeroY *= -1;
        Storage.Player.TeleportHero(posTransferHeroX, posTransferHeroY);
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

        LoadConstructOnPalette(m_ListNamesConstructs[dpntStructurs.value]);
    }

    private void DefaultModeOn()
    {
        ModePaint = ToolBarPaletteMapAction.Cursor;
        btnCursor.isOn = true;
        btnPaste.isOn = false;
        btnClear.isOn = false;
        btnTeleport.isOn = false;
        Storage.DrawGeom.DrawClear();
    }

    private void InitEventsButtonMenu()
    {
        btnClose.onClick.AddListener(delegate
        {
            Show(false);
        });
        btnOnPaint.onValueChanged.AddListener(delegate
        {
            IsPaintsOn = btnOnPaint.isOn;
            if (IsPaintsOn)
            {
                ModePaint = ToolBarPaletteMapAction.Cursor;
                Storage.PlayerController.CursorSelectionOn(true);
                DefaultModeOn();
            }
        });
        btnCursor.onValueChanged.AddListener(delegate
        {
            Storage.PlayerController.CursorSelectionOn(btnCursor.isOn);
            if (btnCursor.isOn)
            {
                ModePaint = ToolBarPaletteMapAction.Cursor;
                btnPaste.isOn = false;
                btnClear.isOn = false;
                btnTeleport.isOn = false;
            }
            else if (ModePaint == ToolBarPaletteMapAction.Cursor)
            {
                ModePaint = ToolBarPaletteMapAction.None;
            }
        });

        btnPaste.onValueChanged.AddListener(delegate
        {
            if (btnPaste.isOn)
            {
                ModePaint = ToolBarPaletteMapAction.Paste;
                //btnCursor.isOn = false;
                btnClear.isOn = false;
                btnTeleport.isOn = false;
            }
            else if(ModePaint == ToolBarPaletteMapAction.Paste)
            {
                Storage.DrawGeom.DrawClear();
                ModePaint = ToolBarPaletteMapAction.None;
            }
        });
        btnClear.onValueChanged.AddListener(delegate
        {
            if (btnClear.isOn)
            {
                ModePaint = ToolBarPaletteMapAction.Clear;
                btnPaste.isOn = false;
                //btnCursor.isOn = false;
                btnTeleport.isOn = false;
            }
            else if (ModePaint == ToolBarPaletteMapAction.Clear)
            {
                Storage.DrawGeom.DrawClear();
                ModePaint = ToolBarPaletteMapAction.None;
            }
        });
        btnTeleport.onValueChanged.AddListener(delegate
        {
            if (btnTeleport.isOn)
            {
                ModePaint = ToolBarPaletteMapAction.Teleport;
                btnPaste.isOn = false;
                btnCursor.isOn = false;
            }
            else if (ModePaint == ToolBarPaletteMapAction.Teleport)
            {
                ModePaint = ToolBarPaletteMapAction.None;
            }
        });


        btnOnLayer.onValueChanged.AddListener(delegate
        {
            if (btnOnLayer.isOn)
            {
                //ModePaint = ToolBarPaletteMapAction.Clear;
            }
            m_PasteOnLayer = btnOnLayer.isOn;
        });


        btnReloadWorld.onClick.AddListener(delegate
        {
            Storage.Events.ReloadWorld();
        });
        btnRefreshMap.onClick.AddListener(delegate
        {
            Storage.Map.Create(true);
        });

        
        btnUp.onClick.AddListener(delegate
        {
            SizeBrush+=3;
            btxSizeBrush.text = SizeBrush.ToString();

        });
        btnDown.onClick.AddListener(delegate
        {
            SizeBrush-=3;
            btxSizeBrush.text = SizeBrush.ToString();
        });

        btxSizeBrush.text = SizeBrush.ToString();
        btxSizeBrush.onValueChange.AddListener(delegate
        {
            SizeBrush = int.Parse(btxSizeBrush.text);
        });

        //    public Button btnUp;
        //public Button btnDown;
        //public InputField btxSizeBrush;
    }

    private void UncheckModeTool(Toggle toggleOn, ToolBarPaletteMapAction actionMode)
    {
        if (toggleOn.isOn)
        {
            ModePaint = actionMode;
            foreach (var toggleItem in m_listToggleMode)
            {
                if (toggleItem.Equals(toggleOn))
                    toggleItem.isOn = false;
            }
        }
        else if (ModePaint == actionMode)
        {
            ModePaint = ToolBarPaletteMapAction.None;
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

    private void ResizeScaleGrid(int column)
    {
        float size = this.gameObject.GetComponent<RectTransform>().rect.width;
        float ratio = 0.9f;
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

    public void Show(bool isClose = false)
    {
        if(!isClose)
        {
            if (!FramePaletteMap.activeSelf && m_ListNamesConstructs.Count==0)
                LoadListConstructsControl();
            FramePaletteMap.SetActive(!FramePaletteMap.activeSelf);
        }
        else
        {
            FramePaletteMap.SetActive(false);
        }
        Storage.DrawGeom.DrawClear();
        //ContentGridPaletteMap
    }

    public void SelectedCellMap(DataTile DataTileCell)
    {
        SelectedCell = DataTileCell;
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

    private void Paste()
    {
        if (Time.time > DelayTimer)
        {
            SaveConstructTileInGridData();
            Storage.GenGrid.ReloadGridLook();
            DefaultModeOn();

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


        posStructFieldStart.y--; //#fix

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



    private void ClearLayerForStructure(string field, bool isClearData = false)
    {
        //Destroy All Objects
        if (Storage.Instance.GamesObjectsReal.ContainsKey(field))
        {
            var listObjs = Storage.Instance.GamesObjectsReal[field];
            foreach (var obj in listObjs.ToArray())
            {
                Storage.Instance.AddDestroyGameObject(obj);
            }
        }


        //Faste
        //if (Storage.Instance.GamesObjectsReal.ContainsKey(field))
        //{
        //    var listObjs = Storage.Instance.GamesObjectsReal[field];
        //    foreach (var obj in listObjs.ToArray())
        //    {
        //        //Storage.Instance.AddDestroyGameObject(obj);
        //        Destroy(obj);
        //    }
        //    Storage.Instance.GamesObjectsReal[field].Clear();
        //}

        if (isClearData)
        {
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
    Transfer
}
