using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamageFeedbackHandler : MonoBehaviour 
{
    [SerializeField] private float _invulnerabilityDuration = 1f;
    [SerializeField] private float _flashInterval = 0.1f;

    private EnemyController _enemyController;
    private SpriteRenderer _spriteRenderer;
    private EnemyMeleeAttack _enemyAttack;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        if (_enemyController == null)

            Debug.LogWarning("Missing EnemyController ");
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
            Debug.LogWarning("Missing SpriteRenderer ");

        _enemyAttack = GetComponent<EnemyMeleeAttack>();
        if (_enemyAttack == null)
            Debug.LogWarning("Missing EnemyMeleeAttack ");
    }

    public IEnumerator PlayFeedback() // Coroutine to play feedback when enemy takes damage
    {
        if (_enemyController != null)
            _enemyController.SetEnemyActive(false);

        if (_enemyAttack != null)
            _enemyAttack.enabled = false;

        float elapsed = 0f;
        while (elapsed < _invulnerabilityDuration)
        {

            if (_spriteRenderer != null)
                _spriteRenderer.enabled = !_spriteRenderer.enabled;

            yield return new WaitForSeconds(_flashInterval);
            elapsed += _flashInterval;
        }

        if (_spriteRenderer != null)
            _spriteRenderer.enabled = true;

        if (_enemyController != null)
            _enemyController.SetEnemyActive(true);

        if (_enemyAttack != null)
            _enemyAttack.enabled = true;
    }
}