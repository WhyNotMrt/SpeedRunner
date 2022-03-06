using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public void StartGameButton()
    {
        SceneManager.LoadScene(levelToLoad);
    }
    public void SettingsButton()
    {

    }
    public void QuitGameButton()
    {
        Application.Quit();
    }
}
