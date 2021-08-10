using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAsynchronousLauncher : SecondaryWeapon
{
    public int firstLauncher = 0;
    public float timeBetweenShots = 0.5f;
    private float subTimer;
    private int curLauncher;
    private bool activated = false;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        curLauncher = firstLauncher;
    }

    public new void Update()
    {
        base.Update();
        // Reset cooldown slider as needed between sub-shots
        if (activated && cooldownSlider != null && ammo > 0 && active)
        {
            cooldownSlider.SetMax(timeBetweenShots);
            cooldownSlider.SetHealth(subTimer);
        }
        // Update timers
        subTimer += Time.deltaTime;
        // Once reload time is reached, activate firing
        if (timer >= waitTime && active)
        {
            activated = true;
            timer -= waitTime;
        }
        // If subtimer reaches time between shots, fire each launcher until first launcher is reached again
        if (subTimer >= timeBetweenShots && activated && Input.GetKey(KeyCode.Space) && active)
        {
            Fire(bulletSpawns[curLauncher]);
            curLauncher = (curLauncher + 1) % bulletSpawns.Length;
            if (curLauncher == firstLauncher)
            {
                activated = false;
            }
            subTimer = 0;
            timer = 0; // First-order reload time is reload time since last shot for player launchers
        }
    }
}
