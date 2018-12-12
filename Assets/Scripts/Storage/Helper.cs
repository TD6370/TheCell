using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public static class Helper { //: MonoBehaviour {

    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

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
            //id = Guid.NewGuid().ToString().Substring(1, 4);
            id = Guid.NewGuid().ToString().Substring(1, 7);
        }

        return tag + "_" + nameFiled + "_" + id;
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

    public static bool IsTerra(SaveLoadData.TypePrefabs typePrefab)
    {
        switch(typePrefab)
        {
            case SaveLoadData.TypePrefabs.PrefabRock:
            case SaveLoadData.TypePrefabs.PrefabVood:
            case SaveLoadData.TypePrefabs.PrefabElka:
            case SaveLoadData.TypePrefabs.PrefabWallRock:
            case SaveLoadData.TypePrefabs.PrefabWallWood:
                return true;
        }
        return false;
    }

    public static bool IsTerra(this string typePrefabStr)
    {
        try
        {
            SaveLoadData.TypePrefabs typePrefab = (SaveLoadData.TypePrefabs)Enum.Parse(typeof(SaveLoadData.TypePrefabs), typePrefabStr);
            return IsTerra(typePrefab);
        }catch(Exception x)
        {
            Debug.Log("IsTerra " + x.Message);
            return false;
        }
    }
        
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

    //TypePrefabs prefabType = (TypePrefabs)Enum.Parse(typeof(TypePrefabs), namePrefab);
    //if (!System.Enum.IsDefined(typeof(SaveLoadData.TypePrefabs), item.name))
    //{
    //    typeTilePrefab = TypesStructure.Terra;
    //    Debug.Log("Not Prefab"); 
    //}
    public static SaveLoadData.TypePrefabs ParsePrefab(string strType)
    {
        if (System.Enum.IsDefined(typeof(SaveLoadData.TypePrefabs), strType))
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

    public static Vector2 NormalizPosToField(System.Single x, System.Single y)
    {
        x = (int)(x / Storage.ScaleWorld);
        y = (int)(y / Storage.ScaleWorld);
        return new Vector2(x, Mathf.Abs((int)y));
    }

    public static Vector2 NormalizFieldToPos(System.Single x, System.Single y)
    {
        x = (int)(x * Storage.ScaleWorld);
        y = (int)(y * Storage.ScaleWorld);
        y *= -1;
        return new Vector2(x, y);
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
        int hx = Storage.Instance.HeroPositionX;
        int hy = Storage.Instance.HeroPositionY;
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



    #endregion

    #region Profiler

    public static void StartControlTime()
    {
        Helper.SpeedTimer = Time.time;
    }

    public static void StopControlTime(string info)
    {
        Storage.Events.ListLogAdd = info + (Time.time - Helper.SpeedTimer);
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
    //"PrefabVood", TexturesMapsTemp["VoodMap"]
}

#region Serialize Helper

public class Serializator
{
    public static Type[] extraTypes = {
            typeof(ModelNPC.FieldData),
            typeof(ModelNPC.ObjectData),

            typeof(ModelNPC.GameDataUfo),

            typeof(ModelNPC.GameDataNPC),
            typeof(ModelNPC.PersonData),

            typeof(ModelNPC.PersonDataBoss),
            typeof(ModelNPC.GameDataBoss),
            typeof(TilesData),
            typeof(DataTile),
            typeof(DataConstructionTiles),
            typeof(ModelNPC.WallData),
            typeof(ModelNPC.TerraData)
    };

    static public void SaveGridXml(ModelNPC.GridData state, string datapath, bool isNewWorld = false)
    {
        string indErr = "start";

        //------- Save parts
        //SaveGridPartsXml(state, datapath,isNewWorld);
        //------- Save cash and big
        SaveGridCashXml(state, datapath, isNewWorld);
        return;
        //----------------


        if (isNewWorld)
        {
            if (File.Exists(datapath))
            {
                try
                {
                    indErr = "delete";
                    File.Delete(datapath);
                }
                catch (Exception x)
                {
                    Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapath + " : " + x.Message);
                }
            }
        }

        try
        {
            indErr = "1";
            //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
            //## 
            state.FieldsXML = state.FieldsD.ToList();
            indErr = "2";
            //## 
            Debug.Log("SaveXml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);

            indErr = "3";
            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);

            indErr = "5";
            FileStream fs = new FileStream(datapath, FileMode.Create);

            indErr = "6";
            serializer.Serialize(fs, state);

            indErr = "7";
            fs.Close();

            indErr = "8";
            state.FieldsXML = null;
            //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
        }catch(Exception x)
        {
            Debug.Log("######### SaveGridXml: " + x.Message + "     to :" + datapath);
        }
    }

    
    static public void SaveGridPartsXml(ModelNPC.GridData state, string datapath, bool isNewWorld = false)
    {

        string indErr = "start";

        if (isNewWorld)
        {
            for (int partX = 0; partX < Helper.SizePart; partX++)
            {
                for (int partY = 0; partY < Helper.SizePart; partY++)
                {
                    indErr = "c.1";
                    string nameFileXML = +(partX + 1) + "x" + (partY + 1);
                    string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
                    datapath = datapathPart;
                    if (File.Exists(datapathPart))
                    {
                        try
                        {
                            indErr = "delete";
                            File.Delete(datapathPart);
                        }
                        catch (Exception x)
                        {
                            Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapathPart + " : " + x.Message);
                        }
                    }

                }
            }

            
        }

        try
        {
            Dictionary<string, ModelNPC.FieldData> FieldsPart = new Dictionary<string, ModelNPC.FieldData>();
            int SizePart = Helper.WidthLevel / Helper.SizePart;

            Dictionary<string, Dictionary<string, ModelNPC.FieldData>> PartsGrids = new Dictionary<string, Dictionary<string, ModelNPC.FieldData>>();
            for (int partX = 0; partX < Helper.SizePart; partX++)
            {
                for (int partY = 0; partY < Helper.SizePart; partY++)
                {
                    FieldsPart = new Dictionary<string, ModelNPC.FieldData>();
                    string nameFileXML =  + (partX + 1) + "x" + (partY + 1);
                    int startX = partX * SizePart;
                    int startY = partY * SizePart;
                    int widthX = startX + SizePart;
                    int widthY = startY + SizePart;
                    for (int x = startX; x < widthX; x++)
                    {
                        for (int y = startY; y < widthY; y++)
                        {
                            indErr = "d.1";
                            string fieldName = Helper.GetNameField(x, y);
                            indErr = "d.2";
                            if (state.FieldsD.ContainsKey(fieldName))
                            {
                                ModelNPC.FieldData copyFields = state.FieldsD[fieldName];

                                FieldsPart.Add(fieldName, copyFields);
                            }
                        }
                    }
                    indErr = "d.3";
                    PartsGrids.Add(nameFileXML, FieldsPart);
                }
            }

            foreach (var partGrid in PartsGrids)
            {
                indErr = "1";
                //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
                //## 
                Dictionary<string, ModelNPC.FieldData> partGridFields = partGrid.Value;
                indErr = "1.1.";
                string nameFileXML = partGrid.Key;
                indErr = "1.2.";
                string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
                datapath = datapathPart;

                indErr = "1.3.";

                state.FieldsXML = partGridFields.ToList();
                indErr = "2";
                //## 
                Debug.Log("SaveXml GridData D:" + partGridFields.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapathPart);

                indErr = "3";
                XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);

                indErr = "5";
                //FileStream fs = new FileStream(datapathPart, FileMode.CreateNew);

                //indErr = "6";
                //serializer.Serialize(fs, state);

                //indErr = "7";
                //fs.Close();
                using (FileStream fs = new FileStream(datapathPart, FileMode.CreateNew))
                {
                    indErr = "6";
                    serializer.Serialize(fs, state);

                    indErr = "7";
                    fs.Close();
                }
            }
            indErr = "8";
            state.FieldsXML = null;
            //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
        }
        catch (Exception x)
        {
            Debug.Log("######### SaveGridXml: " + x.Message + "     to :" + datapath + "    #" + indErr) ;
        }
    }

    static public void SaveGridCashXml(ModelNPC.GridData state, string datapath, bool isNewWorld = false)
    {
        string indErr = "start";
        try
        {
            Dictionary<string, ModelNPC.FieldData> dataSave = new Dictionary<string, ModelNPC.FieldData>(state.FieldsD);

            Dictionary<string, ModelNPC.FieldData> FieldsPart = new Dictionary<string, ModelNPC.FieldData>();
            int SizePart = Helper.WidthLevel / Helper.SizePart;

            Dictionary<string, Dictionary<string, ModelNPC.FieldData>> PartsGrids = new Dictionary<string, Dictionary<string, ModelNPC.FieldData>>();
         
            int partX = 0;
            int partY = 0;
            FieldsPart = new Dictionary<string, ModelNPC.FieldData>();
            string nameFileXML = +(partX + 1) + "x" + (partY + 1);
            int startX = partX * SizePart;
            int startY = partY * SizePart;
            int widthX = startX + SizePart;
            int widthY = startY + SizePart;

            Helper.StartControlTime();

            for (int x = startX; x < widthX; x++)
            {
                for (int y = startY; y < widthY; y++)
                {
                    indErr = "d.1";
                    string fieldName = Helper.GetNameField(x, y);
                    indErr = "d.2";
                    //if (state.FieldsD.ContainsKey(fieldName))
                    if (dataSave.ContainsKey(fieldName))
                    {
                        //ModelNPC.FieldData copyFields = state.FieldsD[fieldName];
                        ModelNPC.FieldData copyFields = dataSave[fieldName];

                        FieldsPart.Add(fieldName, copyFields);
                        //state.FieldsD.Remove(fieldName);
                        dataSave.Remove(fieldName);
                    }
                }
            }
            indErr = "d.3";

            Helper.StopControlTime("...time: SaveGridCashXml FieldsPart: ");

            //Dictionary<string, ModelNPC.FieldData> cashPart = PartsGrids["1x1"];
            Dictionary<string, ModelNPC.FieldData> cashPart = FieldsPart;
            Dictionary<string, ModelNPC.FieldData> bigPart = dataSave;

            Helper.StartControlTime();
            ModelNPC.GridData stateCash = new ModelNPC.GridData() { FieldsD = cashPart };
            SavePartXML(stateCash, "1x1");
            Helper.StopControlTime("...time: SaveGridCashXml Save Cash");

            Helper.StartControlTime();
            ModelNPC.GridData stateBig = new ModelNPC.GridData() { FieldsD = bigPart };
            SavePartXML(stateBig, "1x2");
            Helper.StopControlTime("...time: SaveGridCashXml Save Big");


            indErr = "8";
            state.FieldsXML = null;
            //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
        }
        catch (Exception x)
        {
            Debug.Log("######### SaveGridXml: " + x.Message + "     to :" + datapath + "    #" + indErr);
        }
    }

    //public static SavePartXML(ModelNPC.GridData state, string nameFileXML = "")
    public static void SavePartXML(ModelNPC.GridData state, string nameFileXML = "")
    {
        string indErr = "1";
        //Dictionary<string, ModelNPC.FieldData> partGridFields = partGrid;
        indErr = "1.1.";
        string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
        if (File.Exists(datapathPart))
        {
            try
            {
                indErr = "delete";
                File.Delete(datapathPart);
            }
            catch (Exception x)
            {
                Debug.Log("############# Error SavePartXML NOT File Delete: " + datapathPart + " : " + x.Message);
            }
        }

        indErr = "1.3.";

        state.FieldsXML = state.FieldsD.ToList();
        indErr = "2";
        //## 
        Debug.Log("SaveXml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapathPart);

        indErr = "3";
        //XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);
        XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);


        System.Text.UTF8Encoding encodingT = new System.Text.UTF8Encoding();
        indErr = "5";
        using (FileStream fs = new FileStream(datapathPart, FileMode.CreateNew))
        {
            indErr = "6";

            //-----------
            //var memoryStream = new MemoryStream();
            //var streamWriter = new StreamWriter(memoryStream, System.Text.Encoding.UTF8);
            //serializer.Serialize(streamWriter, state);
            //serializer.Serialize(fs, streamWriter);
            ////----------------
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    //XmlSerializer s = new XmlSerializer(typeof(T));
            //    //Console.WriteLine("Testing for type: {0}", typeof(T));
            //    serializer.Serialize(System.Xml.XmlWriter.Create(stream), state);
            //    stream.Flush();
            //    stream.Seek(0, SeekOrigin.Begin);
            //    object o = s.Deserialize(XmlReader.Create(stream));
            //    Console.WriteLine("  Deserialized type: {0}", o.GetType());
            //}
            //--------------------

            //
            //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            //inputString = encoding.GetString(inputString);
            //serializer.Serialize(fs, state, encodingStyle: "UTF8Encoding");
            //var encoding = Encoding.GetEncoding("ISO-8859-1");
            //using (StreamWriter sw = new StreamWriter(fname, appendMode, encoding))

            //----------------
            serializer.Serialize(fs, state);
            //----------------

            indErr = "7";
            fs.Close();
        }

        //FileStream fs = new FileStream(datapathPart, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        //StreamWriter swFromFileTrueUTF8 = new StreamWriter(datapathPart, true, System.Text.Encoding.UTF8);
        //swFromFileTrueUTF8.Write(textToAdd);
        //swFromFileTrueUTF8.Flush();
        //swFromFileTrueUTF8.Close();
    }

    static public ModelNPC.GridData LoadGridPartsXml()
    {
        string datapath;

        string stepErr = "start";

        ModelNPC.GridData result = new ModelNPC.GridData();
        try
        {
            Debug.Log("Loaded Xml GridData start...");
            for (int partX = 0; partX < 3; partX++)
            {
                for (int partY = 0; partY < 3; partY++)
                {
                    stepErr = "c.1";
                    string nameFileXML = +(partX + 1) + "x" + (partY + 1);
                    string datapathPart = Application.dataPath + "/Levels/LevelDataPart" + nameFileXML + ".xml";
                    datapath = datapathPart;
                    if (File.Exists(datapathPart))
                    {
                        try
                        {
                            ModelNPC.GridData itemGridData = null;
                            stepErr = ".1";
                            stepErr = ".2";
                            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);
                            stepErr = ".3";
                            using (FileStream fs = new FileStream(datapathPart, FileMode.Open))
                            {
                                stepErr = ".4";
                                itemGridData = (ModelNPC.GridData)serializer.Deserialize(fs);
                                stepErr = ".5";
                                fs.Close();
                            }
                            stepErr = ".6";
                            itemGridData.FieldsD = itemGridData.FieldsXML.ToDictionary(x => x.Key, x => x.Value);
                            stepErr = ".7";
                            Debug.Log("Loaded Xml GridData D:" + itemGridData.FieldsD.Count() + "   XML:" + result.FieldsXML.Count() + "     datapath=" + datapathPart);
                            //## 
                            itemGridData.FieldsXML = null;
                            stepErr = ".8";
                            result.FieldsD = result.FieldsD.Concat(itemGridData.FieldsD)
                                .ToDictionary(x => x.Key, x => x.Value);

                        }
                        catch (Exception x)
                        {
                            Debug.Log("############ #" + stepErr + " LoadGridPartsXml : " + datapathPart + " : " + x.Message);
                        }
                    }

                }
            }

            
        }
        catch (Exception x)
        {
            result = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        return result;
    }

    static public ModelNPC.GridData LoadGridXml(string datapath)
    {
        //--Load parts
        //return LoadGridPartsXml();
        //return null;
        //-------------------

        string stepErr = "start";
        ModelNPC.GridData state = null;
        try
        {
            Debug.Log("Loaded Xml GridData start...");

            stepErr = ".1";
            //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
            stepErr = ".2";
            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);
            stepErr = ".3";
            FileStream fs = new FileStream(datapath, FileMode.Open);
            stepErr = ".4";
            state = (ModelNPC.GridData)serializer.Deserialize(fs);
            stepErr = ".5";
            fs.Close();

            stepErr = ".6";
            state.FieldsD = state.FieldsXML.ToDictionary(x => x.Key, x => x.Value);
            stepErr = ".7";
            Debug.Log("Loaded Xml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
            //## 
            state.FieldsXML = null;
        }
        catch (Exception x)
        {
            state = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        return state;
    }

    static public ModelNPC.LevelData LoadPersonXml(string datapath)
    {
        string stepErr = "start";
        ModelNPC.LevelData state = null;
        try
        {
            Debug.Log("Loaded Xml GridData start...");

            stepErr = ".1";
            stepErr = ".2";
            XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.LevelData), extraTypes);
            stepErr = ".3";
            FileStream fs = new FileStream(datapath, FileMode.Open);
            stepErr = ".4";
            state = (ModelNPC.LevelData)serializer.Deserialize(fs);
            stepErr = ".5";
            fs.Close();
            stepErr = ".6";
            state.Persons = state.PersonsXML.ToDictionary(x => x.Key, x => x.Value);
            stepErr = ".7";
            Debug.Log("Loaded Xml CasePersonData :" + state.Persons.Count() + "   XML:" + state.PersonsXML.Count() + "     datapath=" + datapath);
            state.PersonsXML = null;
        }
        catch (Exception x)
        {
            state = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        return state;
    }

    static public TilesData LoadTilesXml(string datapath)
    {
        string stepErr = "start";
        TilesData state = null;
        try
        {
            Debug.Log("Loaded Xml GridData start...");

            stepErr = ".1";
            stepErr = ".2";
            XmlSerializer serializer = new XmlSerializer(typeof(TilesData), extraTypes);
            stepErr = ".3";
            FileStream fs = new FileStream(datapath, FileMode.Open);
            stepErr = ".4";
            state = (TilesData)serializer.Deserialize(fs);
            stepErr = ".5";
            fs.Close();
            stepErr = ".6";
            state.TilesD = state.TilesXML.ToDictionary(x => x.Key, x => x.Value);
            stepErr = ".7";
            Debug.Log("Loaded Xml CasePersonData :" + state.TilesD.Count() + "     datapath=" + datapath);
            state.TilesXML = null;
        }
        catch (Exception x)
        {
            state = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        return state;
    }

    static public void SaveTilesDataXml(TilesData state, string datapath, bool isNewWorld = false)
    {

        if (isNewWorld)
        {
            if (File.Exists(datapath))
            {
                try
                {
                    File.Delete(datapath);
                }
                catch (Exception x)
                {
                    Debug.Log("############# Error TilesData NOT File Delete: " + datapath + " : " + x.Message);
                }
            }
        }

        //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
        //## 
        state.TilesXML = state.TilesD.ToList();

        //## 
        Debug.Log("SaveXml GridData D:" + state.TilesD.Count() + "   XML:" + state.TilesXML.Count() + "     datapath=" + datapath);

        XmlSerializer serializer = new XmlSerializer(typeof(TilesData), extraTypes);

        FileStream fs = new FileStream(datapath, FileMode.Create);

        serializer.Serialize(fs, state);
        fs.Close();

        state.TilesXML = null;
        //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
    }

    //[XmlRoot("TilesData")]
    //[XmlInclude(typeof(DataTile))]
    //public class TilesData
    //{
    //    private Dictionary<string, List<DataTile>> CollectionDataMapTales;
    //    public List<KeyValuePair<string, List<DataTile>>> TilesXML = new List<KeyValuePair<string, List<DataTile>>>();
    //    public TilesData() { }
    //}

    //[XmlType("Tile")]
    //public class DataTile
    //{
    //    public int X { get; set; }
    //    public int Y { get; set; }
    //    public string Name { get; set; }
    //    public string Tag { get; set; }
    //    public bool IsLock { get; set; }

    //    public DataTile() { }
    //}

    static public T LoadXml<T>(string datapath, Type[] p_extraTypes = null) where T : class
    {
        string stepErr = "start";
        T state = null;
        try
        {
            //Debug.Log("Loaded Xml GridData start...");

            if (p_extraTypes == null)
                p_extraTypes = extraTypes;

            stepErr = ".1";
            stepErr = ".2";
            XmlSerializer serializer = new XmlSerializer(typeof(T), p_extraTypes);
            stepErr = ".3";
            FileStream fs = new FileStream(datapath, FileMode.Open);
            stepErr = ".4";
            state = (T)serializer.Deserialize(fs);
            stepErr = ".5";
            fs.Close();
            stepErr = ".6";
            stepErr = ".7";
        }
        catch (Exception x)
        {
            state = null;
            Debug.Log("Error DeXml: " + x.Message + " " + stepErr);
        }

        return state;
    }

    static public void SaveXml<T>(T state, string datapath, bool isResave = false, System.Type[] p_extraTypes = null) where T : class
    {

        if (isResave)
        {
            if (File.Exists(datapath))
            {
                try
                {
                    File.Delete(datapath);
                }
                catch (Exception x)
                {
                    Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapath + " : " + x.Message);
                }
            }
        }

        if (p_extraTypes == null)
            p_extraTypes = extraTypes;

        XmlSerializer serializer = new XmlSerializer(typeof(T), p_extraTypes);

        FileStream fs = new FileStream(datapath, FileMode.Create);

        serializer.Serialize(fs, state);
        fs.Close();

    }
}

#endregion





