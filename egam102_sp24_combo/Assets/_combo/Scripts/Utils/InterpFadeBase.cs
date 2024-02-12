using UnityEngine;

namespace MicroCombo
{
    public abstract class InterpFadeBase : MonoBehaviour
    {
        // Cache
        private bool _isCached = false;

        // Animation
        public Color colorVisible = Color.white;
        public Color colorInvisible = Color.clear;
        public AnimationCurve fadeCurve = null;

        // Spring information
        public BaseInterper interper = null;


        // Use this for initialization
        void Awake()
        {
            _Cache();
        }

        private void _Cache()
        {
            if (!_isCached)
            {
                _isCached = true;
                _OnCache();
            }
        }

        protected virtual void _OnCache()
        {
            // Subscribe
            interper.OnInterpUpdated += _OnSpringUpdated;
            _OnSpringUpdated(interper.value);
        }

        private void _OnSpringUpdated(float val)
        {
            float adjVal = UtilsMath.EvaluateCurve(Mathf.Abs(val), fadeCurve);
            Color color = Color.Lerp(colorVisible, colorInvisible, adjVal);
            _ApplyColor(color);
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

        protected abstract void _ApplyColor(Color color);
    }
}