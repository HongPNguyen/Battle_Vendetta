using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Manages the Game Over menu
public class GameOverMenu : MonoBehaviour
{
    public int currentLevel = 1; // Will probably need to think of a better solution for checkpoints resetting.

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameObject.Find("Squadron").GetComponent<PlaneSwitching>().SetUp();
        GameObject.Find("HUD").GetComponent<Sidebars>().Reset();
        Debug.Log("Reloading Scene");
    }

    public void BackToMainMenu()
    {
        // Reset checkpoints of every scene
        ResetCheckpoints();
        SceneManager.LoadScene(0);
        Debug.Log("Back to Main Menu");
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
