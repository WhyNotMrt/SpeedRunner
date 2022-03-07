using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public GameObject settingsWindow;

    public void StartGameButton()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void SettingsButton()
    {
        gameObject.SetActive(false);
        settingsWindow.SetActive(true);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
