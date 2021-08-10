using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ObjectPool will determine what each pool contains and how large they are
[System.Serializable]
public class ObjectPool : MonoBehaviour
{
     public string name;
     public GameObject pooledObject;
     public int poolSize;
}
