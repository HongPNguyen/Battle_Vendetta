using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemy : Enemy
{
    public Transform gun;
    public GameObject shellType;
    public float bulletSpeed;
    public float waitTime = 5f;
    float timer = 0f;

    //Movement Variables
    public float moveSpeed = 5f;
    public GameObject player;
    public Rigidbody2D rig;
    Vector3 playerPos;
    Vector2 movement;
    float moveWaitTime = 5f;
    float moveTimer = 0f;

    //public float fireRate;
    //public float spread; //In degrees
    //public float powerBuff;
    //public float speedBuff

    new void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
    }

    new void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            Fire();
            timer = timer - waitTime;
        }

        if (rig != null)
        {
            playerPos = player.transform.position;
            moveTimer += Time.deltaTime;
            if (playerPos.x > rig.position.x)
            {
                movement.x = 1;
            }
            else if (playerPos.x < rig.position.x)
            {
                movement.x = -1;
            }
            else
            {
                movement.x = 0;
            }
        }
    }

    void FixedUpdate()
    {
        if (rig != null)
        {
            //Fix basic movement AI
            if (moveTimer < moveWaitTime / 2)
                rig.MovePosition(rig.position + movement * moveSpeed * Time.fixedDeltaTime);

            if (moveTimer >= moveWaitTime)
                moveTimer = 0f;
        }
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(shellType, gun.position, gun.rotation);
        Rigidbody2D rig = bullet.GetComponent<Rigidbody2D>();
        rig.AddForce(gun.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
