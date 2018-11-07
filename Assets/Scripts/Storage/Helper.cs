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
            id = Guid.NewGuid().ToString().Substring(1, 4);
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


        Debug.Log("----- GetPositByField (" + nameFiled + ") : " + x + "x" + y);
        return new Vector2(x, y);
    }

    public static string GetNameFieldByName(string nameGameObject)
    {

        int start = nameGameObject.IndexOf(FieldKey);
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



    public static int WidthLevel
    {
        get { return 100; }
    }
    public static int HeightLevel
    {
        get { return 100; }
    }
    public static int WidthWorld
    {
        get { return 1000; }
    }
    public static int HeightWorld
    {
        get { return 1000; }
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
        if (y > -1) //*-1
            y = offset - Math.Abs(offset);
        if (x > Helper.WidthLevel * Storage.ScaleWorld)
            x = (Helper.WidthLevel * Storage.ScaleWorld) - Math.Abs(offset);
        if (y < (Helper.HeightLevel * Storage.ScaleWorld) * (-1)) //*-1
            y = ((Helper.HeightLevel * Storage.ScaleWorld) - Math.Abs(offset)) * (-1);
        Vector2 result = new Vector2(x, y);
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

}

#region Serialize Helper

public class Serializator
{
    static Type[] extraTypes = {
            typeof(ModelNPC.FieldData),
            typeof(ModelNPC.ObjectData),

            typeof(ModelNPC.GameDataUfo),

            typeof(ModelNPC.GameDataNPC),
            typeof(ModelNPC.PersonData),

            typeof(ModelNPC.PersonDataBoss),
            typeof(ModelNPC.GameDataBoss),
            typeof(TilesData),
            typeof(DataTile),

    };

    static public void SaveGridXml(ModelNPC.GridData state, string datapath, bool isNewWorld = false)
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
                    Debug.Log("############# Error SaveGridXml NOT File Delete: " + datapath + " : " + x.Message);
                }
            }
        }

        //Type[] extraTypes = { typeof(FieldData), typeof(ObjectData), typeof(ObjectDataUfo) };
        //## 
        state.FieldsXML = state.FieldsD.ToList();

        //## 
        Debug.Log("SaveXml GridData D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);

        XmlSerializer serializer = new XmlSerializer(typeof(ModelNPC.GridData), extraTypes);

        FileStream fs = new FileStream(datapath, FileMode.Create);

        serializer.Serialize(fs, state);
        fs.Close();

        state.FieldsXML = null;
        //Debug.Log("Saved Xml GridData L:" + state.Fields.Count() + "  D:" + state.FieldsD.Count() + "   XML:" + state.FieldsXML.Count() + "     datapath=" + datapath);
    }

    static public ModelNPC.GridData LoadGridXml(string datapath)
    {
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

    static public T LoadXml<T>(string datapath) where T : class
    {
        string stepErr = "start";
        T state = null;
        try
        {
            //Debug.Log("Loaded Xml GridData start...");

            stepErr = ".1";
            stepErr = ".2";
            XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);
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

    static public void SaveXml<T>(T state, string datapath, bool isResave = false) where T : class
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

        XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);

        FileStream fs = new FileStream(datapath, FileMode.Create);

        serializer.Serialize(fs, state);
        fs.Close();

    }
}

#endregion





