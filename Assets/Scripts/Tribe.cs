using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tribe : MonoBehaviour
{
    public List<Player> members = new List<Player>();
    public string tribeName;
    public Color tribeColor;

    public void constructTribe(string tribeName)
    {
        this.tribeName = tribeName;
        gameObject.name = tribeName;
    }

    public void AddMember(Player player)
    {
        members.Add(player);
        player.SetOriginalTribeColor(tribeColor);
    }

    public void setTribeColor(Color color)
    {
        this.tribeColor = color;
        foreach (Player player in members)
        {
            player.SetNewTribeColor(this.tribeColor);
        }
    }
}
