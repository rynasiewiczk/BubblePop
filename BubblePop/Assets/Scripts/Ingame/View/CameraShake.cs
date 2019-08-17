using DG.Tweening;
using Ingame.Model.ExplodingAfterCombining;
using UnityEngine;
using Zenject;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform _toShake = null;

    [SerializeField] private float _strengthOnOvergrownExplosion = 3f;
    [SerializeField] private float _durationOnOvergrownExplosion = .3f;
    [SerializeField] private int _vibratoOnOvergrownExplosion = 5;

    [Inject] private readonly SignalBus _signalBus = null;

    private Vector3 _defaultPosition;

    private void Start()
    {
        _defaultPosition = _toShake.localPosition;
        _signalBus.Subscribe<OvergrownExplosionSignal>(ShakeOnOvergrownExplosion);
    }

    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<OvergrownExplosionSignal>(ShakeOnOvergrownExplosion);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShakeOnOvergrownExplosion();
        }
        _toShake.localPosition = new Vector3(_toShake.localPosition.x, _toShake.localPosition.y, 0);
    }

    private void ShakeOnOvergrownExplosion()
    {
        _toShake.DOKill();
        _toShake.DOShakePosition(_durationOnOvergrownExplosion, _strengthOnOvergrownExplosion, _vibratoOnOvergrownExplosion).OnComplete(() =>
            _toShake.transform.DOLocalMove(_defaultPosition, .3f));
    }
}