using Project.Bubbles;
using UnityEngine;
using Zenject;

namespace Project.Grid.BuildingGrid
{
    public class InitialBubblesBuilder : IInitialBubblesBuilder
    {
        private readonly BubbleData _bubbleData;

        private readonly SignalBus _signalBus = null;
        private readonly SpawnBubbleOnGridSignal _spawnBubbleOnGridSignal = new SpawnBubbleOnGridSignal();

        public InitialBubblesBuilder(GridSettings gridSettings, BubbleData bubbleData, SignalBus signalBus)
        {
            _bubbleData = bubbleData;
            _signalBus = signalBus;

            CreateInitialBubbles(gridSettings);
        }

        private void CreateInitialBubbles(GridSettings gridSettings)
        {
            var gridSize = gridSettings.StartGridSize;

            for (int y = gridSettings.StartFreeBottomLines; y < gridSize.y + gridSettings.WarmedRowsSize; y++)
            {
                for (int x = 0; x < gridSettings.StartGridSize.x; x++)
                {
                    var bubbleLevel = Random.Range(1, _bubbleData.InitialMaxBubblesLevel + 1);

                    _spawnBubbleOnGridSignal.Position = new Vector2Int(x, y);
                    _spawnBubbleOnGridSignal.Level = bubbleLevel;
                    _signalBus.Fire(_spawnBubbleOnGridSignal);
                }
            }
        }
    }
}