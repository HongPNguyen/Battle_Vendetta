// A very simple script to rotate an object for aesthetic effects.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed = 60f;
    public bool randomAngles = false;

    // Start is called before the first frame update
    void Start()
    {
        if (randomAngles)
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}
