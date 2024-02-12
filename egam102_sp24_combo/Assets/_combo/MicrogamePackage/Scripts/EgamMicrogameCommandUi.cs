using System.Collections;
using TMPro;
using UnityEngine;

public class EgamMicrogameCommandUi : MonoBehaviour
{   
    // Animation
    [SerializeField] private GameObject _enableHandle = null;
    [SerializeField] private TextMeshProUGUI _textUi = null;
    [SerializeField] private float _duration = 1f;
    private Coroutine _showRoutine = null;

    private void Reset()
    {
        // Undo any values
        if (_showRoutine != null)
        {
            StopCoroutine(_showRoutine);
            _showRoutine = null;
        }

        _enableHandle.SetActive(false);
    }

    public void Setup(string command)
    {
        Reset();

        if (!string.IsNullOrEmpty(command))
        {
            _textUi.text = command;
            _showRoutine = StartCoroutine(ExecuteShow());
        }
    }

    private IEnumerator ExecuteShow()
    {
        // Show
        _enableHandle.SetActive(true);

        yield return new WaitForSeconds(_duration);
        
        // Hide
        _enableHandle.SetActive(false);
        _showRoutine = null;
    }
}
