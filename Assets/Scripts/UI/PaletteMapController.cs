using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteMapController : MonoBehaviour {

    public GameObject FramePaletteMap;
    public Button btnClose;

    public int sizeCellMap = 20;

    public float width;

    private GridLayoutGroup m_GridMap;

    // Use this for initialization
    void Start()
    {
        //ResizeScaleGrid();

        btnClose.onClick.AddListener(delegate
        {
            FramePaletteMap.SetActive(false);
        });
    }

    private void ResizeScaleGrid()
    {
        width = this.gameObject.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / sizeCellMap, width / sizeCellMap);
        m_GridMap = this.gameObject.GetComponent<GridLayoutGroup>();
        m_GridMap.cellSize = newSize;
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

    // Update is called once per frame
    void Update () {
		
	}


}
