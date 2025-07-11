using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAttack : AttackRanged //#TEST
{
    [SerializeField] private RangedAttackData _attackData;

    private EnemyController _enemy;
    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public float AttackRange => _attackData.AttackRange;

    protected override void Awake()
    {
        base.Awake();
        _enemy = GetComponent<EnemyController>();
        rangedAttackData = _attackData;
    }

    public void UpdateAttack()
    {
     
        TryAttack();
    }

    protected override void Update()
    {
        base.Update();

        if (_enemy.currentTarget != null)
        {
            Vector2 direction = (_enemy.currentTarget.position - transform.position).normalized;
            animHandler.SetFloat("LastX", direction.x);
            animHandler.SetFloat("LastY", direction.y);
        }
    }

    protected override void Shoot()
    {
        if (rangedAttackData.ProjectilePrefab == null || shootPoints.Count == 0) return;

        Transform shootPoint = shootPoints[0];
        if (shootPoint == null) return;

        Transform hitPoint = _enemy.currentTarget?.Find("HitPoint");

        Vector2 direction;
        if (hitPoint != null)
        {
            direction = ((Vector2)hitPoint.position - (Vector2)shootPoint.position).normalized;
        }
        else
        {
            direction = ((Vector2)_enemy.currentTarget.position - (Vector2)shootPoint.position).normalized;
            Debug.LogWarning("HitPoint not found");
        }

        GameObject projectileGO = PoolManager.Instance.GetFromPool(
            rangedAttackData.ProjectilePrefab,
            shootPoint.position,
            Quaternion.identity
        );


        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(direction, rangedAttackData, gameObject);
        }
    }
    public void ForceStopAttack()
    {
        canAttack = false;
        animHandler?.SetBool("isAttackingRange", false);
    }
}