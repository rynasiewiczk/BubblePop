using TMPro;
using UnityEngine;
using Zenject;

namespace View.GridScoresDisplay
{
    public class GridScoreDisplayText : MonoBehaviour
    {
        [Inject] private readonly UiData _uiData = null;
        [Inject] private readonly GridScoreDisplayTextPool _gridScoreDisplayTextPool = null;

        [SerializeField] private TextMeshProUGUI _text = null;

        private RectTransform RectTransform => transform as RectTransform;
        
        private float _lifeTimeLeft = 0;
        private float _moveUpSpeed = 0;
        private float _fadeOutSpeed = 0;
        private float _scaleUpSpeed = 0;

        private void Awake()
        {
            Debug.Assert(_text, "Missing reference: _text", this);
        }

        private void Start()
        {
            _moveUpSpeed = _uiData.MoveUpSpeed * Time.deltaTime;
            _fadeOutSpeed = _uiData.GridScoreText_FadeOutSpeed * Time.deltaTime;
            _scaleUpSpeed = _uiData.GridScoreText_ScaleupSpeed * Time.deltaTime;
        }

        private void Update()
        {
            if (_lifeTimeLeft < 0)
            {
                DeSpawn();
            }

            _lifeTimeLeft -= Time.deltaTime;

            transform.position = new Vector2(transform.position.x, transform.position.y + _moveUpSpeed);
            transform.localScale = new Vector2(transform.localScale.x + _scaleUpSpeed, transform.localScale.y + _scaleUpSpeed);
            _text.alpha -= _fadeOutSpeed;
        }

        public void Setup(Vector2 position, string score, float fontSize, Color color)
        {
            transform.localScale = Vector3.one;
            RectTransform.anchoredPosition = position;

            _text.text = score;
            _text.fontSize = fontSize;
            _text.color = color;

            _lifeTimeLeft = _uiData.GridScoreText_LifeTime;
        }

        private void DeSpawn()
        {
            transform.localScale = Vector3.one;
            _text.alpha = 1;

            _gridScoreDisplayTextPool.Despawn(this);
        }
    }
}