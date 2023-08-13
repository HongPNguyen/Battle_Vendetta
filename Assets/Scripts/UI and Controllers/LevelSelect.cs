using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the level select menu.
/// </summary>
public class LevelSelect : MonoBehaviour
{
    /// <summary>
    /// Starts the "Crimson Flare" mission (Mission 1) with boss "Airos".
    /// </summary>
    public void AirosStart()
    {
        SceneTransition.upcomingScene = SceneManager.GetSceneByName("AirosScene").buildIndex;
        Scene1Controller.ResetCheckpoints();

        //If Airos has already been beaten before, allow the player to go to the Hangar
        if (Progression.lastLevelCompleted >= 1)
            SceneManager.LoadScene("Hangar");
        else
            SceneManager.LoadScene("AirosScene");
    }

    /// <summary>
    /// Starts the "Orange Dust" mission (Mission 2) with boss "Terrod".
    /// </summary>
    public void TerrodStart()
    {
        SceneTransition.upcomingScene = SceneManager.GetSceneByName("TerrodScene").buildIndex;
        Scene2Controller.ResetCheckpoints();
        SceneManager.LoadScene("Hangar");
    }
}
