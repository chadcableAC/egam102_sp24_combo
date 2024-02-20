using UnityEngine;

namespace MicroCombo
{
    public class InterpTranslate : MonoBehaviour
    {
        // Cache
        public Transform animHandle = null;
        private Vector3 _originalLocalPosition = Vector3.zero;
        private bool _isCached = false;

        // Animation
        public Vector3 direction = Vector3.up;
        public float strength = 1f;

        // Spring information
        public BaseInterper interper = null;

        public bool isVisible
        {
            get { return interper.value < 0.5f; }
        }

        public bool isVisibleStrict
        {
            get { return interper.value < 0.1f; }
        }

        public bool isHidden
        {
            get { return interper.value >= 0.9f; }
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
                _originalLocalPosition = animHandle.localPosition;

                // Subscribe
                interper.OnInterpUpdated += _OnSpringUpdated;
                _OnSpringUpdated(interper.value);
            }
        }

        private void _OnSpringUpdated(float val)
        {
            Vector3 offset = val * direction * strength;
            if (animHandle != null)
            {
                animHandle.localPosition = _originalLocalPosition + offset;
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