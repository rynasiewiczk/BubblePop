using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable] public class BubbleData
{
    //todo: it should go to struct that describes player level range for bubble level
    [SerializeField] private List<int> _minBubbleSpawnLevelsForPlayerLevel = null;
    [SerializeField] private List<int> _maxBubbleSpawnLevelsForPlayerLevel = null;

    [SerializeField] private int _bubbleLevelAvailableToSpawnAtstart = 3;
    
    public int InitialMaxBubblesLevel = 5;
    [SerializeField] private float _flySpeed = 15f;

    [SerializeField] private float combiningDuration = 13f;

    [Space, SerializeField] private List<Color> _listOfColors = null;

    public float FlySpeed => _flySpeed;
    public float CombiningDuration => combiningDuration;
    public float AfterCombiningDelay = .1f;

    public int GetValueForLevel(int level)
    {
        ValidateProvidedLevel(ref level);
        var result = Mathf.Pow(2, level);
        return (int) result;
    }

    public Color GetColorForLevel(int level)
    {
        ValidateProvidedLevel(ref level);

        if (_listOfColors.Count <= level)
        {
            UpdateListOfColorsUpToRequestedLevel(level);
        }

        return _listOfColors[level];
    }

    private void ValidateProvidedLevel(ref int level)
    {
        if (level <= 0)
        {
            Debug.LogError("Provided level of bubble is less than 1. Level 2 will be returned to avoid breaking the game. Provided level: " + level);
            level = 2;
        }
    }

    private void UpdateListOfColorsUpToRequestedLevel(int level)
    {
        var randomColors = new List<Color>();

        for (int i = _listOfColors.Count; i <= level; i++)
        {
            randomColors.Add(CreateNewRandomColor());
        }

        _listOfColors.AddRange(randomColors);
    }

    private Color CreateNewRandomColor()
    {
        var colorValues = new float[3];
        colorValues[0] = Random.Range(0f, 1f);
        colorValues[1] = Random.Range(0f, 1f);
        colorValues[2] = Random.Range(0f, 1f);

        var colorIndexToBeMax = Random.Range(0, 3);
        colorValues[colorIndexToBeMax] = 1f;

        var randomColor = new Color(colorValues[0], colorValues[1], colorValues[2], 1);
        return randomColor;
    }

    public int GetRandomBubbleLevelBasedOnPlayerLevel(int playerLevel)
    {
        var minLevel = GetMinBubbleLevelByPlayerLevel(playerLevel);
        var maxLevel = GetMaxBubbleLevelByPlayerLevel(playerLevel);

        if (minLevel > maxLevel)
        {
            Debug.LogError("Minimal bubble level formula returned biggest value than formula for maximal bubble level. Returning value that is bigger");
            return minLevel;
        }

        var result = Random.Range(minLevel, maxLevel + 1);
        return result;
    }

    private int GetMinBubbleLevelByPlayerLevel(int playerLevel)
    {
        if (_minBubbleSpawnLevelsForPlayerLevel.Count > playerLevel)
        {
            return _minBubbleSpawnLevelsForPlayerLevel[playerLevel];
        }

        var minLevel = (playerLevel / 5) + 1;
        
        if (minLevel < 1)
        {
            Debug.LogError("Calculated mix bubble level is smaller than minimal possible value.");
        }
        
        var result = Mathf.Max(1, minLevel);
        return result;
    }

    private int GetMaxBubbleLevelByPlayerLevel(int playerLevel)
    {
        if (_maxBubbleSpawnLevelsForPlayerLevel.Count > playerLevel)
        {
            return _maxBubbleSpawnLevelsForPlayerLevel[playerLevel];
        }

        var maxLevel = (playerLevel / 2) - 1;

        if (maxLevel < _bubbleLevelAvailableToSpawnAtstart)
        {
            Debug.LogError("Calculated max bubble level is smaller than one granted by start settings.");
        }
        
        var result = Mathf.Max(_bubbleLevelAvailableToSpawnAtstart, maxLevel);
        return result;
    }
}