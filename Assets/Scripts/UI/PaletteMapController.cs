﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteMapController : MonoBehaviour {

    public GameObject FramePaletteMap;
    public Button btnClose;
    public Dropdown ListConstructsControl;
    public string SelectedStructure { get; set; }
    public DataTile SelectedCell { get; set; }
    public GameObject PrefabCellMapPalette;

    //public Button btnOnPaint;
    public Toggle btnOnPaint;
    public Toggle btnPaste;
    public Toggle btnBrush;
    public Toggle btnClear;
    public Toggle btnCursor;
    public Button btnReloadWorld;
    public Button btnRefreshMap;
    

    public ToolBarPaletteMapAction ModePaint = ToolBarPaletteMapAction.Paste;

    public bool IsPaintsOn = false;
    //public bool IsCursorOn = false;

    private List<Dropdown.OptionData> m_ListConstructsOptopnsData;
    private List<GameObject> m_listCallsOnPalette;
    private List<string> m_ListNamesStructurs;

    public int sizeCellMap = 20;
    public float ActionRate = 0.5f;
    private float DelayTimer = 0F;

    private GridLayoutGroup m_GridMap;

    private void Awake()
    {
        m_GridMap = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Use this for initialization
    void Start()
    {
        //ResizeScaleGrid();

        InitEventsButtonMenu();

        

        m_listCallsOnPalette = new List<GameObject>();

        m_ListNamesStructurs = new List<string>();

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

    IEnumerator LoadMap()
    {
        yield return new WaitForSeconds(0.3f);

        LoadListConstructsControl();
    }

    void DropdownValueChanged(Dropdown dpntStructurs)
    {
        //LoadConstructOnPalette(dpntStructurs.captionText.ToString());
        //LoadConstructOnPalette(dpntStructurs.value);
        //LoadConstructOnPalette(dpntStructurs.itemText.ToString() + " .2");
        //string selectStructure = 
        if(dpntStructurs.value > m_ListNamesStructurs.Count-1)
        {
            Debug.Log("######## DropdownValueChanged NOT dpntStructurs.value[" + dpntStructurs.value  + "] > m_ListNmeStructurs.Length[" + m_ListNamesStructurs.Count + "]");
            return;
        }

        LoadConstructOnPalette(m_ListNamesStructurs[dpntStructurs.value]);
    }

    private void DefaultModeOn()
    {
        ModePaint = ToolBarPaletteMapAction.Cursor;
        btnCursor.isOn = true;
        btnPaste.isOn = false;
        btnClear.isOn = false;
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
            }
        });

        btnPaste.onValueChanged.AddListener(delegate
        {
            if (btnPaste.isOn)
            {
                ModePaint = ToolBarPaletteMapAction.Paste;
                //btnCursor.isOn = false;
                btnClear.isOn = false;
            }
        });
        btnClear.onValueChanged.AddListener(delegate
        {
            if (btnClear.isOn)
            {
                ModePaint = ToolBarPaletteMapAction.Clear;
                btnPaste.isOn = false;
                //btnCursor.isOn = false;
            }
        });

        btnReloadWorld.onClick.AddListener(delegate
        {
            Storage.Events.ReloadWorld();
        });
        btnRefreshMap.onClick.AddListener(delegate
        {
            Storage.Map.Create(true);
        });

    }

    private void LoadConstructOnPalette(string keyStruct)
    {
        Debug.Log("Selected struct :" + keyStruct);
        SelectedStructure = keyStruct;

        if(!Storage.TilesManager.DataMapTiles.ContainsKey(SelectedStructure))
        {
            Debug.Log("######### LoadConstructOnPalette: TilesManager.DataMapTiles  Not find SelectedStructure: " + SelectedStructure);
            return;
        }

        var listTiles = Storage.TilesManager.DataMapTiles[SelectedStructure];

        float col = listTiles.Count;
        //sizeCellMap = listTiles.Count;
        int countColumnMap = (int)Mathf.Sqrt(col);
        m_GridMap.constraintCount = countColumnMap;

        foreach(var oldCell in m_listCallsOnPalette)
        {
            Destroy(oldCell);
        }
        m_listCallsOnPalette.Clear();

        ResizeScaleGrid(countColumnMap);

        foreach (DataTile itemTileData in listTiles)
        {
            string namePrefab = itemTileData.Name;
            string nameTexture = itemTileData.Name;

            GameObject cellMap = (GameObject)Instantiate(PrefabCellMapPalette);
            cellMap.transform.SetParent(this.gameObject.transform);
           
            Texture2D textureTile = Storage.TilesManager.CollectionTextureTiles[nameTexture];
            Sprite spriteTile = Sprite.Create(textureTile, new Rect(0.0f, 0.0f, textureTile.width, textureTile.height), new Vector2(0.5f, 0.5f), 100.0f);
            //cellMap.GetComponent<SpriteRenderer>().sprite = spriteTile;
            cellMap.GetComponent<Image>().sprite = spriteTile;
            cellMap.GetComponent<CellMapControl>().DataTileCell = itemTileData;
            cellMap.SetActive(true);

            m_listCallsOnPalette.Add(cellMap);

            //Add prefab in World
            //GameObject prefabGameObject = Storage.GridData.FindPrefab(namePrefab);
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

    private void CteateDatatTilesOnMapTiles()
    {
        Storage.TilesManager.CreateDataTiles();
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

        m_ListNamesStructurs.Clear();
        m_ListConstructsOptopnsData = new List<Dropdown.OptionData>();
        
        foreach (var itemTileData in Storage.TilesManager.DataMapTiles)
        {
            m_ListNamesStructurs.Add(itemTileData.Key);
            m_ListConstructsOptopnsData.Add(new Dropdown.OptionData() { text = itemTileData.Key });
        }
        //Dropdown.OptionData optionTileConstruct = new Dropdown.OptionData() { text = "Prefab Tail 01." };
        //m_ListConstructsOptopnsData.Add(new Dropdown.OptionData() { text = "Prefab Tail 01." });
        //m_ListConstructsOptopnsData.Add(new Dropdown.OptionData() { text = "Prefab Tail 02." });

        ListConstructsControl.ClearOptions();
        ListConstructsControl.AddOptions(m_ListConstructsOptopnsData);
    }

    public void Show(bool isClose = false)
    {
        if(!isClose)
        {
            FramePaletteMap.SetActive(!FramePaletteMap.activeSelf);
        }
        else
        {
            FramePaletteMap.SetActive(false);
        }
            //ContentGridPaletteMap
    }

    public void SelectedCellMap(DataTile DataTileCell)
    {
        SelectedCell = DataTileCell;
    }

    public void PaintAction()
    {
        //ToolBarPaletteMapAction ModePaint
        switch (ModePaint)
        {
            case ToolBarPaletteMapAction.Paste:
                if (Time.time > DelayTimer)
                {
                    SaveConstructTileInGridData();
                    DefaultModeOn();

                    DelayTimer = Time.time + ActionRate;
                }
                break;
            case ToolBarPaletteMapAction.Clear:
                break;
        }
    }



    private void SaveConstructTileInGridData()
    {
        

        string fieldStart = Storage.Instance.SelectFieldPosHero;
        var listTiles = Storage.TilesManager.DataMapTiles[SelectedStructure];

        Vector2 posStructFieldStart = Helper.GetPositByField(fieldStart);
        Vector2 posStructFieldNew = Helper.GetPositByField(fieldStart);
        foreach (DataTile itemTile in listTiles)
        {
            //Storage.GridData.
            posStructFieldNew = posStructFieldStart + new Vector2(itemTile.X, itemTile.Y);
            //Vector2 posStructNew = new Vector2(posStructFieldNew.x * Storage.ScaleWorld, posStructFieldNew.y * Storage.ScaleWorld * (-1));
            string fieldNew = Helper.GetNameField(posStructFieldNew.x, posStructFieldNew.y);

            Storage.GridData.AddConstructInGridData(fieldNew, itemTile);
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
    Cursor
}
