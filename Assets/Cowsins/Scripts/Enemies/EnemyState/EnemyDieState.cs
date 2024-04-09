using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class EnemyDieState : EnemyStateBase
{
    private float _currentTime;
    public override void OnEnter()
    {
        _currentTime = 0;
        base.OnEnter();
        _zombie.NavMeshAgent.enabled = false;
        _zombie.PlayAni("fallToFace");
    }

    public override void Update(float dt)
    {
        _currentTime += dt;
        if (_currentTime > 1.5f)
        {
            _zombie.Die();
        }
    }
    public EnemyDieState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
        
    }
}
