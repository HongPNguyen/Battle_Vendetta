using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesSystem : MonoBehaviour
{
     public Text objectiveBox;
     public Objective[] objectives;

     void Awake()
     {
          for (int i = 0; i <objectives.Length; i++)
          {

               objectives[i].ResetObjective();
          }
     }

     // Update is called once per frame
     void Update()
     {
          string boxText = "";
          for (int i = 0; i < objectives.Length; i++)
          {
               if (objectives[i].status == Objective.ObjectiveStatus.Active)
               {
                    if (objectives[i].requiredAmount > 0)
                    {
                         boxText += objectives[i].description + ": " +
                              objectives[i].GetCurrentAmount() + "/" + objectives[i].requiredAmount;
                    }
                    else
                    {
                         boxText += objectives[i].description;
                    }
                    if ((i + 1) < objectives.Length)
                         boxText += "\n";
               }
          }
          objectiveBox.text = boxText;
     }
     
     //Update for destroy type objectives
     public void DestroyUpdate(string destroyedName)
     {
          for (int i = 0; i < objectives.Length; i++)
          {
               if (objectives[i].status == Objective.ObjectiveStatus.Active && 
                 objectives[i].type == Objective.ObjectiveType.Destroy && 
                 string.Equals(objectives[i].target.name, destroyedName))
                    objectives[i].IncreaseCurrent();
          }
     }

     //Update for collect type objectives
     public void CollectUpdate()
     {
          for(int i = 0; i < objectives.Length; i++)
          {
               if (objectives[i].status == Objective.ObjectiveStatus.Active &&
                 objectives[i].type == Objective.ObjectiveType.Collect)
                    objectives[i].IncreaseCurrent();
          }
     }

     //Update for tower type objectives
     public void TowerUpdate()
     {
          for(int i = 0; i < objectives.Length; i++)
          {
               if (objectives[i].status == Objective.ObjectiveStatus.Active && objectives[i].type == Objective.ObjectiveType.Tower)
                    objectives[i].IncreaseCurrent();
          }
     }

     //Update after a checkpoint
     public void CheckpointUpdate(int phase)
     {
          for (int i = 0; i < objectives.Length; i++)
          {
               if (objectives[i].status == Objective.ObjectiveStatus.Active)
                    objectives[i].CheckpointUpdate(phase);
          }
     }
 
     //Activates objectives in the argument
     public void ActivateObjectives(int obj1, int obj2)
     {
          if (objectives[obj1].status == Objective.ObjectiveStatus.Inactive)
               objectives[obj1].status = Objective.ObjectiveStatus.Active;

          if (obj2 != -1)
          {
               if (objectives[obj2].status == Objective.ObjectiveStatus.Inactive)
                    objectives[obj2].status = Objective.ObjectiveStatus.Active;
          }
     }
     
     //Properly updates objectives in the argument based on their type
     //Impossible objectives get cancelled
     //Failproof get completed
     //Active status objectives get marked as failed
     public void CompleteAutomatic(int obj1, int obj2)
     {
          if (objectives[obj1].type == Objective.ObjectiveType.Failproof)
          {
               objectives[obj1].status = Objective.ObjectiveStatus.Completed;
               ScoreTextScript.coinAmount += objectives[obj1].reward;
          }
          else if (objectives[obj1].type == Objective.ObjectiveType.Impossible)
          {
               objectives[obj1].status = Objective.ObjectiveStatus.Cancelled;
          }
          else if (objectives[obj1].status == Objective.ObjectiveStatus.Active)
          {
               objectives[obj1].status = Objective.ObjectiveStatus.Failed;
          }

          if (obj2 != -1)
          {
               if (objectives[obj2].type == Objective.ObjectiveType.Failproof)
               {
                    objectives[obj2].status = Objective.ObjectiveStatus.Completed;
                    ScoreTextScript.coinAmount += objectives[obj2].reward;
               }
               else if (objectives[obj2].type == Objective.ObjectiveType.Impossible)
               {
                    objectives[obj2].status = Objective.ObjectiveStatus.Cancelled;
               }
               else if (objectives[obj1].status == Objective.ObjectiveStatus.Active)
               {
                    objectives[obj2].status = Objective.ObjectiveStatus.Failed;
               }
          }
     }

     //Resets the objectives at the beginning of a scene
     //Probably a better way to do this
     public void ResetReset()
     {
          for (int i = 0; i < objectives.Length; i++)
          {

               objectives[i].ResetReset();
          }
     }
}
