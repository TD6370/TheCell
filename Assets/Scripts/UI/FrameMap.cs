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
	void Update () {

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
    }

    private void Zooming(float zoom = 1f)
    {
        //prefabFrameMap.transform.localScale = new Vector3(10F, 10f, 0);
        this.gameObject.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(zoom, zoom, 0);
    }
        
}
