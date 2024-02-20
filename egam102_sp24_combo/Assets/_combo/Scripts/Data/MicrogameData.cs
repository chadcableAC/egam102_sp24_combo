using UnityEngine;

namespace MicroCombo
{
    [CreateAssetMenu(fileName = "Data", menuName = "Combo/Data", order = 21)]
    public class MicrogameData : ScriptableObject
    {
        // Basic information
        [SerializeField] private string _studentName = string.Empty;
        public string studentName 
        {
            get { return _studentName; }
        }

        public string GetFirstName()
        {
            string retString = string.Empty;
            if (!string.IsNullOrEmpty(studentName))
            {
                string[] splitNames = studentName.Split(" ");
                retString = splitNames[0];
            }
            return retString;
        }

        public string GetShortName()
        {
            string retString = string.Empty;
            if (!string.IsNullOrEmpty(studentName))
            {
                string[] splitNames = studentName.Split(" ");
                for (int i = 0; i < splitNames.Length; i++)
                {
                    retString += splitNames[i][0];
                }
            }
            return retString;
        }

        [SerializeField] private string _microgameName = string.Empty;
        public string microgameName 
        {
            get 
            { 
                string ret = _microgameName;
                if (string.IsNullOrEmpty(ret))
                {
                    ret = "Untitled";
                }
                return ret; 
            }
        }

        [SerializeField] private int _microgameNumber = 0;
        public int microgameNumber 
        {
            get 
            { 
                int num = _microgameNumber;
                if (isCover)
                {
                    num = 0;
                }
                return num;
            }
        }

        [SerializeField] private bool _isCover = false;
        public bool isCover 
        {
            get { return _isCover; }
        }

        [SerializeField] private Sprite _previewSprite = null;
        public Sprite previewSprite 
        {
            get { return _previewSprite; }
        }

        // Gallery information
        [TextArea]
        [SerializeField] private string _galleryString = string.Empty;
        public string galleryString 
        {
            get 
            { 
                string ret = _galleryString;
                if (string.IsNullOrEmpty(ret))
                {
                    ret = "No description";
                }
                return ret; 
            }
        }

        public Sprite GetGallerySprite()
        {
            return _previewSprite;
        }

        // Control info
        public enum ControlTypes
        {
            Keyboard,
            KeyboardPlus,
            Mouse,
            All
        }

        [SerializeField] private ControlTypes _controlType = ControlTypes.Keyboard;
        public ControlTypes controlType 
        {
            get { return _controlType; }
        }

        // Scene information
        [SerializeField] private bool _isMicrogamePrefabSupported = true;
        public bool isMicrogamePrefabSupported 
        {
            get { return _isMicrogamePrefabSupported; }
        }

        [SerializeField] private float _timeoutDuration = 10f;
        public float timeoutDuration 
        {
            get { return _timeoutDuration; }
        }

        [SerializeField] private float _additionalExitTime = 0f;
        public float additionalExitTimeWin
        {
            get { return _additionalExitTime; }
        }

        [SerializeField] private float _additionalExitTimeLose = 0f;
        public float additionalExitTimeLose
        {
            get { return _additionalExitTimeLose; }
        }

        [HideInInspector]
        [SerializeField] private string _sceneName;
        public string sceneName
        {
            get { return _sceneName; }
        }

        public static int SceneCount = 1;

        public string GetSceneName(int index)
        {
            return sceneName;
        }

        public string GetSceneVariableName(int index)
        {
            return "_sceneName";
        }

        public string GetScenePath()
        {
            return sceneName;
        }

        public bool isValid
        {
            get 
            {
                bool hasScene = false;
                if (!string.IsNullOrEmpty(sceneName))
                {
                    hasScene = true;    
                }
                return hasScene;
            }
        }
    }
}