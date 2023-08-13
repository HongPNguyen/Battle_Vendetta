using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// This class serves to manage the main menu of the game.
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject OptionMenu;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;
    public Dropdown resolutionDropdown;

    Resolution[] resolutions;
    protected bool fullscreen = true;

    public AudioSource myFx;
    public AudioClip hoverFx;
    public AudioClip clickFx;

    #region Start

    /// <summary>
    /// Prepare settings menu on load.
    /// </summary>
    public void Start()
    {
        // Get distinct supported resolutions
        var resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct();

        // Clear out default options to start with a clean resolutions list based on system environment.
        resolutionDropdown.ClearOptions();

        // converting array to list for add options
        List<string> options = new List<string>();

        // Add supported resolutions to list
        foreach (Resolution res in resolutions)
        {
            string option = res.width + " x " + res.height;
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.onValueChanged.AddListener(delegate { ResolutionChanged(resolutionDropdown); });

        // Set volume sliders in options menu
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.8f);
    }

    #endregion

    #region Game loader

    public void PlayGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    #endregion

    #region Resolution settings

    // Change resolution
    protected void ResolutionChanged(Dropdown menu)
    {
        /* Parse resolution "1280 x 720" by space
         * Hence screen width is element 0, height is element 2. */
        string[] chosenRes = menu.captionText.text.Split(' ');
        int.TryParse(chosenRes[0], out int chosenWidth);
        int.TryParse(chosenRes[2], out int chosenHeight);
        Screen.SetResolution(chosenWidth, chosenHeight, fullscreen);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        fullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
    }

    #endregion

    #region Sounds & sound settings

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);
        musicMixer.SetFloat("volume", Mathf.Log(volume) * 20); // dB calculation (log * 20)
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);
        sfxMixer.SetFloat("volume", Mathf.Log(volume) * 20); // dB calculation (log * 20)
    }


    public void PlayHoverSound()
    {
        myFx.PlayOneShot(hoverFx);
    }

    public void PlayClickSound()
    {
        myFx.PlayOneShot(clickFx);
    }

    #endregion

    #region Quit button

    // Quits the application
    public void QuitGame()
    {
        // Reset all levels
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    #endregion
}
