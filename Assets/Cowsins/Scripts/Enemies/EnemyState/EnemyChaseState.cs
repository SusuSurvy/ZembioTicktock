using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    public override void OnEnter()
    {
        _zombie.NavMeshAgent.isStopped = false;
        base.OnEnter();
        _zombie.PlayAni(_zombie.ChaseAni);
        int random = UnityEngine.Random.Range(0, 10);
        if (random == 0)
        {
            SoundManager.Instance.PlaySound(_zombie.CreateClip, 0, 0, false, 0);
        }
    }

    public override void Update(float dt)
    {
        _zombie.NavMeshAgent.destination = _player.transform.position;
        if (_zombie.GetDistance() < _zombie.GetAttackDis())
        {
            _zombie.ChangeState(EnemyState.Attack);
        }
    }


    public EnemyChaseState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
    }
}
