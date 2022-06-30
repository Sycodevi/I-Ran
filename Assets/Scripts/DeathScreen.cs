using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathScreen : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void menuRestartLevel()
    {
        Time.timeScale = 1;
        var currentscene = SceneManager.GetActiveScene().name;

        SceneManager.UnloadScene(currentscene);
        SceneManager.LoadScene(currentscene);
    }
}
