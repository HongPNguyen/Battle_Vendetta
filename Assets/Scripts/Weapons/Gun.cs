// Class to be used for every gun in the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Gun : WeaponsClassification
{
    public GameObject[] shellTypes;
    public float waitTime = 5f; // Time between shots in seconds. Inverse of fire rate.
    protected float timer = 0f;

    public float spread; // In degrees
    public float powerBuff; // In portion of base damage
    public float speedBuff; // In portion of base damage
    public float recoilForce = 0f;

    public int ammo = -1; // Negative for infinite
    protected int maxAmmo;

    // Dev mode
    protected bool dev = false;

    // Shoot when not moving feature
    public bool shootWhenNotMoving = false;
    public float movementEpsilon = 0.3f; // When slower than this, considered "not moving"
    protected float speed;

    // Shoot when target visible feature
    public bool shootWhenTargetVisible = false;
    public string[] targetTags;
    public float visibleRangeInDegrees = 90f;
    public float visibleDistance = 50f;

    public bool firingEnabled = true;

    // Code to have variable bulletSpawns
    public Transform[] bulletSpawns;

    // Audio source to play gunshot sounds
    public AudioSource soundSource;
    // Array of audio clips for multiple gunshot sound effects
    public AudioClip[] gunShots;
    System.Random rand = new System.Random();

    public new void Start()
    {
        base.Start();
        if (maxAmmo > 0)
            ammo = maxAmmo;
        else
            maxAmmo = ammo;

          // Try to assign
          if (SceneManager.GetActiveScene().name != "Hangar")
          {
               try
               {
                //soundSource = GameObject.Find("GameManager").transform.GetComponent<AudioSource>();
                soundSource = gameObject.GetComponent<AudioSource>();
               }
               catch (System.Exception e)
               {
                    Debug.LogException(e, this);
               } 
          }
    }

    public void SetUp()
    {
        this.Start();
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        // Update timer
        timer += Time.deltaTime;

        if (timer >= waitTime &&
            (CalculateSpeed() < movementEpsilon || !shootWhenNotMoving) &&
            (IsTargetVisible() || !shootWhenTargetVisible) && firingEnabled)
        {
            Fire();
            timer = 0;
        }
    }

    // Function
    // ConeCast function to find targets in a cone
    public static RaycastHit2D[] ConeCastAll(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle, string tag)
    {
        RaycastHit2D[] sphereCastHits = Physics2D.CircleCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance);
        List<RaycastHit2D> coneCastHitList = new List<RaycastHit2D>();

        if (sphereCastHits.Length > 0)
        {
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                if (sphereCastHits[i].transform.gameObject.tag == tag)
                {
                    Vector3 hitPoint = sphereCastHits[i].point;
                    Vector3 directionToHit = hitPoint - origin;
                    float angleToHit = Vector3.Angle(direction, directionToHit);

                    if (angleToHit < coneAngle)
                    {
                        coneCastHitList.Add(sphereCastHits[i]);
                    }
                }
            }
        }

        RaycastHit2D[] coneCastHits = new RaycastHit2D[coneCastHitList.Count];
        coneCastHits = coneCastHitList.ToArray();

        return coneCastHits;
    }

    // Check if there's a target in the visible range.
    public bool IsTargetVisible()
    {
        foreach (string tag in targetTags)
        {
            if (ConeCastAll(transform.position, visibleDistance, transform.up, visibleDistance, visibleRangeInDegrees / 2, tag).Length > 0)
            {
                return true;
            }
        }
        return false;
    }

    // Calculate speed
    public float CalculateSpeed()
    {
        // Calculate speed during update for "shoot when not moving" feature
        // Return velocity from either parent or grandparent
        // The carrier should really not be the great-grandparent of the gun or something like that
        if (transform.parent.gameObject.GetComponent<Rigidbody2D>() != null)
            return transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        else
            return transform.parent.parent.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
    }

    // Shoot from each bulletSpawn
    public void Fire()
    {
        foreach (Transform bulletSpawn in bulletSpawns)
        {
            Fire(bulletSpawn);
        }
    }

    protected void Fire(Transform bulletSpawn)
    {
        Fire(bulletSpawn, shellTypes[0]);
    }

    // Stock method to fire specific bullet for all guns
    protected void Fire(GameObject shellType)
    {
        foreach (Transform bulletSpawn in bulletSpawns)
        {
            Fire(bulletSpawn, shellType);
        }
    }

    // Stock method to fire specific bullet at a specific gun
    protected virtual void Fire(Transform bulletSpawn, GameObject shellType)
    {
        // Fire only if ammo is not 0
        if (ammo != 0)
        {
            // Account for spread by generating random angle
            float curRot = bulletSpawn.rotation.eulerAngles.z;
            Quaternion bulletAngle = Quaternion.Euler(new Vector3(0, 0, curRot + Random.Range(-spread / 2, spread / 2)));
            // Create bullet
            GameObject bullet = ObjectPoolManager.SharedInstance.GetPooledObject(shellType.name);
            if (bullet != null)
            {
                bullet.transform.position = bulletSpawn.transform.position;
                bullet.transform.rotation = bulletAngle;
                bullet.SetActive(true);
                Rigidbody2D rig = bullet.GetComponent<Rigidbody2D>();
                // Apply speed and power buff if bullet is just created (i.e. not recovered from object pool)
                float bulletSpeed = bullet.GetComponent<Bullet>().speed * (1 + speedBuff);
                bullet.GetComponent<Bullet>().SetCurSpeed(bulletSpeed);
                // DEV MODE - Set power to 9999, fire, then reset
                if (dev)
                {
                    bullet.GetComponent<Bullet>().SetCurPower(9999f);
                    Debug.Log("DEV!!!");
                }
                else
                {
                    bullet.GetComponent<Bullet>().SetCurPower(bullet.GetComponent<Bullet>().power * (1 + powerBuff));
                }
                // Push bullet
                rig.velocity = bulletSpawn.up * bulletSpeed;
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

                if (gunShots.Length > 0)
                {
                    soundSource.pitch = rand.Next(1, 20) % 2f;
                    int soundToUse = rand.Next(0, gunShots.Length);
                    soundSource.PlayOneShot(gunShots[soundToUse]);
                }
            }
        }
    }

    // Disable weapon
    public void Disable()
    {
        firingEnabled = false;
    }

    public void Enable()
    {
        firingEnabled = true;
    }

    public IEnumerator DisableFor(float seconds)
    {
        firingEnabled = false;
        yield return new WaitForSeconds(seconds);
        firingEnabled = true;
    }

    // Getters
    public float GetTimerValue()
    {
        return timer;
    }
}
