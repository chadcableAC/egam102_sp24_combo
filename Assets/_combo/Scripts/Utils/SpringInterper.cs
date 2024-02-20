using UnityEngine;
using System.Collections;

namespace MicroCombo
{
    public class SpringInterper : BaseInterper 
    {
        // Spring information
        private float _springVelocity = 0;
        public float springVelocity
        {
            get { return _springVelocity; }
        }
        
        public float springAngularFrequency = 5f;
        public float springDampingRatio = 1f;
        
        public float maximumVelocity = -1f;

        // Optimizations
        public float fallAsleepDuration = 1f;  
        
        protected override void StopInterper()
        {
            base.StopInterper();

            // Also stop the velocity
            _springVelocity = 0;
        }

        protected override void UpdateGoal()
        {
            // Only start if stopped
            if (_interpRoutine == null)
            {
                _interpRoutine = StartCoroutine(ExecuteInterp());  
            }          		
        }
        
        protected override IEnumerator ExecuteInterp()
        {
            float sleepT = 0;
            while (sleepT < fallAsleepDuration)
            {
                yield return null;

                float adjDeltaTime = Time.deltaTime;
                
                if (maximumVelocity > 0)
                {
                    _springVelocity = Mathf.Clamp(_springVelocity, -maximumVelocity, maximumVelocity);
                }
                
                UtilsMath.CalcDampedSimpleHarmonicMotion(ref _value, ref _springVelocity, _goal, 
                    adjDeltaTime, springAngularFrequency, springDampingRatio);
                
                // Position changed?  Reset the timer
                if (UpdatePosition())
                {
                    sleepT = 0;
                }
                else
                {
                    sleepT += adjDeltaTime;
                }
            }

            // All done, set to the final goal
            StopInterper();
        }
            
        public override void Nudge(float strength)
        {
            _springVelocity += strength;
            
            // Wake up the spring
            SetGoal(_goal);
        }
    }
}