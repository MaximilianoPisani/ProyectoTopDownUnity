using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerHandler : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void TriggerAnimation(string state)
    {
        _animator.SetTrigger(state);
    }

    public void SetBool(string parameter, bool value)
    {
        _animator.SetBool(parameter, value);
    }

    public void SetFloat(string parameter, float value)
    {
        _animator.SetFloat(parameter, value);
    }
}