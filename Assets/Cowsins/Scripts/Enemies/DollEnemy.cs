using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollEnemy : ZombieEnemy
{
    public override void InitOriginalState()
    {
        ChangeState(EnemyState.Idle);
    }
    
    public override Vector3 GetBornPos()
    {
        float randomX = 0;
        float randomY = 0;

        float dis = 3;
        Vector3 pos = playerMovement.transform.position;
        randomX = UnityEngine.Random.Range(pos.x - dis, pos.x + dis);
        randomY = UnityEngine.Random.Range(pos.z - dis , pos.z + dis);
        return new Vector3(randomX, pos.y, randomY);
    }
}
