using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrod : MonoBehaviour
{
    //Components to function as a boss
    public Rigidbody2D rig;
    public Rigidbody2D turretRig1;
    public Rigidbody2D turretRig2;
    public Rigidbody2D turretRigMain;
    public Rigidbody2D flak1;
    public Rigidbody2D flak2;

    //Keep track of the Track objects so that movement is done right.
    public GameObject track1;
    public GameObject track2;
    bool isWorking1;
    bool isWorking2;

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
    bool targetOnBack = false; // Check if reversing

    //Add death animation
    public ParticleSystem crater = null;
    public ParticleSystem explosion = null;
    public float explosionDuration = 2f;
    protected Animator terrodAnimator;

    public void Start()
    {
        isWorking1 = true;
        isWorking2 = true;
        originalRotation = transform.eulerAngles.z;
        try
        {
            target = GameObject.FindGameObjectWithTag("ActivePlayer").transform;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log(e);
            target = null;
        }
    }

    public void Update()
    {
        //Try to find the next player plane when it spawns
        try
        {
            target = GameObject.FindGameObjectWithTag("ActivePlayer").transform;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log(e);
            target = null;
        }
        // Track behavior
        isWorking1 = track1.GetComponent<TerrodTracks>().TracksWorking();
        isWorking2 = track2.GetComponent<TerrodTracks>().TracksWorking();
        // Stop movement when a track is not working, else activate it
        if (!isWorking1 || !isWorking2)
        {
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rig.constraints = RigidbodyConstraints2D.None;
        }
        // "Death" upon destruction of all mounted weapons
        if (turretRigMain == null && turretRig1 == null && turretRig2 == null && flak1 == null && flak2 == null)
        {
            //Set the correct progression bool to true
            Progression.ReportLevelCompleted(1);
            //Death animation
            terrodAnimator = gameObject.GetComponent<Animator>();
            GameObject.Find("TextEffectsTerrod").GetComponent<Animator>().SetBool("BossDoomed", true);
            terrodAnimator.SetBool("PlayDeathAnimation", true);
        }
    }

    public void FixedUpdate()
    {
        //Changing tank's movement
        if (target != null)
        {
            Vector2 distance = (Vector2)target.position - rig.position;
            targetOnBack = Vector2.Dot(distance, -transform.up) < 0;
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
        if (Vector3.Dot(direction, -transform.up) <= 0)
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
            rotateAmount = Vector3.Cross(direction, -transform.up).z;
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
                rig.velocity = -transform.up * (curVelocity + moveSpeed * Time.deltaTime / accelTime);
                // Velocity capping
                if (rig.velocity.magnitude > moveSpeed)
                {
                    rig.velocity = -transform.up * moveSpeed;
                }
            }
            else
            {
                rig.velocity = -transform.up * moveSpeed;
            }
        }
    }

    public void Stop()
    {
        // Deceleration
        Vector2 prevVelocity = new Vector2(rig.velocity.x, rig.velocity.y);
        if (rig.velocity.magnitude > 0)
        {
            if (Vector2.Dot(prevVelocity, -transform.up) < 0)
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
                rig.velocity = transform.up * (curVelocity + reverseSpeed * Time.deltaTime / decelTime);
                // Velocity capping
                if (rig.velocity.magnitude > reverseSpeed)
                {
                    rig.velocity = (transform.up) * reverseSpeed;
                }
            }
            else
            {
                rig.velocity = (transform.up) * reverseSpeed;
            }
        }
    }

    // Check if tank is reversing
    public bool IsReversing()
    {
        return Vector3.Dot(rig.velocity, -transform.up) < 0;
    }

    public void TerrodDeath()
    {
        //Add crater
        if (crater != null)
        {
                ParticleSystem curCrater = Instantiate(crater, this.transform.position, explosion.transform.rotation) as ParticleSystem;
                curCrater.Play(true);
        }

        //Play explosion
        if (explosion != null)
        {
            ParticleSystem curExplosion = Instantiate(explosion, this.transform.position, explosion.transform.rotation) as ParticleSystem;
            var main = curExplosion.main;
            main.simulationSpeed = main.duration / explosionDuration;
            curExplosion.Play(true);
        }

        //Actually destroy object
        Destroy(gameObject);
    }
}
