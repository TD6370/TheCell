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

    public DataObjectInventory(string nameObjectInventory, int m_count =0)
    {
        NameInventopyObject = nameObjectInventory;
        Count = m_count;
    }
}

public static class InventoryExtension
{
    public static DataObjectInventory GetInventoryObject(this ModelNPC.ObjectData objData, ModelNPC.GameDataAlien alien = null)
    {
        if (Enum.IsDefined(typeof(SaveLoadData.TypeInventoryObjects), objData.TypePoolPrefabName))
        {
            Debug.Log("######## TypePrefabs not exist NameInventopyObject = " + objData.TypePoolPrefabName);
            return new DataObjectInventory();
        }
        int countResource = 1;
        ModelNPC.TerraData terraRes = objData as ModelNPC.TerraData;
        if (terraRes != null)
        {
            countResource = terraRes.BlockResources;
            if (alien != null)
            {
                terraRes.BlockResources -= alien.WorkPower; // or  WorkPower - is time work
            }
        }
        SaveLoadData.TypeInventoryObjects invObject = (SaveLoadData.TypeInventoryObjects)Enum.Parse(typeof(SaveLoadData.TypeInventoryObjects), objData.TypePoolPrefabName);
        return new DataObjectInventory(invObject.ToString(), countResource);
    }
}