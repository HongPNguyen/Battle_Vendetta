using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartTankMovements : MonoBehaviour
{
    protected Rigidbody2D rig;
    //Movement
    public float moveSpeed = 0.5f;
    public float rotateSpeed = 0.01f;
    public float reverseSpeed = 0.35f;
    public float optimumDistance = 15f; // Optimum distance to player before stopping
    public float accelTime = 1f;  // Acceleration time (until maximum engine power in either direction)
    public float decelTime = 0.5f;  // Deceleration time
    protected float rotateAmount; //public for better testing
    protected float originalRotation;
    protected Transform target;
    // Kinds of targets the bullet is effective towards (see tags)
    public string[] targetTags; // Can be ActivePlayer, Player, Ally, Enemy, etc.
    protected float distanceToTarget = Mathf.Infinity;
    bool targetOnBack = false; // Check if reversing

    public float disableAfter = -1;
    public Transform leavePoint;
    protected float timer = 0;

    public new void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        originalRotation = transform.localEulerAngles.z;
        // May have to change player target to something else for allies
        FindClosestTarget();
    }

    public void OnEnable()
    {
        FindClosestTarget();
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
        timer += Time.deltaTime;
        // Follow waypoint to leave instead of chasing player if after time specified
        if (timer >= disableAfter && disableAfter > 0)
            target = leavePoint;
        else
            FindClosestTarget();
    }

    public void FixedUpdate()
    {
        //Changing tank's movement
        if (target != null)
        {
            Vector2 distance = (Vector2)target.position - rig.position;
            targetOnBack = Vector2.Dot(distance, transform.up) < 0;
            // Move forward only if distance to player is larger than "optimum" distance and player is in front
            if (distance.magnitude > optimumDistance && !targetOnBack)
            {
                Run();
            }
            // Else reverse, but again only if closer than optimum distance
            else if (distance.magnitude < optimumDistance || targetOnBack)
            {
                Reverse();
            }
            // Else simply rotate towards player
            else
            {
                Stop();
                Rotate();
            }
        }
    }

    // Custom rotation
    public void Rotate()
    {
        // Rotate
        Vector2 direction = ((Vector2)target.position - rig.position).normalized;
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
        transform.localEulerAngles = new Vector3(0, 0, originalRotation + rotationAfter);
    }

    // Custom acceleration and deceleration (ASSUME SPRITE FACING DOWNWARD!!!)
    public void Run()
    {
        // If reversing, stop first
        if (IsReversing())
        {
            Stop();
        }
        else
        {
            // Rotate when running
            Rotate();
            // Acceleration
            float curVelocity = rig.velocity.magnitude;
            if (curVelocity < moveSpeed)
            {
                rig.velocity = transform.up * (curVelocity + moveSpeed * Time.deltaTime / accelTime);
                // Velocity capping
                if (rig.velocity.magnitude > moveSpeed)
                {
                    rig.velocity = transform.up * moveSpeed;
                }
            }
            else
            {
                rig.velocity = transform.up * moveSpeed;
            }
        }
    }

    public void Stop()
    {
        // Deceleration
        Vector2 prevVelocity = new Vector2(rig.velocity.x, rig.velocity.y);
        if (rig.velocity.magnitude > 0)
        {
            if (Vector2.Dot(prevVelocity, transform.up) < 0)
            {
                rig.velocity = prevVelocity - prevVelocity.normalized * (reverseSpeed * Time.deltaTime / decelTime);
            }
            else
            {
                rig.velocity = prevVelocity - prevVelocity.normalized * (moveSpeed * Time.deltaTime / accelTime);
            }
        }
        // Velocity capping, also rotate when "completely" stopped
        if (Vector2.Dot(rig.velocity, prevVelocity) < 0)
        {
            rig.velocity = new Vector2(0, 0);
        }
        // Rotate when tank achieves "stability" (active engine power = 10% total engine power)
        if ((!IsReversing() && rig.velocity.magnitude < moveSpeed / 10) || (IsReversing() && rig.velocity.magnitude < reverseSpeed / 10))
        {
            Rotate();
        }
    }

    public void Reverse()
    {
        // Stop first if still going forward
        if (!IsReversing() && rig.velocity.magnitude > 0)
        {
            Stop();
        }
        else
        {
            // Also rotate
            Rotate();
            // Reverse until reverseSpeed
            float curVelocity = rig.velocity.magnitude;
            if (curVelocity < reverseSpeed)
            {
                rig.velocity = -transform.up * (curVelocity + reverseSpeed * Time.deltaTime / decelTime);
                // Velocity capping
                if (rig.velocity.magnitude > reverseSpeed)
                {
                    rig.velocity = (-transform.up) * reverseSpeed;
                }
            }
            else
            {
                rig.velocity = (-transform.up) * reverseSpeed;
            }
        }
    }

    // Check if tank is reversing
    public bool IsReversing()
    {
        return Vector3.Dot(rig.velocity, transform.up) < 0;
    }
}
