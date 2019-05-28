using UnityEngine;
using Zenject;

namespace View.DestroyParticles
{
    public class PieceDestroyParticles : MonoBehaviour
    {
        [Inject] private readonly PieceDestroyParticlesPool _pieceDestroyParticlesPool = null;
        [Inject] private readonly PiecesData _piecesData = null;

        [SerializeField] private ParticleSystem _system = null;

        private float _enableTime;
        private float _lifeTime = 1f;

        private void Awake()
        {
            Debug.Assert(_system, "Missing reference: _system", this);
        }

        private void OnEnable()
        {
            _enableTime = Time.time;
        }

        private void Update()
        {
            if (Time.time - _lifeTime > _enableTime)
            {
                _pieceDestroyParticlesPool.Despawn(this);
            }
        }

        public void Setup(int pieceLevel, Vector3 position)
        {
            transform.position = position;

            var color = _piecesData.GetColorForLevel(pieceLevel);
            var main = _system.main;
            main.startColor = color;

            var particlesToEmit = Random.Range(3, 6);
            _system.Emit(particlesToEmit);
        }
    }
}