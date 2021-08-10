using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;//May use this

public class Narration : MonoBehaviour
{
    public Image portrait;
    public Text textBox;

    public LineSet startSet;
    int currentPriority;
    LineSet currentSet;
    int currentLine;
    int priorityLine;
    float timer;
    float priorityTimer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        priorityTimer = 0;
        if(currentSet == null) currentSet = startSet;
        currentPriority = currentSet.priority;
        currentLine = 0;
        priorityLine = 0;
        currentSet.SetIsActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (currentPriority == 0)
        {
            if (currentLine < currentSet.allLines.Length && currentSet.allLines[currentLine].time <= timer)
            {
                //Play line
                portrait.sprite = currentSet.allLines[currentLine].sprite;
                textBox.text = currentSet.allLines[currentLine].textLine;
                currentLine++;
            }
        }
        else
        {
            priorityTimer += Time.deltaTime;
            if (priorityLine < currentSet.allLines.Length && currentSet.allLines[priorityLine].time <= priorityTimer)
            {
                //Play line
                portrait.sprite = currentSet.allLines[priorityLine].sprite;
                textBox.text = currentSet.allLines[priorityLine].textLine;
                priorityLine++;
            }
            /*else if (priorityLine == currentSet.allLines.Length && (priorityTimer - currentSet.allLines[priorityLine - 1].time) > 2)
            {
                 currentPriority = 0;
                 currentSet.SetIsActive(false);
                 currentSet = startSet;
                 currentSet.SetIsActive(true);
            }*/
        }
    }

     //Changes which line set is being read from, with the one with highest priority being used
    public void ChangeLineSet(LineSet newSet)
    {
        if ((newSet != null && currentSet == null) || newSet.priority >= currentSet.priority)
        {
            priorityTimer = 0;
            priorityLine = 0;
            if (currentSet != null)
                currentSet.SetIsActive(false);
            currentSet = newSet;
            currentSet.SetIsActive(true);
            currentPriority = currentSet.priority;
        }
    }
}
