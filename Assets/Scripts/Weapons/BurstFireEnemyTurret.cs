// An enemy turret, but also fires in bursts.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFireEnemyTurret : EnemyTurret
{
    public float timeBetweenShots;
    public float burstLength;
    protected float subTimer = 0;
    protected bool activated = true;
    public new void Start()
    {
        base.Start();
        timer = -burstLength;
    }

    public new void Update()
    {
        //Try to find the next player plane when it spawns
        base.Update();

        // Update timers
        timer += Time.deltaTime;
        subTimer += Time.deltaTime;

        // Deactivate firing once burstLength is reached
        if (timer >= 0 && activated)
        {
            activated = false;
        }

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
    }
}
