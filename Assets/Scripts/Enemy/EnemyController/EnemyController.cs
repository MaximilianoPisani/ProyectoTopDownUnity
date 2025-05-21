using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 
public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent { get; private set; }
    public Transform currentTarget { get; private set; }
    public AnimationControllerHandler animHandler;
    private IEnemyState _currentState;
    private bool _isMovementEnabled = true;
    private bool _isMoving;
    [SerializeField] private Transform[] _patrolPoints;
    private bool _hasSeenPlayer = false;
    [SerializeField] private float _patrolWaitDuration = 1.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animHandler = GetComponent<AnimationControllerHandler>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        ChangeState(new EnemyPatrolState()); // Start in Patrol state
    }
    private void Update()
    {
       if (_isMovementEnabled && _currentState != null) // If movement is enabled, update the current state logic
        {
           _currentState.UpdateState();
       }
    }
    public float GetPatrolWaitDuration() // Returns how long the enemy waits at patrol points
    {
        return _patrolWaitDuration;
    }
    public void OnTargetEnterVision(Transform target) // Called when a target (the player) enters the enemy’s vision
    {
        currentTarget = target;
        _hasSeenPlayer = true; 
        ChangeState(new EnemyChaseState());
    }
    public void OnTargetExitVision() // Called when the target exits vision
    {
        currentTarget = null;

        agent.ResetPath(); 
        agent.velocity = Vector3.zero; 

        if (_hasSeenPlayer)
        {
            ChangeState(new EnemyIdleState());
        }
        else
        {
            ChangeState(new EnemyPatrolState());
        }
    }

    public void ChangeState(IEnemyState newState) // Changes the enemy’s current state using the state pattern
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState(this);
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
            animHandler.SetBool("isAttacking", false);
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