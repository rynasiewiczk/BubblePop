using System;
using DG.Tweening;
using Enums;
using Model;
using OutGame;
using Project;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class IngameSceneVisibilityController : IIngameSceneVisibilityController, IDisposable
{
    public OnPlayerButtonClickDelegate OnPlayButtonClicked { get; } 
    private readonly SignalBus _signalBus = null;

    public ReactiveCommand OnIngamePaused { get; private set; } = new ReactiveCommand();

    public ReactiveCommand OnIngameUnpaused { get; } = new ReactiveCommand();

    private AsyncOperation _ingameLoadingOperation = null;

    public IngameSceneVisibilityController(SignalBus signalBus)
    {
        _signalBus = signalBus;
        OnPlayButtonClicked += ShowIngameScene;

        //artificial delay to go around a bug with ignoring scene activation allowance 
        DOVirtual.DelayedCall(.1f, LoadIngameSceneAsInactive);
        _signalBus.Subscribe<IngamePausedSignal>(ExecuteIngamePauseCommandIfNeeded);
    }

    private void ExecuteIngamePauseCommandIfNeeded(IngamePausedSignal signal)
    {
        if (signal.Paused)
        {
            OnIngamePaused.Execute();
        }
    }

    private void ShowIngameScene()
    {
        if (_ingameLoadingOperation == null)
        {
            Debug.LogError("Operation of loading ingame was not triggered. Returning.");
            return;
        }

        OnIngameUnpaused.Execute();
    }

    private void LoadIngameSceneAsInactive()
    {
        _ingameLoadingOperation = SceneManager.LoadSceneAsync(SRScenes.IngameScene.name, LoadSceneMode.Additive);
    }

    public void Dispose()
    {
        OnIngamePaused?.Dispose();
        OnIngameUnpaused?.Dispose();

        _signalBus.TryUnsubscribe<IngamePausedSignal>(ExecuteIngamePauseCommandIfNeeded);

        GC.SuppressFinalize(this);
    }
}