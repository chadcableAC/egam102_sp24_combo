using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MicroCombo.MenuBase;

namespace MicroCombo
{
    public class MenuManager : MonoBehaviour
    {
        // UI information
        private class MenuInstance
        {
            public TabType type;
            public MenuBase menu;
            public MenuTab tab;

            public void Init(MenuManager menuManager, MenuData menuData)
            {
                tab.Init(menuManager, menuData);
                menu.Init(menuManager, menuData);            
            }

            public void SetVisible(bool isVisible)
            {
                tab.SetSelected(isVisible);
                menu.SetVisible(isVisible);
            }
        }

        [SerializeField] private TabType _startTab = MenuBase.TabType.Play;
        private List<MenuInstance> _instances = new List<MenuInstance>();

        [SerializeField] private MenuDictionary _dictionary = null;

        [SerializeField] private MenuTab _tabPrefab = null;
        [SerializeField] private Transform _tabParentHandle = null;

        [SerializeField] private Graphic[] _colorableUis = null;

        private ComboManager _comboManager = null;
        public ComboManager comboManager 
        {
            get { return _comboManager; }
        }

        void Awake()
        {
            // Combo information
            _comboManager = GameObject.FindObjectOfType<ComboManager>();

            // Get the tab data
            MenuBase[] menus = transform.GetComponentsInChildren<MenuBase>();
            foreach(MenuBase menu in menus)
            {
                TabType type = menu.tabType;
                if (GetInstance(type) == null)
                {
                    // New data
                    MenuInstance newData = new MenuInstance();
                    newData.type = type;
                    newData.menu = menu;                    

                    // New tab
                    MenuTab newTab = GameObject.Instantiate<MenuTab>(_tabPrefab);
                    newTab.transform.SetParent(_tabParentHandle, false);
                    newData.tab = newTab;

                    // Init
                    MenuData menuData = _dictionary.GetData(type);
                    newData.Init(this, menuData);

                    // Add to the list
                    _instances.Add(newData);
                }
            }

            // Sort the tabs?
            for (int i = _dictionary.datas.Length - 1; i >= 0; i--)            
            {
                MenuInstance menuInstance = GetInstance(_dictionary.datas[i].tabType);
                if (menuInstance != null)
                {
                    menuInstance.tab.transform.SetAsFirstSibling();
                }
            }
            
            // Show the starting data
            SetTab(_startTab);        
        }

        private MenuInstance GetInstance(TabType type)
        {
            MenuInstance ret = null;
            foreach(MenuInstance instance in _instances)
            {
                if (type == instance.type)
                {
                    ret = instance;
                    break;
                }
            }
            return ret;
        }

        public void SetTab(TabType type)
        {
            // Turn all off
            foreach(MenuInstance instance in _instances)
            {
                instance.SetVisible(false);
            }

            // Turn this one on
            MenuInstance inst = GetInstance(type);
            if (inst == null &&
                _instances.Count > 0)
            {
                inst = _instances[0];
            }

            if (inst != null)
            {
                inst.SetVisible(true);
            }

            // Update the colors
            MenuData data = _dictionary.GetData(type);
            for (int i = 0; i < _colorableUis.Length; i++)
            {
                _colorableUis[i].color = data.bgColor;
            }
        }
    }
}