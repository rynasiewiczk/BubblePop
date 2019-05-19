using Model.Progress.PlayerLevelController;
using UnityEngine;
using Zenject;

public class PlayerLevelDebugger : MonoBehaviour
{
    [Inject] private readonly IPlayerLevelController _controller = null;

    private void OnGUI()
    {
        GUI.TextArea(new Rect(0, 100, 50, 50), "Level: " + _controller.PlayerLevel.Value);
    }
}