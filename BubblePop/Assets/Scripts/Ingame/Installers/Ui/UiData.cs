using System;
using UnityEngine;

[Serializable]
public class UiData
{
    public int GridScoreText_MinFontSize = 40;
    public int GridScoreText_MaxFontSize = 150;
    public int GridScoreText_MaxScoreForTopSize = 4096;
    public float GridScoreText_FadeOutSpeed = 25f;
    public float GridScoreText_ScaleupSpeed = 10f;
    public float GridScoreText_LifeTime = .35f;
    public float MoveUpSpeed = 1f;

    public float GetFontSizeForScore(int score)
    {
        var normalizedScore = (float)score / GridScoreText_MaxScoreForTopSize;
        normalizedScore = Mathf.Clamp01(normalizedScore);

        var lerp = Mathf.Lerp(GridScoreText_MinFontSize, GridScoreText_MaxFontSize, normalizedScore);
        return lerp;
    }
}
