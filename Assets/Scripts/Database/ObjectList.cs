using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectList : MonoBehaviour
{
     // Lists for each object type, added to in the shop menu, accessed in the hanger menu
     public static List<Card> planeList;
     public static List<Card> gunList;
     public static List<Card> shellList;

     public List<Card> planes;
     public List<Card> guns;
     public List<Card> shells;

     // Start is called before the first frame update
     void Awake()
     {
          planeList = planes;
          gunList = guns;
          shellList = shells;

          foreach (Card p in planeList)
               p.Awake();
          foreach (Card g in gunList)
               g.Awake();
          foreach (Card s in shellList)
               s.Awake();
     }

     // Update is called once per frame
     void Update()
     {
          planes = planeList;
          guns = gunList;
          shells = shellList;
     }
}
