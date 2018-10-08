using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Storage : MonoBehaviour {

    public static ZonaFieldLook ZonaField { get; set; }
    public static ZonaRealLook ZonaReal { get; set; }

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

    private int _limitHorizontalLook = 22;
    public int LimitHorizontalLook
    {
        get { return _limitHorizontalLook; }
    }
    private int _limitVerticalLook = 18;
    public int LimitVerticalLook
    {
        get { return _limitVerticalLook; }
    }
    
    private int _heroPositionX = 0;
    public int HeroPositionX
    {
        get { return _heroPositionX; }
    }
    private int _heroPositionY = 0;
    public int HeroPositionY
    {
        get { return _heroPositionY; }
    }

    private static Dictionary<string, List<GameObject>> GamesObjectsReal;
    private static SaveLoadData.GridData GridData;

	// Use this for initialization
	void Start () {
        ZonaField = null;
        ZonaReal = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void SetGridData(SaveLoadData.GridData p_GridData)
    {
        GridData = p_GridData;
    }
    public static void SetGamesObjectsReal(Dictionary<string, List<GameObject>> p_GamesObjectsReal)
    {
        GamesObjectsReal = p_GamesObjectsReal;
    }

    public void SetHeroPosition(int x, int y, float xH, float yH)
    {
        int scale = 2;
        _heroPositionX = x;
        _heroPositionY = y;

        int _limitX = _limitHorizontalLook / 2;
        int _limitY = _limitVerticalLook / 2;
        {
            int fX = x - _limitX;
            int fY = y - _limitY;
            
            if (fX < 0) fX = 0;
            if (fY < 0) fY = 0;
            int fX2 = x + _limitX;
            int fY2 = y + _limitY;

            ZonaField = new ZonaFieldLook()
            {
                X = fX,
                Y = fY,
                X2 = fX2,
                Y2 = fY2
            };
            //Debug.Log("ZonaField: X:" + ZonaField.X + " Y:" + ZonaField.Y + " X2:" + ZonaField.X2 + " Y2:" + ZonaField.Y2);
        }
        {
            float limitH = _limitHorizontalLook / 2;
            float limitV = _limitVerticalLook / 2;

            float rX = xH - (_limitX * scale);
            float rY = yH + (_limitY * scale);
            float margin = 0.1f;
            if (rX < 0)
            {
                rX = 0.1f;
                limitH -= margin;
            }
            if (rY > 0)
            {
                rY = -0.1f;
                limitV -= margin;
            }
            int LevelX = WidthLevel * scale;
            int LevelY = HeightLevel * scale;

            float rX2 = xH + (limitH * scale);
            float rY2 = yH - (limitV * scale);
            if (rX2 > LevelX) rX2 = LevelX;
            if (rY2 > LevelY) rY2 = LevelY;

            ZonaReal = new ZonaRealLook()
            {
                X = rX,
                Y = rY,
                X2 = rX2,
                Y2 = rY2
            };
            //Debug.Log("ZonaReal: X:" + ZonaReal.X + " Y:" + ZonaReal.Y + " X2:" + ZonaReal.X2 + " Y2:" + ZonaReal.Y2);
            //Draw result
            //DrawRect(rX,rY,rX2,rY2);
        }
    }

    private void DrawRect(float x,float y, float x2, float y2)
    {
        //return;
        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.Log("LineRenderer is null !!!!");
            return;
        }

        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //lineRenderer.SetColors(c1, c2);
        lineRenderer.SetColors(Color.green, Color.green);
        lineRenderer.SetWidth(0.2F, 0.2F);
        int size = 5;
        lineRenderer.SetVertexCount(size);
        
        Vector3 pos1 = new Vector3(x, y, -2);
        lineRenderer.SetPosition(0, pos1);
        Vector3 pos2 = new Vector3(x2, y, -2);
        lineRenderer.SetPosition(1, pos2);
        Vector3 pos3 = new Vector3(x2, y2, -2);
        lineRenderer.SetPosition(2, pos3);
        Vector3 pos4 = new Vector3(x, y2, -2);
        lineRenderer.SetPosition(3, pos4);
        Vector3 pos5 = new Vector3(x, y, -2);
        lineRenderer.SetPosition(4, pos5);
    }

    public static Vector2 ValidPiontInZona(ref float x,ref float y, float offset=0)
    {
        offset = Mathf.Abs(offset);

        if (x < ZonaReal.X)
            x = ZonaReal.X + offset;
        if (y > ZonaReal.Y) //*-1
            y = ZonaReal.Y - offset;
        if (x > ZonaReal.X2)
            x = ZonaReal.X2 - offset;
        if (y < ZonaReal.Y2) //*-1
            y = ZonaReal.Y + offset;
        Vector2 result = new Vector2(x, y);
        return result;
    }

    public static bool IsValidPiontInZona(float x,float y)
    {
        bool result = true;

        if (x < ZonaReal.X)
            return false;
        if (y > ZonaReal.Y) //*-1
            return false;
        if (x > ZonaReal.X2)
            return false;
        if (y < ZonaReal.Y2) //*-1
            return false;
        return result;
    }


    public class ZonaFieldLook
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public ZonaFieldLook() { }
    }

    public class ZonaRealLook
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public ZonaRealLook() {}
    }

    //+FIX
    public static void UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, Vector3 p_newPosition)
    {
        if (GamesObjectsReal == null || GamesObjectsReal.Count == 0)
        {
            Debug.Log("^^^^ UpfatePosition      GamesObjectsReal is EMPTY");
            return;
        }
        if (GridData == null ||  GridData.FieldsD ==null || GridData.FieldsD.Count == 0)
        {
            Debug.Log("^^^^ UpfatePosition      GridData is EMPTY");
            return;
        }

        List<GameObject> realObjects = GamesObjectsReal[p_OldField];
        List<SaveLoadData.ObjectData> dataObjects = GridData.FieldsD[p_OldField].Objects;

        int indReal = realObjects.FindIndex(p => p.name == p_NameObject);
        if (indReal == -1)
        {
            Debug.Log("^^^^ UpfatePosition Not Real object (" + p_NameObject + ") in field: " + p_OldField);
            return;
        }
        int indData = dataObjects.FindIndex(p => p.NameObject == p_NameObject);
        if (indData == -1)
        {
            Debug.Log("^^^^ UpfatePosition Not DATA object (" + p_NameObject + ") in field: " + p_OldField);
            foreach (var itemObj in dataObjects)
            {
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- object : " + itemObj.NameObject);
            }
            if(dataObjects.Count==0)
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- objects ZERO !!!!!");

            return;
        }
        GameObject gobj = realObjects[indReal];

        dataObjects.RemoveAt(indData);
        //add to new Field
        if (!GridData.FieldsD.ContainsKey(p_NewField))
        {
            //#!!!!  Debug.Log("SaveListObjectsToData GridData ADD new FIELD : " + posFieldReal);
            GridData.FieldsD.Add(p_NewField, new SaveLoadData.FieldData());
        }

        SaveLoadData.ObjectData objDataNow = SaveLoadData.CreateObjectData(gobj);

        //int pp = GridData.FieldsD[p_NewField].Objects.Count;
        objDataNow.NameObject = SaveLoadData.CreateName(objDataNow.TagObject, p_NewField, p_NameObject);

        //objDataNow.Position = p_newPosition; //gobj.transform.position;
        objDataNow.Position = gobj.transform.position;

        GridData.FieldsD[p_NewField].Objects.Add(objDataNow);

        if (!GamesObjectsReal.ContainsKey(p_NewField))
        {
            GamesObjectsReal.Add(p_NewField, new List<GameObject>());
        }
        //Debug.Log("SaveListObjectsToData   MOVE RealObject -> " + posFieldOld + " -> " + posFieldReal + "  (Real PRED)  NewPos=" + GamesObjectsReal[posFieldReal].Count + " OldPos=" + GamesObjectsReal[posFieldOld].Count);
        gobj.name = objDataNow.NameObject;

        //123456789
        PersonalData personData = gobj.GetComponent<PersonalData>();
        personData.PersonalObjectData.Position = gobj.transform.position;
        
        GamesObjectsReal[p_NewField].Add(gobj);

        realObjects.RemoveAt(indReal);

        //12345677899
        //LoadObjectToReal:  DATA -->> PERSONA #P#


        //Debug.Log("SaveListObjectsToData   MOVE RealObject -> " + posFieldOld + " -> " + posFieldReal + "  (Real NEW)  NewPos=" + GamesObjectsReal[posFieldReal].Count + " OldPos=" + GamesObjectsReal[posFieldOld].Count);

        //Debug.Log("^^^^ UpfatePosition UPDATED.......");
        //                                                                                                                                                    ((SaveLoadData.GameDataNPC)objUfo).                                                                          
        //-------------------
        int p_x = (int)objDataNow.Position.x;
        int p_y = (int)objDataNow.Position.y;
        int x = (int)(p_x / 2);
        int y = (int)(p_y / 2);
        string fieldR = FieldKey + (int)x + "x" + Mathf.Abs((int)y) ;
        string strX = "  x: " + objDataNow.Position.x + " -> " + p_x + " -> "+ (p_x/2) + " -> " + x;
        string strY = "  y: " + objDataNow.Position.y + " -> " + p_y + " -> " + (p_y / 2) + " -> " + y;
        fieldR += strX + strY;
        //------------------

        Debug.Log("^^^^ UpfatePosition UPDATED...... " + gobj.name + " : go:" + gobj.transform.position + "  DO:" + objDataNow.Position + "  NPC:" + ((SaveLoadData.GameDataNPC)objDataNow).Position + "  UFO:" + ((SaveLoadData.GameDataUfo)objDataNow).Position + " NEWPOS:" + ((SaveLoadData.GameDataNPC)objDataNow).NewPosition);
        Debug.Log("^^^^ UpfatePosition UPDATED......  >" + fieldR );
    }

    //private string GetNameFieldPosit_2(System.Single x, System.Single y)
    //{
    //    x = (int)(x / 2);
    //    y = (int)(y / 2);
    //    return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    //}

    const string FieldKey = "Field";

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

    public static string GetNameFieldByName(string nameGameObject)
    {

        int start = nameGameObject.IndexOf(FieldKey);
        string result = "";
        string resultInfo = "";
        int valid = 0;
        if (start == -1)
        {
            Debug.Log("# GetNameFieldByName " + nameGameObject + " key 'Field' not found!");
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
}
