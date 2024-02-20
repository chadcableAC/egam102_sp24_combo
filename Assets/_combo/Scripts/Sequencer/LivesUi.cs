using TMPro;
using UnityEngine;

namespace MicroCombo
{
    public class LivesUi : MonoBehaviour
    {
        // UI information
        [SerializeField] private TextMeshProUGUI _liveText = null;
        private int _maxLives = 4;

        [SerializeField] private SpringInterper _livesSpring = null;

        public void Init(int max)
        {
            _livesSpring.SetGoal(0f, true);

            _maxLives = max;
            SetLives(_maxLives);
        }

        public void SetLives(int count)
        {
            _liveText.text = string.Format("Lives: {0}", count);

            float interp = Mathf.Clamp01(count / (_maxLives * 1f));
            _livesSpring.SetGoal(interp, false);
        }

        public void SetInterp(float interp, bool instant)
        {
            _livesSpring.SetGoal(interp, instant);
        }
    }
}
