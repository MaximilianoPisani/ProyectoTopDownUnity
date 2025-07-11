using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : HealthSystem
{
    private EnemyController _enemyController;
    private AnimationControllerHandler _animHandler;
    private EnemyDamageFeedbackHandler _feedbackHandler;
    private EnemyMeleeAttack _meleeAttack;
    private EnemyRangedAttack _rangedAttack;
    private NavMeshAgent _agent;
    private Collider2D _collider;

    private bool _isDead = false;
    private bool _isInvulnerable = false;
    private int _startingHealth;
    private bool _halfPhaseTriggered = false;
    private int _maxHealthBeforeSplit;
    private int _hitCount = 0;
    private EnemyAreaAttack _areaAttack;

    public bool IsDead() => _isDead;

    [SerializeField] private bool resetHealthOnPlayerDeath = false;
    [SerializeField] private bool enableHalfHealthEvent = false;
    [SerializeField] private bool triggersGameEndOnDeath = false;
    [SerializeField] private int hitsToTriggerTeleport = 2;
    [SerializeField] private bool allowMultipleTeleports = true;

    private bool _teleportPhaseActive = false;
    public event Action OnTeleportPhaseTriggered;
    public bool EnableHalfHealthEvent => enableHalfHealthEvent;

    public event Action OnHalfHealthReached;
    public static event Action<EnemyHealth> OnEnemyDied;

    [SerializeField] private bool enableSpecialAttackOnHits = false; 
    [SerializeField] private int hitsToTriggerSpecialAttack = 3; 
    public event Action OnSpecialAttackTriggered;

    protected override void Start()
    {
        base.Start();
        _areaAttack = GetComponent<EnemyAreaAttack>();
        if (_areaAttack != null)
            OnSpecialAttackTriggered += HandleSpecialAttack;

        _collider = GetComponent<Collider2D>();
        _startingHealth = health;
        _maxHealthBeforeSplit = health;

        PlayerHealth.OnPlayerDied += OnPlayerDiedHandler;

        _enemyController = GetComponent<EnemyController>();
        _animHandler = GetComponent<AnimationControllerHandler>();
        _feedbackHandler = GetComponent<EnemyDamageFeedbackHandler>();
        _meleeAttack = GetComponent<EnemyMeleeAttack>();
        _rangedAttack = GetComponent<EnemyRangedAttack>();
        _agent = GetComponent<NavMeshAgent>();
    }

    public override void TakeDamage(int amount)
    {
        if (_isDead || _isInvulnerable) return;

        base.TakeDamage(amount);
        _hitCount++;

        if (!_teleportPhaseActive && _hitCount >= hitsToTriggerTeleport && !enableSpecialAttackOnHits)
        {
            _hitCount = 0;
            _teleportPhaseActive = !allowMultipleTeleports;
            OnTeleportPhaseTriggered?.Invoke();
        }

        if (enableSpecialAttackOnHits && _hitCount >= hitsToTriggerSpecialAttack)
        {
            _hitCount = 0;
            OnSpecialAttackTriggered?.Invoke();
        }

        if (!_isDead)
        {
            if (_feedbackHandler != null)
                StartCoroutine(DamageFeedbackCoroutine());

            if (_enemyController != null)
            {
                if (_enemyController.HasSeenPlayer)
                    return;

                _enemyController.agent.velocity = Vector3.zero;
                _enemyController.SetMoving(false);
                _enemyController.agent.isStopped = true;
                _enemyController.ResetTarget();
                _enemyController.GetComponent<EnemyStateMachine>().ChangeState(new EnemyIdleState());
            }
        }
    }

    private void HandleSpecialAttack()
    {
        if (_rangedAttack != null) _rangedAttack.enabled = false;

        if (_areaAttack != null)
            _areaAttack.StartAreaAttack();
    }

    public void ReactivateRangedAttack()
    {
        if (_rangedAttack != null) _rangedAttack.enabled = true;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDied -= OnPlayerDiedHandler;
    }

    private void OnPlayerDiedHandler()
    {
        if (resetHealthOnPlayerDeath && !_isDead)
        {
            health = _startingHealth;
            NotifyHealthChanged();

            if (_animHandler != null)
            {
                _animHandler.SetBool("isAttackingMelee", false);
                _animHandler.SetBool("isAttackingRange", false);
                _animHandler.SetBool("isDead", false);
            }

            _teleportPhaseActive = false;
            _hitCount = 0;
        }
    }

    private IEnumerator DamageFeedbackCoroutine()
    {
        _isInvulnerable = true;
        _enemyController?.PauseMovement();

        if (_meleeAttack != null) _meleeAttack.enabled = false;
        if (_rangedAttack != null) _rangedAttack.enabled = false;

        yield return StartCoroutine(_feedbackHandler.PlayFeedback());

        if (!_isDead)
        {
            _enemyController?.ResumeMovement();
            if (_meleeAttack != null) _meleeAttack.enabled = true;
            if (_rangedAttack != null) _rangedAttack.enabled = true;

            if (!_enemyController.HasSeenPlayer)
                _enemyController.GetComponent<EnemyStateMachine>().ChangeState(new EnemyPatrolState());
        }

        _isInvulnerable = false;
    }

    protected override void Die()
    {
        _isDead = true;
        _isInvulnerable = false;

        OnEnemyDied?.Invoke(this);

        if (_collider != null)
            _collider.enabled = false;

        if (_agent != null)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.ResetPath();
        }

        if (_enemyController != null)
        {
            _enemyController.enabled = false;
            var stateMachine = _enemyController.GetComponent<EnemyStateMachine>();
            if (stateMachine != null)
                stateMachine.enabled = false;
        }

        if (_meleeAttack != null) _meleeAttack.enabled = false;
        if (_rangedAttack != null) _rangedAttack.enabled = false;

        Vector2 direction = Vector2.right;
        if (_enemyController?.currentTarget != null)
            direction = ((Vector2)(_enemyController.currentTarget.position - transform.position)).normalized;
        else if (_enemyController != null)
            direction = _enemyController.LastLookDirection;

        _animHandler.SetBool("isDead", true);
        _animHandler.SetFloat("LastX", direction.x);
        _animHandler.SetFloat("LastY", direction.y);

        StartCoroutine(HandleDeathSequence());
    }

    private IEnumerator HandleDeathSequence()
    {
        yield return new WaitForSeconds(1.45f);

        if (triggersGameEndOnDeath)
        {
            GameManager.Instance.EndGame();
            yield return new WaitForSeconds(1f);
        }

        PoolManager.Instance.ReturnToPool(gameObject);
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}