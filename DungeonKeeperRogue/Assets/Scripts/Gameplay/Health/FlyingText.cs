using TMPro;
using UnityEngine;

namespace DungeonKeeperRogue.Gameplay
{
    public class FlyingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        [Header("Settings")]
        [SerializeField] private float _duration;
        [SerializeField] private float _topVerticalSpeed;
        [SerializeField] private float _animationStartTime;
        [SerializeField] private AnimationCurve _verticalSpeedCurve;
        [SerializeField] private AnimationCurve _fadeCurve;

        private float _lifetime;
        private float _animationLifetime;

        private float ExitNormalizedTime => _animationLifetime / (_duration - _animationStartTime);

        public void Init(string text, float fontSizeMultiplier = 1.0f)
        {
            _text.SetText(text);
            _text.fontSize *= fontSizeMultiplier;
        }

        private void Update()
        {
            _lifetime += Time.deltaTime;

            if (_lifetime < _animationStartTime)
            {
                return;
            }

            _animationLifetime += Time.deltaTime;
            float delta = _verticalSpeedCurve.Evaluate(ExitNormalizedTime) * _topVerticalSpeed * Time.deltaTime;
            transform.Translate(0f, delta, 0f);
            _text.alpha = 1 - _fadeCurve.Evaluate(ExitNormalizedTime);

            if (_lifetime < _duration)
            {
                return;
            }
        
            Destroy(gameObject);
        }
    }
}