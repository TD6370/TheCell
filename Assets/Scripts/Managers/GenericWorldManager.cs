using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenericWorldManager : MonoBehaviour {

    public enum GenObjectWorldMode
    {
        Standart, Flore, Floor, FloorGray, TreeGray, RockGray, FloreGray, PrefabsGray, GrayGrass, NPC, BlueNPC, RedNPC, GreenNPC, VioletNPC
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
                    ModelNPC.ObjectData objDataSave = BilderGameDataObjects.BildObjectData(prefabName);
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


    private SaveLoadData.TypePrefabs m_prefabNameGenObjectWorld = SaveLoadData.TypePrefabs.PrefabField;
    private int m_maxGenObjWorld = 0;
    private int m_indGenObjWorld = 0;
    //GEN -----
    public SaveLoadData.TypePrefabs GenObjectWorld(GenObjectWorldMode mode = GenObjectWorldMode.Standart)
    {
        switch (mode)
        {
            case GenObjectWorldMode.Standart:
                //m_prefabNameGenObjectWorld = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), Random.Range(12, Enum.GetValues(typeof(SaveLoadData.TypePrefabs)).Length - 1).ToString());
                //m_maxGenObjWorld = Storage.GridData.TypePrefabsCount - 1;
                m_maxGenObjWorld = Storage.GridData.TypePrefabsCount;
                m_indGenObjWorld = UnityEngine.Random.Range(12, m_maxGenObjWorld);
                m_prefabNameGenObjectWorld = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), m_indGenObjWorld.ToString());
                break;
            case GenObjectWorldMode.NPC:
                m_maxGenObjWorld = Storage.GridData.TypePrefabNPCCount;
                m_indGenObjWorld = UnityEngine.Random.Range(2, m_maxGenObjWorld);
                string nameNPC = Storage.GridData.NamesPrefabNPC[m_indGenObjWorld];
                m_prefabNameGenObjectWorld = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), nameNPC);
                break;

            case GenObjectWorldMode.FloorGray:
                m_maxGenObjWorld = Storage.GridData.TypeFloorGrayCount;
                m_indGenObjWorld = UnityEngine.Random.Range(0, m_maxGenObjWorld);
                m_prefabNameGenObjectWorld = Storage.GridData.GetFloorGray[m_indGenObjWorld];
                break;
            case GenObjectWorldMode.TreeGray:
                //m_maxGenObjWorld = Storage.GridData.GetTreeGray.Count;
                m_maxGenObjWorld = Storage.GridData.TypeTreeGrayCount;
                m_indGenObjWorld = UnityEngine.Random.Range(0, m_maxGenObjWorld);
                m_prefabNameGenObjectWorld = Storage.GridData.GetTreeGray[m_indGenObjWorld];
                break;
            case GenObjectWorldMode.RockGray:
                m_maxGenObjWorld = Storage.GridData.TypeRockGrayCount;
                m_indGenObjWorld = UnityEngine.Random.Range(0, m_maxGenObjWorld);
                m_prefabNameGenObjectWorld = Storage.GridData.GetRockGray[m_indGenObjWorld];
                break;
            case GenObjectWorldMode.FloreGray:
                m_maxGenObjWorld = Storage.GridData.TypeFloreGrayCount;
                m_indGenObjWorld = UnityEngine.Random.Range(0, m_maxGenObjWorld);
                m_prefabNameGenObjectWorld = Storage.GridData.GetFloreGray[m_indGenObjWorld];
                break;
            case GenObjectWorldMode.GrayGrass:
                m_maxGenObjWorld = Storage.GridData.TypeGrassGrayCount;
                m_indGenObjWorld = UnityEngine.Random.Range(0, m_maxGenObjWorld);
                m_prefabNameGenObjectWorld = Storage.GridData.GetGrassGray[m_indGenObjWorld];
                break;
            default:

                break;
        }
        return m_prefabNameGenObjectWorld;
    }

    #region Gen palette

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


    #endregion

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

    /*
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
    */


    //TEST OPTIONS
    private bool IsUseGenericTypeTerra = true;
    //private bool IsUseCache = true;
    public bool IsGenOnlyFloor = false;
    public bool IsTestGenNPC = false;
    public bool IsGC = true;
  
    public void GenericWorldPriorityTerra(int PriorityIdleStartPercent, int PriorityDistantionFind, int PriorityPrefabPercent = 0, int PriorityTreePercent = 0, int PriorityRockPercent = 0, int PriorityFlorePercent = 0, bool isStartWorld = false)
    {
        string indErr = "0";
        float testTime = Time.time;
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
        
        //ModelNPC.ObjectData requestedGenTerra = null;
        ModelNPC.ObjectData receivedGenTerra = null;
        Action _loadPriority = LoadTerraPriority;
        try
        {
            Storage.Instance.IsLoadingWorld = true;

            int load = Storage.GridData.GetFloorGray.Count;
            load = Storage.GridData.GetTreeGray.Count;
            load = Storage.GridData.GetRockGray.Count;
            load = Storage.GridData.GetFloreGray.Count;

            if (TerraPriority == null)
                LoadTerraPriority();

            indErr = "1";
            //-------------
            int coutCreateObjects = 0;
            SaveLoadData.TypePrefabs prefabName = SaveLoadData.TypePrefabs.PrefabField;
            Debug.Log("Sart... GenericWorldPriorityTerra..."); indErr = "2";

            Storage.Instance.ClearGridData(); //  >>>TEST 1.

            int countAll = 0;
            int index = 0;
            int indRnd = 0;
            int maxRnd = Helper.HeightLevel * Helper.WidthLevel;

            Dictionary<int, Vector3Int> cellsPos = new Dictionary<int, Vector3Int>(); indErr = "3";
            for (int y = 0; y < Helper.HeightLevel; y++)
            {
                for (int x = 0; x < Helper.WidthLevel; x++)
                {
                    indRnd = Random.Range(1, maxRnd);
                    cellsPos.Add(index++, new Vector3Int(x, y, indRnd));
                }
            }

            Queue<Vector3Int> colectionPosRnd = new Queue<Vector3Int>();
            foreach (var item in cellsPos.OrderBy(p => p.Value.z).Select(p => p.Value))
            {
                colectionPosRnd.Enqueue(item);
            }

            cellsPos.Clear();
            cellsPos = null;

            countAll = colectionPosRnd.Count;

            int startPercent = PriorityIdleStartPercent;
            if (startPercent == 0)
                startPercent = 50;
            int countNotPriority = (countAll * startPercent) / 100;

            int distantionFind = PriorityDistantionFind;
            if (distantionFind == 0)
                distantionFind = 2;

            if(IsGC)
                System.GC.Collect();

            Vector3 pos;
            float scaling = SaveLoadData.Spacing;
            string nameObject = "";
            bool isUsePriority;
            int posX = 0;
            int posY = 0;
            string nameField= "";
            bool isCurrentGrass = false;
            ModelNPC.ObjectData objDataSave;
            //---- Gen Floor
            foreach (Vector3Int cellPos in colectionPosRnd)
            {
                posX = cellPos.x;
                posY = cellPos.y;
                //nameField = Helper.GetNameField(posX, posY);
                Helper.GetNameField_Cache(ref nameField, posX, posY);

                //Storage.EventsUI.SetTittle = String.Format("Loading {0} %", (countAll / (coutCreateObjects + 1)).ToString());
                pos.x = posX * scaling;
                pos.y = (posY * (-1)) * scaling;
                pos.z = -1;

                isUsePriority = coutCreateObjects > countNotPriority;

                if (isCurrentGrass) //Hard grass
                    prefabName = GenObjectWorld(GenObjectWorldMode.GrayGrass);
                else
                    prefabName = GenObjectWorld(GenObjectWorldMode.FloorGray);
                isCurrentGrass = !isCurrentGrass;

                if (isUsePriority)
                {
                    if (IsUseGenericTypeTerra) //ver 2. //GenericTypeTerraOnPriority_Cache v.3 (IsUseCache)
                        prefabName = GenericTypeTerraOnPriority(prefabName, pos, distantionFind);
                    else
                    {
                        //if(IsUseCache)
                            Helper.GenericOnPriorityByType_Cash(ref receivedGenTerra, prefabName, pos, distantionFind, TerraPriority, true);
                        //else
                        //    receivedGenTerra = Helper.GenericOnPriorityByType(prefabName, pos, distantionFind, TerraPriority, null, true);
                        prefabName = receivedGenTerra.TypePrefab;
                    }
                }
                //if(IsUseCache)
                //FIX$$ Helper.CreateName_Cache(ref nameObject, nameField, nameField, "-1");
                Helper.CreateName_Cache(ref nameObject, prefabName.ToString(), nameField, "-1");
                //else
                //    nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");

                objDataSave = BilderGameDataObjects.BildObjectData(prefabName);
                //objDataSave = BilderGameDataObjects.BildObjectData_Cash(prefabName);
                objDataSave.SetNameObject(nameObject);
                objDataSave.Position = pos;
                //Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "CreateDataGamesObjectsWorld");
                Storage.Data.AddFirstDataObjectInGrid(objDataSave, nameField);
            }

            if (IsTestGenNPC && PriorityPrefabPercent == 0)
                PriorityPrefabPercent = 20;

            if (IsGC)
                System.GC.Collect();

            //---- Gen Prefabs
            if (!IsGenOnlyFloor && PriorityPrefabPercent > 0)
            {
                int percentNPC = 0;
                GenObjectWorldMode modeGeneric = GenObjectWorldMode.PrefabsGray;

                //Valid
                if (PriorityPrefabPercent > 30)
                    PriorityPrefabPercent = 30;

                if (PriorityTreePercent == 0)
                    PriorityTreePercent = 50;
                if (PriorityRockPercent == 0)
                    PriorityRockPercent = 20;
                if (PriorityFlorePercent == 0)
                    PriorityFlorePercent = 30;

                if (isStartWorld || IsTestGenNPC)
                {
                    PriorityTreePercent = 40;
                    PriorityRockPercent = 20;
                    PriorityFlorePercent = 30;
                    percentNPC = 10;
                }

                int countPrefabs = (colectionPosRnd.Count() * PriorityPrefabPercent) / 100;
                int countTree = (countPrefabs * PriorityTreePercent) / 100;
                int countRock = (countPrefabs * PriorityRockPercent) / 100;
                countRock += countTree;
                int countFlore = (countPrefabs * PriorityFlorePercent) / 100;
                countFlore += countRock;
                int countNPC = (countPrefabs * percentNPC) / 100;
                countNPC += countFlore;
                int distantionFindPrefabs = 2;

                index = 0;
                foreach (Vector3Int cellPos in colectionPosRnd)
                {
                    //Storage.EventsUI.SetTittle = String.Format("Loading {0} %", (countAll / (coutCreateObjects + 1)).ToString());
                    posX = cellPos.x;
                    posY = cellPos.y;
                    //nameField = Helper.GetNameField(posX, posY);
                    //if (IsUseCache)
                        Helper.GetNameField_Cache(ref nameField, posX, posY);
                    //else
                    //    nameField = Helper.GetNameField(posX, posY);
                    pos.x = posX * scaling;
                    pos.y = (posY * (-1)) * scaling;
                    pos.z = -1;

                    if (index < countTree && countTree != 0)
                    {
                        //---- Gen Tree
                        modeGeneric = GenObjectWorldMode.TreeGray;
                    }
                    else if (index < countRock && countRock != 0)
                    {
                        //---- Gen Rock
                        modeGeneric = GenObjectWorldMode.RockGray;
                    }
                    else if (index < countFlore && countFlore != 0)
                    {
                        //---- Gen Flore
                        modeGeneric = GenObjectWorldMode.FloreGray;
                    }
                    else if (index < countNPC && countNPC != 0)
                    {
                        //---- Gen NPC
                        modeGeneric = GenObjectWorldMode.NPC;
                    }
                    else
                    {
                        //---- Default
                        modeGeneric = GenObjectWorldMode.PrefabsGray;
                    }

                    prefabName = GenObjectWorld(modeGeneric);
                    if (modeGeneric != GenObjectWorldMode.NPC)
                    {
                        //if (IsUseCache)
                            GenericTypeTerraOnPriority_Cache(prefabName, pos, distantionFindPrefabs, GenObjectWorldMode.PrefabsGray);
                        //else
                        //    prefabName = GenericTypeTerraOnPriority(prefabName, pos, distantionFindPrefabs, GenObjectWorldMode.PrefabsGray);
                    }

                    //if (IsUseCache)
                        Helper.CreateName_Cache(ref nameObject, prefabName.ToString(), nameField, "-1");
                    //else
                    //    nameObject = Helper.CreateName(prefabName.ToString(), nameField, "-1");

                    objDataSave = BilderGameDataObjects.BildObjectData(prefabName);
                    objDataSave.SetNameObject(nameObject);
                    objDataSave.Position = pos;
                    Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "GenericWorldPriorityTerra");
                    //Storage.Data.AddFirstDataObjectInGrid(objDataSave, nameField);
                    index++;
                    if (index > countPrefabs)
                        break;
                }
            }

            colectionPosRnd.Clear();
            colectionPosRnd = null;

            System.GC.Collect();


            if (isStartWorld)
                Storage.Data.SaveGridGameObjectsXml(true);

            Debug.Log("CreateDataGamesObjectsWorld IN Data World COUNT====" + coutCreateObjects);

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            float timeGens = (Time.time - testTime);
            Storage.EventsUI.SetTittle = String.Format("World is Loaded : " + ts.TotalMilliseconds);
            Storage.EventsUI.ListLogAdd = Storage.EventsUI.SetTittle;
            Storage.EventsUI.SetMessageBox = Storage.EventsUI.SetTittle;

            Storage.Instance.IsLoadingWorld = false;
            //---------------
        }
        catch (Exception ex)
        {
            Storage.EventsUI.ListLogAdd = "##### GenericCellWorld #" + indErr + "  " + ex.Message;
            Storage.Instance.IsLoadingWorld = false;
            return;
        }
    }

    private SaveLoadData.TypePrefabs GenericTypeTerraOnPriority(SaveLoadData.TypePrefabs typeObserver, Vector3 posRequested, int distantion, GenObjectWorldMode genModeFilter = GenObjectWorldMode.Standart)
    {
        
        int top = 2;

        string fieldName = Helper.GetNameFieldPosit(posRequested.x, posRequested.y);
        Vector2 posField = Helper.GetPositByField(fieldName);
        Vector2Int fieldPosit = new Vector2Int((int)posField.x, (int)posField.y);

        ReaderScene.DataInfoFinder finder = new ReaderScene.DataInfoFinder();
        Dictionary<string, ModelNPC.ObjectData> locationTypes = new Dictionary<string, ModelNPC.ObjectData>();

        int startX = fieldPosit.x - distantion;
        int startY = fieldPosit.y - distantion;
        int endX = fieldPosit.x + distantion;
        int endY = fieldPosit.y + distantion;

        if (startX < 1) startX = 1;
        if (startY < 1) startY = 1;

        int mePower = distantion * 5;
        finder.ResultPowerData.Add(typeObserver.ToString(), mePower); //me power

        List<SaveLoadData.TypePrefabs> listValidTypes = null;

        switch (genModeFilter)
        {
            case GenObjectWorldMode.TreeGray:
                listValidTypes = Storage.GridData.GetTreeGray;
                break;
            case GenObjectWorldMode.RockGray:
                listValidTypes = Storage.GridData.GetRockGray;
                break;
            case GenObjectWorldMode.FloreGray:
                listValidTypes = Storage.GridData.GetFloreGray;
                break;
            case GenObjectWorldMode.PrefabsGray:
                listValidTypes = Storage.GridData.GetPrefabsObjectsAndFloreGray;
                break;
            default:
                break;
        }

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                string field = Helper.GetNameField(x, y);
                List<ModelNPC.ObjectData> objects = ReaderScene.GetObjectsDataFromGridTest(field); //TEST
                foreach (ModelNPC.ObjectData objData in objects)
                {
                    string typeModel = objData.TypePrefab.ToString();
                    if(listValidTypes != null)
                    {
                        if (!listValidTypes.Contains(objData.TypePrefab))
                            continue;
                    }

                    int power = Storage.Person.GetPriorityPowerByJoin(typeObserver, objData.TypePrefab);
                    int powerDist = (distantion - Math.Max(Math.Abs(fieldPosit.x - x), Math.Abs(fieldPosit.y - y))) * 3;
                    power += powerDist;
                    if (!finder.ResultPowerData.ContainsKey(typeModel))
                        finder.ResultPowerData.Add(typeModel, power);
                    else
                        finder.ResultPowerData[typeModel] += power;
                }
            }
        }

        string selType = typeObserver.ToString();
        SaveLoadData.TypePrefabs resultType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), selType); 
        if (finder.ResultPowerData.Count > 0)
        {
            List<string> listTopTypes = new List<string>();
            foreach (var item in finder.ResultPowerData.OrderByDescending(p => p.Value))
            {
                listTopTypes.Add(item.Key);
                if (listTopTypes.Count > top) //top
                    break;
            }
            if (listTopTypes.Count() > 0)
            {
                int indRnd = UnityEngine.Random.Range(0, listTopTypes.Count() - 1);
                selType = listTopTypes[indRnd];
            }
            if(resultType.ToString() != selType)
                resultType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), selType);
        }
        return resultType;
    }

    private GenTerraCashe m_cacheGen = new GenTerraCashe();

    private void GenericTypeTerraOnPriority_Cache(SaveLoadData.TypePrefabs typeObserver, Vector3 posRequested, int distantion, GenObjectWorldMode genModeFilter = GenObjectWorldMode.Standart)
    {
        m_cacheGen.SetName(posRequested, distantion);
        m_cacheGen.Finder.ResultPowerData.Add(typeObserver.ToString(), m_cacheGen.MePower); //me power
        m_cacheGen.ListValidTypes = null;

        switch (genModeFilter)
        {
            case GenObjectWorldMode.TreeGray:
                m_cacheGen.ListValidTypes = Storage.GridData.GetTreeGray;
                break;
            case GenObjectWorldMode.RockGray:
                m_cacheGen.ListValidTypes = Storage.GridData.GetRockGray;
                break;
            case GenObjectWorldMode.FloreGray:
                m_cacheGen.ListValidTypes = Storage.GridData.GetFloreGray;
                break;
            case GenObjectWorldMode.PrefabsGray:
                m_cacheGen.ListValidTypes = Storage.GridData.GetPrefabsObjectsAndFloreGray;
                break;
            default:
                break;
        }

        for (int y = m_cacheGen.StartY; y < m_cacheGen.EndY; y++)
        {
            for (int x = m_cacheGen.StartX; x < m_cacheGen.EndX; x++)
            {
                //m_cacheGen.Field = Helper.GetNameField(x, y);
                Helper.GetNameField_Cache(ref m_cacheGen.Field, x, y);
                foreach (ModelNPC.ObjectData objData in m_cacheGen.GetObjects)
                {
                    m_cacheGen.TypeModel = objData.TypePrefab.ToString();
                    if (m_cacheGen.ListValidTypes != null)
                    {
                        if (!m_cacheGen.ListValidTypes.Contains(objData.TypePrefab))
                            continue;
                    }

                    int power = Storage.Person.GetPriorityPowerByJoin(typeObserver, objData.TypePrefab);
                    m_cacheGen.PowerDistSet(x, y, power);
                }
            }
        }

        m_cacheGen.SelType = typeObserver;
        m_cacheGen.ResultType = m_cacheGen.SelType;
        if (m_cacheGen.Finder.ResultPowerData.Count > 0)
        {
            List<string> listTopTypes = new List<string>();
            foreach (var item in m_cacheGen.Finder.ResultPowerData.OrderByDescending(p => p.Value))
            {
                listTopTypes.Add(item.Key);
                if (listTopTypes.Count > m_cacheGen.Top) //top
                    break;
            }
            if (listTopTypes.Count() > 0)
            {
                int indRnd = UnityEngine.Random.Range(0, listTopTypes.Count() - 1);
                m_cacheGen.SelType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), listTopTypes[indRnd]);
            }
            m_cacheGen.CheckResult();
        }
        typeObserver = m_cacheGen.ResultType;
    }

    public class GenTerraCashe
    {
        public int Top = 2;

        public string FieldName;
        public Vector2 PosField;
        public Vector2Int FieldPosit;

        public ReaderScene.DataInfoFinder Finder = new ReaderScene.DataInfoFinder();
        public Dictionary<string, ModelNPC.ObjectData> LocationTypes = new Dictionary<string, ModelNPC.ObjectData>();
        public List<SaveLoadData.TypePrefabs> ListValidTypes = new List<SaveLoadData.TypePrefabs>();
        public List<string> TistTopTypes = new List<string>();

        public int StartX;
        public int StartY;
        public int EndX;
        public int EndY;

        public int MePower;
        public string Field;
        public string TypeModel;

        public int PowerDist;
        public SaveLoadData.TypePrefabs SelType;
        public SaveLoadData.TypePrefabs ResultType;
        
        public int IndRnd;
        private int m_distantion;

        public void SetName(Vector3 posRequested, int p_distantion)
        {
            //Power = 0;
            m_distantion = p_distantion;
            Finder.ResultPowerData.Clear();
            LocationTypes.Clear();
            ListValidTypes.Clear();
            TistTopTypes.Clear();

            FieldName = Helper.GetNameFieldPosit(posRequested.x, posRequested.y);
            PosField = Helper.GetPositByField(FieldName);
            FieldPosit = new Vector2Int((int)PosField.x, (int)PosField.y);

            StartX = FieldPosit.x - m_distantion;
            StartY = FieldPosit.y - m_distantion;
            EndX = FieldPosit.x + m_distantion;
            EndY = FieldPosit.y + m_distantion;

            if (StartX < 1) StartX = 1;
            if (StartY < 1) StartY = 1;

            MePower = m_distantion * 5;
        }

        public List<ModelNPC.ObjectData> GetObjects
        {
            get
            {
                return ReaderScene.GetObjectsDataFromGridTest(Field);
            }
        }

        private void CalculatePower(int power)
        {
            if (!Finder.ResultPowerData.ContainsKey(TypeModel))
                Finder.ResultPowerData.Add(TypeModel, power);
            else
                Finder.ResultPowerData[TypeModel] += power;
        }

        private int m_power;
        public void PowerDistSet(int x, int y, int p_power)
        {
            PowerDist = (m_distantion - Math.Max(Math.Abs(FieldPosit.x - x), Math.Abs(FieldPosit.y - y))) * 3;
            CalculatePower(p_power + PowerDist);
        }

        public void CheckResult()
        {
            if (ResultType != SelType)
                ResultType = SelType;
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

    public string[] GenericPortal(int count, SaveLoadData.TypePrefabs typePortal = SaveLoadData.TypePrefabs.PrefabPortal)
    {
        List<string> portalsId = new List<string>();
        Vector3 pos = new Vector3();
        bool isRndTypePortal = typePortal == SaveLoadData.TypePrefabs.PrefabPortal;
        float scaling = SaveLoadData.Spacing;
        string nameObject = "";
        Dictionary<int, SaveLoadData.TypePrefabs> listPortals = new Dictionary<int, SaveLoadData.TypePrefabs>
        {
            { 0, SaveLoadData.TypePrefabs.PortalBlue },
            { 1, SaveLoadData.TypePrefabs.PortalRed },
            { 2, SaveLoadData.TypePrefabs.PortalGreen },
            { 3, SaveLoadData.TypePrefabs.PortalViolet },
        };
        int maxPortal = listPortals.Values.Count();
        string nameField = "";
        ModelNPC.ObjectData objDataSave;
        int maxRnd = Helper.HeightLevel;
        int padding = 15;
        int posX = 0;
        int posY = 0;
        int indPort = 0;

        foreach (int ind in Enumerable.Range(0, count))
        {
            posX = Random.Range(padding, maxRnd - padding);
            posY = Random.Range(padding, maxRnd - padding);
            if (isRndTypePortal)
            {
                indPort = Random.Range(0, maxPortal);
                typePortal = listPortals[indPort];
            }
            Helper.GetNameField_Cache(ref nameField, posX, posY);
            pos.x = posX * scaling;
            pos.y = (posY * (-1)) * scaling;
            pos.z = -1;

            Helper.CreateName_Cache(ref nameObject, typePortal.ToString(), nameField, "-1");
            objDataSave = BilderGameDataObjects.BildObjectData(typePortal);
            objDataSave.SetNameObject(nameObject);
            objDataSave.Position = pos;
            Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "GenericPortal");
            portalsId.Add(objDataSave.Id);
        }

        return portalsId.ToArray();
    }

    public void GenericPrefab(SaveLoadData.TypePrefabs typePrefab, Vector3 pos)
    {
        string nameField = "";
        ModelNPC.ObjectData objDataSave;
        int posX = (int)pos.x;
        int posY = (int)pos.y;
        string nameObject = "";
        Helper.GetNameField_Cache(ref nameField, posX, posY);
        pos.x = posX * SaveLoadData.Spacing; ;
        pos.y = (posY * (-1)) * SaveLoadData.Spacing; 
        pos.z = -1;

        Helper.CreateName_Cache(ref nameObject, typePrefab.ToString(), nameField, "-1");
        objDataSave = BilderGameDataObjects.BildObjectData(typePrefab);
        objDataSave.SetNameObject(nameObject);
        objDataSave.Position = pos;
        Storage.Data.AddDataObjectInGrid(objDataSave, nameField, "GenericPrefab");
    }
}

