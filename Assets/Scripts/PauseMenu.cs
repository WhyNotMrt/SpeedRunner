using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingsWindow;
    public GameObject pauseWindow;

    private bool pause = false;

    public void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseWindow.SetActive(true);
        Time.timeScale = 0;
        pause = true;
    }

    public void ResumeGame()
    {
        pauseWindow.SetActive(false);
        Time.timeScale = 1;
        pause = false;
    }

    public void OpenSettings()
    {
        pauseWindow.SetActive(false);
        settingsWindow.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
