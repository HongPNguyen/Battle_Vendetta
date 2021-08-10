using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SceneControllerCore : MonoBehaviour
{
    public Battle[] battles;

    public AudioSource levelMusic;
    public AudioSource bossMusic;

    public int bossBattleId = 5;
    public float bossWait = 2f;
    protected static int checkpointAt = 0;
    protected ObjectivesSystem objSys;
    protected Sidebars hud;

    // For music
    public AudioMixer mixer;
    // For sound effects
    public AudioMixer sfxMixer;

    // Start is called before the first frame update
    protected void Start()
    {
        objSys = GameObject.Find("HUD").GetComponent<ObjectivesSystem>();
        if (checkpointAt == 0)
          objSys.ResetReset();
        hud = GameObject.Find("HUD").GetComponent<Sidebars>();
        mixer.SetFloat("volume", Mathf.Log(PlayerPrefs.GetFloat("musicVolume", 0.8f)) * 20f);
        sfxMixer.SetFloat("volume", Mathf.Log(PlayerPrefs.GetFloat("sfxVolume", 0.8f)) * 20f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Time-based enemy spawner
    protected IEnumerator BattleController()
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
        }
    }

    public int GetPhase()
    {
        return checkpointAt;
    }

    public static void ResetCheckpoints()
    {
        checkpointAt = 0;
    }

    public float GetLevelVolumeLinear()
    {
        float value, temp;
        bool result = mixer.GetFloat("levelVolume", out value);
        bool result2 = mixer.GetFloat("volume", out temp);
        if (result)
        {
            Debug.Log("Master Volume: " + temp);
            Debug.Log("Level Volume: " + value);
            return Mathf.Pow(10f, value / 20f);
        }
        else
        {
            return 0f;
        }
    }

    public float GetBossVolumeLinear()
    {
        float value;
        bool result = mixer.GetFloat("bossVolume", out value);
        if (result)
        {
            return Mathf.Pow(10f, value / 20f);
        }
        else
        {
            return 0f;
        }
    }
}
