using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class  EnemyBehitState : EnemyStateBase
{
    public override void OnEnter()
    {
        _currentTime = 0;
        base.OnEnter();
        _zombie.NavMeshAgent.isStopped = true;
        if (!_zombie.InGround)
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
            {
                _zombie.PlayAni("hit1"); 
            }
            else
            {
                _zombie.PlayAni("hit2"); 
            }
           
        }
    }

    public EnemyBehitState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
    }

    public override void Update(float dt)
    {
        _currentTime += dt;
        if (_currentTime > 0.6f)
        {
            _zombie.ChangeState(EnemyState.Chase);
        }
    }
}
