using Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseButtonController : MonoBehaviour
{
    [SerializeField] private Button _button = null;

    [Inject] private readonly IGameStateController _gameStateController = null;

    private void Awake()
    {
        Debug.Assert(_button, "Missing reference: _button", this);
    }

    private void Start()
    {
        _button.onClick.AddListener(() => _gameStateController.SetPause(true));
    }
}