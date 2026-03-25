using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class TweenAnimBase
{
    public bool _useTween;
    public AnimationCurve _curve;
}

[Serializable]
public class TweenAnimMove : TweenAnimBase
{
    public Vector2 _fromPosition;
    public Vector2 _toPosition;

    public void UpdateTween(Transform transform, float normalizedTime)
    {
        var lerpTime = _curve.Evaluate(normalizedTime);
        transform.localPosition = Vector2.Lerp(_fromPosition, _toPosition, lerpTime);
    }

    public void CompleteTween(Transform transform)
    {
        transform.localPosition = _toPosition;
    }
}

[Serializable]
public class TweenAnimRotate : TweenAnimBase
{
    public float _fromZRotation;
    public float _toZRotation;

    public void UpdateTween(Transform transform, float normalizedTime)
    {
        var lerpTime = _curve.Evaluate(normalizedTime);
        var newZRotation = (_toZRotation - _fromZRotation) * lerpTime + _fromZRotation;
        var baseRotation = transform.rotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(baseRotation.x, baseRotation.y, newZRotation);
    }

    public void CompleteTween(Transform transform)
    {
        var baseRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(baseRotation.x, baseRotation.y, _toZRotation);
    }
}

[Serializable]
public class TweenAnimScale : TweenAnimBase
{
    public Vector2 _fromScale;
    public Vector2 _toScale;

    public void UpdateTween(Transform transform, float normalizedTime)
    {
        var lerpTime = _curve.Evaluate(normalizedTime);
        transform.localScale = Vector2.Lerp(_fromScale, _toScale, lerpTime);
    }

    public void CompleteTween(Transform transform)
    {
        transform.localScale = _toScale;
    }
}

[Serializable]
public class TweenAnimSpriteColour : TweenAnimBase
{
    public SpriteRenderer _spriteRenderer;
    public Color _fromColour;
    public Color _toColour;

    public void UpdateTween(float normalizedTime)
    {
        var lerpTime = _curve.Evaluate(normalizedTime);
        var color = Color.Lerp(_fromColour,  _toColour, lerpTime);
        _spriteRenderer.color = color;
    }

    public void CompleteTween()
    {
        _spriteRenderer.color = _toColour;
    }
}

[Serializable] public class TweenAnimImageColour : TweenAnimBase
{
    public Image _image;
    public Color _fromColour;
    public Color _toColour;
    
    public void UpdateTween(float normalizedTime)
    {
        var lerpTime = _curve.Evaluate(normalizedTime);
        var color = Color.Lerp(_fromColour,  _toColour, lerpTime);
        _image.color = color;
    }

    public void CompleteTween()
    {
        _image.color = _toColour;
    }
}
