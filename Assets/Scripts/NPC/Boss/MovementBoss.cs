using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBoss : MovementNPC
{

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void UpdateData(string callFunc)
    {
        _dataNPC = FindObjectData<ModelNPC.GameDataBoss>(callFunc);// as SaveLoadData.GameDataNPC;
    }

    protected override void StartMoving()
    {
        moveObject = StartCoroutine(MoveObjectToPosition<ModelNPC.GameDataBoss>());
    }
}
