using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace View.DestroyParticles
{
    public class PieceDestroyParticles : MonoBehaviour
    {
        [Inject] private readonly PieceDestroyOnCombineParticlesPool _pieceDestroyOnCombineParticlesPool = null;
        [Inject] private readonly PieceDestroyOnDropParticlesPool _pieceDestroyOnDropParticlesPool = null;

        [Inject] private readonly PiecesData _piecesData = null;
        [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;

        [SerializeField] private ParticleSystem _system = null;

        private float _enableTime;
        private float _duration = 5f;

        private bool _createdOnCombine = false;

        private void Awake()
        {
            Debug.Assert(_system, "Missing reference: _system", this);
        }

        private void Start()
        {
            _duration = _system.main.duration;
        }

        private void OnEnable()
        {
            _enableTime = Time.time;
        }

        private void Update()
        {
            if (!(Time.time - _duration > _enableTime))
            {
                return;
            }

            if (_createdOnCombine)
            {
                _pieceDestroyOnCombineParticlesPool.Despawn(this);
            }
            else
            {
                _pieceDestroyOnDropParticlesPool.Despawn(this);
            }
        }

        public void Setup(int pieceLevel, Vector2 position, bool createdOnCombine)
        {
            var color = _piecesData.GetColorForLevel(pieceLevel);
            Setup(color, position, createdOnCombine);
        }

        public void Setup(Color color, Vector2 position, bool createdOnCombine)
        {
            _createdOnCombine = createdOnCombine;

            transform.position = position;
            var main = _system.main;
            main.startColor = color;

            var particlesToEmitRange = createdOnCombine
                ? _bubbleViewSettings.DestroyOnCombineParticlesAmountRange
                : _bubbleViewSettings.DestroyOnDropParticlesAmountRange;


            var particlesToEmit = Random.Range(particlesToEmitRange.x, particlesToEmitRange.y);
            _system.Emit(particlesToEmit);
        }
    }
}