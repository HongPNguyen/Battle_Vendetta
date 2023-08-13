using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script sets the color of a weapons slot in the hangar based on its grade.
/// </summary>
public class GunSlot : WeaponsClassification
{
    // Reference to Sprite Renderer component, for changing color
    private Renderer rend;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rend = GetComponent<Renderer>();
        switch (grade)
        {
            case Grade.Light:
                rend.material.color = Color.green;
                break;
            case Grade.Medium:
                rend.material.color = Color.yellow;
                break;
            case Grade.Heavy:
                rend.material.color = Color.red;
                break;
            case Grade.Special:
                Color specialColor = new Color(0.5f, 0f, 1f, 1f);
                rend.material.color = specialColor;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
