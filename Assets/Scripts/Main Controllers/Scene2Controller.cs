using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Scene2Controller : SceneControllerCore
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        StartCoroutine(BattleController());
    }

    // Time-based enemy spawner
    new IEnumerator BattleController()
    {
        // Start battle with checkpoint if there's a checkpoint reached.
        for (int i = checkpointAt; i < battles.Length; i++)
        {
            // Start battle after timer since previous battle started, but only if previous battle has been finished.
            Battle battle = battles[i];
            if (i > checkpointAt)
            {
                yield return new WaitForSeconds(battle.timer);
                Battle prevBattle = battles[i - 1];
                yield return new WaitUntil(() => prevBattle.TestBattleOver());
            }

            // If it's the boss battle, delay a little before starting for dramatic effect.
            if (i == bossBattleId && i != checkpointAt)
            {
                StartCoroutine(FadeMixerGroup.Fade(mixer, "levelVolume", 2f, 0f));
                yield return new WaitForSeconds(bossWait);
            }
            battle.StartBattle();

            // Play level music or boss music depending on the battle 
            if (i == checkpointAt && i != bossBattleId)
            {
                levelMusic.Play();
            }
            else if (i == bossBattleId)
            {
                bossMusic.Play();
                mixer.SetFloat("bossVolume", 0f);
                StartCoroutine(FadeMixerGroup.Fade(mixer, "bossVolume", 2f, 1f));
            }

            // Save a checkpoint if battle is specified to have a checkpoint before it.
            if (battle.checkpointBefore)
            {
                if (objSys == null)
                    objSys = GameObject.Find("HUD").GetComponent<ObjectivesSystem>();
                checkpointAt = i;
                objSys.CheckpointUpdate(checkpointAt);
                Debug.Log("Current Phase: " + checkpointAt);
            }

            // Change phase text & objectives based on phase
            switch (i)
            {
                case 0: // Boss
                    hud.SetPhaseText("BOSS");
                    objSys.ActivateObjectives(i, -1);
                    break;
                default:
                    Debug.Log("Error evaluating current phase. Resetting level.");
                    ResetCheckpoints();
                    break;
            }
        }
    }
}
