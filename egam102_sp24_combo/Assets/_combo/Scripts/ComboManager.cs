using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MicroCombo
{
    public class ComboManager : MonoBehaviour
    {
        // Scenes
        [SerializeField] private string _menuSceneName = "menu_scene";
        [SerializeField] private string _gameSceneName = "combo_scene";

        // References
        [SerializeField] private MicrogameDictionary _dictionary = null;
        public MicrogameDictionary dictionary 
        {
            get { return _dictionary; }
        }

        // Scene references
        [SerializeField] private Camera _camera = null;
        [SerializeField] private EventSystem _eventSystem = null;        
        [SerializeField] private AudioListener _audioListener = null;

        [SerializeField] private TransitionManager _transition = null;
        private string _pendingSceneName = string.Empty;
        private bool _isPendingLoad = true;        

        private Coroutine _loadRoutine = null;

        // Data
        private List<MicrogameData> _datas = new List<MicrogameData>();
        public List<MicrogameData> datasToPlay 
        {
            get { return _datas; }
        }

        private bool _isGalleryPick = false;
        public bool isGalleryPick 
        {
            get { return _isGalleryPick; }
        }

        public class PlayOptions
        {
            public void Setup(string sName, MicrogameData.ControlTypes cType, bool inOrder, bool lives)
            {
                _studentName = sName;
                _controlType = cType;
                _playInOrder = inOrder;
                _countLives = lives;
            }

            private string _studentName = string.Empty;
            public string studentName 
            {
                get { return _studentName; }
            }

            private MicrogameData.ControlTypes _controlType = MicrogameData.ControlTypes.All;
            public MicrogameData.ControlTypes controlType 
            {
                get { return _controlType; }
            }

            private bool _playInOrder = false;
            public bool playInOrder 
            {
                get { return _playInOrder; }
            }

            private bool _countLives = true;
            public bool countLives 
            {
                get { return _countLives; }
            }
        }

        private PlayOptions _defaultOptions = new PlayOptions();
        public PlayOptions defaultOptions
        {
            get { return _defaultOptions; }
        }
            
        private PlayOptions _galleryOptions = new PlayOptions();

        public PlayOptions currentOptions
        {
            get 
            {
                PlayOptions options = _defaultOptions;
                if (_isGalleryPick)
                {
                    options = _galleryOptions;
                }
                return options;
            }
        }

        // Scoring
        private static readonly string ScoreKeyFormat = "score_{0}";
        private static readonly int MaxScores = 1;

        public static readonly string AudioMutedKey = "audio";

        // Music
        private MusicManager _musicManager = null;
        [SerializeField] private AudioClip _menuMusic = null;
        [SerializeField] private AudioClip _gameMusic = null;

        void Awake()
        {
            // Make sure we survive scene changes
            DontDestroyOnLoad(gameObject);

            _defaultOptions.Setup(string.Empty, MicrogameData.ControlTypes.All, false, true);

            // Init the data list?
            _datas.Clear();
            _dictionary.GetDatas(ref _datas);
        }        

        void Start()
        {
            _musicManager = FindObjectOfType<MusicManager>();

            // What scene is it?
            Scene scene = SceneManager.GetActiveScene();
            
            AudioClip musicClip = _gameMusic;
            if (GetSafeSceneName(scene.name) == _menuSceneName)
            {
                musicClip = _menuMusic;
            }
            _musicManager.SetMusic(musicClip);

            _musicManager.DuckMusic(true, true);
            _musicManager.DuckMusic(false, false);
        }

        public void GoToMenu()
        {
            if (!_transition.isShowing)
            {
                _loadRoutine = StartCoroutine(ExecuteTransition(_menuSceneName, true));
            }
        }

        public void GoToGameFromGallery(MicrogameData data)
        {
            _isGalleryPick = true;

            _galleryOptions.Setup(string.Empty, MicrogameData.ControlTypes.All, true, true);

            GoToGame(new List<MicrogameData>() { data });
        }

        public void GoToGameFromMenu(string studentName, MicrogameData.ControlTypes controlType, 
            bool playInOrder, bool countLives)
        {
            _isGalleryPick = false;

            _defaultOptions.Setup(studentName, controlType, playInOrder, countLives);

            List<MicrogameData> datas = new List<MicrogameData>();
            _dictionary.GetDatas(ref datas, _defaultOptions.studentName, _defaultOptions.controlType);

            GoToGame(datas);
        }

        public void GoToGame()
        {
            // Use the current settings
            GoToGame(_datas);
        }

        public void GoToGame(List<MicrogameData> datas)
        {
            if (!_transition.isShowing)
            {
                // Setup data
                _datas = new List<MicrogameData>(datas);
                _loadRoutine = StartCoroutine(ExecuteTransition(_gameSceneName, false));
            }
        }

        private IEnumerator ExecuteTransition(string sceneName, bool isToMenu)
        {
            // Fade in
            _transition.Show();
            while (_transition.isRunning)
            {
                yield return null;
            }
            
            // Now we can actually load the scene
            _isPendingLoad = true;
            _pendingSceneName = sceneName;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadSceneAsync(_pendingSceneName, LoadSceneMode.Single);
            while (_isPendingLoad)
            {
                yield return null;
            }

            // To menu and gallery?  Pick that section...
            if (isToMenu && 
                _isGalleryPick)
            {
                MenuManager menu = FindObjectOfType<MenuManager>();
                if (menu != null)
                {
                    menu.SetTab(MenuBase.TabType.Gallery);                     
                    MenuGallery gallery = FindObjectOfType<MenuGallery>();           
                    if (gallery != null)
                    {
                        gallery.AutoSelect(_datas);
                    }
                }   
            }

            AudioClip musicClip = _gameMusic;
            if (isToMenu)
            {
                musicClip = _menuMusic;
            }
            _musicManager.SetMusic(musicClip);
            _musicManager.DuckMusic(false, false);

            // Transition out
            _transition.Hide(false);
            while (_transition.isRunning)
            {
                yield return null;
            }

            // All done!
            _loadRoutine = null;
        }

        void Update()
        {
            // Input / listeners for pause?


            // Search every frame for audio listeners and event managers
            // Overkill, but less management needed
            RefreshAudioListeners();
            RefreshCameras();
        }

        void LateUpdate()
        {
            RefreshEventManagers();
        }

        private void RefreshCameras()
        {
            // Needs to be at least one on
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            int nonUsCamerasEnabled = 0;
            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i] != _camera &&
                    cameras[i].enabled)
                {
                    nonUsCamerasEnabled++;
                }
            }

            _camera.enabled = nonUsCamerasEnabled <= 0;
        }

        private void RefreshAudioListeners()
        {
            AudioListener[] listeners = GameObject.FindObjectsOfType<AudioListener>(true);

            // Only one can be active - prefer not ours?
            bool isOursActive = listeners.Length == 1;
            int activeCount = 0;

            for (int i = 0; i < listeners.Length; i++)
            {
                bool isOurs = listeners[i] == _audioListener;
                
                bool isEnabled = false;
                if (isOurs &&
                    isOursActive)
                {
                    isEnabled = true;   
                }
                else if (!isOurs &&
                    !isOursActive)
                {
                    isEnabled = true;  
                }
                
                if (activeCount > 0)
                {
                    isEnabled = false;
                }

                listeners[i].enabled = isEnabled;
                if (isEnabled)
                {
                    activeCount++;
                }
            }
        }

        private void RefreshEventManagers()
        {
            EventSystem[] eventSystems = GameObject.FindObjectsOfType<EventSystem>(true);

            // Sometimes ALL are knocked out
            bool allDisabled = _transition.isShowing;

            bool restoreCursor = false;

            if (allDisabled)
            {
                for (int i = 0; i < eventSystems.Length; i++)
                {
                    eventSystems[i].enabled = false;
                }
                restoreCursor = true;
            }
            else
            {
                EventSystem newSystem = null;

                // Picking a new system?
                bool isOursActive = eventSystems.Length == 1;
                // if (_pauseUi.isVisible)
                // {
                //     isOursActive = true;
                // }

                // Just get the reference
                for (int i = 0; i < eventSystems.Length; i++)
                {
                    bool isOurs = eventSystems[i] == _eventSystem;

                    bool isEnabled = false;
                    if (isOurs && 
                        isOursActive)
                    {
                        isEnabled = true;
                    }
                    else if (!isOurs &&
                        !isOursActive)
                    {
                        isEnabled = true;   
                    }    

                    if (isEnabled)
                    {
                        newSystem = eventSystems[i];
                        break;
                    }
                }

                if (newSystem != EventSystem.current)
                {
                    // Turn all off
                    for (int i = 0; i < eventSystems.Length; i++)
                    {
                        eventSystems[i].enabled = false;
                    }

                    // Turn only this on
                    if (newSystem != null)
                    {
                        newSystem.enabled = true;
                    }
                }

                // If our system is enabled, restore default controls
                if (newSystem == _eventSystem)
                {
                    restoreCursor = true;
                }
            }

            if (restoreCursor)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == _pendingSceneName)
            {
                SceneManager.SetActiveScene(scene);

                SceneManager.sceneLoaded -= OnSceneLoaded;
                _isPendingLoad = false;
            }
        }

        public static void ResetScores()
        {
            for (int i = 0; i < MaxScores; i++)
            {
                string key = string.Format(ScoreKeyFormat, i);
                PlayerPrefs.DeleteKey(key);
            }
        }

        public static int GetHighScore(int index)
        {
            int score = 0;
            string key = string.Format(ScoreKeyFormat, index);
            if (PlayerPrefs.HasKey(key))
            {
                score = PlayerPrefs.GetInt(key);
            }
            return score;
        }

        public static bool SaveHighScore(int newScore)
        {
            bool isNewHigh = false;

            // Collect the other scores
            List<int> scores = new List<int>();
            for (int i = 0; i < MaxScores; i++)
            {
                int thisScore = GetHighScore(i);
                
                if (!isNewHigh &&
                    newScore >= thisScore)
                {
                    scores.Add(newScore);
                    isNewHigh = true;
                }

                scores.Add(GetHighScore(i));
            }

            // Write out the best scores
            if (isNewHigh)
            {
                for (int i = 0; i < MaxScores; i++)
                {
                    string key = string.Format(ScoreKeyFormat, i);
                    PlayerPrefs.SetInt(key, scores[i]);
                }
            }

            return isNewHigh;
        }

        public static string GetSafeSceneName(string path)
        {
            string ret = path;

            // This helps us survive webGL builds
            if (path.Contains(".unity"))
            {
                string[] splits = path.Split("/");
                string[] endSplits = splits[splits.Length - 1].Split(".");
                ret = endSplits[0];
            }

            return ret;          
        }
    }
}