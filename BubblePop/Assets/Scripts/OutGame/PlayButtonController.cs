using System;
using OutGame;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayButtonController : MonoBehaviour
{
    public Action OnPlayButtonClicked;
    [SerializeField] private Button _button = null;

    [Inject] private readonly IIngameSceneVisibilityController _ingameSceneVisibilityController = null;

    private void Awake()
    {
        Debug.Assert(_button, "Missing reference: _button", this);
    }

    private void Start()
    {
        _button.onClick.AddListener(FirePlayButtonClickAction);
    }

    private void FirePlayButtonClickAction()
    {
        OnPlayButtonClicked?.Invoke();
        _ingameSceneVisibilityController.OnPlayButtonClicked?.Invoke();
    }
}