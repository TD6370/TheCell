using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapWorld : MonoBehaviour {

    public GameObject prefabField;
    public GameObject prefabRock;
    public GameObject prefabVood;
    public GameObject prefabMapCell;
    public GameObject prefabFrameMap;

    public int SizeCellMap = 25;

    private string[,] colorMap = new string[1000, 1000];
    private bool m_IsCreatedMap = false;

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

    private void CreateFrameMap()
    {
        Vector2 posHero = Storage.PlayerController.transform.position;
        //prefabFrameMap.transform.position = new Vector3(posHero.x -12.5f, posHero.y + 5, -2);
        //prefabFrameMap.transform.position = new Vector3(posHero.x, posHero.y, -2);
        prefabFrameMap.transform.position = new Vector3(posHero.x, posHero.y, 0);
        //prefabFrameMap.transform.localScale = new Vector3(10F, 10f, 0);
        prefabFrameMap.SetActive(true);
        prefabFrameMap.transform.SetParent(Storage.PlayerController.transform);
    }

   

    public void Create()
    {
        if (!m_IsCreatedMap)
        {
            CreateTextureMap(SizeCellMap);
            m_IsCreatedMap = true;
        }
        else
        {
            prefabFrameMap.SetActive(!prefabFrameMap.activeSelf);
        }
    }

    public void CreateTextureMap(int scaleCell = 1)
    {
        int sizeMap = Helper.HeightLevel;
        int sizeDraw = Helper.HeightLevel * scaleCell;
        int addSize = scaleCell - 1;

        CreateFrameMap();

        Texture2D texture = new Texture2D(sizeDraw, sizeDraw);
        for (int y = 0; y < sizeMap; y++)
        {
            for (int x = 0; x < sizeMap; x++)
            {
                string nameField = Helper.GetNameField(x, y);
                SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
                    continue;

                foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
                {
                    //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
                    if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
                    datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
                    {
                        prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
                    }
                }
                if (prefabType == SaveLoadData.TypePrefabs.PrefabField)
                    continue;

                if (!ManagerPalette.PaletteColors.ContainsKey(prefabType.ToString()))
                {
                    Debug.Log("#### Not Color in PaletteColors for type : " + prefabType.ToString());
                }
                Color colorCell = ManagerPalette.PaletteColors[prefabType.ToString()];

                //----- DRAW
                int startX = x * scaleCell;
                int startY = y * scaleCell;
                for(int x2= startX; x2< startX + addSize; x2++)
                {
                    for (int y2 = startY; y2 < startY + addSize; y2++)
                    {
                        //texture.SetPixel(x2, y2, colorCell);
                        texture.SetPixel(x2, sizeDraw - y2, colorCell);
                    }
                }
            }
        }
        texture.Apply();

        Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    }

    //public void CreateTextureMap()
    //{

    //    CreateFrameMap();

    //    Texture2D texture = new Texture2D(100, 100);
    //    for (int y = 0; y < Helper.HeightLevel; y++)
    //    {
    //        for (int x = 0; x < Helper.WidthLevel; x++)
    //        {
    //            string nameField = Helper.GetNameField(x, y);
    //            SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
    //            if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                continue;

    //            foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //            {
    //                //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                {
    //                    prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                }
    //            }
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabField)
    //                continue;

    //            if (!ManagerPalette.PaletteColors.ContainsKey(prefabType.ToString()))
    //            {
    //                Debug.Log("#### Not Color in PaletteColors for type : " + prefabType.ToString());
    //            }
    //            Color colorCell = ManagerPalette.PaletteColors[prefabType.ToString()];
    //            texture.SetPixel(x, y, colorCell);
    //        }
    //    }
    //    texture.Apply();

    //    Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //    prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    //}

    //public void CreateTextureMap()
    //{
    //    //CreateTextureMapStep();
    //    //return;

    //    CreateFrameMap();


    //    Texture2D texture = new Texture2D(100, 100);
    //    //GetComponent<Renderer>().material.mainTexture = texture;
    //    //prefabFrameMap.GetComponent<Sprite>().texture = texture;

    //    //Sprite spriteMe = Storage.GridData.GetSpriteBoss(Level);
    //    //Texture2D[] _texturesBoss = Resources.LoadAll<Texture2D>(pathSprites + nameStrite);
    //    //Texture2D _texture = _texturesBoss[0];


    //    //if (spriteMe == null)
    //    //{
    //    //    //string _nameSprite = Storage.GridData.GetNameSpriteForIndexLevel(Level);
    //    //    Debug.Log("######## CreateTextureMap spriteMe == null ");
    //    //    return;
    //    //}

    //    //    for (int y = 0; y < texture.height; y++)
    //    //{
    //    //    for (int x = 0; x < texture.width; x++)
    //    //    {
    //    for (int y = 0; y < Helper.HeightLevel; y++)
    //    {
    //        for (int x = 0; x < Helper.WidthLevel; x++)
    //        {
    //            //---------
    //            //Color color = ((x & y) != 0 ? Color.white : Color.gray);
    //            //texture.SetPixel(x, y, color);
    //            //continue;
    //            //---------
    //            string nameField = Helper.GetNameField(x, y);
    //            SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
    //            if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                continue;

    //            foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //            {
    //                //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                {
    //                    prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                    //Debug.Log("Create Prefab : " + prefabName);
    //                }
    //            }
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabField)
    //                continue;

    //            if (!ManagerPalette.PaletteColors.ContainsKey(prefabType.ToString()))
    //            {
    //                Debug.Log("#### Not Color in PaletteColors for type : " + prefabType.ToString());
    //            }

    //            Color colorCell = ManagerPalette.PaletteColors[prefabType.ToString()];
    //            //if (prefabType == SaveLoadData.TypePrefabs.PrefabRock)
    //            //{
    //            //    colorCell = Color.blue;
    //            //}
    //            //if (prefabType == SaveLoadData.TypePrefabs.PrefabVood)
    //            //{
    //            //    colorCell = Color.green;
    //            //}

    //            texture.SetPixel(x, y, colorCell);
    //            //color = ((x & y) != 0 ? Color.blue : Color.green);
    //            //texture.SetPixel(x, y, color);
    //            //------------




    //        }
    //    }
    //    texture.Apply();

    //    Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //    prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    //}

    //public void CreateTextureMapStep()
    //{
    //    CreateFrameMap();
    //    CreateMapIndexColor();

    //    Texture2D texture = new Texture2D(100, 100);

    //    for (int y = 0; y < Helper.HeightLevel; y++)
    //    {
    //        for (int x = 0; x < Helper.WidthLevel; x++)
    //        {

    //            if (string.IsNullOrEmpty(colorMap[x, y]))
    //                continue;

    //            string prefabType = colorMap[x, y];

    //            //if (!ManagerPalette.PaletteColors.ContainsKey(prefabType))
    //            //{
    //            //    Debug.Log("#### Not Color in PaletteColors for type : " + prefabType);
    //            //}

    //            //Color colorCell = ManagerPalette.PaletteColors[prefabType];
    //            Color colorCell = Color.yellow;
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabRock.ToString())
    //            {
    //                colorCell = Color.blue;
    //            }
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabVood.ToString())
    //            {
    //                colorCell = Color.green;
    //            }

    //            texture.SetPixel(x, y, colorCell);
    //        }
    //    }
    //    texture.Apply();

    //    Sprite spriteMe = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    //    prefabFrameMap.GetComponent<SpriteRenderer>().sprite = spriteMe;
    //}

    //private void CreateMapIndexColor()
    //{

    //    //int[,] array2Da = new int[4, 2];
    //    int maxY = 0;
    //    int maxX = 0;

    //    for (int y = 0; y < Helper.HeightWorld; y++)
    //    {
    //        for (int x = 0; x < Helper.WidthWorld; x++)
    //        {
    //            //---------

    //            //continue;
    //            //---------
    //            string nameField = Helper.GetNameField(x, y);
    //            SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;
    //            if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                continue;

    //            foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //            {
    //                //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                {
    //                    prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                    //Debug.Log("Create Prefab : " + prefabName);
    //                }
    //            }
    //            if (prefabType == SaveLoadData.TypePrefabs.PrefabField)
    //                continue;

    //            //if (!ManagerPalette.PaletteColors.ContainsKey(prefabType.ToString()))
    //            //{
    //            //    Debug.Log("#### Not Color in PaletteColors for type : " + prefabType.ToString());
    //            //}

    //            //Color colorCell = ManagerPalette.PaletteColors[prefabType.ToString()];
    //            //if (prefabType == SaveLoadData.TypePrefabs.PrefabRock)
    //            //{
    //            //    colorCell = Color.blue;
    //            //}
    //            //if (prefabType == SaveLoadData.TypePrefabs.PrefabVood)
    //            //{
    //            //    colorCell = Color.green;
    //            //}
    //            if (x > maxX)
    //                maxX = x;
    //            if (y > maxY)
    //                maxY = y;

    //            colorMap[x,y]= prefabType.ToString();
    //            //------------
    //        }
    //    }
    //    Debug.Log("---------- LIMIT X Y = " + maxX + "x" + maxY);
    //}

    //private void CreateRenderSprite()
    //{
    //    //SpriteRenderer[] loadedRenderers = GetComponentsInChildren<SpriteRenderer>(true);

    //    //Texture2D atlas = Resources.Load(activeSkin, typeof(Texture2D)) as Texture2D;

    //    //for (int i = 0; i < loadedRenderers.Length; i++)
    //    //{
    //    //    try
    //    //    {
    //    //        // replace the current sprite with the desired sprite, but using the loaded sprite as a cut out reference via 'rect'
    //    //        loadedRenderers[i].sprite = Sprite.Create(atlas, loadedRenderers[i].sprite.rect, new Vector2(0.5f, 0.5f));

    //    //        // update name, main texture and shader, these all seem to be required... even thou you'd think it already has a shader :|
    //    //        loadedRenderers[i].sprite.name = loadedRenderers[i].name + "_sprite";
    //    //        loadedRenderers[i].material.mainTexture = atlas as Texture;
    //    //        loadedRenderers[i].material.shader = Shader.Find("Sprites/Default");
    //    //    }
    //    //    catch (Exception e) { }
    //    //}
    //}

    //public void CreatePrefabsMap()
    //{
    //    string indErr = "start";
    //    int index = 0;
    //    float scaleMap = 0.2f;

    //        ClearWorld();

    //    try
    //    {
    //        indErr = "1.";
    //        MapObjects = new Dictionary<string, GameObject>();

    //        int schet = 0;

    //        GameObject newFieldBackground = BildMapObject(SaveLoadData.TypePrefabs.PrefabField);
    //        newFieldBackground.transform.position = new Vector3(10, -10, -2);
    //        newFieldBackground.transform.localScale = new Vector3(100F, 100f, 0);
    //        //newFieldBackground.GetComponent<PositionRenderSorting>()
    //        //newField.GetComponent<Sprite>().
    //        //rectTransform.sizeDelta = new Vector2(width, height);
    //        newFieldBackground.tag = "MapObject";
    //        newFieldBackground.SetActive(true);

    //        indErr = "2.";
    //        for (int y = 0; y < Helper.HeightWorld ; y++)
    //        {
    //            for (int x = 0; x < Helper.WidthWorld; x++)
    //            {
    //                //indErr = "3.";
    //                //int intRndCount = UnityEngine.Random.Range(0, 3);

    //                //indErr = "4.";
    //                //int maxObjectInField = (intRndCount == 0) ? 1 : 0;
    //                indErr = "5.";
    //                string nameField = Helper.GetNameField(x, y);

    //                indErr = "6.";
    //                List<GameObject> ListNewObjects = new List<GameObject>();
    //                //for (int i = 0; i < maxObjectInField; i++)
    //                //{
    //                indErr = "7.";
    //                int _y = y * (-1);
    //                Vector3 pos = new Vector3(x, _y, 0) * scaleMap;
    //                pos.z = -10;
    //                indErr = "8.";
    //                SaveLoadData.TypePrefabs prefabName = SaveLoadData.TypePrefabs.PrefabField;
    //                indErr = "9.";

    //                if (!Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                    continue;

    //                //Storage.Instance.GridDataG.FieldsD[nameField]
    //                if (Storage.Instance.GridDataG.FieldsD.ContainsKey(nameField))
    //                {

    //                    foreach (SaveLoadData.ObjectData datObjItem in Storage.Instance.GridDataG.FieldsD[nameField].Objects)
    //                    {
    //                        //Debug.Log("++++++++ : " + datObjItem + " " + datObjItem.TagObject + " =" + datObjItem.TagObject.IsPerson());
    //                        indErr = "10.";
    //                        //if (!datObjItem.TagObject.IsPerson())
    //                        if (datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabUfo.ToString() &&
    //                        datObjItem.TagObject != SaveLoadData.TypePrefabs.PrefabBoss.ToString())
    //                        {
    //                            indErr = "11.";
    //                            prefabName = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), datObjItem.TagObject);
    //                            //Debug.Log("Create Prefab : " + prefabName);
    //                        }
    //                    }
    //                }
    //                indErr = "12.";
    //                //Debug.Log("Bild : " + prefabName);
    //                GameObject newField = BildMapObject(prefabName);
    //                newField.transform.position = pos;
    //                newField.tag = "MapObject";
    //                newField.SetActive(true);
    //                //newField.name = nameField;
    //                indErr = "13.";
    //                newField.name = "MapPoint_" + nameField + "_" + prefabName.ToString() + index;
    //                indErr = "14.";
    //                //Debug.Log("MapObjects : " + newField.name);
    //                MapObjects.Add(nameField, newField);
    //                indErr = "15.";
    //                index++;
    //               // }
    //            }
    //            indErr = "next";
    //            schet++;
    //            if(schet>10)
    //            {
    //                schet = 0;
    //                Debug.Log("Next y=" + y);
    //            }

    //        }
    //        indErr = "end";

    //    }
    //    catch (Exception x)
    //    {
    //        Debug.Log("############# MapWorld.Create[" + indErr + "] : " + x.Message);
    //    }
    //    indErr = "ok";
    //    Debug.Log("Map Worl is loaded ))");
    //}

    //private void ClearWorld()
    //{
    //    try
    //    {

    //        if (MapObjects == null)
    //            return;

    //        foreach (var mapObjItem in MapObjects.Values)
    //        {
    //            Destroy(mapObjItem);
    //        }
    //        MapObjects.Clear();
    //    }catch(Exception x)
    //    {
    //        Debug.Log("############# MapWorld.ClearWorld : " + x.Message);
    //    }
    //}

    //private GameObject BildMapObject(SaveLoadData.TypePrefabs prefabName)
    //{
    //    GameObject newField = null;
    //    //switch (prefabName)
    //    //{
    //    //    case SaveLoadData.TypePrefabs.PrefabField:
    //    //        newField = (GameObject)Instantiate(prefabField, new Vector3(0, 0, -1), Quaternion.identity);
    //    //        break;
    //    //    case SaveLoadData.TypePrefabs.PrefabRock:
    //    //        newField = (GameObject)Instantiate(prefabRock, new Vector3(0, 0, -1), Quaternion.identity);
    //    //        break;
    //    //    case SaveLoadData.TypePrefabs.PrefabVood:
    //    //        newField = (GameObject)Instantiate(prefabVood, new Vector3(0, 0, -1), Quaternion.identity);
    //    //        break;
    //    //}

    //    newField = (GameObject)Instantiate(prefabMapCell, new Vector3(10, 10, -10), Quaternion.identity);
    //    SpriteRenderer sprtRend = newField.GetComponent<SpriteRenderer>();
    //    switch (prefabName)
    //    {
    //        case SaveLoadData.TypePrefabs.PrefabField:
    //            sprtRend.color = "#8ACA84".ToColor();
    //            break;
    //        case SaveLoadData.TypePrefabs.PrefabRock:
    //            sprtRend.color = "#77A7C2".ToColor();
    //            break;
    //        case SaveLoadData.TypePrefabs.PrefabVood:
    //            sprtRend.color = "#379200".ToColor();
    //            break;
    //    }
    //    return newField;
    //}
}
