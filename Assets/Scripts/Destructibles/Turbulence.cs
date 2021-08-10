using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbulence : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    private int routeToGo;

    private float tParam;

    private Vector2 AirPosition;

    public float speed; // 6 to 9

    public float NTTime; // 7 to 15

    public float TTime; // 3 to 5

    public float timer = 0;

    private bool coroutineAllowed;

    private bool Turb = false;

    // Start is called before the first frame update
    void Start()
    {

        routeToGo = 0;
        tParam = 0f;
        coroutineAllowed = true;
        Randomize();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!Turb && timer > NTTime)
        {
                timer = timer - NTTime;
                Turb = true;
        }
        else if(Turb && timer > TTime)
        {
            timer = timer - TTime;
            Turb = false;
            Randomize();
        }

        if (Turb && coroutineAllowed)
            StartCoroutine(GoByTheRoute(routeToGo));
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {

        coroutineAllowed = false;

        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(2).position;
        Vector2 p3 = routes[routeNumber].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speed;   /// requires some form of speed variable

            AirPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                       3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                       3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                        Mathf.Pow(tParam, 3) * p3;

            transform.position = AirPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0;

        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
            routeToGo = 0;


        coroutineAllowed = true;

    }


    public void Randomize()   
    {
        speed = Random.Range(6f, 11f);
        NTTime = Random.Range(7f, 13f);
        TTime = Random.Range(2f, 6f);

    }

}