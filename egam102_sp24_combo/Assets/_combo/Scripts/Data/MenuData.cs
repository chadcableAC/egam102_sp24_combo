using UnityEngine;

namespace MicroCombo
{
    [CreateAssetMenu(fileName = "Data", menuName = "Combo/Menu Data", order = 31)]
    public class MenuData : ScriptableObject
    {
        [SerializeField] private MenuBase.TabType _type;
        public MenuBase.TabType tabType 
        {
            get { return _type; }
        }

        [SerializeField] private Sprite _icon;
        public Sprite icon 
        {
            get { return _icon; }
        }

        [SerializeField] private string _text;
        public string text 
        {
            get { return _text; }
        }

        [SerializeField] private Color _bgColor;
        public Color bgColor 
        {
            get { return _bgColor; }
        }

        [SerializeField] private Color _fgColor;
        public Color fgColor 
        {
            get { return _fgColor; }
        }
    }
}
