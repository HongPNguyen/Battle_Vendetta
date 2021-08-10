using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Line Set", menuName = "Narration/Line Set")]

[System.Serializable]
public class LineSet : ScriptableObject
{
     public Line[] allLines;
     protected bool isActive = false;
     public int priority;

     public void SetIsActive(bool newIsActive)
     {
          isActive = newIsActive;
     }

     public bool GetIsActive()
     {
          return isActive;
     }
}
