using UnityEngine;

namespace Project.Grid
{
    public class Cell : ICell
    {
        public Vector2Int Position { get; }

        public Cell(Vector2Int position)
        {
            Position = position;
        }
    }
}