using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseInventoryData : MonoBehaviour {

    [NonSerialized]
    public DataObjectInventory DataObjectInv;
    public string NameInventopyObject
    {
        get
        {
            if(DataObjectInv == null)
                DataObjectInv = new DataObjectInventory("");
            //DataObjectInv = new DataObjectInventory(SaveLoadData.TypeInventoryObjects.PrefabField.ToString());
            return DataObjectInv.NameInventopyObject;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
