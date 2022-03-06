using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer am;

    public void SetMusicVolume(float volume)
    {
        am.SetFloat("Music", volume);
    }

    public void SetSoundVolume(float volume)
    {
        am.SetFloat("Sound", volume);
    }

    public void SetDisplay(string display)
    {
        /*Screen.fullScreenMode(display);*/
    }
}
