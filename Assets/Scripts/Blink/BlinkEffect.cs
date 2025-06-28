using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private float _blinkSpeed = 2f;
    [SerializeField] private float _alphaMin = 0.3f;
    [SerializeField] private float _alphaMax = 1f;

    private SpriteRenderer _renderer;
    private float _time;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_renderer == null) return;

        _time += Time.deltaTime * _blinkSpeed;
        float alpha = Mathf.Lerp(_alphaMin, _alphaMax, Mathf.PingPong(_time, 1f));

        Color color = _renderer.color;
        color.a = alpha;
        _renderer.color = color;
    }
}