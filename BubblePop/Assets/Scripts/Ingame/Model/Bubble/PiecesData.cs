using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable] public class PiecesData
{
    //todo: it should go to struct that describes player level range for piece level
    [FormerlySerializedAs("_minpieceSpawnLevelsForPlayerLevel")] [SerializeField]
    private List<int> _minPieceSpawnLevelsForPlayerLevel = null;

    [FormerlySerializedAs("_maxpieceSpawnLevelsForPlayerLevel")] [SerializeField]
    private List<int> _maxPieceSpawnLevelsForPlayerLevel = null;

    [SerializeField] private int _pieceLevelAvailableToSpawnAtstart = 3;

    public int InitialMaxPiecesLevel = 5;
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

    public string GetValueInDisplayFormatFromPieceLevel(int level, int decimalNumbers = 0)
    {
        var value = GetValueForLevel(level);
        var result = GetValueInDisplayFormat(value, decimalNumbers);
        return result;
    }

    public string GetValueInDisplayFormat(int value, int decimalNumbers = 0)
    {
        var suffixes = new string[] {"", "K", "M", "B", "T", "x10¹⁵", "x10¹⁸", "x10²¹", "x10²⁴"};
        var suffixIndex = 0;
        var processedValue = (float) value;
        for (; suffixIndex < suffixes.Length - 1 && processedValue >= 1000; suffixIndex++)
        {
            processedValue /= 1000;
        }

        var result = System.Math.Round(processedValue, decimalNumbers);
        return result.ToString(CultureInfo.CurrentCulture) + suffixes[suffixIndex];
    }

    private void ValidateProvidedLevel(ref int level)
    {
        if (level <= 0)
        {
            Debug.LogError("Provided level of piece is less than 1. Level 2 will be returned to avoid breaking the game. Provided level: " + level);
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


    public int GetRandomPieceLevelBasedOnPlayerLevel(int playerLevel)
    {
        var minLevel = GetMinPieceLevelByPlayerLevel(playerLevel);
        var maxLevel = GetMaxPieceLevelByPlayerLevel(playerLevel);

        if (minLevel > maxLevel)
        {
            Debug.LogError("Minimal piece level formula returned biggest value than formula for maximal piece level. Returning value that is bigger");
            return minLevel;
        }

        var result = Random.Range(minLevel, maxLevel + 1);
        return result;
    }

    private int GetMinPieceLevelByPlayerLevel(int playerLevel)
    {
        if (_minPieceSpawnLevelsForPlayerLevel.Count > playerLevel)
        {
            return _minPieceSpawnLevelsForPlayerLevel[playerLevel];
        }

        var minLevel = (playerLevel / 5) + 1;

        if (minLevel < 1)
        {
            Debug.LogError("Calculated mix piece level is smaller than minimal possible value.");
        }

        var result = Mathf.Max(1, minLevel);
        return result;
    }

    private int GetMaxPieceLevelByPlayerLevel(int playerLevel)
    {
        if (_maxPieceSpawnLevelsForPlayerLevel.Count > playerLevel)
        {
            return _maxPieceSpawnLevelsForPlayerLevel[playerLevel];
        }

        var maxLevel = (playerLevel / 2) - 1;

        if (maxLevel < _pieceLevelAvailableToSpawnAtstart)
        {
            Debug.LogError("Calculated max piece level is smaller than one granted by start settings.");
        }

        var result = Mathf.Max(_pieceLevelAvailableToSpawnAtstart, maxLevel);
        return result;
    }
}