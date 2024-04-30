using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlEnemy : ZombieEnemy
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
        float randomX = 0;
        float randomY = 0;

        float dis = 10;
        Vector3 pos = playerMovement.transform.position;
        randomX = UnityEngine.Random.Range(pos.x - dis, pos.x + dis);
        randomY = UnityEngine.Random.Range(pos.z - dis , pos.z + dis);
        return new Vector3(randomX, pos.y, randomY);
    }
}
