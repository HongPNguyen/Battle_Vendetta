using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrodMainTurret : EnemyTurret
{
    public int attack = 0;
    float beat;
    GameObject activeType;
    int numShellsFiredInAttack;
    public int numShellsPerAttack = 4;
    int activeBulletSpawn = 0;
    System.Random rand;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();

        // Determine which attack pattern the Main Turret starts with
        rand = new System.Random();
        double sentinel = rand.NextDouble(); // NextDouble produces a random double >= 0 and < 1
                                             // Prop (attack 1) = 0.25
                                             // Prop (attack 2) = 0.25
                                             // Prop (attack 3) = 0.5
        if (sentinel < 0.25)
            attack = 1;
        else if (sentinel < 0.5)
            attack = 2;
        else
            attack = 3;

        numShellsFiredInAttack = 0;
        beat = waitTime / bulletSpawns.Length;
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        // Update timer
        timer += Time.deltaTime;
        // Regular fire code for Attacks 1 and 2
        if (attack != 3 && timer >= waitTime)
        {
            Fire();
            timer -= waitTime;
        }
        // Alternative fire code for attack 3 (fire every beat instead of waitTime)
        else if (attack == 3 && timer >= beat) {
            Fire();
            timer -= beat;
        }
    }

    // New Fire function to accomidate for additional attack patterns
    new void Fire()
    {
        // Fire according to attack
        switch (attack)
        {
            case 1:
                // Using shellTypes[0]
                activeType = shellTypes[0];
                base.Fire(activeType);
                numShellsFiredInAttack += bulletSpawns.Length;
                break;
            case 2:
                // Using shellTypes[1]
                activeType = shellTypes[1]; // Make sure shellTypes[1] is not null
                base.Fire(activeType);
                numShellsFiredInAttack += bulletSpawns.Length;
                break;
            case 3:
                // Shoot each gun alternatively, shell type random
                // TODO fix 
                activeType = shellTypes[rand.Next(shellTypes.Length)];
                activeBulletSpawn = (activeBulletSpawn + 1) % bulletSpawns.Length;
                base.Fire(bulletSpawns[activeBulletSpawn], activeType);
                numShellsFiredInAttack++;
                break;
            default:
                // Added a Debug message in case attack is not 1, 2, or 3.
                Debug.Log("The attack value for TerrodMainTurret should not be this value.");
                break;
        }
        // Switch attacks
        if (numShellsFiredInAttack >= numShellsPerAttack)
            SwitchAttack(attack);

    }

    // Switches which of the attacks the Main Cannon will do
    void SwitchAttack(int a)
    {
        // First, reset the counter for the number of bullets spawned for an attack
        numShellsFiredInAttack = 0;
        double sentinel;
        switch (a)
        {
            case 1:
                sentinel = rand.NextDouble(); // NextDouble produces a random double >= 0 and < 1
                if (sentinel < 0.5)
                    attack = 2;
                else
                    attack = 3;
                    timer -= (waitTime - beat); // Timer compensation for alternating shots
                break;
            case 2:
                sentinel = rand.NextDouble(); // NextDouble produces a random double >= 0 and < 1
                if (sentinel < 0.5)
                    attack = 1;
                else
                    attack = 3;
                    timer -= (waitTime - beat); // Timer compensation for alternating shots
                break;
            case 3:
                sentinel = rand.NextDouble(); // NextDouble produces a random double >= 0 and < 1
                if (sentinel < 0.5)
                    attack = 1;
                else
                    attack = 2;
                break;
            default:
                // Added a Debug message in case attack is not 1, 2, or 3.
                Debug.Log("The attack value for TerrodMainTurret should not be this value.");
                break;
        }
    }
}
