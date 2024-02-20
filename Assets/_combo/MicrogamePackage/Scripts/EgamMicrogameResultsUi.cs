using MicroCombo;
using UnityEngine;

public class EgamMicrogameResultsUi : MonoBehaviour
{   
    // Handles
    [SerializeField] private GameObject _winEnableHandle = null;
    [SerializeField] private GameObject _loseEnableHandle = null;

    [SerializeField] private MicroCombo.AnimateShake _loseShaker = null;

    // Music
    private MusicManager _musicManager = null;

    void Start()
    {
        _musicManager = FindObjectOfType<MusicManager>();
    }

    public void Reset()
    {
        // Turn everything off
        _winEnableHandle.SetActive(false);
        _loseEnableHandle.SetActive(false);
    }

    public void SetResult(EgamMicrogameHelper.WinLose type)
    {
        switch (type)
        {
            case EgamMicrogameHelper.WinLose.Win:
                _winEnableHandle.SetActive(true);
                if (_musicManager != null)
                {
                    _musicManager.Play(MusicManager.SfxType.SuccessSmall);
                }
                break;

            case EgamMicrogameHelper.WinLose.Lose:
                _loseEnableHandle.SetActive(true);
                if (_loseShaker != null)
                {
                    _loseShaker.Shake();
                }
                if (_musicManager != null)
                {
                    _musicManager.Play(MusicManager.SfxType.LoseSmall);
                }
                break;
        }
    }
}
