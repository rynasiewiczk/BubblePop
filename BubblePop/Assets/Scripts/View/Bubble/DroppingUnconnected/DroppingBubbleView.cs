using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace View.DroppingUnconnected
{
    public class DroppingBubbleView : MonoBehaviour
    {
        [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;
        [Inject] private readonly DroppingBubbleViewPool _droppingBubbleViewPool = null;

        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        [SerializeField] private TextMeshPro _text = null;

        private int _horizontalDirection = -1;

        private float _verticalVelocity;
        private float _horizontalVelocity;

        private float _gravity;
        private float _rotationSpeed;

        private void Awake()
        {
            Debug.Assert(_spriteRenderer, "Missing reference: _spriteRenderer", this);
            Debug.Assert(_text, "Missing reference: _text", this);
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
                transform.localScale.y - scaleDownSpeed,
                transform.localScale.z);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.x, transform.eulerAngles.z + _rotationSpeed);

            var transparencySpeed = _bubbleViewSettings.DropBubbleTransparencyLossSpeed * Time.deltaTime;
            var color = _spriteRenderer.color;
            color.a = color.a - transparencySpeed;
            _spriteRenderer.color = color;
            _text.alpha = color.a;

            if (transform.position.y < _bubbleViewSettings.DroppBubbleHeightToDespawn)
            {
                DeSpawn();
            }
        }

        public void Setup(Vector2 position, Color color, int value, int direction)
        {
            transform.position = position;
            _spriteRenderer.color = color;
            _text.text = value.ToString();

            _horizontalDirection = direction;
            _horizontalVelocity = _horizontalDirection * Random.Range(0, _bubbleViewSettings.DropBubbleMaxHorizontalVelocity);
            _rotationSpeed = Random.Range(0, _bubbleViewSettings.DropBubbleRotationSpeed * Time.deltaTime);

            _verticalVelocity = Random.Range(_bubbleViewSettings.DropBubbleMinStartVerticalVelocity, _bubbleViewSettings.DropBubbleMaxStartVerticalVelocity);
        }

        private void DeSpawn()
        {
            transform.localScale = Vector3.one;
            transform.eulerAngles = Vector3.zero;
            var color = _spriteRenderer.color;
            color.a = 1;
            _spriteRenderer.color = color;
            _text.alpha = 1;

            _droppingBubbleViewPool.Despawn(this);
        }
    }
}