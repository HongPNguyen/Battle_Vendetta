using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialFire : SecondaryWeapon
{
    protected Player player;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        // References
        player = transform.parent.parent.GetComponent<Player>();
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        // DEV MODE - Set power to 9999, fire, then reset
        if (player.IsDevMode())
        {
            dev = true;
            Fire();
        }
        else
        {
            // Fire normally
            dev = false;
            Fire();
        }
        timer = 0f;
    }
}
