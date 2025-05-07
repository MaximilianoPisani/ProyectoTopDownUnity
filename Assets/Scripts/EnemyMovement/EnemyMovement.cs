using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1.2f;
   

    private Transform _target;
    private AnimationControllerHandler _animHandler;
    private NavMeshAgent _agent;

    private bool isStunned = false;
    private float stunTimer = 0f;

    public Transform CurrentTarget => _target;

    private void Awake()
    {
        _animHandler = GetComponent<AnimationControllerHandler>();
        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _agent.speed = moveSpeed;
        _agent.avoidancePriority = Random.Range(30, 70);
    }

    private void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
            }

            _agent.ResetPath();
            _animHandler.SetBool("Moving", false);
            _animHandler.SetBool("isAttacking", false);
            return;
        }

        if (_target == null)
        {
            _agent.ResetPath();
            _animHandler.SetBool("Moving", false);
            _animHandler.SetBool("isAttacking", false);
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, _target.position);

        if (distanceToTarget <= attackRange)
        {
            _agent.ResetPath();

            Vector2 direction = (_target.position - transform.position).normalized;

            _animHandler.SetFloat("X", direction.x);
            _animHandler.SetFloat("Y", direction.y);
            _animHandler.SetFloat("LastX", direction.x);
            _animHandler.SetFloat("LastY", direction.y);

            _animHandler.SetBool("Moving", false);
            _animHandler.SetBool("isAttacking", true);
        }
        else
        {
            _agent.SetDestination(_target.position);

            Vector2 velocity = _agent.velocity.normalized;

            _animHandler.SetFloat("X", velocity.x);
            _animHandler.SetFloat("Y", velocity.y);
            _animHandler.SetFloat("LastX", velocity.x);
            _animHandler.SetFloat("LastY", velocity.y);

            _animHandler.SetBool("Moving", true);
            _animHandler.SetBool("isAttacking", false);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void RemoveTarget()
    {
        _target = null;
    }

    public void ApplyStun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        _agent.ResetPath();

        _animHandler.SetBool("Moving", false);
        _animHandler.SetBool("isAttacking", false);
    }
}
