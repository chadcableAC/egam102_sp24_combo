using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MicroCombo
{
    public class GalleryButton : MonoBehaviour
    {
        // UI info
        [SerializeField] private Image _iconImage = null;
        [SerializeField] private TextMeshProUGUI _nameText = null;

        [SerializeField] private Sprite _backupSprite = null;

        [SerializeField] private Button _button = null;

        [SerializeField] private Graphic[] _colorableUis = null;
        [SerializeField] private Graphic[] _colorableAltUis = null;

        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _selectedAltColor;

        [SerializeField] private Color _unselectedColor;
        [SerializeField] private Color _unselectedAltColor;

        private MenuGallery _gallery = null;
        
        private MicrogameData _data = null;
        public MicrogameData data  
        {
            get { return _data; }
        }

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

        public void Init(MenuGallery gallery)
        {
            _gallery = gallery;
        }

        public void SetData(MicrogameData data)
        {
            _data = data;

            Sprite iconSprite = data.GetGallerySprite();
            if (iconSprite == null)
            {
                iconSprite = _backupSprite;
            }
            _iconImage.sprite = iconSprite;

            string gameNumber = string.Format("#{0}", data.microgameNumber);
            if (data.isCover)
            {
                gameNumber = "Cover";
            }
            _nameText.text = string.Format("{0} {1}", data.GetFirstName(), gameNumber);
        }

        private void OnClicked()
        {
            _musicManager.Play(MusicManager.SfxType.Button);
            _gallery.OnButtonPressed(_data);
        }

        public void SetSelected(MicrogameData data)
        {
            bool isSelected = data == _data;
            
            Color color = _unselectedColor;
            Color colorAlt = _unselectedAltColor;
            if (isSelected)
            {
                color = _selectedColor;
                colorAlt = _selectedAltColor;
            }

            for (int i = 0; i < _colorableUis.Length; i++)
            {
                _colorableUis[i].color = color;
            }

            for (int i = 0; i < _colorableAltUis.Length; i++)
            {
                _colorableAltUis[i].color = colorAlt;
            }
        }
    }
}