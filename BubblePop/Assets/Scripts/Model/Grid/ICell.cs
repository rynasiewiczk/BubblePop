using UnityEngine;

namespace Project.Grid
{
    public interface ICell
    {
        Vector2Int Position { get; }
    }
}