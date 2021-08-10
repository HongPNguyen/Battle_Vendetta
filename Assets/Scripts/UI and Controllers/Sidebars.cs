using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sidebars : MonoBehaviour
{
    public Text currentPlaneName;
    public Image currentPlaneIcon;
    public Image currentPlaneSecondary;
    public Text secondaryAmmo0;
    public Image currentPlaneSecondary2;
    public Image currentPlaneAmmo;
    public Image currentPlaneAmmo2;
    public Text activeSecondary;

    public Text sidePlaneName1;
    public Image sidePlaneIcon1;
    public HealthBar healthBar1;
    public HealthBar defenseBar1;
    public HealthBar cooldownBar1;
    public Image sidePlaneSecondary1;
    public Text secondaryAmmo1;
    public Image sidePlaneSecondary1_2;

    public Text sidePlaneName2;
    public Image sidePlaneIcon2;
    public HealthBar healthBar2;
    public HealthBar defenseBar2;
    public HealthBar cooldownBar2;
    public Image sidePlaneSecondary2;
    public Text secondaryAmmo2;
    public Image sidePlaneSecondary2_2;

    public Sprite laserSprite;
    public Image[] highlights;
    int current = 0;
    int activeWeapon = -1;
    public Text activeShell;

    public Text phaseText;

    // Start is called before the first frame update
    void Awake()
    {
        //Find the initial plane
        currentPlaneName.text = PlaneSwitching.squadArr[0].name;
        currentPlaneIcon.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
        current = 0;
        EquipmentUpdate(0, 0);
        if (PlaneSwitching.squadArr[1] != null && PlaneSwitching.squadArr[2] != null)
        {
            sidePlaneName1.text = PlaneSwitching.squadArr[1].name;
            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
            healthBar1.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
            healthBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
            defenseBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
            EquipmentUpdate(1, 1);
            sidePlaneName2.text = PlaneSwitching.squadArr[2].name;
            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[2].GetComponent<SpriteRenderer>().sprite;
            healthBar2.SetMax(PlaneSwitching.squadArr[2].GetComponent<Player>().GetMaxHealth());
            healthBar2.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().health);
            defenseBar2.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().defense);
            EquipmentUpdate(2, 2);
        }
        else if (PlaneSwitching.squadArr[1] != null)
        {
            sidePlaneName1.text = PlaneSwitching.squadArr[1].name;
            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
            healthBar1.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
            healthBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
            defenseBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
            EquipmentUpdate(1, 1);
            sidePlaneName2.text = PlaneSwitching.squadArr[1].name;
            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
            healthBar2.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
            healthBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
            defenseBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
            EquipmentUpdate(2, 1);
        }
        else
        {
            sidePlaneName1.gameObject.SetActive(false);
            sidePlaneIcon1.gameObject.SetActive(false);
            healthBar1.gameObject.SetActive(false);
            defenseBar1.gameObject.SetActive(false);
            sidePlaneName2.gameObject.SetActive(false);
            sidePlaneIcon2.gameObject.SetActive(false);
            healthBar2.gameObject.SetActive(false);
            defenseBar2.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if (activeWeapon != PlaneSwitching.squadArr[current].transform.GetComponent<Player>().activeSecondaryWeapon)
        {
            Debug.Log("Swap Weapons");
            for (int i = 0; i < 4; i++)
            {
                if (highlights[i].gameObject.activeSelf)
                    highlights[i].gameObject.SetActive(false);
                else
                    highlights[i].gameObject.SetActive(true);
            }
            activeWeapon = PlaneSwitching.squadArr[current].transform.GetComponent<Player>().activeSecondaryWeapon;
            ShellTextUpdate();
        }
    }

    //Updates the display for the planes in the sidebars
    //Needs to be simplified
    public void UpdatePlanes(int currentPlane, int initialSize, int[] squadStatus)
    {
        switch (initialSize)
        {
            case 1:
                break;
            case 2:
                if (currentPlane == 0 && squadStatus[1] == 1)
                {
                    currentPlaneName.text = PlaneSwitching.squadArr[0].name;
                    currentPlaneIcon.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                    current = 0;
                    EquipmentUpdate(0, 0);
                    sidePlaneName1.text = PlaneSwitching.squadArr[1].name;
                    sidePlaneIcon1.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                    healthBar1.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
                    healthBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
                    defenseBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
                    EquipmentUpdate(1, 1);
                    sidePlaneName2.text = PlaneSwitching.squadArr[1].name;
                    sidePlaneIcon2.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                    healthBar2.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
                    healthBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
                    defenseBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
                    EquipmentUpdate(2, 1);
                }
                else if (currentPlane == 1 && squadStatus[0] == 1)
                {
                    currentPlaneName.text = PlaneSwitching.squadArr[1].name;
                    currentPlaneIcon.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                    current = 1;
                    EquipmentUpdate(0, 1);
                    sidePlaneName1.text = PlaneSwitching.squadArr[0].name;
                    sidePlaneIcon1.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                    healthBar1.SetMax(PlaneSwitching.squadArr[0].GetComponent<Player>().GetMaxHealth());
                    healthBar1.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().health);
                    defenseBar1.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().defense);
                    EquipmentUpdate(1, 0);
                    sidePlaneName2.text = PlaneSwitching.squadArr[0].name;
                    sidePlaneIcon2.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                    healthBar2.SetMax(PlaneSwitching.squadArr[0].GetComponent<Player>().GetMaxHealth());
                    healthBar2.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().health);
                    defenseBar2.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().defense);
                    EquipmentUpdate(2, 0);
                }
                else if (currentPlane == 0 && squadStatus[1] == 0)
                {
                    currentPlaneName.text = PlaneSwitching.squadArr[0].name;
                    currentPlaneIcon.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                    current = 0;
                    EquipmentUpdate(0, 0);
                    sidePlaneName1.gameObject.SetActive(false);
                    sidePlaneIcon1.gameObject.SetActive(false);
                    healthBar1.gameObject.SetActive(false);
                    defenseBar1.gameObject.SetActive(false);
                    sidePlaneName2.gameObject.SetActive(false);
                    sidePlaneIcon2.gameObject.SetActive(false);
                    healthBar2.gameObject.SetActive(false);
                    defenseBar2.gameObject.SetActive(false);
                }
                else
                {
                    currentPlaneName.text = PlaneSwitching.squadArr[1].name;
                    currentPlaneIcon.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                    current = 1;
                    EquipmentUpdate(0, 1);
                    sidePlaneName1.gameObject.SetActive(false);
                    sidePlaneIcon1.gameObject.SetActive(false);
                    healthBar1.gameObject.SetActive(false);
                    defenseBar1.gameObject.SetActive(false);
                    sidePlaneName2.gameObject.SetActive(false);
                    sidePlaneIcon2.gameObject.SetActive(false);
                    healthBar2.gameObject.SetActive(false);
                    defenseBar2.gameObject.SetActive(false);
                }
                break;
            case 3:
                switch (currentPlane)
                {
                    case 0:
                        currentPlaneName.text = PlaneSwitching.squadArr[0].name;
                        currentPlaneIcon.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                        current = 0;
                        EquipmentUpdate(0, 0);
                        if (squadStatus[1] == 1 && squadStatus[2] == 1)
                        {
                            sidePlaneName1.text = PlaneSwitching.squadArr[1].name;
                            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                            healthBar1.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
                            healthBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
                            defenseBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
                            EquipmentUpdate(1, 1);
                            sidePlaneName2.text = PlaneSwitching.squadArr[2].name;
                            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[2].GetComponent<SpriteRenderer>().sprite;
                            healthBar2.SetMax(PlaneSwitching.squadArr[2].GetComponent<Player>().GetMaxHealth());
                            healthBar2.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().health);
                            defenseBar2.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().defense);
                            EquipmentUpdate(2, 2);
                        }
                        else if (squadStatus[1] == 1)
                        {
                            sidePlaneName1.text = PlaneSwitching.squadArr[1].name;
                            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                            healthBar1.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
                            healthBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
                            defenseBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
                            EquipmentUpdate(1, 1);
                            sidePlaneName2.text = PlaneSwitching.squadArr[1].name;
                            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                            healthBar2.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
                            healthBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
                            defenseBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
                            EquipmentUpdate(2, 1);
                        }
                        else if (squadStatus[2] == 1)
                        {
                            sidePlaneName1.text = PlaneSwitching.squadArr[2].name;
                            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[2].GetComponent<SpriteRenderer>().sprite;
                            healthBar1.SetMax(PlaneSwitching.squadArr[2].GetComponent<Player>().GetMaxHealth());
                            healthBar1.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().health);
                            defenseBar1.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().defense);
                            EquipmentUpdate(1, 2);
                            sidePlaneName2.text = PlaneSwitching.squadArr[2].name;
                            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[2].GetComponent<SpriteRenderer>().sprite;
                            healthBar2.SetMax(PlaneSwitching.squadArr[2].GetComponent<Player>().GetMaxHealth());
                            healthBar2.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().health);
                            defenseBar2.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().defense);
                            EquipmentUpdate(2, 2);
                        }
                        else
                        {
                            sidePlaneName1.gameObject.SetActive(false);
                            sidePlaneIcon1.gameObject.SetActive(false);
                            healthBar1.gameObject.SetActive(false);
                            defenseBar1.gameObject.SetActive(false);
                            sidePlaneName2.gameObject.SetActive(false);
                            sidePlaneIcon2.gameObject.SetActive(false);
                            healthBar2.gameObject.SetActive(false);
                            defenseBar2.gameObject.SetActive(false);
                        }
                        break;
                    case 1:
                        currentPlaneName.text = PlaneSwitching.squadArr[1].name;
                        currentPlaneIcon.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                        current = 1;
                        EquipmentUpdate(0, 1);
                        if (squadStatus[2] == 1 && squadStatus[0] == 1)
                        {
                            sidePlaneName1.text = PlaneSwitching.squadArr[2].name;
                            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[2].GetComponent<SpriteRenderer>().sprite;
                            healthBar1.SetMax(PlaneSwitching.squadArr[2].GetComponent<Player>().GetMaxHealth());
                            healthBar1.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().health);
                            defenseBar1.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().defense);
                            EquipmentUpdate(1, 2);
                            sidePlaneName2.text = PlaneSwitching.squadArr[0].name;
                            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                            healthBar2.SetMax(PlaneSwitching.squadArr[0].GetComponent<Player>().GetMaxHealth());
                            healthBar2.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().health);
                            defenseBar2.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().defense);
                            EquipmentUpdate(2, 0);
                        }
                        else if (squadStatus[0] == 1)
                        {
                            sidePlaneName1.text = PlaneSwitching.squadArr[0].name;
                            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                            healthBar1.SetMax(PlaneSwitching.squadArr[0].GetComponent<Player>().GetMaxHealth());
                            healthBar1.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().health);
                            defenseBar1.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().defense);
                            EquipmentUpdate(1, 0);
                            sidePlaneName2.text = PlaneSwitching.squadArr[0].name;
                            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                            healthBar2.SetMax(PlaneSwitching.squadArr[0].GetComponent<Player>().GetMaxHealth());
                            healthBar2.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().health);
                            defenseBar2.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().defense);
                            EquipmentUpdate(2, 0);
                        }
                        else if (squadStatus[2] == 1)
                        {
                            sidePlaneName1.text = PlaneSwitching.squadArr[2].name;
                            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[2].GetComponent<SpriteRenderer>().sprite;
                            healthBar1.SetMax(PlaneSwitching.squadArr[2].GetComponent<Player>().GetMaxHealth());
                            healthBar1.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().health);
                            defenseBar1.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().defense);
                            EquipmentUpdate(1, 2);
                            sidePlaneName2.text = PlaneSwitching.squadArr[2].name;
                            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[2].GetComponent<SpriteRenderer>().sprite;
                            healthBar2.SetMax(PlaneSwitching.squadArr[2].GetComponent<Player>().GetMaxHealth());
                            healthBar2.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().health);
                            defenseBar2.SetHealth(PlaneSwitching.squadArr[2].GetComponent<Player>().defense);
                            EquipmentUpdate(2, 2);
                        }
                        else
                        {
                            sidePlaneName1.gameObject.SetActive(false);
                            sidePlaneIcon1.gameObject.SetActive(false);
                            healthBar1.gameObject.SetActive(false);
                            defenseBar1.gameObject.SetActive(false);
                            sidePlaneName2.gameObject.SetActive(false);
                            sidePlaneIcon2.gameObject.SetActive(false);
                            healthBar2.gameObject.SetActive(false);
                            defenseBar2.gameObject.SetActive(false);
                        }
                        break;
                    case 2:
                        currentPlaneName.text = PlaneSwitching.squadArr[2].name;
                        currentPlaneIcon.sprite = PlaneSwitching.squadArr[2].GetComponent<SpriteRenderer>().sprite;
                        current = 2;
                        EquipmentUpdate(0, 2);
                        if (squadStatus[1] == 1 && squadStatus[0] == 1)
                        {
                            sidePlaneName1.text = PlaneSwitching.squadArr[0].name;
                            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                            healthBar1.SetMax(PlaneSwitching.squadArr[0].GetComponent<Player>().GetMaxHealth());
                            healthBar1.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().health);
                            defenseBar1.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().defense);
                            EquipmentUpdate(1, 0);
                            sidePlaneName2.text = PlaneSwitching.squadArr[1].name;
                            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                            healthBar2.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
                            healthBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
                            defenseBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
                            EquipmentUpdate(2, 1);
                        }
                        else if (squadStatus[0] == 1)
                        {
                            sidePlaneName1.text = PlaneSwitching.squadArr[0].name;
                            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                            healthBar1.SetMax(PlaneSwitching.squadArr[0].GetComponent<Player>().GetMaxHealth());
                            healthBar1.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().health);
                            defenseBar1.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().defense);
                            EquipmentUpdate(1, 0);
                            sidePlaneName2.text = PlaneSwitching.squadArr[0].name;
                            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[0].GetComponent<SpriteRenderer>().sprite;
                            healthBar2.SetMax(PlaneSwitching.squadArr[0].GetComponent<Player>().GetMaxHealth());
                            healthBar2.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().health);
                            defenseBar2.SetHealth(PlaneSwitching.squadArr[0].GetComponent<Player>().defense);
                            EquipmentUpdate(2, 0);
                        }
                        else if (squadStatus[1] == 1)
                        {
                            sidePlaneName1.text = PlaneSwitching.squadArr[1].name;
                            sidePlaneIcon1.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                            healthBar1.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
                            healthBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
                            defenseBar1.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
                            EquipmentUpdate(1, 1);
                            sidePlaneName2.text = PlaneSwitching.squadArr[1].name;
                            sidePlaneIcon2.sprite = PlaneSwitching.squadArr[1].GetComponent<SpriteRenderer>().sprite;
                            healthBar2.SetMax(PlaneSwitching.squadArr[1].GetComponent<Player>().GetMaxHealth());
                            healthBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().health);
                            defenseBar2.SetHealth(PlaneSwitching.squadArr[1].GetComponent<Player>().defense);
                            EquipmentUpdate(2, 1);
                        }
                        else
                        {
                            sidePlaneName1.gameObject.SetActive(false);
                            sidePlaneIcon1.gameObject.SetActive(false);
                            healthBar1.gameObject.SetActive(false);
                            defenseBar1.gameObject.SetActive(false);
                            sidePlaneName2.gameObject.SetActive(false);
                            sidePlaneIcon2.gameObject.SetActive(false);
                            healthBar2.gameObject.SetActive(false);
                            defenseBar2.gameObject.SetActive(false);
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        ShellTextUpdate();
    }

    //Updates the equipment icons in the sidebar
    //Needs to be simplified somehow
    void EquipmentUpdate(int slot, int plane)
    {
        int weaponSlots = 0;
        if (PlaneSwitching.squadArr[plane].name == "Airos")
            weaponSlots = PlaneSwitching.squadArr[plane].transform.childCount - 2;
        else
            weaponSlots = PlaneSwitching.squadArr[plane].transform.childCount - 1;

        switch (slot)
        {
            case 0:
                currentPlaneSecondary.gameObject.SetActive(false);
                currentPlaneSecondary.sprite = null;
                currentPlaneAmmo.gameObject.SetActive(false);
                currentPlaneAmmo2.gameObject.SetActive(false);
                secondaryAmmo0.text = "0";
                currentPlaneSecondary2.gameObject.SetActive(false);
                highlights[0].gameObject.SetActive(false);
                highlights[1].gameObject.SetActive(false);
                highlights[2].gameObject.SetActive(false);
                highlights[3].gameObject.SetActive(false);

                for (int i = 0; i < weaponSlots; i++)
                {
                    if ((int)PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().category == 2 ||
                         (int)PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().category == 3)
                    {
                        if (currentPlaneSecondary.sprite == null)
                        {
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent("PlayerLaser") != null)
                            {
                                currentPlaneSecondary.gameObject.SetActive(true);
                                currentPlaneSecondary.sprite = laserSprite;
                                currentPlaneAmmo.gameObject.SetActive(true);
                                currentPlaneAmmo.sprite = laserSprite;
                            }
                            else
                            {
                                currentPlaneSecondary.gameObject.SetActive(true);
                                currentPlaneSecondary.sprite = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                                currentPlaneAmmo.gameObject.SetActive(true);
                                currentPlaneAmmo.sprite = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().shellTypes[0].GetComponent<SpriteRenderer>().sprite;
                            }
                            secondaryAmmo0.text = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().ammo.ToString();
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SecondaryWeapon>().group == PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon)
                            {
                                highlights[0].gameObject.SetActive(true);
                                highlights[2].gameObject.SetActive(true);
                                activeWeapon = PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon;
                            }
                            else
                            {
                                highlights[0].gameObject.SetActive(false);
                                highlights[2].gameObject.SetActive(false);
                                activeWeapon = PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon;
                            }
                        }
                        else if (currentPlaneSecondary.sprite != laserSprite || currentPlaneSecondary.sprite != PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite)
                        {
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent("PlayerLaser") != null)
                            {
                                currentPlaneSecondary2.gameObject.SetActive(true);
                                currentPlaneSecondary2.sprite = laserSprite;
                                currentPlaneAmmo2.gameObject.SetActive(true);
                                currentPlaneAmmo2.sprite = laserSprite;
                            }
                            else
                            {
                                currentPlaneSecondary2.gameObject.SetActive(true);
                                currentPlaneSecondary2.sprite = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                                currentPlaneAmmo2.gameObject.SetActive(true);
                                currentPlaneAmmo2.sprite = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().shellTypes[0].GetComponent<SpriteRenderer>().sprite;
                            }
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SecondaryWeapon>().group == PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon)
                            {
                                highlights[1].gameObject.SetActive(true);
                                highlights[3].gameObject.SetActive(true);
                                activeWeapon = PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon;
                            }
                            else
                            {
                                highlights[1].gameObject.SetActive(false);
                                highlights[3].gameObject.SetActive(false);
                                activeWeapon = PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon;
                            }
                        }
                    }
                }
                break;
            case 1:
                sidePlaneSecondary1.gameObject.SetActive(false);
                sidePlaneSecondary1.sprite = null;
                secondaryAmmo1.text = "0";
                sidePlaneSecondary1_2.gameObject.SetActive(false);
                cooldownBar1.SetHealth(0);
                highlights[4].gameObject.SetActive(false);
                highlights[5].gameObject.SetActive(false);

                for (int i = 0; i < weaponSlots; i++)
                {
                    if ((int)PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().category == 2 ||
                         (int)PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().category == 3)
                    {
                        if (sidePlaneSecondary1.sprite == null)
                        {
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent("PlayerLaser") != null)
                            {
                                sidePlaneSecondary1.gameObject.SetActive(true);
                                sidePlaneSecondary1.sprite = laserSprite;
                            }
                            else
                            {
                                sidePlaneSecondary1.gameObject.SetActive(true);
                                sidePlaneSecondary1.sprite = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                            }
                            secondaryAmmo1.text = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().ammo.ToString();
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SecondaryWeapon>().group == PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon)
                                highlights[4].gameObject.SetActive(true);
                            else
                                highlights[4].gameObject.SetActive(false);

                        }
                        else if (sidePlaneSecondary1.sprite != laserSprite || sidePlaneSecondary1.sprite != PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite)
                        {
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent("PlayerLaser") != null)
                            {
                                sidePlaneSecondary1_2.gameObject.SetActive(true);
                                sidePlaneSecondary1_2.sprite = laserSprite;
                            }
                            else
                            {
                                sidePlaneSecondary1_2.gameObject.SetActive(true);
                                sidePlaneSecondary1_2.sprite = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                            }
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SecondaryWeapon>().group == PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon)
                                highlights[5].gameObject.SetActive(true);
                            else
                                highlights[5].gameObject.SetActive(false);
                        }
                    }
                }
                break;
            case 2:
                sidePlaneSecondary2.gameObject.SetActive(false);
                sidePlaneSecondary2.sprite = null;
                secondaryAmmo2.text = "0";
                sidePlaneSecondary2_2.gameObject.SetActive(false);
                cooldownBar2.SetHealth(0);
                highlights[6].gameObject.SetActive(false);
                highlights[7].gameObject.SetActive(false);

                for (int i = 0; i < weaponSlots; i++)
                {
                    if ((int)PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().category == 2 ||
                         (int)PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().category == 3)
                    {
                        if (sidePlaneSecondary2.sprite == null)
                        {
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent("PlayerLaser") != null)
                            {
                                sidePlaneSecondary2.gameObject.SetActive(true);
                                sidePlaneSecondary2.sprite = laserSprite;
                            }
                            else
                            {
                                sidePlaneSecondary2.gameObject.SetActive(true);
                                sidePlaneSecondary2.sprite = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                            }
                            secondaryAmmo2.text = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<Gun>().ammo.ToString();
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SecondaryWeapon>().group == PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon)
                                highlights[6].gameObject.SetActive(true);
                            else
                                highlights[6].gameObject.SetActive(false);
                        }
                        else if (sidePlaneSecondary2.sprite != laserSprite || sidePlaneSecondary2.sprite != PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite)
                        {
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent("PlayerLaser") != null)
                            {
                                sidePlaneSecondary2_2.gameObject.SetActive(true);
                                sidePlaneSecondary2_2.sprite = laserSprite;
                            }
                            else
                            {
                                sidePlaneSecondary2_2.gameObject.SetActive(true);
                                sidePlaneSecondary2_2.sprite = PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                            }
                            if (PlaneSwitching.squadArr[plane].transform.GetChild(i).GetChild(0).GetComponent<SecondaryWeapon>().group == PlaneSwitching.squadArr[plane].transform.GetComponent<Player>().activeSecondaryWeapon)
                                highlights[7].gameObject.SetActive(true);
                            else
                                highlights[7].gameObject.SetActive(false);
                        }
                    }
                }
                break;
            default:
                Debug.Log("This message should not appear. Issue with EquipmentUpdate in the Sidebars script.");
                break;
        }
    }

    // Change phase text
    public void SetPhaseText(string text)
    {
        phaseText.text = text;
    }

    //Update text that shows which shell type is currently active
    //Made this into its own function to fix issue where text would not update after planes were swapped
    public void ShellTextUpdate()
    {
        if (PlaneSwitching.squadArr[current].transform.GetComponent<Player>().numberOfSecondaryWeapons > 0)
        {
            for (int i = 0; i < PlaneSwitching.squadArr[current].transform.childCount - 1; i++)
            {
                Debug.Log("Checking for shells");
                if (PlaneSwitching.squadArr[current].transform.GetChild(i).GetChild(0).GetComponent("PlayerAsynchronousLauncher")
                     && PlaneSwitching.squadArr[current].transform.GetChild(i).GetChild(0).GetComponent<PlayerAsynchronousLauncher>().group == activeWeapon)
                {
                    int currentShell = PlaneSwitching.squadArr[current].transform.GetComponent<Player>().activeShellGroup;
                    activeShell.text = PlaneSwitching.squadArr[current].transform.GetChild(i).GetChild(0).GetComponent<SecondaryWeapon>().shellTypes[currentShell].name;
                    break;
                }
                else if (PlaneSwitching.squadArr[current].transform.GetChild(i).GetChild(0).GetComponent("PlayerLaser")
                     && PlaneSwitching.squadArr[current].transform.GetChild(i).GetChild(0).GetComponent<PlayerLaser>().group == activeWeapon)
                {
                    activeShell.text = "Laser";
                    break;
                }
            }
        }
        else
        {
            activeShell.text = "";
        }
    }

    public void Reset()
    {
        this.Awake();
    }
}
