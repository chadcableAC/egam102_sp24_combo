using TMPro;
using UnityEngine;

namespace MicroCombo
{
    public class ResultsScreenUi : MonoBehaviour
    {
        // UI information
        [SerializeField] private GameObject _enableHandle = null;

        [SerializeField] private ComboButtonUi _restartButton = null;
        [SerializeField] private ComboButtonUi _quitButton = null;

        // Scoring
        [SerializeField] private TextMeshProUGUI _roundScore = null;
        [SerializeField] private GameObject _newHighScoreEnableHandle = null;

        [SerializeField] private TextMeshProUGUI[] _highScoreTextUis = null;

        // Music
        private MusicManager _musicManager = null;

        void Start()
        {
            // Sub
            _restartButton.OnButton += _OnRetry;
            _quitButton.OnButton += _OnQuit;

            _musicManager = FindObjectOfType<MusicManager>();

            // Turn off by default
            SetVisible(false);
        }

        private void SetVisible(bool isVisible)
        {
            // Show / hide
            _enableHandle.SetActive(isVisible);
            _musicManager.SetMusicFilter(isVisible);
        }

        public void Show(int score, bool isHighScore)
        {
            // Get the latest scores
            _roundScore.text = score.ToString();
            _newHighScoreEnableHandle.SetActive(isHighScore);

            for (int i = 0; i < _highScoreTextUis.Length; i++)
            {
                int highScore = ComboManager.GetHighScore(i);
                _highScoreTextUis[i].text = highScore.ToString();
            }

            _musicManager.Play(MusicManager.SfxType.Results);

            // Show the screen
            SetVisible(true);
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
