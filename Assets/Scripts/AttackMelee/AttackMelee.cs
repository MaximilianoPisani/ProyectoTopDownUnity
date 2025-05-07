using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : MonoBehaviour
{
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float attackRange = 0.5f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damage = 1;
    [SerializeField] protected float attackCooldown = 1.0f;
    [SerializeField] private float stunDuration = 1.5f;

    protected AnimationControllerHandler _animatorHandler;
    private float _lastAttackTime;
    private bool _isAlive = true;

    public bool canAttack = true;
    private Vector2 lastLookDirection;

    private void Awake()
    {
        _animatorHandler = GetComponent<AnimationControllerHandler>();
    }

    public void SetAliveState(bool state)
    {
        _isAlive = state;
    }

    public void TryAttack()
    {
        if (!canAttack || !_isAlive) return;

        if (Time.time < _lastAttackTime + attackCooldown) return;

        _lastAttackTime = Time.time;

        _animatorHandler.SetBool("isAttacking", true);
        StartCoroutine(PerformAttackAfterDelay(0.1f));
    }

    private IEnumerator PerformAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        UpdateAttackPointPosition();

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player")) continue;

            Vector2 directionToTarget = hit.transform.position - transform.position;

            float angle = Vector2.Angle(lastLookDirection.normalized, directionToTarget.normalized);
            if (angle > 60f) continue;

            if (directionToTarget.magnitude <= attackRange)
            {
                HealthSystem health = hit.GetComponent<HealthSystem>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }

                EnemyMovement enemyMovement = hit.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.ApplyStun(stunDuration);
                }
            }
        }

        _animatorHandler.SetBool("isAttacking", false);
    }

    private void Update()
    {
        float lx = _animatorHandler.Animator.GetFloat("LastX");
        float ly = _animatorHandler.Animator.GetFloat("LastY");

        lastLookDirection = new Vector2(lx, ly).normalized;
    }

    private void UpdateAttackPointPosition()
    {
        if (attackPoint == null) return;

        float offset = 0.6f;
        attackPoint.localPosition = lastLookDirection * offset;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

    }
}