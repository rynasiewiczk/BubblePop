using Project.Bubbles;
using UnityEngine;

namespace Project.Grid.BuildingGrid
{
    public class InitialBubblesBuilder : IInitalBubblesBuilder
    {
        private readonly IGridMap _gridMap = null;
        private readonly GridSettings _gridSettings = null;

        private readonly IBubblesSpawner _bubblesSpawner = null;
        private readonly BubbleData _bubbleData;

        public InitialBubblesBuilder(IGridMap gridMap, GridSettings gridSettings, IBubblesSpawner bubblesSpawner, BubbleData bubbleData)
        {
            _gridMap = gridMap;
            _gridSettings = gridSettings;
            _bubblesSpawner = bubblesSpawner;
            _bubbleData = bubbleData;

            CreateInitialBubbles(gridMap, gridSettings);
        }

        private void CreateInitialBubbles(IGridMap gridMap, GridSettings gridSettings)
        {
            var gridSize = gridSettings.StartGridSize;

            for (int y = gridSettings.StartFreeBottomLines; y < gridSize.y + gridSettings.WarmedRowsSize; y++)
            {
                for (int x = 0; x < gridSettings.StartGridSize.x; x++)
                {
                    var bubbleLevel = Random.Range(1, _bubbleData.InitialMaxBubblesLevel + 1);
                    _bubblesSpawner.SpawnBubble(new Vector2Int(x, y), bubbleLevel);
                }
            }
        }
    }
}