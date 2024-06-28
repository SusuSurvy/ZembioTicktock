using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;
using UnityEngine.AI;
using System;

public enum EnemyState
{
    Idle = 0,
    Chase = 1,
    Attack = 2,
    BeAttack = 3,
    Die = 4,
}

public enum EnemyType
{
    Any = 0,
    Girl = 1,
    FatWomen = 2,
   // Remote = 3,
    ExplosiveGhost = 3,
    Boss = 4,
    //Doll = 6,

}

public class ZombieEnemy : EnemyHealth, IPoolable
{
    private Animator _animator;
    public PlayerMovement playerMovement;
    public AudioClip AttackClip;
    public AudioClip CreateClip;
    public NavMeshAgent NavMeshAgent;
    private float originalSpeed = 3.4f;
    private float crazySpeed = 4.7f;
    private float crazyTime = 0;
    private Color originalColor = new Color(154f / 255, 154f / 255, 154f / 255);
    private Color crazylColor = new Color(255f / 255, 117f / 255, 117f / 255);
    private float _attackIntervalRatio = 1f;
    private EnemyStateBase _currentState;
    private GameObject CrazyEffect;

    public EnemyType EnemyType;

    private Material _material;

    protected Vector3 heightCenter;

    public string ChaseAni;

    public bool InGround = false;

    public Vector3? OriginalBornPos;
    public BoxCollider Collider;
    public Vector3 GetTransCenter()
    {
        return transform.position + heightCenter;
    }

    // Start is called before the first frame update
    void CreateEnemy()
    {
        switch (EnemyType)
        {
            case EnemyType.Boss:
                health = GameDataInstance.Instance.GetBossHp();
                break;
          //  case EnemyType.Doll:
           //     health = GameDataInstance.Instance.GetDollHp();
          //      break;
            case EnemyType.Girl:
                health = GameDataInstance.Instance.GetGirlHp();
                break;
            case EnemyType.FatWomen:
                health = GameDataInstance.Instance.GetFatHp();
                break;
          //  case EnemyType.Remote:
          //      health = GameDataInstance.Instance.GetRemoteHp();
          //      break;
            case EnemyType.ExplosiveGhost:
                health = GameDataInstance.Instance.GetExplosiveGhostHp();
                break;
            default:
                health = maxHealth;
                break;
        }

        _attackIntervalRatio = 1f;
      
        maxHealth = health;
        InGround = false;
        BoxCollider collider = transform.GetComponent<BoxCollider>();
        Collider = collider;
        Collider.enabled = true;
        heightCenter = collider.center;
        _material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _animator.speed = 1f;
        NavMeshAgent.speed = originalSpeed;
        _material.color = originalColor;
        SetPosition(GetBornPos());
        transform.LookAt(playerMovement.transform);
        GetChaseAniName();
        InitOriginalState();
        CrazyEffect = transform.Find("CrazyEffect").gameObject;
        CrazyEffect.SetActive(false);
    }

    public virtual Vector3 GetBornPos()
    {
        return EnemyManager.Instance.GetRandomTransferPos();
        /*
        float randomX = 0;
        float randomY = 0;
      
        Vector3 pos = playerMovement.transform.position;
        randomX = UnityEngine.Random.Range(pos.x - 40, pos.x + 40);
        randomY = UnityEngine.Random.Range(pos.z - 40 , pos.z + 40);
        return new Vector3(randomX, pos.y, randomY);
        */
    }

    public virtual void InitOriginalState()
    {
        SetState(new EnemyChaseState(playerMovement, this));
    }

    public virtual void GetChaseAniName()
    {
        
    }



    public void SetPosition(Vector3 pos)
    {
        if (OriginalBornPos != null)
        {
            pos = OriginalBornPos.Value;
            OriginalBornPos = null;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 100, NavMesh.AllAreas) && NavMeshAgent.CalculatePath( playerMovement.transform.position, new NavMeshPath()))
        {
            NavMeshAgent.Warp(hit.position);
        }
        else
        {
            SetPosition(GetBornPos());
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
                _animator.speed = 1f;
                _attackIntervalRatio = 1f;
                CrazyEffect.SetActive(false);
            }
        }
    }

    public virtual float GetAttackDis()
    {
        return 1.5f;
    }

    public virtual void SetAttackState()
    {
        SetState(new EnemyAttackState(playerMovement, this));
    }
    
    public virtual void SetIdleState()
    {
        SetState( new DollIdleState(playerMovement, this));
    }
    
    public virtual void SetBeAttackState()
    {
        SetState( new EnemyBehitState(playerMovement, this));
    }
    
    public virtual void SetChaseState()
    {
        SetState( new EnemyChaseState(playerMovement, this));
    }
    
    public virtual void SetDieState()
    {
        SetState( new EnemyDieState(playerMovement, this));
    }

    public void ChangeState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Attack:
                SetAttackState();
                break;
            case EnemyState.Idle:
                SetIdleState();
                break;
            case EnemyState.BeAttack:
                SetBeAttackState();
                break;
            case EnemyState.Die:
                SetDieState();
                break;
            case EnemyState.Chase:
                SetChaseState();
                break;
        }
    }

    protected void SetState(EnemyStateBase state)
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
            Collider.enabled = false;
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

    public void CrazyEnemy(int count)
    {
        crazyTime += GameDataInstance.Instance.GetCrazyEnemyTime() * count;
        NavMeshAgent.speed = crazySpeed;
        _material.color = crazylColor;
        _animator.speed = 1.6f;
        _attackIntervalRatio = 0.4f;
        CrazyEffect.SetActive(true);
    }
    
    public float GetAttackInterval()
    {
        return _attackIntervalRatio;
    }

    public static implicit operator ZombieEnemy(GameObject v)
    {
        throw new NotImplementedException();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }


    public void Boom(Vector3 pos)
    {
        EnemyManager.Instance.Boom(pos);
    }

}
