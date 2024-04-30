using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    public override void OnEnter()
    {
        _zombie.NavMeshAgent.isStopped = true;
        base.OnEnter();
        _zombie.PlayAni("attack2");
        SoundManager.Instance.PlaySound(_zombie.AttackClip, 0, 0, false, 0);
    }

    public override void Update(float dt)
    {
        _zombie.transform.LookAt(_player.transform);
        _currentTime += dt;
        if (_currentTime > 2f)
        {
            _currentTime = 0;
            if (_zombie.GetDistance() > 2f)
            {
                _zombie.ChangeState(EnemyState.Chase);
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
