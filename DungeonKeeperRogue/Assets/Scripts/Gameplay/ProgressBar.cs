using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Serializable]
    private class ProgressSpeedThreshold
    {
        [Range(0f, 1f)]
        public float _minFractionOfMaxValue;
        public float _lerpDuration;
    }

    [SerializeField] private GameObject _visualRoot;
    [SerializeField] private bool _visibleOnStart = true;
    [SerializeField] private Image _currentLerpFillImage;

    [Header("Filling")]
    [SerializeField] private List<ProgressSpeedThreshold> _fillSpeeds = new();

    [Header("Emptying")]
    [SerializeField] private bool _hideOnEmpty;
    [SerializeField] private List<ProgressSpeedThreshold> _emptyingSpeeds = new();

    private float _maxValue;
    private float _currentValue;
    private int _activeFillingCoroutines;
    private int _activeEmptyingCoroutines;

    public float MaxValue => _maxValue;

    private void Awake()
    {
        _visualRoot.SetActive(_visibleOnStart);
    }

	private void OnEnable()
	{
		// just became visible, so set out current fill if possible
		if (_maxValue > 0f)
		{
			SetFill(_currentValue / _maxValue);
		}
	}

    public void Init(float maxValue, float currentValue)
    {
        _maxValue = maxValue;
        _currentValue = currentValue;
    
        var normalizedValue = _currentValue / maxValue;
        SetFill(normalizedValue);
    }

    public void SetFill(float normalizedValue)
    {
        _currentLerpFillImage.fillAmount = normalizedValue;
        
        if (_hideOnEmpty && normalizedValue <= 0)
        {
            _visualRoot.SetActive(false);
        }
    }

    public void UpdateProgress(float newCurrentValue)
    {
        _visualRoot.SetActive(true);
    
        var previousValue = _currentValue;
        _currentValue = newCurrentValue;
        var delta = newCurrentValue - previousValue;

		if (isActiveAndEnabled)
		{
			if (delta > 0)
			{
				StartCoroutine(FillCoroutine(delta));
			}

			if (delta < 0)
			{
				StartCoroutine(EmptyCoroutine(Mathf.Abs(delta)));
			}
		}
    }

    private IEnumerator FillCoroutine(float delta)
    {
        ++_activeFillingCoroutines;
    
        var normalizedDeltaValue = Mathf.Clamp(delta / _maxValue, 0f, 1f);
        var speedThreshold = _fillSpeeds.FindLast(st => st._minFractionOfMaxValue <= normalizedDeltaValue);
        var lerpTimer = speedThreshold._lerpDuration;
        var currentAmountFilled = 0f;

        // Lerp the fill up to where it should be based on the speed threshold
        while (lerpTimer > 0f && _currentLerpFillImage.fillAmount is > 0f and < 1f)
        {
            var frameDelta = speedThreshold._lerpDuration * Time.deltaTime;
            frameDelta = Mathf.Min(frameDelta, normalizedDeltaValue - currentAmountFilled);
            _currentLerpFillImage.fillAmount += frameDelta;
            currentAmountFilled += frameDelta;
            lerpTimer -= Time.deltaTime;
            yield return null;
        }

        // If the speed threshold duration was 0f, just fill immediately
        if (currentAmountFilled <= 0)
        {
            _currentLerpFillImage.fillAmount += normalizedDeltaValue;
        }

        --_activeFillingCoroutines;
        PostCoroutineCleanup();
    }

    private IEnumerator EmptyCoroutine(float delta)
    {
        ++_activeEmptyingCoroutines;

        var normalizedDeltaValue = Mathf.Clamp(delta / _maxValue, 0f, 1f);
        var speedThreshold = _emptyingSpeeds.FindLast(st => st._minFractionOfMaxValue <= normalizedDeltaValue);
        var lerpTimer = speedThreshold._lerpDuration;
        var currentAmountEmptied = 0f;

        // Lerp the current fill over time since we aren't using the secondary fill
        while (lerpTimer > 0f && _currentLerpFillImage.fillAmount > 0f)
        {
            var frameDelta = speedThreshold._lerpDuration * Time.deltaTime;
            frameDelta = Mathf.Min(frameDelta, normalizedDeltaValue - currentAmountEmptied);
            _currentLerpFillImage.fillAmount -= frameDelta;
            currentAmountEmptied += frameDelta;
            lerpTimer -= Time.deltaTime;
            yield return null;
        }
        
        // If speed threshold duration is 0f, just empty the fill instantly
        if (currentAmountEmptied <= 0)
        {
            _currentLerpFillImage.fillAmount -= normalizedDeltaValue;
        }

        // Exit here because we aren't doing anything with the secondary fill
        --_activeEmptyingCoroutines;
        PostCoroutineCleanup();
    }

    private void PostCoroutineCleanup()
    {
        if (_activeFillingCoroutines > 0 || _activeEmptyingCoroutines > 0)
        {
            return;
        }
    
        SetFill(_currentValue / _maxValue);
    }
}
