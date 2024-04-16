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
    private float originalSpeed = 3.4f;
    private float crazySpeed = 4.7f;
    private float crazyTime = 0;
    private Color originalColor = new Color(154f / 255, 154f / 255, 154f / 255);
    private Color crazylColor = new Color(255f / 255, 117f / 255, 117f / 255);
    private EnemyStateBase _currentState;

    private Material _material;
    // Start is called before the first frame update
    void CreateEnemy()
    {
        _material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        NavMeshAgent.speed = originalSpeed;
        SetState(new EnemyChaseState(playerMovement, this));
        _material.color = originalColor;
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
        if (crazyTime > 0)
        {
            crazyTime -= Time.deltaTime;
            if (crazyTime < 0)
            {
                NavMeshAgent.speed = originalSpeed;
                _material.color = originalColor;
            }
        }
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
        _animator.Play(aniName, -1, 0f);
        _animator.Update(0);
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
        gameObject.SetActive(true);
        CreateEnemy();
    }

    public void OnDespawn()
    {
       
        gameObject.SetActive(false);
    }

    public void CrazyEnemy()
    {
        crazyTime = 5;
        NavMeshAgent.speed = crazySpeed;
        _material.color = crazylColor;
    }
}
