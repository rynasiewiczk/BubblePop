using Project.Bubbles;
using UnityEngine;
using Zenject;

public class NextBubbleDebug : MonoBehaviour
{
    [Inject] private INextBubbleLevelToSpawnController _nextBubbleController;
    [Inject] private BubbleData _bubbleData;

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        GUI.TextArea(new Rect(0, 0, 50, 50), "Next: " + _bubbleData.GetValueForLevel(_nextBubbleController.BubbleLevelToSpawn.Value).ToString());
    }
}