using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;

    private Transform _target;
    private AnimationControllerHandler _animHandler;
    private NavMeshAgent _agent;
    private EnemyMeleeAttack _enemyAttack;

    private bool _isStunned = false;
    private float _stunTimer = 0f;

    public Transform CurrentTarget => _target;

    private void Awake()
    {
        _animHandler = GetComponent<AnimationControllerHandler>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyAttack = GetComponent<EnemyMeleeAttack>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _moveSpeed;
        _agent.avoidancePriority = Random.Range(30, 70);
    }

    private void Update()
    {
        if (_isStunned)
        {
            _stunTimer -= Time.deltaTime;
            if (_stunTimer <= 0f) _isStunned = false;

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


        if (distanceToTarget <= _enemyAttack.AttackRange)
        {
            _agent.ResetPath();
            FaceTargetAndAnimateAttack();
        }
        else
        {
            _agent.SetDestination(_target.position);
            AnimateMovement();
        }
    }

    private void FaceTargetAndAnimateAttack()
    {
        Vector2 dir = (_target.position - transform.position).normalized;
        _animHandler.SetFloat("X", dir.x);
        _animHandler.SetFloat("Y", dir.y);
        _animHandler.SetFloat("LastX", dir.x);
        _animHandler.SetFloat("LastY", dir.y);
        _animHandler.SetBool("Moving", false);
        _animHandler.SetBool("isAttacking", true);
    }

    private void AnimateMovement()
    {
        Vector2 vel = _agent.velocity.normalized;
        _animHandler.SetFloat("X", vel.x);
        _animHandler.SetFloat("Y", vel.y);
        _animHandler.SetFloat("LastX", vel.x);
        _animHandler.SetFloat("LastY", vel.y);
        _animHandler.SetBool("Moving", true);
        _animHandler.SetBool("isAttacking", false);
    }

    public void SetTarget(Transform target) => _target = target;
    public void RemoveTarget() => _target = null;

    public void ApplyStun(float duration)
    {
        _isStunned = true;
        _stunTimer = duration;
        _agent.ResetPath();
        _animHandler.SetBool("Moving", false);
        _animHandler.SetBool("isAttacking", false);
    }

}
