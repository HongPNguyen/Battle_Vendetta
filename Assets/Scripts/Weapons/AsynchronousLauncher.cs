using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for Airos missiles, where bullets are still fired each reload time,
// But not at the same time

public class AsynchronousLauncher : Gun
{
    public int firstLauncher = 0;
    public float timeBetweenShots = 0.5f;
    private float subTimer;
    private int curLauncher;
    private bool activated = false;

    public new void Start()
    {
        base.Start();
        curLauncher = firstLauncher;
    }

    public new void Update()
    {
        // Update timers
        timer += Time.deltaTime;
        subTimer += Time.deltaTime;
        // Once reload time is reached, activate firing
        if (timer >= waitTime &&
            ((CalculateSpeed() < movementEpsilon) || !shootWhenNotMoving) &&
            (IsTargetVisible() || !shootWhenTargetVisible))
        {
            activated = true;
            timer = 0;
        }
        // If subtimer reaches time between shots, fire each launcher until first launcher is reached again
        if (subTimer >= timeBetweenShots && activated)
        {
            Fire(bulletSpawns[curLauncher]);
            curLauncher = (curLauncher + 1) % bulletSpawns.Length;
            if (curLauncher == firstLauncher)
            {
                activated = false;
            }
            subTimer = 0;
        }
    }
}
