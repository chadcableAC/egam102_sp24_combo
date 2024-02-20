using UnityEngine;

namespace MicroCombo
{
    public class InterpScaleUi : MonoBehaviour
    {
        // Cache
        public RectTransform animHandle = null;
        public bool useScale = false;        

        private Vector2 _originalSize = Vector2.zero;
        private Vector3 _originalScale = Vector3.one;
        private bool _isCached = false;

        // Animation
        public Vector2 scaleShow = new Vector2(1, 1);
        public Vector2 scaleHide = new Vector2(0, 0);
        public bool isUnclamped = false;

        public AnimationCurve animCurve = null;

        // Spring information
        public BaseInterper interper = null;

        public bool isVisible
        {
            get { return interper.value < 0.5f; }
        }

        // Use this for initialization
        void Start()
        {
            _Cache();
        }

        private void _Cache()
        {
            if (!_isCached)
            {
                _isCached = true;

                // Cache
                _originalSize = animHandle.sizeDelta;
                _originalScale = animHandle.localScale;

                // Subscribe
                interper.OnInterpUpdated += _OnSpringUpdated;
                _OnSpringUpdated(interper.value);
            }
        }

        private void _OnSpringUpdated(float val)
        {
            float adjInterp = UtilsMath.EvaluateCurve(val, animCurve);

            Vector2 scale = Vector2.Lerp(scaleShow, scaleHide, adjInterp);
            if (isUnclamped)
            {
                scale = Vector2.LerpUnclamped(scaleShow, scaleHide, adjInterp);
            }

            if (useScale)
            {
                Vector3 adjScale = scale;
                adjScale.z = 1f;

                animHandle.localScale = Vector3.Scale(_originalScale, adjScale);
            }
            else
            {
                animHandle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scale.x * _originalSize.x);
                animHandle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scale.y * _originalSize.y);
            }
        }

        public void SetVisible(bool isVisible, bool instant = false)
        {
            _Cache();

            int springGoal = 0;
            if (!isVisible)
            {
                springGoal = 1;
            }
            interper.SetGoal(springGoal, instant);
        }

        public void Nudge(float strength)
        {
            SpringInterper sInterp = interper as SpringInterper;
            if (sInterp != null)
            {
                sInterp.Nudge(strength);
            }
        }
    }
}