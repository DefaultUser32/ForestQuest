using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIHandler : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // allows scene components to access the gameStateHandler without having to search for it themselves


    [HideInInspector] public GameStateHandler GameStateHandler;

    private void Start()
    {
        GameStateHandler = FindObjectOfType<GameStateHandler>();
    }
    public void LoadLevel(string level)
    {
        GameStateHandler.LoadLevel(level);
    }
}
