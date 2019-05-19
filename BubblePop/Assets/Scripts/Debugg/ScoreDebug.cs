using Model.ScoreController;
using UnityEngine;
using Zenject;

public class ScoreDebug : MonoBehaviour
{
    [Inject] private readonly IScoreController _scoreController = null;

    private void OnGUI()
    {
        GUI.TextArea(new Rect(0, 50, 50, 50), "Score: " + _scoreController.Score.Value.ToString());
    }
}