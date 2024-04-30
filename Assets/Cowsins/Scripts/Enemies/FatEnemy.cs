using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatEnemy : ZombieEnemy
{
    public override void GetChaseAniName()
    {
        base.GetChaseAniName();
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            ChaseAni = "SpeedWalk";
        }
        else
        {
            ChaseAni = "Walk";
        }
    }
}
