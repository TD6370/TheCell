using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellMapControl : MonoBehaviour, IPointerDownHandler {

    PaletteMapController _scriptMap;
    public GameObject ContentGridMap;
    public Text TittleCell;
    public Text InfoCell;
    public GameObject BorderCellPalette;

    private DataTile m_DataTileCell;
    public DataTile DataTileCell
    {
        get
        {
            return m_DataTileCell;
        }
        set
        {
            m_DataTileCell = value;

            if (m_DataTileCell != null)
            {
                TittleCell.text = PosX + "x" + PosY;
                InfoCell.text = m_DataTileCell.Name;
            }
        }
    }

    public int PosX
    {
        get
        {
            return m_DataTileCell.X;
        }
    }
    public int PosY
    {
        get
        {
            return m_DataTileCell.Y;
        }
    }

    // Use this for initialization
    void Start () {
        if(ContentGridMap==null)
        {
            Debug.Log("###### ContentGridMap is Empty");
            return;
        }

        //_scriptMap =GetComponentInParent<PaletteMapController>();
        _scriptMap = ContentGridMap.GetComponent<PaletteMapController>();
        if (_scriptMap == null)
        {
            Debug.Log("###### scriptMap is Empty");
            return;
        }

        BorderCellPalette.SetActive(false);

    }


    // Update is called once per frame
    void Update () {
	    	
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        _scriptMap.SelectedCellMap(m_DataTileCell, this.gameObject, BorderCellPalette);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        
    }

    //private void OnGUI()
    //{
    //    if (DataTileCell != null)
    //    {
    //        //Vector3 posInfo = ContentGridMap.transform.position - this.transform.position;
    //        //Vector2 posInfo = new Vector2(this.transform.position.x, ContentGridMap.transform.position.y - this.transform.position.y);
    //        //Vector2 posInfo = new Vector2(this.transform.position.x, this.transform.position.y - ContentGridMap.transform.position.y);
    //        Vector2 posInfo = new Vector2();
    //        Rect rectInfo = new Rect(posInfo, new Vector2(100, 50));
    //        GUI.Label(rectInfo, PosX + "x" + PosY);
    //    }
    //}

}
