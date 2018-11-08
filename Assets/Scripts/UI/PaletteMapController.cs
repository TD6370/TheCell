using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteMapController : MonoBehaviour {

    public GameObject FramePaletteMap;
    public Button btnClose;
    public Dropdown ListConstructsControl;

    private List<Dropdown.OptionData> m_ListConstructsOptopnsData;

    public int sizeCellMap = 20;

    public float width;

    private GridLayoutGroup m_GridMap;

    // Use this for initialization
    void Start()
    {
        //ResizeScaleGrid();

        btnClose.onClick.AddListener(delegate
        {
            Show(false);
        });


    }

    private void LateUpdate()
    {
        LoadListConstructsControl();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void ResizeScaleGrid()
    {
        width = this.gameObject.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / sizeCellMap, width / sizeCellMap);
        m_GridMap = this.gameObject.GetComponent<GridLayoutGroup>();
        m_GridMap.cellSize = newSize;
    }


    private void LoadListConstructsControl()
    {
        if(ListConstructsControl == null)
        {
            Debug.Log("###### LoadListConstructsContro  ListConstructsControl is Empty");
            return;
        }
        if(Storage.TilesManager == null)
        {
            Debug.Log("###### LoadListConstructsContro  TilesManager is Empty");
            return;
        }
        if (Storage.TilesManager.DataMapTales == null)
        {
            Debug.Log("###### LoadListConstructsContro  DataMapTales is Empty");
            return;
        }

        m_ListConstructsOptopnsData = new List<Dropdown.OptionData>();
        foreach (var itemTileData in Storage.TilesManager.DataMapTales)
        {
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
