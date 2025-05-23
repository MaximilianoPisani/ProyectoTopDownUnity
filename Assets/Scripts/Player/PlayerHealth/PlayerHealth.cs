using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : HealthSystem
{
    [SerializeField] private HealthBar _healthBar;

    private PlayerController _playerController;
    private DamageFeedbackHandler _damageFeedbackHandler;
    private AttackMelee _attackMelee;
    private AnimationControllerHandler _animatorHandler;
    private Rigidbody2D _rb;
    private bool _isInvulnerable = false;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        if (_playerController == null)
            Debug.LogWarning("Missing PlayerController ");

        _animatorHandler = GetComponent<AnimationControllerHandler>();
        if (_animatorHandler == null)
            Debug.LogWarning("Missing AnimatorControllerHandler ");

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
            Debug.LogWarning("Missing Rigidbody2D ");

        _damageFeedbackHandler = GetComponent<DamageFeedbackHandler>();
        if (_damageFeedbackHandler == null)
            Debug.LogWarning("Missing DamageFeedbackHandler ");

        _attackMelee = GetComponent<AttackMelee>();
        if (_attackMelee == null)
            Debug.LogWarning("Missing AttackMelee ");

    }

    protected override void Start()
    {
        base.Start();

        if (_healthBar != null)
            _healthBar.SetHealthSystem(this);
    }

    public override void TakeDamage(int amount)
    {
        if (_isInvulnerable || IsDead()) return;

        base.TakeDamage(amount); // Apply damage using the base method

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(PlayDamageFeedback());
        }
    }

    private IEnumerator PlayDamageFeedback() // Coroutine to handle temporary invulnerability and visual feedback
    {
        _isInvulnerable = true;

        if (_damageFeedbackHandler != null)
        {
            yield return StartCoroutine(_damageFeedbackHandler.PlayFeedback());
        }

        _isInvulnerable = false;
    }

    protected override void Die()
    {
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath() // Handles the player's death and then respawn
    {
        if (_playerController != null)
            _playerController.enabled = false;

        _animatorHandler?.SetBool("isDead", true);
        _animatorHandler?.SetFloat("X", 0f);
        _animatorHandler?.SetFloat("Y", 0f);
        _animatorHandler?.SetBool("Moving", false);

        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        _attackMelee?.SetAliveState(false);

        yield return new WaitForSeconds(1f);


        Respawn();

        if (_rb != null)
            _rb.bodyType = RigidbodyType2D.Dynamic;
        if (_animatorHandler != null)
            _animatorHandler.SetBool("isDead", false);
        if (_playerController != null)
            _playerController.enabled = true;
    }

    private void Respawn() // Resets the player's position and health
    {
        if (CheckpointManager.Instance != null)
            transform.position = CheckpointManager.Instance.GetCheckpointPosition();

        else
            Debug.LogWarning("CheckpointManager instance is null. Player respawn position not set.");

        health = maxHealth;

        if (_attackMelee != null)
            _attackMelee.SetAliveState(true);

        NotifyHealthChanged(); 
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}