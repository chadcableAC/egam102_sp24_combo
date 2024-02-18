using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MicroCombo
{
    public class GalleryPreview : MonoBehaviour
    {
        // UI information
        [SerializeField] private Image _iconImage = null;
        [SerializeField] private Sprite _backupSprite = null;

        [SerializeField] private ControlUi _controlUi = null;

        [SerializeField] private TextMeshProUGUI _nameText = null;
        [SerializeField] private TextMeshProUGUI _studentText = null;
        [SerializeField] private TextMeshProUGUI _descriptionText = null;

        [SerializeField] private GameObject _buttonEnableHandle = null;
        [SerializeField] private ComboButtonUi _playButton = null;

        [SerializeField] private ScrollRect _scrollRect = null;

        private MicrogameData _lastData = null;

        void Awake()
        {
            _playButton.OnButton += OnPlay;
        }

        public void SetData(MicrogameData data)
        {
            _lastData = data;

            Sprite iconSprite = data.GetGallerySprite();
            if (iconSprite == null)
            {
                iconSprite = _backupSprite;
            }
            _iconImage.sprite = iconSprite;

            _controlUi.SetStyle(data.controlType);

            _nameText.text = data.microgameName;
            _descriptionText.text = data.galleryString;

            string gameCount = string.Format("Game #{0}", data.microgameNumber);
            if (data.isCover)
            {
                gameCount = "Warioware Cover";
            }

            _studentText.text = string.Format("{0} / {1}", data.studentName, gameCount);

            // Snap back to the top
            _scrollRect.verticalNormalizedPosition = 1f;

            _buttonEnableHandle.SetActive(data.isMicrogamePrefabSupported);
        }

        private void OnPlay()
        {
            // Get this game loaded...
            if (_lastData != null)
            {
                ComboManager combo = FindObjectOfType<ComboManager>();
                if (combo != null)
                {
                    combo.GoToGameFromGallery(_lastData);
                }
            }
        }
    }
}
