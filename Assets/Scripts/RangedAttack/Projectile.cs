using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour //#TEST
{
    private Vector2 _direction;
    private float _speed;
    private int _damage;
    private float _maxRange;
    private Vector2 _startPosition;

    private GameObject _owner;
    private LayerMask _targetLayer;

    public void Initialize(Vector2 direction, RangedAttackData data, GameObject owner)
    {
        _direction = direction.normalized;
        _speed = data.ProjectileSpeed;
        _damage = data.Damage;
        _maxRange = data.AttackRange;
        _startPosition = transform.position;
        _owner = owner;
        _targetLayer = data.TargetLayer; 

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(_startPosition, transform.position) >= _maxRange)
        {
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _owner) return; 

       
        if ((_targetLayer.value & (1 << other.gameObject.layer)) == 0) return;

        HealthSystem health = other.GetComponent<HealthSystem>();
        if (health != null)
        {
            health.TakeDamage(_damage);
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }
}
