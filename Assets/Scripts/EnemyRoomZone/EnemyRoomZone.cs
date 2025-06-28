using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoomZone : MonoBehaviour
{
    [SerializeField] private GameObject _keyObjectInScene; 
    private List<EnemyHealth> _enemiesInZone = new List<EnemyHealth>();
    private bool _keyActivated = false;

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDied -= OnEnemyDied;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null && !_enemiesInZone.Contains(enemy))
        {
            _enemiesInZone.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            _enemiesInZone.Remove(enemy);
        }
    }

    private void OnEnemyDied(EnemyHealth enemy)
    {
        if (_enemiesInZone.Contains(enemy))
        {
            _enemiesInZone.Remove(enemy);

            if (_enemiesInZone.Count == 0 && !_keyActivated)
            {
                ActivateKey();
                _keyActivated = true;
            }
        }
    }

    private void ActivateKey()
    {
        if (_keyObjectInScene != null)
        {
            _keyObjectInScene.SetActive(true);
            Debug.Log("Llave activada al eliminar a todos los enemigos.");
        }
        else
        {
            Debug.LogWarning("No se asignó el objeto de la llave en la escena.");
        }
    }
}