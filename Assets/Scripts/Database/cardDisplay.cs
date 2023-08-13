using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class cardDisplay : MonoBehaviour
{
    bool purchased = false;

    public Card cards;

    public Image artwork;
    public Text nameText;
    public Text costText;
    public Image lockImage;

    //For Stats bars
    public Slider s1;
    public Text st1;
    public Slider s2;
    public Text st2;
    public Slider s3;
    public Text st3;
    public Slider s4;
    public Text st4;
    public Text description;

    // Start is called before the first frame update
    //TODO Finish
    void Awake()
    {
        // Rotate image to the right
        artwork.sprite = cards.artwork;
        /*artwork.preserveAspect = false;
        artwork.transform.Rotate(0,0,-90);
        artwork.preserveAspect = true;
        artwork.transform.localScale = new Vector3(2, 2, 2);*/

        // Set values
        nameText.text = cards.name;
        if (costText != null)
            costText.text = cards.cost.ToString();

        if (cards.unlockLevel == -1 || Progression.lastLevelCompleted >= 1)
            lockImage.gameObject.SetActive(false);
        else
            lockImage.gameObject.SetActive(true);
    }

    // ENABLE IN RELEASE BUILD
    /*void Update()
    {
        if (cards.unlockLevel == -1 || Progression.progress[cards.unlockLevel])
            lockImage.gameObject.SetActive(false);
        else
            lockImage.gameObject.SetActive(true);
    }*/

    public void BuyItem()
    {
        if (lockImage.gameObject.activeSelf)
        {
            Debug.Log("Item is locked.");
            return;
        }
        if (ScoreTextScript.coinAmount >= cards.cost)
        {
            // Add gameObject to the appropriate list of available objects
            foreach (Card card in ObjectList.planeList)
            {
                if (cards == card)
                {
                    Debug.Log("This item was already purchased.");
                    purchased = true;
                    break;
                }
            }

            if (!purchased)
            {
                switch ((int)cards.cardType)
                {
                    case 1:
                        //Maybe add sound effect here to indicate a purchase has been made
                        ObjectList.planeList.Add(cards);
                        HangarMenu.AddButton(cards);
                        break;
                    case 2:
                        ObjectList.gunList.Add(cards);
                        HangarMenu.AddButton(cards);
                        break;
                    case 3:
                        ObjectList.shellList.Add(cards);
                        HangarMenu.AddButton(cards);
                        break;
                    default:
                        Debug.Log("Please give this card the right card type.");
                        break;
                }

                ScoreTextScript.coinAmount -= cards.cost;
            }
        }
        else
        {
            Debug.Log("Not enough coins.");
        }
    }

    // Updates the stats bars at the top of the Hangar/Shop
    public void UpdateStats()
    {
        //There is propably a better way to do this, but I don't know right now
        switch ((int)cards.cardType)
        {
            case 1:
                s1.value = float.Parse(cards.getSpeed()) * 0.1f;
                st1.text = "SPEED: " + cards.getSpeed();
                s2.value = float.Parse(cards.getHealth()) * 0.1f;
                st2.text = "HEALTH: " + cards.getHealth();
                s3.value = float.Parse(cards.getDefense()) * 0.1f;
                st3.text = "DEFENSE: " + cards.getDefense();
                s4.value = float.Parse(cards.getMass()) * 0.1f;
                st4.text = "MASS: " + cards.getMass();
                break;
            case 2:
                // Primary weapons have fairly large fire rate (max 2000 RPM)
                if (cards.getCategory() == WeaponsClassification.Category.Primary)
                {
                    s1.value = 0.03f / float.Parse(cards.getReloadTime());
                    st1.text = "FIRE RATE: " + 60f / float.Parse(cards.getReloadTime()) + " RPM";
                }
                // Compared to secondary weapons (max 1 charge / 3s)
                else
                {
                    s1.value = 3f / float.Parse(cards.getReloadTime());
                    st1.text = "COOLDOWN: " + cards.getReloadTime() + " s";
                }
                s2.value = float.Parse(cards.getPowerBuff()) * 0.5f;
                st2.text = "POWER BUFF: " + float.Parse(cards.getPowerBuff()) * 100f + "%";
                s3.value = float.Parse(cards.getSpeedBuff()) * 0.5f;
                st3.text = "SPEED BUFF: " + float.Parse(cards.getSpeedBuff()) * 100f + "%";
                s4.value = 0.5f / float.Parse(cards.getSpread());
                st4.text = "SPREAD: " + cards.getSpread() + " Degrees";
                break;
            case 3:
                s1.value = float.Parse(cards.getPower()) * 0.05f;
                st1.text = "POWER: " + cards.getPower();
                s2.value = (float.Parse(cards.getShellSpeed()) - 5) / 30f;
                st2.text = "SPEED: " + cards.getShellSpeed();
                s3.value = float.Parse(cards.getPenetration()) * 0.1f;
                st3.text = "PENETRATION: " + cards.getPenetration();
                s4.value = 0.03f / float.Parse(cards.getDeterioration());
                st4.text = "DETERIORATION: " + float.Parse(cards.getDeterioration()) * 100f + "% / s";
                break;
            default:
                Debug.Log("Please give this card the right card tyoe.");
                break;
        }
        // Display description
        description.text = "Description: " + cards.description;
    }
}
