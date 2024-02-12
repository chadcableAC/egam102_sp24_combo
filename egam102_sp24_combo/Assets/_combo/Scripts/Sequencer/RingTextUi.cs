using System.Collections.Generic;
using UnityEngine;

namespace MicroCombo
{
    public class RingTextUi : MonoBehaviour
    {
        // Letetrs
        [SerializeField] private RingLetterUi _letterReference = null;
        [SerializeField] private Vector2Int _letterCountRange = new Vector2Int(24, 32);
        private List<RingLetterUi> _letters = new List<RingLetterUi>();

        // ANimation
        [SerializeField] private Transform _rotateHandle = null;
        [SerializeField] private float _rotateSpeed = 1f;
        
        void Awake()
        {
            Cache();
        }

        void Update()
        {
            _rotateHandle.localRotation *= Quaternion.Euler(Vector3.back * _rotateSpeed * Time.deltaTime);
        }

        private void Cache()
        {
            if (_letters.Count <= 0)
            {
                // Build the letters
                _letters.Add(_letterReference);

                int maxLetters = _letterCountRange.y;
                for (int i = 1; i < maxLetters; i++)
                {
                    RingLetterUi letter = GameObject.Instantiate<RingLetterUi>(_letterReference);
                    letter.transform.SetParent(_letterReference.transform.parent, false);
                    _letters.Add(letter);
                }

                for (int i = 0; i < _letters.Count; i++)
                {
                    _letters[i].gameObject.SetActive(false);
                }

                // Random start rotation
                _rotateHandle.localRotation *= Quaternion.Euler(Vector3.back * Random.Range(0, 360f));
            }
        }

        public void SetText(string text, bool instant = false)
        {
            Cache();
            
            bool isHide = string.IsNullOrEmpty(text);
            if (isHide)
            {
                Hide(instant);
            }
            else
            {
                text = (text + " ").ToUpper();

                // Pick the best number of letters
                int textLength = text.Length;
                int lettersToShow = 0;
                for (int i = _letterCountRange.y; i >= _letterCountRange.x; i--)
                {
                    if (i % textLength == 0)
                    {
                        lettersToShow = i;
                        break;
                    }
                }

                // Show this number of letters, and lay them out
                for (int i = 0; i < _letters.Count; i++)
                {
                    bool isEnabled = i < lettersToShow;
                    _letters[i].gameObject.SetActive(isEnabled);
                    
                    if (isEnabled)
                    {
                        float interp = Mathf.Clamp01(i / (lettersToShow * 1f));
                        float angle = Mathf.Lerp(0, 360f, interp);
                        _letters[i].transform.localRotation = Quaternion.Euler(Vector3.back * angle);

                        string letter = text.Substring(i % textLength, 1);
                        _letters[i].SetString(letter, instant);
                    }
                }
            }
        }

        void Hide(bool instant)
        {
            // Just trun off letters
            for (int i = 0; i < _letters.Count; i++)
            {
                _letters[i].Hide(instant);
            }
        }
    }
}