using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponsClassification
{
    // ORIGINAL stats for use when returned to object pool
    public float power;
    public float speed;

    // Parameters
    protected const float EXPIRATION_RATIO = 0.9f; // Bullet "expires" once 90% of its speed has been lost.

    protected Vector3 origScale;
    protected float curPower;
    protected float curSpeed;
    public float penetration = 0;
    public float deterioration = 0; //ratio/second
    public float selfDestructTime = -1; // Time until self-destruct for effect. Negative to disable
    protected float time = 0;

    // Kinds of targets the bullet is effective towards (see tags)
    public string[] targetTags; // Can be ActivePlayer, Player, Ally, Enemy, etc.

    // Effects
    public ParticleSystem trailEffect;
    public ParticleSystem engineEffect;
    public ParticleSystem coverEffect;
    public ParticleSystem hitEffect;

    protected Rigidbody2D rb;

    // Base explosion's value for effect
    public float hitEffectRadius = 5f;
    public float hitEffectDuration = 0.5f;
    public float baseExplosionRadius = 3f;

    // Start is called before first frame
    public new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        origScale = transform.localScale;
    }

    // Update is called every frame
    public new void Update()
    {
        base.Update();
        time += Time.deltaTime;
        // Speed deterioration, destroy without any behavior if expires
        if (deterioration * time >= EXPIRATION_RATIO)
        {
            time = 0;
            ObjectPoolManager.SharedInstance.ReturnPooledObject(gameObject.name, gameObject);
        }
        if (curSpeed > 0)
        {
            rb.velocity = curSpeed * (1f - deterioration * time) * rb.transform.up;
            // Also shrink bullet for visual realism
            transform.localScale = Mathf.Pow(1f - deterioration * time, 2f/3f) * origScale;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

        // Effect calls
        if (time >= selfDestructTime && selfDestructTime > 0)
        {
            FragShell fs = gameObject.GetComponent<FragShell>();
            Explosion expl = gameObject.GetComponent<Explosion>();

            // Frag shell effect
            if (fs != null)
                fs.Fracture();

            // Explosion effect
            if (expl != null)
                expl.Detonate();

            // Generate hit effect (assuming explosion)
            if (hitEffect != null)
            {
                ParticleSystem curHitEffect = Instantiate(hitEffect, this.transform.position, Quaternion.identity) as ParticleSystem;
                var main = curHitEffect.main;
                main.simulationSpeed = main.duration / hitEffectDuration;
                float scale = hitEffectRadius / baseExplosionRadius;
                curHitEffect.transform.localScale = new Vector3(scale, scale, scale);
                curHitEffect.Play(true);
            }

            // Return shell to object pool
            time = 0;
            ObjectPoolManager.SharedInstance.ReturnPooledObject(gameObject.name, gameObject);
        }
    }

    // Collision behavior
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision was with a viable target
        foreach (string targetTag in targetTags)
        {
            if (collision.collider.tag == targetTag)
            {
                //TODO add hit effect
                Destructible e = collision.collider.GetComponent<Destructible>();

                // Hit behavior (allow first frame tolerance so bullet doesn't collide with shooter upon spawn)
                if (e != null && time > 0)
                {
                    // Effect calls
                    FragShell fs = gameObject.GetComponent<FragShell>();
                    Explosion expl = gameObject.GetComponent<Explosion>();

                    // Calculate effective defense [Effective defense = defense / cos(angle of contact)]
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 5);
                    Vector3 normal = hit.normal;
                    float penCoeff = Mathf.Abs(Vector3.Cross(rb.velocity.normalized, normal).z);
                    // Debug penCoeff for "bullet traps"
                    if (penCoeff < 0.2f)
                    {
                        penCoeff = 1f - penCoeff;
                    }
                    if (float.IsNaN(penCoeff) || float.IsInfinity(penCoeff))
                    {
                        penCoeff = 1f;
                    }
                    float effectiveDefense = e.defense / penCoeff;

                    // Calculate effective damage [Damage = power * (1- det*time)^2 - Max (effective defense - penetration, 0)]
                    float damage = curPower * Mathf.Pow(1f - deterioration * time, 2f) - Mathf.Max(effectiveDefense - penetration, 0);
                    e.TakeDamage(damage);

                    // Determine penetration status
                    if (damage > 0)
                    {
                        // Frag shell effect
                        if (fs != null)
                            fs.Fracture();

                        // Explosion effect
                        if (expl != null)
                            expl.Detonate();

                        // Generate hit effect (assuming explosion)
                        if (hitEffect != null)
                        {
                            ParticleSystem curHitEffect = Instantiate(hitEffect, this.transform.position, Quaternion.identity) as ParticleSystem;
                            var main = curHitEffect.main;
                            main.simulationSpeed = main.duration / hitEffectDuration;
                            float scale = hitEffectRadius / baseExplosionRadius;
                            curHitEffect.transform.localScale = new Vector3(scale, scale, scale);
                            curHitEffect.Play(true);
                        }
                        // Return shell to object pool
                        time = 0;
                        ObjectPoolManager.SharedInstance.ReturnPooledObject(gameObject.name, gameObject);
                    }
                    // Non-penetration, reflect with energy loss.
                    else
                    {
                        time = time + (1f / deterioration - time) * penCoeff; // HAX
                        // Check to make sure velocity can be assigned (THIS IS SOLELY TO HOLD OFF VELOCITY BUG)
                        if (curSpeed > 0)
                            rb.velocity = Vector3.Reflect(curSpeed * (1f - deterioration * time) * rb.velocity.normalized, normal); //Need to fix as a error is causing NaN
                        else
                            rb.velocity = new Vector2(0, 0);
                        //Store new direction
                        Vector3 newDirection = Vector3.Reflect(transform.up, normal);
                        //Rotate bullet to new direction
                        newDirection = Quaternion.Euler(0, 0, -90) * newDirection;
                        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: newDirection);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100000);
                        break;
                    }
                }
            }
        }
    }

    // Getters

    // Setters
    public void SetCurPower(float input)
    {
        curPower = input;
    }

    public void SetCurSpeed(float input)
    {
        curSpeed = input;
    }
}