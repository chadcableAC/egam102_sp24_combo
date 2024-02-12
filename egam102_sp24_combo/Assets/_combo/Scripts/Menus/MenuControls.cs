using UnityEngine;

namespace MicroCombo
{
    public class MenuControls : MenuBase
    {
        // UI information
        [SerializeField] private ControlUi[] _controlUis = null;

        void Awake()
        {
            for (int i = 0; i < _controlUis.Length; i++)
            {
                MicrogameData.ControlTypes controlType = (MicrogameData.ControlTypes) i;
                _controlUis[i].SetStyle(controlType);
            }
        }   
    }
}