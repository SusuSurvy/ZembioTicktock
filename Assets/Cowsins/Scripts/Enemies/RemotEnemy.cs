using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotEnemy : ZombieEnemy
{
    public override float GetAttackDis()
    {
        return 6f;
    }

    public override void SetAttackState()
    {
        SetState(new RemoteAttackState(playerMovement, this));
    }

    public override void SetChaseState()
    {
        SetState(new RemoteChaseState(playerMovement, this));
    }
    
    public override void GetChaseAniName()
    {
        base.GetChaseAniName();
        ChaseAni = "Walk";
    }
}
