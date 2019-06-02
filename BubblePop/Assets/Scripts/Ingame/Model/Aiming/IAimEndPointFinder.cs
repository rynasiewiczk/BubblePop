using System.Collections.Generic;
using UnityEngine;

namespace Project.Aiming
{
    public interface IAimEndPointFinder
    {
        PieceAimedData AimedPieceData { get; }
        List<Vector2> AimPath { get; }
    }
}