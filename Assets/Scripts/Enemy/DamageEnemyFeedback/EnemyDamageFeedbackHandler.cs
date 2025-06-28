using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamageFeedbackHandler : MonoBehaviour
{
    [SerializeField] private float _invulnerabilityDuration = 1f;
    [SerializeField] private float _flashInterval = 0.1f;
    [SerializeField] private Color _damageColor = Color.red;

    private EnemyController _enemyController;
    private SpriteRenderer _spriteRenderer;
    private MonoBehaviour _enemyAttack;
    private Color _originalColor;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        if (_enemyController == null)
            Debug.LogWarning("Missing EnemyController");

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
            Debug.LogWarning("Missing SpriteRenderer");
        else
            _originalColor = _spriteRenderer.color;

        _enemyAttack = GetComponent<EnemyMeleeAttack>() as MonoBehaviour;
        if (_enemyAttack == null)
            _enemyAttack = GetComponent<EnemyRangedAttack>();
    }

    public IEnumerator PlayFeedback()
    {
        try
        {
            _enemyController?.PauseMovement();

            if (_enemyAttack != null)
                _enemyAttack.enabled = false;

            float elapsed = 0f;
            bool visible = true;

            while (elapsed < _invulnerabilityDuration)
            {
                if (_spriteRenderer != null)
                {

                    visible = !visible;
                    _spriteRenderer.enabled = visible;

                    if (visible)
                        _spriteRenderer.color = _damageColor;
                    else
                        _spriteRenderer.color = _originalColor;
                }

                yield return new WaitForSeconds(_flashInterval);
                elapsed += _flashInterval;
            }
        }
        finally
        {

            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = true;
                _spriteRenderer.color = _originalColor;
            }

            _enemyController?.ResumeMovement();

            if (_enemyAttack != null)
                _enemyAttack.enabled = true;
        }
    }
}