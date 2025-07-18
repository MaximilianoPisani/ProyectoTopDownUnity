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
    [SerializeField] private float _prepareAttackDuration = 0.5f; 
    public AnimationControllerHandler animHandler { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public Transform currentTarget { get; private set; }
    public IAttackStrategy attackStrategy { get; private set; }

    private EnemyStateMachine _stateMachine;
    private bool _isMovementEnabled = true;
    private bool _isMoving;
    private bool _hasSeenPlayer = false;
    private Vector2 _lastLookDirection = Vector2.up;



    public Vector2 LastLookDirection => _lastLookDirection;
    public bool HasSeenPlayer => _hasSeenPlayer;
    public bool shouldChasePlayer => _shouldChasePlayer;
    public float PrepareAttackDuration => _prepareAttackDuration;

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
    public void StartAttackSequence()
    {
        var stateMachine = GetComponent<EnemyStateMachine>();

        if (stateMachine.GetCurrentState() is EnemyPrepareAttackState ||
            stateMachine.GetCurrentState() is EnemyAttackState)
            return;

        stateMachine.ChangeState(new EnemyPrepareAttackState(_prepareAttackDuration));
    }

    public void SetShouldChasePlayer(bool value)
    {
        _shouldChasePlayer = value;

        if (_shouldChasePlayer)
            ResumeMovement();
        else
            PauseMovement();
    }

    public bool ShouldChasePlayer
    {
        get => _shouldChasePlayer;
        set => _shouldChasePlayer = value;
    }

    public void UpdateLookDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            _lastLookDirection = direction.normalized;
        }
    }
    public void PauseMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        SetMoving(false);
    }

    public void ResumeMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }
    public void StopMovement()
    {
        _isMovementEnabled = false;
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        SetMoving(false);
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
        StartAttackSequence();
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
