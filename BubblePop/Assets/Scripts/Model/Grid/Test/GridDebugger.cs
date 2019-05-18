using Project.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class GridDebugger : MonoBehaviour
{
    [Inject] private readonly IGridMap _gridMap = null;

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

        for (int i = 0; i < cellsRegistry.Count; i++)
        {
            var viewPosition = _gridMap.GetGridViewPosition(cellsRegistry[i].Position);
            Gizmos.DrawWireSphere(viewPosition, .5f);
        }
    }

    [Button] private void SwitchRowsMap()
    {
        _gridMap.SwitchRowSidesOnMap(_switches);
    }
}