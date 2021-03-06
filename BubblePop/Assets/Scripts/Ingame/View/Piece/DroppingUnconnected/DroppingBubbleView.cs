using Enums;
using Model;
using TMPro;
using UnityEngine;
using View.DestroyParticles;
using Zenject;
using Random = UnityEngine.Random;

namespace View.DroppingUnconnected
{
    public class DroppingBubbleView : MonoBehaviour
    {
        [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;
        [Inject] private readonly DroppingBubbleViewPool _droppingBubbleViewPool = null;

        [Inject] private readonly PieceDestroyOnDropParticlesPool _pieceDestroyOnDropParticlesPool = null;
        [Inject] private readonly IGameplayCamera _gameplayCamera = null;

        [SerializeField] private PieceView _pieceView = null;

        private int _horizontalDirection = -1;

        private float _verticalVelocity;
        private float _horizontalVelocity;

        private float _gravity;
        private float _rotationSpeed;

        private bool _triggeredFallParticles = false;

        private void Awake()
        {
            Debug.Assert(_pieceView, "Missing reference: _pieceView", this);
        }

        private void Start()
        {
            _gravity = _bubbleViewSettings.DropBubbleGravity;
        }

        private void Update()
        {
            transform.position += new Vector3(_horizontalVelocity * Time.deltaTime, _verticalVelocity * Time.deltaTime, 0);
            _verticalVelocity += _gravity * Time.deltaTime;

            var scaleDownSpeed = _bubbleViewSettings.DropBubbleScaleReduceSpeed * Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x - scaleDownSpeed,
                transform.localScale.y - scaleDownSpeed, transform.localScale.z);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.x, transform.eulerAngles.z + _rotationSpeed);

            var transparencySpeed = _bubbleViewSettings.DropBubbleTransparencyLossSpeed * Time.deltaTime;
            var col = _pieceView.GetAlpha();
            col -= transparencySpeed;
            _pieceView.SetAlpha(col);

            FireDestroyParticlesIfOnEdgeOnView();
            DespawnIfLowEnough();
        }

        private void FireDestroyParticlesIfOnEdgeOnView()
        {
            if (_triggeredFallParticles || !(transform.position.y + .5f <= _gameplayCamera.GetBottomHeightOfCameraView()))
            {
                return;
            }

            _triggeredFallParticles = true;
            var destroyParticles = _pieceDestroyOnDropParticlesPool.Spawn();
            destroyParticles.Setup(_pieceView.GetMainColor(), (Vector2) transform.position + Vector2.up / 2, DestroyParticlesSourceType.Dropping);
        }

        private void DespawnIfLowEnough()
        {
            if (!(transform.position.y < _bubbleViewSettings.DroppBubbleHeightToDespawn))
            {
                return;
            }

            DeSpawn();
        }

        public void Setup(Vector2 position, int level, int direction)
        {
            transform.position = position;
            _pieceView.Setup(level);
            _triggeredFallParticles = false;

            _horizontalDirection = direction;
            _horizontalVelocity = _horizontalDirection * Random.Range(0, _bubbleViewSettings.DropBubbleMaxHorizontalVelocity);
            _rotationSpeed = Random.Range(0, _bubbleViewSettings.DropBubbleRotationSpeed * Time.deltaTime);
            _verticalVelocity = Random.Range(_bubbleViewSettings.DropBubbleMinStartVerticalVelocity, _bubbleViewSettings.DropBubbleMaxStartVerticalVelocity);
        }

        private void DeSpawn()
        {
            transform.localScale = Vector3.one;
            transform.eulerAngles = Vector3.zero;
            _pieceView.SetAlpha(1);

            _droppingBubbleViewPool.Despawn(this);
        }
    }
}