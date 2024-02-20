using TMPro;
using UnityEngine;

namespace MicroCombo
{
    public class MenuSettings : MenuBase
    {
        // UI information
        [SerializeField] private ComboButtonUi _audioButton = null;
        [SerializeField] private ComboButtonUi _resetButton = null;
        [SerializeField] private ComboButtonUi _quitButton = null;

        [SerializeField] private TextMeshProUGUI _versionText = null;
        [SerializeField] private string _versionFormat = "EGAM 102 SP 23<br>Version {0}";

        private MusicManager _musicManager = null;

        void Start()
        {
            // Sub
            _audioButton.OnButton += OnAudio;
            _resetButton.OnButton += OnReset;
            _quitButton.OnButton += OnQuit;

            // Make sure audio is up to date            
            _musicManager = FindObjectOfType<MusicManager>();
            SetMuted(_musicManager.isMuted);

            // Platform specific
            bool isQuitEnabled = false;
#if UNITY_EDITOR || UNITY_STANDALONE
            isQuitEnabled = true;
#endif
            _quitButton.gameObject.SetActive(isQuitEnabled);

            // Version text
            string versionText = string.Format(_versionFormat, Application.version);
            _versionText.text = versionText;
        }   

        private void OnAudio()
        {
            // Toggle the setting
            SetMuted(!_musicManager.isMuted);
        }

        private void SetMuted(bool isMuted)
        {
            // Update the mixer?
            _musicManager.SetMuted(isMuted, true);
            
            // Update the text
            string text = string.Format("Audio is {0}", !_musicManager.isMuted ? "On" : "Off");
            _audioButton.SetText(text);
        }

        private void OnReset()
        {
            // Reset the scores
            ComboManager.ResetScores();            
        }

        private void OnQuit()
        {
            // Exit the app
            Application.Quit();            
        }
    }
}