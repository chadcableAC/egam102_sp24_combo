using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EgamMicrogameTimerUi : MonoBehaviour
{   
    // UI
    [SerializeField] private GameObject _enableHandle = null;
    [SerializeField] private Slider _timerUi = null;

    // Additional timer feedback
    [SerializeField] private GameObject _countdownEnableHandle = null;
    [SerializeField] private TextMeshProUGUI _countTextUi = null;
    [SerializeField] private float _countDuration = 3f;
    [SerializeField] private MicroCombo.AnimateShake _countShaker = null;
    private int _lastCountdownInt = -1;

    public void Reset()
    {
        _enableHandle.SetActive(false);

        if (_countdownEnableHandle != null)
        {
            _countdownEnableHandle.SetActive(false);
        }
        _lastCountdownInt = -1;
    }

    public void SetInterp(float interp)
    {
        _enableHandle.SetActive(interp < 1);
        _timerUi.SetValueWithoutNotify(interp);
    }

    public void SetTimeRemaining(float timeRemaining)
    {
        if (_countdownEnableHandle != null)
        {
            bool isCountdown = timeRemaining < _countDuration;
            _countdownEnableHandle.SetActive(isCountdown);
            if (isCountdown)
            {
                float interp = Mathf.Clamp01(timeRemaining / _countDuration);                
                int text = Mathf.CeilToInt(Mathf.Lerp(0, 3, interp));

                if (text != _lastCountdownInt)
                {
                    _lastCountdownInt = text;

                    _countTextUi.text = text.ToString();
                    float shakeStrenth = Mathf.Lerp(1f, 0.5f, interp);
                    _countShaker.Shake(shakeStrenth);
                }
            }
        }
    }
}
