using UnityEngine;

public class ProgressBarParticles : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform = null;
    [SerializeField] private ParticleSystem _particleSystem = null;

    private void Awake()
    {
        Debug.Assert(_rectTransform, "Missing reference: _rectTransform", this);
        Debug.Assert(_particleSystem, "Missing reference: _particleSystem", this);
    }

    public void Setup(Vector2 spawnPosition, int gainedScore)
    {
        _rectTransform.anchoredPosition = spawnPosition;
        var particlesAmount = GetAmountOfParticlesByGainedScore(gainedScore);
        _particleSystem.Emit(particlesAmount);
    }

    private int GetAmountOfParticlesByGainedScore(int gainedScore)
    {
        if (gainedScore < 10)
        {
            return 3;
        }

        if (gainedScore < 65)
        {
            return 5;
        }

        if (gainedScore < 129)
        {
            return 7;
        }

        if (gainedScore < 1029)
        {
            return 10;
        }

        return 12;
    }
}