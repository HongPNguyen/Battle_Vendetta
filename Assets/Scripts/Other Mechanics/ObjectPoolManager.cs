using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO fix 
public class ObjectPoolManager : MonoBehaviour
{
     public List<ObjectPool> poolList;
     private static Dictionary<string, Queue<GameObject>> objectpoolDictionary;

     // This should hopefully make it so that we don't need to search for the object pools in all of our scripts
     public static ObjectPoolManager SharedInstance;
     void Awake()
     {
          SharedInstance = this;
     }

     void Start()
     {
          objectpoolDictionary = new Dictionary<string, Queue<GameObject>>();
          foreach(ObjectPool pool in poolList)
          {
               Queue<GameObject> poolQueue = new Queue<GameObject>();
               for(int i = 0; i < pool.poolSize; i++)
               {
                    GameObject obj = GameObject.Instantiate(pool.pooledObject);
                    obj.SetActive(false);
                    obj.name = pool.name;
                    poolQueue.Enqueue(obj);
               }

               objectpoolDictionary.Add(pool.name, poolQueue);
          }
     }

     // Retrive the Pooled GameObject that the script accessing the function needs
     public GameObject GetPooledObject(string poolName)
     {
          if (objectpoolDictionary.ContainsKey(poolName))
          {
               if (objectpoolDictionary[poolName].Count > 0)
               {
                    GameObject obj = objectpoolDictionary[poolName].Dequeue();
                    return obj;
               }
               else
               {
                    Debug.Log(poolName + " is empty");
                    return null;
               }
          }
          else
          {
               Debug.Log(poolName + " object pool is not available");
               return null;
          }
     }

     // Returns the object to its pool
     public void ReturnPooledObject(string poolName, GameObject obj)
     {
          if (objectpoolDictionary.ContainsKey(poolName))
          {
               objectpoolDictionary[poolName].Enqueue(obj);
               obj.SetActive(false);
          }
          else
          {
               Debug.Log(poolName + " object pool is not available");
          }
     }
}
