using UnityEngine;

namespace MicroCombo
{
    public class PauseScreenUi : MonoBehaviour
    {
        // UI information
        [SerializeField] private GameObject _enableHandle = null;

        [SerializeField] private ComboButtonUi _resumeButton = null;
        [SerializeField] private ComboButtonUi _restartButton = null;
        [SerializeField] private ComboButtonUi _quitButton = null;

        [SerializeField] private ComboButtonUi _pauseButton = null;

        // State / controls
        private bool _isPaused = false;
        [SerializeField] private KeyCode _keycode = KeyCode.Escape;

        // Music
        private MusicManager _musicManager = null;

        private TransitionManager _transitionManager = null;

        void Start()
        {
            // Sub
            _resumeButton.OnButton += _OnResume;
            _restartButton.OnButton += _OnRetry;
            _quitButton.OnButton += _OnQuit;
            _pauseButton.OnButton += _OnPauseButton;

            _musicManager = FindObjectOfType<MusicManager>();
            _transitionManager = FindObjectOfType<TransitionManager>();

            // Turn off by default
            SetPaused(false);
        }

        void Update()
        {
            // Listen for key presses?
            if (!_transitionManager.isRunning &&
                !_transitionManager.isShowing)
            {
                if (Input.GetKeyDown(_keycode))
                {
                    SetPaused(!_isPaused);
                }
            }
        }

        public void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
            
            // Show / hide
            _enableHandle.SetActive(isPaused);
            _pauseButton.gameObject.SetActive(!isPaused);

            _musicManager.SetMusicFilter(isPaused);

            // Change the time
            float timeScale = 1;
            if (isPaused)
            {
                timeScale = 0;
            }
            Time.timeScale = timeScale;
        }

        private void _OnPauseButton()
        {
            // Undo the pause menu
            SetPaused(true);
        }

        private void _OnResume()
        {
            // Undo the pause menu
            SetPaused(false);
        }

        private void _OnRetry()
        {
            // Restart the scene?
            ComboManager comboManager = FindObjectOfType<ComboManager>();
            if (comboManager != null)
            {
                // Restore time
                Time.timeScale = 1f;
                comboManager.GoToGame();
            }
        }

        private void _OnQuit()
        {
            // Move back to the menu
            ComboManager comboManager = FindObjectOfType<ComboManager>();
            if (comboManager != null)
            {
                // Restore time
                Time.timeScale = 1f;
                comboManager.GoToMenu();
            }
        }
    }
}
