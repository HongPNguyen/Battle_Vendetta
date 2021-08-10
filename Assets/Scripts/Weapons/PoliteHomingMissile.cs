/** A homing missile that steers away from allies.
 * Should only work on allies on the same layer.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliteHomingMissile : HomingMissile
{
    public string[] allyTags;
    public float avoidRadius; // Only bother to avoid if ally is in certain range

    protected Transform ally;
    protected float distanceToAlly = Mathf.Infinity;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        FindClosestAlly();
    }

    public new void OnEnable()
    {
        base.OnEnable();
        FindClosestAlly();
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        FindClosestAlly();
    }

    // Function to find closest target across EVERY ally tag
    public void FindClosestAlly()
    {
        try
        {
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            GameObject closest = null;
            foreach (string tag in allyTags)
            {
                GameObject[] gos;
                gos = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject go in gos)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance && curDistance > 0) // Exclude self
                    {
                        closest = go;
                        distance = curDistance;
                    }
                }
            }
            ally = closest.transform;
            distanceToAlly = Mathf.Sqrt(distance);
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log(e);
            ally = null;
            distanceToAlly = Mathf.Infinity;
        }
    }

    public new void FixedUpdate()
    {
        /* -- PRIORITIZE AVOIDING RATHER THAN SEEKING, but only if object to avoid is within a certain distance
        AND smaller than distance to target -- //
        Avoiding math is much the same as seeking, but uses dot product instead of cross product. */
        if (ally == null || distanceToAlly > avoidRadius || distanceToAlly > distanceToTarget)
        {
            // Seek
            base.FixedUpdate();
        }
        else
        {
            // Avoid
            if (ally != null)
            {
                // When homing is active
                if (timer < rotationTime || !timed)
                {
                    // Always steer away from ally at maximum speed to give space
                    Vector2 direction = (Vector2)ally.position - rb.position; // Direction from self to ally
                    direction.Normalize();
                    rotateAmount = 1;
                    /* Determine rotation direction based on direction to ally
                    (Already tilting left relative to ally --> Continue to rotate left, etc.)
                    Rotate towards target direction if ally is straight ahead. */
                    if ((Vector3.Cross(direction, -transform.up).z < 0 && headingDown) || (Vector3.Cross(direction, transform.up).z < 0 && !headingDown))
                    {
                        rotateAmount = -rotateAmount;
                    }
                    else if ((Vector3.Cross(direction, -transform.up).z == 0 && headingDown) || (Vector3.Cross(direction, transform.up).z == 0 && !headingDown))
                    {
                        direction = (Vector2)target.position - rb.position;
                        direction.Normalize();
                        if ((Vector3.Cross(direction, -transform.up).z > 0 && headingDown) || (Vector3.Cross(direction, transform.up).z > 0 && !headingDown))
                        {
                            rotateAmount = -rotateAmount;
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
                float curRot = transform.localRotation.eulerAngles.z;
                // Rotate AWAY from ally so add rotation instead of subtracting it.
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotateSpeed * rotateAmount));
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
}
