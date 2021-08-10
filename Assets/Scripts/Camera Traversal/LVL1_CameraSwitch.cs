using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//NOTE: Allows you to obtain the !!!!references'!!! to the "Virtual Cameras".

public class LVL1_CameraSwitch : MonoBehaviour
{


    [SerializeField]
    private CinemachineVirtualCamera vcam1;

    [SerializeField]
    private CinemachineVirtualCamera vcam2;

    [SerializeField]
    private CinemachineVirtualCamera vcam3;

    bool Phase1 = true;
    bool PhaseN = false;
    bool Phase2 = false;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchPriority();

        }
    }

    private void SwitchPriority()
    {

        if (Phase1)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
            Phase1 = !Phase1;
            PhaseN = !PhaseN;

        }
        else if (PhaseN)
        {

            vcam2.Priority = 0;
            vcam3.Priority = 1;
            PhaseN = !PhaseN;

        }
        else
        {

            vcam3.Priority = 0;
            vcam1.Priority = 1;
            Phase1 = !Phase1;

        }

    }

}