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
    }

    public override void Update(float dt)
    {
        _zombie.transform.LookAt(_player.transform);
        _currentTime += dt;
        if (_currentTime > 2f * _zombie.GetAttackInterval())
        {
            SoundManager.Instance.PlaySound(_zombie.AttackClip, 0, 1, false, 0);
            _currentTime = 0;
            if (_zombie.GetDistance() > 2f)
            {
                _zombie.ChangeState(EnemyState.Chase);
            }
            else
            {
                if (_zombie.EnemyType == EnemyType.ExplosiveGhost)
                {
                    Vector3 explosiveGhostPosition = _zombie.GetPosition();
                    Debug.Log("ExplosiveGhost ����λ�ã�" + explosiveGhostPosition);
                    _player.DamageRatio(0.2f);
                    _zombie.PlayAni("attack2");
                    _zombie.Boom(explosiveGhostPosition);
                    _zombie.Die();
                }
                else
                {
                    _player.Damage(3);
                    _zombie.PlayAni("attack2");
                }
            }
        }
    }

    public EnemyAttackState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
    }
}
