﻿using Project.Grid;
using UnityEngine;
using Zenject;

public class BubblesDebugger : MonoBehaviour
{
    [Inject] private readonly IGridMap _gridMap = null;
    [Inject] private readonly PiecesData _piecesData = null;

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
                var color = _piecesData.GetColorForLevel(bubble.Level.Value);
                var viewPosition = _gridMap.GetCellsViewPosition(bubble.Position.Value);

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