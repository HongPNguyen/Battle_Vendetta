// Behave mostly like a Gun but spawns other enemies instead of bullets.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : Gun
{
    // Overridden Fire method for minions
    protected override void Fire(Transform bulletSpawn, GameObject shellType)
    {
        // Fire only if ammo is not 0
        if (ammo != 0)
        {
            // Account for spread by generating random angle
            float curRot = bulletSpawn.rotation.eulerAngles.z;
            Quaternion bulletAngle = Quaternion.Euler(new Vector3(0, 0, curRot + Random.Range(-spread / 2, spread / 2)));
            // Create bullet
            GameObject bullet = Object.Instantiate(shellType) as GameObject;
            if (bullet != null)
            {
                bullet.transform.position = bulletSpawn.transform.position;
                bullet.transform.rotation = bulletAngle;
                bullet.SetActive(true);
                // Recoil
                Rigidbody2D parent = transform.parent.GetComponent<Rigidbody2D>();
                if (parent != null)
                {
                    parent.AddForce(-recoilForce * bulletSpawn.up, ForceMode2D.Force);
                }
                // Deplete ammo if not unlimited
                if (ammo > 0)
                {
                    ammo--;
                }
            }
        }
    }
}
