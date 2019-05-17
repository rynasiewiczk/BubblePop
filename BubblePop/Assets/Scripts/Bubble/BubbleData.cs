using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable] public class BubbleData
{
    [SerializeField] private List<Color> _listOfColors;

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
            UpdateListOfColorsWithNew(level);
        }

        return _listOfColors[level];
    }

    private void ValidateProvidedLevel(ref int level)
    {
        if (level <= 0)
        {
            Debug.LogError("Provided level of bubble is less than 1. Level 2 will be returned to avoid breaking the game. Provided level: " + level);
        }

        level = 2;
    }

    private void UpdateListOfColorsWithNew(int level)
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

        Debug.Log("RANDOM COLOR: " + randomColor);
        
        return randomColor;
    }
}