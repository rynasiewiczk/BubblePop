using Ingame.View.UI.Popups.LevelUp;
using Project.Scripts.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpView : PopupView
{
    [SerializeField] private Button _continueButton = null;
    [SerializeField] private TextMeshProUGUI _levelText = null;


    protected override void Awake()
    {
        base.Awake();

        Debug.Assert(_continueButton, "Missing reference: _continueButton");
        Debug.Assert(_levelText, "Missing reference: _levelText");
    }

    public override void Show(IPopupRequest request = null)
    {
        base.Show(request);
        var levelUpRequest = (LevelUpPopupRequest) request;

        _levelText.text = levelUpRequest.Level.ToString();
        _continueButton.onClick.AddListener(() => levelUpRequest.OnContinueButtonClick.Invoke());
    }

    protected override void ClearButtonsListeners()
    {
        _continueButton.onClick.RemoveAllListeners();
    }
}