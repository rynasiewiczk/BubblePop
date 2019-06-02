using Model;
using Project.Input;
using UniRx;
using UnityEngine;
using Zenject;

public class MouseInputEventsNotifier : ITickable, IInputEventsNotifier
{
    public ReactiveProperty<Vector2> OnInputStart { get; private set; }
    public ReactiveProperty<Vector2> OnInputEnd { get; private set; }
    public ReactiveProperty<Vector2> OnInputMove { get; private set; }

    private readonly Camera _gameplayCamera;

    public MouseInputEventsNotifier(IGameplayCamera gameplayCamera)
    {
        _gameplayCamera = gameplayCamera.Camera;

        OnInputStart = new ReactiveProperty<Vector2>();
        OnInputMove = new ReactiveProperty<Vector2>();
        OnInputEnd = new ReactiveProperty<Vector2>();
    }

    public void Tick()
    {
        HandleInputStart();
        HandleInputPosition();
        HandleInputEnd();
    }

    private void HandleInputStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnInputStart.Value = _gameplayCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void HandleInputPosition()
    {
        if (Input.GetMouseButton(0))
        {
            OnInputMove.Value = _gameplayCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void HandleInputEnd()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnInputEnd.Value = _gameplayCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}