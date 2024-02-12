using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MicroCombo
{
    public class ControlUi : MonoBehaviour
    {
        // UI
        [SerializeField] private Image _iconUi = null;
        [SerializeField] private TextMeshProUGUI _textUi = null;
        
        [SerializeField] private Sprite _keyboardSprite = null;
        [SerializeField] private string _keyboardDescription = null;

        [SerializeField] private Sprite _keyboardPlusSprite = null;
        [SerializeField] private string _keyboardPlusDescription = null;

        [SerializeField] private Sprite _mouseSprite = null;
        [SerializeField] private string _mouseDescription = null;

        // Animation
        [SerializeField] private SpringInterper _enabledInterper = null;
                        
        public void SetStyle(MicrogameData.ControlTypes controlType)
        {
            Sprite sprite = null;
            string description = string.Empty;

            switch (controlType)
            {
                case MicrogameData.ControlTypes.Keyboard:
                    sprite = _keyboardSprite;
                    description = _keyboardDescription;
                    break;

                case MicrogameData.ControlTypes.KeyboardPlus:
                    sprite = _keyboardPlusSprite;
                    description = _keyboardPlusDescription;
                    break;

                case MicrogameData.ControlTypes.Mouse:
                    sprite = _mouseSprite;
                    description = _mouseDescription;
                    break;
            }

            _iconUi.sprite = sprite;
            if (_textUi != null)
            {
                _textUi.text = description;
            }
        }

        public void SetEnabled(bool isEnabled, bool instant)
        {
            float goal = 0;
            if (!isEnabled)
            {
                goal = 1;
            }
            _enabledInterper.SetGoal(goal, instant);
        }
    }
}