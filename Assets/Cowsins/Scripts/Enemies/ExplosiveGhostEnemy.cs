using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGhostEnemy : ZombieEnemy
{
    public override void GetChaseAniName()
    {
        base.GetChaseAniName();
        int random = UnityEngine.Random.Range(0, 4);
        if (random == 0)
        {
            ChaseAni = "run";
        }
        else if (random == 1)
        {
            ChaseAni = "walk";
        }
        else if (random == 2)
        {
            ChaseAni = "zombieRun";
        }
        else if (random == 3)
        {
            ChaseAni = "crawl";
            InGround = true;
        }
        else
        {
            ChaseAni = "zombieRun";
        }
    }
    
    public override Vector3 GetBornPos()
    {
        Vector3 pos = playerMovement.transform. position;
        Vector3 spawnPosition = pos + playerMovement.orientation.forward * 2f;
        return new Vector3(spawnPosition.x, pos.y, spawnPosition.z);

    }
}
