using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private bool m_HeroExtremal = false;
    public bool HeroExtremal
    {
        get
        {
            return m_HeroExtremal;
        }
        set
        {
            m_HeroExtremal = value;
            HeroExtremalChangeOn();
        }
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

    public Color ColorCurrentField = Color.yellow;
    PlayerData m_playerDataGame;

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


    private void Awake()
    {
        m_playerDataGame = new PlayerData();

        var collider = Storage.Instance.HeroObject.GetComponent<CapsuleCollider2D>();
    }


    // Use this for initialization
    void Start () {
        //LoadPlayerData();
        
    }

    public void Init()
    {
        LoadPlayerData();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void HeroExtremalChangeOn()
    {
        // m_HeroExtremal
        var hero = Storage.Instance.HeroObject;

        //if (Storage.PlayerController.CameraMap.enabled)
        if(Storage.Map.IsOpen)
            return;
        //hero.camera
        //if (CameraMap.enabled)

        //var collider = hero.GetComponent<CapsuleCollider2D>();
        //if(collider==null)
        //{
        //    Debug.Log("###### HeroExtremalChangeOn CapsuleCollider2D is empty");
        //    return;
        //}

        //collider.enabled = !m_HeroExtremal;
        if (m_HeroExtremal)
            Storage.PlayerController.GhostOn();
        else
            Storage.PlayerController.GhostOff();

        float _alpha = m_HeroExtremal ? 0.5f : 1f;
        //hero.SetAlpha(_alpha);
        if (m_HeroExtremal)
            //hero.GetComponent<SpriteRenderer>().color = new Color(179f, 236f, 255f, _alpha);
            hero.GetComponent<SpriteRenderer>().color = new Color(0f, 0.6f, 0.95f, _alpha);
        //hero.GetComponent<Texture2D>().color = new Color(179, 236, 255, _alpha);
        else
            hero.GetComponent<SpriteRenderer>().color = Color.white;
        //hero.GetComponent<SpriteRenderer>().color = m_HeroExtremal ? "#80D3F6".ToColor() : Color.white;
        //hero.GetComponent<RenderTexture>()
        //if (collider.enabled)
        //{
            

        //}
    }

    public void SavePlayerXML()
    {
        Debug.Log("Saving player data....");
        string path = Storage.Instance.DataPathPlayer;
        Serializator.SaveXml<PlayerData>(m_playerDataGame, path, true);
    }

    

    public void LoadPlayerData()
    {
        Debug.Log("Loading player data....");
        m_playerDataGame = Serializator.LoadXml<PlayerData>(Storage.Instance.DataPathPlayer);
        if (m_playerDataGame == null)
        {
            Debug.Log("############## LoadPlayerData is empty : path=" + Storage.Instance.DataPathPlayer);
            m_playerDataGame = new PlayerData();
        }
    }


    public void SavePosition(int x = -1, int y = -1)
    {
        if (m_playerDataGame == null)
        {
            Debug.Log("############## LoadPlayerData is empty");
        }

        if (x != -1 && y != -1)
        {
            m_playerDataGame.SavePosition = new Vector3(x, y, Storage.PlayerController.transform.position.z);
        } else
        { 
            m_playerDataGame.SavePosition = Storage.PlayerController.transform.position;
        }

        SavePlayerXML();
        Debug.Log("Save Position HERO : " + Storage.PlayerController.transform.position);
    }

    public void TeleportHero()
    {
        Storage.Map.Create();
        int posTransferHeroX = (int)(Storage.Map.SelectPointField.x * Storage.ScaleWorld);
        int posTransferHeroY = (int)(Storage.Map.SelectPointField.y * Storage.ScaleWorld);
        posTransferHeroY *= -1;
        Storage.Player.TeleportHero(posTransferHeroX, posTransferHeroY);
    }


    public void TeleportHero(int x , int y)
    {
        SavePosition(x, y);
        LoadPositionHero();
    }

    public void LoadPositionHero()
    {
        if (m_playerDataGame == null)
        {
            Debug.Log("############## LoadPlayerData is empty");
        }

        Debug.Log("Teleporting Hero....");

        Storage.Instance.StopGame();
        Storage.PlayerController.transform.position = m_playerDataGame.SavePosition;
        Storage.PlayerController.FindFieldCurrent();
        Storage.GenGrid.StartGenGrigField(true);
        Storage.GenGrid.LoadObjectsNearHero();

        Debug.Log("Teleported Hero ))");
    }

    public void RestructGrid()
    {
        var prefabFind =  Storage.PlayerController.FindFieldCurrent();
        if (prefabFind != null)
        {
            Storage.EventsUI.ListLogAdd = prefabFind.name.ToString();
            Helper.GetNameFieldPosit(prefabFind.transform.position.x, prefabFind.transform.position.y);
            //>>>>>> SetTextLog(prefabFind.name.ToString());
            Storage.Instance.SelectFieldPosHero = Helper.GetNameFieldObject(prefabFind);
            prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorCurrentField;
            Storage.Map.DrawLocationHero();
        }
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

            Storage.Instance.SetZonaField(fX, fY, fX2, fY2);
           
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
            int LevelX = Helper.WidthLevel * scale;
            int LevelY = Helper.HeightLevel * scale;

            float rX2 = xH + (limitH * scale);
            float rY2 = yH - (limitV * scale);
            if (rX2 > LevelX) rX2 = LevelX;
            if (rY2 > LevelY) rY2 = LevelY;

            Storage.Instance.SetZonaRealLook(rX, rY, rX2, rY2);
            //if(DrawGeom!=null)
            //    DrawGeom.DrawRect(rX, rY, rX2, rY2);
        }
    }
}
