using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData
{
    public bool IsLoadPosition { get; set; }
    public Vector3 SavePosition { get; set; }
    public PlayerData()
    {
        IsLoadPosition = false;
        SavePosition = new Vector3(10, 10, -3);
    }

}
