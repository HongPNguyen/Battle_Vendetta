using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// require 3D plane, Mesh Filter, and Mesh Renderer.

// since this is the case.  The required image files need to have a copy converted into "material" text files.


public class self_loop_Background : MonoBehaviour
{
    
    public float scrollSpeed = 10f;
    private float offset;
    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(0, offset));   // scrolls vertically
    }
}
