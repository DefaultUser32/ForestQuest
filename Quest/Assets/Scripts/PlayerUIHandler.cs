using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // handle the player's UI (health, coins, time, level)
    public Animator fadeOutImage;

    [Header("Health")] 
    [SerializeField] float healthAnimateSpeed; // speed at which the health animates
    [SerializeField] TMP_Text healthText;
    [SerializeField] RectTransform healthBar;

    [Header("Coins")]
    [SerializeField] TMP_Text coinText;

    [Header("Time")]
    [SerializeField] TMP_Text timeText;

    [Header("Level")] 
    [SerializeField] TMP_Text levelText;


    private PlayerController pc;
    private Vector2 healthBarStartingScale;

    private float visualHealth;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        healthBarStartingScale = healthBar.sizeDelta;
        UpdateLevel();
    }
    private void Update()
    {
        UpdateHealthbar();
        UpdateCoins();
        UpdateTime();
    }
    private void UpdateHealthbar()
    {
        // the difference between the amount of health on the healthbar, and the actual amount of health the player has
        float healthDifference = pc.health - visualHealth;

        // if the visual health is close enough to the true health, set it to the true health
        if (Mathf.Abs(healthDifference) < healthAnimateSpeed + 0.05f)
        {
            visualHealth = pc.health;
        }
        else
        {
            // move the visual health by the health animate speed
            if (healthDifference > 0)
            {
                visualHealth += healthAnimateSpeed;
            }
            else
            {
                visualHealth -= healthAnimateSpeed;
            }

        }

        visualHealth = Mathf.Clamp(visualHealth, 0, pc.maxHealth);

        // get the scale of the healthbar depending on the visual health (0-1)
        float newScale = visualHealth / pc.maxHealth;

        // if visualhealth or maxhealth are 0, the value will be either Infinity or NaN respectively
        if (float.IsNaN(newScale)) newScale = 0; 
        if (float.IsInfinity(newScale)) newScale = 1;

        // get the scale of the bar in accordance with its full scale
        newScale *= healthBarStartingScale.x;

        healthBar.sizeDelta = new(newScale, healthBarStartingScale.y);
        healthText.text = Mathf.Round(visualHealth) + "";
    }
    private void UpdateCoins()
    {
        coinText.text = pc.coins + "";
    }
    private void UpdateTime()
    {
        // format the time (in seconds) as mm:ss
        float time = Time.time - pc.gameStateHandler.startTime;

        string min = Mathf.Floor(time / 60) + "";
        string sec = Mathf.Floor(time % 60) + "";
        if (min.Length < 2)
            min = "0" + min;
        if (sec.Length < 2)
            sec = "0" + sec;

        timeText.text = min + ":" + sec;
    }
    private void UpdateLevel()
    {
        // if the current level is within the dictionary, set the text to be the level
        if (pc.gameStateHandler.levelsDict.TryGetValue(pc.gameStateHandler.activeLevel, out Level level))
        {
            levelText.text = level.levelName;
        }
    }
}
