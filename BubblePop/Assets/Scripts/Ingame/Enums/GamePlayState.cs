namespace Enums
{
    public enum GamePlayState
    {
        None = 0,

        Paused,
        
        Idle,
        Aiming,
        PieceFlying,
        PlacingPieceOnGrid,
        PiecesCombining,
        WaitingForPiecesCombine,
        DropPiecesAfterCombining,
        ScrollRows,
        FillPiecesAboveTop,
    }
}