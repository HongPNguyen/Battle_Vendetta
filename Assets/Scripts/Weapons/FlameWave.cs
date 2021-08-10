// Script for flame wave to damage the player (and later allies, maybe)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWave : MonoBehaviour
{
    protected ParticleSystem part;
    protected List<ParticleCollisionEvent> collisionEvents;
    public float damagePerSecond = 10f;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Destructible e = other.GetComponent<Destructible>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
                e.TakeDamage((damagePerSecond - e.defense) * Time.deltaTime);
            }
            i++;
        }
    }
}
