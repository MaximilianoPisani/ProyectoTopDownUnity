using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackMelee : MonoBehaviour
{
    [SerializeField] protected AttackData _attackData;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] private LayerMask targetLayer;
    protected AnimationControllerHandler _animHandler;
    private float _lastAttackTime;
    private bool _isAlive = true;

    public bool canAttack = true;
    private Vector2 _lastLookDirection;

    private void Awake()
    {
        _animHandler = GetComponent<AnimationControllerHandler>();
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
        _animHandler.SetBool("isAttacking", true);
        StartCoroutine(PerformAttackAfterDelay(0.1f));
    }

    private IEnumerator PerformAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateAttackPointPosition();

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, _attackData.AttackRange, targetLayer);
        foreach (Collider2D hit in hits)
        {
            Vector2 directionToTarget = hit.transform.position - transform.position;
            float angle = Vector2.Angle(_lastLookDirection.normalized, directionToTarget.normalized);
            if (angle > 60f) continue;

            if (directionToTarget.magnitude <= _attackData.AttackRange)
            {
                HealthSystem health = hit.GetComponent<HealthSystem>();
                if (health != null)
                    health.TakeDamage(Mathf.RoundToInt(_attackData.Damage));
            }
        }

        _animHandler.SetBool("isAttacking", false);
    }

    private void Update()
    {
        float lx = _animHandler.Animator.GetFloat("LastX");
        float ly = _animHandler.Animator.GetFloat("LastY");
        _lastLookDirection = new Vector2(lx, ly).normalized;
    }

    private void UpdateAttackPointPosition()
    {
        if (attackPoint == null) return;
        float offset = _attackData.AttackRange * 0.1f;
        attackPoint.localPosition = _lastLookDirection * offset;
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