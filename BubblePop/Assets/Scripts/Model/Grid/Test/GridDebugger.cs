using Project.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class GridDebugger : MonoBehaviour
{
    [Inject] private readonly IGridMap _gridMap = null;
    [Inject] private readonly GridSettings _gridSettings = null;

    [SerializeField] private Color _cellsColor = Color.gray;
    [SerializeField] private int _switches = 1;

    [SerializeField] private bool _showGrid = false;
    [Space, SerializeField] private bool _showLineForBottomRowAvailableForBubbles = false;
    [SerializeField] private bool _showLineForTopRowAvailableForBubbles = false;
    [SerializeField] private bool _showLineForBubblesAbovePlayableArea = false;

    private void OnDrawGizmos()
    {
        if (_gridMap == null)
        {
            return;
        }

        if (_showGrid)
        {
            DrawGrid();
        }

        if (_showLineForBottomRowAvailableForBubbles)
        {
            DrawLineForBottomRowAvailableForBubbles();
        }

        if (_showLineForTopRowAvailableForBubbles)
        {
            DrawLineForTopRowAvailableForBubbles();
        }

        if (_showLineForBubblesAbovePlayableArea)
        {
            DrawLineForBubblesAbovePlayableArea();
        }
    }

    private void DrawLineForBubblesAbovePlayableArea()
    {
        var height = GridMapHelper.GetHeightOfRows(_gridSettings.StartGridSize.y)- .5f;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(-30, height), new Vector2(30, height));
    }

    private void DrawLineForTopRowAvailableForBubbles()
    {
        var level = GridMapHelper.GetHeightOfRows(_gridSettings.RowToScrollTokensDown)- .5f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(-30, level), new Vector2(30, level));
    }

    private void DrawLineForBottomRowAvailableForBubbles()
    {
        var level = GridMapHelper.GetHeightOfRows(_gridSettings.AlwasFreeBottomLines) - .5f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(-30, level), new Vector2(30, level));
    }

    private void DrawGrid()
    {
        Gizmos.color = _cellsColor;

        var cellsRegistry = _gridMap.CellsRegistry;

        for (int i = 0; i < cellsRegistry.Count; i++)
        {
            var viewPosition = _gridMap.GetCellsViewPosition(cellsRegistry[i].Position);
            Gizmos.DrawWireSphere(viewPosition, .5f);
        }
    }

    [Button] private void SwitchRowsMap()
    {
        _gridMap.SwitchRowSidesOnMap(_switches);
    }
}