using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsClassification : MonoBehaviour
{
    // Gun classification params
    [System.Serializable]
    public enum Category
    {
        Enemy = 0,
        Primary = 1,
        SecondaryAir = 2,
        SecondaryGround = 3
    }

    [System.Serializable]
    public enum Grade
    {
        Light = 0,
        Medium = 1,
        Heavy = 2,
        Special = 3
    }

    [System.Serializable]
    public enum Type
    {
        RPM = 0,
        Damage = 1,
        Penetration = 2
    }

    public Type type;
    public Grade grade;
    public Category category;

    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

    }
}
