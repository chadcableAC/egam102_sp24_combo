using UnityEngine;

namespace MicroCombo
{
    public class LivesBg : MonoBehaviour
    {
        [SerializeField] private SpringInterper _interper = null;
        [SerializeField] private RectTransform _bgRect = null;
                
        void Awake()
        {
            _interper.OnInterpUpdated += OnInterp;
        }

        private void OnInterp(float interp)
        {
            Vector2 max = _bgRect.anchorMax;
            max.y = Mathf.LerpUnclamped(0, 1f, interp);
            _bgRect.anchorMax = max;
        }
    }
}