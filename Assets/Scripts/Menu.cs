using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void menuSetActive()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }
    public void menuSetInactive()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void menuRestartLevel()
    {
        Time.timeScale = 1;
        var currentscene = SceneManager.GetActiveScene().name;

        SceneManager.UnloadScene(currentscene);
        SceneManager.LoadScene(currentscene);
    }

    
}
