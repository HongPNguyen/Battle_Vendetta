using System;
using UnityEngine;

//Keeps track of which levels have been beaten
public class Progression : MonoBehaviour
{
    public const int NUM_LEVELS = 8;
    public static int lastLevelCompleted = 0;
    public static bool[] objectivesCompleted;

    // Start is called before the first frame update
    private void Awake()
    {
        objectivesCompleted = new bool[8];
        lastLevelCompleted = PlayerPrefs.GetInt("LastLevelCompleted", 0);
    }

    /// <summary>
    /// Processes completion of a level.
    /// </summary>
    /// <param name="levelCompleted">The level that was just completed.</param>
    public static void ReportLevelCompleted(int levelCompleted)
    {
        lastLevelCompleted = levelCompleted;
        PlayerPrefs.SetInt("LastLevelCompleted", lastLevelCompleted);
    }

    /// <summary>
    /// Processes completion of a level's objectives.
    /// </summary>
    /// <param name="levelCompleted">The level that just had its special objectives completed.</param>
    public static void ReportLevelObjectivesCompleted(int level)
    {
        if (level <= 0 || level > NUM_LEVELS)
        {
            throw new Exception("Invalid level has its objectives reported completed.");
        }

        objectivesCompleted[level - 1] = true;

        for (int i = 1; i <= NUM_LEVELS; i++)
        {
            PlayerPrefs.SetInt("Objective " + i + "Completed", objectivesCompleted[i - 1] ? 1 : 0);
        }
    }
}
