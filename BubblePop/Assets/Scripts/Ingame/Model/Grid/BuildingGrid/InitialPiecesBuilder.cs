using Project.Pieces;
using UnityEngine;
using Zenject;

namespace Project.Grid.BuildingGrid
{
    public class InitialPiecesBuilder : IInitialPiecesBuilder
    {
        private readonly PiecesData _piecesData;

        private readonly SignalBus _signalBus = null;
        private readonly SpawnPieceOnGridSignal _spawnPieceOnGridSignal = new SpawnPieceOnGridSignal();

        public InitialPiecesBuilder(GridSettings gridSettings, PiecesData piecesData, SignalBus signalBus)
        {
            _piecesData = piecesData;
            _signalBus = signalBus;

            CreateInitialPieces(gridSettings);
        }

        private void CreateInitialPieces(GridSettings gridSettings)
        {
            var gridSize = gridSettings.StartGridSize;

            for (int y = gridSettings.StartFreeBottomLines; y < gridSize.y + gridSettings.WarmedRowsSize; y++)
            {
                for (int x = 0; x < gridSettings.StartGridSize.x; x++)
                {
                    var pieceLevel = Random.Range(1, _piecesData.GetBiggestPieceLevelToLiveOnGrid(1) + 1);

                    _spawnPieceOnGridSignal.Position = new Vector2Int(x, y);
                    _spawnPieceOnGridSignal.Level = pieceLevel;
                    _signalBus.Fire(_spawnPieceOnGridSignal);
                }
            }
        }
    }
}