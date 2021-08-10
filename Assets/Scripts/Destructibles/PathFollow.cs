// AI script for path-following enemies. Based on Airos.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    [SerializeField]
    private Path[] paths;

    private int routeToGo = 0;
    private float tParam;
    private Vector2 AirPosition;

    private bool coroutineAllowed;
    protected Rigidbody2D rb;
    protected float timer;
    public float waitBeforeFirstPath = 0f;

    // Params for optional features
    public bool repeating = false;
    public bool rotateTowardsTarget = true;
    public string[] targetTags;
    protected Transform target;
    protected float distanceToTarget = Mathf.Infinity;
    protected float rotateAmount;
    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        routeToGo = 0;
        tParam = 0f;
        coroutineAllowed = true;
        if (rotateTowardsTarget)
            FindClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        // Wait until WaitBeforeFirstPath before going.
        if (timer <= waitBeforeFirstPath)
        {
            timer += Time.deltaTime;
            if (timer >= waitBeforeFirstPath && coroutineAllowed)
                StartCoroutine(Go());
        }
        if (rotateTowardsTarget)
            FindClosestTarget();
    }

    // Rotate towards playercode
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

    void FixedUpdate()
    {
        if (target != null && rotateTowardsTarget)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            if (Vector3.Dot(direction, transform.up) <= 0)
            {
                rotateAmount = 1f;
            }
            else
            {
                rotateAmount = Vector3.Cross(direction, transform.up).z;
            }
            float curRot = transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, curRot - rotateSpeed * rotateAmount));
        }
    }

    private IEnumerator Go()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            Path path = paths[i];
            for (int routeToGo = 0; routeToGo <= path.pathObject.childCount - 1; routeToGo++)
            {
                coroutineAllowed = false;

                Vector2 p0 = path.pathObject.GetChild(routeToGo).GetChild(0).position;
                Vector2 p1 = path.pathObject.GetChild(routeToGo).GetChild(1).position;
                Vector2 p2 = path.pathObject.GetChild(routeToGo).GetChild(2).position;
                Vector2 p3 = path.pathObject.GetChild(routeToGo).GetChild(3).position;

                while (tParam < 1)
                {
                    tParam += Time.deltaTime * path.bezierSpeed;   /// requires some form of speed variable
                    Vector2 lastPosition = transform.position;
                    AirPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                               3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                               3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                                Mathf.Pow(tParam, 3) * p3;

                    //transform.position = AirPosition;
                    rb.velocity = AirPosition - lastPosition;
                    // If not rotating towards target, rotate towards direction of movement
                    if (!rotateTowardsTarget)
                    {
                        var dir = AirPosition - lastPosition;
                        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    }
                    yield return new WaitForEndOfFrame();
                }

                tParam = 0;

                if (i == paths.Length - 1 && routeToGo > path.pathObject.childCount - 1 && repeating)
                    i = 0;
            }
            // If reached the end of path, temporarily resets velocity to 0.
            // Velocity might be increased back to follow next path later.
            rb.velocity = new Vector2(0, 0);
            coroutineAllowed = true;
            yield return new WaitForSeconds(path.delayBeforeNextPath);
        }
    }

    // Represent a single path
    [System.Serializable]
    private class Path
    {
        public Transform pathObject;
        public float delayBeforeNextPath = 0f; // Timer is the time the wave will be started AFTER THE BATTLE STARTS
        public float bezierSpeed = 0.1f; // = 1 / (Time to finish a Bezier Curve)
    }
}
