using System.Collections.Generic;
using UnityEngine;

namespace MicroCombo
{
    public class ControlLegendUi : MonoBehaviour
    {
        // UI
        [SerializeField] private ControlUi _controlPrefab = null;
        [SerializeField] private Transform _parentHandle = null;
        private List<ControlUi> _controlUis = new List<ControlUi>();

        void Awake()
        {
            // Make UIs
            for (int i = 0; i < UtilsMath.Size<MicrogameData.ControlTypes>(); i++)
            {
                MicrogameData.ControlTypes type = (MicrogameData.ControlTypes) i;
                if (type != MicrogameData.ControlTypes.All)
                {                
                    ControlUi controlUi = GameObject.Instantiate<ControlUi>(_controlPrefab);
                    controlUi.transform.SetParent(_parentHandle, false);

                    controlUi.SetStyle(type);

                    _controlUis.Add(controlUi);                   
                }
            }
        }
        
        public void SetNone(bool instant)
        {
            for (int i = 0; i < _controlUis.Count; i++)
            {
                _controlUis[i].SetEnabled(false, instant);
            }
        }

        public void SetStyle(MicrogameData.ControlTypes controlType)
        {
            for (int i = 0; i < _controlUis.Count; i++)
            {
                bool isMatch = i == (int) controlType;
                _controlUis[i].SetEnabled(isMatch, false);
            }
        }
    }
}