using System.Collections.Generic;
using UnityEngine;

namespace MicroCombo
{
    public class RingTextManagerUi : MonoBehaviour
    {
        // Spin up two rings
        [SerializeField] private RingTextUi _ringText = null;
        private List<RingTextUi> _textUis = new List<RingTextUi>();        
        private int _index = 0;

        // Start is called before the first frame update
        void Start()
        {
            Cache();
        }

        private void Cache()
        {
            if (_textUis.Count <= 0)
            {
                // Build two
                _textUis.Add(_ringText);
                for (int i = 1; i < 2; i++)
                {
                    RingTextUi textUi = Instantiate<RingTextUi>(_ringText);
                    textUi.transform.SetParent(_ringText.transform.parent, false);
                    _textUis.Add(textUi);                                
                }

                for (int i = 0; i < _textUis.Count; i++)
                {
                    _textUis[i].SetText(string.Empty, true);
                }                
            }
        }

        public void SetText(string text, bool instant)
        {
            Cache();

            // Hide curret, show next
            if (!instant)
            {
                _textUis[_index].SetText(string.Empty);
                _index = (_index + 1) % _textUis.Count;
            }

            _textUis[_index].SetText(text, instant);
        }
    }
}
