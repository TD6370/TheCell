using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Helper { //: MonoBehaviour {

    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public static int LayerTerra = 4;
    public static int LayerFloor = 7;
    public static int LayerDark = 22;
    public static int LayerDefault = 0;
    public static string LayerTerraName = "LayerTerra";
    public static string LayerFloorName = "LayerFloor";

    public static bool IsBigWorld = true;// false; //

    public static float SpeedTimer = 0f;// Time.time

    //private float  = 100f;


    public static int WidthZona
    {
        get { return 100; }
    }
    public static int HeightZona
    {
        get { return 100; }
    }

    public static int WidthLevel
    {
        get {
            if (IsBigWorld)
                return WidthWorld;
            else
                return WidthZona;
        }
    }
    public static int HeightLevel
    {
        get {
            if (IsBigWorld)
                return HeightWorld;
            else
                return HeightZona;
        }
    }
    public static int WidthWorld
    {
        get { return 300; }
    }
    public static int HeightWorld
    {
        get { return 300; }
    }

    public static int SpeedWorld
    {
        get
        {
            return WidthLevel / 100;
        }
    }

    public static int SizeWorldOffSet
    {
        get
        {
            return WidthLevel-100;
        }
    }

    public static int SizeBigCollider
    {
        get { return 25 * SpeedWorld; }
    }

    public static int SizePart
    {
        get { return 3; }
    }
    #region Helper

    private static string FieldKey{
        get{
            return Storage.FieldKey;
        }
    }

    public static string CreateName(string tag, string nameFiled, string id = "", string nameObjOld = "")
    {
        if (string.IsNullOrEmpty(id))
        {
            if (string.IsNullOrEmpty(nameObjOld))
            {
                Debug.Log("!!!!!! Error create name !!!!!!!!!!");
            }
            else
            {

                int i = nameObjOld.LastIndexOf("_");
                //int i2 = nameObjOld.IndexOf("_");
                if (i != -1)
                {
                    //123456789
                    //Debug.Log("_______________________CREATE NAME i_l=" + i + "     i=" + i2 + "     len=" + nameObjOld.Length + "      :" + nameObjOld);
                    id = nameObjOld.Substring(i + 1, nameObjOld.Length - i - 1);
                    //id = nameObjOld.Substring(nameObjOld.Length - i, i);
                    //Debug.Log("_______________________CREATE NAME  ID:" + id + "       nameObjOld: " + nameObjOld);
                }
                else
                    Debug.Log("!!!!!! Error create name prefix !!!!!!!!!!");
            }
        }
        else
        {
            //Debug.Log("__CreateName old id=" + id);
        }

        if (id == "-1")
        {
            id = Guid.NewGuid().ToString().Substring(1, 7);
        }

        return tag + "_" + nameFiled + "_" + id;
    }

    public static void CreateName_Cache(ref string result, string tag, string nameFiled, string id = "", string nameObjOld = "")
    {
        if (string.IsNullOrEmpty(id))
        {
            if (string.IsNullOrEmpty(nameObjOld))
            {
                Debug.Log("!!!!!! Error create name !!!!!!!!!!");
            }
            else
            {
                int i = nameObjOld.LastIndexOf("_");
                if (i != -1)
                {
                    id = nameObjOld.Substring(i + 1, nameObjOld.Length - i - 1);
                }
            }
        }
        if (id == "-1") {
            id = Guid.NewGuid().ToString().Substring(1, 7);
        }

        //StringBuilder sb = new StringBuilder("ABC", 50);
        // Append three characters (D, E, and F) to the end of the StringBuilder.
        //sb.Append(new char[] { 'D', 'E', 'F' });
        result = tag + "_" + nameFiled + "_" + id;
    }

    public static string GetGameObjectID(GameObject gobj)
    {
        string nameObj = gobj.name;
        return GetID(nameObj);
    }

    public static string GetID(string nameObj)
    {
        string id = "";
        int i = nameObj.LastIndexOf("_");
        //int i2 = nameObjOld.IndexOf("_");
        if (i != -1)
        {
            //123456789
            //Debug.Log("_______________________CREATE NAME i_l=" + i + "     i=" + i2 + "     len=" + nameObjOld.Length + "      :" + nameObjOld);
            id = nameObj.Substring(i + 1, nameObj.Length - i - 1);
            //Debug.Log("_______________________GetGameObjectID  ID:" + id);
        }
        else
        {
            if (nameObj != StoragePerson._Boss &&
                nameObj != StoragePerson._Ufo)
            {
                Debug.Log("!!!!!! GetID Error  on " + nameObj + " !!!!!!!!!!");
            }
        }

        return id;
    }

    public static string GetTag(string nameObj)
    {
        string tag = "";
        int i = nameObj.IndexOf("_");
        if (i != -1)
        {
            tag = nameObj.Substring(0, i);
        }
        else
            Debug.Log("!!!!!! GetTag Error !!!!!!!!!!");

        Debug.Log("----- GetTag (" + nameObj + ") : " + tag);
        return tag;
    }

    public static string GetNameField(int x, int y)
    {
        return FieldKey + x + "x" + Mathf.Abs(y);
    }

    public static void GetNameField_Cache(ref string result, int x, int y)
    {
        result = FieldKey + x + "x" + Mathf.Abs(y);
    }

    public static string GetNameField(Vector2 pos)
    {
        return FieldKey + (int)pos.x + "x" + Mathf.Abs((int)pos.y);
    }


    public static string GetNameField(System.Single x, System.Single y)
    {
        return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    }

    public static string GetNameFieldPosit(int x, int y)
    {
        x = (int)(x / Storage.ScaleWorld);
        y = (int)(y / Storage.ScaleWorld);
        return FieldKey + x + "x" + Mathf.Abs(y);
    }

    public static string GetNameFieldPosit(System.Single x, System.Single y)
    {
        x = (int)(x / Storage.ScaleWorld);
        y = (int)(y / Storage.ScaleWorld);
        return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    }

    public static void GetFieldPositByWorldPosit(ref int xOut, ref int yOut,  System.Single xIn, System.Single yIn)
    {
        xOut = (int)(xIn / Storage.ScaleWorld);
        yOut = (int)Mathf.Abs(yIn / Storage.ScaleWorld);
        //return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    }

    public static string GetNameFieldObject(GameObject gobj)
    {
        System.Single x = gobj.transform.position.x;
        System.Single y = gobj.transform.position.y;
        return GetNameFieldPosit(x, y);
    }

    public static SaveLoadData.TypePrefabs GetTypePrefab(GameObject gobj)
    {
        SaveLoadData.TypePrefabs prefabType = SaveLoadData.TypePrefabs.PrefabField;

        if (gobj == null)
            return SaveLoadData.TypePrefabs.PrefabField;
        if (gobj.tag == null)
            return SaveLoadData.TypePrefabs.PrefabField;
        if(string.IsNullOrEmpty(gobj.tag.ToString()))
            return SaveLoadData.TypePrefabs.PrefabField;
        if (gobj.tag.ToString().IndexOf("Prefab")==-1)
            return SaveLoadData.TypePrefabs.PrefabField;

        try
        {
            prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), gobj.tag.ToString());
        }catch(Exception x)
        {
            Debug.Log("##############SaveLoadData.TypePrefabs GetTypePrefab : " + x.Message);
        }
        return prefabType;
    }

    //public static bool IsTerra(SaveLoadData.TypePrefabs typePrefab)
    //{
    //    bool isTerra = Enum.IsDefined(typeof(SaveLoadData.TypePrefabObjects), typePrefab.ToString());
    //    //bool isTerra = IsTypeTypePrefabObjectsContains(typePrefab.ToString());
    //    return isTerra;

    //    //switch(typePrefab)
    //    //{
    //    //    case SaveLoadData.TypePrefabs.PrefabRock:
    //    //    case SaveLoadData.TypePrefabs.PrefabVood:
    //    //    case SaveLoadData.TypePrefabs.PrefabElka:
    //    //    case SaveLoadData.TypePrefabs.PrefabWallRock:
    //    //    case SaveLoadData.TypePrefabs.PrefabWallWood:
    //    //        return true;
    //    //}
    //    //return false;
    //}

    //public static bool IsTerra(this string typePrefabStr)
    //{
    //    try
    //    {
    //        SaveLoadData.TypePrefabs typePrefab = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), typePrefabStr);
    //        return IsTerra(typePrefab);
    //    }catch(Exception x)
    //    {
    //        Debug.Log("IsTerra " + x.Message);
    //        return false;
    //    }
    //}
        
    public static bool IsUpdateTexture(SaveLoadData.TypePrefabs typePrefab)
    {
        switch (typePrefab)
        {
            case SaveLoadData.TypePrefabs.PrefabElka:
            case SaveLoadData.TypePrefabs.PrefabVood:
            //case SaveLoadData.TypePrefabs.PrefabField:
                return true;
        }
        return false;
    }

    public static SaveLoadData.TypePrefabs ParsePrefab(string strType)
    {
        if (IsTypePrefabs(strType))
        {
            var prefabType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), strType);
            return prefabType;
        }
        else
        {
            Debug.Log("########## GetTypePrefab [" + strType + "] Not TypePrefabs");
            return SaveLoadData.TypePrefabs.PrefabField;
        }
    }


    public static bool IsTerraAlpha(SaveLoadData.TypePrefabs typePrefab)
    {
        switch (typePrefab)
        {
            case SaveLoadData.TypePrefabs.PrefabVood:
            case SaveLoadData.TypePrefabs.PrefabElka:
            case SaveLoadData.TypePrefabs.PrefabWallRock:
            case SaveLoadData.TypePrefabs.PrefabWallWood:
                return true;
        }
        return false;
    }

    public static bool IsTerraAlpha(PoolGameObjects.TypePoolPrefabs typePrefab)
    {
        switch (typePrefab)
        {
            //case PoolGameObjects.TypePoolPrefabs.PoolFlore:
            case PoolGameObjects.TypePoolPrefabs.PoolWall:
            case PoolGameObjects.TypePoolPrefabs.PoolWood:
                return true;
        }
        return false;
    }

    public static bool IsTerraLayer(PoolGameObjects.TypePoolPrefabs typePrefab)
    {
        switch (typePrefab)
        {
            case PoolGameObjects.TypePoolPrefabs.PoolFlore:
            case PoolGameObjects.TypePoolPrefabs.PoolWall:
            case PoolGameObjects.TypePoolPrefabs.PoolWood:
                return true;
        }
        return false;
    }

    public static Vector2 NormalizPosToField(System.Single x, System.Single y)
    {
        x = (int)(x / Storage.ScaleWorld);
        y = (int)(y / Storage.ScaleWorld);
        return new Vector2(x, Mathf.Abs((int)y));
    }

    public static Vector2 NormalizFieldToPos(Vector2 posit)
    {
        return NormalizFieldToPos(posit.x, posit.y);
    }

    public static Vector2 NormalizFieldToPos(System.Single x, System.Single y)
    {
        x = (int)(x * Storage.ScaleWorld);
        y = (int)(y * Storage.ScaleWorld);
        y *= -1;
        return new Vector2(x, y);
    }

    public static void GetPositByField_Cache(ref Vector2Int result, string nameFiled)
    {
        if (string.IsNullOrEmpty(nameFiled))
        {
            Debug.Log("########## Error GetPositByField nameFiled is Empty");
            return;
        }

        string strPos = nameFiled.Replace(FieldKey, "");
        string[] masPos = strPos.Split('x');
        float x;
        float y;
        if (masPos.Length < 2)
        {
            Debug.Log("########## Error GetPositByField --  : " + nameFiled);
            return;
        }
        if (!float.TryParse(masPos[0], out x))
        {
            Debug.Log("########## Error GetPositByField -- x : " + nameFiled);
            return;
        }
        if (!float.TryParse(masPos[1], out y))
        {
            Debug.Log("########## Error GetPositByField -- y : " + nameFiled);
            return;
        }

        //Debug.Log("----- GetPositByField (" + nameFiled + ") : " + x + "x" + y);
        // return new Vector2(x, y);
        result.x = (int)x;
        result.y = (int)y;
    }


    public static Vector2 GetPositByField(string nameFiled)
    {
        if(string.IsNullOrEmpty(nameFiled))
        {
            Debug.Log("########## Error GetPositByField nameFiled is Empty");
            return new Vector2(0, 0);
        }

        string strPos = nameFiled.Replace(FieldKey, "");
        string[] masPos = strPos.Split('x');
        float x;
        float y;
        if (masPos.Length < 2)
        {
            Debug.Log("########## Error GetPositByField --  : " + nameFiled);
            return new Vector2(0, 0);
        }
        if (!float.TryParse(masPos[0], out x))
        {
            Debug.Log("########## Error GetPositByField -- x : " + nameFiled);
            return new Vector2(0, 0);
        }
        //else
        //{
        //    Debug.Log("------ GetPositByField -- X = " + x);
        //}


        if (!float.TryParse(masPos[1], out y))
        {
            Debug.Log("########## Error GetPositByField -- y : " + nameFiled);
            return new Vector2(0, 0);
        }
        //else
        //{
        //    Debug.Log("------ GetPositByField -- Y = " + y);
        //}


        //Debug.Log("----- GetPositByField (" + nameFiled + ") : " + x + "x" + y);
        return new Vector2(x, y);
    }

    public static string GetNameFieldByName(string nameGameObject)
    {
        
        int start = nameGameObject.LastIndexOf(FieldKey);
        string result = "";
        string resultInfo = "";
        int valid = 0;
        if (start == -1)
        {
            Debug.Log("# GetNameFieldByName " + nameGameObject + " key 'Field' not found!");
            //return nameGameObject;
            return null;
        }
        start += "Field".Length;

        //int i = nameGameObject.IndexOf("Field");
        for (int i = start; i < nameGameObject.Length - 1; i++)
        {
            var symb = nameGameObject[i];
            resultInfo += symb;
            if (symb == 'x')
            {
                result += symb.ToString();
                continue;
            }
            if (Int32.TryParse(symb.ToString(), out valid))
            {
                result += valid.ToString();
                continue;
            }
            break;
        }
        result = FieldKey + result;
        //Debug.Log("# GetNameFieldByName " + nameGameObject + " >> " + result + "     text: " + resultInfo + "   start=" + start);
        return result;
    }

    public static Vector3 ConvVector3(Vector3 copyPos)
    {
        return new Vector3(copyPos.x, copyPos.y, copyPos.z);
    }

    public static bool IsDataInit(GameObject gameObject)
    {
        int start = gameObject.name.IndexOf(FieldKey);
        if (start == -1)
        {
            //Debug.Log("# IsDataInit " + gameObject.name + " key 'Field' not found!");
            //yield return null;
            return false;
        }
        return true;
    }



    public static bool IsValidPiontInZona(float x, float y)
    {
        bool result = true;

        if (x < Storage.Instance.ZonaReal.X)
            return false;
        if (y > Storage.Instance.ZonaReal.Y) //*-1
            return false;
        if (x > Storage.Instance.ZonaReal.X2)
            return false;
        if (y < Storage.Instance.ZonaReal.Y2) //*-1
            return false;
        return result;
    }

    public static bool IsValidPiontInZonaCorr(float x, float y)
    {
        bool result = true;
        int corr = 2;

        if (x + corr < Storage.Instance.ZonaReal.X)
            return false;
        if (y - corr > Storage.Instance.ZonaReal.Y) //*-1
            return false;
        if (x - corr > Storage.Instance.ZonaReal.X2)
            return false;
        if (y + corr < Storage.Instance.ZonaReal.Y2) //*-1
            return false;
        return result;
    }

    public static bool IsValidObjectNameInZona(string fieldNameObj)
    {
        string fieldName = GetNameFieldByName(fieldNameObj);
        return IsValidFieldInZona(fieldName);
    }

    public static bool IsValidFieldInZona(string fieldName)
    {
        bool result = true;
        Vector2 posFieldObject = GetPositByField(fieldName);
        int x = (int)posFieldObject.x;
        int y = (int)posFieldObject.y;
        //int corrX = Storage.Instance.LimitHorizontalLook /2;
        //int corrY = Storage.Instance.LimitVerticalLook / 2;
        int corrX = 20;
        int corrY = 20;
        int hx = Storage.Player.HeroPositionX;
        int hy = Storage.Player.HeroPositionY;
        int limitX1 = hx - corrX;
        int limitX2 = hx + corrX;
        int limitY1 = hy - corrY;
        int limitY2 = hy + corrY;

        if (x < limitX1)
            return false;
        if (x > limitX2) 
            return false;
        if (y < limitY1)
            return false;
        if (y > limitY2)
            return false;

        return result;
    }

    public static bool IsValidFieldInZona(float checkX, float checkY)
    {
        bool result = true;
        //string field = Helper.GetNameField(checkX, checkY);
        var posField = Helper.NormalizFieldToPos(checkX, checkY);
        float x = posField.x;
        float y = posField.y;

        if (x < Storage.Instance.ZonaReal.X)
            return false;
        if (y > Storage.Instance.ZonaReal.Y) //*-1
            return false;
        if (x > Storage.Instance.ZonaReal.X2)
            return false;
        if (y < Storage.Instance.ZonaReal.Y2) //*-1
            return false;
        return result;
    }

    #endregion



    #region Gen Grid Field

    public static void InitRange(int p_PosHero, int p_limitLook, int gridSize, out int p_startPos, out int limit)
    {
        int maxSize = p_limitLook + 1;
        p_startPos = p_PosHero - (p_limitLook / 2);

        if (p_startPos < 0)
            p_startPos = 0;

        limit = p_startPos + maxSize;
        if (limit > gridSize)
            limit = gridSize;
    }


    public static bool ValidateAddedY(Vector2 _movement, int gridHeight, int TopY, int DownY)
    {
        if (TopY < 0 && _movement.y > 0)
            return false;
        if (DownY > gridHeight && _movement.y < 0)
            return false;

        return true;
    }


    public static bool ValidateAddedX(Vector2 _movement, int gridWidth, int LeftX, int RightX)
    {
        if (RightX > gridWidth && _movement.x > 0)
            return false;

        if (LeftX < 0 && _movement.x < 0)
            return false;

        return true;
    }


    public static bool ValidateRemoveY(Vector2 _movement, int gridHeight, int TopRemoveY, int DownRemoveY)
    {
        if (TopRemoveY < 0 && _movement.y < 0)
            return false;

        if (DownRemoveY > gridHeight && _movement.y > 0)
            return false;

        return true;
    }


    public static bool ValidateRemoveX(Vector2 _movement, int gridWidth, int LeftRemoveX, int RightRemoveX)
    {
        if (LeftRemoveX < 0 && _movement.x > 0)
            return false;

        if (RightRemoveX > gridWidth && _movement.x < 0)
            return false;

        return true;
    }

    public static Vector2 ValidPiontInZona(ref float x, ref float y, float offset = 0)
    {
        offset = Mathf.Abs(offset);

        if (x < Storage.Instance.ZonaReal.X)
            x = Storage.Instance.ZonaReal.X + offset;
        if (y > Storage.Instance.ZonaReal.Y) //*-1
            y = Storage.Instance.ZonaReal.Y - offset;
        if (x > Storage.Instance.ZonaReal.X2)
            x = Storage.Instance.ZonaReal.X2 - offset;
        if (y < Storage.Instance.ZonaReal.Y2) //*-1
            y = Storage.Instance.ZonaReal.Y + offset;
        Vector2 result = new Vector2(x, y);
        return result;
    }

    public static Vector2 ValidPiontInZonaWorld(ref float x, ref float y, float offset = 0)
    {
        offset = Mathf.Abs(offset);

        if (x < 1)
            x = 1 + Math.Abs(offset);
        //if (y > -1) //*-1
            //y = offset - Math.Abs(offset);
        if (y > -1) // #fix validate
            y = (1 + Math.Abs(offset)) *(-1);
        if (x > Helper.WidthLevel * Storage.ScaleWorld)
            x = (Helper.WidthLevel * Storage.ScaleWorld) - Math.Abs(offset);
        if (y < (Helper.HeightLevel * Storage.ScaleWorld) * (-1)) //*-1
            y = ((Helper.HeightLevel * Storage.ScaleWorld) - Math.Abs(offset)) * (-1);
        Vector2 result = new Vector2(x, y);
        return result;
    }

    public static byte[] StringToUTF8ByteArray(string pXmlString)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }


    #endregion

    #region Profiler

    public static void StartControlTime()
    {
        Helper.SpeedTimer = Time.time;
    }

    public static void StopControlTime(string info)
    {
        Storage.EventsUI.ListLogAdd = info + (Time.time - Helper.SpeedTimer);
    }

    #endregion

    //fixed System.Enum.IsDefined
    public static bool IsTypePrefabs(string strType)
    {
        return Enum.GetNames(typeof(SaveLoadData.TypePrefabs)).Any(x => x.ToLower() == strType.ToLower());
    }

    public static bool IsTypePrefabNPC(string strType)
    {
        return Enum.GetNames(typeof(SaveLoadData.TypePrefabNPC)).Any(x => x.ToLower() == strType.ToLower());
    }

    //public static bool IsTypeTypePrefabObjects(string strType)
    //{
    //    return Enum.GetNames(typeof(SaveLoadData.TypePrefabObjects)).Any(x => x.ToLower() == strType.ToLower());
    //}

    #region Priority utility

     public static ModelNPC.ObjectData GenericOnPriorityByType(SaveLoadData.TypePrefabs typeRequested, Vector3 posRequested, int distantionFind, Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> p_prioritys, Action p_actionLoadPriority, bool isFoor)
    {
        if (p_prioritys == null)
        {
            Storage.EventsUI.ListLogAdd = "### GenericTerraOnPriority >> PersonPriority is null";
            if (p_actionLoadPriority != null)
                p_actionLoadPriority();
            return null;
        }
        
        ModelNPC.ObjectData result = new ModelNPC.ObjectData();
        result = FindFromLocationType(typeRequested, posRequested, distantionFind, isFoor);
        return result;
    }

    public static void GenericOnPriorityByType_Cash(ref ModelNPC.ObjectData result, SaveLoadData.TypePrefabs typeRequested, Vector3 posRequested, int distantionFind, Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> p_prioritys, bool isFoor)
    {
        FindFromLocationType_Cache(ref result, typeRequested, posRequested, distantionFind, isFoor);
    }
     

    public static ModelNPC.ObjectData FindFromLocationType(SaveLoadData.TypePrefabs typeRequested, Vector3 posRequested, int distantion, bool isFoor)
    {
        GetFieldPositByWorldPosit(ref posFieldInt_FindFromLocation_X, ref posFieldInt_FindFromLocation_Y, posRequested.x, posRequested.y);
        ReaderScene.DataInfoFinder finder = ReaderScene.GetDataInfoLocation(posFieldInt_FindFromLocation_X, posFieldInt_FindFromLocation_Y, distantion, string.Empty, typeRequested, string.Empty, isFoor);
        return finder.ResultData;
    }

    public static void FindFromLocationType_Cache(ref ModelNPC.ObjectData result, SaveLoadData.TypePrefabs typeRequested, Vector3 posRequested, int distantion, bool isFoor)
    {
        GetFieldPositByWorldPosit(ref posFieldInt_FindFromLocation_X, ref posFieldInt_FindFromLocation_Y, posRequested.x, posRequested.y);
        result = ReaderScene.GetDataInfoLocation(posFieldInt_FindFromLocation_X, posFieldInt_FindFromLocation_Y, distantion, string.Empty, typeRequested, string.Empty, isFoor).ResultData;
    }

    /*
    public static ModelNPC.ObjectData FindFromLocation(ModelNPC.ObjectData data, int distantion)
    {
        //Vector2 Position = data.Position;
        string id_Observer = data.Id;
        ModelNPC.GameDataAlien dataAlien = data as ModelNPC.GameDataAlien;
        string id_PrevousTarget = dataAlien == null ? string.Empty : dataAlien.PrevousTargetID;
        //if (id_PrevousTarget == string.Empty)
        //    Storage.EventsUI.ListLogAdd = "######## FindFromLocation id_PrevousTarget == string.Empty >> " + data.NameObject;
        if (id_PrevousTarget != string.Empty)
            Storage.EventsUI.ListLogAdd = "!!!!!!!! FindFromLocation id_PrevousTarget >> " + data.NameObject + " == " + id_PrevousTarget;


        SaveLoadData.TypePrefabs typeObserver = data.TypePrefab;

        string fieldName = Helper.GetNameFieldPosit(data.Position.x, data.Position.y);
        Vector2 posField = Helper.GetPositByField(fieldName);
        Vector2Int posFieldInt = new Vector2Int((int)posField.x, (int)posField.y);

        ReaderScene.DataInfoFinder finder = ReaderScene.GetDataInfoLocation(posFieldInt.x, posFieldInt.y, distantion, id_Observer, typeObserver, id_PrevousTarget, false);
        return finder.ResultData;
    }
    */

    //FindFromLocation_Cache

    //private static Vector2Int posFieldInt_FindFromLocation = new Vector2Int();
    private static int posFieldInt_FindFromLocation_X;
    private static int posFieldInt_FindFromLocation_Y;
    private static ModelNPC.GameDataAlien dataAlien_FindFromLocation_Cache;
    public static void FindFromLocation_Cache(ref ModelNPC.ObjectData result, ModelNPC.ObjectData data, int distantion)
    {
        dataAlien_FindFromLocation_Cache = data as ModelNPC.GameDataAlien;
        GetFieldPositByWorldPosit(ref posFieldInt_FindFromLocation_X, ref posFieldInt_FindFromLocation_Y, data.Position.x, data.Position.y);
        result = ReaderScene.GetDataInfoLocation(posFieldInt_FindFromLocation_X, posFieldInt_FindFromLocation_Y, distantion, data.Id, data.TypePrefab, dataAlien_FindFromLocation_Cache.PrevousTargetID, false).ResultData;
    }

    public static int GetPriorityPower(string id, PriorityFinder priority)
    {
        if (Storage.ReaderWorld.CollectionInfoID.ContainsKey(id))
        {
            Debug.Log("######### Error GetPrioriryPower=" + id);
        }

        int power = 0;

        var objData = Storage.ReaderWorld.CollectionInfoID[id].Data;
        return GetPriorityPower(objData, priority);
    }

    public static Dictionary<string, int> FillPrioritys(Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> pioritys)
    {
        var max = Enum.GetValues(typeof(SaveLoadData.TypePrefabs)).Length - 1;

        var _CollectionPowerAllTypes = new Dictionary<string, int>();

        SaveLoadData.TypePrefabs prefabNameType;
        SaveLoadData.TypePrefabs prefabNameTypeTarget;
        for (int ind = 0; ind < max; ind++)
        {
            prefabNameType = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), ind.ToString());
            //if (Helper.IsTypePrefabNPC(prefabNameType.ToString()))
            if (pioritys.ContainsKey(prefabNameType))
            {
                ModelNPC.ObjectData objData = BilderGameDataObjects.BildObjectData(prefabNameType.ToString());
                PriorityFinder prioritys = pioritys[prefabNameType];
                for (int indT = 0; indT < max; indT++)
                {
                    prefabNameTypeTarget = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), indT.ToString());
                    ModelNPC.ObjectData objDataTarget = BilderGameDataObjects.BildObjectData(prefabNameTypeTarget.ToString());
                    int power = GetPriorityPower(objDataTarget, prioritys);
                    string keyJoinNPC = prefabNameType + "_" + prefabNameTypeTarget;
                    _CollectionPowerAllTypes.Add(keyJoinNPC, power);
                }
            }
        }
        return _CollectionPowerAllTypes;
    }

    public static List<SaveLoadData.TypePrefabs> GetPrioritysTypeModel(PriorityFinder priority)
    {
        List<SaveLoadData.TypePrefabs> getPrioritysTypeModel = new List<SaveLoadData.TypePrefabs>();
        getPrioritysTypeModel.AddRange(priority.PrioritysTypeModelAll);

        foreach (var item in priority.PrioritysTypeModelNPC)
        {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }
        foreach (var item in priority.PrioritysTypeModelFloor)
        {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }
        foreach (var item in priority.PrioritysTypeModelFlore)
        {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }
        foreach (var item in priority.PrioritysTypeModelWood)
        {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }
        foreach (var item in priority.PrioritysTypeModelWall)
        {
            var prioriT = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), item.ToString());
            getPrioritysTypeModel.Add(prioriT);
        }


        return getPrioritysTypeModel;
    }


    public static int GetPriorityPower(ModelNPC.ObjectData objData, PriorityFinder priority)
    {
        int power = 0;
        SaveLoadData.TypePrefabs typeModel = objData.TypePrefab;
        PoolGameObjects.TypePoolPrefabs typePool = objData.TypePoolPrefab;
        TypesBiomNPC biomNPC = GetBiomByTypeModel(typeModel);

        int slotPower = 3;
        int maxtPrioprity = 10;
        //maxtPrioprity = priority.GetPrioritysTypeModel().Count() * slotPower;
        maxtPrioprity = GetPrioritysTypeModel(priority).Count() * slotPower;
        //foreach (SaveLoadData.TypePrefabs itemModel in priority.GetPrioritysTypeModel())
        foreach (SaveLoadData.TypePrefabs itemModel in GetPrioritysTypeModel(priority))
        {
            if (itemModel == typeModel)
            {
                power += maxtPrioprity;
                break;
            }
            maxtPrioprity -= slotPower;
        }
        maxtPrioprity = priority.PrioritysTypeBiomNPC.Count() * slotPower;
        foreach (TypesBiomNPC itemBiom in priority.PrioritysTypeBiomNPC)
        {
            if (itemBiom == biomNPC)
            {
                power += maxtPrioprity;
                break;
            }
            maxtPrioprity -= slotPower;
        }
        maxtPrioprity = priority.PrioritysTypeBiomNPC.Count() * slotPower;
        foreach (PoolGameObjects.TypePoolPrefabs itemPool in priority.PrioritysTypePool)
        {
            if (itemPool == typePool)
            {
                power += maxtPrioprity;
                break;
            }
            maxtPrioprity -= slotPower;
        }

        return power;
    }

    public static TypesBiomNPC GetBiomByTypeModel(SaveLoadData.TypePrefabs typeModel)
    {
        TypesBiomNPC resType = TypesBiomNPC.None;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomBlue), typeModel.ToString()))
            return TypesBiomNPC.Blue;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomGreen), typeModel.ToString()))
            return TypesBiomNPC.Green;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomRed), typeModel.ToString()))
            return TypesBiomNPC.Red;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomViolet), typeModel.ToString()))
            return TypesBiomNPC.Violet;
        if (Enum.IsDefined(typeof(SaveLoadData.TypesBiomGray), typeModel.ToString()))
            return TypesBiomNPC.Gray;
        return resType;
    }

    public static Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> GetPrioritys(ContainerPriorityFinder p_containerPrioritys, string tag)
    {
        Dictionary<SaveLoadData.TypePrefabs, PriorityFinder> result = null;
        try
        {
            Storage.EventsUI.ListLogAdd = "...LoadPriorityPerson....";
            if (p_containerPrioritys == null)
            {
                p_containerPrioritys = ScriptableObjectUtility.LoadContainerPriorityFinderByTag(tag);
                if (p_containerPrioritys == null)
                {
                    Storage.EventsUI.ListLogAdd = "ContainerPriority is null";
                    return null;
                }
            }
            if (p_containerPrioritys.CollectionPriorityFinder == null)
            {
                Storage.EventsUI.ListLogAdd = "ContainerPriority.CollectionPriorityFinder is null";
                return null;
            }
            Storage.EventsUI.ListLogAdd = "CollectionPriorityFinder Count = " + p_containerPrioritys.CollectionPriorityFinder.Count();
            result = new Dictionary<SaveLoadData.TypePrefabs, PriorityFinder>();
            foreach (var prior in p_containerPrioritys.CollectionPriorityFinder)
            {
                result.Add(prior.TypeObserver, prior);
            }

        }
        catch (Exception ex)
        {
            Storage.EventsUI.ListLogAdd = "##### LoadPriorityPerson : " + ex.Message;
        }
        return result;
    }
    #endregion

}


public static class HelperExtension
{

    public static string GetID(this string nameObj)
    {
        string id = "";
        int i = nameObj.LastIndexOf("_");
        //int i2 = nameObjOld.IndexOf("_");
        if (i != -1)
        {
            //123456789
            //Debug.Log("_______________________CREATE NAME i_l=" + i + "     i=" + i2 + "     len=" + nameObjOld.Length + "      :" + nameObjOld);
            id = nameObj.Substring(i + 1, nameObj.Length - i - 1);
            //Debug.Log("_______________________GetGameObjectID  ID:" + id);
        }
        else
        {
            if (nameObj != StoragePerson._Boss &&
                nameObj != StoragePerson._Ufo)
            {
                Debug.Log("!!!!!! GetID Error  on " + nameObj + " !!!!!!!!!!");
            }
        }

        return id;
    }

    public static bool IsField(this string text)
    {
        return !string.IsNullOrEmpty(text) && text == SaveLoadData.TypePrefabs.PrefabField.ToString();
    }

    public static bool IsPoolFloor(this GameObject gobj)
    {
        bool isTrue = PoolGameObjects.TypePoolPrefabs.PoolFloor.ToString() == gobj.tag.ToString();
        return isTrue;
    }
    public static bool IsPoolFlore(this GameObject gobj)
    {
        bool isTrue = PoolGameObjects.TypePoolPrefabs.PoolFlore.ToString() == gobj.tag.ToString();
        return isTrue;
    }
    public static bool IsPoolWood(this GameObject gobj)
    {
        bool isTrue = PoolGameObjects.TypePoolPrefabs.PoolWood.ToString() == gobj.tag.ToString();
        return isTrue;
    }
    public static bool IsPoolWall(this GameObject gobj)
    {
        bool isTrue = PoolGameObjects.TypePoolPrefabs.PoolWall.ToString() == gobj.tag.ToString();
        return isTrue;
    }
    public static bool IsPoolPerson(this GameObject gobj)
    {
        bool isTrue = PoolGameObjects.TypePoolPrefabs.PoolPerson.ToString() == gobj.tag.ToString();
        return isTrue;
    }

    public static string ClearClone(this string text)
    {
        return text.Replace("(Clone)","");
    }
    

    public static string GetNameTextureMap(this string nameObj)
    {
        string result = nameObj.Replace("Prefab", "") + "Map";
        return result;
    }
    public static string GetNamePrefabByTextureMap(this string nameObj)
    {
        string result = "Prefab" + nameObj.Replace("Map", "");
        result = result.Replace("(Clone)", "");
        return result;
    }

    public static int GetOrderPositFromStep(this int number, int step)
    {
        return number - ((((int)(number/step)) - 1)*step);
    }

    //"PrefabVood", TexturesMapsTemp["VoodMap"]
}







