using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Turret
{
    protected float distanceToTarget = Mathf.Infinity;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        targetTags = new string[2];
        targetTags[0] = "ActivePlayer";
        targetTags[1] = "Ally";
        // Initialize player target
        // May have to change player target to something else for allies
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

    // Update is called once per frame
    public new void Update()
    {
        //Try to find the next player plane when it spawns
        FindClosestTarget();
        base.Update();
    }

    public new void FixedUpdate()
    {
        // Perform turret behavior on player plane
        base.FixedUpdate();
    }
}
