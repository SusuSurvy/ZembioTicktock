using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;
using UnityEngine.AI;
public class ZombieController : MonoBehaviour
{
    private Animator _animator;
    public PlayerMovement playerMovement;

    public NavMeshAgent NavMeshAgent;

    private EnemyStateBase _currentState;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        SetState(new EnemyChaseState(playerMovement, this));
    }

    // Update is called once per frame
    void Update()  
    {
        _currentState?.Update(Time.deltaTime);
    }
    
    public void SetState(EnemyStateBase state)
    {
        _currentState?.OnExit();
        _currentState = state;
        _currentState.OnEnter();
    }

    public float GetDistance()
    {
        return Vector3.Distance(transform.position, playerMovement.transform.position);
    }

    public void PlayAni(string aniName)
    {
        _animator.Play(aniName);
    }
}
