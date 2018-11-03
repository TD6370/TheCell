using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public Color ColorCurrentField = Color.yellow;
    PlayerData m_playerDataGame;

    private void Awake()
    {
        m_playerDataGame = new PlayerData();
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

  
    public void SavePosition()
    {
        if (m_playerDataGame == null)
        {
            Debug.Log("############## LoadPlayerData is empty");
        }

        m_playerDataGame.SavePosition = Storage.PlayerController.transform.position;

        SavePlayerXML();
        Debug.Log("Save Position HERO : " + Storage.PlayerController.transform.position);
    }

    public void LoadPositionHero()
    {
        if (m_playerDataGame == null)
        {
            Debug.Log("############## LoadPlayerData is empty");
        }

        Debug.Log("Teleporting Hero....");


        //foreach(var fieldItem in Storage.Instance.Fields.Values)
        //{
        //    Destroy(fieldItem);
        //}
        //Storage.Instance.Fields.Clear();

        //Destroy Old Objects
        //Storage.Instance.DestroyAllGamesObjects();
        //Storage.Instance.Fields.Clear();
        Storage.Instance.StopGame();

        Storage.PlayerController.transform.position = m_playerDataGame.SavePosition;
        //Storage.PlayerController.Move(m_playerDataGame.SavePosition);
        

        Storage.GenGrid.StartGenGrigField(true);
        Storage.PlayerController.FindFieldCurrent();
        Storage.GenGrid.LoadObjectsNearHero();

        //------------
        Storage.PlayerController.transform.position = m_playerDataGame.SavePosition;
        Storage.GenGrid.StartGenGrigField(true);
        Storage.PlayerController.FindFieldCurrent();
        Storage.GenGrid.LoadObjectsNearHero();
        //---------------------

        Debug.Log("Teleported Hero ))");
    }

    public void RestructGrid()
    {
        var prefabFind =  Storage.PlayerController.FindFieldCurrent();
        if (prefabFind != null)
        {
            Storage.Events.ListLogAdd = prefabFind.name.ToString();
            Helper.GetNameFieldPosit(prefabFind.transform.position.x, prefabFind.transform.position.y);
            //Storage.Person.ColorFindCursorObject  Curent  Helper.GetNameFieldObject(prefabFind);
            //Storage.PlayerController.Cur
            //>>>>>> SetTextLog(prefabFind.name.ToString());
            Storage.Instance.SelectFieldPosHero = Helper.GetNameFieldObject(prefabFind);
            prefabFind.gameObject.GetComponent<SpriteRenderer>().color = ColorCurrentField;

            Storage.Map.DrawLocationHero();
        }
    }


}
