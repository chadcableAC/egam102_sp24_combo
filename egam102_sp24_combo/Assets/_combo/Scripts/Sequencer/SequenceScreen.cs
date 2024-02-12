using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MicroCombo
{
    public class SequenceScreen : MonoBehaviour   
    {
        // UI 
        [SerializeField] private LivesUi _livesUi = null;

        [SerializeField] private ControlLegendUi _controlUi = null;

        [SerializeField] private RingTextManagerUi _ringTextUi = null;
        [SerializeField] private string _ringTextStart = "Ready...";
        [SerializeField] private string _ringTextPostStart = "Set...";
        [SerializeField] private string _ringTextFirst = "GO!";
        [SerializeField] private string _ringTextWin = "Win!";
        [SerializeField] private string _ringTextLose = "Lose...";
        [SerializeField] private string _ringTextScoreFormat = "Score {0}";
        [SerializeField] private string _ringTextGameOver = "Game Over";

        [SerializeField] private GameUi _gamePrefab = null;
        [SerializeField] private Transform _gameParentHandle = null;
        
        private List<GameUi> _gameUis = new List<GameUi>();
        private int _gameUiIndex = 0;
        private GameUi _gameUi 
        {
            get { return _gameUis[_gameUiIndex]; }
        }
        
        [SerializeField] private TextMeshProUGUI _bylineTextUi = null;
        [SerializeField] private SpringInterper _bylineInterper = null;

        private int _score = 0;

        // Animation
        private Coroutine _mainRoutine = null;
        private Coroutine _altRoutine = null;
        private bool _isLocalRoutine = false;
        public bool isRunning 
        {
            get { return _isLocalRoutine || (_mainRoutine != null); }
        }

        [SerializeField] private float _preGameStartDuration = 1f;
        [SerializeField] private float _gameStartDuration = 1f;
        [SerializeField] private float _postGameStartDuration = 1f;

        [SerializeField] private float _introDuration = 1f;
        [SerializeField] private float _inGameResultDuration = 1f;
        public float inGameResultDuration
        {
            get { return _inGameResultDuration; }
        }

        [SerializeField] private float _resultDuration = 1f;
        [SerializeField] private float _winDuration = 1f;
        [SerializeField] private float _loseLifeDuration = 1f;

        [SerializeField] private AnimationCurve _fillCurve = null;

        // Music
        private MusicManager _musicManager = null;

        void Start()
        {
            _musicManager = FindObjectOfType<MusicManager>();
        }

        public void Init(int lives)
        {
            // Reset the screen to the original state
            _livesUi.Init(lives);

            // Switch to start text
            _ringTextUi.SetText(string.Empty, true);

            // Reset
            _controlUi.SetNone(true);

            if (_bylineInterper != null)
            {
                _bylineInterper.SetGoal(1, true);
            }
        }

        public void PlayGameStart()
        {
            Stop();

            _isLocalRoutine = true;
            _mainRoutine = StartCoroutine(ExecuteGameStart());
            _altRoutine = StartCoroutine(ExecuteCountdown());
        }

        private IEnumerator ExecuteGameStart()
        {
            _livesUi.SetInterp(0, true);
            yield return new WaitForSeconds(_preGameStartDuration);

            // Fill up the lives UI
            float animT = 0;
            while (animT < _gameStartDuration)
            {
                float interp = Mathf.Clamp01(animT / _gameStartDuration);
                float animInterp = UtilsMath.EvaluateCurve(interp, _fillCurve);

                _livesUi.SetInterp(animInterp, false);

                yield return null;
                animT += Time.deltaTime;
            }            

            yield return new WaitForSeconds(_postGameStartDuration);
            
            // All done
            _isLocalRoutine = false;
            _mainRoutine = null;
        }

        private IEnumerator ExecuteCountdown()
        {
            float startTime = _gameStartDuration + _postGameStartDuration;

            yield return new WaitForSeconds(_preGameStartDuration);
            _ringTextUi.SetText(_ringTextStart, false);

            yield return new WaitForSeconds(startTime * 0.5f);
            _ringTextUi.SetText(_ringTextPostStart, false);

            _altRoutine = null;
        }

        public void QueueGame(MicrogameData data)
        {
            Stop();

            if (_gameUis.Count <= 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    GameUi gameUi = GameObject.Instantiate<GameUi>(_gamePrefab);
                    gameUi.transform.SetParent(_gameParentHandle, false);

                    gameUi.SetInterp(-1, true);

                    _gameUis.Add(gameUi);
                }
            }

            _gameUi.SetData(data);
            _gameUi.SetInterp(-1, true);

            if (_bylineTextUi != null)
            {
                string gameName = data.microgameName;
                string studentName = data.studentName;

                _bylineTextUi.text = string.Format("<b>{0}</b><br>by {1}", gameName, studentName);
            }
            
            if (_bylineInterper != null)
            {
                _bylineInterper.SetGoal(1, true);
                _bylineInterper.SetGoal(0);
            }

            // Switch to score text
            if (_score == 0)
            {
                _ringTextUi.SetText(_ringTextFirst, false);
            }
            else
            {
                DisplayScore();
            }

            // Update control types
            _controlUi.SetStyle(data.controlType);

            // Get this new game focused
            _isLocalRoutine = true;
            _mainRoutine = StartCoroutine(ExecuteIntro());
        }

        private IEnumerator ExecuteIntro()
        {
            // Show the results
            _gameUi.SetInterp(0, false);
            yield return new WaitForSeconds(_introDuration);
            
            _musicManager.DuckMusic(true, false);

            // All done
            _isLocalRoutine = false;
            _mainRoutine = null;
        }

        public void PrepResults(bool isWin, int newLivesCount)
        {
            // Update text
            string ringText = _ringTextLose;
            if (isWin)
            {
                ringText = _ringTextWin;
            }
            else if (newLivesCount <= 0)
            {
                ringText = _ringTextGameOver;
            }
            _ringTextUi.SetText(ringText, true);

            if (_bylineInterper != null)
            {
                _bylineInterper.SetGoal(1, true);
            }
        }

        public void SetResults(bool isWin, int newLivesCount, int score)
        {
            Stop();

            _score = score;

            _isLocalRoutine = true;
            _mainRoutine = StartCoroutine(ExecuteResults(isWin, newLivesCount));
        }

        private IEnumerator ExecuteResults(bool isWin, int newLivesCount)
        {
            _musicManager.DuckMusic(false, false);

            // Try to hide the results UI
            EgamMicrogameResultsUi resultsUi = GameObject.FindObjectOfType<EgamMicrogameResultsUi>();
            if (resultsUi != null)
            {
                resultsUi.Reset();
            }

            _controlUi.SetNone(false);

            // Lose a life?
            _gameUi.SetResult(isWin);
            if (!isWin)
            {
                yield return new WaitForSeconds(_loseLifeDuration);
            }
            else
            {
                yield return new WaitForSeconds(_winDuration);
            }

            // Show the results        
            _livesUi.SetLives(newLivesCount);
            _gameUi.SetInterp(1f, false);
            yield return new WaitForSeconds(_resultDuration);

            // Move to the next UI
            _gameUi.transform.SetAsLastSibling();
            _gameUiIndex = (_gameUiIndex + 1) % _gameUis.Count;
            
            // All done
            _isLocalRoutine = false;
            _mainRoutine = null;
        }

        public void Stop()
        {
            if (_mainRoutine != null)
            {
                StopCoroutine(_mainRoutine);
                _mainRoutine = null;
            }

            if (_altRoutine != null)
            {
                StopCoroutine(_altRoutine);
                _altRoutine = null;
            }

            _isLocalRoutine = false;
        }

        private void DisplayScore(bool instant = false)
        {
            string scoreText = string.Format(_ringTextScoreFormat, _score);
            _ringTextUi.SetText(scoreText, instant);
        }
    }
}