using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // script assosicated with a level selector icon

    [SerializeField] TitleUIHandler titleUIHandler;
    public string levelId; // id of the level 
    public string dependancyId; // id of the level required to be beaten in order to unlock

    // text references
    public TMP_Text title; 
    public TMP_Text plays;
    public TMP_Text fails;
    public TMP_Text minTime;
    public Image lockIcon;

    private GameStateHandler handler;
    private void Start() // find game state handler and use its infomation to update self
    {
        handler = FindObjectOfType<GameStateHandler>();
        UpdateSelf();
    }
    public void UpdateSelf()
    {
        // get information to do with the level assosicated with this icon
        Level selfLevel = handler.levelsDict[levelId];

        // if there is a level that you are required to have beaten, check if its been completed
        if (dependancyId != "")
        {
            lockIcon.enabled = !handler.levelsDict[dependancyId].isComplete;
        } else
        {
            lockIcon.enabled = false;
        }

        // update text variables
        title.text = selfLevel.levelName + " - " + selfLevel.levelDescription;
        plays.text = "Attempts: " + selfLevel.plays;
        fails.text = "Deaths: " + selfLevel.fails;

        // format time from seconds into mm:ss
        string min = Mathf.Floor(selfLevel.minTime / 60) + "";
        string sec = Mathf.Floor(selfLevel.minTime % 60) + "";

        // if there is not 2 digits in mins/secs 
        if (min.Length < 2)
            min = "0" + min;
        if (sec.Length < 2)
            sec = "0" + sec;

        minTime.text = "Best Time: " + min + ":" + sec;
    }
    public void LoadSelf() // load self
    {
        titleUIHandler.LoadLevel(levelId);
    }
}
