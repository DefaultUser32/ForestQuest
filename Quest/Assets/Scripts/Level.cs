using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // data class for level information

    public string levelId; // scene name of level
    public string levelName; // displayed level name
    public string levelDescription; // description/level title
    public float minTime; // fastest completion of levels (in seconds)
    public int plays; // number of attempts on level
    public int fails; // number of fails on level
    public bool isComplete; // is complete?
}
