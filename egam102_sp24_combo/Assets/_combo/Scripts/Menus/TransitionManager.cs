using System.Collections;
using UnityEngine;

namespace MicroCombo
{
    public class TransitionManager : MonoBehaviour
    {
        // Visuals
        private Coroutine _mainRoutine = null;
        private bool _isLocalRoutine = false;
        public bool isRunning
        {
            get { return _isLocalRoutine || (_mainRoutine != null); }
        }

        private bool _isShowing = false;
        public bool isShowing
        {
            get { return _isShowing; }
        }

        [SerializeField] private float _showDuration = 1f;
        [SerializeField] private float _hideDuration = 0.5f;

        [SerializeField] private RectTransform _animHandle = null;
        [SerializeField] private AnimationCurve _animCurve = null;

        [SerializeField] private CanvasGroup _transitionGroup = null;
        
        // Music
        private MusicManager _musicManager = null;
        
        void Awake()
        {
            _musicManager = FindObjectOfType<MusicManager>();
            EgamInput.IsInputEnabled = true;

            Hide(true);
        }

        public void Show()
        {
            Transition(true);
        }

        public void Hide(bool instant)
        {
            Transition(false, instant);
        }

        private void Transition(bool isShow, bool instant = false)
        {
            // Stop any existing
            if (_mainRoutine != null)
            {
                StopCoroutine(_mainRoutine);
                _mainRoutine = null;
            }

            // Instant = set final states
            if (instant)
            {
                SetInterp(-2);
            }
            else
            {
                _isLocalRoutine = true;
                _mainRoutine = StartCoroutine(ExecuteTransition(isShow));
            }
        }

        private IEnumerator ExecuteTransition(bool isShow)
        {
            float fromInterp = 0;
            float toInterp = 0.5f;
            if (!isShow)
            {
                fromInterp = 0.5f;
                toInterp = 1;
            }

            // Start of transition
            SetInterp(fromInterp);

            float duration = isShow ? _showDuration : _hideDuration;

            // Play the transition
            float animT = 0;
            while (animT < duration)
            {
                float interp = Mathf.Clamp01(animT / duration);
                float adjInterp = Mathf.Lerp(fromInterp, toInterp, interp);
                SetInterp(adjInterp);

                yield return null;
                animT += Time.deltaTime;
            }

            // Done / end
            SetInterp(toInterp);

            _isLocalRoutine = false;
            _mainRoutine = null;
        }

        private void SetInterp(float interp)
        {
            // Cover information
            float animInterp = UtilsMath.EvaluateCurve(interp, _animCurve);

            Vector2 min = Vector2.zero;
            Vector2 max = Vector2.one;
            
            float buffer = 0.25f;
            min.x = Mathf.Lerp(-1f - buffer, 1f + buffer, animInterp);
            max.x = Mathf.Lerp(-buffer, 2f + buffer, animInterp);

            _animHandle.anchorMin = min;
            _animHandle.anchorMax = max;

            // Transition
            float transitionAlpha = 1f;//Mathf.Lerp(0, 1, interp);
            if (interp <= 0 ||
                interp >= 1)
            {
                transitionAlpha = 0;
            }

            _transitionGroup.alpha = transitionAlpha;
            _transitionGroup.gameObject.SetActive(transitionAlpha > 0);
            
            _isShowing = transitionAlpha > 0;

            float musicInterp = Mathf.Clamp01(1 - Mathf.Abs(interp));            
            _musicManager.OnTransitionInterp(musicInterp);

            // Input enabled or not?
            bool hasInput = !_isShowing;
            EgamInput.IsInputEnabled = hasInput;
        }

        public void Stop()
        {
            if (_mainRoutine != null)
            {
                StopCoroutine(_mainRoutine);
                _mainRoutine = null;
            }

            _isLocalRoutine = false;
        }
    }
}
