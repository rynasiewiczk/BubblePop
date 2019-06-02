using Enums;
using Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace View.Aiming
{
    public class LineDrawerController : MonoBehaviour
    {
        [SerializeField] private LineDrawer _lineDrawer = null;
        [Inject] private readonly IGameStateController _gameStateController = null;

        private void Awake()
        {
            Debug.Assert(_lineDrawer, "Missing reference: _lineDrawer", this);
        }

        private void Start()
        {
            _gameStateController.GamePlayState.Where(x => x == GamePlayState.Aiming).Subscribe(x => _lineDrawer.Show());
            _gameStateController.GamePlayState.Where(x => x != GamePlayState.Aiming).Subscribe(x => _lineDrawer.Hide());
        }
    }
}