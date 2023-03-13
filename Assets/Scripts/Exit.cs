using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextLevel());
    }
    private IEnumerator LoadNextLevel()
    { 
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        var currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneBuildIndex + 1);
    }

}
