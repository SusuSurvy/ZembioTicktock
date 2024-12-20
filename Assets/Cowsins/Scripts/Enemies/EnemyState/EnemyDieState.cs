using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class EnemyDieState : EnemyStateBase
{
    public override void OnEnter()
    {
        _currentTime = 0;
        base.OnEnter();
        _zombie.NavMeshAgent.isStopped = true;
        _zombie.PlayAni("fallToFace");
    }

    public override void Update(float dt)
    {
        _currentTime += dt;
        if (_currentTime > 2.5f)
        {
            _zombie.Die();
        }
    }
    public EnemyDieState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
        
    }
}
