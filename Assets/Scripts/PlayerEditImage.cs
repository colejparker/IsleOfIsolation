using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEditImage : MonoBehaviour
{

    [SerializeField] Image Skin;
    [SerializeField] Image Eyes;
    public Image Hair;
    public int currentHairIndex = 0;
    [SerializeField] Transform HairChoiceParent;
    public Image Shirt;
    public int currentShirtIndex = 0;
    [SerializeField] Transform ShirtChoiceParent;
    public Image Pants;
    public int currentPantsIndex = 0;
    [SerializeField] Transform PantsChoiceParent;
    public Image Shoes;
    public int currentShoesIndex = 0;
    [SerializeField] Transform ShoesChoiceParent;
    [SerializeField] Image Beard;
    [SerializeField] Image Glasses;

    public Player player;

    public void CreatePlayerImage(Player incomingPlayer)
    {
        player = incomingPlayer;
        this.Beard.enabled = player.Beard.enabled;
        this.Beard.color = player.Beard.color;
        this.Glasses.enabled = player.Glasses.enabled;
        this.Skin.color = player.Skin.color;
        this.Eyes.color = player.Eyes.color;
        Shirt = GetImageFromPlayer(player.ShirtChoiceParent.GetComponentsInChildren<SpriteRenderer>(), ShirtChoiceParent.GetComponentsInChildren<Image>(), currentShirtIndex);
        Shirt.color = player.Shirt.color;
        Shirt.enabled = true;
        Pants = GetImageFromPlayer(player.PantsChoiceParent.GetComponentsInChildren<SpriteRenderer>(), PantsChoiceParent.GetComponentsInChildren<Image>(), currentPantsIndex);
        Pants.color = player.Pants.color;
        Pants.enabled = true;
        Shoes = GetImageFromPlayer(player.ShoesChoiceParent.GetComponentsInChildren<SpriteRenderer>(), ShoesChoiceParent.GetComponentsInChildren<Image>(), currentShoesIndex);
        Shoes.color = player.Shoes.color;
        Shoes.enabled = true;
        Hair = GetImageFromPlayer(player.HairChoiceParent.GetComponentsInChildren<SpriteRenderer>(), HairChoiceParent.GetComponentsInChildren<Image>(), currentHairIndex);
        Hair.color = player.Hair.color;
        Hair.enabled = true;
    }

    private void ChangeElement(int incOrDec, ref Image elementToChange, ref int elementIndex, Transform parent)
    {
        Color color = elementToChange.color;
        Image[] choices = parent.GetComponentsInChildren<Image>();
        if (incOrDec>0)
        {
            elementIndex++;
            if (elementIndex >= choices.Length)
            {
                elementIndex = 0;
            }
        } else
        {
            elementIndex--;
            if (elementIndex < 0)
            {
                elementIndex = choices.Length - 1;
            }
        }
        elementToChange = choices[elementIndex];
        elementToChange.color = color;
        
    }

    public void NextHair()
    {
        ChangeElement(1, ref Hair, ref currentHairIndex, HairChoiceParent);
    }
    public void NextShoes()
    {
        ChangeElement(1, ref Shoes, ref currentShoesIndex, ShoesChoiceParent);
    }
    public void NextShirt()
    {
        ChangeElement(1, ref Shirt, ref currentShirtIndex, ShirtChoiceParent);
    }
    public void NextPants()
    {
        ChangeElement(1, ref Pants, ref currentPantsIndex, PantsChoiceParent);
    }
    public void PrevHair()
    {
        ChangeElement(-1, ref Hair, ref currentHairIndex, HairChoiceParent);
    }
    public void PrevShoes()
    {
        ChangeElement(-1, ref Shoes, ref currentShoesIndex, ShoesChoiceParent);
    }
    public void PrevShirt()
    {
        ChangeElement(-1, ref Shirt, ref currentShirtIndex, ShirtChoiceParent);
    }
    public void PrevPants()
    {
        ChangeElement(-1, ref Pants, ref currentPantsIndex, PantsChoiceParent);
    }


    private static Image GetImageFromPlayer(SpriteRenderer[] sprites, Image[] images, int currentIndex)
    {
        Image imageToReturn = null;
        int counter = 0;
        foreach (SpriteRenderer s in sprites)
        {
            if (s.enabled)
            {
                imageToReturn = images[counter];
                currentIndex = counter;
            }
            counter++;
        }
        return imageToReturn;
    }
}
