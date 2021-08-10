using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_camera : MonoBehaviour
{
    public Rigidbody2D rig;
    Vector2 movement;
    public float moveSpeed = 2f;
    float StopTime = 20f;
    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        movement.y = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (rig != null)
        {
            //Fix basic movement AI
            if (timer < StopTime)
                rig.MovePosition(rig.position + movement * moveSpeed * Time.fixedDeltaTime); // will be adjusted

            if (timer >= StopTime)
                movement.y = 0;
        }
    }
}
