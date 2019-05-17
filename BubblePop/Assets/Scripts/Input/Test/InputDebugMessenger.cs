using DefaultNamespace;
using UnityEngine;
using Zenject;
using UniRx;

public class InputDebugMessenger : MonoBehaviour
{
    [Inject] private readonly IInputEventsNotifier _inputEventsNotifier = null;

    private void Start()
    {
        _inputEventsNotifier.OnInputStart.Skip(1).Subscribe(x => Debug.Log("Input START at pos: " + x.x + ", " + x.y));
        _inputEventsNotifier.OnInputMove.Skip(1).Subscribe(x => Debug.Log("Input MOVE at pos: " + x.x + ", " + x.y));
        _inputEventsNotifier.OnInputEnd.Skip(1).Subscribe(x => Debug.Log("Input END at pos: " + x.x + ", " + x.y));
    }
}
