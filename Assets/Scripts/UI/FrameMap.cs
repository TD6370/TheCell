using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameMap : MonoBehaviour {

    public GameObject MapCellFrame;

    public float KoofPosCell = 1f;

    public float SizeZoom = 1f;
    [System.NonSerialized]
    public Vector2 SelectPointField = new Vector2(0, 0);
    [System.NonSerialized]
    public Vector2 SelectFieldPos = new Vector2(0, 0);

    public bool IsRuntimeViewMarker = true;

    public float OffSetZomm1 = 0f;
    public float OffSetZomm11 = 4f;
    public float OffSetZomm12 = 8f;
    public float OffSetZomm13 = 11f;
    public float OffSetZomm14 = 13.8f;
    public float OffSetZomm15 = 16f;
    public float OffSetZomm16 = 18.2f;
    public float OffSetZomm17 = 20f;
    public float OffSetZomm18 = 22f;
    public float OffSetZomm19 = 23f;
    public float OffSetZomm2 = 24.6f;
    public float OffSetZomm21 = 26.6f;
    public float OffSetZomm22 = 26.8f;
    public float OffSetZomm23 = 27.8f;
    public float OffSetZomm24 = 28.8f;
    public float OffSetZomm25 = 30f;

    public float OffSetZomm08 = -12f;
    public float OffSetZomm09 = -5f;

    public float OffsetCell11 = 2.5f;
    public float OffsetCell12 = 5f;
    public float OffsetCell13 = 7.5f;
    public float OffsetCell14 = 10f;
    public float OffsetCell15 = 12f;
    public float OffsetCell16 = 14f;
    public float OffsetCell17 = 17f;
    public float OffsetCell18 = 20f;
    public float OffsetCell19 = 22f;
    public float OffsetCell2 = 25f;
    public float OffsetCell21 = 27f;
    public float OffsetCell22 = 29.5f;
    public float OffsetCell23 = 31.5f;
    public float OffsetCell24 = 35f;
    public float OffsetCell25 = 37f;

    //Mouse wheel
    float upLevel = 0;
    float speedWheel = 0.02f;
    float limitZoomMax = 2.5f;
    float limitZoomMin = 0.8f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {

        if (!IsActive)
        {
            return;
        }

        OnMauseWheel();

        if (Input.GetMouseButtonDown(0))
        {
            MouseDownOnChange();
        }

        if (Input.GetMouseButtonDown(1))
        {
            //After CalculatePointOnMap
            MouseDownSpecOnChange();
        }
    }

    private void FixedUpdate()
    {
        if (IsActive && IsRuntimeViewMarker)
        {
            CalculatePointOnMap();
        }
    }

    private void MouseDownOnChange()
    {
        CalculatePointOnMap();

        ShowSelectorCell();
    }

    private void MouseDownSpecOnChange()
    {
        RunTeleportHero();
    }

    private void RunTeleportHero()
    {
        if (Storage.Events.IsCommandTeleport)
        {
            Storage.Map.Create();
            int posTransferHeroX = (int)(Storage.Map.SelectPointField.x * Storage.ScaleWorld);
            int posTransferHeroY = (int)(Storage.Map.SelectPointField.y * Storage.ScaleWorld);
            posTransferHeroY *= -1;
            Storage.Player.TeleportHero(posTransferHeroX, posTransferHeroY);
        }
    }

    private void OnMauseWheel()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel != 0) // back
        {
            //Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 1);
            if (wheel > 0)
            {
                SizeZoom += 0.1f;
                if (SizeZoom > limitZoomMax)
                    SizeZoom = limitZoomMax;
            }
            else
            {
                SizeZoom -= 0.1f;
                if (SizeZoom < limitZoomMin)
                    SizeZoom = limitZoomMin;
            }
            Zooming(SizeZoom);
        }
    }

    private bool IsActive
    {
        get
        {
            Camera cameraMap = Storage.PlayerController.CameraMap;
            if (cameraMap == null)
            {
                return false; ;
            }
            return cameraMap.enabled;
        }
    }

    private void CalculatePointOnMap()
    {
        Camera cameraMap = Storage.PlayerController.CameraMap;
        if (cameraMap == null)
        {
            Debug.Log("################ cameraMap is EMPTY ");
            return;
        }
        if (!cameraMap.enabled)
            return;

        bool isHitCollider = false;
        float mapX = 0;
        float mapY = 0;
        Ray ray1 = cameraMap.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit1 = Physics2D.GetRayIntersection(ray1, Mathf.Infinity);

        if (hit1.collider != null && hit1.collider.transform == this.gameObject.transform)
        {
            BoxCollider2D colliderMap = GetComponent<BoxCollider2D>();
            if (colliderMap != null)
            {

                isHitCollider = true;
                NormalizedMapPoint(hit1, colliderMap, out mapX, out mapY);

                Storage.Events.ListLogAdd = "MAP ORIGINAL: pos = " + mapX + "x" + mapY + "  Zoom: " + SizeZoom;

                if (SizeZoom == 1)
                {

                }
                else if (SizeZoom > 1f)
                {

                    float _zoom = SizeZoom;

                    #region Test
                    //TEST --------------------
                    //float mapY_T1 = (int)mapY;
                    //float mapX_T1 = (int)mapX;
                    //mapY_T1 = (int)(mapY_T1 / _zoom);
                    //mapX_T1 = (int)(mapX_T1 / _zoom);

                    //Storage.Events.ListLogAdd = "Corrr zoom T1= " + (int)mapX_T1 + "x" + (int)mapY_T1;

                    //float mapY_T2 = (int)(mapY / _zoom);
                    //float mapX_T2 = (int)(mapX / _zoom);

                    //Storage.Events.ListLogAdd = "Corrr zoom T2= " + (int)mapX_T2 + "x" + (int)mapY_T2;
                    //---------------------------
                    #endregion

                    mapY = (mapY / _zoom);
                    mapX = (mapX / _zoom);

                    _zoom = (float)System.Math.Round(_zoom, 1);

                    float offsetCenter = 0f;

                    //Debug.Log("_zoom===" + _zoom);
                    offsetCenter = OffsetZoomUp(_zoom);

                    Storage.Events.ListLogAdd = "Corrr zoom: " + (int)mapX + "x" + (int)mapY + "  offset= " + offsetCenter + " zoom: " + _zoom;

                    //mapX = (int)mapX;
                    //mapY = (int)mapY;

                    //!!! CORR DISTANCE
                    int centrW = Helper.HeightLevel / 2;
                    Vector3 centerPos = new Vector3(centrW, centrW, 0);
                    float koofOnCenterX = centerPos.x / mapX;
                    float koofOnCenterY = centerPos.y / mapY;
                    Storage.Events.ListLogAdd = "Map koofOnCenter: " + koofOnCenterX + " x " + koofOnCenterY;
                    //------------------

                    mapX += offsetCenter;
                    mapY += offsetCenter;
                }
                else
                {
                    float _zoom = SizeZoom;

                    //mapX = (int)mapX;
                    //mapY = (int)mapY;

                    mapY = (int)(mapY / _zoom);
                    mapX = (int)(mapX / _zoom);

                    float offsetCenter = OffsetZoomDown(_zoom);

                    Storage.Events.ListLogAdd = "Corrr zoom:  " + (int)mapX + "x" + (int)mapY + "  offsetCenter= " + offsetCenter;

                    mapX += offsetCenter;
                    mapY += offsetCenter;
                }

                Storage.Events.ListLogAdd = "MAP pos = " + mapX + "x" + mapY;
                //Debug.Log("MAP pos = " + mapX + "x" + mapY);
            }
        }
        else
        {
            if (!IsRuntimeViewMarker)
            {
                Storage.Events.ListLogAdd = "CalculatePointOnMap Not hit collider " + (int)mapX + "x" + (int)mapY;
            }
        }

        SelectPointField = new Vector2(mapX, mapY);
        SelectFieldPos = new Vector2((int)mapX, (int)mapY);
        Storage.Map.SelectPointField = SelectPointField;

        //if (Storage.Map.SelectFieldMap == "Field0x0")
        //{
        //    Debug.Log("######### CalculatePointOnMap FIELD=Field0x0     isHitCollider=" + isHitCollider);
        //}

        Storage.Map.UpdateMarkerPointCell();
    }

    private void NormalizedMapPoint(RaycastHit2D hit1, BoxCollider2D colliderMap, out float mapX, out float mapY)
    {
        float widthCellPexel = 25;
        float widthMap = colliderMap.size.x;
        float offSerX = (hit1.point.x - transform.position.x);
        float radiusX = widthMap / 2;
        mapX = offSerX + radiusX;
        float heightMap = colliderMap.size.y;
        float offSerY = (hit1.point.y - transform.position.y);
        float radiusY = heightMap / 2;
        mapY = offSerY + radiusY;
        mapY = widthCellPexel - mapY;

        mapX = mapX / widthCellPexel * Helper.WidthLevel;
        mapY = mapY / widthCellPexel * Helper.HeightLevel;
    }

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

    private float OffsetZoomDown(float _zoom)
    {
        float offsetCenter = 0;

        if (_zoom >= 0.8f)
            offsetCenter = OffSetZomm08;
        //if (_zoom >= 0.85f)
        //    offsetCenter = -7f;
        //if (_zoom >= 0.95f)
        //    offsetCenter = -4f;
        if (_zoom >= 0.9f)
            offsetCenter = OffSetZomm09;
        return offsetCenter;
    }

    private float OffsetZoomUp(float _zoom)
    {
        float offsetCenter = 0;
        if (_zoom >= 1f)
            offsetCenter = OffSetZomm1;
        if (_zoom >= 1.1f)
            offsetCenter = OffSetZomm11;// 5f;
        if (_zoom >= 1.2f)
            offsetCenter = OffSetZomm12; // 10f;
        if (_zoom >= 1.3f)
            offsetCenter = OffSetZomm13;// 12f;
        if (_zoom >= 1.4f)
            offsetCenter = OffSetZomm14; //15f
        if (_zoom >= 1.5f)
            offsetCenter = OffSetZomm15;
        if (_zoom >= 1.6f)
            offsetCenter = OffSetZomm16;
        if (_zoom >= 1.7f)
            offsetCenter = OffSetZomm17;
        if (_zoom >= 1.8f)
            offsetCenter = OffSetZomm18;
        if (_zoom >= 1.9f)
            offsetCenter = OffSetZomm19;
        //if ((int)_zoom >= 2f)
        //    offsetCenter = 25.5f;// 25f;
        if (_zoom >= 2f)
            offsetCenter = OffSetZomm2;// 25f;
        if (_zoom >= 2.1f)
            offsetCenter = OffSetZomm21;// 25f;
        if (_zoom >= 2.2f)
            offsetCenter = OffSetZomm22;// 25f;
        if (_zoom >= 2.3f)
            offsetCenter = OffSetZomm23;// 25f;
        if (_zoom >= 2.4f)
            offsetCenter = OffSetZomm24;// 25f;
        if (_zoom >= 2.5f)
            offsetCenter = OffSetZomm25;// 25f;
        return offsetCenter;
    }

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

    void OnMouseDown()
    {
        //Print
        Storage.Events.ListLogAdd = "MAP OnMouseDown";
    }

    private void Zooming(float zoom = 1f)
    {
        Storage.Map.ZoomMap = SizeZoom;

        //this.gameObject.transform.localScale = new Vector3(zoom, zoom, 0);
        this.gameObject.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(zoom, zoom, 0);
    }

    //9.4 5.7
    Vector3 posOld = new Vector3();

    private void ShowSelectorCell()
    {
        string nameField = Storage.Map.SelectFieldMap;

        if(nameField== "Field0x0")
        {
            Debug.Log("######### ShowSelectorCell FIELD=Field0x0");
        }

        //nameField = "Field20x50";

        if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
        {
            //if(!Storage.Instance.TestExistField(nameField))
            //{
            //    Debug.Log("");
            //}

            //DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, prefabType);
            //continue;
            Debug.Log("Selector Cell Field " + nameField + " is Empty     " + DateTime.Now);
            Storage.Events.ListLogAdd = "Selector Cell Field " + nameField + " is Empty     " + DateTime.Now;
            return;
        }

        //Celect Cell on World
        Storage.Person.VeiwCursorGameObjectData(nameField);

        //Draw Icon on Cell Map

        SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
        List<SaveLoadData.TypePrefabs> fieldListPrefbs = new List<SaveLoadData.TypePrefabs>();
        List<Texture2D> listPersonsMapTexture = new List<Texture2D>();

        foreach (ModelNPC.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
        {
            Debug.Log("Selector Cell : " + datObjItem.NameObject + "  " + DateTime.Now);

            prefabType = SaveLoadData.TypePrefabs.PrefabField;

            if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
            datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
            {
                prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
            }
            else
            {
                ModelNPC.GameDataBoss bossObj = datObjItem as ModelNPC.GameDataBoss;
                if (bossObj != null)
                {
                    prefabType = SaveLoadData.TypePrefabs.PrefabBoss;

                    fieldListPrefbs.Add(prefabType);

                    ////+++DRAW PERSON ---------------------------------
                    Texture2D personMapTexture = TypeBoss.Instance.GetNameTextureMapForIndexLevel(bossObj.Level);
                    if (personMapTexture == null)
                    {
                        Debug.Log("####### ShowSelectorCell Textute is Empty  TypeBoss.Instance.GetNameTextureMapForIndexLevel(" + bossObj.Level + ") ");
                        break;
                    }
                    listPersonsMapTexture.Add(personMapTexture);
                    //-----------------------------------------------------
                }
            }
            //fieldListPrefbs.Add(prefabType);
        }

        int cellX = (int)SelectPointField.x;
        int cellY = (int)SelectPointField.y;
        bool _isPerson = false;

        for (int indMap2D = 0; indMap2D < listPersonsMapTexture.Count; indMap2D++)
        {
            Texture2D texturePerson = listPersonsMapTexture[indMap2D];
            if (texturePerson == null)
            {
                Debug.Log("####### ShowSelectorCell Textute is Empty    listPersonsMapTexture[" + indMap2D + "] ");
                break;
            }

            // Draw Texture On Map
            //Storage.Map.DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, texturePerson);

            Storage.Map.DrawMapCell(cellX, cellY, texturePerson);

            Sprite spriteCell = Sprite.Create(texturePerson, new Rect(0.0f, 0.0f, texturePerson.width, texturePerson.height), new Vector2(0.5f, 0.5f), 100.0f);
            MapCellFrame.GetComponent<SpriteRenderer>().sprite = spriteCell;
            _isPerson = true;
            //MapCellFrame.GetComponent<SpriteRenderer>().sprite.texture = texturePerson;
            break;
        }

        if (!_isPerson)
        {
            Texture2D texturePrefab = Storage.Map.GetPrefabTexture(prefabType);
            if (texturePrefab == null)
            {
                Debug.Log("###### ShowSelectorCell  prefabType:" + prefabType + " texturePrefab Is NULL ");
                //continue;
            }
            else
            {
                Sprite spriteCell = Sprite.Create(texturePrefab, new Rect(0.0f, 0.0f, texturePrefab.width, texturePrefab.height), new Vector2(0.5f, 0.5f), 100.0f);
                MapCellFrame.GetComponent<SpriteRenderer>().sprite = spriteCell;
            }

        }

        MapCellFrame.GetComponent<RectTransform>().position = SetLocationCell();
    }

    private Vector3 SetLocationCell()
    {
        //------------ Location cell
        Vector2 movementCell = new Vector3(-12.4f, 12.4f);

        //!!!!!!!!!!!!!!!
        //@ >>>
        //MapCellFrame.transform.SetParent(null);
        //MapCellFrame.transform.SetParent(this.gameObject.transform);
        //!!!!!!!!!!!!!!!

        if (posOld == new Vector3())
            posOld = MapCellFrame.GetComponent<RectTransform>().position;// = new Vector3(-2, -2, 0);

        int _koofPosCell = 2;

        float addX = (SelectPointField.x / (Storage.ScaleWorld * _koofPosCell));
        float addY = (SelectPointField.y / (Storage.ScaleWorld * _koofPosCell));

        float correctPosX = posOld.x + addX;
        float correctPosY = posOld.y - addY;

        float correctZomm = SizeZoom;

        if (correctZomm > 1)
        {
            Debug.Log("Save normal posit");
        }

        correctPosX -= Storage.Map.DistMoveCameraMap.x;
        correctPosY -= Storage.Map.DistMoveCameraMap.y;

        Vector3 newPos = new Vector3(correctPosX, correctPosY, -10);

        ValidateStartPosition();

        //Correct Offset Zoom
        if (correctZomm != 1)
        {
            int x = (int)SelectPointField.x;
            int y = (int)SelectPointField.y;

            int centrW = Helper.HeightLevel / 2;
            Vector3 centerPos = new Vector3(centrW, centrW, 0);

            float offSetOnCenterX = centerPos.x - x;
            float offSetOnCenterY = centerPos.y - y;

            float koofOnCenterX = centerPos.x / x;
            float koofOnCenterY = centerPos.y / y;
            if (koofOnCenterX < 0)
                koofOnCenterX += 1;
            if (koofOnCenterY < 0)
                koofOnCenterY += 1;

            Storage.Events.ListLogAdd = "New pos Cell: " + newPos.x + "x" + newPos.y;
            Storage.Events.ListLogAdd = "-- Cell koof: " + koofOnCenterX + "x" + koofOnCenterY;

            float korrCellX = offSetOnCenterX / 100;
            float korrCellY = offSetOnCenterY / 100;

            float OffsetCell = 1f;

            OffsetCell = GetOffsetCell(correctZomm);

            Storage.Events.ListLogAdd = ":: OffsetCell corr: % " + OffsetCell;

            korrCellX *= OffsetCell;
            korrCellY *= OffsetCell;

            newPos.x -= korrCellX;
            newPos.y += korrCellY;

            Storage.Events.ListLogAdd = ":: Cell corr: " + korrCellX + " x " + korrCellY;
        }

        
        Storage.Events.ListLogAdd = "***** Cell pos: " + newPos.x + "x" + newPos.y;
        //<<< @
        //MapCellFrame.transform.SetParent(this.gameObject.transform);
        //----------------------------

        return newPos;
    }

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

    private float GetOffsetCell(float correctZomm)
    {
        float OffsetCell = 1f;

        if (correctZomm >= 1.1f)
            OffsetCell = OffsetCell11;
        if (correctZomm >= 1.2f)
            OffsetCell = OffsetCell12;
        if (correctZomm >= 1.3f)
            OffsetCell = OffsetCell13;

        if (correctZomm >= 1.4f)
            OffsetCell = OffsetCell14;
        if (correctZomm >= 1.5f)
            OffsetCell = OffsetCell15;
        if (correctZomm >= 1.6f)
            OffsetCell = OffsetCell16;
        if (correctZomm >= 1.7f)
            OffsetCell = OffsetCell17;
        if (correctZomm >= 1.8f)
            OffsetCell = OffsetCell18;
        if (correctZomm >= 1.9f)
            OffsetCell = OffsetCell19;
        if (correctZomm >= 2f)
            OffsetCell = OffsetCell2;
        if (correctZomm >= 2.1f)
            OffsetCell = OffsetCell21;
        if (correctZomm >= 2.2f)
            OffsetCell = OffsetCell22;
        if (correctZomm >= 2.3f)
            OffsetCell = OffsetCell23;
        if (correctZomm >= 2.4f)
            OffsetCell = OffsetCell24;
        if (correctZomm >= 2.5f)
            OffsetCell = OffsetCell25;

        return OffsetCell;
    }

    private void ValidateStartPosition()
    {
        double startMapX = System.Math.Round(Storage.Map.StartPositFrameMap.x, 1);
        double startMapY = System.Math.Round(Storage.Map.StartPositFrameMap.y, 1);
        if (startMapX != 21.8 &&
             startMapY != -6.7)
        {
            Debug.Log("############ Incorrect start position Map frame Offset");
        }
        double startCellX = System.Math.Round(posOld.x, 1);
        double startCellY = System.Math.Round(posOld.y, 1);
        if (startCellX != 9.4 &&
             startCellY != 5.7)
        {
            Debug.Log("############ Incorrect start position Map Cell Offset");
        }
    }

    #region Rest Hit point

    private void HitTextMousePointOnEbject()
    {
        //-----------------------
        //Camera cameraMap = Camera.main;
        Camera cameraMap = Storage.PlayerController.CameraMap;// Camera.main;
        //Camera cameraMap = Storage.PlayerController.MainCamera;// Camera.main;

        if (!cameraMap.enabled)
            return;

        Ray ray = cameraMap.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Casts the ray and get the first game object hit
        Physics.Raycast(ray, out hit);

        //-----------------
        //RaycastHit hit;
        ////Using physics
        ////var hit : RaycastHit;
        //if (Physics.Raycast(cameraMap.ScreenPointToRay(Input.mousePosition), hit))
        //{

        //}
        //we hit
        //-----------------
        //Raycast against a specific collider (plane is a gameObject or Transform)
        //if (plane.collider.Raycast(cameraMap.ScreenPointToRay(Input.mousePosition), hit))
        //if (this.GetComponent<Collider2D>().Raycast(cameraMap.ScreenPointToRay(Input.mousePosition), hit))
        //{
        //    //we hit
        //}

        //-----------------
        //Raycast against the plane itself (plane is a Plane)
        //var enter : float;
        //if (plane.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition)))
        //{

        //}

        //-----------------
        //var t = cameraMap.ScreenPointToRay(Input.mousePosition).GetPoint(enter);

        //-----------------

        //if (!cameraMap.isActiveAndEnabled)
        //    return;
        //if (!cameraMap.gameObject.activeSelf)
        //    return;



        //-----------------
        //Debug.Log("This hit at " + hit0.point);
        //if(hit0.point==new Vector3(0,0,0))
        //{
        //    Debug.Log(">>>>>>>>>>>>> YES CLICK");
        //}
        //-----------------
        //if (Input.touchCount > 0)
        //{
        //    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //    RaycastHit2D hit4 = Physics2D.Raycast(ray.origin, ray.direction);
        //    if (hit4 != null)
        //    {
        //        Debug.Log("MAP raycast hit this gameobject 444444 " + hit4.point.x + "x" + hit4.point.y);
        //        Storage.Events.ListLogAdd = "MAP raycast hit this gameobject 44 " + hit4.point.x + "x" + hit4.point.y;
        //    }
        //}
        //-----------------
        //-----------------


        //-----------------------

        //------------------
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = cameraMap.ScreenToWorldPoint(Input.mousePosition);

            if (Physics2D.OverlapPoint(mousePosition))
            {
                Debug.Log(">>>>>>>>>>>>>>>>>> do great stuff : mousePosition:" + mousePosition.x + "x" + mousePosition.y);
                //Storage.Events.ListLogAdd = "MAP mousePosition:" + mousePosition.x + "x" + mousePosition.y;
                //do great stuff
            }
        }


        //---------------------
        Ray ray2 = cameraMap.ScreenPointToRay(new Vector3(500, 500, 0));
        Debug.DrawRay(ray2.origin, ray2.direction * 10, Color.yellow);
        //---------------------
    }
    #endregion

}
