// THIS SCRIPT CONTROLS SCENE SWITCHES AND IN-GAME MENUS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectEvents : MonoBehaviour
{
    //All work in progress, may not need this script in the future.
    public GameObject player;
    public GameObject levelBoss;

    public GameObject gameOverMenu;
    protected int nextSceneLoad;

    protected float levelEndTimer = 0f;
    public float levelEndTime = 4f;

    bool bossNarrationDone;
    public LineSet lineSetToUse;

    public void Start()
    {
        //Find player squadron
        player = GameObject.Find("Squadron");
        player.GetComponent<PlaneSwitching>().SetUp();
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        bossNarrationDone = false;
    }

    public void Update()
    {
        if (player.GetComponent<PlaneSwitching>().GetIsDead())
        {
            gameOverMenu.SetActive(true);
        }

        // SCENE END BEHAVIOR (when boss is destroyed)
        if (levelBoss == null)
        {
            if (!bossNarrationDone)
            {
                GameObject.Find("HUD").GetComponent<Narration>().ChangeLineSet(lineSetToUse);
                bossNarrationDone = true;
            }
            levelEndTimer += Time.deltaTime;
            if (levelEndTimer >= levelEndTime)
            {
                Debug.Log("boss is dead");
                Progression.ReportLevelCompleted(SceneTransition.upcomingScene - 2); //Upcoming scene still represents the current level
                SceneControllerCore.ResetCheckpoints(); // Reset checkpoints of current level so next time level starts at beginning
                 
                //Clear the squadron
                for (int i = player.transform.childCount; i > 0; i--)
                {
                     Destroy(player.transform.GetChild(i - 1).gameObject);
                }

                if (SceneTransition.upcomingScene < 3)
                { 
                     player.SetActive(false);
                     SceneTransition.upcomingScene++; //Update upcomingscene to represent the next level
                     SceneManager.LoadScene("Hangar");
                }
                else
                {
                     Destroy(player);
                     SceneManager.LoadScene(0);
                }

                if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
                {
                    PlayerPrefs.SetInt("levelAt", nextSceneLoad);
                    Debug.Log("level unlock");
                }
            }
        }
    }


}
