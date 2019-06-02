using System;
using UniRx;

namespace OutGame
{
    public interface IIngameSceneController
    {
        ReactiveCommand OnIngameSceneActivated { get; }
        ReactiveCommand OnIngamePaused { get; }
        ReactiveCommand OnIngameUnpaused { get; }
    }
}