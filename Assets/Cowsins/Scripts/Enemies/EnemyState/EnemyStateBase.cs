using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public abstract  class EnemyStateBase
{
    protected float _currentTime = 0;
    protected ZombieEnemy _zombie;
    protected PlayerMovement _player;

    public EnemyStateBase(PlayerMovement player, ZombieEnemy zombie)
    {
        _player = player;
        _zombie = zombie;
    }
    
    public abstract void Update(float dt);
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}
