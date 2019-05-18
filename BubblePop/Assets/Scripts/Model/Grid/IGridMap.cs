using System.Collections.Generic;
using Project.Bubbles;
using UniRx;
using UnityEngine;

namespace Project.Grid
{
    public interface IGridMap
    {
        ReactiveProperty<Vector2Int> Size { get; }
        ReactiveCollection<IBubble> BubblesRegistry { get; }
        ReactiveCollection<ICell> CellsRegistry { get; }

        ReactiveDictionary<int, GridRowSide> GridRowSidesMap { get; }
        
        IBubble GetBubbleAtPosition(Vector2Int position);
        IBubble GetBubbleAtPosition(int x, int y);
        void CreateCellsRegistry(List<ICell> cells);

        void CreateGridRowSidesMap(Dictionary<int, GridRowSide> dictionary);
        void SwitchRowSidesOnMap(int switches);

        GridRowSide GetGridSideForRow(int row);
    }
}