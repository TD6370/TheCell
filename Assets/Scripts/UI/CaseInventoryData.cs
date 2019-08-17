using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseInventoryData : MonoBehaviour {

    [NonSerialized]
    //public SaveLoadData.TypePrefabs TypeInventopyObject;
    public string NameInventopyObject;

    public SaveLoadData.TypePrefabs TypeInventopyObject
    {
        get
        {
            if(Enum.IsDefined(typeof(SaveLoadData.TypePrefabs), NameInventopyObject))
            {
                Debug.Log("######## TypePrefabs not exist NameInventopyObject = " + NameInventopyObject);
                return SaveLoadData.TypePrefabs.PrefabField;
            }
            return (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), NameInventopyObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
