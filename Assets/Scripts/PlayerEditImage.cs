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
    [SerializeField] Image Beard;
    [SerializeField] Image Glasses;

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

    private void ChangeElement(int incOrDec, Image elementToChange, List<Image> imageList)
    {
        Color color = elementToChange.color;
        elementToChange.color = color;
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
        elementToChange.color = color;
    }

    public void NextHair()
    {
        ChangeElement(1, Hair, imageHairChoices);
    }
    public void NextShoes()
    {
        ChangeElement(1, Shoes, imageShoesChoices);
    }
    public void NextShirt()
    {
        ChangeElement(1, Shirt, imageShirtChoices);
    }
    public void NextPants()
    {
        ChangeElement(1, Pants, imagePantsChoices);
    }
    public void PrevHair()
    {
        ChangeElement(-1, Hair, imageHairChoices);
    }
    public void PrevShoes()
    {
        ChangeElement(-1, Shoes, imageShoesChoices);
    }
    public void PrevShirt()
    {
        ChangeElement(-1, Shirt, imageShirtChoices);
    }
    public void PrevPants()
    {
        ChangeElement(-1, Pants, imagePantsChoices);
    }
}
