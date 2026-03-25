using UnityEngine;

public class Hand : MonoBehaviour
{
	[SerializeField] private AnimationCurve _showCurve;
	[SerializeField] private Transform _showPosition;
	[SerializeField] private float _showDuration;
	[SerializeField] private AnimationCurve _hideCurve;
	[SerializeField] private Transform _hidePosition;
	[SerializeField] private float _hideDuration;

	private bool _shown = false;
	private float _animTimer = 0f;
	private float _activeAnimDuration = 0f;
	private Vector3 _startPos;

	private Vector3 TargetPosition => _shown ? _showPosition.position : _hidePosition.position;


	private void Update()
	{
		if (_activeAnimDuration <= 0f)
		{
			return;
		}

		_animTimer += Time.deltaTime;

		float ratio = _animTimer / _activeAnimDuration;
		if (ratio >= 1f)
		{
			// done
			_activeAnimDuration = 0f;
			transform.position = TargetPosition;
		}
		else
		{
			AnimationCurve curve = _shown ? _showCurve : _hideCurve;
			transform.position = Vector3.Lerp(_startPos, TargetPosition, curve.Evaluate(ratio));
		}
	}

	public void Show(bool show)
	{
		if (show == _shown)
		{
			return;
		}

		_animTimer = 0f;
		_startPos = transform.position;
		_shown = show;

		if (show)
		{
			_activeAnimDuration = _showDuration;
		}
		else
		{
			_activeAnimDuration = _hideDuration;
		}
	}
}
