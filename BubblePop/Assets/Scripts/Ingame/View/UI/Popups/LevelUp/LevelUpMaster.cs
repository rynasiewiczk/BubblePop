using System;
using Ingame.View.UI.Popups.LevelUp;
using Model;
using Project.Scripts.Popups;
using UnityEngine;
using Zenject;

public class LevelUpMaster : PopupMaster
{
    [Inject] private readonly IGameStateController _gameStateController = null;

    private void Start()
    {
        SubscribeToRequest();
        HideView();
    }

    private void OnDestroy()
    {
        UnsubscribeFromRequest();
    }

    protected override void SubscribeToRequest()
    {
        SignalBus.Subscribe<LevelUpPopupRequest>(OnRequest);
    }

    protected override void UnsubscribeFromRequest()
    {
        SignalBus.Unsubscribe<LevelUpPopupRequest>(OnRequest);
    }

    protected override void OnRequest(IPopupRequest request)
    {
        _gameStateController.SetPause(true);

        var levelUpRequest = (LevelUpPopupRequest) request;
        levelUpRequest.OnContinueButtonClick = ShowAddOrContinue;
        
        base.OnRequest(request);
    }

    private void ShowAddOrContinue()
    {
        if (ShouldShowAdd())
        {
            ShowAdd(Hide);
        }
        else
        {
            Hide();
        }
    }

    private void ShowAdd(Action callback)
    {
        Debug.Log("show add");
        callback.Invoke();
    }

    private bool ShouldShowAdd()
    {
        return true;
    }

    private void Hide()
    {
        _gameStateController.SetPause(false);
        HideView();
    }
}