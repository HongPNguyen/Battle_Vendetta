using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// DOES NOT CONTAIN SHOOTING CODE!
// Must be used with another script that containts shooting
// (Like PlayerSpecialFire or PlayerAsynchronousLauncher)

public class SecondaryWeapon : Gun
{
    public int group = 0; // Every secondary weapon in a group will be fired at once
    protected int activeGroup; // The currently active secondary weapon as dictated by player plane
    protected bool active = false;
    protected HealthBar cooldownSlider;
    protected Text ammoText;
    protected Player parent;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        parent = transform.parent.parent.gameObject.GetComponent<Player>(); // Take grandparent because the direct parent is the gun slot
        // Setup cooldown slider and ammo text
          if (SceneManager.GetActiveScene().name != "Hangar") {
               cooldownSlider = parent.getCooldownSlider();
               ammoText = parent.getSecondaryAmmo();
          }
    }

     // Update is called once per frame
     public new void Update()
     {
          if (SceneManager.GetActiveScene().name != "Hangar")
          {
               // Setup cooldown slider and ammo text
               if (cooldownSlider == null)
               {
                    cooldownSlider = parent.getCooldownSlider();
               }
               if (ammoText == null)
               {
                    ammoText = parent.getSecondaryAmmo();
               }
               // Update timer
               timer += Time.deltaTime;
               // Setup active group
               activeGroup = parent.activeSecondaryWeapon;
               if (group == activeGroup)
               {
                    active = true;
               }
               else
               {
                    active = false;
               }
               // Set cooldown slider
               if (active && cooldownSlider != null)
               {
                    if (ammo != 0)
                    {
                         cooldownSlider.SetMax(waitTime);
                         cooldownSlider.SetHealth(timer);
                    }
                    else
                    {
                         cooldownSlider.SetMax(0);
                    }
               }
               // Set ammo text
               if (active && ammoText != null)
               {
                    if (ammo >= 0)
                    {
                         ammoText.text = ammo.ToString();
                    }
                    else
                    {
                         ammoText.text = "Inf";
                    }
               }
          }
     }
}
