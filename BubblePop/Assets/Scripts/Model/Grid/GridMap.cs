using System.Collections.Generic;
using System.Linq;
using Project.Pieces;
using UniRx;
using UnityEngine;

namespace Project.Grid
{
    public class GridMap : IGridMap
    {
        public ReactiveProperty<Vector2Int> Size { get; private set; }
        public ReactiveCollection<IPiece> PiecesRegistry { get; private set; }
        public ReactiveCollection<ICell> CellsRegistry { get; private set; }

        public ReactiveDictionary<int, GridRowSide> GridRowSidesMap { get; private set; }

        public GridMap(GridSettings gridSettings, IBubblesSpawner bubblesSpawner)
        {
            bubblesSpawner.LatestSpawnedBubble.Where(x => x != null).Subscribe(AddBubbleToRegistry);

            Size = new ReactiveProperty<Vector2Int>(gridSettings.StartGridSize);
            CellsRegistry = new ReactiveCollection<ICell>();
            PiecesRegistry = new ReactiveCollection<IPiece>();

            Size.Value = gridSettings.StartGridSize;
        }

        public ICell GetCellAtPositionOrNull(Vector2Int position)
        {
            var cell = CellsRegistry.FirstOrDefault(x => x.Position == position);
            return cell;
        }

        public IPiece GetPieceAtPositionOrNull(Vector2Int position)
        {
            foreach (var bubble in PiecesRegistry)
            {
                if (bubble.Position.Value == position)
                {
                    return bubble;
                }                
            }
            return null;
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
            if (GridRowSidesMap.ContainsKey(row))
            {
                return GridRowSidesMap[row];
            }
            
            Debug.LogError("GridRowSidesMap does not contain provided row. Provided row: " + row + ". Returning GridRowSide.Left");
            return GridRowSide.Left;

        }

        private void AddBubbleToRegistry(IPiece piece)
        {
            if (PiecesRegistry.FirstOrDefault(x => x.Position.Value == piece.Position.Value) != null)
            {
                Debug.LogError("BubblesRegistry already contains bubble at position " + piece.Position.Value);
            }

            piece.Destroyed.Subscribe(x => RemoveDestroyedBubbleFromRegistry(piece));
            PiecesRegistry.Add(piece);
        }

        private void RemoveDestroyedBubbleFromRegistry(IPiece piece)
        {
            if (!PiecesRegistry.Contains(piece))
            {
                Debug.LogError("BubblesRegistry does not contain provided bubble! Position of provided bubble: " + piece.Position + ". Returning.");
                return;
            }

            PiecesRegistry.Remove(piece);
        }
    }
}