using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

namespace MicroCombo
{
    public class MenuPlay : MenuBase
    {
        // UI info
        [SerializeField] private ComboButtonUi _playButton = null;

        [SerializeField] private GameObject _noneAvailableEnableHandle = null;
        [SerializeField] private TextMeshProUGUI _nonAvailableText = null;

        [SerializeField] private TMP_Dropdown _dropdownStudents = null;
        [SerializeField] private TMP_Dropdown _dropdownControls = null;
        [SerializeField] private TMP_Dropdown _dropdownLives = null;
        [SerializeField] private TMP_Dropdown _dropdownOrder = null;

        private class DropdownInfo
        {
            public DropdownInfo(string infoId, string infoDisplay)
            {
                _id = infoId;
                _display = infoDisplay;
            }

            private string _id;
            public string id 
            {
                get { return _id; }
            }

            private string _display;
            public string display 
            {
                get { return _display; }
            }
        }

        private List<DropdownInfo> _studentInfos = new List<DropdownInfo>();
        private List<DropdownInfo> _controlInfos = new List<DropdownInfo>();
        private List<DropdownInfo> _orderInfos = new List<DropdownInfo>();
        private List<DropdownInfo> _liveInfos = new List<DropdownInfo>();

        private ComboManager _comboManager = null;

        void Start()
        {
            _playButton.OnButton += OnPlay;

            _comboManager = FindObjectOfType<ComboManager>();

            // Names
            _studentInfos.Clear();
            _studentInfos.Add(new DropdownInfo("all", "All Students"));

            for (int i = 0; i < _comboManager.dictionary.microgameDatas.Length; i++)
            {                
                bool isNewName = true;

                string studentName = _comboManager.dictionary.microgameDatas[i].studentName;
                for (int j = 0; j < _studentInfos.Count; j++)
                {
                    if (_studentInfos[j].id == studentName)
                    {
                        isNewName = false;
                        break;
                    }
                }

                if (isNewName)
                {
                    _studentInfos.Add(new DropdownInfo(
                        studentName, 
                        string.Format("{0} only", studentName)
                    ));
                }
            }
            SetupDropdown(_dropdownStudents, _studentInfos, _comboManager.defaultOptions.studentName);
            
            // Controls
            _controlInfos.Clear();
            _controlInfos.Add(new DropdownInfo("all", "All Control Styles"));
            for (int i = 0; i < UtilsMath.Size<MicrogameData.ControlTypes>(); i++)
            {
                MicrogameData.ControlTypes controlType = (MicrogameData.ControlTypes) i;
                string controlName = string.Empty;

                switch (controlType)
                {
                    case MicrogameData.ControlTypes.Keyboard:
                        controlName = "Keyboard only";
                        break;
                    case MicrogameData.ControlTypes.KeyboardPlus:
                        controlName = "Keyboard+ only (adds Shift key)";
                        break;
                    case MicrogameData.ControlTypes.Mouse:
                        controlName = "Mouse only";
                        break;
                }

                if (!string.IsNullOrEmpty(controlName))
                {
                    _controlInfos.Add(new DropdownInfo(controlType.ToString(), controlName));
                }
            }
            SetupDropdown(_dropdownControls, _controlInfos, _comboManager.defaultOptions.controlType.ToString());
            
            // Order
            _orderInfos.Clear();
            _orderInfos.Add(new DropdownInfo("0", "Shuffle order"));
            _orderInfos.Add(new DropdownInfo("1", "Play in order"));
            SetupDropdown(_dropdownOrder, _orderInfos, _comboManager.defaultOptions.playInOrder ? "1" : "0");
            
            // Lives
            _liveInfos.Clear();
            _liveInfos.Add(new DropdownInfo("1", "Four lives"));
            _liveInfos.Add(new DropdownInfo("0", "Unlimited lives"));
            SetupDropdown(_dropdownLives, _liveInfos, _comboManager.defaultOptions.countLives ? "1" : "0");
            
            // Refres options
            OnDropdownChange(0);
        }

        public void OnPlay()
        {
            // Get the settings
            string studentName = string.Empty;
            MicrogameData.ControlTypes controlType = MicrogameData.ControlTypes.All;
            bool inOrder = false;
            bool countLives = true;
            GetSettings(out studentName, out controlType, out inOrder, out countLives);

            // Launch the game!
            ComboManager comboManager = GameObject.FindObjectOfType<ComboManager>();
            comboManager.GoToGameFromMenu(studentName, controlType, inOrder, countLives);
        }

        private void GetSettings(out string studentName, out MicrogameData.ControlTypes controlType,
            out bool inOrder, out bool countLives)
        {
            studentName = string.Empty;
            DropdownInfo studentInfo = GetInfo(_dropdownStudents);
            if (studentInfo != null &&
                studentInfo.id != "all")
            {
                studentName = studentInfo.id;
            }

            controlType = MicrogameData.ControlTypes.All;
            DropdownInfo controlInfo = GetInfo(_dropdownControls);
            if (controlInfo != null &&
                controlInfo.id != "all")
            {
                controlType = 
                    (MicrogameData.ControlTypes) Enum.Parse(typeof(MicrogameData.ControlTypes), controlInfo.id);                
            }

            inOrder = false;
            DropdownInfo orderInfo = GetInfo(_dropdownOrder);
            if (orderInfo != null)
            {
                inOrder = (orderInfo.id == "1");
            }

            countLives = true;
            DropdownInfo liveInfo = GetInfo(_dropdownLives);
            if (liveInfo != null)
            {
                countLives = (liveInfo.id == "1");
            }
        }

        private DropdownInfo GetInfo(TMP_Dropdown dropdown)
        {
            int index = dropdown.value;
            TMP_Dropdown.OptionData optionData = dropdown.options[index];
            
            List<DropdownInfo> list = null;
            if (dropdown == _dropdownStudents)
            {
                list = _studentInfos;
            }
            else if (dropdown == _dropdownControls)
            {
                list = _controlInfos;
            }
            else if (dropdown == _dropdownLives)
            {
                list = _liveInfos;
            }
            else if (dropdown == _dropdownOrder)
            {
                list = _orderInfos;
            }
            
            DropdownInfo info = null;
            if (list != null)
            {
                info = GetInfo(optionData.text, list);
            }
            return info;
        }

        private DropdownInfo GetInfo(string needle, List<DropdownInfo> haystack)
        {
            DropdownInfo info = null;
            
            for (int i = 0; i < haystack.Count; i++)
            {
                if (haystack[i].display == needle)
                {
                    info = haystack[i];
                    break;
                }
            }

            return info;
        }

        private void OnDropdownChange(int index)
        {
            // Get the settings
            string studentName = string.Empty;
            MicrogameData.ControlTypes controlType = MicrogameData.ControlTypes.All;
            bool inOrder = false;
            bool countLives = true;
            GetSettings(out studentName, out controlType, out inOrder, out countLives);

            List<MicrogameData> datas = new List<MicrogameData>();
            _comboManager.dictionary.GetDatas(ref datas, studentName, controlType);

            // We need to make sure there are datas available from the configuration
            bool isGameAvailable = datas.Count > 0;            

            _playButton.button.interactable = isGameAvailable;
            if (_noneAvailableEnableHandle != null)
            {
                _noneAvailableEnableHandle.SetActive(!isGameAvailable);
            } 
            if (_nonAvailableText != null)
            {
                _nonAvailableText.alpha = isGameAvailable ? 0 : 1;
            }
        }

        private void SetupDropdown(TMP_Dropdown dropdown, List<DropdownInfo> _studentInfos, string selected)
        {
            int index = 0;

            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < _studentInfos.Count; i++)
            {
                TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
                data.text = _studentInfos[i].display;
                options.Add(data);

                if (selected == _studentInfos[i].id)
                {
                    index = i;
                }
            }

            dropdown.ClearOptions();
            dropdown.AddOptions(options);

            dropdown.SetValueWithoutNotify(index);
            dropdown.onValueChanged.AddListener(OnDropdownChange);
        }
    }
}