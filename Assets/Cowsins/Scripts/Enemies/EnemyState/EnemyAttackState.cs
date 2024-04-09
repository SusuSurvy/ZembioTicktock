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
        if (_zombie.GetDistance() > 2f)
        {
            _zombie.SetState(new EnemyChaseState(_player, _zombie));
        }
    }


    public EnemyAttackState(PlayerMovement player, ZombieController zombie) : base(player, zombie)
    {
    }
}
