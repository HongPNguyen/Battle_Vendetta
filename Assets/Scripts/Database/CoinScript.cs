using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{

     ObjectivesSystem objSys;

     void Awake()
     {
          objSys = GameObject.Find("HUD").GetComponent<ObjectivesSystem>();
     }

     //Allow the player to collect the coins
    void OnTriggerEnter2D(Collider2D collision) {
          if(collision.tag == "ActivePlayer")
          {
               ScoreTextScript.coinAmount += 10;
               objSys.CollectUpdate();
               Destroy(gameObject);
          }
    }
}


