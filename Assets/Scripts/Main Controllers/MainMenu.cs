using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

/*
 * This class serves to manage the main menu of the game.
 */
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

    public void HoverSound()
    {
        myFx.PlayOneShot(hoverFx);
    }

    public void ClickSound()
    {
        myFx.PlayOneShot(clickFx);
    }

    public void delay()
    {
        StartCoroutine(TemporarilyDeactivate(2.0f));
    }

    private IEnumerator TemporarilyDeactivate(float duration)
    {
        OptionMenu.SetActive(false);
        yield return new WaitForSeconds(duration);
        OptionMenu.SetActive(true);
    }

    public void Start()
    {
        // Get distinct supported resolutions
        var resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct();

        // clear out default options to start with a cealn resoulation 
        resolutionDropdown.ClearOptions();

        // converting array to list for add options
        List<string> options = new List<string>();

        // add element option list
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

    // Change resolution
    protected void ResolutionChanged(Dropdown menu)
    {
        string[] chosenRes = menu.captionText.text.Split(' ');
        int.TryParse(chosenRes[0], out int chosenWidth);
        int.TryParse(chosenRes[2], out int chosenHeight);
        Screen.SetResolution(chosenWidth, chosenHeight, fullscreen);
    }

    // Set fullscreen
    public void SetFullScreen(bool isFullscreen)
    {
        fullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
    }


    public void LoadingGame()
    {
        Invoke("PlayGame", 2.0f);
    }



    // Starts the game
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Goes to settings menu
    // TODO: Modify this so that we don't have to change it each time the scene order is changed
    public void GoToSettings()
    {
        SceneManager.LoadScene(5);
    }

    // Adjust music 
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);
        musicMixer.SetFloat("volume", Mathf.Log(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);
        sfxMixer.SetFloat("volume", Mathf.Log(volume) * 20);
    }



    // Quits the application
    public void QuitGame()
    {
        Debug.Log("Game Quit");
        //reset all levels
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
