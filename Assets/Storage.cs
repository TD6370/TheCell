using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Storage : MonoBehaviour {

    const string FieldKey = "Field";

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

    //123456789
    private static Dictionary<string, List<GameObject>> _GamesObjectsReal;
    public static Dictionary<string, List<GameObject>> GamesObjectsReal
    {
        get { return _GamesObjectsReal; }
    }

    private static SaveLoadData.GridData _GridData;
    public static SaveLoadData.GridData GridData
    {
        get { return _GridData; }
    }

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
        _GridData = p_GridData;
    }
    public static void SetGamesObjectsReal(Dictionary<string, List<GameObject>> p_GamesObjectsReal)
    {
        _GamesObjectsReal = p_GamesObjectsReal;
    }

    public void SetHeroPosition(int x, int y, float xH, float yH)
    {
        //Debug.Log("SetHeroPosition...");

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

    //public static SaveLoadData.ObjectData FindDataObject(GameObject p_gobject)
    //{
    //    SaveLoadData.ObjectData resData;
    //    string nameField = Storage.GetNameFieldByName(p_gobject.name);

    //    if (!Storage.GridData.FieldsD.ContainsKey(nameField))
    //    {
    //        Debug.Log("!!!!!!!!! Error CreateObjectData FIELD NOT FOUND :" + nameField);
    //        return null;
    //    }
    //    var objects = Storage.GridData.FieldsD[nameField].Objects;
    //    var index = objects.FindIndex(p => p.NameObject == p_gobject.name);
    //    if (index == -1)
    //    {
    //        Debug.Log("!!!!!!!!! Error CreateObjectData OBJECT NOT FOUND : " + p_gobject.name + "   in Field: " + nameField);
    //        return null;
    //    }

    //    return resData;
    //}
    
    public static string GetGameObjectID(GameObject gobj)
    {
        string nameObj = gobj.name;
        string id = "";
        int i = nameObj.LastIndexOf("_");
        //int i2 = nameObjOld.IndexOf("_");
        if (i != -1)
        {
            //123456789
            //Debug.Log("_______________________CREATE NAME i_l=" + i + "     i=" + i2 + "     len=" + nameObjOld.Length + "      :" + nameObjOld);
            id = nameObj.Substring(i + 1, nameObj.Length - i - 1);
            Debug.Log("_______________________GetGameObjectID  ID:" + id);
        }
        else
            Debug.Log("!!!!!! GetGameObjectID Error create name prefix !!!!!!!!!!");

        return id;
    }

    //public static void NextPosition(GameObject gobj) //, Vector3 p_newPosition)
    //{
    //    Vector3 _newPosition = gobj.transform.position;
    //    string nameObject = gobj.name;

    //    SaveLoadData.ObjectData objData = SaveLoadData.CreateObjectData(gobj);
    //    if (objData == null)
    //    {
    //        Debug.Log("!!!!!!!!! Error NextPosition      OBJECT DATA NOT FOUND : " + nameObject);
    //        return;
    //    }

    //    Vector3 _oldPosition = objData.Position;

    //    string posFieldOld = GetNameFieldPosit(_oldPosition.x, _oldPosition.y);
    //    string posFieldReal = GetNameFieldPosit(_newPosition.x, _newPosition.y);

    //    if (posFieldOld != posFieldReal)
    //    {
    //        //-------------------
    //        int p_x = (int)_newPosition.x;
    //        int p_y = (int)_newPosition.y;
    //        int x = (int)(p_x / 2);
    //        int y = (int)(p_y / 2);
    //        string fieldR = FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    //        string strX = "  x: " + _newPosition.x + " -> " + p_x + " -> " + (p_x / 2) + " -> " + x;
    //        string strY = "  y: " + _newPosition.y + " -> " + p_y + " -> " + (p_y / 2) + " -> " + y;
    //        fieldR += strX + strY;
    //        //------------------

    //        //123456789
    //        //NewPosition = p_newPosition;
    //        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~ NextPosition " + nameObject + "          " + posFieldOld + " > " + posFieldReal + "     " + _oldPosition + "  >>  " + _newPosition);
    //        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~ NextPosition " + fieldR);
    //        Storage.UpdateGamePosition(posFieldOld, posFieldReal, nameObject, objData, _newPosition);
    //        //123456789
    //        //NewPosition = p_newPosition;
    //    }
    //}

    //public static void UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, SaveLoadData.ObjectData objData, Vector3 p_newPosition)
    //public static void UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, SaveLoadData.ObjectData objData)
    //public static string UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, SaveLoadData.ObjectData objData)
    public static string UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, SaveLoadData.ObjectData objData, Vector3 p_newPosition, bool isDestroy = false)
    {
        if (_GamesObjectsReal == null || _GamesObjectsReal.Count == 0)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      GamesObjectsReal is EMPTY");
            return "";
        }
        if (_GridData == null || _GridData.FieldsD == null || _GridData.FieldsD.Count == 0)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      GridData is EMPTY");
            return "";
        }

        if(!_GamesObjectsReal.ContainsKey(p_OldField))
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition      GamesObjectsReal not found OldField = " + p_OldField);
            return "";
        }
        if (!_GridData.FieldsD.ContainsKey(p_OldField))
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition      GridData not found OldField = " + p_OldField);
            return "";
        }

        List<GameObject> realObjectsOldField = _GamesObjectsReal[p_OldField];
        List<SaveLoadData.ObjectData> dataObjectsOldField = _GridData.FieldsD[p_OldField].Objects;

        if (realObjectsOldField == null)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** UpdatePosition     realObjectsOldField is Null !!!!");
            if (!_GamesObjectsReal.ContainsKey(p_OldField))
            {
                Debug.Log("********** UpdatePosition     in GamesObjectsReal not found OldField = " + p_OldField);
                return "";
            }
            else
            {
                _GamesObjectsReal[p_OldField] = new List<GameObject>();
            }
            return "";
        }

        //#TEST -----
        for (int i = realObjectsOldField.Count - 1; i >= 0;i--)
        {
            if (realObjectsOldField[i] == null)
            {
                Debug.Log("UGP: (" +  p_NameObject + ") ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
                Debug.Log("^^^^ UpfatePosition  -- remove destroy realObjects");
                realObjectsOldField.RemoveAt(i);
            }
        }
        //--------------

        int indReal = realObjectsOldField.FindIndex(p => p.name == p_NameObject);
        if (indReal == -1)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpfatePosition Not Real object (" + p_NameObject + ") in field: " + p_OldField);
            return "";
        }
        int indData = dataObjectsOldField.FindIndex(p => p.NameObject == p_NameObject);
        if (indData == -1)
        {
            //--------------------
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpfatePosition Not DATA object (" + p_NameObject + ") in field: " + p_OldField);
            foreach (var itemObj in dataObjectsOldField)
            {
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- object : " + itemObj.NameObject);
            }
            if (dataObjectsOldField.Count == 0)
                Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- objects ZERO !!!!!");
            //--------------------
            return "";
        }
        GameObject gobj = realObjectsOldField[indReal];
        if (gobj == null)
        {
            Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("^^^^ UpdatePosition      gobj is Destroy");
            return "";
        }

        //add to new Field
        if (!_GridData.FieldsD.ContainsKey(p_NewField))
        {
            //#!!!!  Debug.Log("SaveListObjectsToData GridData ADD new FIELD : " + posFieldReal);
            _GridData.FieldsD.Add(p_NewField, new SaveLoadData.FieldData());
        }

        if (p_newPosition != gobj.transform.position)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** ERROR UpdatePosition 1.     GOBJ UPDATE POSITOIN : NEW POS: " + p_newPosition + "       REAL POS: " + gobj.transform.position + "  REAL FIELD: " + Storage.GetNameFieldPosit(gobj.transform.position.x, gobj.transform.position.y));
            return "";
        }

        //Debug.Log("--------------------PRED NAME :" + objDataNow.NameObject);
        objData.NameObject = SaveLoadData.CreateName(objData.TagObject, p_NewField, "", p_NameObject);
        gobj.name = objData.NameObject;
        //Debug.Log("--------------------POST NAME :" + objDataNow.NameObject);

        //Debug.Log("UpdateGamePosition TEST POSITION GameObj ref: " + p_newPosition + "     GameObj realObjects: " + gobj.transform.position);

        //@POS@ Debug.Log("---- SET POS --- GO:" + gobj.name + "    DO:" + objData.NameObject);
        if (p_newPosition != gobj.transform.position)
        {
            Debug.Log("********** ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Debug.Log("********** ERROR UpdatePosition 2.     GOBJ UPDATE POSITOIN : NEW POS: " + p_newPosition + "       REAL POS: " + gobj.transform.position);
            return "";
        }
        objData.Position = gobj.transform.position;
        
        if (isDestroy)
            objData.IsReality = false;

        if (!_GamesObjectsReal.ContainsKey(p_NewField))
        {
            _GamesObjectsReal.Add(p_NewField, new List<GameObject>());
        }

        //add
        if (!isDestroy)
            _GamesObjectsReal[p_NewField].Add(gobj);
        _GridData.FieldsD[p_NewField].Objects.Add(objData);
        
        //remove
        dataObjectsOldField.RemoveAt(indData);
        realObjectsOldField.RemoveAt(indReal);

        //-------------------
        //int p_x = (int)objData.Position.x;
        //int p_y = (int)objData.Position.y;
        //int x = (int)(p_x / 2);
        //int y = (int)(p_y / 2);
        //string fieldR = FieldKey + (int)x + "x" + Mathf.Abs((int)y);
        //string strX = "  x: " + objData.Position.x + " -> " + p_x + " -> " + (p_x / 2) + " -> " + x;
        //string strY = "  y: " + objData.Position.y + " -> " + p_y + " -> " + (p_y / 2) + " -> " + y;
        //------------------

        //@POS@ Debug.Log("^^^^ UpfatePosition UPDATED...... : go:" + gobj.transform.position + "  DO:" + objData.Position + "  NPC:" + ((SaveLoadData.GameDataNPC)objData).Position + "  UFO:" + ((SaveLoadData.GameDataUfo)objData).Position + "     " + gobj.name);
        //Debug.Log("^^^^ UpfatePosition UPDATED......  >" + fieldR);

        return gobj.name;
    }

    //+FIX
    //public static void UpdateGamePosition(string p_OldField, string p_NewField, string p_NameObject, Vector3 p_newPosition)
    //{
    //    if (_GamesObjectsReal == null || _GamesObjectsReal.Count == 0)
    //    {
    //        Debug.Log("^^^^ UpfatePosition      GamesObjectsReal is EMPTY");
    //        return;
    //    }
    //    if (_GridData == null ||  _GridData.FieldsD ==null || _GridData.FieldsD.Count == 0)
    //    {
    //        Debug.Log("^^^^ UpfatePosition      GridData is EMPTY");
    //        return;
    //    }

    //    List<GameObject> realObjects = _GamesObjectsReal[p_OldField];
    //    List<SaveLoadData.ObjectData> dataObjects = _GridData.FieldsD[p_OldField].Objects;

    //    int indReal = realObjects.FindIndex(p => p.name == p_NameObject);
    //    if (indReal == -1)
    //    {
    //        Debug.Log("^^^^ UpfatePosition Not Real object (" + p_NameObject + ") in field: " + p_OldField);
    //        return;
    //    }
    //    int indData = dataObjects.FindIndex(p => p.NameObject == p_NameObject);
    //    if (indData == -1)
    //    {
    //        Debug.Log("^^^^ UpfatePosition Not DATA object (" + p_NameObject + ") in field: " + p_OldField);
    //        foreach (var itemObj in dataObjects)
    //        {
    //            Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- object : " + itemObj.NameObject);
    //        }
    //        if(dataObjects.Count==0)
    //            Debug.Log("^^^^ UpfatePosition IN DATA (" + p_OldField + ") --------- objects ZERO !!!!!");

    //        return;
    //    }
    //    GameObject gobj = realObjects[indReal];

    //    dataObjects.RemoveAt(indData);
    //    //add to new Field
    //    if (!_GridData.FieldsD.ContainsKey(p_NewField))
    //    {
    //        //#!!!!  Debug.Log("SaveListObjectsToData GridData ADD new FIELD : " + posFieldReal);
    //        _GridData.FieldsD.Add(p_NewField, new SaveLoadData.FieldData());
    //    }

    //    //@1.Create object and save person -> gobj
    //    SaveLoadData.ObjectData objData = SaveLoadData.CreateObjectData(gobj); //<< newObject.UpdateGameObject


    //    //Debug.Log("--------------------PRED NAME :" + objDataNow.NameObject);
    //    objData.NameObject = SaveLoadData.CreateName(objData.TagObject, p_NewField, "", p_NameObject);
    //    gobj.name = objData.NameObject;
    //    //Debug.Log("--------------------POST NAME :" + objDataNow.NameObject);

    //    objData.Position = gobj.transform.position; 

    //    _GridData.FieldsD[p_NewField].Objects.Add(objData);

    //    if (!_GamesObjectsReal.ContainsKey(p_NewField))
    //    {
    //        _GamesObjectsReal.Add(p_NewField, new List<GameObject>());
    //    }
    //    //Debug.Log("SaveListObjectsToData   MOVE RealObject -> " + posFieldOld + " -> " + posFieldReal + "  (Real PRED)  NewPos=" + GamesObjectsReal[posFieldReal].Count + " OldPos=" + GamesObjectsReal[posFieldOld].Count);
        

    //    //123456789
    //    //@PD@ PersonalData personData = gobj.GetComponent<PersonalData>();

    //    //personData.PersonalObjectData.Position = gobj.transform.position;
    //    //personData.PersonalObjectData = (SaveLoadData.ObjectData)objDataNow.Clone();

    //    //!!!!!!!!! !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //    //@PD@gobj.GetComponent<PersonalData>().PersonalObjectData = objDataNow;

    //    //987654321
    //    //@PD@ Debug.Log("^^^^ UpfatePosition  TEST----------------------  gobj PersonalObjectData=" + GamesObjectsReal[p_OldField][indReal].GetComponent<PersonalData>().PersonalObjectData.NameObject);

    //    _GamesObjectsReal[p_NewField].Add(gobj);

    //    realObjects.RemoveAt(indReal);

    //    //12345677899
    //    //LoadObjectToReal:  DATA -->> PERSONA #P#


    //    //Debug.Log("SaveListObjectsToData   MOVE RealObject -> " + posFieldOld + " -> " + posFieldReal + "  (Real NEW)  NewPos=" + GamesObjectsReal[posFieldReal].Count + " OldPos=" + GamesObjectsReal[posFieldOld].Count);

    //    //Debug.Log("^^^^ UpfatePosition UPDATED.......");
    //    //                                                                                                                                                    ((SaveLoadData.GameDataNPC)objUfo).                                                                          
    //    //-------------------
    //    int p_x = (int)objData.Position.x;
    //    int p_y = (int)objData.Position.y;
    //    int x = (int)(p_x / 2);
    //    int y = (int)(p_y / 2);
    //    string fieldR = FieldKey + (int)x + "x" + Mathf.Abs((int)y) ;
    //    string strX = "  x: " + objData.Position.x + " -> " + p_x + " -> "+ (p_x/2) + " -> " + x;
    //    string strY = "  y: " + objData.Position.y + " -> " + p_y + " -> " + (p_y / 2) + " -> " + y;
    //    //fieldR += strX + strY;
    //    //------------------

    //    Debug.Log("^^^^ UpfatePosition UPDATED...... " + gobj.name + " : go:" + gobj.transform.position + "  DO:" + objData.Position + "  NPC:" + ((SaveLoadData.GameDataNPC)objData).Position + "  UFO:" + ((SaveLoadData.GameDataUfo)objData).Position + " NEWPOS:" + ((SaveLoadData.GameDataNPC)objData).NewPosition);
    //    Debug.Log("^^^^ UpfatePosition UPDATED......  >" + fieldR );
    //    //@PD@ Debug.Log("^^^^ UpfatePosition UPDATED......  gobj PersonalObjectData=" + gobj.GetComponent<PersonalData>().PersonalObjectData.ToString());
        
    //}

    //private string GetNameFieldPosit_2(System.Single x, System.Single y)
    //{
    //    x = (int)(x / 2);
    //    y = (int)(y / 2);
    //    return FieldKey + (int)x + "x" + Mathf.Abs((int)y);
    //}

    

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
}
