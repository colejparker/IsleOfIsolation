using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPickerUtil
{
    public class Demo : MonoBehaviour
    {
        [SerializeField] ColorPicker colorPicker;
        Image currColor;

        public void OpenColorPicker(Image img)
        {
            currColor = img;
            colorPicker.currentColor = img.color;
        }

        public void PickColor()
        {
            currColor.color = colorPicker.newColor;
        }
    }
}