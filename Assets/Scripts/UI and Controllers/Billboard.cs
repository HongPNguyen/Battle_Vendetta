using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //This class will help ensure that an enemy's health bar is always above them, and dosen't rotate
    private Transform cam;
    
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
    }

    //Fix so that health bar dosen't rotate.
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
