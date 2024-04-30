using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class RemoteChaseState : EnemyStateBase
{
    public RemoteChaseState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
    }

    public override void OnEnter()
    {
        _zombie.NavMeshAgent.isStopped = false;
        base.OnEnter();
        _zombie.PlayAni(_zombie.ChaseAni);
    }
    
    public override void Update(float dt)
    {
        _zombie.NavMeshAgent.destination = _player.transform.position;
        if (_zombie.GetDistance() < _zombie.GetAttackDis() && CanAttack(_zombie.GetTransCenter(), _player.GetTransCenter()))
        {
            _zombie.ChangeState(EnemyState.Attack);
        }
    }
    
    private bool CanAttack(Vector3 source, Vector3 target)
    {
        Vector3 direction = target - source;
        float dis = Vector3.Distance(source, target);
        RaycastHit[] hits = Physics.RaycastAll(source, direction, dis + 1);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.name.Contains("Wall") || hit.collider.gameObject.name.Contains("Floor") || hit.collider.gameObject.name.Contains("Stairs"))
            {
                Debug.LogError("attack can not ,碰到了墙");
                return false;
            }
        }
        return true;
    }
}
