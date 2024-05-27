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

    public override Vector3 GetBornPos()
    {
        Vector3 pos = playerMovement.transform.position;
        Vector3 spawnPosition = pos + playerMovement.orientation.forward * 2f;
        return new Vector3(spawnPosition.x, pos.y, spawnPosition.z);

    }
}
