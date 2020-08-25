using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipPlayerSlider : MonoBehaviour
{
    [SerializeField] StatsSlider relSlider;
    [SerializeField] PlayerEditImage relPlayerImage;
    public Player relPlayer;

    public void CreateFromPlayer(Player incomingPlayer)
    {
        gameObject.name = incomingPlayer.gameObject.name + " Relationship";
        relPlayer = incomingPlayer;
        relSlider.startString = incomingPlayer.gameObject.name;
        relPlayerImage.CreatePlayerImage(relPlayer);
    }

    // Update is called once per frame
    public void UpdateSlider(int value)
    {
        relSlider.slider.value = value;
    }

    public int GetSliderValue()
    {
        return Mathf.RoundToInt(relSlider.slider.value);
    }
}
