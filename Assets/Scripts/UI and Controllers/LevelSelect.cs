using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Manages the level select menu
public class LevelSelect : MonoBehaviour

{
    public void AirosStart()
    {
          SceneTransition.upcomingScene = 2;
          Scene1Controller.ResetCheckpoints();
          //If Airos has already been beaten before, allow the player to go to the Hangar
          if (Progression.progress[0] == 1)
               SceneManager.LoadScene("Hangar");
          else
               SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void TerrodStart()
    {
        SceneTransition.upcomingScene = 3;
          Scene2Controller.ResetCheckpoints();
        SceneManager.LoadScene("Hangar");
    }

    public void LynchStart()
    {
        Debug.Log("Coming Soon");
    }


}
