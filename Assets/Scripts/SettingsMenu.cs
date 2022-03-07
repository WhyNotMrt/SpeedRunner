using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer am;
    public Dropdown resolutionDropdown;
    public GameObject mainMenu;

    private Resolution[] resolutions;
    private FullScreenMode fsm;

    public void Start()
    {
        fsm = FullScreenMode.MaximizedWindow;
        resolutions = Screen.resolutions.Select(r => new Resolution { width = r.width, height = r.height }).Distinct().ToArray();

        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;
        List<string> options = new List<string>();

        resolutionDropdown.ClearOptions();

        int currentResolutionsIndex = 0;
        int index = 0;
        foreach(Resolution r in resolutions)
        {
            int width = r.width;
            int height = r.height;
            string option = width + "x" + height;
            /*if (index == 0 || options[index - 1] != option)
            {*/
                options.Add(option);
                if (width == currentWidth && height == currentHeight) currentResolutionsIndex = index;
                index++;
            /*}*/
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionsIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Back")) ReturnToMainMenu();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fsm);
    }

    public void SetDisplay(int display)
    {
        switch (display)
        {
            case 0:
                fsm = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                fsm = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                fsm = FullScreenMode.MaximizedWindow;
                break;
            default:
                fsm = FullScreenMode.MaximizedWindow;
                break;
        }
        Screen.fullScreenMode = fsm;
    }

    public void SetMusicVolume(float volume)
    {
        am.SetFloat("Music", volume);
    }

    public void SetSoundVolume(float volume)
    {
        am.SetFloat("Sound", volume);
    }

    public void ReturnToMainMenu()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
