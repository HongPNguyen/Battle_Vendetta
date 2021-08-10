using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    Animator cutsceneAnimator;
    Scene1Controller sceneControl;
    int progression;
    public bool introNeeded = true;
    bool isTutorialObjectiveDone;
    bool tutorialActivated = false;
    bool tutorialComplete = false;
    bool condorIntroComplete = false;
    bool airosIntroActivated = false;
    bool condorFled = false;
    bool airosArrived = false;
    bool introStarted = false;
    public float timeforintro = 10;
    public float timefortutorial = 10;
    public float timeforEucalypso = 6;
    public float timeforAiros = 10;
    public GameObject Scene1Airos;
    GameObject sceneCondor;
    public GameObject Scene1BlackCondor;
    GameObject player;
    Animator airosMove;
    Animator condorMove;
    public GameObject ControlsBillboard = null;
    float timestart, timepassed;
    float timeEucalypsoAttack = -1;
    float timeAirosIntro = -1;
    // Start is called before the first frame update
    void Start()
    {
        sceneControl = GameObject.Find("SceneController").GetComponent<Scene1Controller>();
        cutsceneAnimator = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("ActivePlayer");
        timestart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        timepassed = Time.time;
        player = GameObject.FindWithTag("ActivePlayer");
        progression = sceneControl.GetPhase();

        if (progression > 0 && GameObject.FindWithTag("IntroBlackCondor") != null) GameObject.FindWithTag("IntroBlackCondor").SetActive(false);

        if (introNeeded && progression == 0 && !introStarted && player != null)
        {
            introStarted = true;
            player.GetComponent<Player>().SeizeMovement();
            if (ControlsBillboard != null) ControlsBillboard.SetActive(true);
        }
        
        isTutorialObjectiveDone = gameObject.GetComponent<Tutorial>().ObjectivesDone();

        if (timepassed - timestart > timeforintro && introNeeded)
        {
            if (player != null) player.GetComponent<Player>().ReleaseMovement();
            
            if (isTutorialObjectiveDone && !tutorialActivated) StartTutorial();
        }

        if (timepassed - timestart > timefortutorial + timeforintro && tutorialActivated && introNeeded)
        {
            tutorialComplete = true;
            if (ControlsBillboard != null) ControlsBillboard.SetActive(false);
        }

        //Tutorial checks
        //player = GameObject.FindWithTag("ActivePlayer");
        
        //if (player.GetComponent<Animator>().GetBool("introcomplete")) EndTutorial();

        //Cutscene checks
        
        if(GameObject.FindWithTag("IntroCondor") != null && !condorIntroComplete)
        {
            StartCondorCutscene();
        }

        if(GameObject.FindWithTag("Lv1BlackCondor") != null && !condorFled)
        {
            StartCondorFlees();
        }

        if(timeEucalypsoAttack != -1 && timepassed - timeEucalypsoAttack > timeforEucalypso && player != null)
        {
            player.GetComponent<Player>().ReleaseMovement();
        }

        if(GameObject.FindWithTag("Airos") != null && !airosArrived)
        {
            StartAirosCutscene();
        }

        if(timeAirosIntro != -1 && timepassed - timeAirosIntro > timeforAiros && GameObject.FindWithTag("IntroAiros") != null)
        {
            GameObject.FindWithTag("IntroAiros").SetActive(false);
        }
    }

    void StartTutorial()
    {
        //player = GameObject.FindWithTag("ActivePlayer");
        //cutsceneAnimator.SetBool("deployDrone", true);
        tutorialActivated = true;
    }

    void EndTutorial()
    {
        //player.GetComponent<Animator>().SetBool("freezeplayer", true);

    }

    void StartAirosCutscene()
    {
        //Scene1Airos = GameObject.Find("Airos");
        //airosMove = Scene1Airos.GetComponent<Animator>();
        airosMove = GameObject.FindWithTag("IntroAiros").GetComponent<Animator>();
        airosMove.SetBool("playIntro", true);
        airosArrived = true;
        timeAirosIntro = Time.time;
    }

    void StartCondorCutscene()
    {
        sceneCondor = GameObject.FindWithTag("IntroBlackCondor");
        condorMove = sceneCondor.GetComponent<Animator>();
        condorMove.SetBool("startFlyBy", true);
        cutsceneAnimator.SetBool("condorAttack", true);
        condorIntroComplete = true;
    }

    void StartCondorFlees()
    {
        //Scene1BlackCondor = GameObject.Find("Black Condor Lv 1");
        //if(sceneCondor == null) sceneCondor = GameObject.FindWithTag("IntroBlackCondor");
        //sceneCondor.SetActive(false);
        condorMove = Scene1BlackCondor.GetComponent<Animator>();
        cutsceneAnimator.SetBool("eucAttack", true);
        condorMove.SetBool("condorEscape", true);
        //player.GetComponent<Animator>().SetBool("dodgebomb", true);
        player.GetComponent<Player>().SeizeMovement();
        timeEucalypsoAttack = Time.time;
        condorFled = true;
    }

    public void TutorialTimeDone()
    {
        //tutorialComplete = true;
        cutsceneAnimator.SetBool("deployDrone", false);
    }
    public void CondorIntroDone()
    {
        condorIntroComplete = true;
    }
}
