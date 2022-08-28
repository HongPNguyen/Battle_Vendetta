using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for basic laser behavior with charging
// IMPORTANT: Please make sure laser is scaled by 1 in WORLD SCALE for correct rendering

public class Laser : MonoBehaviour
{
    // Effect variables
    public ParticleSystem laserStartParticles;
    public ParticleSystem prelaserStartParticles;
    public ParticleSystem laserEndParticles;
    public LineRenderer line;
    public LineRenderer preLine;
    protected bool startParticlesPlaying = false;
    protected bool prestartParticlesPlaying = false;
    protected bool endParticlesPlaying = false;
    protected float origScale;
    protected float scale;

    // Timing variables
    public float timeBetweenFiring = 10;
    public float preLaserDuration = 2;
    public float laserDuration = 2;
    public float laserLength = 25f;
    public LayerMask layerMask;

    // Timer variables
    protected float startTime;
    protected float timePassed;
    protected float timeDifference;

    // Laser damage variables
    public float power; // Maximum power per second
    public float laserEfficiency; // k term in Sigmoid function

    public bool piercing = false;

    // Start is called before the first frame update
    void Start()
    {
        //line = GetComponent<LineRenderer>();
        startTime = Time.time;
        line.SetPosition(1, new Vector3(laserLength, 0, 0));
        prelaserStartParticles.Stop(true);
        laserStartParticles.Stop(true);
        preLine.enabled = false;
        line.enabled = false;
        origScale = line.startWidth;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed = Time.time;
        timeDifference = timePassed - startTime;
        if ((timeDifference > timeBetweenFiring) ||
            (timeDifference > preLaserDuration && prestartParticlesPlaying == true) ||
            (timeDifference > laserDuration && startParticlesPlaying == true))
        {
            startTime = Time.time;

            // Charging beam start
            if (prestartParticlesPlaying == false && startParticlesPlaying == false)
            {
                prestartParticlesPlaying = true;
                prelaserStartParticles.Play(true);
                //prelaserStartParticles.gameObject.transform.position = transform.position;
                preLine.enabled = true;
                preLine.SetPosition(1, new Vector3(laserLength, 0, 0));
                return;
            }

            // Laser start
            if (prestartParticlesPlaying == true && (timeDifference > preLaserDuration))
            {
                prestartParticlesPlaying = false;
                startParticlesPlaying = true;
                prelaserStartParticles.Stop(true);
                laserStartParticles.Play(true);
                //laserStartParticles.gameObject.transform.position = transform.position;
                preLine.enabled = false;
                line.enabled = true;

                // Scale laser for visual realism
                /*scale = Time.deltaTime * (2f / (1f + Mathf.Exp(-timeDifference * laserEfficiency)) - 1f) * origScale;
                AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0, scale);
                curve.AddKey(1, scale);
                line.widthCurve = curve;*/
                return;
            }

            // Laser stop
            if (startParticlesPlaying == true && (timeDifference > laserDuration))
            {
                // If start particles (laser's function part) have stopped, end the laser and reset scale
                AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0, origScale);
                curve.AddKey(1, origScale);
                line.widthCurve = curve;
                startParticlesPlaying = false;
                laserStartParticles.Stop(true);
                line.enabled = false;
                return;
            }
        }
    }

    void FixedUpdate()
    {
        // Hit effect
        if (startParticlesPlaying == true)
        {
            // If piercing laser, ray cast all and deal damage to each target
            if (piercing)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, laserLength, layerMask);
                foreach (RaycastHit2D hit in hits)
                {
                    Destructible e = hit.collider.gameObject.GetComponent<Destructible>();
                    if (e != null)
                    {
                        // Take damage by Sigmoid function
                        e.TakeDamage((power - e.defense) * Time.deltaTime * (2f / (1f + Mathf.Exp(-timeDifference * laserEfficiency)) - 1f));
                    }
                }
            }
            else
            // If not a piercing, use normal ray cast
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, laserLength, layerMask);
                if (hit.collider != null)
                {
                    Destructible e = hit.collider.gameObject.GetComponent<Destructible>();
                    if (e != null)
                    {
                        // Shorten laser to hit point only
                        float distance = (hit.point - (Vector2)transform.position).magnitude;
                        line.SetPosition(1, new Vector3(distance, 0, 0));
                        // Take damage by Sigmoid function
                        e.TakeDamage((power - e.defense) * Time.deltaTime * (2f / (1f + Mathf.Exp(-timeDifference * laserEfficiency)) - 1f));
                        //Move impact particles to correct position
                        laserEndParticles.gameObject.transform.position = hit.point;
                        //Start impact particles
                        if (endParticlesPlaying == false)
                        {
                            laserEndParticles.Play(true);
                            endParticlesPlaying = true;
                        }
                    }
                }
                // If hit not detected, laser has normal length and hit timer is reset (i.e. target is no longer being cut through)
                else
                {
                    line.SetPosition(1, new Vector3(laserLength, 0, 0));
                    //Ensure impact particles are off.
                    laserEndParticles.Stop(true);
                    endParticlesPlaying = false;
                }
            }
        }
        else
        {
            laserEndParticles.Stop(true);
            endParticlesPlaying = false;
        }
    }
}
