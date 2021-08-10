using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For use in player's turrets with auto-aiming feature.
public class PlayerTurret : PlayerGunFire
{
    protected Transform target;
    public float rotateSpeed = 1f;
    protected float rotateAmount; //public for better testing

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        try
        {
            target = GameObject.FindGameObjectWithTag("Enemy").transform;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log(e);
            target = null;
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        try
        {
            target = GameObject.FindGameObjectWithTag("Enemy").transform;
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log(e);
            target = null;
        }
        // Mode switching between auto and manual
        if (Input.GetMouseButtonDown(1))
        {
            auto = !auto;
        }
    }

    // Turret code
    public void FixedUpdate()
    {
        Vector2 direction;
        if (target != null && auto)
        {
            // If auto, rotate towards target
            direction = (Vector2)target.position - (Vector2)transform.position;
        }
        else
        {
            // If in manual mode or target not found, rotate to regular position
            direction = (Vector2)transform.parent.transform.up;
        }
        // Homing missile code for aiming
        direction.Normalize();
        if (Vector3.Dot(direction, transform.up) <= 0)
        {
            if (Vector3.Cross(direction, transform.up).z >= 0)
            {
                rotateAmount = 1;
            }
            else
            {
                rotateAmount = -1;
            }
        }
        else
        {
            rotateAmount = Vector3.Cross(direction, transform.up).z;
        }
        float curRot = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, curRot - rotateSpeed * rotateAmount));
    }
}
