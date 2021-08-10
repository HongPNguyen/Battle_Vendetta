using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Destructible
{
    public float mass = 10;

    // Start is called before the first frame
    public new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
    }

    // Collision behavior
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision was with a viable target
        foreach (string targetTag in collidableTags)
        {
            if (collision.collider.tag == targetTag)
            {
                //TODO add hit effect
                Destructible e = collision.collider.GetComponent<Destructible>();

                // Hit behavior (allow first frame tolerance so bullet doesn't collide with shooter upon spawn)
                if (e != null)
                {
                    // For simplicity, collision damage is not scaled with angle of contact since the "shell" is big
                    // Hence, there's some part of it that can be considered normal to the armor

                    // Calculate effective damage [Damage = mass1 * mass2 * relative velocity ^ 2 / 256 - effectiveDefense]
                    // Scaling factor by 1/256 to base damage to account for mass and velocity.
                    Rigidbody2D erb = e.gameObject.GetComponent<Rigidbody2D>();
                    float edamage = mass * erb.mass * Mathf.Pow((rb.velocity - erb.velocity).magnitude, 2) / 256 - e.defense;
                    float damage = edamage + e.defense - defense;
                    Debug.Log("Collision Damage" + damage);
                    e.TakeDamage(edamage);
                    TakeDamage(damage);
                }
            }
        }
    }
}
