using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MicroCombo
{
    public class MenuGallery : MenuBase
    {
        // UI informatino
        [SerializeField] private GalleryButton _buttonPrefab = null;
        [SerializeField] private Transform _buttonParentHandle = null;

        private List<GalleryButton> _buttons = new List<GalleryButton>();

        [SerializeField] private GalleryPreview _preview = null;

        [SerializeField] private ScrollRect _scrollRect = null;
        [SerializeField] private GridLayoutGroup _gridLayout = null;

        public override void Init(MenuManager menuManager, MenuData menuData)
        {
            // Build the UIs
            MicrogameDictionary dictionary = menuManager.comboManager.dictionary;
            
            List<MicrogameData> datas = new List<MicrogameData>();
            dictionary.GetDatas(ref datas);

            // Create / set
            for (int i = 0; i < datas.Count; i++)
            {
                GalleryButton button = GetButton(i);
                button.SetData(datas[i]);

                _buttons[i].gameObject.SetActive(true);
            }
            
            // Deactivate leftovers
            for (int i = datas.Count; i < _buttons.Count; i++)
            {
                _buttons[i].gameObject.SetActive(false);
            }

            // Set the gallery to the first one?
            if (datas.Count > 0)
            {
                OnButtonPressed(datas[0]);
            }            
        }

        private GalleryButton GetButton(int index)
        {
            while (index >= _buttons.Count)
            {
                GalleryButton newButton = GameObject.Instantiate<GalleryButton>(_buttonPrefab);
                newButton.transform.SetParent(_buttonParentHandle, false);
                newButton.Init(this);        

                _buttons.Add(newButton);        
            }

            return _buttons[index];
        }

        public void OnButtonPressed(MicrogameData data)
        {
            // Populate the viewer
            _preview.SetData(data);

            // Highlight buttons
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].SetSelected(data);
            }
        }

        public void AutoSelect(List<MicrogameData> datas)
        {
            if (datas.Count > 0)
            {
                MicrogameData dataToSelect = datas[0];
                OnButtonPressed(dataToSelect);

                // Do we need the normalized position?
                GalleryButton button = null;
                for (int i = 0; i < _buttons.Count; i++)
                {
                    if (_buttons[i].data == dataToSelect)
                    {
                        button = _buttons[i];
                        break;
                    }
                }

                if (button != null)
                {
                    Vector3 buttonPos = button.transform.localPosition;
                    Vector2 size = _scrollRect.content.sizeDelta;
                    Vector2 gridSize = _gridLayout.cellSize;
                    RectOffset gridPadding = _gridLayout.padding;

                    float cellHeight = gridSize.y;
                    float topBuffer = gridPadding.top;
                    float bottomBuffer = gridPadding.bottom;

                    float pos = Mathf.Abs(buttonPos.y) - topBuffer - (cellHeight * 0.5f);
                    float height = size.y - cellHeight - topBuffer - bottomBuffer;

                    float normPos = 1f - Mathf.Clamp01(pos / height);
                    _scrollRect.verticalNormalizedPosition = normPos;

                    // Debug.LogFormat("pos {0}, size {1}, adjPos {2}, adjSize {3}", buttonPos, size, pos, height);
                }
            }
        }

#if UNITY_EDITOR
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                int randomIndex = Random.Range(0, _buttons.Count);
                List<MicrogameData> temp = new List<MicrogameData>();
                temp.Add(_buttons[randomIndex].data);
                AutoSelect(temp);
            }
        }
#endif
    }
}