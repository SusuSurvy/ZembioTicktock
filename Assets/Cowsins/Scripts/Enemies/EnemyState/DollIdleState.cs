using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class DollIdleState : EnemyChaseState
{
    public override void OnEnter()
    {
        _zombie.NavMeshAgent.isStopped = false;
        base.OnEnter();
        _zombie.PlayAni("Idle");
        _zombie.transform.LookAt(_player.transform);
        //int random = UnityEngine.Random.Range(0, 10);
       // if (random == 0)
       // {
            SoundManager.Instance.PlaySound(_zombie.CreateClip, 0, 1, false, 0);
        //}
    }
    
    public DollIdleState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
    }
    

    public override void Update(float dt)
    {
        
    }
}
