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

    private void Awake()
    {
        MapObjects = new Dictionary<string, GameObject>();
    }

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

    public void CreateTextureMap()
    {
        Texture2D texture = new Texture2D(128, 128);
        GetComponent<Renderer>().material.mainTexture = texture;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color color = ((x & y) != 0 ? Color.white : Color.gray);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
    }

    public void Create()
    {
        string indErr = "start";
        int index = 0;
        float scaleMap = 0.2f;

            ClearWorld();

        try
        {
            indErr = "1.";
            MapObjects = new Dictionary<string, GameObject>();

            int schet = 0;

            GameObject newFieldBackground = BildMapObject(SaveLoadData.TypePrefabs.PrefabField);
            newFieldBackground.transform.position = new Vector3(10, -10, -2);
            newFieldBackground.transform.localScale = new Vector3(100F, 100f, 0);
            //newFieldBackground.GetComponent<PositionRenderSorting>()
            //newField.GetComponent<Sprite>().
            //rectTransform.sizeDelta = new Vector2(width, height);
            newFieldBackground.tag = "MapObject";
            newFieldBackground.SetActive(true);

            indErr = "2.";
            for (int y = 0; y < Helper.HeightWorld ; y++)
            {
                for (int x = 0; x < Helper.WidthWorld; x++)
                {
                    //indErr = "3.";
                    //int intRndCount = UnityEngine.Random.Range(0, 3);

                    //indErr = "4.";
                    //int maxObjectInField = (intRndCount == 0) ? 1 : 0;
                    indErr = "5.";
                    string nameField = Helper.GetNameField(x, y);

                    indErr = "6.";
                    List<GameObject> ListNewObjects = new List<GameObject>();
                    //for (int i = 0; i < maxObjectInField; i++)
                    //{
                    indErr = "7.";
                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * scaleMap;
                    pos.z = -10;
                    indErr = "8.";
                    SaveLoadData.TypePrefabs prefabName = SaveLoadData.TypePrefabs.PrefabField;
                    indErr = "9.";

                    if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                        continue;

                    //Storage.Instance.GridDataG.FieldsD[nameField]
                    if (Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                    {
                        
                        foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
                        {
                            //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
                            indErr = "10.";
                            //if (!datObjItem.TagObject.IsPerson())
                            if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                            datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
                            {
                                indErr = "11.";
                                prefabName = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
                                //Debug.Log("Create Prefab : " + prefabName);
                            }
                        }
                    }
                    indErr = "12.";
                    //Debug.Log("Bild : " + prefabName);
                    GameObject newField = BildMapObject(prefabName);
                    newField.transform.position = pos;
                    newField.tag = "MapObject";
                    newField.SetActive(true);
                    //newField.name = nameField;
                    indErr = "13.";
                    newField.name = "MapPoint_" + nameField + "_" + prefabName.ToString() + index;
                    indErr = "14.";
                    //Debug.Log("MapObjects : " + newField.name);
                    MapObjects.Add(nameField, newField);
                    indErr = "15.";
                    index++;
                   // }
                }
                indErr = "next";
                schet++;
                if(schet>10)
                {
                    schet = 0;
                    Debug.Log("Next y=" + y);
                }

            }
            indErr = "end";

        }
        catch (Exception x)
        {
            Debug.Log("############# MapWorld.Create[" + indErr + "] : " + x.Message);
        }
        indErr = "ok";
        Debug.Log("Map Worl is loaded ))");
    }

    private void ClearWorld()
    {
        try
        {

            if (MapObjects == null)
                return;

            foreach (var mapObjItem in MapObjects.Values)
            {
                Destroy(mapObjItem);
            }
            MapObjects.Clear();
        }catch(Exception x)
        {
            Debug.Log("############# MapWorld.ClearWorld : " + x.Message);
        }
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
        
        newField = (GameObject)Instantiate(prefabMapCell, new Vector3(10, 10, -10), Quaternion.identity);
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
                sprtRend.color = "#379200".ToColor();
                break;
        }
        return newField;
    }
}
