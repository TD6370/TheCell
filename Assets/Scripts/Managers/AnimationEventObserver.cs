using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventObserver : MonoBehaviour {

    PortalController controlPortal;
    private bool isPortal = false;

    private void Awake()
    {
        if(gameObject.transform.parent!=null)
        {
            controlPortal = gameObject.transform.parent.gameObject.GetComponent<PortalController>();
            if (controlPortal != null)
                isPortal = true;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EventAnimationAttack()
    {
    }

    public void EventIncubationCompleted()
    {
        if(isPortal)
        {
            controlPortal.IncubationCompleted();
        }
    }
}
