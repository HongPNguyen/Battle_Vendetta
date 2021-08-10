using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : Destructible
{
    //To get access to scriptable objects information
    public Transform HUD;
    public Card cards;
    protected Text nameText;
    protected HealthBar cooldownSlider;
    protected Text secondaryAmmo;
    public float maxSpeed = 5f;
    public float enginePower = 1000f;
    Vector2 movement;
    Vector2 moveDir;
    bool playerControl = true;
    public Collider2D area;
    //Values for rotation
    protected Camera cam;
    Vector2 mousePos;
    // Active weapons stuff
    public int activeSecondaryWeapon = 0;
    public int activeShellGroup = 0;
    public int numberOfSecondaryWeapons = 0;
    public int numberOfShellGroups = 1;

    int isDestroyed;
    public LineSet lineSetToUse;

    //Add death animation

    public int drg = 10;

    int sx = 1;  //stop speed variable
    int sy = 1;


    public bool left, right, up, down = false; // for the triggers

    // DEV MODE DEV MODE DEV MODE!!! - "V" key to activate
    public bool devModeUnlocked = false;
    protected bool devMode = false; // Unlimited (9999) power
    protected bool invincible = false;

    new void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (SceneManager.GetActiveScene().name != "Hangar")
        {
            //Debug.Log("Start assigning values for the player");
            // Initialize HUD components
            HUD = GameObject.Find("HUD").GetComponent<Transform>();
            cam = Camera.main;
            healthBar = FindTag("HealthBar").GetComponent<HealthBar>();
            defenseBar = FindTag("DefenseBar").GetComponent<HealthBar>();
            cooldownSlider = FindTag("Cooldown").GetComponent<HealthBar>();
            nameText = FindTag("NameText").GetComponent<Text>();
            secondaryAmmo = FindTag("Ammo").GetComponent<Text>();
            // Base start
            base.Start();
            // To display name on HealthDock
            nameText.text = cards.name;
            if (maxHealth > 0)
                health = maxHealth;
            else
                maxHealth = health;
            isDestroyed = 1;
            // Set cooldown slider 
        }
    }

    public void SetUp()
    {
        if (SceneManager.GetActiveScene().name != "Hangar")
            Start();
    }

    // Update is called once per frame
    // Input
    new void Update()
    {
        if (SceneManager.GetActiveScene().name != "Hangar")
        {
            base.Update();

            // Movement
            movement.x = Input.GetAxisRaw("Horizontal") * sx;
            movement.y = Input.GetAxisRaw("Vertical") * sy;

            // Get Mouse Position
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            // Update active secondary weapon
            if (Input.GetKeyDown(KeyCode.LeftShift) && numberOfSecondaryWeapons > 0)
            {
                activeSecondaryWeapon = (activeSecondaryWeapon + 1) % numberOfSecondaryWeapons;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) && numberOfSecondaryWeapons > 0)
            {
                activeSecondaryWeapon = (activeSecondaryWeapon + numberOfSecondaryWeapons - 1) % numberOfSecondaryWeapons;
            }

            // Update active shell group
            if (Input.mouseScrollDelta.y > 0 && numberOfSecondaryWeapons > 0)
            {
                activeShellGroup = (activeShellGroup + 1) % (numberOfShellGroups + 1);
            }
            else if (Input.mouseScrollDelta.y < 0 && numberOfSecondaryWeapons > 0)
            {
                activeShellGroup = (activeShellGroup + numberOfShellGroups - 1) % (numberOfShellGroups + 1);
            }
        }

        // Dev mode activate
        if(devModeUnlocked && Input.GetKeyDown(KeyCode.V))
        {
            devMode = !devMode;
        }
        
        if(devModeUnlocked && Input.GetKeyDown(KeyCode.B))
        {
            invincible = !invincible;
        }
        /* if (Input.GetKeyDown(KeyCode.LeftArrow)) // left  !!!CANNOT TAKE TWO KEYS AT ONCE!!!
         {
             stack.Push(1);
         }
         if (Input.GetKeyDown(KeyCode.RightArrow)) //right
         {
             stack.Push(3);
         }
         if (Input.GetKeyDown(KeyCode.DownArrow)) //down
         {
             stack.Push(4);
         }
         if (Input.GetKeyDown(KeyCode.UpArrow)) //up
         {
             stack.Push(2);
         }*/
    }

    //Movement
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "Hangar" && playerControl)
        {
            movement.Normalize();
            // Move in the direction specified, then force the speed back to max speed if it is already reached.
            // (Provided the max speed is due to moment and not knockback or external factor)
            rb.AddForce(movement * enginePower, ForceMode2D.Force);
            moveDir = rb.velocity / rb.velocity.magnitude;
            if (rb.velocity.magnitude > maxSpeed && movement.magnitude > 0)
            {
                rb.velocity = maxSpeed * moveDir;
            }

            //Rotate the Player
            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    // need to define which collider was hit instead of movment direction.

    void OnTriggerEnter2D(Collider2D other)  //for edge collider.  OnTriggerExit for polygon and box collider
    {  //OnCollisionEnter2D  runs this code


        if (other.gameObject.name == "left")
        {
            rb.drag = drg;
            sx = 0;
            left = true;
        }
        if (other.gameObject.name == "right")
        {
            rb.drag = drg;
            sx = 0;
            right = true;
        }
        if (other.gameObject.name == "up")
        {
            rb.drag = drg;
            sy = 0;
            up = true;
        }
        if (other.gameObject.name == "down")
        {
            rb.drag = drg;
            sy = 0;
            down = true;
        }


        Debug.Log("enter");
    }  //proof that the code ran.


    void OnTriggerStay2D(Collider2D other)  //for edge collider.  OnTriggerExit for polygon and box collider
    {  //OnCollisionEnter2D  runs this code
        if (left == true)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                rb.drag = 3;
                sx = 1;

            }
            else
            {
                rb.drag = drg;
                sx = 0;
            }
        }
        if (right == true)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                rb.drag = 3;
                sx = 1;

            }
            else
            {
                rb.drag = drg;
                sx = 0;
            }
        }
        if (up == true)
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                rb.drag = 3;
                sy = 1;

            }
            else
            {
                rb.drag = drg;
                sy = 0;
            }
        }
        if (down == true)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                rb.drag = 3;
                sy = 1;
            }
            else
            {
                rb.drag = drg;
                sy = 0;
            }
        }

        Debug.Log("stay");
        //proof that the code ran.
    }

    void OnTriggerExit2D(Collider2D other)  //for edge collider.  OnTriggerExit for polygon and box collider
    {  //OnCollisionEnter2D  runs this code
        if (other.gameObject.name == "left")
        {
            sx = 1;
            left = false;
        }
        if (other.gameObject.name == "right")
        {
            sx = 1;
            right = false;
        }
        if (other.gameObject.name == "up")
        {
            sy = 1;
            up = false;
        }
        if (other.gameObject.name == "down")
        {
            sy = 1;
            down = false;
        }

        Debug.Log("exit");
    }

    //Player Death
    public new void Die()
    {
        if (isDestroyed == 1 && !invincible)
        {
            //Play death animation
            HUD.GetComponent<Narration>().ChangeLineSet(lineSetToUse);
            transform.gameObject.SetActive(false);
            isDestroyed = 0;
        }
    }

    // Getters and setters for non-Inspector-editable fields
    public HealthBar getCooldownSlider()
    {
        return cooldownSlider;
    }

    public Text getSecondaryAmmo()
    {
        return secondaryAmmo;
    }

    public int GetIsDestroyed()
    {
        return isDestroyed;
    }

    public void setCooldownSlider(HealthBar input)
    {
        cooldownSlider = input;
    }

    public void setNameText(Text input)
    {
        nameText = input;
    }

    public void setSecondaryAmmo(Text input)
    {
        secondaryAmmo = input;
    }

    public bool IsDevMode()
    {
        return devMode;
    }

    // Other helper functions
    public Transform FindTag(string tag)
    {
        foreach (Transform child in HUD)
        {
            if (child.tag == tag)
            {
                return child;
            }
        }
        return null;
    }

    public void SeizeMovement()
    {
        playerControl = false;
    }
    public void ReleaseMovement()
    {
        playerControl = true;
    }
}
