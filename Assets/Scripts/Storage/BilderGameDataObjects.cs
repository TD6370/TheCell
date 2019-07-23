using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using ModelNPC;

public class BilderGameDataObjects : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static ModelNPC.ObjectData BildObjectData(string prefabTypeStr)
    {
        SaveLoadData.TypePrefabs prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), prefabTypeStr);
        return BildObjectData(prefabType, false);
    }

    public static ModelNPC.ObjectData BildObjectData(SaveLoadData.TypePrefabs prefabType, bool isTerraGen = false)
    {
        ModelNPC.ObjectData objGameBild;

        switch (prefabType)
        {
            case SaveLoadData.TypePrefabs.PrefabUfo:
                objGameBild = new ModelNPC.GameDataUfo();
                break;
            case SaveLoadData.TypePrefabs.PrefabBoss:
                objGameBild = new ModelNPC.GameDataBoss(); //$$
                break;
            case SaveLoadData.TypePrefabs.PrefabField:
                //objGameBild = new ModelNPC.TerraData(isTerraGen); //$$
                objGameBild = new ModelNPC.TerraData(); //$$
                break;
            case SaveLoadData.TypePrefabs.PrefabNPC:
                objGameBild = new ModelNPC.GameDataAlien();
                break;
            case SaveLoadData.TypePrefabs.PrefabFlore:
                objGameBild = new ModelNPC.ObjectData();
                break;
            default:
                objGameBild = new ModelNPC.ObjectData();
                break;
        }
        return objGameBild;
    }
}

public static class BilderExtension
{

    public static bool IsNPC(this ModelNPC.ObjectData model)
    {
        return model is ModelNPC.GameDataNPC;
    }
    public static bool IsBoss(this ModelNPC.ObjectData model)
    {
        return model is ModelNPC.GameDataBoss;
    }
    public static bool IsUFO(this ModelNPC.ObjectData model)
    {
        return model is ModelNPC.GameDataUfo;
    }
}
