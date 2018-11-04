﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float SizeZoom = 1f;
    public Vector2 SelectPointField = new Vector2(0, 0);

    float upLevel = 0;
    float speedWheel = 0.02f;
    float limitZoomMax = 2.5f;
    float limitZoomMin = 0.8f;

    // Update is called once per frame
    void Update() {

        OnMauseWheel();
      
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownOnChange();
        }

    }

    private void MouseDownOnChange()
    {
        CalculatePointOnMap();
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

    private void CalculatePointOnMap()
    {
        Camera cameraMap = Storage.PlayerController.CameraMap;
        if (cameraMap == null)
        {
            Debug.Log("################ cameraMap is EMPTY ");
            return;
        }

        float mapX = 0;
        float mapY = 0;
        Ray ray1 = cameraMap.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit1 = Physics2D.GetRayIntersection(ray1, Mathf.Infinity);

        if (hit1.collider != null && hit1.collider.transform == this.gameObject.transform)
        {
            BoxCollider2D colliderMap = GetComponent<BoxCollider2D>();
            if (colliderMap != null)
            {
                
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

                    Debug.Log("_zoom===" + _zoom);
                    offsetCenter = OffsetZoomUp(_zoom);

                    Storage.Events.ListLogAdd = "Corrr zoom= " + (int)mapX + "x" + (int)mapY + "  offset= " + offsetCenter + " zoom: " + _zoom;

                    //mapX = (int)mapX;
                    //mapY = (int)mapY;

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

                    Storage.Events.ListLogAdd = "map = " + (int)mapX + "x" + (int)mapY + "  offsetCenter= " + offsetCenter;

                    mapX += offsetCenter;
                    mapY += offsetCenter;
                }

                Storage.Events.ListLogAdd = "MAP pos = " + mapX + "x" + mapY;
                //Debug.Log("MAP pos = " + mapX + "x" + mapY);
                Storage.Events.ListLogAdd = "Cell: " + (int)mapX + "x" + (int)mapY;
            }
        }

        SelectPointField = new Vector2(mapX, mapY);
        Storage.Map.SelectPointField = SelectPointField;
        ShowSelectorCell();
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

    private static float OffsetZoomDown(float _zoom)
    {
        float offsetCenter = 0;

        if (_zoom >= 0.8f)
            offsetCenter = -12f;
        //if (_zoom >= 0.85f)
        //    offsetCenter = -7f;
        //if (_zoom >= 0.95f)
        //    offsetCenter = -4f;
        if (_zoom >= 0.9f)
            offsetCenter = -5f;
        return offsetCenter;
    }

    private static float OffsetZoomUp(float _zoom)
    {
        float offsetCenter = 0;
        if (_zoom >= 1f)
            offsetCenter = 0;
        if (_zoom >= 1.1f)
            offsetCenter = 4f;// 5f;
        if (_zoom >= 1.2f)
            offsetCenter = 8f; // 10f;
        if (_zoom >= 1.3f)
            offsetCenter = 11f;// 12f;
        if (_zoom >= 1.4f)
            offsetCenter = 14f; //15f
        if (_zoom >= 1.5f)
            offsetCenter = 16f;
        if (_zoom >= 1.6f)
            offsetCenter = 18f;
        if (_zoom >= 1.7f)
            offsetCenter = 20f;
        if (_zoom >= 1.8f)
            offsetCenter = 22f;
        if (_zoom >= 1.9f)
            offsetCenter = 23f;
        //if ((int)_zoom >= 2f)
        //    offsetCenter = 25.5f;// 25f;
        if (_zoom >= 2f)
            offsetCenter = 25.5f;// 25f;
        if (_zoom >= 2.1f)
            offsetCenter = 26.5f;// 25f;
        if (_zoom >= 2.2f)
            offsetCenter = 28f;// 25f;
        if (_zoom >= 2.3f)
            offsetCenter = 28.5f;// 25f;
        if (_zoom >= 2.4f)
            offsetCenter = 29.5f;// 25f;
        if (_zoom >= 2.5f)
            offsetCenter = 30f;// 25f;
        return offsetCenter;
    }

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

    private void ShowSelectorCell()
    {
        //string nameField = Helper.GetNameField(x, y);
        string nameField = Storage.Map.SelectFieldMap;
        if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
        {
            //DrawTextureTo(scaleCell, indErr, addSize, texture, y, x, prefabType);
            //continue;
            return;
        }

        SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
        List<SaveLoadData.TypePrefabs> fieldListPrefbs = new List<SaveLoadData.TypePrefabs>();

        foreach (ModelNPC.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
        {
            prefabType = SaveLoadData.TypePrefabs.PrefabField;

            //if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
            //datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
            //{
                prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
            //}
            //else
            //{
            //    ModelNPC.GameDataBoss bossObj = datObjItem as ModelNPC.GameDataBoss;
            //    if (bossObj != null)
            //    {
            //        colorCell = TypeBoss.TypesBoss.Find(p => p.Level == bossObj.Level).ColorTrack;
            //        if (colorCell == null)
            //        {
            //            Debug.Log("############# colorCell Point Map Person is null");
            //            continue;
            //        }
            //        prefabType = SaveLoadData.TypePrefabs.PrefabBoss;
            //    }
            //}
            fieldListPrefbs.Add(prefabType);
        }

        int cellX = (int)SelectPointField.x;
        int cellY = (int)SelectPointField.y;

        foreach (var typeObj in fieldListPrefbs)
        {
           Storage.Map.DrawMapCell(cellX, cellY, typeObj);
            continue;
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
