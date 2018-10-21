using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        //Debug.Log("&&&& EventsObject OnMouseDown");
        SelectIdFromTextBox();
    }

    public void Kill()
    {
        Storage.Instance.AddDestroyGameObject(this.gameObject);
    }

    private void SelectIdFromTextBox()
    {
        var gobj = this.gameObject;
        if (gobj.tag.ToString().IsPerson())
        {
            string objID = Helper.GetID(this.gameObject.name);
            Storage.Instance.SelectGameObjectID = objID;
            //Debug.Log("&&&& EventsObject Select " + objID + "   " + this.gameObject.name);
            Storage.Events.SetTestText(objID);
        }
        else
        {
            //Debug.Log("&&&& EventsObject Select " + gobj.tag.ToString() + "     " + this.gameObject.name);
            Storage.Events.SetTestText(gobj.tag.ToString());
        }
    }
}
