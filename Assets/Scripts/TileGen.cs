using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

//using System; 
//using System.Collections; 
//using System.Collections.Generic; 
//using UnityEngine; 
//using UnityEngine.Tilemaps; 

//#if UNITY_EDITOR 
//using UnityEditor; 
//#endif 

//public class BitmaskTile49 : Tile 
//{ 
//    public Sprite[] sprites; 
//    public Sprite previewSprite; 

//    public Dictionary<int, int> maskIndex = new Dictionary<int, int>() { 
//     { 0, 0 }, 
//     { 4, 1 }, 
//     { 92, 2 }, 
//     { 124, 3 }, 
//     { 116, 4 }, 
//     { 80, 5 }, 
//     // { 0, 6 }, 
//     { 16, 7 }, 
//     { 20, 8 }, 
//     { 87, 9 }, 
//     { 223, 10 }, 
//     { 241, 11 }, 
//     { 21, 12 }, 
//     { 64, 13 }, 
//     { 29, 14 }, 
//     { 117, 15 }, 
//     { 85, 16 }, 
//     { 71, 17 }, 
//     { 221, 18 }, 
//     { 125, 19 }, 
//     { 112, 20 }, 
//     { 31, 21 }, 
//     { 253, 22 }, 
//     { 113, 23 }, 
//     { 28, 24 }, 
//     { 127, 25 }, 
//     { 247, 26 }, 
//     { 209, 27 }, 
//     { 23, 28 }, 
//     { 199, 29 }, 
//     { 213, 30 }, 
//     { 95, 31 }, 
//     { 255, 32 }, 
//     { 245, 33 }, 
//     { 81, 34 }, 
//     { 5, 35 }, 
//     { 84, 36 }, 
//     { 93, 37 }, 
//     { 119, 38 }, 
//     { 215, 39 }, 
//     { 193, 40 }, 
//     { 17, 41 }, 
//     // { 0, 42 }, 
//     { 1, 43 }, 
//     { 7, 44 }, 
//     { 197, 45 }, 
//     { 69, 46 }, 
//     { 68, 47 }, 
//     { 65, 48 } 
//    }; 

//    public override void RefreshTile (Vector3Int location, ITilemap tilemap) 
//    { 
//     for (int y = -1; y <= 1; y++) { 
//      for (int x = -1; x <= 1; x++) { 
//       Vector3Int nextLocation = new Vector3Int (location.x + x, location.y + y, location.z); 

//       if (HasBitmaskTile (nextLocation, tilemap)) { 
//        tilemap.RefreshTile (nextLocation); 
//       } 
//      } 
//     } 
//    } 

//    public override void GetTileData (Vector3Int location, ITilemap tilemap, ref TileData tileData) 
//    { 
//     int north = HasBitmaskTile (location + Vector3Int.up, tilemap) == true ? 1 : 0; 
//     int west = HasBitmaskTile (location + Vector3Int.left, tilemap) == true ? 1 : 0; 
//     int east = HasBitmaskTile (location + Vector3Int.right, tilemap) == true ? 1 : 0; 
//     int south = HasBitmaskTile (location + Vector3Int.down, tilemap) == true ? 1 : 0; 
//     int northwest = HasBitmaskTile (location + Vector3Int.up + Vector3Int.left, tilemap) == true ? 1 & north & west : 0; 
//     int northeast = HasBitmaskTile (location + Vector3Int.up + Vector3Int.right, tilemap) == true ? 1 & north & east : 0; 
//     int southwest = HasBitmaskTile (location + Vector3Int.down + Vector3Int.left, tilemap) == true ? 1 & south & west : 0; 
//     int southeast = HasBitmaskTile (location + Vector3Int.down + Vector3Int.right, tilemap) == true ? 1 & south & east : 0; 

//     int mask = 1 * north + 2 * northeast + 4 * east + 8 * southeast + 16 * south + 32 * southwest + 64 * west + 128 * northwest; 
//     mask -= mask > 255 ? 256 : 0; 

//     tileData.sprite = sprites [maskIndex [mask]]; 
//    } 

//    public bool HasBitmaskTile (Vector3Int location, ITilemap tilemap) 
//    { 
//     return tilemap.GetTile (location) == this; 
//    } 

//    #if UNITY_EDITOR 

//    [MenuItem ("Assets/Create/Bitmasking/Bitmask Tile 49")] 
//    public static void CreateRoadTile() 
//    { 
//     string path = EditorUtility.SaveFilePanelInProject ("Save Tile Bitmask 49", "New Tile Bitmask 49", "Asset", "Save Tile Bitmask 49", "Assets"); 
//     if (path == "") 
//      return; 
//     AssetDatabase.CreateAsset (ScriptableObject.CreateInstance<BitmaskTile49>(), path); 
//    } 

//    #endif 
//} 

