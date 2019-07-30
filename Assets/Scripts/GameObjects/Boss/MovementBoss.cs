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
        _dataNPC = GetUpdateData(callFunc) as ModelNPC.GameDataBoss;
        objID = Helper.GetID(this.name);
    }

    protected override void StartMoving()
    {
        moveObject = StartCoroutine(MoveObjectToPosition<ModelNPC.GameDataBoss>());
    }
}
