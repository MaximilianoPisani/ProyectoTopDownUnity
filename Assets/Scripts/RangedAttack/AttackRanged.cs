using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRanged : MonoBehaviour //#TEST
{
    protected RangedAttackData rangedAttackData;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected List<Transform> shootPoints = new List<Transform>();
    [SerializeField] private int _currentShootIndex = 0;

    protected AnimationControllerHandler animHandler;
    protected Vector2 lastLookDirection;

    protected bool canAttack = true;
    protected bool isAlive = true;

    protected virtual void Awake()
    {
        animHandler = GetComponent<AnimationControllerHandler>();
        if (animHandler == null)
            Debug.LogWarning("AnimationControllerHandler not found");
    }

    protected virtual void Update()
    {
        float lx = animHandler.Animator.GetFloat("LastX");
        float ly = animHandler.Animator.GetFloat("LastY");
        lastLookDirection = new Vector2(Mathf.Round(lx), Mathf.Round(ly)).normalized;
    }

    public void SetAliveState(bool state)
    {
        isAlive = state;
    }

    public virtual void TryAttack()
    {
        if (!canAttack || !isAlive) return;

        canAttack = false;
        animHandler.SetBool("isAttackingRange", true);
        Shoot();

        Invoke(nameof(ResetAttack), rangedAttackData.Cooldown);
    }

    public void EndAttacks() 
    {
        animHandler.SetBool("isAttackingRange", false);
    }

    protected virtual void Shoot()
    {
        if (rangedAttackData.ProjectilePrefab == null || shootPoints.Count < 4) return;

        int directionIndex = GetDirectionIndex(lastLookDirection);
        Transform shootPoint = shootPoints[directionIndex];
        if (shootPoint == null) return;

        GameObject projectile = Instantiate(rangedAttackData.ProjectilePrefab, shootPoint.position, Quaternion.identity);
        Projectile p = projectile.GetComponent<Projectile>();
        if (p != null)
        {
            p.Initialize(lastLookDirection, rangedAttackData, this.gameObject); 
        }
    }
    private int GetDirectionIndex(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            return dir.x > 0 ? 3 : 2; 
        }
        else
        {
            return dir.y > 0 ? 0 : 1; 
        }
    }

    protected void ResetAttack()
    {
        canAttack = true;
    }

    
    public void SetShootIndex(int index)
    {
        if (index >= 0 && index < shootPoints.Count)
           _currentShootIndex = index;
    }
}