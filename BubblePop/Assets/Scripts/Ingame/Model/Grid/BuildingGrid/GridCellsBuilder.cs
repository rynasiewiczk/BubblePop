using System.Collections.Generic;
using UnityEngine;

namespace Project.Grid.BuildingGrid
{
    public class GridCellsBuilder : IGridCellsBuilder
    {
        private readonly IGridMap _gridMap = null;

        public GridCellsBuilder(IGridMap gridMap, GridSettings gridSettings)
        {
            _gridMap = gridMap;
            BuildCellsRegistry(gridMap, gridSettings);
        }

        private void BuildCellsRegistry(IGridMap gridMap, GridSettings gridSettings)
        {
            var gridSize = gridSettings.StartGridSize;
            var cells = new List<ICell>(gridSize.x * gridSize.y + gridSize.x * gridSettings.WarmedRowsSize);
            var rowSides = new Dictionary<int, GridRowSide>(gridSize.y + gridSettings.WarmedRowsSize);

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    var cell = new Cell(new Vector2Int(x, y));
                    cells.Add(cell);
                }

                var rowSide = (GridRowSide) (((int) gridSettings.startGridRowSide + y) % 2);
                rowSides.Add(y, rowSide);
            }

            cells.AddRange(BuildCellsAboveTheTop(gridSize, gridSettings.WarmedRowsSize, gridSettings.SafetyRows));
            AddRowSidesAboveTheTop(gridSize, gridSettings.WarmedRowsSize + gridSettings.SafetyRows, gridSettings.startGridRowSide, rowSides);

            gridMap.CreateCellsRegistry(cells);
        }


        private List<ICell> BuildCellsAboveTheTop(Vector2Int gridSize, int gridSettingsWarmedRowsSize, int safetyRows)
        {
            var list = new List<ICell>();

            for (int y = 0; y < gridSettingsWarmedRowsSize + safetyRows; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    var cell = new Cell(new Vector2Int(x, y + gridSize.y));
                    list.Add(cell);
                }
            }

            return list;
        }

        private void AddRowSidesAboveTheTop(Vector2Int gridSize, int additionalSize, GridRowSide gridSettingsStartGridRowSide,
            Dictionary<int, GridRowSide> rowSidesMap)
        {
            for (int y = 0; y < additionalSize; y++)
            {
                var rowSide = (GridRowSide) (((int) gridSettingsStartGridRowSide + gridSize.y + y) % 2);
                rowSidesMap.Add(y + gridSize.y, rowSide);
            }

            _gridMap.CreateGridRowSidesMap(rowSidesMap);
        }
    }
}