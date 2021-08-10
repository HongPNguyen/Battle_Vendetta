/**
 * This script acts as a reference to an enemy and informs the game the current state of the enemy.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleSpawn : MonoBehaviour
{
    public Destructible obj;
    private bool alive = false;

    private void Awake()
    {
        obj = transform.GetChild(0).gameObject.GetComponent<Destructible>();
        obj.gameObject.SetActive(false);
    }

    private void Start()
    {
    }

    public void Spawn()
    {
        //obj = Instantiate(obj, transform) as Destructible;
        obj.gameObject.SetActive(true);
        alive = true;
        obj.SetSpawner(this);
    }

    public void Die()
    {
          if(obj != null) //Helps fix an issue where the call for the obj.Die Coroutine would constantly stop the game with errors as obj was already destroyed
               StartCoroutine(obj.Die());
    }

    // Getters and setters
    public bool IsAlive()
    {
        return alive;
    }

    public void SetAlive(bool alive)
    {
        this.alive = alive;
    }

}
