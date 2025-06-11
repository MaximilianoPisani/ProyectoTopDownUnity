using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 
public class EnemyController : MonoBehaviour
{
    public EnemyTypeData enemyTypeData;

    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _patrolWaitDuration = 1.5f;
    [SerializeField] private bool _shouldChasePlayer = true;

    public AnimationControllerHandler animHandler { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public Transform currentTarget { get; private set; }
    public IAttackStrategy attackStrategy { get; private set; }

    private EnemyStateMachine _stateMachine;
    private bool _isMovementEnabled = true;
    private bool _isMoving;
    private bool _hasSeenPlayer = false;
    public bool HasSeenPlayer => _hasSeenPlayer;
    public bool shouldChasePlayer => _shouldChasePlayer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animHandler = GetComponent<AnimationControllerHandler>();
        _stateMachine = GetComponent<EnemyStateMachine>();

        if (agent == null)
            Debug.LogError("Missing NavMeshAgent component.");
        if (animHandler == null)
            Debug.LogError("Missing AnimationControllerHandler component.");
        if (_stateMachine == null)
            Debug.LogError("Missing EnemyStateMachine component.");
        if (enemyTypeData == null)
        {
            Debug.LogError("EnemyTypeData not assigned in " + gameObject.name);
            return;
        }

        var factory = enemyTypeData.CreateFactory();

        if (factory is MeleeEnemyFactory)
        {
            _shouldChasePlayer = true; 
            attackStrategy = new MeleeAttackStrategy(this);
        }
        else if (factory is RangedEnemyFactory)
        {
            _shouldChasePlayer = false; 
            attackStrategy = new RangedAttackStrategy(this);
        }
        else if (factory is BossEnemyFactory)
        {
            _shouldChasePlayer = true; 
            attackStrategy = new ComboAttackStrategy(this);
        }
        else
        {
            Debug.LogError("Unknown factory type for " + gameObject.name);
        }

        SetAttackStrategy(attackStrategy);


        var initialState = factory.CreateInitialState(this);
        if (initialState == null)
        {
            Debug.LogError("Initial state is null for " + gameObject.name);
            return;
        }

        _stateMachine.SetInitialState(initialState);

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (_isMovementEnabled)
        {
            _stateMachine.UpdateState();
        }
    }

    public IAttackStrategy GetAttackStrategy() => attackStrategy;

    public void SetAttackStrategy(IAttackStrategy strategy)
    {
        attackStrategy = strategy;
    }

    public float GetPatrolWaitDuration() => _patrolWaitDuration;

    public void OnTargetEnterVision(Transform target)
    {
        currentTarget = target;

        if (_shouldChasePlayer)
        {
            _stateMachine.ChangeState(new EnemyChaseState());
        }
        else
        {
            _stateMachine.ChangeState(new EnemyAttackState());
        }
    }

    public void OnTargetExitVision()
    {
        currentTarget = null;
        _hasSeenPlayer = true;

        _stateMachine.ChangeState(new EnemyIdleState());
    }

    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    public void MoveTo(Vector3 destination)
    {
        if (_isMovementEnabled)
            agent.SetDestination(destination);
    }

    public void SetEnemyActive(bool isActive)
    {
        _isMovementEnabled = isActive;
        agent.isStopped = !isActive;

        if (!isActive)
        {
            agent.velocity = Vector3.zero;
            animHandler.SetBool("Moving", false);

            if (animHandler.HasParameter("isAttackingMelee"))
                animHandler.SetBool("isAttackingMelee", false);

            if (animHandler.HasParameter("isAttackingRange"))
                animHandler.SetBool("isAttackingRange", false);
        }
        else
        {
            animHandler.SetBool("Moving", _isMoving);
        }
    }

    public void ResetTarget()
    {
        currentTarget = null;
        _hasSeenPlayer = false;
    }

    public void SetMoving(bool isMoving)
    {
        _isMoving = isMoving;
    }

    public Transform[] GetPatrolPoints() => _patrolPoints;
}
