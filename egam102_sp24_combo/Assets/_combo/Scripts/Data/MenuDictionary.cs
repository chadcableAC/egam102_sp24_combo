using UnityEngine;

namespace MicroCombo
{
    [CreateAssetMenu(fileName = "Dictionary", menuName = "Combo/Menu Dictionary", order = 30)]
    public class MenuDictionary : ScriptableObject
    {
        // Microgame data
        [SerializeField] private MenuData[] _datas = null;
        public MenuData[] datas 
        {
            get { return _datas; }
        }

        public MenuData GetData(MenuBase.TabType type)
        {
            MenuData ret = null;
            foreach(MenuData data in _datas)
            {
                if (type == data.tabType)
                {
                    ret = data;
                    break;
                }
            }
            return ret;
        }
    }
}