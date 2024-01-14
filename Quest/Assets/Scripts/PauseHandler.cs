using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // handles the game pause menu

    [SerializeField] GameObject pauseParent;
    bool isPaused;

    // internal
    private PlayerController pc;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
    }
    private void Update() // pause when escape is pressed
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }
    public void Toggle() // flip pause state
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f; // timeScale changes the rate at which time passes in game.  0 means paused, 1 means normal
        pauseParent.SetActive(isPaused); // update visibility of pause menu
    }
    public void ResetLevel() // used to call restart level from a UI button
    {
        pc.gameStateHandler.RestartLevel();
    }
    public void ExitLevel() // used to call exit level from a UI button
    {
        pc.gameStateHandler.ExitLevel();
    }
}
