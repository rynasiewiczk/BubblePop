using System;
using Enums;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace View.DestroyParticles
{
    public class PieceDestroyParticles : MonoBehaviour
    {
        [Inject] private readonly PieceDestroyOnCombineParticlesPool _pieceDestroyOnCombineParticlesPool = null;
        [Inject] private readonly PieceDestroyOnDropParticlesPool _pieceDestroyOnDropParticlesPool = null;
        [Inject] private readonly PieceDestroyOnOvergrownExplosionParticlesPool _pieceDestroyOnOvergrownExplosionParticlesPool = null;

        [Inject] private readonly PiecesData _piecesData = null;
        [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;

        [SerializeField] private ParticleSystem _system = null;

        private float _enableTime;
        private float _duration = 5f;

        private DestroyParticlesSourceType _destroyParticlesSourceType;

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


            switch (_destroyParticlesSourceType)
            {
                case DestroyParticlesSourceType.Combine:
                    _pieceDestroyOnCombineParticlesPool.Despawn(this);
                    break;
                case DestroyParticlesSourceType.Dropping:
                    _pieceDestroyOnDropParticlesPool.Despawn(this);
                    break;
                case DestroyParticlesSourceType.ExplodeOvergrown:
                    _pieceDestroyOnOvergrownExplosionParticlesPool.Despawn(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Setup(int pieceLevel, Vector2 position, DestroyParticlesSourceType type)
        {
            var color = _piecesData.GetColorsSetForLevel(pieceLevel).InnerColor;
            Setup(color, position, type);
        }

        public void Setup(Color color, Vector2 position, DestroyParticlesSourceType type)
        {
            _destroyParticlesSourceType = type;

            transform.position = position;
            var main = _system.main;

            Vector2Int particlesToEmitRange;

            switch (type)
            {
                case DestroyParticlesSourceType.Combine:
                    particlesToEmitRange = _bubbleViewSettings.DestroyOnCombineParticlesAmountRange;
                    main.startColor = color;
                    break;
                case DestroyParticlesSourceType.Dropping:
                    particlesToEmitRange = _bubbleViewSettings.DestroyOnDropParticlesAmountRange;
                    main.startColor = color;
                    break;
                case DestroyParticlesSourceType.ExplodeOvergrown:
                    particlesToEmitRange = _bubbleViewSettings.DestroyOnOvergrownExplosionParticlesAmountRange;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var particlesToEmit = Random.Range(particlesToEmitRange.x, particlesToEmitRange.y);
            _system.Emit(particlesToEmit);
        }
    }
}