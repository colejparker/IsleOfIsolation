using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] TextAsset TribeColorsTextList;
    [SerializeField] TextAsset TribeNamesTextList;

    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject TribeObject;

    List<string> nameList = new List<string>();
    List<Color> colorList = new List<Color>();

    public List<Player> playersStartingGame = new List<Player>();
    public List<Tribe> tribes = new List<Tribe>();
    public List<Tuple<Player, Player, int>> relationshipsOfPlayers = new List<Tuple<Player, Player, int>>();

    [SerializeField] GameObjectContainer playerContainer;
    [SerializeField] GameObjectContainer tribeContainer;

    [SerializeField] AllPlayer allPlayer;

    public void Start()
    {
        SortTribeLists();
    }

    public void SortTribeLists()
    {
        colorList.Clear();
        nameList.Clear();
        char[] archDelim = new char[] { '\r', '\n' };
        string[] colorStringArray = GenerateStringArrayFromList(TribeColorsTextList);
        string[] nameStringArray = GenerateStringArrayFromList(TribeNamesTextList);
        foreach(string colorString in colorStringArray)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(colorString, out color))
            {
                colorList.Add(color);
            }
        }
        foreach (string nameString in nameStringArray)
        {
            nameList.Add(nameString);
        }
    }

    private string[] GenerateStringArrayFromList(TextAsset list)
    {
        char[] archDelim = new char[] { '\r', '\n' };
        return list.text.Split(archDelim, StringSplitOptions.RemoveEmptyEntries);
    }

    public void StartGame(int numberOfTribes, int numberOfPlayers)
    {
        this.playersStartingGame = createPlayers(numberOfPlayers);
        this.tribes = initializeTribes(numberOfTribes, this.playersStartingGame);
        this.relationshipsOfPlayers = generateRelationships(this.playersStartingGame);
        allPlayer.InitializeGame(tribes);
    }

    public List<Player> createPlayers(int numberOfPlayers)
    {
        List<Player> players = new List<Player>();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players.Add(generatePlayer(i));
        }
        return players;
    }

    public Player generatePlayer(int playerIndex)
    {
        GameObject newPO = Instantiate(PlayerObject, transform.localPosition, Quaternion.identity);
        newPO.transform.parent = playerContainer.transform;
        Player player = newPO.GetComponent<Player>();
        player = player.createPlayer(playerIndex);
        return player;
    }

    public List<Tribe> initializeTribes(int numberOfTribes, List<Player> players)
    {
        List<Tribe> tribes = new List<Tribe>();
        List<Player> strongPlayers = new List<Player>();
        List<Player> mediumPlayers = new List<Player>();
        List<Player> weakPlayers = new List<Player>();
        int numberOfPlayers = players.Count;
        foreach (Player p in players)
        {
            int currentStrength = p.challengeStrength;
            if (currentStrength >= 7)
            {
                strongPlayers.Add(p);
            }
            else if (currentStrength >= 4)
            {
                mediumPlayers.Add(p);
            }
            else
            {
                weakPlayers.Add(p);
            }
        }
        int[] tribeDivisionArray = new int[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            tribeDivisionArray[i] = i % numberOfTribes;
        }
        int j = 0;
        List<List<Player>> playersSplit = new List<List<Player>>();
        for (int l = 0; l < numberOfTribes; l++)
        {
            playersSplit.Add(new List<Player>());
        }
        foreach (Player sp in strongPlayers)
        {
            playersSplit[tribeDivisionArray[j]].Add(sp);
            j++;
        }
        foreach (Player mp in mediumPlayers)
        {
            playersSplit[tribeDivisionArray[j]].Add(mp);
            j++;
        }
        foreach (Player wp in weakPlayers)
        {
            playersSplit[tribeDivisionArray[j]].Add(wp);
            j++;
        }
        for (int k = 0; k < numberOfTribes; k++)
        {
            GameObject newTO = Instantiate(TribeObject, transform.localPosition, Quaternion.identity);
            newTO.transform.parent = tribeContainer.transform;
            Tribe tribe = newTO.GetComponent<Tribe>();
            tribe.constructTribe(generateTribeName());
            tribe.setTribeColor(generateTribeColor());
            foreach (Player p in playersSplit[k])
            {
                tribe.AddMember(p);
            }
            tribes.Add(tribe);
        }
        return tribes;
    }

    private string generateTribeName()
    {
        int index = Random.Range(0, this.nameList.Count - 1);
        string tribeName = this.nameList[index];
        this.nameList.Remove(tribeName);
        return tribeName;
    }

    private Color generateTribeColor()
    {
        int index = Random.Range(0, this.colorList.Count - 1);
        Color tribeColor = this.colorList[index];
        this.colorList.Remove(tribeColor);
        return tribeColor;
    }

    private List<Tuple<Player, Player, int>> generateRelationships(List<Player> players)
    {
        List<Tuple<Player, Player, int>> listOfRelationships = new List<Tuple<Player, Player, int>>();
        int playerCounter = 0;
        int numberOfPlayers = players.Count;
        foreach (Player player in players)
        {
            int partnerCounter = 0;
            foreach (Player partner in players)
            {
                //to redo counting in likeability
                if (partnerCounter > playerCounter)
                {
                    int strength = Mathf.Clamp(Mathf.RoundToInt(Random.Range(partner.likeability - 2, partner.likeability + 2)), 2, 7);
                    Tuple<Player, Player, int> playerRel = new Tuple<Player, Player, int>(player, partner, strength);
                    int partnerStrength = Mathf.Clamp(Mathf.RoundToInt(Random.Range(player.likeability - 2, player.likeability + 2)), 2, 7);
                    Tuple<Player, Player, int> partnerRel = new Tuple<Player, Player, int>(partner, player, partnerStrength);
                    listOfRelationships.Add(playerRel);
                    player.addRelationship(playerRel);
                    listOfRelationships.Add(partnerRel);
                    partner.addRelationship(partnerRel);
                }
                partnerCounter++;
            }
            playerCounter++;
        }
        return listOfRelationships;
    }
}
