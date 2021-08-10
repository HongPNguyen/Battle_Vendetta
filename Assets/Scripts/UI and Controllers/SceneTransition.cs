using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
     public static int upcomingScene = 2;
     public static GameObject squadron;
     public GameObject squad;

     void Update()
     {
          if (squad != squadron)
               squadron = squad;
     }

     //Transitions to the next scene after the Hangar Scene
     public static void NextScene()
     {
          squadron.SetActive(true);
          DontDestroyOnLoad(squadron);
          SceneManager.LoadScene(upcomingScene);
     }

     //Transition to the Hangar before the next level
     public void GoToHangar()
     {
          squadron.SetActive(false);
          SceneManager.LoadScene("Hangar");
     }

     //Exits out to the main menu
     public void ExitToMenu()
     {
          SceneManager.LoadScene(0);
     }
}
