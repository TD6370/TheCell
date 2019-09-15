using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenericWorldManager : MonoBehaviour {

    public enum GenObjectWorldMode
    {
        Standart, Floorl, FloorGray, Flore, Prefab
    }

    [SerializeField]
    public ContainerPriorityFinder ContainerPrioritysTerra;
    public Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> TerraPriority;
    public Dictionary<string, int> CollectionPowerAllTypes;

    [Header("Count Prioritys Join ID")]
    public int CountPrioritysJoinID = 0;

    // Use this for initialization
    void Start () {
		
	}
	// Update is called once per frame
	void Update () {
	}

    public void GenericWorldLegacy()
    {
        int coutCreateObjects = 0;
        SaveLoadData.TypePrefabs prefabName = SaveLoadData.TypePrefabs.PrefabField;
        Debug.Log("# CreateDataGamesObjectsWorld...");
        Storage.Instance.ClearGridData();

        int countAll = Helper.HeightLevel * 2;
        int index = 1;

        for (int y = 0; y < Helper.HeightLevel; y++)
        {
            for (int x = 0; x < Helper.WidthLevel; x++)
            {
                int intRndCount = UnityEngine.Random.Range(0, 3);

                int maxObjectInField = (intRndCount == 0) ? 1 : 0;
                string nameField = Helper.GetNameField(x, y);

                List<GameObject> ListNewObjects = new List<GameObject>();
                for (int i = 0; i < maxObjectInField; i++)
                {
                    //GEN -----
                    prefabName = GenObjectWorld();// UnityEngine.Random.Range(1, 8);
                    //if (prefabName == TypePrefabs.PrefabField)
                    //    continue;
                    //-----------

                    index++;
                    Storage.EventsUI.SetTittle = String.Format("Loading {0} %", (countAll / index).ToString());

                    int _y = y * (-1);
                    Vector3 pos = new Vector3(x, _y, 0) * SaveLoadData.Spacing;
                    pos.z = -1;
                    if (prefabName == SaveLoadData.TypePrefabs.PrefabUfo)
                        pos.z = -2;

                    //Debug.Log("CreateGamesObjectsWorld  " + nameFiled + "  prefabName=" + prefabName + " pos =" + pos + "    Spacing=" + Spacing + "   x=" + "   y=" + y);

                    string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");// prefabName.ToString() + "_" + nameFiled + "_" + i;
                    ModelNPC.ObjectData objDataSave = BilderGameDataObjects.BildObjectData(prefabName, true);
                    objDataSave.SetNameObject(nameObject);
                    objDataSave.Position = pos;
                    //objDataSave.SetPosition(pos);//###ERR

                    coutCreateObjects++;

                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                }
            }
        }

        Storage.Data.SaveGridGameObjectsXml(true);

        Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

        Storage.EventsUI.SetTittle = String.Format("World is Loaded");
    }

    public void GenericWorld()
    {
        Storage.Instance.IsLoadingWorld = true;

        if (Storage.PaletteMap.SelectedCell == null)
        {
            Storage.PaletteMap.GenericOnWorld(false, SaveLoadData.TypePrefabs.PrefabWallWood);
        }
        else
        {
            Storage.PaletteMap.GenericOnWorld(true);
        }
        Storage.Instance.IsLoadingWorld = false;
    }

    public void GenericWorldExtremal()
    {
        //StartCoroutine(CreateDataGamesObjectsExtremalWorldProgress());
    }


    //GEN -----
    public SaveLoadData.TypePrefabs GenObjectWorld(GenObjectWorldMode mode = GenObjectWorldMode.Standart)
    {
        SaveLoadData.TypePrefabs prefabName = SaveLoadData.TypePrefabs.PrefabField;
        int max = 0;
        int ind = 0;
        int lockTest = 0;

        switch (mode)
        {
            case GenObjectWorldMode.Standart:
                max = Enum.GetValues(typeof(SaveLoadData.TypePrefabs)).Length - 1;
                ind = UnityEngine.Random.Range(12, max);
                prefabName = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), ind.ToString());
                break;
            case GenObjectWorldMode.FloorGray:
                SaveLoadData.TypePrefabFloors floor = SaveLoadData.TypePrefabFloors.Desert;
                max = Storage.GridData.GetFloorGray.Count;
                ind = UnityEngine.Random.Range(0, max);
                prefabName = Storage.GridData.GetFloorGray[ind];
                break;
            default:
                break;
        }

        return prefabName;

        int intGen = UnityEngine.Random.Range(1, 4);
        int intTypePrefab = 0;
        

        if (intGen == 1)
            prefabName = SaveLoadData.TypePrefabs.PrefabBoss;
        else
        {
            intTypePrefab = UnityEngine.Random.Range(1, 8);
            prefabName = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), intTypePrefab.ToString()); ;
        }
        int rnd1 = UnityEngine.Random.Range(1, 3);
        if (rnd1 != 1)
        {
            prefabName = SaveLoadData.TypePrefabs.PrefabField;
        }
        return prefabName;
    }

    public void GenericStandart(PaletteMapController palette, DataTile selDataTile, int startX, int startY, int ContAll, int sizeX, int sizeY, int limitRepeat, int indexRepeat)
    {
        bool isValidResult = true;
        bool _isClearLayer = palette.IsClearStart;
        for (int i = 0; i < ContAll; i++)
        {
            int x = Random.Range(startX, sizeX);
            int y = Random.Range(startY, sizeY);
            string fieldNew = Helper.GetNameField(x, y);
            if (_isClearLayer)
                ClearLayerForStructure(fieldNew);

            if (!palette.IsGenericContruct)
            {
                isValidResult = Storage.GridData.AddConstructInGridData(fieldNew, selDataTile, palette.TypeModeOptStartDelete, palette.TypeModeOptStartCheck);
            }
            else
            {
                Storage.Instance.SelectFieldCursor = fieldNew;
                palette.GenericContructInGridData();
            }

            if (!isValidResult && palette.IsRepeatFind && indexRepeat < limitRepeat)
            {
                //#repeat
                i--;
                indexRepeat++;
            }
        }
    }

    public void GenericSpawnPoint(PaletteMapController palette, DataTile selDataTile, int startX, int startY, int ContAll, int radiusSpawn,  int limitRepeat, int indexRepeat)
    {
        bool _isClearLayer = palette.IsClearStart;
        int indexDiff = 0;
        //------ Spawn Point
        for (int i = 0; i < ContAll; i++)
        {

            int offsetX = Random.Range(1, radiusSpawn);
            int offsetY = Random.Range(1, radiusSpawn);
            bool isLeft = 1 == Random.Range(1, 3);
            bool isTop = 1 == Random.Range(1, 3);
            if (isLeft)
                offsetX *= -1;
            if (isTop)
                offsetY *= -1;
            int x = startX + offsetX;
            int y = startY + offsetY;

            string fieldNew = Helper.GetNameField(x, y);
            if (_isClearLayer)
                ClearLayerForStructure(fieldNew);

            bool isValidResult = Storage.GridData.AddConstructInGridData(fieldNew, selDataTile, palette.TypeModeOptStartDelete, palette.TypeModeOptStartCheck);

            if (!isValidResult && palette.IsRepeatFind && indexRepeat < limitRepeat)
            {
                //#repeat
                i--;
                indexRepeat++;
            }

            //Spawn activity
            indexDiff++;
            if (indexDiff >= palette.SpawnPointScope)
            {
                indexDiff = 0;
                radiusSpawn -= palette.SpawnPointScope;
                if (radiusSpawn < 2)
                    radiusSpawn = 2;
            }
        }
    }


    public void GenericSegment(PaletteMapController palette, DataTile selDataTile, int startX, int startY, int ContAll, int sizeX, int sizeY, int limitRepeat, int indexRepeat, int SubsystemSegments, int SubsystemLevel)
    {
        bool isValidResult = true;
        bool _isClearLayer = palette.IsClearStart;
        int minLevel = (SubsystemLevel / 2) * (-1);
        int maxLevel = (SubsystemLevel / 2);

        for (int i = 0; i < ContAll; i++)
        {
            startX = Random.Range(startX, sizeX);
            startY = Random.Range(startY, sizeY);

            for (int s = 0; s < SubsystemSegments; s++)
            {
                int offsetX = Random.Range(minLevel, maxLevel);
                int offsetY = Random.Range(minLevel, maxLevel);
                int x = 0;
                int y = 0;
                if (palette.IsSteps)
                {
                    startX += offsetX;
                    startY += offsetY;
                    x = startX;
                    y = startY;
                }
                else
                {
                    x = startX + offsetX;
                    y = startY + offsetY;
                }
                string fieldNew = Helper.GetNameField(x, y);
                if (_isClearLayer)
                    ClearLayerForStructure(fieldNew);

                if (!palette.IsGenericContruct)
                {
                    isValidResult = Storage.GridData.AddConstructInGridData(fieldNew, selDataTile, palette.TypeModeOptStartDelete, palette.TypeModeOptStartCheck);
                }
                else
                {
                    Storage.Instance.SelectFieldCursor = fieldNew;
                    palette.GenericContructInGridData();
                }

                if (!isValidResult && palette.IsRepeatFind && indexRepeat < limitRepeat)
                {
                    //#repeat
                    s--;
                    indexRepeat++;
                }
            }
            indexRepeat = 0;
        }

    }

    public string GenericSegmentNextPoint(PaletteMapController palette, DataTile selDataTile, int startFieldX, int startFieldY, int ContAll, int sizeX, int sizeY, int limitRepeat, int indexRepeat, int SubsystemSegments, int SubsystemLevel)
    {
        List<Vector2Int> stepsPointsSart = new List<Vector2Int>();
        int startSegmentX = 0;
        int startSegmentY = 0;
        string info = "";

        bool isValidResult = true;
        bool _isClearLayer = palette.IsClearStart;
        int minLevel = (SubsystemLevel / 2) * (-1);
        int maxLevel = (SubsystemLevel / 2);
        int startX = 0;
        int startY = 0;

        for (int i = 0; i < ContAll; i++)
        {
            startX = Random.Range(startFieldX, sizeX);
            startY = Random.Range(startFieldY, sizeY);

            if (!palette.IsSteps)
                stepsPointsSart = new List<Vector2Int>();

            stepsPointsSart.Add(new Vector2Int(startX, startY));

            for (int s = 0; s < SubsystemSegments; s++)
            {
                if (!palette.IsSteps)
                {
                    startSegmentX = 0;
                    startSegmentY = 0;
                }
                if (s < 3)
                {
                    startSegmentX = startX;
                    startSegmentY = startY;
                }
                else
                {
                    //-------------------- Select Next start Point segment
                    if (palette.ModeSegmentMarginLimit == PaletteMapController.ModeStartSegmentGen.Margin) //------------ number limit
                    {
                        int max = stepsPointsSart.Count - 1;

                        Vector2Int corrRange = palette.GetValidMargin((int)palette.FirstLimit, (int)palette.LastLimit, max);
                        int firstCorr = corrRange.x;
                        int lastCorr = corrRange.y;

                        int SelectedPintindex = 0;
                        if (palette.IsFirstStartSegment)
                        {
                            SelectedPintindex = Random.Range(0, firstCorr);

                            if (palette.IsLogGenericWorld)
                                info = "margin (0--" + firstCorr + ")   max = " + max + "  I=" + SelectedPintindex + "  [" + palette.FirstLimit + @"\" + palette.LastLimit + "]";
                        }
                        else
                        {
                            SelectedPintindex = Random.Range(max - lastCorr, max);

                            if (palette.IsLogGenericWorld)
                                info = "margin (" + (max - lastCorr) + "--" + max + ")    max = " + max + "  I=" + SelectedPintindex + "  [" + palette.FirstLimit + @"\" + palette.LastLimit + "]";
                        }

                        startSegmentX = stepsPointsSart[SelectedPintindex].x;
                        startSegmentY = stepsPointsSart[SelectedPintindex].y;

                        if (palette.IsLogGenericWorld)
                        {
                            info += " < " + startSegmentX + "x" + startSegmentY + " >";
                            //if (palette.isLog)
                            //    Storage.EventsUI.ListLogAdd = info;
                            //Debug.Log(info);
                        }
                    }
                    if (palette.ModeSegmentMarginLimit == PaletteMapController.ModeStartSegmentGen.Range) //------------ percent limit
                    {
                        int max = stepsPointsSart.Count - 1;

                        float firstCorr = palette.FirstLimit;
                        if (firstCorr < 0f || firstCorr >= 1f)
                            firstCorr = 0.5f;
                        float lastCorr = palette.LastLimit;
                        if (lastCorr < 0f || firstCorr >= 1f)
                            firstCorr = 0.5f;

                        int SelectedPintindex = 0;
                        if (palette.IsFirstStartSegment)
                        {
                            firstCorr = max * palette.FirstLimit;
                            firstCorr = (int)firstCorr;
                            if (firstCorr < 1)
                                firstCorr = 1;

                            SelectedPintindex = Random.Range(0, (int)firstCorr);

                            if (palette.IsLogGenericWorld)
                                info = "percent 0 -- " + firstCorr + "  max = " + max + "   index = " + SelectedPintindex;
                        }
                        else
                        {

                            lastCorr = max - (max * palette.LastLimit);
                            lastCorr = (int)lastCorr;
                            SelectedPintindex = Random.Range((int)lastCorr, max);

                            if (palette.IsLogGenericWorld)
                                info = "percent  " + lastCorr + " -- " + max + "    max = " + max + "   index = " + SelectedPintindex;
                        }

                        startSegmentX = stepsPointsSart[SelectedPintindex].x;
                        startSegmentY = stepsPointsSart[SelectedPintindex].y;

                        if (palette.IsLogGenericWorld)
                        {
                            info += " < " + startSegmentX + "x" + startSegmentY + " >";
                            //if (isLog)
                            //    Storage.EventsUI.ListLogAdd = info;
                            //Debug.Log(info);
                        }
                    }
                    //--------------------
                }

                //---- #fix start gen
                int offsetX = Random.Range(minLevel, maxLevel);
                int offsetY = Random.Range(minLevel, maxLevel);
                int x = 0;
                int y = 0;
                bool isLeft = 1 == Random.Range(1, 3);
                bool isTop = 1 == Random.Range(1, 3);
                if (isLeft)
                    offsetX *= -1;
                if (isTop)
                    offsetY *= -1;
                x = startSegmentX + offsetX;
                y = startSegmentY + offsetY;
                //------------

                stepsPointsSart.Add(new Vector2Int(x, y));

                string fieldNew = Helper.GetNameField(x, y);

                if (_isClearLayer || palette.TypeModeOptStartDelete == PaletteMapController.SelCheckOptDel.DelFull)
                    GenericWorldManager.ClearLayerForStructure(fieldNew);

                if (!palette.IsGenericContruct)
                {
                    isValidResult = Storage.GridData.AddConstructInGridData(fieldNew, selDataTile, palette.TypeModeOptStartDelete, palette.TypeModeOptStartCheck);
                }
                else
                {
                    Storage.Instance.SelectFieldCursor = fieldNew;
                    palette.GenericContructInGridData();
                }

                if (!isValidResult && palette.IsRepeatFind && indexRepeat < limitRepeat)
                {
                    //#repeat
                    s--;
                    indexRepeat++;
                }
            }
            indexRepeat = 0;
        }
        //------
        return info;
    }


    public static void ClearLayerForStructure(string field, bool isClearData = false)
    {
        //Destroy All Objects
        if (Storage.Instance.GamesObjectsReal.ContainsKey(field))
        {
            var listObjs = Storage.Instance.GamesObjectsReal[field];

            foreach (var obj in listObjs.ToArray())
            {
                if (PoolGameObjects.IsUsePoolObjects)
                {
                    obj.DisableComponents();
                    Storage.Instance.DestroyFullObject(obj);
                }
                else
                {
                    Storage.Instance.AddDestroyGameObject(obj);
                }
            }
        }

        if (isClearData)
        {
            if (Storage.Map.IsGridMap)
                Storage.Map.CheckSector(field);

            //Destroy All DATA Objects
            if (ReaderScene.IsGridDataFieldExist(field))
            {
                Storage.Data.ClearObjecsDataFromGrid(field);
            }
        }
    }

    #region Generic on Priority

    public void GenericWorldPriorityTerra(int PriorityIdleStartPercent, int PriorityPrefabPercent, int PriorityTreePercent, int PriorityRockPercent, int PriorityFlorePercent, int PriorityDistantionFind)
    {
        string indErr = "0";
        ModelNPC.ObjectData requestedGenTerra = null;
        ModelNPC.ObjectData receivedGenTerra = null;
        Action _loadPriority = LoadTerraPriority;
        try
        {
            if (TerraPriority == null)
                LoadTerraPriority();

            indErr = "1";
            //receivedGenTerra = Helper.GenericOnPriority(requestedGenTerra, TerraPriority, _loadPriority);

            //-------------
            int coutCreateObjects = 0;
            SaveLoadData.TypePrefabs prefabName = SaveLoadData.TypePrefabs.PrefabField;
            Debug.Log("Sart... GenericWorldPriorityTerra...");              indErr = "2";
            Storage.Instance.ClearGridData();

            int countAll = 0;
            int index = 0;
            Dictionary<int, Vector2Int> cellsPos = new Dictionary<int, Vector2Int>(); indErr = "3";
            for (int y = 0; y < Helper.HeightLevel; y++)
            {
                for (int x = 0; x < Helper.WidthLevel; x++)
                {
                    int indRnd = Random.Range(1, index+5);
                    cellsPos.Add(index++, new Vector2Int(x, y));
                }
            }
            
            Queue<Vector2Int> colectionPosRnd = new Queue<Vector2Int>();
            //List<Vector2Int> listPosRnd = cellsPos.OrderBy(p => p.Key).Select(p => p.Value).ToList(); ;
            foreach(var item in cellsPos.OrderBy(p => p.Key).Select(p => p.Value))
            {
                colectionPosRnd.Enqueue(item);
            }

            countAll = colectionPosRnd.Count;

            int startPercent = PriorityIdleStartPercent;
            if (startPercent == 0)
                startPercent = 50;
            int countNotPriority = (countAll * startPercent) / 100;

            int distantionFind = PriorityDistantionFind;
            if (distantionFind == 0)
                distantionFind = 2;

            foreach (Vector2Int cellPos in colectionPosRnd)
            {
                int x = cellPos.x;
                int y = cellPos.y;
                string nameField = Helper.GetNameField(x, y);
                Storage.EventsUI.SetTittle = String.Format("Loading {0} %", (countAll / (coutCreateObjects + 1)).ToString());
                int _y = y * (-1);
                Vector3 pos = new Vector3(x, _y, 0) * SaveLoadData.Spacing;
                pos.z = -1;

                //GEN ----- floor gray
                bool isUsePriority = coutCreateObjects > countNotPriority;
                prefabName = GenObjectWorld(GenObjectWorldMode.FloorGray);
                if (isUsePriority)
                {
                    receivedGenTerra = Helper.GenericOnPriorityByType(prefabName, pos, distantionFind, TerraPriority, _loadPriority, true);
                    prefabName = receivedGenTerra.TypePrefab;
                }
                string nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");
                ModelNPC.ObjectData objDataSave = BilderGameDataObjects.BildObjectData(prefabName, true);
                objDataSave.SetNameObject(nameObject);
                objDataSave.Position = pos;
                coutCreateObjects++;

                Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
            }

            //Storage.Data.SaveGridGameObjectsXml(true);

            Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

            Storage.EventsUI.SetTittle = String.Format("World is Loaded");
            //---------------
        }
        catch (Exception ex)
        {
            Storage.EventsUI.ListLogAdd = "##### GenericCellWorld #" + indErr + "  " + ex.Message;
            return;
        }
    }

    private void LoadTerraPriority()
    {
        try
        {
            TerraPriority = Helper.GetPrioritys(ContainerPrioritysTerra, "Terra");
            CollectionPowerAllTypes = Helper.FillPrioritys(TerraPriority);
        }
        catch (Exception ex)
        {
            Storage.EventsUI.ListLogAdd = "##### LoadTerraPriority : " + ex.Message;
        }
    }

    public int GetPriorityPowerByJoin(SaveLoadData.TypePrefabs prefabNameType, SaveLoadData.TypePrefabs prefabNameTypeTarget)
    {
        string keyJoinNPC = prefabNameType + "_" + prefabNameTypeTarget;
        if (!CollectionPowerAllTypes.ContainsKey(keyJoinNPC))
        {
            Debug.Log("########## GetPriorityPowerByJoin Not Key = " + keyJoinNPC);
            return 0;
        }
        return CollectionPowerAllTypes[keyJoinNPC];
    }

    #endregion
}

