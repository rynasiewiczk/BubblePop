using System;
using Project.Grid;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

public class GridDebugger : MonoBehaviour
{
    [Inject] private IGridMap _gridMap;

    [SerializeField] private Color _cellsColor = Color.gray;
    [SerializeField] private int _switches = 1;

    private void OnDrawGizmos()
    {
        if (_gridMap == null)
        {
            return;
        }

        Gizmos.color = _cellsColor;

        var cellsRegistry = _gridMap.CellsRegistry;
        var rowsSideMap = _gridMap.GridRowSidesMap;


        for (int i = 0; i < cellsRegistry.Count; i++)
        {
            var offset = 0f;
            if (rowsSideMap[cellsRegistry[i].Position.y] == GridRowSide.Right)
            {
                offset += .5f;
            }

            Gizmos.DrawWireSphere((Vector2) cellsRegistry[i].Position + new Vector2(offset, 0), .5f);
        }
    }

    [Button]
    private void SwitchRowsMap()
    {
        _gridMap.SwitchRowSidesOnMap(_switches);
    }
}