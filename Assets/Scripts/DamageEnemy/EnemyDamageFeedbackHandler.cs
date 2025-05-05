using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamageFeedbackHandler : MonoBehaviour
{
    [SerializeField] private float _invulnerabilityDuration = 1f;
    [SerializeField] private float _flashInterval = 0.1f;

    private EnemyMovement _enemyMovement;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private NavMeshAgent _agent;
    private EnemyMeleeAttack _enemyAttack;

    private void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyAttack = GetComponent<EnemyMeleeAttack>();
    }

    public IEnumerator PlayFeedback()
    {
        
        if (_enemyMovement != null) _enemyMovement.enabled = false;
        if (_agent != null) _agent.isStopped = true;
        if (_enemyAttack != null) _enemyAttack.enabled = false;

        float elapsed = 0f;
        while (elapsed < _invulnerabilityDuration)
        {
            if (_rb != null)
                _rb.velocity = Vector2.zero;

            if (_spriteRenderer != null)
                _spriteRenderer.enabled = !_spriteRenderer.enabled;

            yield return new WaitForSeconds(_flashInterval);
            elapsed += _flashInterval;
        }

       
        if (_spriteRenderer != null)
            _spriteRenderer.enabled = true;
        if (_enemyMovement != null)
            _enemyMovement.enabled = true;
        if (_agent != null)
            _agent.isStopped = false;
        if (_enemyAttack != null)
            _enemyAttack.enabled = true;
    }
}
