using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Grid
{
    [Serializable]
    public class GridSettings
    {
        public Vector2Int StartGridSize = new Vector2Int(11, 6);
        public int WarmedRowsSize = 5;

        public int StartFreeBottomLines = 5;

        [FormerlySerializedAs("StartGridRow")] public GridRowSide startGridRowSide = GridRowSide.Left;
        
        public int AlwasFreeBottomLines = 3;
    }
}