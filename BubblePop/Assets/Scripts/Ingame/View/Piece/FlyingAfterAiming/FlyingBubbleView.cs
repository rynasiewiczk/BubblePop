using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace View.FlyingAfterAiming
{
    public class FlyingBubbleView : MonoBehaviour
    {
        [SerializeField] private PieceView _pieceView = null;
        
        [Inject] private readonly FlyingBubbleViewPool _flyingBubbleViewPool = null;

        private Vector2[] _path;
        private int _value;

        private void Awake()
        {
            Debug.Assert(_pieceView, "Missing reference: _pieceView", this);
        }

        public void Setup(Vector3[] path, int level, float duration)
        {
            _pieceView.Setup(level);
            transform.DOPath(path, duration).OnComplete(() => { _flyingBubbleViewPool.Despawn(this); });
        }
    }
}