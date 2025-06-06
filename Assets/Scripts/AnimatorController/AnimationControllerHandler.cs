using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerHandler : MonoBehaviour
{
    private Animator _animator;
    public Animator Animator => _animator; // Allows other scripts to get the Animator component reference without being able to modify it.
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is not here ");

        }
    }

    public void TriggerAnimation(string state) // This activates a transition based on the given trigger name.
    {
        _animator.SetTrigger(state);
    }

    public void SetBool(string parameter, bool value) // Used to control animations based on true/false conditions.
    {
        if (HasParameter(parameter))
        {
            _animator.SetBool(parameter, value);
        }
        else
        {
            Debug.LogWarning("Animator parameter does not exist ");
        }
    }

    public void SetFloat(string parameter, float value) // Used to control animations based on numeric values.
    {
        _animator.SetFloat(parameter, value);
    }

    public void SafeSetBool(string parameter, bool value)
    {
        if (HasParameter(parameter))
        {
            _animator.SetBool(parameter, value);
        }
    }
    public bool HasParameter(string paramName)
    {
        if (_animator == null)
        {
            Debug.LogError("Animator is null in AnimationControllerHandler");
            return false;
        }

        foreach (AnimatorControllerParameter param in _animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
}
