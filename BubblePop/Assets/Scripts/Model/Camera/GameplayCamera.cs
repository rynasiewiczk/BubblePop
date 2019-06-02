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
        
        public float GetBottomHeightOfCameraView()
        {
            var result = _camera.transform.position.y - _camera.orthographicSize;
            return result;
        }
    }
}