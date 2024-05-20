using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorGame
{
    public class ColorReceiver : MonoBehaviour
    {
        private SpriteRenderer SPR_Image;

        [SerializeField] private ColoringManager REF_ColoringManager;

        void Start()
        {
            SPR_Image = GetComponent<SpriteRenderer>();
            // ColorPicker.OnColorPicked += GetColor;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // 0 is for left mouse button
            {
                Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    ApplyColor();
                }
            }
        }

        public void ApplyColor()
        {
            SPR_Image.color = REF_ColoringManager.GetCurrentColor();
        }
    }
}
