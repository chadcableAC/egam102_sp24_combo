using System.Collections;
using UnityEngine;

namespace MicroCombo
{
    public abstract class BaseInterper : MonoBehaviour 
    {
        // Spring information
        [SerializeField]
        protected string _interperName = string.Empty;

        public float startGoal = 0;
        private bool _interpSet = false;

        protected float _goal = 0;
        public float goal
        {
            get { return _goal; }
        }
        
        protected float _value = 0;
        public float value
        {
            get { return _value; }
        }
        
        public bool isSleeping
        {
            get { return _interpRoutine == null; }
        }

        // Callbacks
        public delegate void InterpChangedHandler(float interpValue);
        public event InterpChangedHandler OnInterpUpdated = delegate {};
        
        // Optimizations
        private GameObject _cachedGameObject = null;
        private bool _isCached = false;

        protected Coroutine _interpRoutine = null;
        protected float _lastInterpValue = 0;
        
        public float minimumDelta = 0.01f;

        [SerializeField] private Vector2 _visibilityValues = new Vector2(1f, 0f);
        
        // Use this for initialization
        void Start () 
        {
            // Cache
            if (!_interpSet)
            {
                SetGoal(startGoal, true);
            }
        }

        private void Cache()
        {
            if (!_isCached)
            {
                _isCached = true;
                _cachedGameObject = gameObject;
            }
        }

        public void SetVisible(bool isVisible, bool instant)
        {
            float goal = _visibilityValues.x;
            if (isVisible)
            {
                goal = _visibilityValues.y;
            }
            SetGoal(goal, instant);
        }
        
        public virtual void SetGoal(float goal, bool instant = false)
        {
            _interpSet = true;
            _goal = goal;

            // Disabled?  Sets become instant
            Cache();
            if (!_cachedGameObject.activeInHierarchy)
            {
                instant = true;
            }

            /*
            // Are we too close to the goal?  This becomes instant
            float goalDelta = _springGoal - lastSpringValue;
            if (Mathf.Abs(goalDelta) <= minimumDelta &&
                Mathf.Abs(_springVelocity) <= minimumDelta)
            {
                instant = true;
            }
            */
        
            if (instant)
            {
                StopInterper();
            }   
            else
            {
                UpdateGoal();
            } 
        }

        protected virtual void StopInterper()
        {
            // Stop any existing routines
            if (_interpRoutine != null)
            {
                StopCoroutine(_interpRoutine);
                _interpRoutine = null;
            }    

            // Immediately go to the goal
            _value = _goal;
            UpdatePosition(true);
        }

        protected abstract void UpdateGoal();
        protected abstract IEnumerator ExecuteInterp();

        public virtual void Nudge(float strength)
        {
            
        }

        public void OffsetGoal(float delta, bool instant = false)
        {
            SetGoal(_goal + delta, instant);
        }
        
        protected bool UpdatePosition(bool force = false)
        {
            bool springAdjusted = false;
            
            bool doUpdate = force;
            if (!doUpdate)
            {
                // Make sure this is far away enough
                float delta = _value - _lastInterpValue;
                doUpdate = Mathf.Abs(delta) > minimumDelta;
            }
            
            if (doUpdate)
            {
                springAdjusted = true;
                _lastInterpValue = _value;
                
                OnInterpUpdated(_value);
            }
            
            return springAdjusted;
        }

        void OnDisable()
        {
            // Survive the disable by stopping the interper (jumping to the end)
            StopInterper();
        }
    }
}