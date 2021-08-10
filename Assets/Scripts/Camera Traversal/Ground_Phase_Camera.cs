// Script to control camera of Level 1. ADD ONE TO EVERY PHASE NUMBER SINCE WE HAVE TWO INTRO BATTLES!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Ground_Phase_Camera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vcam1, //PHASES 3 & 4
        vcam2, vcam3, vcam4;

    [SerializeField]
    private Transform[] Path;

    int path;

    private int routeToGo;

    private float tParam;

    private Vector2 AirPosition;

    public float speed; // 6 to 9

    public float P3_speed;

    public float P4_speed;
    public float P4_speed_2 = 0.088f;

    public float timer = 0;

    private bool coroutineAllowed;

    private Scene1Controller sceneController;

     public EdgeCollider2D edge; //Test

    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        coroutineAllowed = true;
        speed = P3_speed;
        sceneController = GameObject.Find("SceneController").GetComponent<Scene1Controller>();
        // Initialize which path to take based on phase
        switch (sceneController.GetPhase())
        {
            case 5:
                path = 1;
                break;
            case 6:
                path = 2;
                break;
            default:
                path = 0;
                break;
        }
    }

    //Update is called once per frame
    void Update()
    {
        // Count time for debug purposes
        timer += Time.deltaTime;

        if (vcam1.Priority == 1 || vcam2.Priority == 1 || vcam3.Priority == 1)
        {
            if (coroutineAllowed)
            {
                StartCoroutine(GoByTheRoute(routeToGo));
            }
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {

        coroutineAllowed = false;

        // add if statements if(path==# && routeNumber == #){ change [speed] value for each route as needed. 
        // Phase 4 - route 1 (before fence) camera
        if (path == 1 && routeNumber == 0)
        {
            speed = P4_speed;
        }
        // Phase 4 - route 2 camera
        if (path == 1 && routeNumber == 1)
        {
            vcam2.Priority = 0;
            vcam3.Priority = 1;
            speed = P4_speed_2;
            edge.enabled = true;
        }
        //Path --> route --> position point.  what this code is doing
        if (0 <= routeNumber && routeNumber < Path[path].childCount)
        {
            Vector2 p0 = Path[path].GetChild(routeNumber).GetChild(0).position;
            Vector2 p1 = Path[path].GetChild(routeNumber).GetChild(1).position;
            Vector2 p2 = Path[path].GetChild(routeNumber).GetChild(2).position;
            Vector2 p3 = Path[path].GetChild(routeNumber).GetChild(3).position;

            while (tParam < 1)
            {
                tParam += Time.deltaTime * speed;   /// requires some form of speed variable

                Vector2 lastPosition = transform.position;

                AirPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                           3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                           3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                            Mathf.Pow(tParam, 3) * p3;

                transform.position = AirPosition;

                yield return new WaitForEndOfFrame();
            }

            tParam = 0;

            routeToGo += 1;

            Debug.Log("Path: " + path + " Route: " + routeNumber + " Time: " + timer);
        }

        // Only switch to Phase 4 camera if Phase 3 (length of 66 seconds - 2 seconds pre-phase-3 transition) has passed.
        if (routeToGo > Path[path].childCount - 1 && sceneController.GetPhase() == 5)
        { ///was routes.Length still counts the number of routes
            routeToGo = 0;
            path = 1;
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }

        coroutineAllowed = true;

        // Switch to Airos camera
        if (routeToGo > Path[path].childCount - 1 && sceneController.GetPhase() == 6)
        {
            path = 2;
            vcam3.Priority = 0;
            vcam4.Priority = 1;
            edge.enabled = false;
               Debug.Log("Turning the smaller collider off.");
            coroutineAllowed = false;
        }
        else
        {
            coroutineAllowed = true;
        }

    }

}
