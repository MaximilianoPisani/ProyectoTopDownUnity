using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public OnEndGame onEndGame;
    public delegate void OnEndGame();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void EndGame()
    {
        Debug.Log("Game Finished");


        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        onEndGame?.Invoke();
    }
}