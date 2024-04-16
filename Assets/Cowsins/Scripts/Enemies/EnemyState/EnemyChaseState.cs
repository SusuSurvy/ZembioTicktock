using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    public override void OnEnter()
    {
        _zombie.NavMeshAgent.enabled = true;
        base.OnEnter();
        int random = UnityEngine.Random.Range(0, 4);
        if (random == 0)
        {
            _zombie.PlayAni("run");
        }
        else if (random == 1)
        {
            _zombie.PlayAni("walk");
        }
        else if (random == 2)
        {
            _zombie.PlayAni("zombieRun");
        }
        else if (random == 3)
        {
            _zombie.PlayAni("crawl");
        }
    }

    public override void Update(float dt)
    {
        _zombie.NavMeshAgent.destination = _player.transform.position;
        if (_zombie.GetDistance() < 1.5f)
        {
            _zombie.SetState(new EnemyAttackState(_player, _zombie));
        }
    }


    public EnemyChaseState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
    }
}
