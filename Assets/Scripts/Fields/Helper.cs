using System;
using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("!!!!!! GetID Error  !!!!!!!!!!");

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
        x = (int)(x / 2);
        y = (int)(y / 2);
        return FieldKey + x + "x" + Mathf.Abs(y);
    }

    public static string GetNameFieldPosit(System.Single x, System.Single y)
    {
        x = (int)(x / 2);
        y = (int)(y / 2);
        return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    }

    public static Vector2 GetPositByField(string nameFiled)
    {
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
}
