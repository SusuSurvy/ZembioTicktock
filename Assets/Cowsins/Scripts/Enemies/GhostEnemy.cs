using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : ZombieEnemy
{
    public override void GetChaseAniName()
    {
        base.GetChaseAniName();

            ChaseAni = "run";


    }

}
