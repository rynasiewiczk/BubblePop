using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonController : MonoBehaviour
{
    public Action OnPlayButtonClicked;
    [SerializeField] private Button _button = null;

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
    }
}
