using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackMelee : MonoBehaviour
{
    [SerializeField] protected MeleeAttackData meleeAttackData;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask targetLayer; 
    protected AnimationControllerHandler animHandler;
    protected Vector2 lastLookDirection;
    private float _lastAttackTime;
    protected bool _isAlive = true;
    protected bool canAttack = true;
    [SerializeField] private float _attackOffsetMultiplier = 0.5f;
    private void Awake()
    {
        animHandler = GetComponent<AnimationControllerHandler>();
        if (animHandler == null)
            Debug.LogWarning("AnimationControllerHandler not found ");
    }
    private void Update()
    {
        float lx = animHandler.Animator.GetFloat("LastX");
        float ly = animHandler.Animator.GetFloat("LastY");
        lastLookDirection = new Vector2(lx, ly).normalized;
        UpdateAttackPointPosition();
    }
    public void SetAttackEnabled(bool enabled)
    {
        canAttack = enabled;
    }
    public void SetAliveState(bool state) // Set alive or dead state to enable/disable attack capability
    {
        _isAlive = state;
    }

    public virtual void TryAttack() // Attempt to initiate an attack if possible 
    {
        if (!canAttack || !_isAlive) return;
        if (Time.time < _lastAttackTime + meleeAttackData.Cooldown) return;

        _lastAttackTime = Time.time;
        animHandler.SetBool("isAttackingMelee", true);

    }

    public void EndAttack() // Called to reset attack state after animation ends
    {
        animHandler.SetBool("isAttackingMelee", false);
    }

    protected void UpdateAttackPointPosition() // Repositions the attack point based on the last look direction and offset multiplier
    {
        if (attackPoint == null) return;
        float offset = meleeAttackData.AttackRange * _attackOffsetMultiplier;
        attackPoint.localPosition = lastLookDirection * offset;
    }


    public virtual void ApplyDamage() // Applies damage to all valid targets within attack range and angle.
    {
        UpdateAttackPointPosition();

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, meleeAttackData.AttackRange, targetLayer);
        foreach (Collider2D hit in hits)
        {
            Vector2 directionToTarget = hit.transform.position - transform.position;
            float angle = Vector2.Angle(lastLookDirection.normalized, directionToTarget.normalized);

            if (angle > 60f) continue;  

            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(Mathf.RoundToInt(meleeAttackData.Damage));
            }
        }
    }
    public bool IsCooldownReady()
    {
        return Time.time >= _lastAttackTime + meleeAttackData.Cooldown;
    }

    private void OnDrawGizmosSelected() // Draws a gizmo in the editor to visualize the attack range
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, meleeAttackData.AttackRange);
        }
    }
}