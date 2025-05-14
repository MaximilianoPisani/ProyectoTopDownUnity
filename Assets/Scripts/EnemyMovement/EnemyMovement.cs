using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class EnemyMovement : MonoBehaviour
{
    private IEnemyState _currentState;
    private Transform _target;
    private NavMeshAgent _agent;
    private AnimationControllerHandler _animHandler;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private AttackData _enemyAttackData;
    private bool _isDead = false;
    public Transform[] PatrolPoints => _patrolPoints; 
    public Transform CurrentTarget => _target;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animHandler = GetComponent<AnimationControllerHandler>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Start()
    {
        ChangeState(new EnemyPatrolState());
    }

    private void Update()
    {
        if (_isDead) return;

        _currentState?.UpdateState();
        AnimateMovement();
    }

    public void ChangeState(IEnemyState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void SetTarget(Transform target) => _target = target;
    public void RemoveTarget() => _target = null;
    public void SetMoving(bool isMoving) => _agent.isStopped = !isMoving;

    public void OnTargetEnterVision(Transform target)
    {
        SetTarget(target);
        ChangeState(new EnemyChaseState());
    }

    public void OnTargetExitVision()
    {
        RemoveTarget();
        ChangeState(new EnemyPatrolState());
    }

    private void AnimateMovement()
    {
        bool moving = _agent.velocity.magnitude > 0.1f;
        _animHandler.SetBool("Moving", moving);
       

        if (moving)
        {
            Vector2 vel = _agent.velocity.normalized;
            _animHandler.SetFloat("X", vel.x);
            _animHandler.SetFloat("Y", vel.y);
            _animHandler.SetFloat("LastX", vel.x);
            _animHandler.SetFloat("LastY", vel.y);
        }
    }
    public Transform[] GetPatrolPoints()
    {
        return _patrolPoints;
    }
    public void DisableMovement()
    {
        _agent.isStopped = true;
        _animHandler.SetBool("Moving", false);
    }

    public void MoveTo(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }
}