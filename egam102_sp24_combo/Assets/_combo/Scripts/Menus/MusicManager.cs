using UnityEngine;
using UnityEngine.Audio;

namespace MicroCombo
{
    public class MusicManager : MonoBehaviour
    {
        // References
        [SerializeField] private AudioMixer _audioMixer = null;
        [SerializeField] private AudioMixerSnapshot _defaultSnapshot = null;
        [SerializeField] private AudioMixerSnapshot _resultsSnapshot = null;
        [SerializeField] private Vector2 _audioRange = new Vector2(-80f, 0f);

        [SerializeField] private string _audioMasterName = "MasterVolume";
        [SerializeField] private string _audioComboName = "ComboVolume";
        [SerializeField] private string _audioMicroName = "MicroVolume";

        [SerializeField] private AudioMixerGroup _microgameGroup = null;
        public AudioMixerGroup microgameGroup 
        {
            get { return _microgameGroup; }
        }

        private bool _isMuted = false;
        public bool isMuted 
        {
            get { return _isMuted; }
        }

        [SerializeField] private AudioSource _musicSource = null;

        [SerializeField] private SpringInterper _volumeInterper = null;
        private float _transitionInterp = 0f;
        
        public enum SfxType
        {
            Success,
            Lose,
            Button,
            Results,
            SuccessSmall,
            LoseSmall
        }

        [SerializeField] private AudioSource[] _sfxsSuccess = null;
        [SerializeField] private AudioSource[] _sfxsLose = null;
        [SerializeField] private AudioSource[] _sfxsButton = null;
        [SerializeField] private AudioSource[] _sfxsResults = null;
        [SerializeField] private AudioSource[] _sfxsSuccessSmall = null;
        [SerializeField] private AudioSource[] _sfxsLoseSmall = null;

        void Awake()
        {
            // Sub
            _volumeInterper.OnInterpUpdated += OnSubVolumeInterp;         

            // Set the initial muted state
            if (PlayerPrefs.HasKey(ComboManager.AudioMutedKey))
            {
                SetMuted(PlayerPrefs.GetInt(ComboManager.AudioMutedKey) == 1, false);
            }

            // Prep going to the current scene
            DuckMusic(false, true);
        }

        public void OnTransitionInterp(float interp)
        {
            _transitionInterp = Mathf.Lerp(1f, 0f, interp);
            UpdateVolume();
        }

        private void OnSubVolumeInterp(float interp)
        {
            UpdateVolume();
        }

        private void UpdateVolume()
        {
            float comboInterp = _volumeInterper.value;
            float microInterp = 1f - comboInterp;
            float masterInterp = 1f;

            if (_isMuted)
            {
                comboInterp = 0;
                microInterp = 0;
                masterInterp = 0f;
            }
            else
            {
                comboInterp = Mathf.Min(_transitionInterp, comboInterp);
                microInterp = Mathf.Min(_transitionInterp, microInterp);
            }

            // Combo sounds (mostly music fading)
            float comboVolume = Mathf.Lerp(_audioRange.x, _audioRange.y, comboInterp);
            _audioMixer.SetFloat(_audioComboName, comboVolume);
            
            // Individual microgames
            float microVolume = Mathf.Lerp(_audioRange.x, _audioRange.y, microInterp);
            _audioMixer.SetFloat(_audioMicroName, microVolume);

            // Remaining effects (menus)
            float masterVolume = Mathf.Lerp(_audioRange.x, _audioRange.y, masterInterp);
            _audioMixer.SetFloat(_audioMasterName, masterVolume);
        }

        public void SetMuted(bool isMuted, bool writeToDisk)
        {
            _isMuted = isMuted;
            UpdateVolume();

            if (writeToDisk)
            {
                PlayerPrefs.SetInt(ComboManager.AudioMutedKey, isMuted ? 1 : 0);
            }

            // Debug.LogFormat("muted {0}", isMuted);
        }

        public void DuckMusic(bool isDucked, bool instant)
        {
            _volumeInterper.SetGoal(isDucked ? 0f : 1f, instant);

            // Debug.LogFormat("ducking {0}, instant {1}", isDucked, instant);
        }

        public void SetMusic(AudioClip musicClip)
        {
            if (musicClip != null &&
                _musicSource.clip != musicClip)
            {
                _musicSource.clip = musicClip;
                _musicSource.Play();
            }
        }

        public void SetMusicFilter(bool isFiltered)
        {
            if (isFiltered)
            {
                _resultsSnapshot.TransitionTo(0);
            }
            else            
            {
                _defaultSnapshot.TransitionTo(0);
            }            
        }

        public void Play(SfxType sfx)
        {
            AudioSource[] sources = null;
            
            switch (sfx)
            {
                case SfxType.Button:
                    sources = _sfxsButton;
                    break;
                case SfxType.Lose:
                    sources = _sfxsLose;
                    break;
                case SfxType.Success:
                    sources = _sfxsSuccess;
                    break;
                case SfxType.Results:
                    sources = _sfxsResults;
                    break;
                case SfxType.SuccessSmall:
                    sources = _sfxsSuccessSmall;
                    break;
                case SfxType.LoseSmall:
                    sources = _sfxsLoseSmall;
                    break;
            }

            if (sources != null)
            {
                // Find the first non-playing one
                for (int i = 0; i < sources.Length; i++)
                {
                    if (!sources[i].isPlaying)
                    {
                        sources[i].Play();
                        break;
                    }
                }
            }
        }
    }
}
