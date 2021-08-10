using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airos : Enemy /// Always include "Enemy" and "Die()" function
{
    public float bezierSpeed = 0.1f; // = 1 / (Time to finish a Bezier Curve)
    public float rotateSpeed = 0.01f;
    protected float rotateAmount; //public for better testing
    protected Transform target;
    protected int deathCounter = 0;
    public ParticleSystem smoke;
    public bool endDeathAnimation = false;

    [SerializeField]
    private Transform[] Path;
    private int routeToGo;
    private float tParam;
    private Vector2 AirPosition;
    private bool coroutineAllowed;

   
    //Path randomizer variables and selector
    double p1 = 0.3;
    double p2 = 0.4;
    double p3 = 0.3;

    double Rp1 = 0.4;
    double Rp2 = 0.6;

    double select;
    int start = 1;  /// keep an eye on the start position switching
    int path;

    //Boolean to help prep for deathanimation.
    protected bool isDying = false;


    new void Start()
    {
        base.Start();
        routeToGo = 0;
        tParam = 0f;
        coroutineAllowed = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        Randomize(); // random start path
        // Select player plane to track
        try
        {
            target = GameObject.FindGameObjectWithTag("ActivePlayer").transform;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log(e);
            target = null;
        }
        // Assign explosion chain for death animation
        explosionChain = GetComponent<ExplosionChain>();
    }

    new void Update() //// check on this HERE!!!!!!!!
    {
        base.Update();
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
        if (isDying == true) return;
        else if (coroutineAllowed)
            StartCoroutine(GoByTheRoute(routeToGo));
    }           //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    void FixedUpdate()
    {
        if (deathCounter > 0)
        {
            rb.MoveRotation(deathCounter);
            deathCounter++;

            if (deathCounter == 30)
            {
                smoke.Play(true);
            }

            if (deathCounter == 400)
            {
                endDeathAnimation = true;
            }
        }
        if (target != null && isDying == false)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            if (Vector3.Dot(direction, -transform.up) <= 0)
            {
                rotateAmount = 1f;
            }
            else
            {
                rotateAmount = Vector3.Cross(direction, -transform.up).z;
            }
            float curRot = transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, curRot - rotateSpeed * rotateAmount));
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {

        coroutineAllowed = false;

        //Path --> route --> position point.  what this code is doing

        Vector2 p0 = Path[path].GetChild(routeNumber).GetChild(0).position;  
        Vector2 p1 = Path[path].GetChild(routeNumber).GetChild(1).position;
        Vector2 p2 = Path[path].GetChild(routeNumber).GetChild(2).position;
        Vector2 p3 = Path[path].GetChild(routeNumber).GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * bezierSpeed;   /// requires some form of speed variable

            Vector2 lastPosition = transform.position;

            AirPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                       3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                       3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                        Mathf.Pow(tParam, 3) * p3;

            //transform.position = AirPosition;
            rb.velocity = AirPosition - lastPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0;

        routeToGo += 1;

        if (routeToGo > Path[path].childCount - 1){ ///was routes.Length still counts the number of routes
            routeToGo = 0;
            Randomize();
        }

        coroutineAllowed = true;

    }



    public void Randomize()   // Path randomizer
    {
        select = Random.Range(0.0f, 1.0f);
        if (start == 1)
        {

            if (select <= p1)
            {
                path = 0;

            }
            else if (select < p1 + p2)
            {
                path = 1;
                start = 2;

            }
            else
            {
                path = 2;
                start = 2;
            }
        }
        else
        {
            if (select <= Rp1)
            {
                path = 3;

            }
            else
            {
                path = 4;
                start = 1;
            }
        }
        
    }

    // DeathAnimation stuff
    public new void DeathAnimationProcess()
    {
        GameObject.Find("TextEffectsAiros").GetComponent<Animator>().SetBool("AirosDoomed", true);
        deathAnimation.SetBool("PlayDeathAnimation", true);
    }
    
    public void DeathAnimation()
    {
        deathCounter = 1;
        if (explosionChain != null)
            explosionChain.TriggerExplosionChain();
    }

    public void endDefaultAnimation()
    {
        isDying = true;
    }
}


