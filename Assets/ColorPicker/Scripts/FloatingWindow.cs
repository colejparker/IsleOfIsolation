using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ColorPickerUtil
{
    public class FloatingWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        bool dragging = false;
        Vector3 dif;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            dragging = true;
            dif = transform.position - Input.mousePosition;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            dragging = false;
        }

        private void Update()
        {
            if (dragging)
            {
                transform.position = Input.mousePosition + dif;
            }
        }
    }
}