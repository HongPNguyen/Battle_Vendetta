// Specific script for Black Condor's Fire Airmine weapon
// MUST BE USED TOGETHER WITH FireAirmineGun FOR OFFENSIVE FEATURES!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAirmine : Enemy
{
    public float dropSpeed = 5f;
    public float rotateSpeed = 60f;

    // Gun params for firing flame waves
    public GameObject shellType;
    public Transform[] bulletSpawns;
    public float initialWait = 1f; // Time to wait before starting first shot
    public float timeBetweenShots = 0.5f;
    public float spread = 2f;

    // Timers
    protected float timer = 0f;
    protected float subTimer = 0f;
    protected bool activated = false;
    protected bool finalShotActivated = false;
    protected int curLauncher = 0;

    // Just move down and rotate
    public new void Start()
    {
        base.Start();
        // Set to drop and spin at random angle
        rb.velocity = new Vector2(0, -dropSpeed);
        transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }

    public void Update()
    {
        // Update timers
        timer += Time.deltaTime;
        subTimer += Time.deltaTime;

        // Once reload time is reached, activate firing
        if (timer >= initialWait)
        {
            activated = true;
            timer = 0;
        }

        // If subtimer reaches time between shots, fire each launcher until first launcher is reached again
        if (subTimer >= timeBetweenShots && activated)
        {
            // If final shot activated, fire all launchers and explode
            if (finalShotActivated)
            {

                foreach (Transform bulletSpawn in bulletSpawns)
                {
                    Fire(bulletSpawn);
                }
                finalShotActivated = false;
                activated = false;
                StartCoroutine(Die());
            }
            else
            {
                Fire(bulletSpawns[curLauncher]);
                curLauncher = (curLauncher + 1) % bulletSpawns.Length;
                // Once every launcher has been fired, activate final shot
                if (curLauncher == 0)
                {
                    finalShotActivated = true;
                }
            }
            subTimer = 0;
        }
    }

    public void FixedUpdate()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    // Stock method to fire specific bullet at a specific gun
    protected void Fire(Transform bulletSpawn)
    {
        // Account for spread by generating random angle
        float curRot = bulletSpawn.rotation.eulerAngles.z;
        Quaternion bulletAngle = Quaternion.Euler(new Vector3(0, 0, curRot + Random.Range(-spread / 2, spread / 2) + 225f));
        // Create flame wave
        GameObject bullet = Object.Instantiate(shellType);
        if (finalShotActivated)
        {
            bullet.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (bullet != null)
        {
            bullet.transform.position = bulletSpawn.transform.position;
            bullet.transform.rotation = bulletAngle;
            bullet.SetActive(true);
        }
    }
}
