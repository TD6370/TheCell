using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlType("DataObjectInventory")]
public class DataObjectInventory {

    public string NameInventopyObject;
    public int Count;

    [XmlIgnore]
    public SaveLoadData.TypeInventoryObjects TypeInventoryObject
    {
        get
        {
            if (Enum.IsDefined(typeof(SaveLoadData.TypeInventoryObjects), NameInventopyObject))
            {
                Debug.Log("######## TypePrefabs not exist NameInventopyObject = " + NameInventopyObject);
                return SaveLoadData.TypeInventoryObjects.None;
            }
            return (SaveLoadData.TypeInventoryObjects)Enum.Parse(typeof(SaveLoadData.TypeInventoryObjects), NameInventopyObject);
        }
    }

    public DataObjectInventory() { }

    public DataObjectInventory(string nameObjectInventory)
    {
        NameInventopyObject = nameObjectInventory;
    }
}
