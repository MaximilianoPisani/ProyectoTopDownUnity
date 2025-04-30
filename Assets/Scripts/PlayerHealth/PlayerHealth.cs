using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : HealthSystem
{
    [SerializeField] private HealthBar _healthBar;

    private PlayerMovement _movement;
    private AnimationControllerHandler _animatorHandler;
    private Rigidbody2D _rb;
    private DamageFeedbackHandler _feedbackHandler;
    private bool _isInvulnerable = false;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _animatorHandler = GetComponent<AnimationControllerHandler>();
        _rb = GetComponent<Rigidbody2D>();
        _feedbackHandler = GetComponent<DamageFeedbackHandler>();
    }

    protected override void Start()
    {
        base.Start();
        if (_healthBar != null)
            _healthBar.InitializeHealthBar(maxHealth);
    }

    public override void TakeDamage(int amount)
    {
        if (_isInvulnerable)
            return;

        base.TakeDamage(amount);

        if (_healthBar != null)
            _healthBar.ChangeCurrentHealth(health);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            _isInvulnerable = true;
            if (_feedbackHandler != null)
                StartCoroutine(_feedbackHandler.PlayFeedback(() => _isInvulnerable = false));
        }
    }

    protected override void Die()
    {
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        if (_movement != null)
            _movement.enabled = false;

        if (_animatorHandler != null)
        {
            _animatorHandler.SetBool("isDead", true);
            _animatorHandler.SetFloat("X", 0f);
            _animatorHandler.SetFloat("Y", 0f);
            _animatorHandler.SetBool("Moving", false);
        }

        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        yield return new WaitForSeconds(1f);

        Respawn();

        if (_rb != null)
            _rb.bodyType = RigidbodyType2D.Dynamic;
        if (_animatorHandler != null)
            _animatorHandler.SetBool("isDead", false);
        if (_movement != null)
            _movement.enabled = true;
    }

    private void Respawn()
    {
        transform.position = CheckpointManager.Instance.GetCheckpointPosition();
        health = maxHealth;
        if (_healthBar != null)
            _healthBar.ChangeCurrentHealth(health);
    }
}