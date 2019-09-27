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
            if(String.IsNullOrEmpty(NameInventopyObject))
                return SaveLoadData.TypeInventoryObjects.None;

            if (!Enum.IsDefined(typeof(SaveLoadData.TypeInventoryObjects), NameInventopyObject))
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
    public DataObjectInventory(DataObjectInventory inventry)
    {
        NameInventopyObject = inventry.NameInventopyObject;
        Count = inventry.Count;
    }
    public static DataObjectInventory EmptyInventory()
    {
        return new DataObjectInventory(SaveLoadData.TypeInventoryObjects.None.ToString(), 0);
    }

    public void Clear()
    {
        NameInventopyObject = SaveLoadData.TypeInventoryObjects.None.ToString();
        Count = 0;
    }

    public override string ToString()
    {
        return string.Format("{0} ({1})", NameInventopyObject, Count);
    }
}

public static class InventoryExtension
{
    public static DataObjectInventory LootObjectToInventory(this ModelNPC.ObjectData targetData, ModelNPC.GameDataAlien alien = null)
    {
        if (!Enum.IsDefined(typeof(SaveLoadData.TypeInventoryObjects), targetData.TypePrefabName))
        {
            Debug.Log("######## TypePrefabs not exist NameInventopyObject = " + targetData.TypePrefabName);
            return new DataObjectInventory();
        }
        int countResource = 1;
        ModelNPC.TerraData terraRes = targetData as ModelNPC.TerraData;
        if (terraRes != null)
        {
            countResource = terraRes.BlockResources;
            if (alien != null)
            {
                terraRes.BlockResources -= alien.WorkPower; // or  WorkPower - is time work
            }
        }
        SaveLoadData.TypeInventoryObjects invObject = (SaveLoadData.TypeInventoryObjects)Enum.Parse(typeof(SaveLoadData.TypeInventoryObjects), targetData.TypePrefabName);
        return new DataObjectInventory(invObject.ToString(), countResource);
    }

    public static DataObjectInventory GetInventoryObject(this ModelNPC.ObjectData targetData)
    {
        if (!Enum.IsDefined(typeof(SaveLoadData.TypeInventoryObjects), targetData.TypePrefabName))
        {
            Debug.Log("######## TypePrefabs not exist NameInventopyObject = " + targetData.TypePrefabName);
            return new DataObjectInventory();
        }
        
        int countResource = 1;
        ModelNPC.TerraData terraRes = targetData as ModelNPC.TerraData;
        if (terraRes != null)
        {
            countResource = terraRes.BlockResources;
        }
        SaveLoadData.TypeInventoryObjects invObject = (SaveLoadData.TypeInventoryObjects)Enum.Parse(typeof(SaveLoadData.TypeInventoryObjects), targetData.TypePrefabName);
        return new DataObjectInventory(invObject.ToString(), countResource);
    }
}