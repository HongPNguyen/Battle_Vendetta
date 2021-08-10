using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/PlayerPlanes")]
//This will be a template frr the data to store.

public class Card : ScriptableObject
{
    [System.Serializable]
    public enum CardType
    {
        Plane = 1,
        Gun = 2,
        Shell = 3
    }
    public CardType cardType; // 1 for plane, 2 for gun, 3 for shell
    public GameObject obj;
    public int unlockLevel = 1;  // The LEVEL that the equipment is unlocked in
    public new string name;
    public int cost;
    public Sprite artwork;
    [TextArea]
    public string description;

    // Plane values
    protected string speed;
    protected string defense;
    protected string health;
    protected string mass;

    public int gunSlotNum;
    public GameObject[] gunSlotObj;
    public WeaponsClassification.Type[] slotType;
    public WeaponsClassification.Grade[] slotGrade;
    public WeaponsClassification.Category[] slotCategory;

     public float[] positionScaleX;
     public float[] positionScaleY;
     public float[] hangarScaleX;
     public float[] hangarScaleY;

     //Not a perfect fix to allow for image scaleing after selecting a new weapon, but will do
     public float gunScaleX;
     public float gunScaleY;

    // Gun classification params
    protected WeaponsClassification.Type type;
    protected WeaponsClassification.Grade grade;
    protected WeaponsClassification.Category category;

    // Gun values
    protected string reloadTime;
    protected string powerBuff;
    protected string speedBuff;
    protected string spread;

    // Shell values
    protected string power;
    protected string shellSpeed;
    protected string penetration;
    protected string deterioration;

    // Retrieve info from object
    public void Awake()
    {
        switch ((int)cardType)
        {
            case 1:
                Player plane = obj.GetComponent<Player>();
                Rigidbody2D planeRig = obj.GetComponent<Rigidbody2D>();
                speed = plane.maxSpeed.ToString();
                defense = plane.defense.ToString();
                health = plane.health.ToString();
                mass = planeRig.mass.ToString();

                gunSlotObj = new GameObject[gunSlotNum];
                slotType = new WeaponsClassification.Type[gunSlotNum];
                slotGrade = new WeaponsClassification.Grade[gunSlotNum];
                slotCategory = new WeaponsClassification.Category[gunSlotNum];
                for (int p = 0; p < gunSlotNum; p++)
                {
                    //Debug.Log("Assigning gun slots in the card of " + obj.name);
                    gunSlotObj[p] = obj.transform.GetChild(p).GetChild(0).gameObject;
                    slotType[p] = gunSlotObj[p].GetComponent<Gun>().type;
                    slotGrade[p] = gunSlotObj[p].GetComponent<Gun>().grade;
                    slotCategory[p] = gunSlotObj[p].GetComponent<Gun>().category;
                }
                break;
            case 2:
                Gun gun = obj.GetComponent<Gun>();
                type = gun.type;
                grade = gun.grade;
                category = gun.category;
                reloadTime = gun.waitTime.ToString();
                powerBuff = gun.powerBuff.ToString();
                speedBuff = gun.speedBuff.ToString();
                spread = gun.spread.ToString();
                break;
            case 3:
                Bullet shell = obj.GetComponent<Bullet>();
                type = shell.type;
                grade = shell.grade;
                category = shell.category;
                power = shell.power.ToString();
                shellSpeed = shell.speed.ToString();
                penetration = shell.penetration.ToString();
                deterioration = shell.deterioration.ToString();
                break;
            default:
                Debug.Log("Please give this card the right card type.");
                break;
        }
    }

    // Getters
    public string getSpeed()
    {
        return speed;
    }

    public string getDefense()
    {
        return defense;
    }

    public string getHealth()
    {
        return health;
    }

    public string getMass()
    {
        return mass;
    }

    public WeaponsClassification.Type getType()
    {
        return type;
    }

    public WeaponsClassification.Grade getGrade()
    {
        return grade;
    }

    public WeaponsClassification.Category getCategory()
    {
        return category;
    }

    public string getReloadTime()
    {
        return reloadTime;
    }

    public string getPowerBuff()
    {
        return powerBuff;
    }

    public string getSpeedBuff()
    {
        return speedBuff;
    }

    public string getSpread()
    {
        return spread;
    }

    public string getPower()
    {
        return power;
    }

    public string getShellSpeed()
    {
        return shellSpeed;
    }

    public string getPenetration()
    {
        return penetration;
    }

    public string getDeterioration()
    {
        return deterioration;
    }
}
