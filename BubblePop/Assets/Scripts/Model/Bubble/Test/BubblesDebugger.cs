using System;
using System.Collections;
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
            var color = _bubbleData.GetColorForLevel(bubble.Level.Value);
            Gizmos.color = color;

            var offset = 0f;
            if (_gridMap.GridRowSidesMap[bubble.Position.Value.y] == GridRowSide.Right)
            {
                offset += .5f;
            }

            Gizmos.DrawSphere(bubble.Position.Value + new Vector2(offset, 0), .5f);
        }
    }
}