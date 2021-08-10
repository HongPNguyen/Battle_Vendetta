/** Script for the Black Condor as he appears in Level 1
 * The Black Condor will attempt to evade incoming bullets
 * and other objects defined by the "tagsToAvoid" field.
 * It strafes left or right to avoid the object,
 * and strafes slowly or quickly depending on how far the object is.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCondorLv1 : Enemy
{
    public string[] tagsToAvoid = { "ActivePlayer", "Ally" };
    protected Transform target;
    protected Collider2D collider;
    protected float distanceToTarget;
    protected Vector3 directionToTarget;
    protected float originalRotation = 0;
    public float scanRadius = 20f; // If a bullet is inside this radius, attempts to avoid
    public float maxSpeed = 12f;
    public float accelSpeed = 25f;
    public float decelSpeed = 25f;
    public float screenWidth = 29f;
    protected float leftBound;
    protected float rightBound;
    public float center = 90f;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        collider = gameObject.GetComponent<Collider2D>();
        originalRotation = transform.localEulerAngles.z;
        center = transform.position.x;
        leftBound = center - screenWidth / 2;
        rightBound = center + screenWidth / 2;
        // May have to change player target to something else for allies
        FindClosestTarget();
    }

    public void OnEnable()
    {
        FindClosestTarget();
    }

    // Function to find closest non-bullet object to avoid across EVERY tag
    public void FindClosestTarget()
    {
        try
        {
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            GameObject closest = null;
            foreach (string tag in tagsToAvoid)
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
            directionToTarget = (target.position - transform.position).normalized;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log(e);
            target = null;
            distanceToTarget = Mathf.Infinity;
            directionToTarget = new Vector3(0, 1, 0);
        }
    }

    // Function to find closest allied bullets
    public void FindClosestBullet()
    {
        LayerMask mask = LayerMask.GetMask("Player Bullets");
        Collider2D[] bulletsInSight = Physics2D.OverlapCircleAll(transform.position, scanRadius, mask);
        foreach (Collider2D bullet in bulletsInSight)
        {
            float distanceToBullet = collider.Distance(bullet).distance;
            if (distanceToBullet < distanceToTarget)
            {
                distanceToTarget = distanceToBullet;
                directionToTarget = -collider.Distance(bullet).normal;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        FindClosestBullet();
    }

    // Avoidant strafing movement
    void FixedUpdate()
    {
        if (0 < distanceToTarget && distanceToTarget <= scanRadius && Vector3.Dot(transform.up, directionToTarget) <= 0.3)
        {
            // There's an object to avoid in the scanning radius
            if (Vector3.Cross(transform.up, directionToTarget).z >= -0.05 && transform.position.x < rightBound)
            {
                rb.velocity = transform.right * maxSpeed * Mathf.Min(1, scanRadius / (distanceToTarget * 10f));
            }
            else if (Vector3.Cross(transform.up, directionToTarget).z < -0.05 && transform.position.x > leftBound)
            {
                rb.velocity = -transform.right * maxSpeed * Mathf.Min(1, scanRadius / (distanceToTarget * 10f));
            }
            else
            {
                FlyTowardsCenter();
            }
        }
        else
        {
            // There's no object to avoid, stop
            FlyTowardsCenter();
        }
    }

    public void Stop()
    {
        // Deceleration
        Vector2 prevVelocity = new Vector2(rb.velocity.x, rb.velocity.y);
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity = prevVelocity - prevVelocity.normalized * (decelSpeed * Time.deltaTime);
        }
        // Velocity capping
        if (Vector2.Dot(rb.velocity, prevVelocity) < 0)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void FlyTowardsCenter()
    {
        // If to the right of center, fly left
        if (transform.position.x > center)
        {
            Vector2 unitLeft = new Vector2(-1, 0);
            if (rb.velocity.x > 0)
            {
                Stop();
            }
            else
            {
                if (rb.velocity.magnitude < maxSpeed)
                    rb.velocity += unitLeft * (accelSpeed * Time.deltaTime);
                if (rb.velocity.magnitude > maxSpeed)
                    rb.velocity = unitLeft * maxSpeed;
                float newPos = transform.position.x - rb.velocity.x;
                if (newPos < center)
                    Stop();
            }
        }
        else
        {
            // Else fly right
            Vector2 unitRight = new Vector2(1, 0);
            if (rb.velocity.x < 0)
            {
                Stop();
            }
            else
            {
                if (rb.velocity.magnitude < maxSpeed)
                    rb.velocity += unitRight * (accelSpeed * Time.deltaTime);
                if (rb.velocity.magnitude > maxSpeed)
                    rb.velocity = unitRight * maxSpeed;
                float newPos = transform.position.x + rb.velocity.x;
                if (newPos > center)
                    Stop();
            }
        }
    }
}
