using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FrameMap : MonoBehaviour, IPointerClickHandler
{

    public GameObject MapCellFrame;
    public GameObject MapCellBorder;

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
    private float upLevel = 0;
    private float speedWheel = 0.02f;
    private float limitZoomMax = 2.5f;
    private float limitZoomMin = 0.8f;

    //9.4 5.7
    private Vector3 posOld = new Vector3();

    //public GraphicRaycaster m_Raycaster;
    //private PointerEventData m_PointerEventData;
    //public EventSystem m_EventSystem;

    Camera cameraMap;// = Storage.PlayerController.CameraMap;
    BoxCollider2D colliderMap;

    // Use this for initialization
    void Start()
    {
        cameraMap = Storage.PlayerController.CameraMap;
        colliderMap = GetComponent<BoxCollider2D>();

        //-------------- Raycast
        ////Fetch the Raycaster from the GameObject (the Canvas)
        //m_Raycaster = GetComponent<GraphicRaycaster>();
        //if (m_Raycaster == null)
        //{
        //    Debug.Log("###### Raycaster == null");
        //}
        ////Fetch the Event System from the Scene
        //m_EventSystem = GetComponent<EventSystem>();
        //if (m_EventSystem == null)
        //{
        //    Debug.Log("###### EventSystem == null");
        //}
    }

    // Update is called once per frame
    void Update() {

        if (!IsActive)
        {
            return;
        }

        
    }

    private void FixedUpdate()
    {
        if (Time.time < Storage.PaletteMap.DelayTimerPaletteUse)
        {
            //Debug.Log("!!!!!!!!! MAP CAST STOP !!!!!! ");
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
        //return;//#FIX
        if (IsActive && IsRuntimeViewMarker)
        {
            CalculatePointOnMap();
            UpdateBorderCellLocation();
            Storage.Map.UpdateMarkerPointCell();
            //Storage.Map.ShowBorderBrush();
        }
        //EventsUI();

    }

    public void OnPointerClick(PointerEventData data)
    {
        // This will only execute if the objects collider was the first hit by the click's raycast
        Debug.Log("---------------------------OnPointerClick   WP:" + data.worldPosition + "  POS:" + data.position + "  PP:" + data.pointerPress.name);
        MouseDownOnChange();
    }

    void OnMouseDown()
    {
        //Print
        Storage.Events.ListLogAdd = "MAP OnMouseDown";
    }

    //private void EventsUI()
    //{
    //    //Check if the left Mouse button is clicked
    //    //if (Input.GetKey(KeyCode.Mouse0))
    //    //{
    //        //Set up the new Pointer Event
      
    //    if (m_Raycaster == null || m_EventSystem == null)
    //    {
    //        return;
    //    }

    //    m_PointerEventData = new PointerEventData(m_EventSystem);
    //    if (m_PointerEventData == null)
    //    {
    //        Debug.Log("####### EventsU MAP ### PointerEventData==null");
    //        return;
    //    }

    //    //Set the Pointer Event Position to that of the mouse position
    //    m_PointerEventData.position = Input.mousePosition;

    //    //cameraMap = Storage.PlayerController.CameraMap;
    //    //m_PointerEventData.position = cameraMap.ScreenToWorldPoint(Input.mousePosition);

    //    //Create a list of Raycast Results
    //    List<RaycastResult> results = new List<RaycastResult>();

    //        //Raycast using the Graphics Raycaster and mouse click position
    //        m_Raycaster.Raycast(m_PointerEventData, results);

    //        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
    //        foreach (RaycastResult result in results)
    //        {
    //            Debug.Log(">>>>>>>>>> Hit MAP ### " + result.gameObject.name);
    //        }
    //    //}

    //}

    private void MouseDownOnChange()
    {
        CalculatePointOnMap();

        ShowSelectorCell();
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

    private void MouseDownSpecOnChange()
    {
        RunTeleportHero();
    }

    private void RunTeleportHero()
    {
        if (Storage.Events.IsCommandTeleport || Storage.PaletteMap.ModePaint == ToolBarPaletteMapAction.Teleport)
        {
            Storage.Map.Create();
            int posTransferHeroX = (int)(Storage.Map.SelectPointField.x * Storage.ScaleWorld);
            int posTransferHeroY = (int)(Storage.Map.SelectPointField.y * Storage.ScaleWorld);
            posTransferHeroY *= -1;
            Storage.Player.TeleportHero(posTransferHeroX, posTransferHeroY);
        }
    }

   

    public void Show(bool isShow = true)
    {
        this.gameObject.SetActive(isShow);
        IsRuntimeViewMarker = isShow;
    }

    public void Restart()
    {
        Storage.Map.StartPositFrameMap = new Vector3();
        Vector2 posHero = Storage.PlayerController.transform.position;
        transform.position = new Vector3(posHero.x, posHero.y, 0);
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


    /*
    private void CalculatePointOnMap_()
    {
        //bool isLog = false;
        bool isLog = true;

        Camera cameraMap = Storage.PlayerController.CameraMap;
        if (cameraMap == null)
        {
            Debug.Log("################ cameraMap is EMPTY ");
            return;
        }
        if (!cameraMap.enabled)
            return;

        //cameraMap
        Vector2 posClick = new Vector2();
        bool isMousePos = false;

        //HitTextMousePointOnEbject();

        float mapX = 0;
        float mapY = 0;

        


        Vector2 mousePosition = cameraMap.ScreenToWorldPoint(Input.mousePosition);
        if (Physics2D.OverlapPoint(mousePosition))
        {
            //Storage.Events.ListLogAdd = "3. Physics2D.OverlapPoint(TestHasPoint) : " + TestHasPoint.x + "x" + TestHasPoint.y;
            posClick = mousePosition;
            isMousePos = true;
        }
       
        if (isMousePos)
        {
            BoxCollider2D colliderMap = GetComponent<BoxCollider2D>();
            if (colliderMap != null)
            {
                if (isLog)
                    Storage.Events.ListLogAdd = "Original mouse=" + mousePosition.x + "x" + mousePosition.y;
                NormalizedMapPoint(posClick, colliderMap, out mapX, out mapY);
                //++++++++++++
                //posClick.y *= (-1);
                //var mouseXandYNormalized1 = Rect.PointToNormalized(new Rect(0,0, colliderMap.size.x , colliderMap.size.y ), posClick);
                //mapX = mouseXandYNormalized1.x * 100;
                //mapY = mouseXandYNormalized1.y * 100;
                //mapX += transform.position.x;
                //mapY += transform.position.y;

                //var mouseXandYNormalized2 = Rect.NormalizedToPoint(new Rect(0, 0, colliderMap.size.x, colliderMap.size.y), posClick);
                //mapX = mouseXandYNormalized2.x;
                //mapY = mouseXandYNormalized2.y;
                //++++++++++++

                if (isLog)
                {
                    Storage.Events.ListLogAdd = "MAP ORIGINAL: pos = " + mapX + "x" + mapY + "  Zoom: " + SizeZoom;
                    Storage.Events.ListLogAdd = "mouse=" + posClick.x + "x" + posClick.y;
                    
                }
                //Storage.Events.ListLogAdd = "MAP ORIGINAL: pos = " + mapX + "x" + mapY + "  Zoom: " + SizeZoom;

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

                    //<<#
                    //_zoom = (float)System.Math.Round(_zoom, 1);

                    float offsetCenter = 0f;

                    //Debug.Log("_zoom===" + _zoom);

                    //<<#
                    //offsetCenter = OffsetZoomUp(_zoom);

                    //+++++++++++++++
                    Vector2 mouseXandY = new Vector2(mapX, mapY);
                    
                    RectTransform rt = (RectTransform)this.gameObject.transform;
                    //Rect screenRect = new Rect(0f, 0f, this.gameObject.GetComponent<SpriteRenderer>().sprite.rect.width, this.gameObject.GetComponent<SpriteRenderer>().sprite.rect.height);
                    Rect screenRect = new Rect(0f, 0f, colliderMap.size.x, colliderMap.size.y);
                    Rect screenRect3 = new Rect(0f, 0f, rt.rect.x, rt.rect.y);

                    //realHeight = xptoExample.sprite.rect.height;

                    //Rect.PointToNormalized
                    //Rect.NormalizedToPoint
                    var mouseXandYNormalized = Rect.PointToNormalized(screenRect,  mouseXandY);
                    mapX = mouseXandYNormalized.x;
                    mapY = mouseXandYNormalized.y;
                    //+++++++++++++++++++++++

                    if (isLog)
                        Storage.Events.ListLogAdd = "Corrr zoom: " + (int)mapX + "x" + (int)mapY + "  offset= " + offsetCenter + " zoom: " + _zoom;

                    //mapX = (int)mapX;
                    //mapY = (int)mapY;

                    //!!! CORR DISTANCE
                    //int centrW = Helper.HeightLevel / 2;
                    //Vector3 centerPos = new Vector3(centrW, centrW, 0);
                    //float koofOnCenterX = centerPos.x / mapX;
                    //float koofOnCenterY = centerPos.y / mapY;
                    //if (isLog)
                    //    Storage.Events.ListLogAdd = "Map koofOnCenter: " + koofOnCenterX + " x " + koofOnCenterY;
                    ////------------------

                    //mapX += offsetCenter;
                    //mapY += offsetCenter;
                }
                else
                {
                    float _zoom = SizeZoom;

                    //mapX = (int)mapX;
                    //mapY = (int)mapY;

                    mapY = (int)(mapY / _zoom);
                    mapX = (int)(mapX / _zoom);

                    float offsetCenter = OffsetZoomDown(_zoom);

                    if (isLog)
                        Storage.Events.ListLogAdd = "Corrr zoom:  " + (int)mapX + "x" + (int)mapY + "  offsetCenter= " + offsetCenter;

                    mapX += offsetCenter;
                    mapY += offsetCenter;
                }

                if (isLog)
                    Storage.Events.ListLogAdd = "MAP pos = " + mapX + "x" + mapY;
            }
        }

        SelectPointField = new Vector2(mapX, mapY);
        SelectFieldPos = new Vector2((int)mapX, (int)mapY);

        Storage.Map.SelectPointField = SelectPointField;

        Storage.Map.UpdateMarkerPointCell();
    }
    */

    private void CalculatePointOnMap()
    {
        bool isLog = false;
        //Camera cameraMap = Storage.PlayerController.CameraMap;
        if (cameraMap == null)
        {
            Debug.Log("################ cameraMap is EMPTY ");
            return;
        }
        if (!cameraMap.enabled)
            return;

      

        //cameraMap
        Vector2 posClick = new Vector2();
        bool isMousePos = false;

        //HitTextMousePointOnEbject();

        float mapX = 0;
        float mapY = 0;

        //Rect.PointToNormalized
        //Rect.NormalizedToPoint

        //int LayerUI = LayerMask.NameToLayer("LayerUI");
        //int LayerViewUI = LayerMask.NameToLayer("UI");
        //int LayerObjects = LayerMask.NameToLayer("LayerObjects");

        //------------
        //Ray ray1 = cameraMap.ScreenPointToRay(Input.mousePosition);
        ////RaycastHit2D hit1 = Physics2D.GetRayIntersection(ray1, 15f, LayerViewUI);
        //RaycastHit2D hit1 = Physics2D.GetRayIntersection(ray1, 15f);
        ////if (hit1.collider != null && hit1.collider.transform == this.gameObject.transform)
        //if (hit1.collider != null)
        //{
        //    //Debug.Log("-------------GetRayIntersection On GOBJ: " + hit1.collider.gameObject.name);
        //}
        //----------------

        Vector2 mousePosition = cameraMap.ScreenToWorldPoint(Input.mousePosition);
        //if (Physics2D.OverlapPoint(mousePosition))

        //Collider2D clickedCollider = Physics2D.OverlapPoint(mousePosition, LayerUI);
        Collider2D clickedCollider = Physics2D.OverlapPoint(mousePosition);

        //if (Physics2D.OverlapPoint(mousePosition, LayerViewUI))
        if (clickedCollider)
        {
            //Storage.Events.ListLogAdd = "3. Physics2D.OverlapPoint(TestHasPoint) : " + TestHasPoint.x + "x" + TestHasPoint.y;
            posClick = mousePosition;
            isMousePos = true;
        }

        if (isMousePos)
        {
            //BoxCollider2D colliderMap = GetComponent<BoxCollider2D>();
            //if (colliderMap != null)
            //if(clickedCollider.Equals(colliderMap))
            if(clickedCollider.gameObject.name.Equals(colliderMap.gameObject.name))
            {
                //Debug.Log("-------------Point On GOBJ: " + clickedCollider.gameObject.name);

                NormalizedMapPoint(posClick, colliderMap, out mapX, out mapY);

                if (isLog)
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

                    if (isLog)
                        Storage.Events.ListLogAdd = "Corrr zoom: " + (int)mapX + "x" + (int)mapY + "  offset= " + offsetCenter + " zoom: " + _zoom;

                    //mapX = (int)mapX;
                    //mapY = (int)mapY;

                    //!!! CORR DISTANCE
                    int centrW = Helper.HeightLevel / 2;
                    Vector3 centerPos = new Vector3(centrW, centrW, 0);
                    float koofOnCenterX = centerPos.x / mapX;
                    float koofOnCenterY = centerPos.y / mapY;
                    if (isLog)
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

                    if (isLog)
                        Storage.Events.ListLogAdd = "Corrr zoom:  " + (int)mapX + "x" + (int)mapY + "  offsetCenter= " + offsetCenter;

                    mapX += offsetCenter;
                    mapY += offsetCenter;
                }

                if (isLog)
                    Storage.Events.ListLogAdd = "MAP pos = " + mapX + "x" + mapY;
            }
        }

        SelectPointField = new Vector2(mapX, mapY);
        SelectFieldPos = new Vector2((int)mapX, (int)mapY);

        Storage.Map.SelectPointField = SelectPointField;

        //Storage.Map.UpdateMarkerPointCell();
        //Storage.Map.ShowBorderBrush();

       
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

    private void NormalizedMapPoint(Vector2 hit1, BoxCollider2D colliderMap, out float mapX, out float mapY)
    {
        float widthCellPexel = 25;
        float widthMap = colliderMap.size.x;
        float offSerX = (hit1.x - transform.position.x);
        float radiusX = widthMap / 2;
        mapX = offSerX + radiusX;
        float heightMap = colliderMap.size.y;
        float offSerY = (hit1.y - transform.position.y);
        float radiusY = heightMap / 2;
        mapY = offSerY + radiusY;
        mapY = widthCellPexel - mapY;

        mapX = mapX / widthCellPexel * Helper.WidthLevel;
        mapY = mapY / widthCellPexel * Helper.HeightLevel;
    }



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

    
    private void Zooming(float zoom = 1f)
    {
        Storage.Map.ZoomMap = SizeZoom;

        //this.gameObject.transform.localScale = new Vector3(zoom, zoom, 0);
        this.gameObject.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(zoom, zoom, 0);
    }

    private void SelectCellAction(string nameField)
    {
        Storage.Instance.SelectFieldCursor = nameField;
        Storage.Events.CursorClickAction(nameField);

        //Storage.Person.VeiwCursorGameObjectData(nameField);
        //if (Storage.PaletteMap.IsPaintsOn)
        //{
        //    Storage.Instance.SelectFieldCursor = nameField;
        //    Storage.PaletteMap.PaintAction();
        //}
    }

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
            //Debug.Log("Selector Cell Field " + nameField + " is Empty     " + DateTime.Now);
            //Storage.Events.ListLogAdd = "Selector Cell Field " + nameField + " is Empty [" + DateTime.Now.ToShortTimeString() + "]";
            return;
        }

        //Celect Cell on World
        SelectCellAction(nameField);

        //Draw Icon on Cell Map
        SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
        List<SaveLoadData.TypePrefabs> fieldListPrefbs = new List<SaveLoadData.TypePrefabs>();
        List<Texture2D> listPersonsMapTexture = new List<Texture2D>();

        foreach (ModelNPC.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
        {
            //Debug.Log("Selector Cell : " + datObjItem.NameObject + "  " + DateTime.Now);

            prefabType = SaveLoadData.TypePrefabs.PrefabField;

            Storage.Events.ListLogAdd = "Find: " + datObjItem.NameObject;


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

    public void UpdateBorderCellLocation()
    {
        if(MapCellBorder==null)
        {
            Debug.Log("########### UpdateBorderCellLocation : MapCellBorder is Empty");
            return;
        }
        MapCellBorder.GetComponent<RectTransform>().position = SetLocationCell(); 
    }

    private Vector3 SetLocationCell()
    {
        bool isLog = false;

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
            //Debug.Log("Save normal posit");
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

            if(isLog)
                Storage.Events.ListLogAdd = "New pos Cell: " + newPos.x + "x" + newPos.y;
            if (isLog)
                Storage.Events.ListLogAdd = "-- Cell koof: " + koofOnCenterX + "x" + koofOnCenterY;

            float korrCellX = offSetOnCenterX / 100;
            float korrCellY = offSetOnCenterY / 100;

            float OffsetCell = 1f;

            OffsetCell = GetOffsetCell(correctZomm);

            if (isLog)
                Storage.Events.ListLogAdd = ":: OffsetCell corr: % " + OffsetCell;

            korrCellX *= OffsetCell;
            korrCellY *= OffsetCell;

            newPos.x -= korrCellX;
            newPos.y += korrCellY;

            if (isLog)
                Storage.Events.ListLogAdd = ":: Cell corr: " + korrCellX + " x " + korrCellY;
        }


        if (isLog)
            Storage.Events.ListLogAdd = "***** Cell pos: " + newPos.x + "x" + newPos.y;
        //<<< @
        //MapCellFrame.transform.SetParent(this.gameObject.transform);
        //----------------------------

        return newPos;
    }

   
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
            Storage.Map.StartPositFrameMap = new Vector3(21.8f, -6.7f, Storage.Map.StartPositFrameMap.z);
            Debug.Log("############ Incorrect start position Map frame Offset");
        }
        double startCellX = System.Math.Round(posOld.x, 1);
        double startCellY = System.Math.Round(posOld.y, 1);
        if (startCellX != 9.4 &&
             startCellY != 5.7)
        {
            posOld = new Vector3(9.4f, 5.7f, posOld.z);
            Debug.Log("############ Incorrect start position Map Cell Offset");
        }
    }

    #region Rest Hit point
    Vector2 TestHasPoint = new Vector2();

    private void HitTextMousePointOnEbject()
    {
        //-----------------------
        //Collider2D[] hitColliders = new Collider2D[1];
        //hitCollidersP
        //if (Physics2D.OverlapPointNonAlloc(mousePosition, hitColliders))
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

                TestHasPoint = mousePosition;
                //Debug.Log(">>>>>>>>>>>>>>>>>> do great stuff : mousePosition:" + mousePosition.x + "x" + mousePosition.y);
                //Storage.Events.ListLogAdd = "Physics2D.OverlapPoint(mousePosition) : " + mousePosition.x + "x" + mousePosition.y;
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
