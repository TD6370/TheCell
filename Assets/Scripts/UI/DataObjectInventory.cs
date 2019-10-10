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
    public int OrderIndex;

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

    [XmlIgnore]
    public SaveLoadData.TypePrefabs TypePrefabObject
    {
        get
        {
            if (!Enum.IsDefined(typeof(SaveLoadData.TypePrefabs), NameInventopyObject))
            {
                Debug.Log("######## TypePrefabObject not exist NameInventopyObject = " + NameInventopyObject);
                return SaveLoadData.TypePrefabs.PrefabField;
            }
            return (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), NameInventopyObject);
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

    public bool IsEmpty
    {
        get
        {
            if((NameInventopyObject != SaveLoadData.TypeInventoryObjects.None.ToString() && Count == 0) ||
                (NameInventopyObject == SaveLoadData.TypeInventoryObjects.None.ToString() && Count > 0))
                Clear();
            return Count == 0 || string.IsNullOrEmpty(NameInventopyObject) || NameInventopyObject == SaveLoadData.TypeInventoryObjects.None.ToString();
        }
    }

    public override string ToString()
    {
        return string.Format("{0} ({1})", NameInventopyObject, Count);
    }

    public bool EqualsInv(SaveLoadData.TypePrefabs typeRes)
    {
        return NameInventopyObject  == typeRes.ToString();
        //return base.Equals(obj);
    }
}

public static class InventoryExtension
{
    private static Dictionary<SaveLoadData.TypePrefabs, bool> m_emptyTrgetObjects = new Dictionary<SaveLoadData.TypePrefabs, bool>
    {
        {SaveLoadData.TypePrefabs.Ground, true},
        {SaveLoadData.TypePrefabs.Ground02, true},
        {SaveLoadData.TypePrefabs.Ground03, true}
    };

    public static DataObjectInventory LootObjectToInventory(this ModelNPC.ObjectData targetData, ModelNPC.GameDataAlien alien = null)
    {
        //if(targetData.TypePrefab == SaveLoadData.TypePrefabs.Ground) //Filter: Loot
        if (m_emptyTrgetObjects.ContainsKey(targetData.TypePrefab)) //Filter: Loot
            return DataObjectInventory.EmptyInventory();

        if (!Enum.IsDefined(typeof(SaveLoadData.TypeInventoryObjects), targetData.TypePrefabName))
        {
            Debug.Log("######## TypePrefabs not exist NameInventopyObject = " + targetData.TypePrefabName);
            return DataObjectInventory.EmptyInventory();
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

    public static void AddToInventory(this ModelNPC.GameDataAlien alien, ModelNPC.PortalData portal, int indexRes, int count)
    {
        DataObjectInventory resourceStorage = portal.Resources[indexRes];
        portal.Resources[indexRes].Count -= count;
        if (portal.Resources[indexRes].Count <= 0)
            portal.Resources.RemoveAt(indexRes);
        alien.Inventory = new DataObjectInventory(resourceStorage.NameInventopyObject, count);
    }

    public static void AddToInventory(this ModelNPC.GameDataAlien alien, ModelNPC.PortalData portal, DataObjectInventory res, int count)
    {
        int indexRes = portal.Resources.FindIndex(p => p == res);
        if(indexRes == -1)
        {
            Debug.Log("### AddToInventory not fount res: " + res);
            return;
        }
        DataObjectInventory resourceStorage = portal.Resources[indexRes];
        portal.Resources[indexRes].Count -= count;
        if (portal.Resources[indexRes].Count <= 0)
            portal.Resources.RemoveAt(indexRes);
        alien.Inventory = new DataObjectInventory(resourceStorage.NameInventopyObject, count);
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