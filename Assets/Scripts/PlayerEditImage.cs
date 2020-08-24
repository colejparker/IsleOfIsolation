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

    List<Image> imageHairChoices = new List<Image>();
    List<Image> imageShirtChoices = new List<Image>();
    List<Image> imageShoesChoices = new List<Image>();
    List<Image> imagePantsChoices = new List<Image>();

    List<SpriteRenderer> spriteHairChoices = new List<SpriteRenderer>();
    List<SpriteRenderer> spriteShirtChoices = new List<SpriteRenderer>();
    List<SpriteRenderer> spriteShoesChoices = new List<SpriteRenderer>();
    List<SpriteRenderer> spritePantsChoices = new List<SpriteRenderer>();


    public Player player;

    public void CreatePlayerImage(Player incomingPlayer)
    {
        player = incomingPlayer;

        MakeLists();

        this.Beard.enabled = player.Beard.enabled;
        this.Beard.color = player.Beard.color;
        this.Glasses.enabled = player.Glasses.enabled;
        this.Skin.color = player.Skin.color;
        this.Eyes.color = player.Eyes.color;

        Shirt = imageShirtChoices[spriteShirtChoices.IndexOf(player.Shirt)];
        Shirt.color = player.Shirt.color;
        Shirt.enabled = true;
        Pants = imagePantsChoices[spritePantsChoices.IndexOf(player.Pants)];
        Pants.color = player.Pants.color;
        Pants.enabled = true;
        Shoes = imageShoesChoices[spriteShoesChoices.IndexOf(player.Shoes)];
        Shoes.color = player.Shoes.color;
        Shoes.enabled = true;
        Hair = imageHairChoices[spriteHairChoices.IndexOf(player.Hair)];
        Hair.color = player.Hair.color;
        Hair.enabled = true;
    }

    private void MakeLists()
    {
        imageHairChoices = MakeImageListFromArray(HairChoiceParent.GetComponentsInChildren<Image>());
        imageShirtChoices = MakeImageListFromArray(ShirtChoiceParent.GetComponentsInChildren<Image>());
        imageShoesChoices = MakeImageListFromArray(ShoesChoiceParent.GetComponentsInChildren<Image>());
        imagePantsChoices = MakeImageListFromArray(PantsChoiceParent.GetComponentsInChildren<Image>());

        spriteHairChoices = MakeSpriteListFromArray(player.HairChoiceParent.GetComponentsInChildren<SpriteRenderer>());
        spriteShoesChoices = MakeSpriteListFromArray(player.ShoesChoiceParent.GetComponentsInChildren<SpriteRenderer>());
        spriteShirtChoices = MakeSpriteListFromArray(player.ShirtChoiceParent.GetComponentsInChildren<SpriteRenderer>());
        spritePantsChoices = MakeSpriteListFromArray(player.PantsChoiceParent.GetComponentsInChildren<SpriteRenderer>());
    }

    private List<Image> MakeImageListFromArray(Image[] imageArray)
    {
        List<Image> imageList = new List<Image>();
        foreach(Image image in imageArray)
        {
            imageList.Add(image);
        }
        return imageList;
    }

    private List<SpriteRenderer> MakeSpriteListFromArray(SpriteRenderer[] imageArray)
    {
        List<SpriteRenderer> imageList = new List<SpriteRenderer>();
        foreach (SpriteRenderer image in imageArray)
        {
            imageList.Add(image);
        }
        return imageList;
    }

    private void ChangeElement(int incOrDec, ref Image elementToChange, List<Image> imageList)
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
}
