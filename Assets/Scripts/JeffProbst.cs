using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeffProbst : MonoBehaviour
{
    public List<Player> playersStartingGame;
    public List<Player> playersInGame;
    public List<Player> jury;
    public List<Player> preJuryBoots;
    public List<Tribe> startingTribes;
    public List<Tribe> tribesInGame;

    [SerializeField] float yMaximum = 15.0f;
    [SerializeField] float ySpacing = 6.3f;
    [SerializeField] float xMinimum = -25.0f;
    [SerializeField] float xSpacing = 12.5f;
    // Start is called before the first frame update
    void Start()
    {
        playersInGame = playersStartingGame = ArrayToList(FindObjectsOfType<Player>());
        startingTribes = tribesInGame = ArrayToList(FindObjectsOfType<Tribe>());
        foreach(Player player in playersStartingGame)
        {
            player.challengeBar.gameObject.SetActive(false);
            player.socialBar.gameObject.SetActive(false);
            player.strategyBar.gameObject.SetActive(false);
            player.inEditMode = false;
        }
        RearrangePlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<T> ArrayToList<T>(T[] objects)
    {
        List<T> objectList = new List<T>();
        foreach (T o in objects)
        {
            objectList.Add(o);
        }
        return objectList;
    }

    private void RearrangePlayers()
    {
        int xCounter = 0;
        int yCounter = 0;
        foreach(Tribe tribe in tribesInGame)
        {
            foreach (Player player in tribe.members)
            {
                if (xCounter >= 5)
                {
                    xCounter = 0;
                    yCounter++;
                }
                player.transform.position = new Vector3(xMinimum + (xSpacing * xCounter), yMaximum - (ySpacing * yCounter));
                xCounter++;
            }
            xCounter = 0;
            yCounter++;
        }
    }
}
