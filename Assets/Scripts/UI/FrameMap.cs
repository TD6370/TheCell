using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    float SizeZoom = 1f;
    float upLevel = 0;
    float speedWheel = 0.02f;

    // Update is called once per frame
    void Update() {

        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel != 0) // back
        {
            //Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 1, 1);
            if (wheel > 0)
            {
                upLevel += speedWheel;
                SizeZoom += (0.06f + upLevel);
            }
            else
            {
                upLevel -= speedWheel;
                if (upLevel < 0)
                    upLevel = 0;
                SizeZoom -= (0.06f + upLevel);
            }
            Zooming(SizeZoom);
        }
        //if (Input.GetAxis("Mouse ScrollWheel") & gt; 0) // forward
        // {
        //    Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize - 1, 6);
        //}

        
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
        if (cameraMap==null)
        {
            Debug.Log("################ cameraMap is EMPTY ");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray1 = cameraMap.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit1 = Physics2D.GetRayIntersection(ray1, Mathf.Infinity);
            if (hit1.collider != null && hit1.collider.transform == this.gameObject.transform)
            {
                // raycast hit this gameobject
                Debug.Log(">>>>>>>>>>>>>>>>>> raycast hit this gameobject " + hit1.point.x + "x" + hit1.point.y);
                //Storage.Events.ListLogAdd = "MAP raycast hit this gameobject " + hit1.point.x + "x" + hit1.point.y;
                //Storage.Events.ListLogAdd = "MAP raycast hit this gameobject x1 = " + (hit1.point.x - transform.position.x);
                BoxCollider2D colliderMap = GetComponent<BoxCollider2D>();
                if (colliderMap != null)
                {
                    //Storage.Events.ListLogAdd = "MAP colliderMap.size = " + colliderMap.size;
                    float widthCellPexel = 25;
                    float widthMap = colliderMap.size.x;
                    float offSerX = (hit1.point.x - transform.position.x);
                    float radiusX = widthMap/2;
                    float mapX = offSerX + radiusX;

                    float heightMap = colliderMap.size.y;
                    float offSerY = (hit1.point.y - transform.position.y);
                    float radiusY = heightMap / 2;
                    float mapY = offSerY + radiusY;
                    mapY = widthCellPexel - mapY;

                    //mapX = mapX / widthCellPexel * Helper.WidthLevel;
                    //mapY = mapY / widthCellPexel * Helper.HeightLevel;

                    mapX = mapX / widthCellPexel * Helper.WidthLevel;
                    mapY = mapY / widthCellPexel * Helper.HeightLevel;

                    //Storage.Events.ListLogAdd = "MAP mapX = " + mapX + "    offSerX(" + offSerX + ") + radius(" +  radius + ")";
                    Storage.Events.ListLogAdd = "MAP pos = " + mapX + "x" + mapY + "    SizeZoom: " + SizeZoom;

                    if (SizeZoom > 1)
                    {

                        float koof =  SizeZoom - 1 ;

                        float proc = koof * 10;
                        Storage.Events.ListLogAdd = "proc=" + proc + " %  /k= " + koof;

                        //mapX -= mapX / 100 * proc;
                        //mapY -= mapY / 100 * proc;

                        mapY = mapY / 1.5f;
                        mapX = mapX / 1.5f;

                        //koof = (koof - 1) * 10;

                        //Storage.Events.ListLogAdd = "  /k= " + koof;

                        //mapX /= koof;
                        //mapY /= koof;
                    }
                    //else
                    //{
                    //    //float koof = 1 - SizeZoom;
                    //    mapX = (mapX * SizeZoom);
                    //    mapY = (mapY * SizeZoom);
                    //}
                    Storage.Events.ListLogAdd = "MAP pos = " + mapX + "x" + mapY;


                    Debug.Log("MAP pos = " + mapX + "x" + mapY);
                    Storage.Events.ListLogAdd = "Cell: " + (int)mapX + "x" + (int)mapY;
                    //Debug.Log("MAP mapX = " + mapX + "    offSerX(" + offSerX + ") + radius(" + radius + ")");
                }
                

                //Storage.Events.ListLogAdd = "MAP raycast hit this gameobject x1 = " + (hit1.point.x - transform.position.x);
                //var pointX = transform.InverseTransformPoint(hit.point).x;
                //Debug.Log(">>>>>>>>>>>>>>>>>> pointX " + pointX);
                //Debug.Log(">>>>>>>>>>>>>>>>>> Dist " + (transform.position.x - hit.point.x));
            }
        }

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

    void OnMouseDown()
    {
        //Print
        Storage.Events.ListLogAdd = "MAP OnMouseDown";
    }

    private void Zooming(float zoom = 1f)
    {
        //this.gameObject.transform.localScale = new Vector3(zoom, zoom, 0);
        this.gameObject.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(zoom, zoom, 0);
    }
        
}
