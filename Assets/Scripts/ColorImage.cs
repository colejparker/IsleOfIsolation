using ColorPickerUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorImage : MonoBehaviour
{
    Image image;
    [SerializeField] ColorPicker colorPicker;
    [SerializeField] PlayerEditImage mainPlayerImage;
    public string affectedElement = "Hair";

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }


    private void OnMouseDown()
    {
        colorPicker.currentColor = image.color;
    }

    public void PickColor()
    {
        if (affectedElement == "Hair")
        {
            mainPlayerImage.Hair.color = colorPicker.newColor;
            mainPlayerImage.Beard.color = colorPicker.newColor;
        }
        else if (affectedElement == "Shoes")
        {
            mainPlayerImage.Shoes.color = colorPicker.newColor;
        }
        else if (affectedElement == "Shirt")
        {
            mainPlayerImage.Shirt.color = colorPicker.newColor;
        } else
        {
            mainPlayerImage.Pants.color = colorPicker.newColor;
        }
        image.color = colorPicker.newColor;
        
    }
}
