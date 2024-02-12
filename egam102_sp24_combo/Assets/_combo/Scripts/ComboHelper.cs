using UnityEngine;

namespace MicroCombo
{
    public class ComboHelper : MonoBehaviour
    {
        // Object to instantiate
        [SerializeField] private ComboManager _prefab = null;

        void Awake()
        {
            ComboManager manager = GameObject.FindObjectOfType<ComboManager>();
            if (manager == null)
            {
                GameObject.Instantiate<ComboManager>(_prefab);
            }
        }
    }
}
