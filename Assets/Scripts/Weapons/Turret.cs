using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Finish and Test
public class Turret : Gun
{
    public float rotateSpeed = 1f;
    protected float rotateAmount; //public for better testing
    public bool limitRotation = false; // Whether to limit the rotation of the gun/turret
    public float limitRotationCW = 180; // How much the turret can rotate clockwise from starting position
    public float limitRotationCCW = 180; // How much the turret can rotate counter-clockwise from starting position
    protected float originalRotation;
    public Transform target;


    // Initialize rigid body
    public new void Start()
    {
        base.Start();
        originalRotation = transform.localEulerAngles.z;
    }

    // Update is called once per frame
    public new void Update() {
        base.Update();
    }

    public void FixedUpdate()
    {
        if (target != null)
        {
            // Homing missile code for aiming
            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
            direction.Normalize();
            if (Vector3.Dot(direction, transform.up) <= 0)
            {
                if (Vector3.Cross(direction, transform.up).z >= 0)
                {
                    rotateAmount = 1;
                }
                else
                {
                    rotateAmount = -1;
                }
            }
            else
            {
                rotateAmount = Vector3.Cross(direction, transform.up).z;
            }
            float curRot = transform.localEulerAngles.z - originalRotation;
            // Limit retrieved angle to +- pi for math.
            if (curRot > 180)
            {
                curRot = -360 + curRot;
            }
            else if (curRot < -180)
            {
                curRot = 360 + curRot;
            }
            float rotationAfter = curRot - rotateSpeed * rotateAmount;

            // Check if turret will be in the allowed rotation range. If not, snap.
            if (limitRotation && rotationAfter > limitRotationCCW)
            {
                transform.localEulerAngles = new Vector3(0, 0, originalRotation + limitRotationCCW);
            }
            else if (limitRotation && rotationAfter < -limitRotationCW)
            {
                transform.localEulerAngles = new Vector3(0, 0, originalRotation - limitRotationCW);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0, originalRotation + rotationAfter);
            }
        }
    }
}
