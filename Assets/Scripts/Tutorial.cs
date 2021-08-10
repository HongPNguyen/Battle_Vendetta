using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    bool shootGun = false;
    bool moveUp = false;
    bool moveDown = false;
    bool turnLeft = false;
    bool turnRight = false;
    bool objectivesComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!shootGun)
        {
            shootGun = Input.GetKey("mouse 0");
        }
        if (!moveUp)
        {
            moveUp = Input.GetKey("w");
        }
        if (!moveDown)
        {
            moveDown = Input.GetKey("s");
        }
        if (!turnLeft)
        {
            turnLeft = Input.GetKey("a");
        }
        if (!turnRight)
        {
            turnRight = Input.GetKey("d");
        }

        if (shootGun && moveUp && moveDown && turnLeft && turnRight) objectivesComplete = true;
    }

    public bool ObjectivesDone()
    {
        return objectivesComplete;
    }
}
