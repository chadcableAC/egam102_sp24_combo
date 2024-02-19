using System.Collections;
using TMPro;
using UnityEngine;

public class EgamMicrogameInstance : MonoBehaviour
{
    // UI information
    [SerializeField] private EgamMicrogameCommandUi _commandUi = null;
    [SerializeField] private EgamMicrogameTimerUi _timerUi = null;
    [SerializeField] private EgamMicrogameResultsUi _resultsUi = null;

    [SerializeField] private TextMeshProUGUI _versionTextUi = null;
    private readonly string VersionNumber = "1.1";

    private readonly float AdditionalDuration = 0.5f;

    // Timing information / state
    private EgamMicrogameHelper.WinLose _timeoutType = EgamMicrogameHelper.WinLose.Lose;

    public bool _isGameOver = false;
    public bool isGameOver
    {
        get { return _isGameOver; }        
    }

    private EgamMicrogameHelper.WinLose _result = EgamMicrogameHelper.WinLose.Lose;
    public EgamMicrogameHelper.WinLose result 
    {
        get { return _result; }
    }
    
    private float _duration = 10f;
    private float _gameTimer = 0f;
    public float timeInterp
    {
        get 
        {
            float interp = 0;
            if (_duration > 0)
            {
                interp = 1f - Mathf.Clamp01(_gameTimer / _duration);
            }
            return interp;
        }
    }

    private Coroutine _gameRoutine = null;
    private bool _isLocalRoutine = false;
    private bool _isRunning 
    {
        get { return _isLocalRoutine || (_gameRoutine != null); }
    }

    void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        // Turn everything off
        _commandUi.Setup(string.Empty);
        _timerUi.Reset();
        _resultsUi.Reset();        
    }
    
    public void StartGame(string command, float duration, EgamMicrogameHelper.WinLose timeout)
    {
        if (_versionTextUi != null)
        {   
            _versionTextUi.text = string.Format("Version {0}", VersionNumber);
        }        
        
        // Cache values
        _commandUi.Setup(command);
        _duration = duration;
        _timeoutType = timeout;

        // Restart the sequence
        _isGameOver = false;
        _gameTimer = -AdditionalDuration;
        _resultsUi.Reset();

        // Kickoff the routine
        if (_gameRoutine != null)
        {
            StopCoroutine(_gameRoutine);
            _gameRoutine = null;
        }

        _isLocalRoutine = true;
        _gameRoutine = StartCoroutine(ExecuteGame());
    }

    private IEnumerator ExecuteGame()
    {
        // Count up, wait for the game to end
        while (_gameTimer <= _duration)
        {
            // Update timer
            _timerUi.SetInterp(timeInterp);
            
            float timeRemaining = _duration - _gameTimer;
            _timerUi.SetTimeRemaining(timeRemaining);

            // Wait for the next frame
            yield return null;
            _gameTimer += Time.deltaTime;
        }
        
        _timerUi.SetInterp(timeInterp);
        _timerUi.SetTimeRemaining(0);

        // Out of time?  End the game
        switch (_timeoutType)
        {
            case EgamMicrogameHelper.WinLose.Win:
                WinGame();
                break;

            case EgamMicrogameHelper.WinLose.Lose:
                LoseGame();
                break;
        }
        
        _isLocalRoutine = false;
        _gameRoutine = null;
    }

    public void WinGame()
    {
        EndGame(EgamMicrogameHelper.WinLose.Win);
    }

    public void LoseGame()
    {
        EndGame(EgamMicrogameHelper.WinLose.Lose);
    }

    private void EndGame(EgamMicrogameHelper.WinLose type)
    {
        // Only if the game is active / running
        if (!_isGameOver)
        {
            _isGameOver = true;
            _result = type;

            // End the main routine?
            if (_gameRoutine != null)
            {
                StopCoroutine(_gameRoutine);
                _gameRoutine = null;
            }
            _isLocalRoutine = false;

            // Kickoff the results
            _resultsUi.SetResult(type);
            _timerUi.Reset();
        }
    }
}
