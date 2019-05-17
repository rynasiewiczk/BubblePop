using System.Collections.Generic;
using Project.Bubbles;
using UniRx;
using UnityEngine;

namespace Project.Grid
{
    public class GridMap : IGridMap
    {
        private readonly GridSettings _gridSettings = null;

        public ReactiveProperty<Vector2Int> Size { get; private set; }
        public ReactiveCollection<Bubble> BubblesRegistry { get; private set; }
        public ReactiveCollection<ICell> CellsRegistry { get; private set; }
        public ReactiveDictionary<int, GridRowSide> GridRowSidesMap { get; private set; }

        public GridMap(GridSettings gridSettings)
        {
            Size = new ReactiveProperty<Vector2Int>(gridSettings.StartGridSize);
            CellsRegistry = new ReactiveCollection<ICell>();
            BubblesRegistry = new ReactiveCollection<Bubble>();

            _gridSettings = gridSettings;
            Size.Value = _gridSettings.StartGridSize;
        }

        public Bubble GetBubbleAtPosition(Vector2Int position)
        {
            throw new System.NotImplementedException();
        }

        public Bubble GetBubbleAtPosition(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public void CreateCellsRegistry(List<ICell> cells)
        {
            foreach (var cell in cells)
            {
                CellsRegistry.Add(cell);
            }
        }

        public void CreateGridRowSidesMap(Dictionary<int, GridRowSide> dictionary)
        {
            GridRowSidesMap = new ReactiveDictionary<int, GridRowSide>(dictionary);
        }

        public void SwitchRowSidesOnMap(int switches)
        {
            if (switches % 2 == 0)
            {
                return;
            }

            for (int i = 0; i < GridRowSidesMap.Count; i++)
            {
                GridRowSidesMap[i] = GridRowSidesMap[i] == GridRowSide.Left ? GridRowSide.Right : GridRowSide.Left;
            }
        }
    }
}