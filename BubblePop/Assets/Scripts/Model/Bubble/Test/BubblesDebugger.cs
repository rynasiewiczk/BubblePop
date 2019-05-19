using Project.Grid;
using UnityEngine;
using Zenject;

public class BubblesDebugger : MonoBehaviour
{
    [Inject] private readonly IGridMap _gridMap = null;
    [Inject] private readonly BubbleData _bubbleData = null;

    private void OnDrawGizmos()
    {
        if (_gridMap == null)
        {
            return;
        }

        
        foreach (var bubble in _gridMap.BubblesRegistry)
        {
            try
            {
                var color = _bubbleData.GetColorForLevel(bubble.Level.Value);
                var viewPosition = _gridMap.GetGridViewPosition(bubble.Position.Value);

                Gizmos.color = color;
                Gizmos.DrawSphere(viewPosition, .5f);
            }
            catch
            {
                //debug only
            }
        }
    }
}