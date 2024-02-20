using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MicroCombo
{
    public class GameUi : MonoBehaviour
    {
        // UI 
        [SerializeField] private Sprite _backupSprite = null;
        [SerializeField] private Image _iconImage = null;

        [SerializeField] private GameObject _winEnableHandle = null;
        [SerializeField] private GameObject _loseEnableHandle = null;

        // Animation
        [SerializeField] private SpringInterper _interper = null;
        [SerializeField] private AnimateShake _shaker = null;
        [SerializeField] private ParticleSystem _fxWin = null;

        // Fall
        [SerializeField] private float _fallDelay = 0.33f;

        [SerializeField] private Transform _launchHandle = null;
        [SerializeField] private Transform _launchRotateHandle = null;
        private Vector3 _originalLaunchPosition = Vector3.zero;
        private Quaternion _originalLaunchRotation = Quaternion.identity;

        [SerializeField] private Vector2 _launchDegreeRange = new Vector2(10, 30);
        [SerializeField] private Vector2 _launchStrengthRange = new Vector2(10, 30);
        [SerializeField] private float _gravity = -1f;
        [SerializeField] private float _fallDuration = 2f;
        [SerializeField] private Vector2 _rotateDegreeRange = new Vector2(10, 30);
        private Coroutine _fallRoutine = null;
        
        // Music
        private MusicManager _musicManager = null;

        void Awake()
        {
            _originalLaunchPosition = _launchHandle.localPosition;
            _originalLaunchRotation = _launchRotateHandle.localRotation;

            SetData(null);
        }

        void Start()
        {
            _musicManager = FindObjectOfType<MusicManager>();
        }

        public void SetData(MicrogameData data)
        {
            Sprite icon = null;
            if (data != null)
            {
                icon = data.GetGallerySprite();
            }
            if (icon == null)
            {
                icon = _backupSprite;
            }
            _iconImage.sprite = icon;

            _winEnableHandle.SetActive(false);
            _loseEnableHandle.SetActive(false);

            _launchHandle.localPosition = _originalLaunchPosition;
            _launchRotateHandle.localRotation = _originalLaunchRotation;
        }

        public void SetInterp(float interp, bool instant)
        {
            _interper.SetGoal(interp, instant);

            if (interp >= 1)
            {
                _winEnableHandle.SetActive(false);
                _loseEnableHandle.SetActive(false);
            }
        }

        public void SetResult(bool isWin)
        {
            _winEnableHandle.SetActive(isWin);
            _loseEnableHandle.SetActive(!isWin);

            if (isWin)
            {
                _fxWin.Play();
                _musicManager.Play(MusicManager.SfxType.Success);
            }
            else
            {
                // Shake and fall off screen
                _shaker.Shake();
                Fall();
                _musicManager.Play(MusicManager.SfxType.Lose);
            }
        }

        public void Fall()
        {
            // Kickoff the routien
            Stop();
            _fallRoutine = StartCoroutine(ExecuteFall());
        }

        private IEnumerator ExecuteFall()
        {
            yield return new WaitForSeconds(_fallDelay);

            // Pick an angle
            float launchDegree = Random.Range(_launchDegreeRange.x, _launchDegreeRange.y);
            float launchStrength = Random.Range(_launchStrengthRange.x, _launchStrengthRange.y);

            Vector3 direction = Quaternion.Euler(Vector3.back * launchDegree) * Vector3.up * launchStrength;
            
            float rotationDegree = Random.Range(_rotateDegreeRange.x, _rotateDegreeRange.y);
            
            float animT = 0;
            while (animT < _fallDuration)
            {
                direction.y += _gravity * Time.deltaTime;
                _launchHandle.localPosition += (direction * Time.deltaTime);

                _launchRotateHandle.localRotation *= Quaternion.Euler(Vector3.back * rotationDegree * Time.deltaTime);

                yield return null;
                animT += Time.deltaTime;
            }

            _fallRoutine = null;
        }

        void OnDisable()
        {
            Stop();
        }

        private void Stop()
        {
            if (_fallRoutine != null)
            {
                StopCoroutine(_fallRoutine);
                _fallRoutine = null;
            }
        }
    }
}
