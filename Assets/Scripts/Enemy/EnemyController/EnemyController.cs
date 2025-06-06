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
    private IEnemyState _currentState;

    private bool _isMovementEnabled = true;
    private bool _isMoving;
    private bool _hasSeenPlayer = false;

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
        if (factory == null)
        {
            Debug.LogError("Could not create factory for " + gameObject.name);
            return;
        }

       
        attackStrategy = factory is MeleeEnemyFactory
            ? new MeleeAttackStrategy()
            : new RangedAttackStrategy();

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
    public IAttackStrategy GetAttackStrategy()
    {
        return attackStrategy;
    }
    public void SetAttackStrategy(IAttackStrategy strategy)
    {
        attackStrategy = strategy;
        attackStrategy = strategy;
    }
    public float GetPatrolWaitDuration() // Returns how long the enemy waits at patrol points
    {
        return _patrolWaitDuration;
    }
    public void OnTargetEnterVision(Transform target) // Called when a target (the player) enters the enemy’s vision
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
    public void OnTargetExitVision() // Called when the target exits vision
    {
        currentTarget = null;
        _hasSeenPlayer = true;

        _stateMachine.ChangeState(new EnemyIdleState());
    }
    public void SetTarget(Transform target) // Set the current target manually
    {
        currentTarget = target;
    }

    public void MoveTo(Vector3 destination) // Orders the enemy to move to a specific location using NavMesh
    {
        if (_isMovementEnabled)
            agent.SetDestination(destination);
    }

    public void SetEnemyActive(bool isActive) // Enables or disables enemy movement and animation
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

    public void SetMoving(bool isMoving) // Used to control if the enemy is currently moving
    {
        _isMoving = isMoving;
    }
   

    public Transform[] GetPatrolPoints() // Returns the patrol points assigned to this enemy
    {
        return _patrolPoints;
    }
}