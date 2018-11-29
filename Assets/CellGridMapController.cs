﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellGridMapController : MonoBehaviour {

    public Text LabelCellMapGrid;

    public string NameMap { get; private set; }
    public string Field { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    private bool IsFirstLoading = false;

    private Sprite Sprite
    {
        set
        {
            this.GetComponent<SpriteRenderer>().sprite = value;
        }
    }

    // Use this for initialization
    void Start () {

        NameMap = this.name;
        Field = NameMap.Replace("MapGridCell", "");
        //Field = NameMap;
        Vector2 pos = Helper.GetPositByField(Field);
        X = (int)pos.x;
        Y = (int)pos.y;

        //Debug.Log("--------------- Start Load Map Cell " + name);
        LabelCellMapGrid.text = X + "x" + Y;

        //if (X == 2 && Y == 1)
        //{
        //if ((X == 1 && Y == 1) ||
        //    (X == 2 && Y == 1) ||
        //    (X == 1 && Y == 2) ||
        //    (X == 2 && Y == 2))
        //if ((X == 1 && Y == 1) ||
        //    (X == 1 && Y == 2) ||
        //    (X == 3 && Y == 1) ||
        //    (X == 12 && Y == 1))
        //{ 
            
        //}
	}

    private void LateUpdate()
    {
        if (!IsFirstLoading)
        {
            IsFirstLoading = true;
            StartCoroutine(LoadSpriteMap());
        }
    }

    // Update is called once per frame
    void Update () {
        
    }

    public void Refresh()
    {
        StartCoroutine(LoadSpriteMap());
    }

    IEnumerator LoadSpriteMap()
    {
        //yield return new WaitForSeconds(1f);

        int offsetX = (X-1);
        int offsetY = (Y-1);

        yield return null;

        Sprite = Storage.Map.GetSpriteMap(Storage.Map.SizeCellMap, false, offsetX, offsetY);

        //yield return new WaitForSeconds(0.5f);
    }

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(0, 0, 100, 100), X + "x" + Y);
    //}



}