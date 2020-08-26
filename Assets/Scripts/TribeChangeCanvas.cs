using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TribeChangeCanvas : MonoBehaviour
{
    public Tribe tribe;

    [SerializeField] ColorImage colorImage;
    [SerializeField] TMP_InputField InputText;
    EditTribeTitle[] editTribeTitles;
    EditTribeTitle passedTitle;

    public void CreateFromTribe(Tribe incomingTribe, EditTribeTitle incomingTitle)
    {
        FindObjectOfType<Player>().AllPlayersEditToggle(false);
        this.passedTitle = incomingTitle;
        editTribeTitles = FindObjectsOfType<EditTribeTitle>();
        foreach (EditTribeTitle edt in editTribeTitles)
        {
            edt.gameObject.SetActive(false);
        }
        tribe = incomingTribe;
        colorImage.TribeColor(tribe);
        InputText.text = tribe.name;
    }

    public void SaveTribe()
    {
        this.passedTitle.text.text = InputText.text;
        tribe.tribeName = InputText.text;
        tribe.gameObject.name = InputText.text;
        tribe.EditTribeColor(colorImage.image.color);
        CloseWindow();
    }

    public void CloseWindow()
    {
        foreach (EditTribeTitle edt in editTribeTitles)
        {
            edt.gameObject.SetActive(true);
        }
        FindObjectOfType<Player>().AllPlayersEditToggle(true);
        Destroy(gameObject);
    }
}
