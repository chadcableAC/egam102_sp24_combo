using TMPro;
using UnityEngine;

namespace MicroCombo
{
    public class RingLetterUi : MonoBehaviour
    {
        // Text element
        [SerializeField] private TextMeshProUGUI _textUi = null;
        [SerializeField] private AnimationCurve _fadeCurve = null;

        // Animation
        [SerializeField] private float _fadeDuration = 1f;
        private float _fadeT = 0;

        private bool _isFadeOut = false;

        public void SetString(string text, bool instant)
        {
            _isFadeOut = false;
            _textUi.text = text;

            if (instant)
            {
                SetInterp(1f);
            }
            else
            {
                _fadeT = _fadeDuration * -0.5f;
                SetInterp(0f);
            }
        }

        public void Hide(bool instant)
        {
            _isFadeOut = true;
            if (instant)
            {
                SetInterp(0f);
            }
            else
            {
                _fadeT = 0;
                SetInterp(1f);
            }
        }

        void Update()
        {
            if (_fadeT < _fadeDuration)
            {
                _fadeT += Time.deltaTime;

                float interp = Mathf.Clamp01(_fadeT / _fadeDuration);
                if (_isFadeOut)
                {
                    interp = 1f - interp;
                }

                SetInterp(interp);
            }
        }

        private void SetInterp(float interp)
        {
            float animInterp = UtilsMath.EvaluateCurve(interp, _fadeCurve);
            _textUi.alpha = Mathf.Lerp(0, 1f, animInterp);            
        }
    }
}