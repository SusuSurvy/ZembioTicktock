using System.Collections;
using System.Collections.Generic;
using cowsins;
using UnityEngine;

public class RemoteAttackState : EnemyStateBase
{
    public RemoteAttackState(PlayerMovement player, ZombieEnemy zombie) : base(player, zombie)
    {
    }
    
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
        if (_currentTime > 2.5f)
        {
            EnemyManager.Instance.CreatBullet(_zombie.transform.position + new Vector3(0, 1, 0));
            _currentTime = 0;
            if (_zombie.GetDistance() > _zombie.GetAttackDis() || !CanAttack(_zombie.GetTransCenter(), _player.GetTransCenter()))
            {
                _zombie.ChangeState(EnemyState.Chase);
            }
            else
            {
                _zombie.PlayAni("attack2");
                
            }
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
