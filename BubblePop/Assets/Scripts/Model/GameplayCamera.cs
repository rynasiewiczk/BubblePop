using UnityEngine;

namespace Model
{
    public class GameplayCamera : MonoBehaviour, IGameplayCamera
    {
        [SerializeField] private Camera _camera = null;
        public Camera Camera => _camera;

        private void Awake()
        {
            Debug.Assert(_camera, "Missing reference: _camera", this);
        }
    }
}