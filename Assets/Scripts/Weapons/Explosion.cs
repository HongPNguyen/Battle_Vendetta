using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Finish and TEST
//To be used with Terrod's Flak shells and other explosive GameObjects
public class Explosion : MonoBehaviour
{
    public float power = 5f;
    public float radius = 5f;
    public float effectDuration = 0.5f;
    public ParticleSystem effect;
    public bool startOnAwake = false;

    public float forceThreshold = 3000f;

    public bool decentralized = false; // Whether the explosion deals constant damage among radius.
    public LayerMask layerMask; // Layer to perform explosion on

    // Base explosion's value for effect
    public float baseExplosionRadius = 3f;

    private void Start()
    {
        if (startOnAwake) Detonate();
    }

    // Function to cascade simulation speed to sub-particle-systems.
    private void SetSpeedForChildren(ParticleSystem parent, float newSpeed, float scale)
    {
        ParticleSystem[] childrenParticleSytems = parent.gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem child in childrenParticleSytems)
        {
            var childmain = child.main;
            childmain.simulationSpeed = newSpeed;
            child.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    //May not need
    public void Detonate()
    {
        // Generate explosion effect
        ParticleSystem curEffect = Instantiate(effect, this.transform.position, Quaternion.identity) as ParticleSystem;
        // Adjust explosion effect params and do the same for children effects
        var main = curEffect.main;
        float newSpeed = main.duration / effectDuration;
        main.simulationSpeed = newSpeed;
        float scale = radius / baseExplosionRadius;
        curEffect.transform.localScale = new Vector3(scale, scale, scale);
        SetSpeedForChildren(curEffect, newSpeed, scale);
        curEffect.Play(true);

        // Explosion damage code
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        foreach (Collider2D hit in targets)
        {
            Vector3 distanceVec = hit.transform.position - transform.position;
            float distance = distanceVec.magnitude;
            if (distance > 0)
            {
                // Calculate effective power and force
                float effectivePower;
                float effectiveForce;
                if (decentralized)
                {
                    effectivePower = power;
                    effectiveForce = 0;
                }
                else
                {
                    effectivePower = power / Mathf.Pow(distance, 2);
                    effectiveForce = forceThreshold * Mathf.Pow(radius / distance, 2);
                }
                // Deal explosion damage
                Destructible p = hit.GetComponent<Destructible>();
                if (p != null)
                {
                    p.TakeDamage(Mathf.Min(power,effectivePower) - p.defense);
                }
                // Knockback
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    distanceVec.Normalize();
                    rb.AddForce(effectiveForce * distanceVec, ForceMode2D.Force);
                }
            }
        }
    }
}
