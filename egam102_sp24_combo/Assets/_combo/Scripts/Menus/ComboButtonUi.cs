using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MicroCombo
{
    public class ComboButtonUi : MonoBehaviour
    {
        // Button / callbacks
        [SerializeField] private Button _button = null;
        public Button button 
        {
            get { return _button; }
        }

        [SerializeField] private TextMeshProUGUI _text = null;

        public delegate void ButtonPressedHandler();
        public event ButtonPressedHandler OnButton = delegate {};

        // Music
        private MusicManager _musicManager = null;

        void Start()
        {
            // Sub
            _button.onClick.AddListener(_OnPressed);

            _musicManager = FindObjectOfType<MusicManager>();
        }
        
        private void _OnPressed()
        {
            _musicManager.Play(MusicManager.SfxType.Button);
            OnButton();
        }

        public void SetText(string text)
        {
            if (_text != null)
            {
                _text.text = text;
            }
        }
    }
}