using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEmp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }


    //private void SaveLayerConstrTileInGridData(string keyStruct, List<DataTile> listTiles, TypesStructure typeCell = TypesStructure.None)
    //{
    //    string fieldStart = Storage.Instance.SelectFieldCursor;
    //    Vector2 posStructFieldStart = Helper.GetPositByField(fieldStart);
    //    Vector2 posStructFieldNew = Helper.GetPositByField(fieldStart);

    //    bool isClearLayer = !m_PasteOnLayer;

    //    int size = (int)Mathf.Sqrt(listTiles.Count) - 1;
    //    foreach (DataTile itemTile in listTiles)
    //    {
    //        if(typeCell != TypesStructure.None)
    //            itemTile.Tag = typeCell.ToString();

    //        //Correct position
    //        posStructFieldNew = posStructFieldStart + new Vector2(itemTile.X, size - itemTile.Y);

    //        string fieldNew = Helper.GetNameField(posStructFieldNew.x, posStructFieldNew.y);

    //        if (isClearLayer)
    //            ClearLayerForStructure(fieldNew);

    //        Storage.GridData.AddConstructInGridData(fieldNew, itemTile, isClearLayer);
    //    }
    //}

    //private void LoadConstructOnPalette(string keyStruct)
    //{
    //    Debug.Log("Selected struct :" + keyStruct);
    //    SelectedConstruction = keyStruct;

    //    if(!Storage.TilesManager.DataMapTiles.ContainsKey(SelectedConstruction))
    //    {
    //        Debug.Log("######### LoadConstructOnPalette: TilesManager.DataMapTiles  Not find SelectedStructure: " + SelectedConstruction);
    //        return;
    //    }

    //    var listTiles = Storage.TilesManager.DataMapTiles[SelectedConstruction];

    //    float col = listTiles.Count;
    //    //sizeCellMap = listTiles.Count;
    //    int countColumnMap = (int)Mathf.Sqrt(col);
    //    m_GridMap.constraintCount = countColumnMap;

    //    foreach(var oldCell in m_listCallsOnPalette)
    //    {
    //        Destroy(oldCell);
    //    }
    //    m_listCallsOnPalette.Clear();

    //    ResizeScaleGrid(countColumnMap);

    //    foreach (DataTile itemTileData in listTiles)
    //    {
    //        string namePrefab = itemTileData.Name;
    //        string nameTexture = itemTileData.Name;

    //        GameObject cellMap = (GameObject)Instantiate(PrefabCellMapPalette);
    //        cellMap.transform.SetParent(this.gameObject.transform);

    //        //Texture2D textureTile = Storage.TilesManager.CollectionTextureTiles[nameTexture];
    //        //Sprite spriteTile = Sprite.Create(textureTile, new Rect(0.0f, 0.0f, textureTile.width, textureTile.height), new Vector2(0.5f, 0.5f), 100.0f);
    //        Sprite spriteTile = Storage.TilesManager.CollectionSpriteTiles[nameTexture];

    //        //cellMap.GetComponent<SpriteRenderer>().sprite = spriteTile;
    //        cellMap.GetComponent<Image>().sprite = spriteTile;
    //        cellMap.GetComponent<CellMapControl>().DataTileCell = itemTileData;
    //        cellMap.SetActive(true);

    //        m_listCallsOnPalette.Add(cellMap);

    //        //Add prefab in World
    //        //GameObject prefabGameObject = Storage.GridData.FindPrefab(namePrefab);
    //    }
    //}

    //private void SaveConstructTileInGridData()
    //{
    //    if(string.IsNullOrEmpty(SelectedConstruction))
    //    {
    //        Storage.Events.SetTittle = "No selected construction";
    //        return;
    //    }

    //    if(!Storage.TilesManager.DataMapTiles.ContainsKey(SelectedConstruction))
    //    {
    //        Storage.Events.SetTittle = "Not exist : " + SelectedConstruction;
    //        Debug.Log("#######  SaveConstructTileInGridData : Not exist SelectedConstruction: " + SelectedConstruction);
    //        return;
    //    }

    //    //string fieldStart = Storage.Instance.SelectFieldPosHero;
    //    string fieldStart = Storage.Instance.SelectFieldCursor;
    //    var listTiles = Storage.TilesManager.DataMapTiles[SelectedConstruction];

    //    Vector2 posStructFieldStart = Helper.GetPositByField(fieldStart);
    //    Vector2 posStructFieldNew = Helper.GetPositByField(fieldStart);

    //    bool isClearLayer = !m_PasteOnLayer;

    //    int size = (int)Mathf.Sqrt(listTiles.Count) - 1;
    //    foreach (DataTile itemTile in listTiles)
    //    {
    //        //Correct position
    //        posStructFieldNew = posStructFieldStart + new Vector2(itemTile.X, size - itemTile.Y);

    //        string fieldNew = Helper.GetNameField(posStructFieldNew.x, posStructFieldNew.y);

    //        if (isClearLayer)
    //            ClearLayerForStructure(fieldNew);

    //        Storage.GridData.AddConstructInGridData(fieldNew, itemTile, isClearLayer);
    //    }

    //}

    //public void LoadGridTiles_Test()
    //{

    //    int countFindTiles = 0;
    //    //--- TAILS ---
    //    //GameObject BackPalette;
    //    // Grid GridTails;
    //    //Layer Back
    //    //public GameObject TailsMap;
    //    //GridTails.
    //    //TailsMap.


    //    string NameStructMap = "Bild1";

    //    CollectionDataMapTales = new Dictionary<string, List<DataTile>>();
    //    List<DataTile> listDataTiles = new List<DataTile>();


    //    Tilemap tilemap = TailsMap.GetComponent<Tilemap>();

    //    BoundsInt bounds = tilemap.cellBounds;
    //    TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

    //    for (int x = 0; x < bounds.size.x; x++)
    //    {
    //        for (int y = 0; y < bounds.size.y; y++)
    //        {
    //            TileBase tile = allTiles[x + y * bounds.size.x];
    //            if (tile != null)
    //            {
    //                //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
    //                //int cellX = x + bounds.x;
    //                //int cellY = y + bounds.y;
    //                int cellX = x;// + bounds.x;
    //                int cellY = y;// + bounds.y;

    //                DataTile dataTiles = new DataTile()
    //                {
    //                    //X = x,
    //                    //Y = y,
    //                    X = cellX,
    //                    Y = cellY,
    //                    NameTales = tile.name
    //                };

    //                listDataTiles.Add(dataTiles);
    //                countFindTiles++;
    //            }
    //            else
    //            {
    //                //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
    //            }
    //        }
    ////    }
    ////}
    //    }

    //    CollectionDataMapTales.Add(NameStructMap, listDataTiles);

    //    foreach(var item in CollectionDataMapTales)
    //    {
    //        foreach(DataTile tileData in CollectionDataMapTales[item.Key])
    //        {
    //            Storage.Events.ListLogAdd = "DataTile : " + tileData.NameTales + " " + tileData.X + "x" + tileData.Y;
    //            Debug.Log("DataTile : " + tileData.NameTales + " " + tileData.X + "x" + tileData.Y);
    //        }
    //    }

    //    //foreach (var itemT in allTiles)
    //    //{
    //    //    if(itemT!=null)
    //    //        Debug.Log("Tile info: " + itemT.name + "    " + itemT.GetInstanceID() + " -- " + itemT.ToString() ); // + itemT.GetTileData.position);
    //    //}

    //    Storage.Events.ListLogAdd = "Count fid liles: " + countFindTiles;

    //    //---------------------------
    //    //In this case if you have, suppose, custom tile TileRoad, inherited from Tile or TileBase, then you can get all TileRoad tiles with call:
    //    //TileBase[] = tilemap.GetTiles<RoadTile>();

    //}

    //public void CreateDataTiles()
    //{
    //    CreateStructDataTile("BildTest1", new BoundsInt(new Vector3Int(0, 0, 0), new Vector3Int(3, 3, 0)));


    //    foreach (var item in CollectionDataMapTales)
    //    {
    //        Storage.Events.ListLogAdd = "Structure : " + item.Key;
    //        foreach (DataTile tileData in CollectionDataMapTales[item.Key])
    //        {
    //            Storage.Events.ListLogAdd = "DataTile : " + tileData.NameTales + " " + tileData.X + "x" + tileData.Y;
    //            Debug.Log("DataTile : " + tileData.NameTales + " " + tileData.X + "x" + tileData.Y);
    //        }
    //    }

    //}

    //private void CalculatePointOnMap_()
    //{
    //    bool isLog = false;
    //    Camera cameraMap = Storage.PlayerController.CameraMap;
    //    if (cameraMap == null)
    //    {
    //        Debug.Log("################ cameraMap is EMPTY ");
    //        return;
    //    }
    //    if (!cameraMap.enabled)
    //        return;

    //    //cameraMap

    //    HitTextMousePointOnEbject();

    //    bool isHitCollider = false;
    //    float mapX = 0;
    //    float mapY = 0;
    //    Ray ray1 = cameraMap.ScreenPointToRay(Input.mousePosition);
    //    //RaycastHit2D hit1 = Physics2D.GetRayIntersection(ray1, Mathf.Infinity);
    //    RaycastHit2D hit1 = Physics2D.GetRayIntersection(ray1, 15f);

    //    if (hit1.collider != null && hit1.collider.transform == this.gameObject.transform)
    //    {
    //        BoxCollider2D colliderMap = GetComponent<BoxCollider2D>();
    //        if (colliderMap != null)
    //        {

    //            isHitCollider = true;
    //            NormalizedMapPoint(hit1, colliderMap, out mapX, out mapY);

    //            if (isLog)
    //                Storage.Events.ListLogAdd = "MAP ORIGINAL: pos = " + mapX + "x" + mapY + "  Zoom: " + SizeZoom;

    //            if (SizeZoom == 1)
    //            {

    //            }
    //            else if (SizeZoom > 1f)
    //            {

    //                float _zoom = SizeZoom;

    //                #region Test
    //                //TEST --------------------
    //                //float mapY_T1 = (int)mapY;
    //                //float mapX_T1 = (int)mapX;
    //                //mapY_T1 = (int)(mapY_T1 / _zoom);
    //                //mapX_T1 = (int)(mapX_T1 / _zoom);

    //                //Storage.Events.ListLogAdd = "Corrr zoom T1= " + (int)mapX_T1 + "x" + (int)mapY_T1;

    //                //float mapY_T2 = (int)(mapY / _zoom);
    //                //float mapX_T2 = (int)(mapX / _zoom);

    //                //Storage.Events.ListLogAdd = "Corrr zoom T2= " + (int)mapX_T2 + "x" + (int)mapY_T2;
    //                //---------------------------
    //                #endregion

    //                mapY = (mapY / _zoom);
    //                mapX = (mapX / _zoom);

    //                _zoom = (float)System.Math.Round(_zoom, 1);

    //                float offsetCenter = 0f;

    //                //Debug.Log("_zoom===" + _zoom);
    //                offsetCenter = OffsetZoomUp(_zoom);

    //                if (isLog)
    //                    Storage.Events.ListLogAdd = "Corrr zoom: " + (int)mapX + "x" + (int)mapY + "  offset= " + offsetCenter + " zoom: " + _zoom;

    //                //mapX = (int)mapX;
    //                //mapY = (int)mapY;

    //                //!!! CORR DISTANCE
    //                int centrW = Helper.HeightLevel / 2;
    //                Vector3 centerPos = new Vector3(centrW, centrW, 0);
    //                float koofOnCenterX = centerPos.x / mapX;
    //                float koofOnCenterY = centerPos.y / mapY;
    //                if (isLog)
    //                    Storage.Events.ListLogAdd = "Map koofOnCenter: " + koofOnCenterX + " x " + koofOnCenterY;
    //                //------------------

    //                mapX += offsetCenter;
    //                mapY += offsetCenter;
    //            }
    //            else
    //            {
    //                float _zoom = SizeZoom;

    //                //mapX = (int)mapX;
    //                //mapY = (int)mapY;

    //                mapY = (int)(mapY / _zoom);
    //                mapX = (int)(mapX / _zoom);

    //                float offsetCenter = OffsetZoomDown(_zoom);

    //                if (isLog)
    //                    Storage.Events.ListLogAdd = "Corrr zoom:  " + (int)mapX + "x" + (int)mapY + "  offsetCenter= " + offsetCenter;

    //                mapX += offsetCenter;
    //                mapY += offsetCenter;
    //            }

    //            if (isLog)
    //                Storage.Events.ListLogAdd = "MAP pos = " + mapX + "x" + mapY;
    //            //Debug.Log("MAP pos = " + mapX + "x" + mapY);
    //        }
    //        else
    //        {
    //            //---------------- TEST
    //            if (TestHasPoint != new Vector2())
    //            {
    //                Debug.Log("@@@@@@@@@@@@@@ 3. do great stuff : TestHasPoint:" + TestHasPoint.x + "x" + TestHasPoint.y);
    //                Storage.Events.ListLogAdd = "3. Physics2D.OverlapPoint(TestHasPoint) : " + TestHasPoint.x + "x" + TestHasPoint.y;
    //                TestHasPoint = new Vector2();
    //            }
    //            //---------------- TEST
    //            Vector2 mousePositionTest = cameraMap.ScreenToWorldPoint(Input.mousePosition);
    //            if (Physics2D.OverlapPoint(mousePositionTest))
    //            {
    //                Debug.Log(">>>>>>>>>>>>>>>>>> 1. do great stuff : mousePosition:" + mousePositionTest.x + "x" + mousePositionTest.y);
    //                Storage.Events.ListLogAdd = "1. Physics2D.OverlapPoint(mousePosition) : " + mousePositionTest.x + "x" + mousePositionTest.y;
    //                //Storage.Events.ListLogAdd = "MAP mousePosition:" + mousePosition.x + "x" + mousePosition.y;
    //                //do great stuff
    //            }
    //            //----------------
    //        }
    //    }
    //    else
    //    {
    //        //---------------- TEST
    //        if (TestHasPoint != new Vector2())
    //        {
    //            Debug.Log("@@@@@@@@@@@@@@ 4. do great stuff : TestHasPoint:" + TestHasPoint.x + "x" + TestHasPoint.y);
    //            Storage.Events.ListLogAdd = "4. Physics2D.OverlapPoint(TestHasPoint) : " + TestHasPoint.x + "x" + TestHasPoint.y;
    //            TestHasPoint = new Vector2();
    //        }
    //        //---------------- TEST
    //        Vector2 mousePositionTest = cameraMap.ScreenToWorldPoint(Input.mousePosition);
    //        if (Physics2D.OverlapPoint(mousePositionTest))
    //        {
    //            Debug.Log(">>>>>>>>>>>>>>>>>> 2. do great stuff : mousePosition:" + mousePositionTest.x + "x" + mousePositionTest.y);
    //            Storage.Events.ListLogAdd = "2. Physics2D.OverlapPoint(mousePosition) : " + mousePositionTest.x + "x" + mousePositionTest.y;
    //            //Storage.Events.ListLogAdd = "MAP mousePosition:" + mousePosition.x + "x" + mousePosition.y;
    //            //do great stuff
    //        }
    //        //----------------

    //        if (!IsRuntimeViewMarker)
    //        {
    //            if (isLog)
    //                Storage.Events.ListLogAdd = "CalculatePointOnMap Not hit collider " + (int)mapX + "x" + (int)mapY;
    //        }
    //    }

    //    SelectPointField = new Vector2(mapX, mapY);
    //    SelectFieldPos = new Vector2((int)mapX, (int)mapY);

    //    Storage.Map.SelectPointField = SelectPointField;

    //    //if (Storage.Map.SelectFieldMap == "Field0x0")
    //    //{
    //    //    Debug.Log("######### CalculatePointOnMap FIELD=Field0x0     isHitCollider=" + isHitCollider);
    //    //}

    //    Storage.Map.UpdateMarkerPointCell();
    //}

    //private static float OffsetZoomDown(float _zoom)
    //{
    //    float offsetCenter = 0;

    //    if (_zoom >= 0.8f)
    //        offsetCenter = -12f;
    //    //if (_zoom >= 0.85f)
    //    //    offsetCenter = -7f;
    //    //if (_zoom >= 0.95f)
    //    //    offsetCenter = -4f;
    //    if (_zoom >= 0.9f)
    //        offsetCenter = -5f;
    //    return offsetCenter;
    //}

    //private static float OffsetZoomUp(float _zoom)
    //{
    //    float offsetCenter = 0;
    //    if (_zoom >= 1f)
    //        offsetCenter = 0;
    //    if (_zoom >= 1.1f)
    //        offsetCenter = 4f;// 5f;
    //    if (_zoom >= 1.2f)
    //        offsetCenter = 8f; // 10f;
    //    if (_zoom >= 1.3f)
    //        offsetCenter = 11f;// 12f;
    //    if (_zoom >= 1.4f)
    //        offsetCenter = 14f; //15f
    //    if (_zoom >= 1.5f)
    //        offsetCenter = 16f;
    //    if (_zoom >= 1.6f)
    //        offsetCenter = 18f;
    //    if (_zoom >= 1.7f)
    //        offsetCenter = 20f;
    //    if (_zoom >= 1.8f)
    //        offsetCenter = 22f;
    //    if (_zoom >= 1.9f)
    //        offsetCenter = 23f;
    //    //if ((int)_zoom >= 2f)
    //    //    offsetCenter = 25.5f;// 25f;
    //    if (_zoom >= 2f)
    //        offsetCenter = 25.5f;// 25f;
    //    if (_zoom >= 2.1f)
    //        offsetCenter = 26.5f;// 25f;
    //    if (_zoom >= 2.2f)
    //        offsetCenter = 28f;// 25f;
    //    if (_zoom >= 2.3f)
    //        offsetCenter = 28.5f;// 25f;
    //    if (_zoom >= 2.4f)
    //        offsetCenter = 29.5f;// 25f;
    //    if (_zoom >= 2.5f)
    //        offsetCenter = 30f;// 25f;
    //    return offsetCenter;
    //}


    //public float OffSetZomm1 = 0f;
    //public float OffSetZomm11 = 4f;
    //public float OffSetZomm12 = 8f;
    //public float OffSetZomm13 = 11f;
    //public float OffSetZomm14 = 14f;
    //public float OffSetZomm15 = 16f;
    //public float OffSetZomm16 = 18f;
    //public float OffSetZomm17 = 20f;
    //public float OffSetZomm18 = 22f;
    //public float OffSetZomm19 = 23f;
    //public float OffSetZomm2 = 25.5f;
    //public float OffSetZomm21 = 26.5f;
    //public float OffSetZomm22 = 28f;
    //public float OffSetZomm23 = 28.5f;
    //public float OffSetZomm24 = 29.5f;
    //public float OffSetZomm25 = 30f;

    //public float OffSetZomm08 = -12f;
    //public float OffSetZomm09 = -5f;

    //private void SetLocationCell()
    //{
    //    //------------ Location cell
    //    Vector2 movementCell = new Vector3(-12.4f, 12.4f);

    //    //movementCell += new Vector2(SelectPointField.x / Storage.ScaleWorld, SelectPointField.y / Storage.ScaleWorld * (-1));

    //    //MapCellFrame.transform.position = movementCell;
    //    //MapCellFrame.GetComponent<RectTransform>().rect.left = movementCell.x;
    //    //MapCellFrame.GetComponent<RectTransform>().rect.top = movementCell.y;
    //    //MapCellFrame.GetComponent<RectTransform>().position.x = movementCell.x;
    //    //MapCellFrame.GetComponent<RectTransform>().position.y = movementCell.y;

    //    //MapCellFrame.GetComponent<RectTransform>().position = new Vector3(movementCell.x, movementCell.y, 0);

    //    //if(posOld == new Vector3())
    //    //    posOld = MapCellFrame.GetComponent<RectTransform>().position;// = new Vector3(-2, -2, 0);
    //    //Vector3 newPos = new Vector3(posOld.x + (SelectPointField.x / Storage.ScaleWorld), posOld.y - (SelectPointField.y / Storage.ScaleWorld), -10);

    //    //!!!!!!!!!!!!!!!
    //    //@ >>>
    //    //MapCellFrame.transform.SetParent(null);
    //    //MapCellFrame.SetActive(false);
    //    //MapCellFrame.transform.SetParent(this.gameObject.transform);
    //    //MapCellFrame.SetActive(true);
    //    //!!!!!!!!!!!!!!!



    //    if (posOld == new Vector3())
    //        posOld = MapCellFrame.GetComponent<RectTransform>().position;// = new Vector3(-2, -2, 0);

    //    int _koofPosCell = 2;

    //    //float correctPosX = posOld.x + (SelectPointField.x / Storage.ScaleWorld);
    //    //float correctPosY = posOld.y - (SelectPointField.y / Storage.ScaleWorld);
    //    float addX = (SelectPointField.x / (Storage.ScaleWorld * _koofPosCell));
    //    float addY = (SelectPointField.y / (Storage.ScaleWorld * _koofPosCell));

    //    float correctPosX = posOld.x + addX;
    //    float correctPosY = posOld.y - addY;
    //    //float correctPosX = posOld.x + SelectPointField.x;
    //    //float correctPosY = posOld.y - SelectPointField.y;


    //    float correctZomm = SizeZoom;

    //    //if(correctZomm>1)
    //    //{
    //    //    correctZomm *= (1.1f + Storage.PlayerController.DistMoveCameraMapXY);
    //    //}
    //    if (correctZomm > 1)
    //    {
    //        Debug.Log("Save normal posit");
    //    }

    //    //correctPosX /= correctZomm;
    //    //correctPosY /= correctZomm;

    //    correctPosX -= Storage.Map.DistMoveCameraMap.x;
    //    correctPosY -= Storage.Map.DistMoveCameraMap.y;

    //    Vector3 newPos = new Vector3(correctPosX, correctPosY, -10);

    //    ValidateStartPosition();

    //    //Correct Offset Zoom
    //    if (correctZomm > 1)
    //    {
    //        //float distanceSenter = Vector3.Distance(Storage.Map.StartPositFrameMap, newPos);
    //        //float offSetOnCenterX = Storage.Map.StartPositFrameMap.x - newPos.x;
    //        //float offSetOnCenterY = Storage.Map.StartPositFrameMap.y - newPos.y;
    //        //float offSetOnCenterX = Storage.Map.StartPositFrameMap.x - newPos.x;
    //        //float offSetOnCenterY = Storage.Map.StartPositFrameMap.y - newPos.y;
    //        int x = (int)SelectPointField.x;
    //        int y = (int)SelectPointField.y;
    //        //int sizeW = Helper.HeightLevel;
    //        int centrW = Helper.HeightLevel / 2;
    //        Vector3 centerPos = new Vector3(centrW, centrW, 0);
    //        //if (x> centrW)
    //        //float distanceSenter = Vector3.Distance(SelectPointField, centerPos);
    //        float offSetOnCenterX = centerPos.x - x;
    //        float offSetOnCenterY = centerPos.y - y;

    //        float koofOnCenterX = centerPos.x / x;
    //        float koofOnCenterY = centerPos.y / y;
    //        if (koofOnCenterX < 0)
    //            koofOnCenterX += 1;
    //        if (koofOnCenterY < 0)
    //            koofOnCenterY += 1;

    //        //if (correctZomm >= 1.1f) {
    //        //    offSetOnCenterX = OffsetCell11;
    //        //    offSetOnCenterY = OffsetCell11;
    //        //}
    //        Storage.Events.ListLogAdd = "New pos Cell: " + newPos.x + "x" + newPos.y;
    //        Storage.Events.ListLogAdd = "-- Cell koof: " + koofOnCenterX + "x" + koofOnCenterY;
    //        //Storage.Events.ListLogAdd = "-- Cell offset: " + offSetOnCenterX + "x" + offSetOnCenterY;

    //        //newPos.x += offSetOnCenterX;
    //        //newPos.y += offSetOnCenterY;
    //        float korrCellX = offSetOnCenterX / 100;
    //        float korrCellY = offSetOnCenterY / 100;

    //        //OffsetCell11
    //        float OffsetCell = 1f;

    //        OffsetCell = GetOffsetCell(correctZomm);
    //        //if (correctZomm >= 1.1f)
    //        //    OffsetCell = OffsetCell11;
    //        //if (correctZomm >= 1.2f)
    //        //    OffsetCell = OffsetCell11;

    //        Storage.Events.ListLogAdd = ":: OffsetCell corr: % " + OffsetCell;

    //        korrCellX *= OffsetCell;
    //        korrCellY *= OffsetCell;

    //        //string operCorrX = "";
    //        //string operCorrY = "";

    //        //if (centerPos.x > x)
    //        //{
    //            newPos.x -= korrCellX;
    //            //operCorrX = "-";
    //        //}
    //        //else{
    //        //    newPos.x += korrCellX;
    //        //}

    //        //if (centerPos.y > y)
    //        //{
    //        //    operCorrY = "-";
    //            newPos.y += korrCellY;
    //        //}
    //        //else
    //        //{
    //        //    newPos.y += korrCellY;
    //        //}

    //        //newPos.x = (int)newPos.x;
    //        //newPos.y = (int)newPos.y;

    //        Storage.Events.ListLogAdd = ":: Cell corr: " + korrCellX + " x " + korrCellY;
    //        //correctZomm *= (1.1f + Storage.PlayerController.DistMoveCameraMapXY);
    //    }

    //    MapCellFrame.GetComponent<RectTransform>().position = newPos;
    //    Storage.Events.ListLogAdd = "***** Cell pos: " + newPos.x + "x" + newPos.y;
    //    //<<< @
    //    //MapCellFrame.transform.SetParent(this.gameObject.transform);
    //    //----------------------------
    //}


    //private void OnMauseWheel()
    //{
    //    float wheel = Input.GetAxis("Mouse ScrollWheel");

    //    if (wheel != 0) // back
    //    {
    //        //Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 1);
    //        if (wheel > 0)
    //        {
    //            //--For TEST---
    //            //upLevel += speedWheel;
    //            //SizeZoom += (0.06f + upLevel);

    //            //if (SizeZoom >= 1)
    //            //{
    //            SizeZoom += 0.1f;
    //            //}
    //            //else
    //            //{
    //            //    SizeZoom += 0.05f;
    //            //}


    //            if (SizeZoom > limitZoomMax)
    //                SizeZoom = limitZoomMax;
    //        }
    //        else
    //        {
    //            //--For TEST---
    //            //    upLevel -= speedWheel;
    //            //    if (upLevel < 0)
    //            //        upLevel = 0;
    //            //    SizeZoom -= (0.06f + upLevel);
    //            //if (SizeZoom >= 1)
    //            //{
    //            SizeZoom -= 0.1f;
    //            //}
    //            //else
    //            //{
    //            //    SizeZoom -= 0.05f;
    //            //}

    //            if (SizeZoom < limitZoomMin)
    //                SizeZoom = limitZoomMin;
    //        }
    //        Zooming(SizeZoom);
    //    }
    //    //if (Input.GetAxis("Mouse ScrollWheel") & gt; 0) // forward
    //    // {
    //    //    Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize - 1, 6);
    //    //}
    //}

    //public void DrawTrack(List<Vector3> trackPoints, Color colorTrack)
    //{
    //    Debug.Log("DrawTrack Storage ...........");

    //    //DrawTrack2(trackPoints, colorTrack);
    //    //    return;

    //    LineRenderer lineRenderer = GetComponent<LineRenderer>();
    //    if (lineRenderer == null)
    //    {
    //        Debug.Log("LineRenderer is null !!!!");
    //        return;
    //    }

    //    lineRenderer.startColor = colorTrack;
    //    lineRenderer.endColor = colorTrack;

    //    //lineRenderer.StartColor(Color.green, Color.green);

    //    //lineRenderer.SetWidth(0.2F, 0.2F);
    //    lineRenderer.startWidth = 0.2F;
    //    lineRenderer.endWidth = 0.2F;

    //   //int size = 2;
    //    //lineRenderer.SetVertexCount(size);
    //    lineRenderer.positionCount = trackPoints.Count();// size;


    //    //Vector3 pos1 = new Vector3(posTrack.x, posTrack.y, -2);
    //    //lineRenderer.SetPosition(0, pos1);
    //    //Vector3 pos2 = new Vector3(posTrack.x + 2, posTrack.y + 2, -2);
    //    //lineRenderer.SetPosition(1, pos2);
    //    //Vector3 pos3 = new Vector3(posTrack.x - 2, posTrack.y + 2, -2);
    //    //lineRenderer.SetPosition(2, pos3);
    //    //Vector3 pos4 = new Vector3(posTrack.x - 2, posTrack.y - 2, -2);
    //    //lineRenderer.SetPosition(3, pos4);
    //    //Vector3 pos5 = new Vector3(posTrack.x + 2, posTrack.y - 2, -2);
    //    //lineRenderer.SetPosition(4, pos5);
    //    //Vector3 pos6 = new Vector3(posTrack.x, posTrack.y, -2);
    //    //lineRenderer.SetPosition(5, pos6);
    //    string indErr = "";
    //    try
    //    {
    //        for (int i = 0; i < trackPoints.Count(); i++)
    //        {
    //            indErr = i.ToString();
    //            Vector3 posNext = new Vector3(trackPoints[i].x, trackPoints[i].y, -2);
    //            indErr += "pos1=" + posNext.x + "x" + posNext.y;
    //            if (lineRenderer != null)
    //            {
    //                //Debug.Log("?????? DrawTrack lineRenderer i =" + i + "   posNext=" + posNext);
    //                lineRenderer.SetPosition(i, posNext);
    //            }
    //            else
    //                Debug.Log("####### DrawTrack lineRenderer is null");
    //        }
    //    }catch(Exception x)
    //    {
    //        Debug.Log("####### Error DrawTrack (" + indErr + ") : " + x.Message);
    //    }

    //}

    //private void CreateTypesBoss()
    //{
    //    _typesBoss = new List<TypeBoss>()
    //    {
    //         new TypeBoss(){ NameTextura2D= "SpriteBossLizard" },
    //        new TypeBoss(){ NameTextura2D=  "SpriteBossRed" },
    //        new TypeBoss(){ NameTextura2D=  "SpriteBossBandos" },
    //        new TypeBoss(){ NameTextura2D=  "SpriteBossBooble" },
    //        new TypeBoss(){ NameTextura2D=  "SpriteBossAlien" },
    //        new TypeBoss(){ NameTextura2D=  "SpriteBossDroid" },
    //        new TypeBoss(){ NameTextura2D= "SpriteBossArm" },
    //        new TypeBoss(){ NameTextura2D= "SpriteBoss" },
    //        new TypeBoss(){ NameTextura2D= "SpriteBoss" },
    //        new TypeBoss(){ NameTextura2D= "SpriteBoss" },
    //        new TypeBoss(){ NameTextura2D= "SpriteBoss" },
    //        new TypeBoss(){ NameTextura2D= "SpriteBoss" },
    //        new TypeBoss(){ NameTextura2D= "SpriteBoss" },
    //        new TypeBoss(){ NameTextura2D= "SpriteBoss" },
    //        new TypeBoss(){ NameTextura2D= "SpriteBoss" },
    //    };

    //    //NemesSpritesBoss = new string[]
    //    //{
    //    //    "SpriteBossLizard",
    //    //    "SpriteBossRed",
    //    //    "SpriteBossBandos",
    //    //    "SpriteBossBooble",
    //    //    "SpriteBossAlien",
    //    //    "SpriteBossDroid",
    //    //    "SpriteBossArm",
    //    //    "SpriteBoss",
    //    //    "SpriteBoss",
    //    //    "SpriteBoss",
    //    //    "SpriteBoss",
    //    //    "SpriteBoss",
    //    //    "SpriteBoss",
    //    //    "SpriteBoss",
    //    //    "SpriteBoss",
    //    //};
    //}


    //private string[] NemesSpritesBoss = new string[]
    //{
    //    "SpriteBossLizard",
    //    "SpriteBossRed",
    //    "SpriteBossBandos",
    //    "SpriteBossBooble",
    //    "SpriteBossAlien",
    //    "SpriteBossDroid",
    //    "SpriteBossArm",
    //    "SpriteBoss",
    //    "SpriteBoss",
    //    "SpriteBoss",
    //    "SpriteBoss",
    //    "SpriteBoss",
    //    "SpriteBoss",
    //    "SpriteBoss",
    //    "SpriteBoss",
    //};


    //<Field>
    //  <Key>Field43x32</Key>
    //  <Value>
    //    <Objects>
    //      <ObjectData xsi:type="Ufo">
    //        <NameObject>PrefabUfo_Field41x38_20b2</NameObject>
    //        <TagObject>PrefabUfo</TagObject>
    //        <Position>
    //          <x>83.3700943</x>
    //          <y>-76.03779</y>
    //          <z>-2</z>
    //        </Position>
    //      </ObjectData>
    //    </Objects>
    //    <NameField>Field43x32</NameField>
    //  </Value>
    //</Field>

    //-------------
    //123456789
    //private void CreateDataGamesObjectsWorld(bool isAlwaysCreate = false)
    //       _scriptNPC.SartCrateNPC();
    //LoadObjectsNearHero();
    //@POS@
    //---------------


    //G
    //LoadObjectToReal:  DATA -->> PERSONA #P#
    //+++ CreateObjectData +++ LoadObjectForLook
    //private GameObject CreatePrefabByName(SaveLoadData.ObjectData objData)

    //G
    //RemoveRealObjects Update DATA <<<<------- PERSONA  #P#
    //private void SaveListObjectsToData(string p_nameField)
    //S -- CreateObjectData

    //G
    //private void SaveNewGameObjectToData(string p_nameField, GameObject p_saveObject)
    //---> SaveLoadData.CreateObjectData(p_saveObject, true); NEW DATA  -------->>>>  PERSONA  #P#

    //S
    //+++ CreatePrefabByName +++
    //public static ObjectData CreateObjectData(GameObject p_gobject, bool isNewGen = false)
    //NEW DATA  -------->>>>  PERSONA  #P#
    //newObject.UpdateGameObject(p_gobject);
    //RemoveRealObjects Update DATA <<<<------- PERSONA  #P#
    //newObject = personalData.PersonalObjectData.Clone() as ObjectDataUfo;



    //------------------------------------------

    //public void GenGrigLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    //{
    //    int gridWidth = 100;
    //    int gridHeight = 100;
    //    //gridWidth = (int)GridX;
    //    //gridHeight = (int)GridY;

    //    int maxVertical = (int)p_limitVerticalLook + 1;// *-1;
    //    int maxHorizontal = (int)p_limitHorizontalLook + 1;

    //    if (Fields.Count != _counter || _counter == 0)
    //    {
    //        //DebugLogT("--------- Cancel GenGrigLook");
    //        return;
    //    }
    //    else
    //    {
    //        //Debug.Log("Start GenGrigLook count: " + Fields.Count);
    //    }

    //    if (_movement.x != 0)
    //    {
    //        int p_startPosY = p_PosHeroY - (p_limitVerticalLook / 2);
    //        //Validate
    //        if (p_startPosY < 0)
    //        {
    //            //Debug.Log("GenGrigLook --!!!--- not validate startPosY=" + p_startPosY + 
    //            //    " p_PosHeroY=" + p_PosHeroY  +
    //            //    " p_limitVerticalLook=" + p_limitVerticalLook +
    //            //    "        startPosY = p_PosHeroY + (p_limitVerticalLook / 2);");
    //            p_startPosY = 0;
    //        }
    //        int limitVertical = p_startPosY + maxVertical;
    //        if (limitVertical > gridHeight)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate limitVertical=" + limitVertical);
    //            limitVertical = gridHeight;
    //        }

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int x = 0;
    //        int LeftX = p_PosHeroX - (p_limitHorizontalLook / 2);
    //        int RightX = p_PosHeroX + (p_limitHorizontalLook / 2);
    //        int LeftRemoveX = LeftX - 1;
    //        int RightRemoveX = RightX + 1;
    //        //Validate
    //        if (LeftRemoveX < 0)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate LeftRemoveX=" + LeftRemoveX);
    //            if (_movement.x > 0)
    //                isRemove = false;
    //        }
    //        if (LeftX < 0)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate LeftX=" + LeftX);
    //            LeftX = 0;
    //            if (_movement.x < 0)
    //                isAdded = false;
    //        }
    //        if (RightRemoveX > gridWidth)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate RightRemoveX=" + RightRemoveX);
    //            if (_movement.x < 0)
    //                isRemove = false;
    //        }
    //        if (RightX > gridWidth)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate RightX=" + RightX);
    //            RightX = gridWidth;
    //            if (_movement.x > 0)
    //                isAdded = false;
    //        }

    //        //DebugLogT("GenGrigLook movement X");

    //        //Debug.Log("GenGrigLook >> Remove Vertical  p_startPosY=" + p_startPosY + "  >>>>>  limitVertical=" + limitVertical);
    //        //Debug.Log("p_startPosY = p_PosHeroY - (p_limitVerticalLook / 2)" +
    //        //    "  p_startPosY =" + p_startPosY +
    //        //    "  p_PosHeroY =" + p_PosHeroY +
    //        //    "  p_limitVerticalLook =" + p_limitVerticalLook);
    //        //Debug.Log("limitVertical = p_startPosY + maxVertical" +
    //        //    "  limitVertical =" + limitVertical +
    //        //    "  p_startPosY=" + p_startPosY +
    //        //    "  maxVertical=" + maxVertical);

    //        if (!isRemove)
    //        {
    //            //Debug.Log("GenGrigLook Not Remove Vertical ");
    //        }
    //        else
    //        {
    //            //x = _movement.x > 0 ?
    //            //    //Remove Vertical
    //            //LeftX :
    //            //RightX;
    //            x = _movement.x > 0 ?
    //                //Remove Vertical
    //            LeftRemoveX :
    //            RightRemoveX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;
    //                //Debug.Log("GenGrigLook Remove Vertical --- " + nameFiled);
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                {
    //                    //DebugLogT("GenGrigLook Remove Vertical not Field : " + nameFiled);
    //                    continue;
    //                }
    //                GameObject findFiled = Fields[nameFiled];

    //                //Destroy !!!
    //                // Kills the game object in 5 seconds after loading the object
    //                //Debug.Log("GenGrigLook Destroy");
    //                Destroy(findFiled, 0.5f);
    //                //Debug.Log("GenGrigLook Fields.Remove");
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //                //Debug.Log("GenGrigLook Remove Vertical +++ " + nameFiled);
    //            }
    //            //DebugLogT("GenGrigLook Remove Vertical +++ " + _nameFiled);
    //        }

    //        if (!isAdded)
    //        {
    //            //DebugLogT("GenGrigLook Not Added Vertical ");
    //        }
    //        else
    //        {
    //            x = _movement.x > 0 ?
    //                //Added Vertical
    //                RightX :
    //                LeftX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;
    //                //Debug.Log("GenGrigLook Added Vertical --- " + nameFiled);

    //                if (Fields.ContainsKey(nameFiled))
    //                {
    //                    // Debug.Log("GenGrigLook Added Vertical YES Field ))) : " + nameFiled);
    //                    continue;
    //                }
    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                //DebugLogT("Field Added  name init : " + nameFiled);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //                //Debug.Log("GenGrigLook Added Vertical +++ " + nameFiled);
    //            }
    //            //DebugLogT("GenGrigLook Added Vertical +++ " + _nameFiled);
    //        }
    //    }

    //    if (_movement.y != 0)
    //    {
    //        //DebugLogT("GenGrigLook movement Y");

    //        int p_startPosX = p_PosHeroX - (p_limitHorizontalLook / 2); //#
    //        //Validate
    //        if (p_startPosX < 0)
    //        {
    //            //Debug.Log("GenGrigLook --!!!--- not validate startPosX=" + p_startPosX);
    //            p_startPosX = 0;
    //        }

    //        int limitHorizontal = p_startPosX + maxHorizontal;
    //        if (limitHorizontal > gridWidth)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate Horizontal=" + limitHorizontal);
    //            limitHorizontal = gridWidth;
    //        }

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int y = 0;
    //        //int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
    //        //int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
    //        int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
    //        int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
    //        int TopRemoveY = TopY - 1;
    //        int DownRemoveY = DownY + 1;
    //        //Debug.Log("GenGrigLook PosHeroY=" + p_PosHeroY);
    //        //Debug.Log("GenGrigLook TopY=" + TopY);
    //        //Debug.Log("GenGrigLook DownY=" + DownY);

    //        //Validate
    //        if (TopRemoveY < 0)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate TopRemoveY=" + TopRemoveY);
    //            if (_movement.y < 0)
    //                isRemove = false;
    //        }
    //        if (TopY < 0)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate TopY=" + TopY);
    //            TopY = 0;
    //            if (_movement.y > 0)
    //                isAdded = false;
    //        }
    //        if (DownRemoveY > gridHeight)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate DownRemoveY=" + DownRemoveY);
    //            if (_movement.y > 0)
    //                isRemove = false;
    //        }
    //        if (DownY > gridHeight)
    //        {
    //            //DebugLogT("GenGrigLook --!!!--- not validate DownY=" + DownY);
    //            DownY = gridHeight;
    //            if (_movement.y < 0)
    //                isAdded = false;
    //        }

    //        if (!isRemove)
    //        {
    //            //DebugLogT("GenGrigLook Not Remove Horizontal ");
    //        }
    //        else
    //        {
    //            //    //Remove Horizontal //#
    //            //    TopY :
    //            //    DownY; //#
    //            y = _movement.y < 0 ?
    //                //Remove Horizontal //#
    //                TopRemoveY :
    //                DownRemoveY; //#

    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                //Debug.Log("GenGrigLook Remove Horizontal --- " + nameFiled);
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                {
    //                    //DebugLogT("GenGrigLook Remove Vertical not Field : " + nameFiled + "   Fields cpont: " + Fields.Count);
    //                    continue;
    //                }
    //                GameObject findFiled = Fields[nameFiled];

    //                //Destroy !!!
    //                // Kills the game object in 5 seconds after loading the object
    //                //Debug.Log("GenGrigLook Destroy");
    //                Destroy(findFiled, 0.5f);
    //                //Debug.Log("GenGrigLook Fields.Remove");
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //                //DebugLogT("GenGrigLook Removed Horizontal +++ " + nameFiled);
    //            }
    //        }

    //        if (!isAdded)
    //        {
    //            //DebugLogT("GenGrigLook Not Added Horizontal ");
    //        }
    //        else
    //        {
    //            //y = _movement.y > 0 ?
    //            y = _movement.y < 0 ?
    //                //Added Horizontal
    //                DownY :
    //                TopY; //#
    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                //Debug.Log("GenGrigLook Added Horizontal --- " + nameFiled);

    //                if (Fields.ContainsKey(nameFiled))
    //                {
    //                    // DebugLogT("GenGrigLook Added Vertical YES Field ))) : " + nameFiled + "   Fields cpont: " + Fields.Count);
    //                    continue;
    //                }
    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                //Debug.Log("Field Added name init : " + nameFiled);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //                //DebugLogT("Field Added name +++ " + nameFiled);
    //            }
    //        }
    //    }
    //}

    //-------------------------

    //public void CreateFields_()
    //{
    //    Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Field==" + sizeSpriteRendererField);


    //    float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
    //    float heightFiled = sizeSpriteRendererField.y;


    //    Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Panel==" + sizeSpriteRendererField);

    //    //Debug.Log("Panel. Scale.x =" + prefabPanel.transform.localScale.x.ToString());
    //    //Debug.Log("Panel. Scale.y =" + prefabPanel.transform.localScale.y.ToString());
    //    var scaleX = prefabPanel.transform.localScale.x;
    //    var scaleY = prefabPanel.transform.localScale.y;


    //    //float widthPanel = sizeSpriteRendererprefabPanel.x; // .Size.Width;
    //    //float heightPanel = sizeSpriteRendererprefabPanel.y;
    //    float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
    //    float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

    //    int widthLenght = (int)(widthPanel / widthFiled);
    //    int heightLenght = (int)(heightPanel / heightFiled);

    //    int maxLengthOfArray = widthLenght * heightLenght;
    //    Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
    //    int counter = 0;

    //    Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
    //    Debug.Log("panelLocation =" + panelLocation.ToString());

    //    //prefabPanel.Visible = false;
    //    //Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());
    //    prefabPanel.GetComponent<Renderer>().enabled = false;
    //    //Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());

    //    //#TEST
    //    //widthFiled = 35;
    //    //heightFiled = 35;

    //    widthFiled = 1f;
    //    heightFiled = 1f;
    //    //widthFiled = 0.5f;
    //    //heightFiled = 0.5f;


    //    //Vector2 offset = new Vector2();
    //    //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
    //    //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
    //    //float offsetX = prefabField.transform.position.x;
    //    //float offsetY = prefabField.transform.position.y;
    //    float offsetX = 1;
    //    float offsetY = 1;
    //    //Debug.Log("offset 1=" + offset.ToString());
    //    for (int heig = 0; heig < widthLenght; heig++)
    //    {
    //        for (int wid = 0; wid < heightLenght; wid++)
    //        {
    //            counter++;
    //            //Vector2 newPos = new Vector2();
    //            //newPos.x = panelLocation.x + offset.x;
    //            //newPos.y = panelLocation.y + offset.y;
    //            //Debug.Log("offset 2..=" + offset.ToString());
    //            //newPos.x = offset.x;
    //            //newPos.y = offset.y;
    //            Vector2 newPos = new Vector2(offsetX, offsetY);
    //            //newPos = new Vector2(offsetX + prefabField.transform.position.x, offsetY + prefabField.transform.position.y);


    //            //Debug.Log("Instantiate(prefabField) " + counter.ToString());
    //            Debug.Log("Inst Field => " + counter.ToString());

    //            //GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            //GameObject newFiled = (GameObject)Instantiate(prefabField, newPos * Time.deltaTime);
    //            //GameObject newFiled = (GameObject)Instantiate(prefabField, prefabPanel.transform);
    //            GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            //GameObject newFiled = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //            //SpriteRenderer sr = newFiled.AddComponent<SpriteRenderer>();
    //            //sr = prefabField.GetComponent<SpriteRenderer>();

    //            //Debug.Log("newFiled.transform.position init");
    //            //newFiled.transform.position = newPos * Time.deltaTime;
    //            //newFiled.transform.position = new Vector2(prefabField.transform.position.x + offset.x * Time.deltaTime, prefabField.transform.position.y + offset.y * Time.deltaTime);
    //            //newFiled.transform.position = new Vector2(offset.x * Time.deltaTime, offset.y * Time.deltaTime);
    //            //newFiled.transform.position = newPos *Time.deltaTime;
    //            newFiled.transform.position = new Vector2(offsetX, offsetY);
    //            //newFiled.transform.position = newPos;
    //            //newFiled.transform.position = new Vector2(offsetX * Time.deltaTime, offsetY * Time.deltaTime);
    //            //newFiled.transform.position = new Vector2(prefabField.transform.position.x + offset.x, prefabField.transform.position.y + offset.y);
    //            Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());

    //            //Fields.Add(counter, newFiled);
    //            //offset.y += widthFiled;
    //            //offset.x += widthFiled;
    //            offsetX += widthFiled;

    //        }
    //        //Debug.Log("");
    //        //offset.x = 0;
    //        //offset.y -= heightFiled;
    //        offsetX = 0;
    //        offsetY -= heightFiled;
    //    }
    //    //for (int y = 0; y < 15; y++)
    //    //{
    //    //    for (int x = 0; x < 15; x++)
    //    //    {
    //    //        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    //        //cube.AddComponent<Rigidbody>();
    //    //        cube.transform.position = new Vector3(x, y, 0);
    //    //    }
    //    //}
    //    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    // cube.transform.position = new Vector3(1, 1);
    //    // GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    // cube2.transform.position = new Vector3(1, 0);
    //    // GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    // cube3.transform.position = new Vector3(2, 1);
    //}

    //--------------

    //public Rigidbody rocket;
    //public float speed = 10f;

    //void FireRocket()
    //{
    //    Rigidbody rocketClone = (Rigidbody)Instantiate(rocket, transform.position, transform.rotation);
    //    rocketClone.velocity = transform.forward * speed;

    //    // You can also access other components / scripts of the clone
    //    //rocketClone.GetComponent<MyRocketScript>().DoSomething();
    //}

    //// Calls the fire method when holding down ctrl or mouse
    //void Update()
    //{
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        FireRocket();
    //    }
    //}

    //------------------

    //IEnumerator CreateFieldsAsync_()
    //{
    //    if (prefabField == null)
    //    {
    //        Debug.Log("prefabField == null");
    //        yield break;
    //    }
    //    if (prefabPanel == null)
    //    {
    //        Debug.Log("prefabPanel == null");
    //        yield break;
    //    }

    //    if (prefabField.transform == null)
    //    {
    //        Debug.Log("prefabField.transform == null");
    //        yield break;
    //    }

    //    //Debug.Log("SpriteRenderer init");
    //    Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Field==" + sizeSpriteRendererField);

    //    //GetComponent<Collider>().bounds.size
    //    //var t1 = prefabField.GetComponent<Renderer>().bounds.size;
    //    //Debug.Log("RectTransform prefabbFieldTransform complete");

    //    //float widthFiled = FieldTransform.rect.width; // .Size.Width;
    //    //float heightFiled = FieldTransform.rect.height;
    //    float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
    //    float heightFiled = sizeSpriteRendererField.y;


    //    //Debug.Log("RectTransform prefabbPanelTransform init");
    //    //RectTransform PanelTransform = (RectTransform)prefabPanel.transform;
    //    //float widthPanel = PanelTransform.rect.width; // .Size.Width;
    //    //float heightPanel = PanelTransform.rect.height;

    //    Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Panel==" + sizeSpriteRendererField);


    //    float widthPanel = sizeSpriteRendererprefabPanel.x; // .Size.Width;
    //    float heightPanel = sizeSpriteRendererprefabPanel.y;

    //    int widthLenght = (int)(widthPanel / widthFiled);
    //    int heightLenght = (int)(heightPanel / heightFiled);

    //    int maxLengthOfArray = widthLenght * heightLenght;
    //    Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
    //    int counter = 0;

    //    //Point panelLocation = prefabPanel.Location;
    //    //Vector2 panelLocation = PanelTransform.rect.position;
    //    //Vector2 panelLocation = PanelTransform.rect.position;
    //    //Debug.Log("prefabPanel.GetComponent<Renderer> init");
    //    Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
    //    Debug.Log("panelLocation =" + panelLocation.ToString());

    //    //prefabPanel.Visible = false;
    //    Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());
    //    prefabPanel.GetComponent<Renderer>().enabled = false;
    //    Debug.Log("visible=" + prefabPanel.GetComponent<Renderer>().enabled.ToString());

    //    //#TEST
    //    widthFiled = 50;
    //    heightFiled = 50;

    //    //var renderers = gameObject.GetComponentsInChildren.();
    //    //for (var r Renderer in renderers) {
    //    //    r.enabled = !r.enabled;
    //    //}
    //    //Vector2 offset = new Vector2(prefabField.transform.position.x, prefabField.transform.position.y);
    //    Vector2 offset = new Vector2();
    //    for (int heig = 0; heig < widthLenght; heig++)
    //    {
    //        for (int wid = 0; wid < heightLenght; wid++)
    //        {
    //            counter++;
    //            Vector2 newPos = new Vector2();
    //            //newPos.x = panelLocation.x + offset.x;
    //            //newPos.y = panelLocation.y + offset.y;
    //            newPos.x = offset.x;
    //            newPos.y = offset.y;

    //            Debug.Log("Instantiate(prefabField) " + counter.ToString());
    //            GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            //Debug.Log("newFiled.transform.position init");
    //            newFiled.transform.position = newPos * Time.deltaTime;
    //            Debug.Log("newFiled.transform.position =" + newPos.ToString());

    //            //Fields.Add(counter, newFiled);
    //            //offset.y += widthFiled;
    //            offset.x -= widthFiled;
    //            yield return null;
    //        }
    //        //Debug.Log("");
    //        offset.x = 0;
    //        offset.y -= heightFiled;
    //    }

    //    ////foreach (GameObject b in cells)
    //    ////    Controls.Add(b.btn);
    //}

    //IEnumerator CreateFieldsAsync()
    //{
    //    Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Field==" + sizeSpriteRendererField);

    //    float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
    //    float heightFiled = sizeSpriteRendererField.y;

    //    Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Panel==" + sizeSpriteRendererField);

    //    var scaleX = prefabPanel.transform.localScale.x;
    //    var scaleY = prefabPanel.transform.localScale.y;

    //    float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
    //    float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

    //    int widthLenght = (int)(widthPanel / widthFiled);
    //    int heightLenght = (int)(heightPanel / heightFiled);

    //    int maxLengthOfArray = widthLenght * heightLenght;
    //    Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
    //    int counter = 0;

    //    Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
    //    Debug.Log("panelLocation =" + panelLocation.ToString());

    //    prefabPanel.GetComponent<Renderer>().enabled = false;

    //    widthFiled = 1f;
    //    heightFiled = 1f;

    //    float offsetX = 1;
    //    float offsetY = 1;
    //    for (int heig = 0; heig < widthLenght; heig++)
    //    {
    //        for (int wid = 0; wid < heightLenght; wid++)
    //        {
    //            counter++;
    //            Vector2 newPos = new Vector2(offsetX, offsetY);
    //            GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            newFiled.transform.position = new Vector2(offsetX, offsetY);
    //            Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());
    //            offsetX += widthFiled;

    //            yield return null;
    //        }
    //        offsetX = 0;
    //        offsetY -= heightFiled;
    //    }

    //}

    //-------------------------------------

    //public IEnumerator GenGrigLookAsync(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    //{
    //    int gridWidth = 100;
    //    int gridHeight = 100;
    //    //gridWidth = (int)GridX;
    //    //gridHeight = (int)GridY;

    //    int maxVertical = (int)p_limitVerticalLook + 1;// *-1;
    //    int maxHorizontal = (int)p_limitHorizontalLook + 1;

    //    if (Fields.Count != _counter || _counter == 0)
    //        yield break;

    //    if (_movement.x != 0)
    //    {
    //        int p_startPosY = p_PosHeroY - (p_limitVerticalLook / 2);
    //        //Validate
    //        if (p_startPosY < 0)
    //            p_startPosY = 0;

    //        int limitVertical = p_startPosY + maxVertical;
    //        if (limitVertical > gridHeight)
    //            limitVertical = gridHeight;

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int x = 0;
    //        int LeftX = p_PosHeroX - (p_limitHorizontalLook / 2);
    //        int RightX = p_PosHeroX + (p_limitHorizontalLook / 2);
    //        int LeftRemoveX = LeftX - 1;
    //        int RightRemoveX = RightX + 1;
    //        //Validate
    //        if (LeftRemoveX < 0)
    //        {
    //            if (_movement.x > 0)
    //                isRemove = false;
    //        }
    //        if (LeftX < 0)
    //        {
    //            LeftX = 0;
    //            if (_movement.x < 0)
    //                isAdded = false;
    //        }
    //        if (RightRemoveX > gridWidth)
    //        {
    //            if (_movement.x < 0)
    //                isRemove = false;
    //        }
    //        if (RightX > gridWidth)
    //        {
    //            RightX = gridWidth;
    //            if (_movement.x > 0)
    //                isAdded = false;
    //        }

    //        if (isRemove)
    //        {
    //            x = _movement.x > 0 ?
    //                //Remove Vertical
    //            LeftRemoveX :
    //            RightRemoveX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                    continue;

    //                GameObject findFiled = Fields[nameFiled];
    //                //Destroy !!!
    //                //Destroy(findFiled, 0.5f);
    //                Destroy(findFiled);
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //                //yield return null;
    //            }
    //            yield return null;
    //        }

    //        if (isAdded)
    //        {
    //            x = _movement.x > 0 ?
    //                //Added Vertical
    //                RightX :
    //                LeftX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;

    //                if (Fields.ContainsKey(nameFiled))
    //                    continue;

    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //                //yield return null;
    //            }
    //            yield return null;
    //        }
    //    }

    //    if (_movement.y != 0)
    //    {
    //        int p_startPosX = p_PosHeroX - (p_limitHorizontalLook / 2); //#
    //        //Validate
    //        if (p_startPosX < 0)
    //            p_startPosX = 0;

    //        int limitHorizontal = p_startPosX + maxHorizontal;
    //        if (limitHorizontal > gridWidth)
    //            limitHorizontal = gridWidth;

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int y = 0;
    //        int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
    //        int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
    //        int TopRemoveY = TopY - 1;
    //        int DownRemoveY = DownY + 1;

    //        //Validate
    //        if (TopRemoveY < 0 && _movement.y < 0)
    //            isRemove = false;
    //        if (TopY < 0 && _movement.y > 0)
    //        {
    //            TopY = 0;
    //            //_movement.y > 0
    //            isAdded = false;
    //        }
    //        if (DownRemoveY > gridHeight && _movement.y > 0)
    //            isRemove = false;

    //        if (DownY > gridHeight && _movement.y < 0)
    //        {
    //            DownY = gridHeight;
    //            //if (_movement.y < 0)
    //            isAdded = false;
    //        }

    //        if (isRemove)
    //        {
    //            y = _movement.y < 0 ?
    //                //Remove Horizontal //#
    //                TopRemoveY :
    //                DownRemoveY; //#

    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                    continue;

    //                GameObject findFiled = Fields[nameFiled];
    //                //Destroy !!!
    //                //Destroy(findFiled, 0.5f);
    //                Destroy(findFiled);
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //                //yield return null;
    //            }
    //            yield return null;
    //        }

    //        if (isAdded)
    //        {
    //            y = _movement.y < 0 ?
    //                //Added Horizontal
    //                DownY :
    //                TopY; //#
    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);

    //                if (Fields.ContainsKey(nameFiled))
    //                    continue;

    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //                //yield return null;
    //            }
    //            yield return null;
    //        }
    //    }
    //}

    //--------------

    //public void GenGrigLook(Vector2 _movement, int p_PosHeroX = 0, int p_limitHorizontalLook = 0, int p_PosHeroY = 0, int p_limitVerticalLook = 0)
    //{
    //    int gridWidth = 100;
    //    int gridHeight = 100;
    //    //gridWidth = (int)GridX;
    //    //gridHeight = (int)GridY;

    //    int maxVertical = (int)p_limitVerticalLook + 1;// *-1;
    //    int maxHorizontal = (int)p_limitHorizontalLook + 1;

    //    if (Fields.Count != _counter || _counter == 0)
    //        return;

    //    if (_movement.x != 0)
    //    {
    //        int p_startPosY = p_PosHeroY - (p_limitVerticalLook / 2);
    //        //Validate
    //        if (p_startPosY < 0)
    //            p_startPosY = 0;

    //        int limitVertical = p_startPosY + maxVertical;
    //        if (limitVertical > gridHeight)
    //            limitVertical = gridHeight;

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int x = 0;
    //        int LeftX = p_PosHeroX - (p_limitHorizontalLook / 2);
    //        int RightX = p_PosHeroX + (p_limitHorizontalLook / 2);
    //        int LeftRemoveX = LeftX - 1;
    //        int RightRemoveX = RightX + 1;
    //        //Validate ValidateRemoveX
    //        if (LeftRemoveX < 0)
    //        {
    //            if (_movement.x > 0)
    //                isRemove = false;
    //        }
    //        if (RightRemoveX > gridWidth)
    //        {
    //            if (_movement.x < 0)
    //                isRemove = false;
    //        }

    //        if (RightX > gridWidth)
    //        {
    //            RightX = gridWidth;
    //            if (_movement.x > 0)
    //                isAdded = false;
    //        }
    //        if (LeftX < 0)
    //        {
    //            LeftX = 0;
    //            if (_movement.x < 0)
    //                isAdded = false;
    //        }



    //        if (isRemove)
    //        {
    //            x = _movement.x > 0 ?
    //                //Remove Vertical
    //            LeftRemoveX :
    //            RightRemoveX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                    continue;

    //                GameObject findFiled = Fields[nameFiled];
    //                //Destroy !!!
    //                //Destroy(findFiled, 0.5f);
    //                Destroy(findFiled);
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //            }
    //        }

    //        if (isAdded)
    //        {
    //            x = _movement.x > 0 ?
    //                //Added Vertical
    //                RightX :
    //                LeftX;

    //            string _nameFiled = "";
    //            for (int y = p_startPosY; y < limitVertical; y++)
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                _nameFiled = nameFiled;

    //                if (Fields.ContainsKey(nameFiled))
    //                    continue;

    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //            }
    //        }
    //    }

    //    if (_movement.y != 0)
    //    {
    //        int p_startPosX = p_PosHeroX - (p_limitHorizontalLook / 2); //#
    //        //Validate
    //        if (p_startPosX < 0)
    //            p_startPosX = 0;

    //        int limitHorizontal = p_startPosX + maxHorizontal;
    //        if (limitHorizontal > gridWidth)
    //            limitHorizontal = gridWidth;

    //        bool isRemove = true;
    //        bool isAdded = true;
    //        int y = 0;
    //        int TopY = p_PosHeroY - (p_limitVerticalLook / 2); //#
    //        int DownY = p_PosHeroY + (p_limitVerticalLook / 2); //#
    //        int TopRemoveY = TopY - 1;
    //        int DownRemoveY = DownY + 1;

    //        //Validate
    //        if (TopRemoveY < 0 && _movement.y < 0)
    //            isRemove = false;
    //        if (TopY < 0 && _movement.y > 0)
    //        {
    //            TopY = 0;
    //            //_movement.y > 0
    //            isAdded = false;
    //        }
    //        if (DownRemoveY > gridHeight && _movement.y > 0)
    //            isRemove = false;

    //        if (DownY > gridHeight && _movement.y < 0)
    //        {
    //            DownY = gridHeight;
    //            //if (_movement.y < 0)
    //            isAdded = false;
    //        }

    //        if (isRemove)
    //        {
    //            y = _movement.y < 0 ?
    //                //Remove Horizontal //#
    //                TopRemoveY :
    //                DownRemoveY; //#

    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);
    //                //Find
    //                if (!Fields.ContainsKey(nameFiled))
    //                    continue;

    //                GameObject findFiled = Fields[nameFiled];
    //                //Destroy !!!
    //                //Destroy(findFiled, 0.5f);
    //                Destroy(findFiled);
    //                Fields.Remove(nameFiled);
    //                _counter--;
    //            }
    //        }

    //        if (isAdded)
    //        {
    //            y = _movement.y < 0 ?
    //                //Added Horizontal
    //                DownY :
    //                TopY; //#
    //            for (int x = p_startPosX; x < limitHorizontal; x++) //#
    //            {
    //                string nameFiled = "Filed" + x + "x" + Mathf.Abs(y);

    //                if (Fields.ContainsKey(nameFiled))
    //                    continue;

    //                Vector3 pos = new Vector3(x, y * (-1), 1) * Spacing;
    //                pos.z = 0;
    //                GameObject newFiled = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);
    //                newFiled.name = nameFiled;
    //                Fields.Add(nameFiled, newFiled);
    //                _counter++;
    //            }
    //        }
    //    }
    //}

    //---------------------

    //public void CreateFields()
    //{
    //    Vector2 sizeSpriteRendererField = prefabField.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Field==" + sizeSpriteRendererField);

    //    float widthFiled = sizeSpriteRendererField.x; // .Size.Width;
    //    float heightFiled = sizeSpriteRendererField.y;

    //    Vector2 sizeSpriteRendererprefabPanel = prefabPanel.GetComponent<SpriteRenderer>().sprite.rect.size;
    //    Debug.Log("size Panel==" + sizeSpriteRendererField);

    //    var scaleX = prefabPanel.transform.localScale.x;
    //    var scaleY = prefabPanel.transform.localScale.y;

    //    float widthPanel = sizeSpriteRendererprefabPanel.x * scaleX;
    //    float heightPanel = sizeSpriteRendererprefabPanel.y * scaleY;

    //    int widthLenght = (int)(widthPanel / widthFiled);
    //    int heightLenght = (int)(heightPanel / heightFiled);

    //    int maxLengthOfArray = widthLenght * heightLenght;
    //    Debug.Log("maxLengthOfArray =" + maxLengthOfArray.ToString());
    //    int counter = 0;

    //    Vector2 panelLocation = prefabPanel.GetComponent<Renderer>().bounds.size;
    //    Debug.Log("panelLocation =" + panelLocation.ToString());

    //    prefabPanel.GetComponent<Renderer>().enabled = false;

    //    widthFiled = 1f;
    //    heightFiled = 1f;

    //    float offsetX = 1;
    //    float offsetY = 1;
    //    for (int heig = 0; heig < widthLenght; heig++)
    //    {
    //        for (int wid = 0; wid < heightLenght; wid++)
    //        {
    //            counter++;
    //            Vector2 newPos = new Vector2(offsetX, offsetY);
    //            GameObject newFiled = (GameObject)Instantiate(prefabField);
    //            newFiled.transform.position = new Vector2(offsetX, offsetY);
    //            Debug.Log("newFiled.transform.position =" + newFiled.transform.position.ToString());
    //            offsetX += widthFiled;

    //            //Fields.Add(counter, newFiled);
    //        }
    //        offsetX = 0;
    //        offsetY -= heightFiled;
    //    }
    //}
    //----------------------
    //#.D
    //private void RemoveRealObjectD(string p_nameFiled)
    //{
    //    if (!GamesObjectsReal.ContainsKey(p_nameFiled))
    //    {
    //        //Debug.Log("RemoveRealObject Not in field : " + p_nameFiled);
    //        return;
    //    }
    //    else
    //    {
    //        //---------------------------
    //        if (GridData != null)
    //        {
    //            Debug.Log("RemoveRealObjectD GridData -- EMPTY !!!!!!!");
    //            return;
    //        }
    //        if (GridData.Fields.Find(p => p.NameField == p_nameFiled) == null)
    //            return;

    //        int indexFieldData = GridData.Fields.FindIndex(p => p.NameField == p_nameFiled);
    //        if (indexFieldData == -1)
    //        {
    //            Debug.Log("RemoveRealObjectD Not field in Data: " + p_nameFiled);
    //            return;
    //        }
    //        List<SaveLoadData.ObjectData> dataObjects = GridData.Fields[indexFieldData].Objects;

    //        //int indexObjectData = listDataObjectInField.FindIndex(p => p.NameObject == p_saveObject.name);
    //        //SaveLoadData.ObjectData objDataOld = listDataObjectInField[indexObjectData];

    //        //var dataObjectSave = SaveLoadData.CreateObjectData(p_saveObject);
    //        //int indexNN = dataObjects.Count + 1;

    //        //if (objDataOld == null)
    //        //{
    //        //    dataObjectSave.NameObject += indexNN;
    //        //    dataObjects.Add(dataObjectSave);
    //        //}
    //        //else
    //        //{
    //        //    objDataOld = dataObjectSave;
    //        //}
    //        //-------------------------

    //        //List<GameObject> activeObjects = GamesObjectsActive[p_nameFiled];
    //        List<GameObject> realObjects = GamesObjectsReal[p_nameFiled];


    //        for (int i = activeObjects.Count - 1; i >= 0; i--)
    //        {


    //            activeObjects[i].SetActive(false); //#

    //            if (realObjects.Count <= i)
    //                continue;
    //            if (realObjects[i] == null)
    //                continue;



    //            var pos1 = activeObjects[i].transform.position;
    //            var pos2 = realObjects[i].transform.position;

    //            var f = pos1.y;
    //            string posFieldOld = GetNameFiled(pos1.x, pos1.y);
    //            string posFieldReal = GetNameFiled(pos2.x, pos2.y);

    //            //---------------------------------------------
    //            if (posFieldOld != posFieldReal)
    //            {
    //                Debug.Log("RemoveRealObject posFieldOld(" + posFieldOld + ") != posFieldReal(" + posFieldReal + ")      " + activeObjects[i].name + "    " + realObjects[i].name);

    //                activeObjects.RemoveAt(i);
    //                if (!GamesObjectsActive.ContainsKey(posFieldReal))
    //                {
    //                    Debug.Log("RemoveRealObject Not new posFieldReal =" + posFieldReal);
    //                }
    //                else
    //                {
    //                    //Add in new Filed
    //                    List<GameObject> activeObjectsNew = GamesObjectsActive[posFieldReal];

    //                    var realObj = realObjects[i];
    //                    var coyObj = CreatePrefabByName(realObj.tag, realObj.name, realObj.transform.position);

    //                    activeObjectsNew.Add(coyObj);

    //                }
    //            }
    //            else
    //            {
    //                //---------------------------------------------
    //                //Save Real value in memory
    //                activeObjects[i] = Instantiate(realObjects[i]); //#
    //                activeObjects[i].SetActive(false); //#
    //            }
    //        }

    //        SaveToDataObject(p_nameFiled);

    //        foreach (var obj in realObjects)
    //        {
    //            _counter--;
    //            Destroy(obj);
    //            //obj.SetActive(false);
    //        }
    //        GamesObjectsReal.Remove(p_nameFiled);

    //        //DebugLogT("RemoveRealObject objects in field ++++ " + p_nameFiled);


    //    }
    //}

    //---------------------

    //private void DeactivateGameObjectForLook(string p_nameFiled)
    //{
    //    //DebugLog("# DeactivateGameObjectForLook");

    //    if (!GamesObjectsActive.ContainsKey(p_nameFiled))
    //    {
    //        //DebugLog("DeactivateGameObjectForLook Not in field : " + p_nameFiled);
    //        return;
    //    }

    //    Debug.Log("# CreateGameObjectActiveForLook : " + p_nameFiled);

    //    List<GameObject> listGameObjectInField = GamesObjectsActive[p_nameFiled];
    //    foreach (var gameObj in listGameObjectInField)
    //    {
    //        //Destroy();// gameObj.SetActive(false);
    //        _counter--;
    //        DebugLog("# CreateGameObjectActiveForLook " + newFiled.name + " " + newFiled.tag + "  in  " + p_nameFiled);
    //    }
    //}

    //private void CreateGameObjectActive(string p_nameFiled)
    //{

    //}


    //-----------------------------

    //NotSupportedException: The type System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] is not supported because it implements IDictionary.
    //System.Xml.Serialization.TypeData.get_ListItemType ()
    //System.Xml.Serialization.TypeData.get_ListItemTypeData ()
    //System.Xml.Serialization.TypeData..ctor (System.Type type, System.String elementName, Boolean isPrimitive, System.Xml.Serialization.TypeData mappedType, System.Xml.Schema.XmlSchemaPatternFacet facet)
    //System.Xml.Serialization.TypeData..ctor (System.Type type, System.String elementName, Boolean isPrimitive)
    //System.Xml.Serialization.TypeTranslator.GetTypeData (System.Type runtimeType, System.String xmlDataType)
    //System.Xml.Serialization.TypeTranslator.GetTypeData (System.Type type)
    //System.Xml.Serialization.XmlReflectionImporter.CreateMapMember (System.Type declaringType, System.Xml.Serialization.XmlReflectionMember rmember, System.String defaultNamespace)
    //System.Xml.Serialization.XmlReflectionImporter.ImportClassMapping (System.Xml.Serialization.TypeData typeData, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
    //Rethrow as InvalidOperationException: There was an error reflecting field 'FieldsD'.
    //System.Xml.Serialization.XmlReflectionImporter.ImportClassMapping (System.Xml.Serialization.TypeData typeData, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
    //System.Xml.Serialization.XmlReflectionImporter.ImportTypeMapping (System.Xml.Serialization.TypeData typeData, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
    //Rethrow as InvalidOperationException: There was an error reflecting type 'SaveLoadData+GridData'.
    //System.Xml.Serialization.XmlReflectionImporter.ImportTypeMapping (System.Xml.Serialization.TypeData typeData, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
    //System.Xml.Serialization.XmlReflectionImporter.ImportTypeMapping (System.Type type, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
    //System.Xml.Serialization.XmlSerializer..ctor (System.Type type, System.Xml.Serialization.XmlAttributeOverrides overrides, System.Type[] extraTypes, System.Xml.Serialization.XmlRootAttribute root, System.String defaultNamespace)
    //System.Xml.Serialization.XmlSerializer..ctor (System.Type type, System.Type[] extraTypes)
    //SaveLoadData+Serializator.DeXml (System.String datapath) (at Assets/Scripts/SaveLoadData.cs:363)
    //SaveLoadData.LoadPathData () (at Assets/Scripts/SaveLoadData.cs:67)
    //SaveLoadData.Start () (at Assets/Scripts/SaveLoadData.cs:44)

    //---------------------------------

    ////public Dictionary(IDictionary<TKey, TValue> dictionary);
    //        //public Dictionary(IEqualityComparer<TKey> comparer);
    //        //public Dictionary(int capacity);
    //        //public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer);
    //        //public Dictionary(int capacity, IEqualityComparer<TKey> comparer);
    //        //protected Dictionary(SerializationInfo info, StreamingContext context);

    //        //public class Dictionary<TKey, TValue> : IEnumerable, ISerializable, ICollection, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IDictionary, IDeserializationCallback
    //        //Dictionary<string, FieldData>
    //        IEnumerable<KeyValuePair<string, FieldData>> listXML = state.FieldsXML;
    //        state.FieldsD = new Dictionary<string, FieldData>();
    //        //IEnumerable<FieldData> source = listXML.Select(x => x.Value);
    //        //Func<FieldData, string> selector = (fd) => {
    //        //    FieldData data = fd;
    //        //    //data
    //        //    return "";
    //        //};
    //        //state.FieldsD = ToDict(state.FieldsXML).ToDictionary(x => x.Key, x => x.Value);
    //        state.FieldsD = state.FieldsXML.ToDictionary(x => x.Key, x => x.Value); 
    //        //-----------------------

    //            //Dictionary<TKey, TSource>
    //            //(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);
    //        //state.FieldsD = state.FieldsXML.ToDictionary<string, FieldData>(source, selector);
    //        //state.FieldsD = state.FieldsXML.ToDictionary<string, FieldData>(source, new Func<TSource, TKey>);

    //        //state.FieldsD = state.FieldsXML.ToDictionary<string, FieldData>(x => x.Key, y => y.Value);

    //        //public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>
    //        //            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);
    //        //public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>
    //        //          (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector);
    //        //public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>
    //        //              (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer);
    //        //public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>
    //        //          (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer);


    //        //Dictionary<TKey, TSource>
    //        //(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);

    //        IEnumerable<KeyValuePair<int, string>> listT = new List<KeyValuePair<int, string>>();
    //        Dictionary<int, string> d1 = new Dictionary<int, string>();
    //        //d1 = listT.ToDictionary<int, string>(x => x.Key, x => x.Value);
    //        //d1 = listT.ToDictionary<int, string>(listT);
    //        //d1 = listT.ToDictionary<int, string>(x => x.Value, x => x.Key);


    //        IEnumerable<string> source2 = listT.Select(x => x.Value);
    //        Func<string, string> selectorTest = str => str.ToUpper();
    //        Func<string, int> selector2 = str => str.Length;
    //        Func<int, string> selector3 = key => key.ToString();

    //        // Dictionary<TKey, TSource> 
    //        // ToDictionary<TSource, TKey>
    //        //(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);
    //        //d1 = listT.ToDictionary<int, string>(source2, selector2);
    //        //d1 = listT.ToDictionary<int, string>(source2, selector2);
    //        //d1 = listT.ToDictionary<int, string>(x => x.ToString(), x => x.Value.Length);
    //        //d1 = listT.ToDictionary<int, string>(x => x.Value.ToString(), c=>c.Key.ToString().Length);
    //        //d1 = listT.ToDictionary<int, string>(x => x.Value.ToString(), x => x.Key.ToString());
    //        //d1 = listT.ToDictionary<string, int>(x => x.Value.ToString(), x => x.Key.ToString());
    //        //var ress = listT.ToDictionary<string, int>(key => key.ToString().Length); //
    //        //var ress = listT.ToDictionary<string, int>(source2, selector2); //

    //        //var ress = listT.ToDictionary<string, int>(x => x.Key, ); //

    //        //d1 = listT.ToDictionary<int, string>(x => x.Value, x => x.Key);
    //        //d1 = listT.ToDictionary<int, string>(key => key.ToString());
    //        //var _d77 = listT.ToDictionary<int, string>(source2, selector2);
    //        //d1 = listT.GetComponents().ToDictionary(x => x.Key, x => x.Value);
    //        //d1 = listT.GetComponents().ToDictionary(x => x.Key, x => x.Value);
    //        //d1 = GetComponents2().ToDictionary(x => x.Key, x => x.Value);
    //        d1 = GetComponents3(listT).ToDictionary(x => x.Key, x => x.Value);
    //        //var tt= (IEnumerable<KeyValuePair<int, string>>)listT;
    //        //d1 = tt;


    //XmlSerializer serializer = new XmlSerializer(typeof(GridData), extraTypes);
    ////XmlSerializer serializer2 = new XmlSerializer(typeof(item[]),
    ////                     new XmlRootAttribute() { ElementName = "items" });

    ////List<KeyValuePair<K,V>>а затем (re) создайте ее в хэш-таблицу.

    //FileStream fs = new FileStream(datapath, FileMode.Create);

    ////serializer.Serialize(fs,
    ////  state.FieldsD.Select(kv => new item() { id = kv.Key, value = kv.Value }).ToArray());

    //----------------------------------

    ////Загрузка объектов из стека памяти на поле ADDED FOR LOOK
    ////GamesObjectsActive -> listGameObjectReal
    //private void LoadGameObjectActiveForLook(string p_nameField)
    //{
    //    //return;

    //    //DebugLog("# LoadGameObjectActiveForLook");
    //    if (GamesObjectsActive == null)
    //    {
    //        Debug.Log("LoadGameObjectActiveForLook GamesObjectsActive is EMPTY ");
    //        return;
    //    }


    //    if (!GamesObjectsActive.ContainsKey(p_nameField))
    //    {
    //        //Debug.Log("LoadGameObjectActiveForLook Not in field : " + p_nameFiled);
    //        return;
    //    }

    //    //# Debug.Log("# LoadGameObjectActiveForLook : " + p_nameFiled);

    //    List<GameObject> listGameObjectInField = GamesObjectsActive[p_nameField];
    //    List<GameObject> listGameObjectReal = new List<GameObject>();

    //    bool isExistFieldReal = false;
    //    if (!GamesObjectsReal.ContainsKey(p_nameField))
    //    {
    //        //# Debug.Log("LoadGameObjectActiveForLook GamesObjectsReal add field - " + p_nameFiled);
    //        GamesObjectsReal.Add(p_nameField, listGameObjectReal);
    //    }
    //    else
    //    {
    //        listGameObjectReal = GamesObjectsReal[p_nameField];
    //    }

    //    foreach (var gameObj in listGameObjectInField)
    //    {
    //        //Debug.Log("# LoadGameObjectActiveForLook REAL ++++++++ " + gameObj.name + " " + gameObj.tag + "  in  " + p_nameFiled );

    //        //# TYPE.1
    //        //GameObject newFiled = (GameObject)Instantiate(gameObj, gameObj.transform.position, Quaternion.identity);
    //        //newFiled.SetActive(true);

    //        //# TYPE.3
    //        //gameObj.SetActive(true);
    //        //GameObject newFiled = gameObj;

    //        //# TYPE.2
    //        GameObject newField = CreatePrefabByName(gameObj.tag, gameObj.name, gameObj.transform.position);

    //        //Fields.Add(nameFiled, newFiled);
    //        //# rem TYPE.3 
    //        listGameObjectReal.Add(newField);
    //        Counter++;
    //        //Debug.Log("# LoadGameObjectActiveForLook " + newFiled.name + " " + newFiled.tag + "  in  " + p_nameFiled + "  pos=" + gameObj.transform.position);
    //    }
    //}

    ////#.D загрузка из данныx объектов из памяти и создание их на поле  ADDED FOR LOOK - DATA
    //private void LoadGameObjectDataForLook(string p_nameField)
    //{
    //    //GridData
    //    if (GridData == null)
    //    {
    //        //Debug.Log(" LoadGameObjectDataForLook GridData IS EMPTY !!!");
    //        return;
    //    }

    //    if (GridData.Fields == null)
    //        //Debug.Log(" LoadGameObjectDataForLook GridData.Fields IS EMPTY !!!");

    //        if (GridData.Fields.Find(p => p.NameField == p_nameField) == null)
    //        {
    //            //Debug.Log(" LoadGameObjectDataForLook GridData.Fields not find: " + p_nameFiled);
    //            return;
    //        }

    //    List<SaveLoadData.ObjectData> listGameObjectInField = GridData.Fields.Find(p => p.NameField == p_nameField).Objects;
    //    List<GameObject> listGameObjectReal = new List<GameObject>();

    //    if (!GamesObjectsReal.ContainsKey(p_nameField))
    //    {
    //        GamesObjectsReal.Add(p_nameField, listGameObjectReal);
    //    }
    //    else
    //    {
    //        listGameObjectReal = GamesObjectsReal[p_nameField];
    //    }

    //    int _count = Counter;
    //    foreach (var gameObj in listGameObjectInField)
    //    {
    //        GameObject newField = CreatePrefabByName(gameObj.TagObject, gameObj.NameObject, gameObj.Position);

    //        listGameObjectReal.Add(newField);
    //        Counter++;
    //        //Debug.Log(" LoadGameObjectDataForLook added +++ : " + gameObj.NameObject);
    //    }

    //    //Debug.Log(" LoadGameObjectDataForLook.... ADDED: " + (_counter - _count));
    //}

    ////REMOVE FOR LOOK
    //private void RemoveRealObject_Active(string p_nameField)
    //{
    //    //Debug.Log("RemoveRealObject_Active..... ");

    //    if (!GamesObjectsReal.ContainsKey(p_nameField))
    //    {
    //        //Debug.Log("RemoveRealObject Not in field : " + p_nameFiled);
    //        return;
    //    }
    //    else
    //    {
    //        List<GameObject> activeObjects = null;
    //        if (GamesObjectsActive.ContainsKey(p_nameField))
    //            activeObjects = GamesObjectsActive[p_nameField];

    //        List<GameObject> realObjects = GamesObjectsReal[p_nameField];

    //        if (activeObjects != null)
    //        {

    //            for (int i = activeObjects.Count - 1; i >= 0; i--)
    //            {
    //                //Debug.Log("RemoveRealObject_Active");

    //                activeObjects[i].SetActive(false); //#

    //                if (realObjects.Count <= i)
    //                    continue;
    //                if (realObjects[i] == null)
    //                    continue;

    //                //Debug.Log("RemoveRealObject TYPE.2 Save new position .3");

    //                var pos1 = activeObjects[i].transform.position;
    //                var pos2 = realObjects[i].transform.position;
    //                var f = pos1.y;
    //                string posFieldOld = GetNameFieldPosit(pos1.x, pos1.y);
    //                string posFieldReal = GetNameFieldPosit(pos2.x, pos2.y);

    //                //---------------------------------------------
    //                if (posFieldOld != posFieldReal)
    //                {
    //                    Debug.Log("RemoveRealObject posFieldOld(" + posFieldOld + ") != posFieldReal(" + posFieldReal + ")      " + activeObjects[i].name + "    " + realObjects[i].name);
    //                    //activeObjects[i].transform.position = realObjects[i].transform.position;

    //                    //Debug.Log("RemoveRealObject ........... Remove in old Filed");
    //                    //Remove in old Filed
    //                    //activeObjects[i].SetActive(false);
    //                    activeObjects.RemoveAt(i);
    //                    //if (GamesObjectsActive[posFieldReal])
    //                    if (!GamesObjectsActive.ContainsKey(posFieldReal))
    //                    {
    //                        Debug.Log("RemoveRealObject Not new posFieldReal =" + posFieldReal);
    //                    }
    //                    else
    //                    {
    //                        //Debug.Log("RemoveRealObject ........... Add in new Filed");

    //                        //Add in new Filed
    //                        List<GameObject> activeObjectsNew = GamesObjectsActive[posFieldReal];

    //                        //Debug.Log("RemoveRealObject ........... Add in new Filed  pred=" + GamesObjectsActive[posFieldReal].Count);

    //                        //realObjects[i].SetActive(false);
    //                        //activeObjectsNew.Add(Instantiate(realObjects[i]));

    //                        //## var coyObj = Instantiate(realObjects[i]);
    //                        var realObj = realObjects[i];
    //                        var coyObj = CreatePrefabByName(realObj.tag, realObj.name, realObj.transform.position);

    //                        activeObjectsNew.Add(coyObj);
    //                        //coyObj.SetActive(false);

    //                        //Debug.Log("RemoveRealObject ........... Add in new Filed  post=" + GamesObjectsActive[posFieldReal].Count);
    //                        //activeObjectsNew[activeObjectsNew.Count-1].SetActive(false);
    //                    }
    //                }
    //                else
    //                {
    //                    //---------------------------------------------
    //                    //Save Real value in memory
    //                    activeObjects[i] = Instantiate(realObjects[i]); //#
    //                    activeObjects[i].SetActive(false); //#
    //                }
    //            }
    //        }
    //        foreach (var obj in realObjects)
    //        {
    //            Counter--;
    //            Destroy(obj);
    //            //obj.SetActive(false);
    //        }
    //        //Debug.Log("RemoveRealObject_Active.....6");
    //        GamesObjectsReal.Remove(p_nameField);
    //    }
    //}

    ////ADD NEW GEN GAME OBJECT -- ACTIVE
    //private void AddNewActiveGameObject(string p_nameField, GameObject p_saveObject)
    //{
    //    DebugLog("# AddNewActiveGameObject " + p_saveObject.name + "  " + p_saveObject.tag);

    //    int index = 0;

    //    List<GameObject> gobjects = new List<GameObject>();
    //    //List<GameObject> gobjects;
    //    if (GamesObjectsActive.ContainsKey(p_nameField))
    //    {
    //        gobjects = GamesObjectsActive[p_nameField];
    //        index = gobjects.Count + 1; //.Find(p => p.tag == p_saveObject.tag);
    //        //var index = gobjects.Where(p => p.tag == p_saveObject.tag).Count;
    //    }
    //    else
    //    {
    //        gobjects = new List<GameObject>();
    //        GamesObjectsActive.Add(p_nameField, gobjects);
    //        gobjects = GamesObjectsActive[p_nameField]; //???
    //        index = 1;
    //    }

    //    p_saveObject.name = p_saveObject.tag + "_" + p_nameField + index;

    //    gobjects.Add(p_saveObject);
    //    DebugLog("# AddNewActiveGameObject Init +++ " + p_saveObject.name);
    //}

    ////#.D ADD --NEW GEN-- GAME OBJECT -- DATA
    //private void SaveNewGameObjectToData(string p_nameField, GameObject p_saveObject)
    //{
    //    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //    return;

    //    DebugLog("# SaveNewAGameObjectInData " + p_saveObject.name + "  " + p_saveObject.tag);

    //    if (GridData == null)
    //    {
    //        return;
    //    }

    //    if (GridData.Fields.Find(p => p.NameField == p_nameField) == null)
    //        return;

    //    int indexFieldData = GridData.Fields.FindIndex(p => p.NameField == p_nameField);
    //    List<SaveLoadData.ObjectData> listDataObjectInField = GridData.Fields[indexFieldData].Objects;
    //    //List<SaveLoadData.ObjectData> listDataObjectInField = GridData.Fields.Find(p => p.NameField == p_nameFiled).Objects;


    //    List<GameObject> listGameObjectReal = new List<GameObject>();

    //    int indexObjectData = listDataObjectInField.FindIndex(p => p.NameObject == p_saveObject.name);
    //    //SaveLoadData.ObjectData objData = listDataObjectInField.Find(p => p.NameObject == p_saveObject.name);
    //    SaveLoadData.ObjectData objDataOld = listDataObjectInField[indexObjectData];


    //    var dataObjectSave = SaveLoadData.CreateObjectData(p_saveObject);
    //    int indexNN = listDataObjectInField.Count + 1;

    //    if (objDataOld == null)
    //    {
    //        //new object
    //        //Name prefab in design game
    //        // p_saveObject.tag + "_" + p_saveObject.name + "_" + p_nameFiled + indexNN;
    //        dataObjectSave.NameObject += indexNN;
    //        listDataObjectInField.Add(dataObjectSave);
    //    }
    //    else
    //    {
    //        //update object
    //        objDataOld = dataObjectSave;
    //    }
    //    Counter++;

    //}

    //-------------------------------------------

    //private GameObject CreatePrefabByName(string typePrefab, string namePrefab, Vector3 pos = new Vector3())
    //{
    //    //Debug.Log("# CreatePrefabByName REAL ++++++++ " + namePrefab + " " + typePrefab + "  in  pos=" + pos);

    //    //#TEST #PREFABF
    //    //---------------- 1.
    //    //GameObject newPrefab = FindPrefab(typePrefab);
    //    //GameObject newObjGame = (GameObject)Instantiate(newPrefab, pos, Quaternion.identity);
    //    //----------------2.
    //    GameObject newObjGame = FindPrefab(typePrefab);
    //    newObjGame.transform.position = pos;
    //    //----------------

    //    newObjGame.name = namePrefab;
    //    //newObjGame.SetActive(false);
    //    return newObjGame;
    //}
    //------------------------
    //public static ObjectData CreateObjectData_(GameObject p_gobject)
    //{
    //    //Debug.Log("# CreateObjectData from " + p_gobject.name + " " + p_gobject.tag);
    //    ObjectData newObject = new ObjectData()
    //    {
    //        NameObject = p_gobject.name,
    //        TagObject = p_gobject.tag,
    //        Position = p_gobject.transform.position
    //    };
    //    return newObject;
    //}
    //----------------------------
    //public Dictionary<string, List<GameObject>> GamesObjectsActive;
    //public void SaveGrid(Dictionary<string, List<GameObject>> p_gamesObjectsActive)
    //private void SaveGrid()
    //{
    //    Debug.Log("# SaveGrid...");

    //    Dictionary<string, List<GameObject>> p_gamesObjectsActive = _scriptGrid.GamesObjectsActive;

    //    List<FieldData> listFields = new List<FieldData>();

    //    Debug.Log("# SaveGrid count object=" + p_gamesObjectsActive.Count);

    //    foreach (var item in p_gamesObjectsActive)
    //    {
    //        List<GameObject> gobjects = item.Value;
    //        var nameFiled = item.Key;

    //        FieldData fieldData;

    //        fieldData = listFields.Find(p => p.NameField == nameFiled);
    //        //create new Field in data
    //        if (fieldData == null)
    //        {
    //            fieldData = new FieldData() { NameField = nameFiled};
    //            listFields.Add(fieldData);
    //        }

    //        if(gobjects.Count>0)
    //            Debug.Log("# SaveGrid " + nameFiled + " add object=" + gobjects.Count);

    //        foreach (var obj in gobjects)
    //        {
    //            ObjectData objectSave = CreateObjectData(obj);
    //            fieldData.Objects.Add(objectSave);
    //        }
    //    }

    //    GridData data = new GridData()
    //    {
    //        Fields = listFields
    //    };

    //    Serializator.SaveXml(data, _datapath);
    //}

    //private void LoadDataGrid()
    //{
    //    //_datapath = Application.dataPath + "/Saves/SavedData" + Application.loadedLevel + ".xml";
    //    if (_gridData == null)
    //    {
    //        Debug.Log("# LoadDataGrid... gridData IS EMPTY");
    //        return;
    //    }

    //    Debug.Log("# LoadDataGrid... " + _datapath);

    //    Dictionary<string, List<GameObject>> _gamesObjectsActive = new Dictionary<string, List<GameObject>>();
    //    foreach (var field in _gridData.Fields)
    //    {
    //        Debug.Log("# LoadDataGrid field: " + field.NameField);

    //        List<GameObject> ListNewObjects = new List<GameObject>();
    //        foreach (ObjectData objGame in field.Objects)
    //        {
    //            Debug.Log("# LoadDataGrid objGame: " + objGame.NameObject + "   " + objGame.TagObject);

    //            GameObject newObjGame = CreatePrefabByObjectData(objGame);
    //            if (newObjGame != null)
    //                ListNewObjects.Add(newObjGame);
    //        }
    //        _gamesObjectsActive.Add(field.NameField, ListNewObjects);

    //    }
    //    _scriptGrid.GamesObjectsActive = _gamesObjectsActive;

    //}
    //--------------------------------------

    //private void CreateGamesObjectsWorld()
    //{
    //    Dictionary<string, List<GameObject>> _gamesObjectsActive = new Dictionary<string, List<GameObject>>();
    //    int maxWidth = 100;// (int)GridY * -1;
    //    int maxHeight = 100; //(int)GridX;
    //    int coutCreateObjects = 0;


    //    Debug.Log("# CreateGamesObjectsWorld...");

    //    for (int y = 0; y < maxWidth; y++)
    //    {
    //        for (int x = 0; x < maxHeight; x++)
    //        {
    //            int intRndCount = UnityEngine.Random.Range(0, 3);
    //            //Debug.Log("CreateGamesObjectsWorld intRndCount intRndCount=" + intRndCount);

    //            int maxObjectInField = (intRndCount==0)? 1: 0;
    //            //string nameFiled  = "Filed" + x + "x" + Mathf.Abs(y);
    //            string nameFiled  = GenerateGridFields.GetNameField(x,y);

    //            List<GameObject> ListNewObjects = new List<GameObject>();
    //            for(int i=0; i< maxObjectInField; i++){

    //                //Type prefab
    //                //int intTypePrefab = UnityEngine.Random.Range(1, 3);
    //                int intTypePrefab = UnityEngine.Random.Range(1, 4);
    //                //DebugLogT("CreateGamesObjectsWorld  " + nameFiled + "  intTypePrefab=" + intTypePrefab);

    //                TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;
    //                //DebugLogT("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName);

    //                int _y = y*(-1);
    //                Vector3 pos = new Vector3(x, _y, 0) * Spacing;
    //                pos.z = -1;
    //                if (prefabName == TypePrefabs.PrefabUfo)
    //                    pos.z = -2;

    //                //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

    //                string nameOnject = prefabName.ToString() + "_" + nameFiled + "_" + i;
    //                ObjectData objGame = new ObjectData()
    //                {
    //                    NameObject = nameOnject,
    //                    TagObject = prefabName.ToString(),
    //                    //Position = new Vector3(x, y*(-1), -1) 
    //                    Position = pos 
    //                };
    //                GameObject newObjGame = CreatePrefabByObjectData(objGame);
    //                if (newObjGame != null)
    //                {
    //                    ListNewObjects.Add(newObjGame);
    //                    //Debug.Log("CreateGamesObject IN Data World ++++ " + nameFiled + "   " + nameOnject);
    //                    coutCreateObjects++;
    //                }
    //            }
    //            _gamesObjectsActive.Add(nameFiled, ListNewObjects);
    //        }
    //    }
    //    _scriptGrid.GamesObjectsActive = _gamesObjectsActive;
    //    _scriptGrid.GridData = _gridData;

    //    Debug.Log("CreateGamesObject IN Data World COUNT====" + coutCreateObjects + "     count fields: " + _scriptGrid.GamesObjectsActive.Count);

    //    //step 2.
    //    //SaveGrid();

    //    //step 3.
    //    //LoadDataGrid();
    //}
    //------------------------------------------
    //#TEST
    //public static GameObject CreatePrefabByObjectData(ObjectData objGameData)
    //public GameObject CreatePrefabByObjectData(ObjectData objGameData)
    //{
    //    string nameFind = objGameData.NameObject;
    //    string tagFind = objGameData.TagObject;
    //    Vector3 pos = objGameData.Position;
    //    GameObject newPrefab = null;

    //    string typeFind = String.IsNullOrEmpty(tagFind) ? nameFind : tagFind;

    //    newPrefab = FindPrefab(typeFind);

    //    if (newPrefab == null)
    //    {
    //        Debug.Log("# CreatePrefabByObjectData Not Find Prefab =" + typeFind);
    //        return null;
    //    }

    //    GameObject newObjGame = (GameObject)Instantiate(newPrefab, pos, Quaternion.identity);
    //    newObjGame.name = nameFind;
    //    //Hide active object
    //    newObjGame.SetActive(false);

    //    return newObjGame;
    //}

    //---------------------------

    //#PPP
    //[XmlType("Ufo")]
    //public class ObjectDataUfo : ObjectData
    //{
    //    [XmlIgnore]
    //    public Color ColorRender = Color.black;
    //    [XmlIgnore]
    //    public Vector3 TargetPosition;

    //    private Vector3 m_Position = new Vector3(0, 0, 0);
    //    public override Vector3 Position 
    //    {
    //        get { return m_Position; }
    //        set { 
    //            m_Position = value;
    //            if (IsCanSetTargetPosition)
    //                SetTargetPosition();
    //        }
    //    }

    //    private bool IsCanSetTargetPosition {
    //        get {
    //            return (TargetPosition == null || TargetPosition == new Vector3(0, 0, 0)) && m_Position != null && m_Position != new Vector3(0, 0, 0);
    //        }
    //    }

    //    public PersonDataUfo() : base()
    //    {
    //        ColorRender = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);

    //        //if (IsCanSetTargetPosition)
    //        //{
    //        //    //Debug.Log("UFO set TargetPosition after init");
    //        //    SetTargetPosition();
    //        //}
    //    }

    //    public void SetTargetPosition()
    //    {
    //        int distX = UnityEngine.Random.Range(-15, 15);
    //        int distY = UnityEngine.Random.Range(-15, 15);

    //        float x = m_Position.x + distX;
    //        float y = m_Position.y + distY;
    //        if (y > -1)
    //            y = m_Position.y - distY;
    //        if (x < 1)
    //            x = m_Position.x - distX;

    //        TargetPosition = new Vector3(x, y, -1);
    //        //Debug.Log("UFO SetTargetPosition ==== " + TargetPosition);
    //    }

    //    public override void UpdateGameObject(GameObject objGame)
    //    {
    //        objGame.GetComponent<SpriteRenderer>().color = ColorRender;
    //        //Debug.Log("UPDATE CLONE THIS " + ((ObjectDataUfo)this).ToString());
    //        objGame.GetComponent<PersonalData>().PersonalObjectData = (PersonDataUfo)this.Clone();
    //    }

    //    public override string ToString()
    //    {
    //        return NameObject + " " + TagObject + " " + Position + " " + ColorRender;
    //        //return base.ToString();
    //    }
    //}

    //-----------------------------------------------
    //private void DebugLog(string log)
    //{
    //    Debug.Log(log);
    //}

    //private void DebugLogT(string log)
    //{
    //    return;
    //    Debug.Log(log);
    //}

    //-------------------------------

    /*
                    //TEST --------------------------
                    Debug.Log("****** NextPosition (" + gobj.name + ") IsReality=" + IsReality);

                    string idObj = Storage.GetID(gobj.name);
                    //GameObject gobjRealName = Storage.Instance.GamesObjectsReal[posFieldName].Find(p => p.name == gobj.name);

                    //-----------------------FIND In REAL DATA
                    GameObject gobjRealName = null;
                    if(Storage.Instance.GamesObjectsReal.ContainsKey(posFieldName))
                        gobjRealName = Storage.Instance.GamesObjectsReal[posFieldName].Find(p => { return p.name.IndexOf(idObj) != -1; });
                    else
                        Debug.Log("****** NextPosition (" + gobj.name + ") Not Real Field : " + posFieldName);

                    GameObject gobjOldPos = null;// Storage.Instance.GamesObjectsReal[posFieldOld].Find(p => p.name == gobj.name);
                    if(Storage.Instance.GamesObjectsReal.ContainsKey(posFieldOld))
                        gobjOldPos = Storage.Instance.GamesObjectsReal[posFieldOld].Find(p => p.name == gobj.name);
                    else
                        Debug.Log("****** NextPosition (" + gobj.name + ") Not Real Field : " + posFieldOld);

                    if(gobjRealName!=null)
                        Debug.Log("******** NextPosition (" + gobj.name + ")  Exist real object in field: " + posFieldName);
                    if (gobjOldPos != null)
                        Debug.Log("******** NextPosition (" + gobj.name + ")  Exist real object in field: " + posFieldOld);
                    if (gobjRealName != null && gobjOldPos != null)
                    {
                        Debug.Log("******** Destroy dublicat : " + gobj.name);
                        Storage.Instance.KillObject.Add(gobj.name);
                        Destroy(gobj);
                        //Storage.Instance.AddDestroyRealObject(gobj);
                    }

                    //return "";

                    if (gobjRealName == null && gobjOldPos == null)
                    {
                        Debug.Log("******** (" + idObj + ") NOT FOUND Real in Fields: " + posFieldName + "  &  " + posFieldOld);
                        if (IsReality)
                        {
                            //-----------------------FIXED Correct
                            Debug.Log("+++++ CORRECT ++++  (" + idObj + ") >>>>  Add in Real Object Fields: " + posFieldName);
                            Storage.Instance.AddRealObject(gobj, posFieldName, "NextPosition");
                        }
                    }
                    //-----------------------FIND In DATA

                    SaveLoadData.ObjectData dataObjRealName = null;
                    if (Storage.Instance.GridDataG.FieldsD.ContainsKey(posFieldName))
                    {
                        dataObjRealName = Storage.Instance.GridDataG.FieldsD[posFieldName].Objects.Find(p => { return p.NameObject.IndexOf(idObj) != -1; });
                        if (dataObjRealName != null)
                            Debug.Log("******** NextPosition (" + idObj + ") RealName Exist Data in field: " + posFieldName);
                    }
                    else
                        Debug.Log("****** NextPosition (" + gobj.name + ")  RealName Not DATA Field : " + posFieldName);
                    SaveLoadData.ObjectData dataObjOldPos = null;
                    if (Storage.Instance.GridDataG.FieldsD.ContainsKey(posFieldOld))
                    {
                        dataObjOldPos = Storage.Instance.GridDataG.FieldsD[posFieldOld].Objects.Find(p => { return p.NameObject.IndexOf(idObj) != -1; });
                        if (dataObjOldPos != null)
                            Debug.Log("******** NextPosition (" + idObj + ") OldPos Exist Data in field: " + posFieldOld);
                    }
                    else
                        Debug.Log("****** NextPosition (" + gobj.name + ") OldPos Not DATA Field : " + posFieldOld);

                    //-----------------------FIXED Correct
                    foreach (var item in Storage.Instance.GridDataG.FieldsD)
                    {
                        string nameField = item.Key;
                        List<SaveLoadData.ObjectData> resListData = Storage.Instance.GridDataG.FieldsD[nameField].Objects.Where(p => { return p.NameObject.IndexOf(idObj) != -1; }).ToList();
                        if (resListData != null)
                        {
                            //foreach (var obj in resListData)
                            for(int i=0; i< resListData.Count() ; i++)
                            {
                                var obj =  resListData[i];
                                //Debug.Log("----------- Exist " + idObj + " in Data Field: " + nameField + " --- " + obj.NameObject);
                                if (nameField != posFieldName)
                                {
                                    //if (obj.NameObject != nameObject)
                                    //{
                                        Debug.Log("+++++ CORRECT ++++  DELETE (" + idObj + ") >>>> in DTA Object Fields: " + nameField + "     obj=" + obj);
                                        Storage.Instance.RemoveDataObjectInGrid(nameField, i, "NextPosition");
                                    //}
                                }
                            }
                        }
                    }
                    //---------------------
                                        
                    if (dataObjRealName == null)
                    {
                        IsReality = true;
                        //-----------------------FIXED Correct
                        Debug.Log("+++++ CORRECT ++++  (" + idObj + ") >>>>  Add in DTA Object Fields: " + posFieldName);
                        Storage.Instance.AddDataObjectInGrid(this, posFieldName, "NextPosition");
                    }

                    Debug.Log("+++++ CORRECT ++++  (" + gobj.name + ")  Update This DATA -->  Position and Name");
                    this.NameObject = gobj.name;
                    this.Position = gobj.transform.position;
                    return "Update";
                    */

    //----------------------------
    //---calculation
    //============================
    /*
    string testText = "";


    float diffX = 1;
    float diffY = 1;

    int Rows = 10;
    int Columns = 18;
    float sizeW = ScreenWidth / Columns;
    float sizeH = ScreenHeight / Rows;
    float korrectSizeY = 0;// -10;
    float korrectSizeX = 0;
    //float korrectSizeY = _diffCenterY * 10;// -10;
    //float korrectSizeX = _diffCenterX * -10;
    //float korrectSizeX = (_diffCenterX < 0) ?
    //    _diffCenterX * -10 :
    //    _diffCenterX * 10;
    //float korrectSizeX = 0;
    float mX = (_MousePosition.x + korrectSizeX);
    float mY = ScreenHeight - _MousePosition.y + korrectSizeY;

    diffX = (int)(mX / sizeW);
    diffY = (int)(mY / sizeH);
    //}
    */

    //Vector2 posCursorToField = CalculatePositionCursorToField((int)diffX, (int)diffY, 1);



    //string findField = Helper.GetNameField(fieldPosNormaliz.x, fieldPosNormaliz.y);

    // + "\nfindField: " + findField;
    //_infoPoint = testText;

    //============================


    //private Vector2 CalculatePositionCursorToField(float zoom)
    //{
    //    float positionMx = _rectCursor.x / 28.4f;
    //    float positionMy = _rectCursor.y / 28.4f;
    //    Vector2 posCursorToField = Helper.NormalizPosToField(positionMx, positionMy);
    //    int centerHeroX = 8;
    //    int centerHeroY = 5;
    //    int offsetX = (int)_PosHeroToField.x - centerHeroX;
    //    int offsetY = (int)_PosHeroToField.y - centerHeroY;
    //    posCursorToField += new Vector2(offsetX * zoom, offsetY * zoom);
    //    return posCursorToField;
    //}

    //private Vector2 CalculatePositionCursorToField(int x, int y, float zoom)
    //{
    //    int centerHeroX = 8;
    //    int centerHeroY = 5;
    //    int offsetX = (int)_PosHeroToField.x - centerHeroX;
    //    int offsetY = (int)_PosHeroToField.y - centerHeroY;
    //    return new Vector2(x,y) + new Vector2(offsetX * zoom, offsetY * zoom);
    //}

    //============================

    //private void GetMousePositionOnScene_()
    //{
    //    string errInd = "satrt";
    //    try
    //    {
    //        //return;
    //        errInd = "1";
    //        if (Event.current == null)
    //        {
    //            //Debug.Log("########## Error GetMousePositionOnScene Event.current==null");
    //            return;
    //        }
    //        errInd = "1.2";
    //        if (Event.current.button == null)
    //        {
    //            Debug.Log("########## Error GetMousePositionOnScene Event.current.button");
    //            return;
    //        }
    //        errInd = "1.3";
    //        if (Event.current.type != EventType.MouseDown || Event.current.button != 0)
    //            return;

    //        errInd = "2";
    //        // convert GUI coordinates to screen coordinates
    //        Vector3 screenPosition = Event.current.mousePosition;
    //        errInd = "3";
    //        if (Camera.current == null)
    //        {
    //            Debug.Log("########## Error GetMousePositionOnScene Camera.current = null");
    //            return;
    //        }

    //        screenPosition.y = Camera.current.pixelHeight - screenPosition.y;
    //        //screenPosition.y = MainCamera.current.pixelHeight - screenPosition.y;
    //        errInd = "4";
    //        Ray ray = Camera.current.ScreenPointToRay(screenPosition);
    //        errInd = "5";
    //        RaycastHit hit;
    //        errInd = "6";
    //        // use a different Physics.Raycast() override if necessary
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            errInd = "7";
    //            // do stuff here using hit.point
    //            // tell the event system you consumed the click
    //            Event.current.Use();
    //        }
    //        errInd = "8";
    //    }
    //    catch (Exception x)
    //    {
    //        Debug.Log("########## Error GetMousePositionOnScene (" + errInd + ") " + x.Message + "");
    //    }
    //}

    //-------------------------
    //private void CalculateDiffCenterHero()
    //{
    //    int scale = 2;
    //    int posX = 0;
    //    int posY = 0;
    //    posX = (int)((transform.position.x / scale));
    //    posY = (int)((transform.position.y / scale));

    //    float restX = (posX * scale);
    //    float restY = (posY * scale);

    //    string textTest = "";
    //    _diffCenterX = transform.position.x - restX;
    //    _diffCenterY = transform.position.y - restY;
    //    float diffTestX;
    //    float diffTestY;
    //    float TestX = 1;
    //    float TestY = 1;

    //    if (Math.Abs(_diffCenterX) < 1)
    //    {
    //        diffTestX = -1;
    //        TestX = Math.Abs(_diffCenterX) * 100;
    //        TestX = (float)Math.Round(TestX, 2);
    //    }
    //    else
    //    {
    //        diffTestX = 1;
    //        TestX = (Math.Abs(_diffCenterX) - 1) * 100;
    //        TestX = (float)Math.Round(TestX, 2);
    //    }
    //    if (Math.Abs(_diffCenterY) < 1)
    //    {
    //        diffTestY = -1;
    //        TestY = Math.Abs(_diffCenterY) * 100;
    //        TestY = (float)Math.Round(TestY, 2);
    //    }
    //    else
    //    {
    //        diffTestY = 1;
    //        TestY = (Math.Abs(_diffCenterY) - 1) * 100;
    //        TestY = (float)Math.Round(TestY, 2);
    //    }

    //    textTest = "\nDiff Center Hero=\nX=(" + _diffCenterX + ")\n" + diffTestX + "x" + diffTestY + "\nY=(" + _diffCenterY + ")" +
    //        "\n diffX=" + TestX +
    //        "\n diffY=" + TestY;

    //    SetTextLog("?" + _fieldHero + " " + textTest);
    //}
    //-------------------------

    //public IEnumerable<GameObject> GetAllRealPersons(string field)
    //{
    //    var count1 = Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).ToList().Count();
    //    //Debug.Log("PERSON PAIR (" + field + ")  COUNT " + count1);

    //    //var listT1=  Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).ToList();
    //    //foreach (var t3 in listT1.SelectMany(x => x.Value).ToList())
    //    foreach (var t3 in Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).SelectMany(x => x.Value).ToList())
    //    {
    //        Debug.Log("PERSON(" + field + ") 2.: " + t3);
    //    }

    //    //foreach(var pair in listT1)
    //    //{
    //    //    foreach (var obj in pair.Value)
    //    //    {
    //    //        Debug.Log("PERSON PAIR (" + field + ") 1.: " + obj.name);
    //    //    }
    //    //}

    //    //foreach (var listM in listT1.Select(p => p.Value.Select(c => c.name)))
    //    //{
    //    //    foreach (var obj in listM)
    //    //    {
    //    //        Debug.Log("PERSON  VALUES(" + field + ") 1.: " + obj);
    //    //    }

    //    //}
    //    //var listT2 = listT1.SelectMany(x => x.Value).ToList();
    //    //foreach (var t3 in listT2)
    //    //{
    //    //    Debug.Log("PERSON(" + field + ") 2.: " + t3);
    //    //}


    //    //var listT3 = listT2.Where(p => p.tag.ToString() == _Ufo || p.tag.ToString() == _Boss);

    //    //foreach (var t3 in listT3)
    //    //{
    //    //    Debug.Log("PERSON(" + field + ") 3.: " + t3);
    //    //}

    //    return Storage.Instance.GamesObjectsReal.Where(p => p.Key == field).
    //            SelectMany(x => x.Value).
    //            Where(p => p.tag == _Ufo || p.tag == _Boss).ToList();
    //}
    //------------------------
    /*
 public void AddExpand(string tittle, List<string> listText, List<Button> listButtonCommand)
 {
     if (PrefabExpandPanel == null)
     {
         Debug.Log("########### PrefabExpandPanel is Empty");
         return;
     }

     //GameObject contentListExpandPerson;
     //GameObject PrefabExpandPanel;

     Vector3 pos = new Vector3(0, 0, 0);
     GameObject expandGO = (GameObject)Instantiate(PrefabExpandPanel, pos, Quaternion.identity);
     //GameObject expandGO = (GameObject)Instantiate(PrefabExpandPanel, pos, Quaternion.identity);
     //resGO.name = nameGO;
     //expandGO.Find
     if (expandGO == null)
     {
         Debug.Log("########### expandGO is Empty");
         return;
     }

     if (expandGO.transform == null)
     {
         Debug.Log("########### expandGO.transform = null");
         return;
     }

     var transContentExpandGO = expandGO.transform.Find("ContentListLogCommand");

     if (transContentExpandGO == null)
         transContentExpandGO = GameObject.Find("ContentListLogCommand").transform;

     //transContentExpandGO = expandGO.Find("ContentListLogCommand").transform;

     if (transContentExpandGO == null)
     {
         Debug.Log("########### NOT Find trans ContentExpand ");
         return;
     }

     GameObject contentExpandGO = transContentExpandGO.gameObject;
     if (contentExpandGO == null)
     {
         Debug.Log("########### Content Expand is Empty");
         return;
     }

     Transform transExpandButton = expandGO.transform.Find("textExpanderButton");

     GameObject ExpandButton = null;
     if (transExpandButton == null)
     {
         ExpandButton = GameObject.Find("textExpanderButton");
     }
     else
     {
         ExpandButton = transExpandButton.gameObject;
     }

     if (ExpandButton == null)
     {
         Debug.Log("########### textBlock Expand is Empty");
         return;
     }

     //tbExpand = transExpand.gameObject;
     //if (tbExpand == null)
     //{
     //    Debug.Log("########### textBlock Expand is Empty");
     //    return;
     //}
     Text textExpanderButton = ExpandButton.GetComponent<Text>();
     if (textExpanderButton == null)
     {
         Debug.Log("########### textBlock Expand GetComponent<Text> is Empty");
         return;
     }
     textExpanderButton.text = tittle;

     foreach (string text in listText)
     {
         CreateCommandLogText(text, Color.white, contentExpandGO.transform);
     }

     //resGO.text = p_text;
     //expand.transform.SetParent(contentListExpandPerson);
 }
 */

    //public GameObject GetFindExpandContent(Transform transExpand)
    //{
    //    for (var child in transform.gameObject)
    //    {
    //        if (child.name == "Bone")
    //        {
    //            // the code here is called 
    //            // for each child named Bone
    //            return child;
    //        }
    //    }
    //    return null;
    //}

}
