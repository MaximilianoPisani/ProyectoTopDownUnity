using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackMelee : MonoBehaviour
{
    [SerializeField] protected AttackData _attackData;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] private LayerMask targetLayer;
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

     
        if (Time.time < _lastAttackTime + _attackData.Cooldown) return;

        _lastAttackTime = Time.time;

        _animatorHandler.SetBool("isAttacking", true);
        StartCoroutine(PerformAttackAfterDelay(0.1f));
    }

    private IEnumerator PerformAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        UpdateAttackPointPosition();

        
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, _attackData.AttackRange, targetLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player")) continue;

            Vector2 directionToTarget = hit.transform.position - transform.position;

            float angle = Vector2.Angle(lastLookDirection.normalized, directionToTarget.normalized);
            if (angle > 60f) continue;

            if (directionToTarget.magnitude <= _attackData.AttackRange)
            {
                HealthSystem health = hit.GetComponent<HealthSystem>();
                if (health != null)
                {
                    health.TakeDamage(Mathf.RoundToInt(_attackData.Damage)); 
                }

                EnemyMovement enemyMovement = hit.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.ApplyStun(_attackData.StunDuration); 
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
            Gizmos.DrawWireSphere(attackPoint.position, _attackData.AttackRange);
        }

    }
}