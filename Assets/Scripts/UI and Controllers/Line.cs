using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Line", menuName = "Narration/Line")]

[System.Serializable]
public class Line : ScriptableObject
{
    [TextArea]
    public string textLine;
    public Sprite sprite;
    public float time;
}
