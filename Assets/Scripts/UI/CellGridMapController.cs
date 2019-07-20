using System.Collections;
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

    //private bool IsAutoAction = false;

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
        if (!IsFirstLoading && Storage.Map.IsAutoAction)
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

        UpdateSprite();
        //StartCoroutine(LoadSpriteMap());
    }

    IEnumerator LoadSpriteMap()
    {
        //yield return null;

        MapWorld.IsReloadGridMap = true;

        //yield return new WaitForSeconds(1f);

        //int offsetX = (X-1);
        //int offsetY = (Y-1);

        yield return null;

        //Sprite = Storage.Map.GetSpriteMap(Storage.Map.SizeCellMap, false, offsetX, offsetY);

        //yield return new WaitForSeconds(0.5f);

        UpdateSprite();
    }

    Texture2D TextureMap = null;

    private void UpdateSprite()
    {
        int offsetX = (X - 1);
        int offsetY = (Y - 1);

        Sprite = null;
        //#fix mem 3.
        if (TextureMap != null)
            Destroy(TextureMap);



        Sprite = Storage.Map.GetBildSpriteMap(out TextureMap, Storage.Map.SizeCellMap, false, offsetX, offsetY);

        MapWorld.IsReloadGridMap = false;
    }

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(0, 0, 100, 100), X + "x" + Y);
    //}



}
