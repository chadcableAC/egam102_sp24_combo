using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MicroCombo
{
    public class MenuTab : MonoBehaviour
    {
        // Shared info
        private MenuManager _menu;

        [SerializeField] private MenuBase.TabType _type = MenuBase.TabType.Play;
        [SerializeField] private GameObject _enableHandle = null;

        [SerializeField] private GameObject _selectedEnableHandle = null;
        [SerializeField] private GameObject _unselectedEnableHandle = null;

        // UI information
        [SerializeField] private Image _iconImage = null;
        [SerializeField] private TextMeshProUGUI _nameText = null;

        [SerializeField] private Button _button = null;
        public Button button
        {
            get { return _button; }
        }

        // Coloring
        [SerializeField] private Graphic[] _colorableUis = null;
        [SerializeField] private Graphic[] _colorableAltUis = null;

        // Music
        private MusicManager _musicManager = null;

        void Awake()
        {
            _button.onClick.AddListener(OnClicked);
        }

        void Start()
        {
            _musicManager = FindObjectOfType<MusicManager>();
        }

        public void Init(MenuManager menu, MenuData menuData)
        {
            _menu = menu;
            _type = menuData.tabType;

            // Switch based on type?
            _iconImage.sprite = menuData.icon;
            _nameText.text = menuData.text;

            // Colors
            for (int i = 0; i < _colorableUis.Length; i++)
            {
                _colorableUis[i].color = menuData.bgColor;
            }
            
            for (int i = 0; i < _colorableAltUis.Length; i++)
            {
                _colorableAltUis[i].color = menuData.fgColor;
            }
        }

        public void SetSelected(bool isSelected)
        {
            _selectedEnableHandle.SetActive(isSelected);
            _unselectedEnableHandle.SetActive(!isSelected);
        }

        public void SetVisible(bool isVisible)
        {
            _enableHandle.SetActive(isVisible);
        }

        private void OnClicked()
        {
            _menu.SetTab(_type);
            _musicManager.Play(MusicManager.SfxType.Button);
        }
    }
}