using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objective/Objectives")]

[System.Serializable]
public class Objective : ScriptableObject
{
     public enum ObjectiveType
     {
          Destroy,
          Collect,
          Tower,
          Failproof,
          Impossible
     }

     public enum ObjectiveStatus
     {
          Inactive,
          Active,
          Completed,
          Failed,
          Cancelled
     }

     public ObjectiveType type;
     public ObjectiveStatus status;
     public string description;
     public GameObject target;
     public int reward;
     public int requiredAmount;
     public int currentAmount;

     //To help with keeping objectives completed after retrying at a checkpoint
     protected static ObjectiveStatus resetStatus = ObjectiveStatus.Inactive;
     protected static int resetAmount = -1;

     void Awake()
     {
          status = ObjectiveStatus.Inactive;
          currentAmount = 0;
     }

     //Checks to see if the objective should be complete
     void CheckCompletion()
     {
          if(currentAmount >= requiredAmount && status == ObjectiveStatus.Active)
          {
               status = ObjectiveStatus.Completed;
               ScoreTextScript.coinAmount += reward;
          }
     }

     //Increases the tally towards completing the objective
     public void IncreaseCurrent()
     {
          if (status == ObjectiveStatus.Active)
          {
               currentAmount++;
               CheckCompletion();
          }
     }

     //Returns the tally for the objective
     public int GetCurrentAmount()
     {
          return currentAmount;
     }

     //Updates after a checkpoint and ensures a proper reset at the beginning of a scene
     public void CheckpointUpdate(int phase)
     {
          if (phase != 0)
          {
               resetAmount = currentAmount;
               ResetObjective();
          }
          else
          {
               resetAmount = -1;
               ResetObjective();
          }
     }

     //Resets the objectives to how they were at the last checkpoint
     public void ResetObjective()
     {
          status = resetStatus;

          if (resetAmount == -1)
               currentAmount = 0;
          else
               currentAmount = resetAmount;
     }

     //Resets the objectives at the beginning of a scene
     //Probably a better way to do this
     public void ResetReset()
     {
          resetAmount = -1;
          currentAmount = 0;
     }
}
