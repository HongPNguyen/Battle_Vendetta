using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera_Bound : MonoBehaviour
{
    public Camera MCamera;

    public GameObject left;
    public GameObject right;
    public GameObject up;
    public GameObject down;

    public float scale;
    public Vector3 vleft;
    public Vector3 vright;
    public Vector3 vup;
    public Vector3 vdown;


    Vector2[] points;
    // Start is called before the first frame update


    void Start()
    {
        //Screen.width;
        scale = MCamera.GetComponent<Camera>().orthographicSize / 10;
        vleft = left.transform.localPosition;
        vright = right.transform.localPosition;
        vup = up.transform.localPosition;
        vdown = down.transform.localPosition;

        for (int i = 0; i < 4; i++)
        {
            points = new Vector2[2];

            if (i == 0)
            {
                left.transform.localPosition = vleft * scale;
                EdgeCollider2D edge = left.AddComponent<EdgeCollider2D>();
                points[0] = new Vector2(0, -20);
                points[1] = new Vector2(0, 20);
                edge.points = points;
                edge.edgeRadius = 0.2f;
                edge.isTrigger = true;
            }

            if (i == 1)
            {
                right.transform.localPosition = vright * scale;
                EdgeCollider2D edge = right.AddComponent<EdgeCollider2D>();
                points[0] = new Vector2(0, -20);
                points[1] = new Vector2(0, 20);
                edge.points = points;
                edge.edgeRadius = 0.2f;
                edge.isTrigger = true;
            }

            if (i == 2)
            {
                up.transform.localPosition = vup * scale;
                EdgeCollider2D edge = up.AddComponent<EdgeCollider2D>();
                points[0] = new Vector2(-20,0);
                points[1] = new Vector2(20,0);
                edge.points = points;
                edge.edgeRadius = 0.2f;
                edge.isTrigger = true;
            }

            if (i == 3)
            {
                down.transform.localPosition = vdown * scale;
                EdgeCollider2D edge = down.AddComponent<EdgeCollider2D>();
                points[0] = new Vector2(-20, 0);
                points[1] = new Vector2(20, 0);
                edge.points = points;
                edge.edgeRadius = 0.2f;
                edge.isTrigger = true;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
 
        scale = MCamera.GetComponent<Camera>().orthographicSize / 10;
       
        left.transform.localPosition = vleft * scale;
        right.transform.localPosition = vright * scale;
        up.transform.localPosition = vup * scale;
        down.transform.localPosition = vdown * scale;

    }
}
