using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFire : Gun
{
    public float timeBetweenShots;
    public float burstLength;
    protected float subTimer = 0;
    protected bool activated = false;

    public new void Update()
    {
        // Update timers
        timer += Time.deltaTime;
        subTimer += Time.deltaTime;

        // Once reload time is reached, activate firing
        if (timer >= waitTime &&
            (CalculateSpeed() < movementEpsilon || !shootWhenNotMoving) &&
            (IsTargetVisible() || !shootWhenTargetVisible))
        {
            activated = true;
            timer = -burstLength;
        }

        // If subtimer reaches time between shots, fire until burstLength is reached
        if (subTimer >= timeBetweenShots && activated)
        {
            Fire();
            subTimer = 0;
        }

        // Deactivate firing once burstLength is reached
        if (timer >= 0 && activated)
        {
            activated = false;
        }
    }
}
