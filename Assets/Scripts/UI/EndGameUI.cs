using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private GameObject _HealthBar;

    void Start()
    {
        GameManager.Instance.onEndGame += TurnOnPanel;
    }

    private void TurnOnPanel()
    {
        _endPanel.SetActive(true);
        _HealthBar.SetActive(false);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("WinSound");
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.onEndGame -= TurnOnPanel;
    }
}