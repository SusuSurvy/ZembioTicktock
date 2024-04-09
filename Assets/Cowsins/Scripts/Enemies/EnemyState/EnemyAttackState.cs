using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    public override void OnEnter()
    {
        _zombie.NavMeshAgent.enabled = false;
        base.OnEnter();
        _zombie.PlayAni("attack2");
    }

    public override void Update(float dt)
    {
        _currentTime += dt;
        if (_currentTime > 1)
        {
            _currentTime = 0;
            if (_zombie.GetDistance() > 2f)
            {
                _zombie.SetState(new EnemyChaseState(_player, _zombie));
            }
            else
            {
                _player.Damage(3);
                _zombie.PlayAni("attack2");
            }
        }

      
    }


    public EnemyAttackState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
    }
}
