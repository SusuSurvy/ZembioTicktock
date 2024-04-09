using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;
using UnityEngine.AI;
public class ZombieEnemy : EnemyHealth, IPoolable
{
    private Animator _animator;
    public PlayerMovement playerMovement;

    public NavMeshAgent NavMeshAgent;

    private EnemyStateBase _currentState;
    // Start is called before the first frame update
    void CreateEnemy()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        SetState(new EnemyChaseState(playerMovement, this));
    }
    
    public void SetPosition(Vector3 pos)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 100, NavMesh.AllAreas))
        {
            NavMeshAgent.Warp(hit.position);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        _currentState?.Update(Time.deltaTime);
    }
    
    public void SetState(EnemyStateBase state)
    {
        _currentState?.OnExit();
        _currentState = state;
        _currentState?.OnEnter();
    }
    public float GetDistance()
    {
        return Vector3.Distance(transform.position, playerMovement.transform.position);
    }

    public void PlayAni(string aniName)
    {
        _animator.Play(aniName);
    }

    public override void Damage(float _damage)
    {
        base.Damage(_damage);
        if (isDead)
        {
            if (!(_currentState is EnemyDieState))
            {
                SetState(new EnemyDieState(playerMovement, this));
            }
        }
        else
        {
            SetState(new EnemyBehitState(playerMovement, this));
        }
    }

    public override void Die()
    {
        SetState(null);
        EnemyManager.Instance.Despawn(this);
    }

    public void OnSpawn()
    {
        CreateEnemy();
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
       
        gameObject.SetActive(false);
    }
}
