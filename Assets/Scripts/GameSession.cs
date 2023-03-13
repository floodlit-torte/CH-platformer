using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int lifeCount = 3;

    //Singleton
    private void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numberOfGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessDeath()
    {
        if(lifeCount > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void ResetGameSession()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    private void TakeLife()
    {
        lifeCount--;
        Debug.Log(lifeCount);
    }
}
