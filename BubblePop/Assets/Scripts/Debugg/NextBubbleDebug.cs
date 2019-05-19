using Project.Bubbles;
using UnityEngine;
using Zenject;

public class NextBubbleDebug : MonoBehaviour
{
    [Inject] private readonly INextBubbleLevelToSpawnController _nextBubbleController = null;
    [Inject] private readonly BubbleData _bubbleData = null;

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        GUI.TextArea(new Rect(0, 0, 50, 50), "Next: " + _bubbleData.GetValueForLevel(_nextBubbleController.NextBubbleLevelToSpawn.Value).ToString());
    }
}