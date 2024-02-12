using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MicroCombo
{
    public class ScrollRawImageUv : MonoBehaviour
    {
        // UI information
        [SerializeField] private RawImage _image = null;
        [SerializeField] private Vector2 _scrollSpeed = Vector2.right;
        private Rect _rect;

        // Start is called before the first frame update
        void Start()
        {
            _rect = _image.uvRect;
        }

        // Update is called once per frame
        void Update()
        {
            _rect.position += _scrollSpeed * Time.deltaTime;
            _image.uvRect = _rect;
        }
    }
}
