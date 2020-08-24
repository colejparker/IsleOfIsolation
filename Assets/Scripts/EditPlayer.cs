using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPlayer : MonoBehaviour
{
    [SerializeField] PlayerEditImage mainPlayerImage;
    [SerializeField] TMP_InputField InputText;
    Player player;
    [SerializeField] Image hairColorImage;
    public void setPlayer(Player incomingPlayer)
    {
        if(incomingPlayer)
        {
            player = incomingPlayer;
            mainPlayerImage.CreatePlayerImage(player);
            hairColorImage.color = player.Hair.color;
            InputText.text = player.name;
        }
    }

    public void CloseWindow()
    {
        player.ToggleAllPlayers(true);
        Destroy(gameObject);
    }

    public void SavePlayer()
    {
        mainPlayerImage.SavePlayer();
        player.name = InputText.text;
        player.namePass.text = InputText.text;
        CloseWindow();
    }
}
