using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEditImage : MonoBehaviour
{

    [SerializeField] Image Skin;
    [SerializeField] Image Eyes;
    public Image Hair;
    [SerializeField] Transform HairChoiceParent;
    public Image Shirt;
    [SerializeField] Transform ShirtChoiceParent;
    public Image Pants;
    [SerializeField] Transform PantsChoiceParent;
    public Image Shoes;
    [SerializeField] Transform ShoesChoiceParent;
    public Image Beard;
    [SerializeField] Image Glasses;
    [SerializeField] Image Buff;
    public List<Image> imageHairChoices;
    public List<Image> imageShirtChoices;
    public List<Image> imageShoesChoices;
    public List<Image> imagePantsChoices;


    public Player player;

    public void CreatePlayerImage(Player incomingPlayer)
    {
        player = incomingPlayer;

        this.Beard.enabled = player.Beard.enabled;
        this.Beard.color = player.Beard.color;
        this.Glasses.enabled = player.Glasses.enabled;
        this.Skin.color = player.Skin.color;
        this.Eyes.color = player.Eyes.color;
        this.Buff.color = player.Buff.color;

        Shirt = imageShirtChoices[player.shirtChoices.IndexOf(player.Shirt)];
        Shirt.color = player.Shirt.color;
        Shirt.enabled = true;
        Pants = imagePantsChoices[player.pantsChoices.IndexOf(player.Pants)];
        Pants.color = player.Pants.color;
        Pants.enabled = true;
        Shoes = imageShoesChoices[player.shoesChoices.IndexOf(player.Shoes)];
        Shoes.color = player.Shoes.color;
        Shoes.enabled = true;
        Hair = imageHairChoices[player.hairChoices.IndexOf(player.Hair)];
        Hair.color = player.Hair.color;
        Hair.enabled = true;
    }

    private void ChangeElement(int incOrDec, ref Image elementToChange, List<Image> imageList)
    {
        Color color = elementToChange.color;
        elementToChange.color = color;
        elementToChange.enabled = false;
        int currentIndex = imageList.IndexOf(elementToChange);
        if (incOrDec > 0)
        {
            currentIndex++;
            if (currentIndex>=imageList.Count)
            {
                currentIndex = 0;
            }
        } else
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = imageList.Count-1;
            }
        }
        elementToChange = imageList[currentIndex];
        elementToChange.enabled = true;
        elementToChange.color = color;
    }

    public void NextHair()
    {
        ChangeElement(1, ref Hair, imageHairChoices);
    }
    public void NextShoes()
    {
        ChangeElement(1, ref Shoes, imageShoesChoices);
    }
    public void NextShirt()
    {
        ChangeElement(1, ref Shirt, imageShirtChoices);
    }
    public void NextPants()
    {
        ChangeElement(1, ref Pants, imagePantsChoices);
    }
    public void PrevHair()
    {
        ChangeElement(-1, ref Hair, imageHairChoices);
    }
    public void PrevShoes()
    {
        ChangeElement(-1, ref Shoes, imageShoesChoices);
    }
    public void PrevShirt()
    {
        ChangeElement(-1, ref Shirt, imageShirtChoices);
    }
    public void PrevPants()
    {
        ChangeElement(-1, ref Pants, imagePantsChoices);
    }

    public void ToggleBeard()
    {
        Beard.enabled = !Beard.enabled;
    }

    public void ToggleGlasses()
    {
        Glasses.enabled = !Glasses.enabled;
    }

    public void SavePlayer()
    {
        player.Beard.enabled = this.Beard.enabled;
        player.Beard.color = this.Beard.color;
        player.Glasses.enabled = this.Glasses.enabled;
        player.Skin.color = this.Skin.color;
        player.Eyes.color = this.Eyes.color;
        player.Buff.color = this.Buff.color;

        player.Shirt.enabled = false;
        player.Shoes.enabled = false;
        player.Hair.enabled = false;
        player.Pants.enabled = false;

        player.Shirt = player.shirtChoices[imageShirtChoices.IndexOf(Shirt)];
        player.Shirt.color = Shirt.color;
        player.Pants = player.pantsChoices[imagePantsChoices.IndexOf(Pants)];
        player.Pants.color = Pants.color;
        player.Shoes = player.shoesChoices[imageShoesChoices.IndexOf(Shoes)];
        player.Shoes.color = Shoes.color;
        player.Hair = player.hairChoices[imageHairChoices.IndexOf(Hair)];
        player.Hair.color = Hair.color;

        player.Shirt.enabled = true;
        player.Shoes.enabled = true;
        player.Hair.enabled = true;
        player.Pants.enabled = true;
    }
}
