using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 
public class EnemyController : MonoBehaviour
{

    public NavMeshAgent agent { get; private set; }
    public Transform currentTarget { get; private set; }
    public AnimationControllerHandler animHandler;

    private EnemyStateMachine _stateMachine;

    private bool _isMovementEnabled = true;
    private bool _isMoving;
    private bool _hasSeenPlayer = false;

    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _patrolWaitDuration = 1.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError("Is missing a NavMeshAgent component ");

        animHandler = GetComponent<AnimationControllerHandler>();
        if (animHandler == null)
            Debug.LogError("Is missing an AnimationControllerHandler component ");

        _stateMachine = GetComponent<EnemyStateMachine>();
        if (_stateMachine == null) 
            Debug.LogError("Missing EnemyStateMachine");

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        _stateMachine.ChangeState(new EnemyPatrolState()); // Start in Patrol state
    }

    private void Update()
    {
        if (_isMovementEnabled)
        {
            _stateMachine.UpdateState();
        }
    }
    public float GetPatrolWaitDuration() // Returns how long the enemy waits at patrol points
    {
        return _patrolWaitDuration;
    }
    public void OnTargetEnterVision(Transform target) // Called when a target (the player) enters the enemy�s vision
    {
        currentTarget = target;
        _hasSeenPlayer = true;
        _stateMachine.ChangeState(new EnemyChaseState());
    }
    public void OnTargetExitVision() // Called when the target exits vision
    {
        currentTarget = null;

        agent.ResetPath(); 
        agent.velocity = Vector3.zero;

        if (_hasSeenPlayer)
            _stateMachine.ChangeState(new EnemyIdleState());
        else
            _stateMachine.ChangeState(new EnemyPatrolState());
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