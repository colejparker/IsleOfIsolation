using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPlayer : MonoBehaviour
{
    [SerializeField] PlayerEditImage mainPlayerImage;
    Player player;
    public void setPlayer(Player incomingPlayer)
    {
        if(incomingPlayer)
        {
            player = incomingPlayer;
            mainPlayerImage.CreatePlayerImage(incomingPlayer);
        }
    }
}
