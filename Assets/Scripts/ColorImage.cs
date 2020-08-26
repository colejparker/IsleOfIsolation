using ColorPickerUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorImage : MonoBehaviour
{
    public Image image;
    [SerializeField] ColorPicker colorPicker;
    [SerializeField] PlayerEditImage mainPlayerImage;
    public string affectedElement;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (affectedElement == "Hair")
        {
            image.color = mainPlayerImage.Hair.color;
        }
        else if (affectedElement == "Shoes")
        {
            image.color = mainPlayerImage.Shoes.color;
        }
        else if (affectedElement == "Shirt")
        {
            image.color = mainPlayerImage.Shirt.color;
        }
        else if (affectedElement == "Pants")
        {
            image.color = mainPlayerImage.Pants.color;
        }
        else if (affectedElement == "Eyes")
        {
            image.color = mainPlayerImage.Eyes.color;
        }
        else if (affectedElement == "Skin")
        {
            image.color = mainPlayerImage.Skin.color;
        }
    }

    public void TribeColor(Tribe tribe)
    {
        image.color = tribe.tribeColor;
    }


    public void OpenColorPicker()
    {
        colorPicker.currentColor = image.color;
        colorPicker.colorImage = this;
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
        } else if (affectedElement == "Pants")
        {
            mainPlayerImage.Pants.color = colorPicker.newColor;
        }
        else if (affectedElement == "Eyes")
        {
            mainPlayerImage.Eyes.color = colorPicker.newColor;
        }
        else if (affectedElement == "Skin")
        {
            mainPlayerImage.Skin.color = colorPicker.newColor;
        }
        image.color = colorPicker.newColor;
        
    }
}
