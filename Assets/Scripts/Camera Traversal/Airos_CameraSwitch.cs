using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//NOTE: Allows you to obtain the !!!!references'!!! to the "Virtual Cameras".

public class Airos_CameraSwitch : MonoBehaviour
{


    [SerializeField]
    private CinemachineVirtualCamera vcam1;

    [SerializeField]
    private CinemachineVirtualCamera vcam2;

    bool Phase1 = true;

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

        }
        else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;

        }
        Phase1 = !Phase1;
    }

}