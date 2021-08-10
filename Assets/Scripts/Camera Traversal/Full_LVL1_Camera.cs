// Script to control camera. ADD ONE TO EVERY PHASE NUMBER SINCE WE HAVE TWO INTRO BATTLES!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Full_LVL1_Camera : MonoBehaviour
{

    public GameObject cameraOne;
    public GameObject cameraTwo;
    protected Scene1Controller sceneController;

    AudioListener cameraOneAudioLis;
    AudioListener cameraTwoAudioLis;


    /// GIVE MORE UNIQUE NAMES TO TRANSITION VIRTUAL CAMERA
    [SerializeField]
    private CinemachineVirtualCamera vcam_1_2, // PHASES 1 & 2
        vcam_Ground_Transition,
        vcam_3,  // PHASES 3 & 4
        vcam_4,
        vcam_4_2,
        vcam_Boss_Transition,
        vcam_Boss;

    //cutscene virtual cameras
    // [SerializeField]
    // private CinemachineVirtualCamera ecam1, ecam2, ecam3, ecam4;




    public float timer = 0;
    public float subTimer = 0;
    // Use this for initialization 

    bool Phase1_2 = true;
    bool PhaseN3, Phase3_4, PhaseNB, PhaseB = false;

    //cutscene virtual camera states
    //bool EPhase1, EPhase2, EPhase3, EPhase4, EPhase5 = false;


    // repeat background variables
    public float scrollSpeed;

    //public GameObject[] levels; need 2 for phases 3 and 4
    // assimiate code from Scroll_background

    void Start()
    {
        timer = 0;
        subTimer = 0;
        //Get Camera Listeners
        cameraOneAudioLis = cameraOne.GetComponent<AudioListener>();
        cameraTwoAudioLis = cameraTwo.GetComponent<AudioListener>();

        //Grab scene controller
        sceneController = GameObject.Find("SceneController").GetComponent<Scene1Controller>();


        //Camera Position Set based on checkpoint phase
        cameraPositionChange(0);
        switch (sceneController.GetPhase())
        {
            case 1:
            case 2:
            case 3:
                vcam_1_2.Priority = 1;
                break;
            case 4:
                vcam_3.Priority = 1;
                break;
            case 5:
                vcam_4.Priority = 1;
                break;
            case 6:
                vcam_Boss.Priority = 1;
                break;
            default:
                Debug.Log("ERROR: Invalid phase.");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // virtual camera priority switching

        // MATT!!!!MATT: "cameraPositionChange(1);" FOR THE CUTSCENES   MATT!!!!MATT

        //!!!
        //HONG  time values for when to switch the cameras
        //!!!

        timer += Time.deltaTime;
        if (PhaseN3)
            subTimer += Time.deltaTime;


        //IF THE PLAYER DIES AND CLICKS RETRY
        // CHANGE GET PHASE VALUE ON CUTSCENE IMPLEMENTED!
        if (Phase1_2 && sceneController.GetPhase() == 4) /// TRANSITION to GROUND PHASES
        {
            vcam_1_2.Priority = 0;
            vcam_Ground_Transition.Priority = 1;
            Phase1_2 = false;
            PhaseN3 = true; //transition
        }

        //GROUND_PHASE.CS TAKES OVER
        else if (PhaseN3 && subTimer > 2) //Phase 3 CAMERA ACTIVATE
        {
            vcam_Ground_Transition.Priority = 0;
            vcam_3.Priority = 1;
            PhaseN3 = false;
            Phase3_4 = true;
        }


        // Load checkpoint from Phase 4
        else if (sceneController.GetPhase() == 5)
        {
            vcam_1_2.Priority = 0;
            vcam_Ground_Transition.Priority = 0;
            vcam_3.Priority = 0;
            vcam_4.Priority = 1;
            Phase1_2 = false;
            PhaseN3 = false;
            Phase3_4 = true;
        }

        // BOSS TRANSITION
        if (vcam_4.Priority == 1 && Phase3_4 && sceneController.GetPhase() == 6) //***
        {
            Phase3_4 = false;
            PhaseNB = true;
            vcam_4.Priority = 0;
            vcam_Boss_Transition.Priority = 1;
            timer = 0;

        }

        //AIROS CAMERAS

        else if (PhaseNB && timer > 2 && sceneController.GetPhase() == 6) //TRANSITION TO AIROS FIGHT
        {
            vcam_Boss_Transition.Priority = 0;
            vcam_Boss.Priority = 1;
            PhaseNB = false;
            PhaseB = true;


            // cameraPositionChange(1);
        }





    }
    void LateUpdate()
    {

    }



    //Camera change Logic
    void cameraPositionChange(int camPosition)
    {
        if (camPosition > 1)
        {
            camPosition = 0;
        }

        //Set camera position database
        PlayerPrefs.SetInt("CameraPosition", camPosition);

        //Set camera position 1
        if (camPosition == 0)
        {
            cameraOne.SetActive(true);
            cameraOneAudioLis.enabled = true;

            cameraTwoAudioLis.enabled = false;
            cameraTwo.SetActive(false);
        }

        //Set camera position 2
        if (camPosition == 1)
        {
            cameraTwo.SetActive(true);
            cameraTwoAudioLis.enabled = true;

            cameraOneAudioLis.enabled = false;
            cameraOne.SetActive(false);


        }

    }


    ///insert scroll_background scripts here.
    ///
}
