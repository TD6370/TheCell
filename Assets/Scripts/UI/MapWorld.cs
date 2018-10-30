using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWorld : MonoBehaviour {

    public GameObject prefabField;
    public GameObject prefabRock;
    public GameObject prefabVood;
    public GameObject prefabMapCell;

    public Dictionary<string, GameObject> MapObjects;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //public void Create()
    //{
    //    var worldData = Storage.GridData.

    //            string nameField = Helper.GetNameField(x, y);

    //            List<GameObject> ListNewObjects = new List<GameObject>();
    //            for (int i = 0; i < maxObjectInField; i++)
    //            {

    //                int intTypePrefab = UnityEngine.Random.Range(1, 5);
    //                //TypePrefabs prefabName = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), intTypePrefab.ToString()); ;
    //                string prefabName = "";

    //                int _y = y * (-1);
    //                Vector3 pos = new Vector3(x, _y, 0) * Storage.ScaleWorld;
    //                pos.z = -1;
    //                //if (prefabName == TypePrefabs.PrefabUfo)
    //                //    pos.z = -2;

    //                //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

    //                string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
    //                //GameObject newField = (GameObject)Instantiate(prefabField, pos, Quaternion.identity);

    //                newField.tag = "Field";
    //                newField.name = nameField;
    //                Storage.Instance.Fields.Add(nameField, newField);
    //            }

    //}

    public void Create()
    {
        ClearWorld();
        int index = 0;
        float scaleMap = 0.5f;

        for (int y = 0; y < Helper.HeightWorld ; y++)
        {
            for (int x = 0; x < Helper.WidthWorld; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 3);

                int maxObjectInField = (intRndCount == 0) ? 1 : 0;
                string nameField = Helper.GetNameField(x, y);

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {
                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * scaleMap;
                    pos.z = -1;

                    
                    SaveLoadData.TypePrefabs prefabName = SaveLoadData.TypePrefabs.PrefabField;
                    foreach (SaveLoadData.ObjectData datObjItem in  Storage.Instance.GridDataG.FieldsD[nameField].Objects)
                    {
                        if(!datObjItem.TagObject.IsPerson())
                        {
                            prefabName = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject); 
                        }
                    }
                    
                    GameObject newField = BildMapObject(prefabName);
                    newField.transform.position = pos;
                    newField.tag = "MapObject";
                    //newField.name = nameField;
                    newField.name = nameField + "_" + prefabName.ToString() + index;
                    MapObjects.Add(nameField, newField);
                    index++;
                }
            }
        }
    }

    private void ClearWorld()
    {
        foreach(var mapObjItem in MapObjects.Values)
        {
            Destroy(mapObjItem);
        }
        MapObjects.Clear();
    }

    private GameObject BildMapObject(SaveLoadData.TypePrefabs prefabName)
    {
        GameObject newField = null;
        //switch (prefabName)
        //{
        //    case SaveLoadData.TypePrefabs.PrefabField:
        //        newField = (GameObject)Instantiate(prefabField, new Vector3(0, 0, -1), Quaternion.identity);
        //        break;
        //    case SaveLoadData.TypePrefabs.PrefabRock:
        //        newField = (GameObject)Instantiate(prefabRock, new Vector3(0, 0, -1), Quaternion.identity);
        //        break;
        //    case SaveLoadData.TypePrefabs.PrefabVood:
        //        newField = (GameObject)Instantiate(prefabVood, new Vector3(0, 0, -1), Quaternion.identity);
        //        break;
        //}
        
        newField = (GameObject)Instantiate(prefabMapCell, new Vector3(0, 0, -1), Quaternion.identity);
        SpriteRenderer sprtRend = newField.GetComponent<SpriteRenderer>();
        switch (prefabName)
        {
            case SaveLoadData.TypePrefabs.PrefabField:
                sprtRend.color = "#8ACA84".ToColor();
                break;
            case SaveLoadData.TypePrefabs.PrefabRock:
                sprtRend.color = "#77A7C2".ToColor();
                break;
            case SaveLoadData.TypePrefabs.PrefabVood:
                sprtRend.color = "#8ACA84".ToColor();
                break;
        }
        return newField;
    }
}
