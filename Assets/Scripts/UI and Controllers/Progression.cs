using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keeps track of which levels have been beaten
public class Progression : MonoBehaviour
{
     public static int[] progress;

    // Start is called before the first frame update
    void Awake()
    {
          //Update to have more later if need be.
          progress = new int[9];
          progress[0] = PlayerPrefs.GetInt("p0", 0);
          progress[1] = PlayerPrefs.GetInt("p1", 0);
          progress[2] = PlayerPrefs.GetInt("p2", 0);
          progress[3] = PlayerPrefs.GetInt("p3", 0);
          progress[4] = PlayerPrefs.GetInt("p4", 0);
          progress[5] = PlayerPrefs.GetInt("p5", 0);
          progress[6] = PlayerPrefs.GetInt("p6", 0);
          progress[7] = PlayerPrefs.GetInt("p7", 0);
          progress[8] = PlayerPrefs.GetInt("p8", 0);
    }

     public static void ProgressUpdate(int p)
     {
          progress[p] = 1;
          PlayerPrefs.SetInt("p0", progress[0]);
          PlayerPrefs.SetInt("p1", progress[1]);
          PlayerPrefs.SetInt("p2", progress[2]);
          PlayerPrefs.SetInt("p3", progress[3]);
          PlayerPrefs.SetInt("p4", progress[4]);
          PlayerPrefs.SetInt("p5", progress[5]);
          PlayerPrefs.SetInt("p6", progress[6]);
          PlayerPrefs.SetInt("p7", progress[7]);
          PlayerPrefs.SetInt("p8", progress[8]);
     }
}
