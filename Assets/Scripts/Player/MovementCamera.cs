using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCamera : MonoBehaviour {

    private float temp_size = 0;
    [Range(-200,200)]
    public float SizeOnDebug = -150f;

    // Use this for initialization
    void Start () {
		//StartGen();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Camera temp_cam;

    public void MoveOnDebugSceneInfo()
    {
        if (!Storage.Instance.MainCamera.enabled)
            return;

        if(temp_size == 0)
            temp_size = Storage.Instance.MainCamera.orthographicSize;

        Storage.Instance.MainCamera.orthographicSize = SizeOnDebug;

        //temp_cam = Storage.Instance.MainCamera;
    }

    public void ResetPosition()
    {
        if (temp_size != 0)
            Storage.Instance.MainCamera.orthographicSize = temp_size;
        temp_size = 0;
    }

}
