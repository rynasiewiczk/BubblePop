using System;
using DG.Tweening;
using Enums;
using Model;
using OutGame;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class IngameSceneController : IIngameSceneController, IDisposable
{
    private readonly SignalBus _signalBus = null;
    
    public ReactiveCommand OnIngameSceneActivated { get; private set; } = new ReactiveCommand();
    public ReactiveCommand OnIngamePaused { get; private set; } = new ReactiveCommand();
    public ReactiveCommand OnIngameUnpaused { get; private set; } = new ReactiveCommand();

    private AsyncOperation _ingameLoadingOperation = null;

    public IngameSceneController(PlayButtonController playButtonController, SignalBus signalBus)
    {
        _signalBus = signalBus;
        
        //artificial delay to go around a bug with ignoring scene activation allowance 
        DOVirtual.DelayedCall(.1f, LoadIngameSceneAsInactive);

        playButtonController.OnPlayButtonClicked += ActivateIngameScene;

        _signalBus.Subscribe<GameStateChangeSignal>(ExecuteIngameStateChangeCommandsIfNeeded);
    }

    private void ExecuteIngameStateChangeCommandsIfNeeded(GameStateChangeSignal x)
    {
        if (x.GamePlayState == GamePlayState.Paused)
        {
            OnIngamePaused.Execute();
        }
        else if (x.GamePlayState != GamePlayState.Paused && x.PrevState == GamePlayState.Paused)
        {
            OnIngameUnpaused.Execute();
        }
    }

    private void ActivateIngameScene()
    {
        if (_ingameLoadingOperation == null)
        {
            Debug.LogError("Operation of loading ingame was not triggered. Returning.");
            return;
        }

        _ingameLoadingOperation.allowSceneActivation = true;
        OnIngameSceneActivated?.Execute();
    }

    private void LoadIngameSceneAsInactive()
    {
        _ingameLoadingOperation = SceneManager.LoadSceneAsync(SRScenes.IngameScene.name, LoadSceneMode.Additive);
        _ingameLoadingOperation.allowSceneActivation = false;
    }

    public void Dispose()
    {
        OnIngameSceneActivated?.Dispose();
        OnIngamePaused?.Dispose();
        OnIngameUnpaused?.Dispose();
        
        _signalBus.TryUnsubscribe<GameStateChangeSignal>(ExecuteIngameStateChangeCommandsIfNeeded);
        
        GC.SuppressFinalize(this);
    }
}