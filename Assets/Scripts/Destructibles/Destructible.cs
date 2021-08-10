using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destructible : MonoBehaviour
{
    public float health;
    protected float maxHealth;
    public float defense;
    protected Billboard enemyHealth;
    public HealthBar healthBar; // Health bar
    public HealthBar defenseBar; // Transparent DEFENSE bar

    // Explosion effects
    public ParticleSystem explosion = null;
    protected ExplosionChain explosionChain;
    public bool hasExplosionChain = false;
    public float explosionDuration = 2f;
    public ParticleSystem crater;
    public bool groundCrater = false;
    public GameObject coin;
    protected Animator deathAnimation;
    public bool hasAnimator = false;

    public GameObject parent;
    protected DestructibleSpawn spawner = null;

    protected Rigidbody2D rb;
    public Vector3 CenterOfMass;

    // Defense bar stuff
    protected bool nonPenetration = false;
    protected float penetrationTimer = 0f;
    protected float penetrationTime = 0.1f;

    // Kinds of objects the destructible can deal collision damage with (see tags)
    public string[] collidableTags; // Can be ActivePlayer, Player, Ally, Enemy, etc.

     ObjectivesSystem objSys;

    // Start is called before the first frame update
    public void Awake()
    {
        // Grab rigidbody and healthbars
        rb = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Billboard>() != null)
            {
                enemyHealth = child.gameObject.GetComponent<Billboard>();
                healthBar = enemyHealth.transform.GetChild(0).GetComponent<HealthBar>();
                defenseBar = enemyHealth.transform.GetChild(1).GetComponent<HealthBar>();
            }
        }
        // Set health and center of mass
        if (CenterOfMass != null)
            rb.centerOfMass = CenterOfMass;

        if (defenseBar != null)
        {
            defenseBar.SetMax(defense);
            Color defenseColor = defenseBar.transform.GetChild(0).GetComponent<Image>().color;
            defenseColor.a = 2f / (1f + Mathf.Exp(-defense / 2)) - 1f;
            defenseBar.transform.GetChild(0).GetComponent<Image>().color = defenseColor;
        }

        if (maxHealth > 0)
            health = maxHealth;
        else
            maxHealth = health;

        if (healthBar != null)
        {
            healthBar.SetMax(maxHealth);
        }
        // Register explosion chain as a death effect
        if (hasExplosionChain)
        {
            explosionChain = GetComponent<ExplosionChain>();
        }

        if(transform.gameObject.GetComponent<Player>() == null)
          objSys = GameObject.Find("HUD").GetComponent<ObjectivesSystem>();
    }

    public void Start()
    {
    }

    public void Update()
    {
    }

    // Damage calculations
    public virtual void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            health -= damage;
            if (healthBar != null)
                healthBar.SetHealth(health);
            if (defenseBar != null)
                defenseBar.SetHealth(defense * health / maxHealth);
        }
        else if (damage < 0 && defenseBar != null)
        {
            defenseBar.SetHealth(-damage * health / maxHealth);
        }
        if (health <= 0)
        {
            if (hasAnimator == true)
            {
                deathAnimation = gameObject.GetComponent<Animator>();
                DeathAnimationProcess();
            }

            else StartCoroutine(Die());
        }
    }

    // Death animation processor, more for specific enemies
    public void DeathAnimationProcess()
    {
        deathAnimation.SetBool("PlayDeathAnimation", true);
    }

    // Destruction function
    public IEnumerator Die()
    {
        //Add crater
        if (crater != null)
        {
            if (groundCrater == false)
            {
                crater.Play(true);
            }
            else
            {
                ParticleSystem curCrater = Instantiate(crater, this.transform.position, explosion.transform.rotation) as ParticleSystem;
                curCrater.Play(true);
            }
        }

        // Detonate Explosion comp for kamikaze enemies
        if (gameObject.GetComponent<Explosion>() != null)
        {
            gameObject.GetComponent<Explosion>().Detonate();
        }

        //Spawn coin if supposed to
        if (coin != null)
            Instantiate(coin, transform.position, transform.rotation);

        //Play explosion effect
        if (explosion != null)
        {
            ParticleSystem curExplosion = Instantiate(explosion, this.transform.position, explosion.transform.rotation) as ParticleSystem;
            var main = curExplosion.main;
            float newSpeed = main.duration / explosionDuration;
            main.simulationSpeed = main.duration / explosionDuration;
            ParticleSystem[] childrenParticleSytems = curExplosion.gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem child in childrenParticleSytems)
            {
                var childmain = child.main;
                childmain.simulationSpeed = newSpeed;
            }
            curExplosion.Play(true);
        }

        // Play explosion chain
        if (hasExplosionChain)
        {
            explosionChain.TriggerExplosionChain();
            float waitTime = explosionChain.explosionTiming * (explosionChain.explosion.Length * explosionChain.timesRepeated - 1f) + 0.9f; // 2 is the default explosion time
            yield return new WaitForSeconds(waitTime);
        }

        // Report to spawner that it's dead, if eligible
        if (spawner)
        {
            spawner.SetAlive(false);
        }

        //Actually destroy object
        if (transform.gameObject.GetComponent<Player>() != null)
            transform.gameObject.GetComponent<Player>().Die(); //Hopefully this works
        else
        {
            objSys.DestroyUpdate(transform.name);
            Destroy(gameObject);
        }

        yield return new WaitForEndOfFrame();
    }

    // Disable weapons
    public void DisableWeapons()
    {
        Gun[] guns = GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            gun.Disable();
        }
    }

    public void EnableWeapons()
    {
        Gun[] guns = GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            gun.Enable();
        }
    }
    public void DisableWeaponsFor(float seconds)
    {
        Gun[] guns = GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            StartCoroutine(gun.DisableFor(seconds));
        }
    }

    // Getters
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    // Setters
    public void SetSpawner(DestructibleSpawn spawner)
    {
        this.spawner = spawner;
    }
}
