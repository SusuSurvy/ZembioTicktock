using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class BossAttackState : EnemyStateBase
{
    public BossAttackState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
        _zombie.deathEffect?.SetActive(true);
        _zombie.NavMeshAgent.isStopped = true;
        base.OnEnter();
        _zombie.PlayAni("attack2");
        SoundManager.Instance.PlaySound(_zombie.AttackClip, 0, 0, false, 0);
    }

    public override void Update(float dt)
    {
        _zombie.transform.LookAt(_player.transform);
        _currentTime += dt;
        if (_currentTime > 4f)
        {
            _currentTime = 0;
            EnemyManager.Instance.CreateEnemy(EnemyType.Any, _zombie.transform.position + _zombie.transform.forward);
    
            if (_zombie.GetDistance() > _zombie.GetAttackDis())
            {
               
                _zombie.ChangeState(EnemyState.Chase);
            }
            else
            {
                _zombie.PlayAni("attack2");
             
            }
        }
    }

    public override void OnExit()
    {
        _zombie.deathEffect?.SetActive(false);
        base.OnExit();
    }
}
