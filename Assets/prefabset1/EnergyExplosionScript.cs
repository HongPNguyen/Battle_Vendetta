using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattExplosionScript : MonoBehaviour
{
    public float speed = 1;
    public float size = 1;
    public ParticleSystem[] ExplosionParts = new ParticleSystem[13];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void editSpeed()
    {
        for (int i = 0; i < ExplosionParts.Length; i++)
        {

        }
    }
    void showTime()
    {
        ExplosionParts[0].Play();
    }

    void curtains()
    {
        ExplosionParts[0].Stop();
    }
}
