using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFeedbackHandler : MonoBehaviour 
{
    [SerializeField] private float _invulnerabilityDuration = 1f;
    [SerializeField] private float _flashInterval = 0.1f;

    private PlayerController _playerController;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator PlayFeedback(System.Action onComplete = null) // Coroutine to play damage feedback (invulnerability + flashing)
    {
        if (_playerController != null)
            _playerController.enabled = false;

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

        if (_playerController != null)
            _playerController.enabled = true;

        onComplete?.Invoke();
    }
}