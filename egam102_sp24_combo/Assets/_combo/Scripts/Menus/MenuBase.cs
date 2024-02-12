using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MicroCombo
{
    public abstract class MenuBase : MonoBehaviour
    {
        // Tab types
        public enum TabType
        {
            Play,
            Gallery,
            Credits,
            Settings,
            Controls
        }

        // Shared info
        [SerializeField] private TabType _type = TabType.Play;
        public TabType tabType
        {
            get { return _type; }
        }

        [SerializeField] private GameObject _enableHandle = null;

        public virtual void Init(MenuManager menuManager, MenuData menuData)
        {
            
        }

        public virtual void SetVisible(bool isVisible)
        {
            _enableHandle.SetActive(isVisible);
        }
    }
}