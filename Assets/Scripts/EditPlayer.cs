using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditPlayer : MonoBehaviour
{
    [SerializeField] PlayerEditImage mainPlayerImage;
    [SerializeField] TMP_InputField InputText;
    Player player;
    public void setPlayer(Player incomingPlayer)
    {
        if(incomingPlayer)
        {
            player = incomingPlayer;
            mainPlayerImage.CreatePlayerImage(player);
            InputText.text = player.name;
        }
    }

    public void CloseWindow()
    {
        player.ToggleAllPlayers(true);
        Destroy(gameObject);
    }
}
