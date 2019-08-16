using Model;
using UnityEngine;
using Zenject;

public class GameStateDebugger : MonoBehaviour
{
    [Inject] private IGameStateController _gameStateController = null;

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        GUI.TextArea(new Rect(0, 150, 50, 50), "GameState: " + _gameStateController.GamePlayState.Value.ToString());
    }
}