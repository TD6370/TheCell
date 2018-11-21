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

    public virtual ModelNPC.GameDataBoss GetUpdateData(string callFunc)
    {
        var _dataBoss = FindObjectData<ModelNPC.GameDataBoss>(callFunc);// as SaveLoadData.GameDataNPC;
        return _dataBoss;
    }

    protected override void StartMoving()
    {
        moveObject = StartCoroutine(MoveObjectToPosition<ModelNPC.GameDataBoss>());
    }
}
