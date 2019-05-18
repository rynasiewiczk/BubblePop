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
        ICell GetCellAtPositionOrNull(Vector2Int position);
        ReactiveDictionary<int, GridRowSide> GridRowSidesMap { get; }
        
        IBubble GetBubbleAtPositionOrNull(Vector2Int position);
        void CreateCellsRegistry(List<ICell> cells);

        void CreateGridRowSidesMap(Dictionary<int, GridRowSide> dictionary);
        void SwitchRowSidesOnMap(int switches);

        GridRowSide GetGridSideForRow(int row);
    }
}