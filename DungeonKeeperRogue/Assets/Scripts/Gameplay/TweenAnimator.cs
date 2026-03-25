using UnityEngine;
using UnityEngine.Events;

public class TweenAnimator : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private bool _playOnStart;
    [SerializeField] private bool _playOnEnable;
    [SerializeField] private bool _doLoop;
    [Tooltip("-1 = infinite")]
    [SerializeField] private int _loops = -1;

    [Header("Tween anims")]
    [SerializeField] private TweenAnimMove _moveTween;
    [SerializeField] private TweenAnimRotate _rotateTween;
    [SerializeField] private TweenAnimScale _scaleTween;
    [SerializeField] private TweenAnimSpriteColour _spriteColourTween;
    [SerializeField] private TweenAnimImageColour _imageColourTween;

    [Header("Events")]
    [SerializeField] private UnityEvent _onCompleteEvent;

    private bool _isPlaying;
    private int _currentLoops;
    private float _timer;

    private void Start()
    {
        if (_playOnStart)
        {
            PlayFromStart();
        }
    }

    private void OnEnable()
    {
        if (_playOnEnable)
        {
            PlayFromStart();
        }
    }

    private void Update()
    {
        if (_isPlaying == false)
        {
            return;
        }
        
        _timer += Time.deltaTime;

        if (_timer >= _duration && (_doLoop == false || (_loops > 0 && _currentLoops >= _loops)))
        {
            CompleteTweens();
            return;
        }

        if (_timer >= _duration)
        {
            Loop();
            return;
        }
        
        UpdateTweens(_timer / _duration);
    }
    
    private void Loop()
    {
        ++_currentLoops;
        _timer = 0;
        UpdateTweens(0f);
    }

    #region Update tweens

    private void UpdateTweens(float normalizedTime)
    {
        if (_moveTween._useTween)
        {
            _moveTween.UpdateTween(transform, normalizedTime);
        }

        if (_rotateTween._useTween)
        {
            _rotateTween.UpdateTween(transform, normalizedTime);
        }
        
        if (_scaleTween._useTween)
        {
            _scaleTween.UpdateTween(transform, normalizedTime);
        }

        if (_spriteColourTween._useTween)
        {
            _spriteColourTween.UpdateTween(normalizedTime);
        }

        if (_imageColourTween._useTween)
        {
            _imageColourTween.UpdateTween(normalizedTime);
        }
    }

    #endregion

    #region Complete tweens

    private void CompleteTweens()
    {
        _isPlaying = false;

        if (_moveTween._useTween)
        {
            _moveTween.CompleteTween(transform);
        }

        if (_rotateTween._useTween)
        {
            _rotateTween.CompleteTween(transform);
        }
        
        if (_scaleTween._useTween)
        {
            _scaleTween.CompleteTween(transform);
        }

        if (_spriteColourTween._useTween)
        {
            _spriteColourTween.CompleteTween();
        }

        if (_imageColourTween._useTween)
        {
            _imageColourTween.CompleteTween();
        }
        
        _onCompleteEvent?.Invoke();
    }

    #endregion

    #region Public methods

    public void PlayFromStart()
    {
        _isPlaying = true;
        _timer = 0f;
        _currentLoops = 0;
        
        UpdateTweens(0f);
    }

    public void ResetAnims()
    {
        _isPlaying = false;
        _timer = 0f;
        _currentLoops = 0;
        
        UpdateTweens(0f);
    }

    #endregion
}
