using System.Collections.Generic;
using UnityEngine;

namespace MicroCombo
{
    [CreateAssetMenu(fileName = "Dictionary", menuName = "Combo/Dictionary", order = 20)]
    public class MicrogameDictionary : ScriptableObject
    {
        // Microgame data
        [SerializeField] private MicrogameData[] _microgameDatas = null;
        public MicrogameData[] microgameDatas 
        {
            get { return _microgameDatas; }
        }

        public void GetDatas(ref List<MicrogameData> datas)
        {
            GetDatas(ref datas, string.Empty, MicrogameData.ControlTypes.All);
        }

        public void GetDatas(ref List<MicrogameData> datas, 
            string studentName, MicrogameData.ControlTypes controlType)
        {
            datas.Clear();

            // Add all of them
            datas.AddRange(_microgameDatas);

            // Prune based on settings
            for (int i = datas.Count - 1; i >= 0; i--)
            {
                if (!datas[i].isValid)
                {
                    datas.RemoveAt(i);
                }
            }

            if (MicrogameData.ControlTypes.All != controlType)
            {
                for (int i = datas.Count - 1; i >= 0; i--)
                {
                    if (datas[i].controlType != controlType)
                    {
                        datas.RemoveAt(i);
                    }
                }
            }

            if (!string.IsNullOrEmpty(studentName))
            {
                for (int i = datas.Count - 1; i >= 0; i--)
                {
                    if (studentName != datas[i].studentName)
                    {
                        datas.RemoveAt(i);
                    }
                }
            }
        }
    }
}