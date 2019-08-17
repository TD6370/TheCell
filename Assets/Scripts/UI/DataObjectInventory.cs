using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObjectInventory {

    public string NameInventopyObject;

    public SaveLoadData.TypeInventoryObjects TypeInventopyObject
    {
        get
        {
            if (Enum.IsDefined(typeof(SaveLoadData.TypeInventoryObjects), NameInventopyObject))
            {
                Debug.Log("######## TypePrefabs not exist NameInventopyObject = " + NameInventopyObject);
                return SaveLoadData.TypeInventoryObjects.PrefabField;
            }
            return (SaveLoadData.TypeInventoryObjects)Enum.Parse(typeof(SaveLoadData.TypeInventoryObjects), NameInventopyObject);
        }
    }

    public DataObjectInventory(string nameObjectInventory)
    {
        NameInventopyObject = nameObjectInventory;
    }
}
