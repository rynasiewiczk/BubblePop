using System.Collections.Generic;
using UnityEngine;

namespace Calculations
{
    public static class VectorsHelper 
    {
        public static float SumMagnitudeOfVectors(List<Vector2> vectors)
        {
            var distance = 0f;
            for (int i = 1; i < vectors.Count; i++)
            {
                distance += (vectors[i] - vectors[i - 1]).magnitude;
            }

            return distance;
        }
    }
}