using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using ModelNPC;

public class BilderGameDataObjects //: MonoBehaviour 
{ 
	// Use this for initialization
	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

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
            case SaveLoadData.TypePrefabs.PrefabRock:
                objGameBild = new ModelNPC.Rock();
                break;
            case SaveLoadData.TypePrefabs.PrefabVood:
                objGameBild = new ModelNPC.Vood();
                break;
            case SaveLoadData.TypePrefabs.PrefabElka:
                objGameBild = new ModelNPC.Elka();
                break;
            case SaveLoadData.TypePrefabs.PrefabWallRock:
                objGameBild = new ModelNPC.WallRock();
                break;
            case SaveLoadData.TypePrefabs.PrefabWallWood:
                objGameBild = new ModelNPC.WallWood();
                break;
            case SaveLoadData.TypePrefabs.PrefabUfo:
                objGameBild = new ModelNPC.GameDataUfo();
                break;
            case SaveLoadData.TypePrefabs.PrefabBoss:
                objGameBild = new ModelNPC.GameDataBoss();
                break;
            case SaveLoadData.TypePrefabs.PrefabField:
                objGameBild = new ModelNPC.TerraData();
                break;
            case SaveLoadData.TypePrefabs.PrefabFloor:
                objGameBild = new ModelNPC.TerraData();
                break;
            case SaveLoadData.TypePrefabs.PrefabPerson:
                objGameBild = new ModelNPC.GameDataAlien();
                break;
            case SaveLoadData.TypePrefabs.PrefabFlore:
                objGameBild = new ModelNPC.ObjectData();
                break;
            case SaveLoadData.TypePrefabs.Inspector:
                objGameBild = new ModelNPC.GameDataAlienInspector();
                break;
            case SaveLoadData.TypePrefabs.Machinetool:
                objGameBild = new ModelNPC.GameDataAlienMachinetool();
                break;
            case  SaveLoadData.TypePrefabs.Mecha:
                objGameBild = new ModelNPC.GameDataAlienMecha();
                break;
            case  SaveLoadData.TypePrefabs.Dendroid:
                objGameBild = new ModelNPC.GameDataAlienDendroid();
                break;
            case SaveLoadData.TypePrefabs.Gary:
                objGameBild = new ModelNPC.GameDataAlienGarry();
                break;
            case SaveLoadData.TypePrefabs.Lollipop:
                objGameBild = new ModelNPC.GameDataAlienLollipop();
                break;
            case SaveLoadData.TypePrefabs.Blastarr:
                objGameBild = new ModelNPC.GameDataAlienBlastarr();
                break;
            case SaveLoadData.TypePrefabs.Hydragon:
                objGameBild = new ModelNPC.GameDataAlienHydragon();
                break;
            case SaveLoadData.TypePrefabs.Pavuk:
                objGameBild = new ModelNPC.GameDataAlienPavuk();
                break;
            case SaveLoadData.TypePrefabs.Skvid:
                objGameBild = new ModelNPC.GameDataAlienSkvid();
                break;
            case SaveLoadData.TypePrefabs.Fantom:
                objGameBild = new ModelNPC.GameDataAlienFantom();
                break;
            case SaveLoadData.TypePrefabs.Mask:
                objGameBild = new ModelNPC.GameDataAlienMask();
                break;
            case SaveLoadData.TypePrefabs.Vhailor:
                objGameBild = new ModelNPC.GameDataAlienVhailor();
                break;

            case SaveLoadData.TypePrefabs.Swamp:
                objGameBild = new ModelNPC.Swamp();
                break;
            case SaveLoadData.TypePrefabs.Chip:
                objGameBild = new ModelNPC.Chip();
                break;
            case SaveLoadData.TypePrefabs.Gecsagon:
                objGameBild = new ModelNPC.Gecsagon();
                break;
            case SaveLoadData.TypePrefabs.Kamish:
                objGameBild = new ModelNPC.Kamish();
                break;
            case SaveLoadData.TypePrefabs.Kishka:
                objGameBild = new ModelNPC.Kishka();
                break;
            case SaveLoadData.TypePrefabs.Nerv:
                objGameBild = new ModelNPC.Nerv();
                break;
            case SaveLoadData.TypePrefabs.Orbits:
                objGameBild = new ModelNPC.Orbits();
                break;
            case SaveLoadData.TypePrefabs.Shampinion:
                objGameBild = new ModelNPC.Shampinion();
                break;
            case SaveLoadData.TypePrefabs.Berry:
                objGameBild = new ModelNPC.Berry();
                break;
            case SaveLoadData.TypePrefabs.Mashrooms:
                objGameBild = new ModelNPC.Mashrooms();
                break;
            case SaveLoadData.TypePrefabs.Weed:
                objGameBild = new ModelNPC.Weed();
                break;
            case SaveLoadData.TypePrefabs.Weedflower:
                objGameBild = new ModelNPC.Weedflower();
                break;
            case SaveLoadData.TypePrefabs.Corals:
                objGameBild = new ModelNPC.Corals();
                break;
            case SaveLoadData.TypePrefabs.Desert:
                objGameBild = new ModelNPC.Desert();
                break;
            case SaveLoadData.TypePrefabs.Diods:
                objGameBild = new ModelNPC.Diods();
                break;
            case SaveLoadData.TypePrefabs.Parket:
                objGameBild = new ModelNPC.Parket();
                break;
            //--------- Wall
            case SaveLoadData.TypePrefabs.Kolba:
                objGameBild = new ModelNPC.Kolba();
                break;
            case SaveLoadData.TypePrefabs.Lantern:
                objGameBild = new ModelNPC.Lantern();
                break;
            case SaveLoadData.TypePrefabs.Bananas:
                objGameBild = new ModelNPC.Bananas();
                break;
            case SaveLoadData.TypePrefabs.Cluben:
                objGameBild = new ModelNPC.Cluben();
                break;
            case SaveLoadData.TypePrefabs.Chpok:
                objGameBild = new ModelNPC.Chpok();
                break;
            case SaveLoadData.TypePrefabs.Pandora:
                objGameBild = new ModelNPC.Pandora();
                break;
            case SaveLoadData.TypePrefabs.Nadmozg:
                objGameBild = new ModelNPC.Nadmozg();
                break;
            case SaveLoadData.TypePrefabs.Triffid:
                objGameBild = new ModelNPC.Triffid();
                break;
            case SaveLoadData.TypePrefabs.Aracul:
                objGameBild = new ModelNPC.Aracul();
                break;
            case SaveLoadData.TypePrefabs.Cloudwood:
                objGameBild = new ModelNPC.Cloudwood();
                break;

            default:
                objGameBild = new ModelNPC.ObjectData();
                break;
        }

        //objGameBild.Id = Guid.NewGuid().ToString();
        return objGameBild;
    }


}

public static class BilderExtension
{

    public static bool IsNPC(this ModelNPC.ObjectData model)
    {
        return model is ModelNPC.GameDataNPC;
    }
    public static bool IsFloor(this ModelNPC.ObjectData model)
    {
        return model is ModelNPC.FloorData;
    }
    public static bool IsFlore(this ModelNPC.ObjectData model)
    {
        return model is ModelNPC.FloreData;
    }
    public static bool IsWall(this ModelNPC.ObjectData model)
    {
        return model is ModelNPC.WallData;
    }
    public static bool IsWood(this ModelNPC.ObjectData model)
    {
        return model is ModelNPC.WallWood;
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
