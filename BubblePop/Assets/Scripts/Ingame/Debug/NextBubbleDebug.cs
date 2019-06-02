using Project.Pieces;
using UnityEngine;
using Zenject;

public class NextBubbleDebug : MonoBehaviour
{
    [Inject] private readonly INextBubbleLevelToSpawnController _nextBubbleController = null;
    [Inject] private readonly PiecesData _piecesData = null;

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        GUI.TextArea(new Rect(0, 0, 50, 50), "Next: " + _piecesData.GetValueForLevel(_nextBubbleController.NextBubbleLevelToSpawn.Value).ToString());
    }
}