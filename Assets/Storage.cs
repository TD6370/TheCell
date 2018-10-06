using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour {

    public static ZonaFieldLook ZonaField { get; set; }
    public static ZonaRealLook ZonaReal { get; set; }

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

	// Use this for initialization
	void Start () {
        ZonaField = null;
        ZonaReal = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetHeroPosition(int x, int y, float xH, float yH)
    {
        int scale = 2;
        _heroPositionX = x;
        _heroPositionY = y;

        int _limitX = _limitHorizontalLook / 2;
        int _limitY = _limitVerticalLook / 2;

        int fX = x - _limitX;
        int fY = y - _limitY;
        //int fX2 = x + _limitHorizontalLook;
        //int fY2 = y + _limitVerticalLook;
        if (fX < 0) fX = 0;
        if (fY < 0) fY = 0;
        int fX2 = fX + _limitHorizontalLook;
        int fY2 = y + _limitVerticalLook;

        ZonaField = new ZonaFieldLook() 
        {
            X = fX, 
            Y = fY,
            X2 = fX2,
            Y2 = fY2
        };
        Debug.Log("ZonaField: X:" + ZonaField.X + " Y:" + ZonaField.Y + " X2:" + ZonaField.X2 + " Y2:" + ZonaField.Y2);

        float rX = xH - (_limitX * scale);
        float rY = yH + (_limitY * scale);
        //float rX2 = xH + (_limitHorizontalLook * scale);
        //float rY2 = yH - (_limitVerticalLook * scale);
        if (rX < 0) rX = 0.5f;
        if (rY > 0) rY = -0.5f;
        float rX2 = rX + (_limitHorizontalLook * scale);
        float rY2 = yH - (_limitVerticalLook * scale);

        ZonaReal = new ZonaRealLook()
        {
            X = rX,
            Y = rY,
            X2 = rX2,
            Y2 = rY2
        };
        Debug.Log("ZonaReal: X:" + ZonaReal.X + " Y:" + ZonaReal.Y + " X2:" + ZonaReal.X2 + " Y2:" + ZonaReal.Y2);
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
}
