/* HOMING MISSILE WITH TIMER */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO improve interface implementation.
public class HomingMissile : Bullet
{
    public float rotateSpeed = 10f;
    protected float rotateAmount;          //public for better testing
    protected float originalRotation;

    protected Transform target;
    protected float distanceToTarget = Mathf.Infinity;

    protected float timer = 0f;
    public float rotationTime = 5f;

    public bool headingDown = false;    // Whether the sprite is heading down
    public bool timed = true;           // Whether the homing is only activated for a certain time

    public new void Start()
    {
        base.Start();
        originalRotation = transform.localEulerAngles.z;
        // May have to change player target to something else for allies
        FindClosestTarget();
        timer = 0f;
    }

    public void OnEnable()
    {
        FindClosestTarget();
        timer = 0f;
    }

    // Function to find closest target across EVERY tag
    public void FindClosestTarget()
    {
        try
        {
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            GameObject closest = null;
            foreach (string tag in targetTags)
            {
                GameObject[] gos;
                gos = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject go in gos)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = go;
                        distance = curDistance;
                    }
                }
            }
            target = closest.transform;
            distanceToTarget = Mathf.Sqrt(distance);
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log(e);
            target = null;
            distanceToTarget = Mathf.Infinity;
        }
    }

    public new void Update()
    {
        base.Update();
        FindClosestTarget();
    }

    //Handles the physics and math for the homing missile
    public void FixedUpdate()
    {
        if (target != null)
        {
            // When homing is active
            if (timer < rotationTime || !timed)
            {
                // Calculate rotateAmount (relative to maximum rotateSpeed) using cross product math
                Vector2 direction = (Vector2)target.position - rb.position;
                direction.Normalize();
                if ((Vector3.Dot(direction, transform.up) <= 0 && !headingDown) || (Vector3.Dot(direction, -transform.up) <= 0 && headingDown))
                {
                    if (headingDown)
                    {
                        if (Vector3.Cross(direction, -transform.up).z >= 0)
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
                        if (Vector3.Cross(direction, transform.up).z >= 0)
                        {
                            rotateAmount = 1;
                        }
                        else
                        {
                            rotateAmount = -1;
                        }
                    }
                }
                else
                {
                    if (headingDown)
                    {
                        rotateAmount = Vector3.Cross(direction, -transform.up).z;
                    }
                    else
                    {
                        rotateAmount = Vector3.Cross(direction, transform.up).z;
                    }
                }
                // Update timer if time-limited
                if (timed)
                {
                    timer += Time.deltaTime;
                }
            }
            // If homing is inactive, stop rotation
            else
            {
                rotateAmount = 0;
            }
            // Set rotation angle by rotating by rotateSpeed * rotateAmount
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
            transform.localEulerAngles = new Vector3(0, 0, originalRotation + rotationAfter);
        }
        else
        {
            rb.angularVelocity = 0;
        }

        // Move missile forward using velocity
        if (headingDown)
        {
            rb.velocity = -transform.up * speed;
        }
        else
        {
            rb.velocity = transform.up * speed;
        }
    }
}
