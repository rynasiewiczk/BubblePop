using UnityEngine;
using Zenject;

namespace View.GridScoresDisplay
{
    public class GridScoreDisplayTextPool : MonoMemoryPool<Transform, GridScoreDisplayText>
    {
        protected override void Reinitialize(Transform p1, GridScoreDisplayText item)
        {
            item.transform.SetParent(p1);
        }
    }
}