using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaAttack : MonoBehaviour
{
    [Header("Area Attack Settings")]
    [SerializeField] private float attackRadius = 3f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackDuration = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Color attackColorStart = new Color(1f, 0.2f, 0.2f); 
    [SerializeField] private Color attackColorEnd = new Color(1f, 0.6f, 0.3f);   
    [SerializeField] private float flashInterval = 0.1f;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minLineWidth = 0.05f;
    [SerializeField] private float maxLineWidth = 0.15f;

    private EnemyController _enemyController;
    private AnimationControllerHandler _animHandler;
    private SpriteRenderer _spriteRenderer;
    private LineRenderer _lineRenderer;
    private int _segments = 50;
    private bool _isAreaAttackActive = false;
    private Color _originalColor;
    private EnemyHealth _enemyHealth;

    public bool IsAreaAttackActive => _isAreaAttackActive;

    private void Awake()
    {
        _animHandler = GetComponent<AnimationControllerHandler>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHealth = GetComponent<EnemyHealth>();

        if (_spriteRenderer == null)
            Debug.LogWarning("Missing SpriteRenderer on enemy!");
        else
            _originalColor = _spriteRenderer.color; 

        if (_animHandler == null)
            Debug.LogWarning("Missing AnimationControllerHandler on enemy!");

        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.positionCount = _segments + 1;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.loop = true;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.enabled = false;
    }
    public void StartAreaAttack()
    {
        if (!_isAreaAttackActive)
            StartCoroutine(AreaAttackCoroutine());
    }

    private void DrawCircle(float radius)
    {
        float angle = 0f;
        for (int i = 0; i <= _segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            Vector3 pos = new Vector3(x, y, 0) + transform.position;
            _lineRenderer.SetPosition(i, pos);
            angle += 360f / _segments;
        }
    }

    private IEnumerator AreaAttackCoroutine()
    {
        _isAreaAttackActive = true;

        DrawCircle(attackRadius);
        _lineRenderer.enabled = true;

        if (_animHandler != null)
        {
            _animHandler.SafeSetBool("isAttackingMelee", false);
            _animHandler.SafeSetBool("Moving", false);
        }

        float elapsed = 0f;
        bool visible = true;
        bool damageApplied = false; 

        while (elapsed < attackDuration)
        {
            if (_enemyHealth != null && _enemyHealth.IsDead())
            {
                if (_spriteRenderer != null)
                {
                    _spriteRenderer.enabled = true;
                    _spriteRenderer.color = _originalColor;
                }

                _lineRenderer.enabled = false;
                _isAreaAttackActive = false;
                yield break;
            }


            if (!damageApplied && elapsed >= attackDuration / 2f)
            {
                Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
                if (hit != null && hit.CompareTag("Player"))
                {
                    PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                        playerHealth.TakeDamage(attackDamage);
                }
                damageApplied = true;
            }

            if (_spriteRenderer != null)
            {
                visible = !visible;
                _spriteRenderer.enabled = visible;
                _spriteRenderer.color = visible ? attackColorStart : attackColorEnd;
            }

            float pulse = Mathf.Lerp(minLineWidth, maxLineWidth, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            _lineRenderer.startWidth = pulse;
            _lineRenderer.endWidth = pulse;

            Color lerpedColor = Color.Lerp(attackColorStart, attackColorEnd, Mathf.PingPong(Time.time * 3f, 1f));
            _lineRenderer.startColor = lerpedColor;
            _lineRenderer.endColor = lerpedColor;

            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.color = _originalColor;
        }

        _lineRenderer.enabled = false;
        _isAreaAttackActive = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, attackRadius);
    }
}