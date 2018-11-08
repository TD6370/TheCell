using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMapControl : MonoBehaviour {

    PaletteMapController _scriptMap;
    public DataTile DataTileCell;
    public GameObject ContentGridMap;

    public int PosX
    {
        get
        {
            return DataTileCell.X;
        }
    }
    public int PosY
    {
        get
        {
            return DataTileCell.Y;
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
        
    }

    // Update is called once per frame
    void Update () {
	    	
	}

    private void OnMouseDown()
    {
        _scriptMap.SelectedCellMap(DataTileCell);
    }

}
