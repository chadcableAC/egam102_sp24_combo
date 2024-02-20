using UnityEngine;
using System.Collections;

namespace MicroCombo
{
    public class AnimateShake : MonoBehaviour
    {
        // Transform
        public Transform animHandle = null;
        private Vector3 _originalLocalPosition = Vector3.zero;
        private Quaternion _originalLocalRotation = Quaternion.identity;


        // Animation information
        public bool loopShake = false;

        private float _shakeT = -1f;
        public float shakeDuration = 1f;
        public int shakesPerDuration = 5;

        private float _shakeFactor = 1f;
        public float shakeStrength = 1f;
        public Vector3 shakeDirection = Vector2.one;

        public AnimationCurve strengthMaskCurve = null;

        public float rotationStrength = 10f;
        public Vector3 rotationDirection = Vector3.forward;

        private Coroutine _activeRoutine = null;
        public bool isAnimating
        {
            get { return _activeRoutine != null; }
        }

        public void Shake(float factor = 1f)
        {
            _shakeFactor = factor;

            // Kickoff routine?
            if (_activeRoutine == null)
            {
                _activeRoutine = StartCoroutine(ShakeAnimated());
            }
            else
            {
                _shakeT = 0;
            }
        }

        public void StopShake()
        {
            if (_activeRoutine != null)
            {
                StopCoroutine(_activeRoutine);
                _activeRoutine = null;

                // Restore positioning
                RestoreTransform();
            }
        }

        private IEnumerator ShakeAnimated()
        {
            // Setup
            _originalLocalPosition = animHandle.localPosition;
            _originalLocalRotation = animHandle.localRotation;

            // Animate
            float shakeFrequency = 1f / shakesPerDuration;

            _shakeT = 0;
            float lastInterp = 0;

            while (_shakeT < shakeDuration)
            {
                float interp = Mathf.Clamp01(_shakeT / shakeDuration);

                // Are we allowed to shake?                        
                int shakeIndex = Mathf.CeilToInt(lastInterp / shakeFrequency);
                float shakeThreshold = shakeIndex * shakeFrequency;
                if (lastInterp < shakeThreshold &&
                    interp >= shakeThreshold)
                {
                    // Pick a random offset
                    Vector3 randomOffset = Vector3.Scale(UtilsMath.RandomPlusMinusVector3().normalized, shakeDirection);
                    randomOffset *= Random.Range(-shakeStrength, shakeStrength) * _shakeFactor;

                    float mask = 1;
                    if (strengthMaskCurve != null &&
                        strengthMaskCurve.length > 0)
                    {
                        mask = UtilsMath.EvaluateCurve(interp, strengthMaskCurve);
                    }

                    randomOffset *= mask;

                    animHandle.localPosition = _originalLocalPosition + randomOffset;


                    // Randomly rotate too
                    if (rotationStrength != 0)
                    {
                        float degrees = Random.Range(-rotationStrength, rotationStrength) * _shakeFactor;
                        degrees *= mask;

                        Quaternion rotateOffset = Quaternion.Euler(rotationDirection * degrees);
                        animHandle.localRotation = _originalLocalRotation * rotateOffset;
                    }
                }

                lastInterp = interp;

                yield return null;
                float adjDeltaTime = Time.deltaTime;
                _shakeT += adjDeltaTime;

                while (loopShake &&
                       _shakeT >= shakeDuration)
                {
                    _shakeT -= shakeDuration;
                }
            }

            RestoreTransform();

            // Complete
            _activeRoutine = null;
        }

        private void RestoreTransform()
        {
            animHandle.localPosition = _originalLocalPosition;
            animHandle.localRotation = _originalLocalRotation;
        }
    }
}