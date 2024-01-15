using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateHandler : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // Handles the state of the game (levels beaten, attempts, time, current level), and the changes of them

    public List<Level> levels = new List<Level>(); // contains level data for all levels
    public Dictionary<string, Level> levelsDict = new Dictionary<string, Level>(); // allows for easier access of levels via dictionary.  Accessable by the level id (scene name)
    public string activeLevel; // current level
    public float startTime; // in game time starting from level load
    private void Awake() // Awake runs before start, ensuring that gameStateHandler will always have loading priority
    {
        // if multiple gameStateHandlers are already in the scene, it deletes itself.
        GameObject[] gameStateHandlers = GameObject.FindGameObjectsWithTag("GameStateHandler");
        if (gameStateHandlers.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        // allow handler to persist across levels
        DontDestroyOnLoad(this.gameObject);

        // adds all levels to the dictionary
        foreach (Level level in levels)
        {
            levelsDict.Add(level.levelId, level);
        }

    }
    public void RestartLevel() // restarts level
    {
        levelsDict[activeLevel].fails++; // Increment fails
        LoadLevel(activeLevel); // Loads level
    }
    public void CompleteLevel() // handles logic for when the level is won
    {
        // updates fastest level time if you beat the level faster, or if the current fastest time is less than 0.05
        if (Time.time - startTime < levelsDict[activeLevel].minTime || levelsDict[activeLevel].minTime < 0.05f)
        {
            levelsDict[activeLevel].minTime = Time.time - startTime;
        }

        // update completion status of level
        levelsDict[activeLevel].isComplete = true;

        // return to main menu
        ExitLevel();
    }
    public void LoadLevel(string level) // handles logic for loading the level
    {
        // reset time for if load happend when game is paused
        Time.timeScale = 1f;


        // update variables
        levelsDict[level].plays++;
        startTime = Time.time;
        activeLevel = level;

        // if given level is not found, send error
        if (!levelsDict.ContainsKey(level))
        {
            Debug.LogError("NOT A LEVEL: " + level);
            return;
        }

        SceneManager.LoadScene(level);
    }
    public void ExitLevel() // return no main menu
    {
        // reset timescale if game was paused
        Time.timeScale = 1f;

        activeLevel = "Title";
        SceneManager.LoadScene("Title");
    }
}
