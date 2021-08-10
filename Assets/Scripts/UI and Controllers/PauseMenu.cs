using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public int currentLevel = 1;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
    }

    //TODO: create a variable for loadscene menu
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        // Reset checkpoints of every scene
        ResetCheckpoints();

        //Remove squadron
        GameObject player = GameObject.Find("Squadron");
        Destroy(player);

        SceneManager.LoadScene("RickysMainMenu");
    }

    public void Quit()
    {
        Debug.Log("Quiting game");
        // Reset checkpoints of every scene
        ResetCheckpoints();
        Application.Quit();
    }

    public void ResetCheckpoints()
    {
        switch (currentLevel)
        {
            case 1:
                Scene1Controller.ResetCheckpoints();
                break;
        }
    }
}