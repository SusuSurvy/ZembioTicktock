using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class EnemyBehitState : EnemyStateBase
{
    public override void OnEnter()
    {
        _currentTime = 0;
        base.OnEnter();
        _zombie.NavMeshAgent.enabled = false;
        _zombie.PlayAni("hit1");
    }

    public EnemyBehitState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
    }

    public override void Update(float dt)
    {
        _currentTime += dt;
        if (_currentTime > 0.6f)
        {
            if (_zombie.GetDistance() < 1.5f)
            {
                _zombie.SetState(new EnemyAttackState(_player, _zombie));
            }
            else
            {
                _zombie.SetState(new EnemyChaseState(_player, _zombie));
            }
        }
    }
}
