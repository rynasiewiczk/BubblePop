using System.Collections.Generic;
using System.Linq;
using Project.Bubbles;
using UniRx;
using UnityEngine;

namespace Project.Grid
{
    public class GridMap : IGridMap
    {
        public ReactiveProperty<Vector2Int> Size { get; private set; }
        public ReactiveCollection<IBubble> BubblesRegistry { get; private set; }
        public ReactiveCollection<ICell> CellsRegistry { get; private set; }
        public ReactiveDictionary<int, GridRowSide> GridRowSidesMap { get; private set; }

        public GridMap(GridSettings gridSettings, IBubblesSpawner bubblesSpawner)
        {
            bubblesSpawner.JustSpawned.Where(x => x != null).Subscribe(AddBubbleToRegistry);

            Size = new ReactiveProperty<Vector2Int>(gridSettings.StartGridSize);
            CellsRegistry = new ReactiveCollection<ICell>();
            BubblesRegistry = new ReactiveCollection<IBubble>();

            Size.Value = gridSettings.StartGridSize;
        }

        public IBubble GetBubbleAtPosition(Vector2Int position)
        {
            var bubble = BubblesRegistry.FirstOrDefault(x => x.Position.Value == position);
            return bubble;
        }

        public IBubble GetBubbleAtPosition(int x, int y)
        {
            var position = new Vector2Int(x, y);
            return GetBubbleAtPosition(position);
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

        public GridRowSide GetGridSideForRow(int row)
        {
            if (!GridRowSidesMap.ContainsKey(row))
            {
                Debug.LogError("GridRowSidesMap does not contain provided row. Provided row: " + row + ". Returning GridRowSide.Left");
                return GridRowSide.Left;
            }

            return GridRowSidesMap[row];
        }

        private void AddBubbleToRegistry(IBubble bubble)
        {
            if (BubblesRegistry.FirstOrDefault(x => x.Position.Value == bubble.Position.Value) != null)
            {
                Debug.LogError("BubblesRegistry already contains bubble at position " + bubble.Position.Value);
            }

            bubble.Destroyed.Subscribe(x => RemoveDestroyedBubbleFromRegistry(bubble));
            BubblesRegistry.Add(bubble);
        }

        private void RemoveDestroyedBubbleFromRegistry(IBubble bubble)
        {
            if (!BubblesRegistry.Contains(bubble))
            {
                Debug.LogError("BubblesRegistry does not contain provided bubble! Position of provided bubble: " + bubble.Position + ". Returning.");
                return;
            }

            BubblesRegistry.Remove(bubble);
        }
    }
}