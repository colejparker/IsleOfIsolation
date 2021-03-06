﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public bool inEditMode = true;

    public SpriteRenderer Skin;
    public SpriteRenderer Eyes;
    public SpriteRenderer Hair;
    public Transform HairChoiceParent;
    public SpriteRenderer Shirt;
    public Transform ShirtChoiceParent;
    public SpriteRenderer Pants;
    public Transform PantsChoiceParent;
    public SpriteRenderer Shoes;
    public Transform ShoesChoiceParent;
    public SpriteRenderer Buff;
    [SerializeField] ParticleSystem TorchFire;
    [SerializeField] SpriteRenderer ImmunityNecklace;
    public SpriteRenderer Beard;
    public SpriteRenderer Glasses;

    [SerializeField] EditPlayer editPlayerPrefab;

    [SerializeField] SpriteRenderer hoverSquare;

    [SerializeField] GameObject editStats;
    [SerializeField] SpriteRenderer voteParchment;
    public TextMesh namePass;
    public Transform challengeBar;
    public Transform socialBar;
    public Transform strategyBar;

    [SerializeField] TextAsset HairColorsList;
    [SerializeField] TextAsset EyesColorsList;
    [SerializeField] TextAsset PossibleNamesList;

    public List<SpriteRenderer> hairChoices;
    public List<SpriteRenderer> shirtChoices;
    public List<SpriteRenderer> pantsChoices;
    public List<SpriteRenderer> shoesChoices;

    public int leadership; // 0-9 on how likely they are to be higher in alliance hierarchy
    public int likeability;  // 0-9 on how likely they are to form good relationships
    public int read;  // 0-9 on how likely to know when they're the primary target
    public int loyalty; // 0-9 on how likely they are to stick with an alliance based on age of the alliance
    public int temperament;  // 0-9 on how likely they are to recover relationships after the other has wronged them

    public int aggressiveness; // 0-9 on how willing they are to target threats other than themselves
    public int hunting;  // 0-9 on how likely they are to find advantages
    public int numbers;  // 0-9 on how strong they are at figuring out the likely numbers of tribal and adjusting accordingly
    public int scramble;  // 0-9 on how good they are at avoiding being the target while they're in the minority
    public int downplay;  // 0-9 on how good they are at avoiding being the target while they're a large threat

    public int strength;  // 0-9 on how good they are at strength challenges
    public int endurance;  // 0-9 on how good they are at endurance challenges
    public int puzzles;  // 0-9 on how good they are at puzzle challenges
    public int speed;  // 0-9 on how fast they are on land and sea

    public int socialStrength;  // 0-9 on how good they are socially
    public int strategyStrength;  // 0-9 on how good they are strategically
    public int challengeStrength;  // 0-9 on how good they are at challenges

    public int threatLevel;  // 0-9+ on how threatening they currently are in the game. starts as average of strengths but can go higher or lower

    public int playerID;  // unique numerical identifier
    public List<Tuple<Player, Player, int>> relationships = new List<Tuple<Player, Player, int>>();  // 0-9 on how good their relationship is with another player;
    public bool isImmune = false;  // boolean on if they are immune or not in tribal

    int votes = 1; // how many votes they have at a tribal council

    EditTribeTitle[] editTribeTitles;

    public List<Advantage> advantages = new List<Advantage>(); // the advantages currently in possession of the player

    // Start is called before the first frame update
    //When initializing a player, SetOriginalTribeColor and SetPlayerID also need to be called.
    void Start()
    {
        
    }

    public Player createPlayer(int playerID)
    {
        this.playerID = playerID;
        RandomizeStats();
        GenerateName();
        //ToggleTorch();
        UpdateStrengths();
        RandomizeAppearance();
        return this;
    }

    private void GenerateName()
    {
        gameObject.name = GetStringFromList(PossibleNamesList);
        namePass.text = gameObject.name;
    }

    private void RandomizeStats()
    {
        likeability = RandomBetweenTwo(0, 11);
        read = RandomBetweenTwo(0, 11);
        scramble = RandomBetweenTwo(0, 11);
        downplay = RandomBetweenTwo(0, 11);
        temperament = RandomBetweenTwo(0, 11);

        aggressiveness = RandomBetweenTwo(0, 11);
        hunting = RandomBetweenTwo(0, 11);
        numbers = RandomBetweenTwo(0, 11);
        loyalty = RandomBetweenTwo(0, 11);

        strength = RandomBetweenTwo(0, 11);
        if (strength > 5)
        {
            leadership = RandomBetweenTwo(3, 11);
            endurance = RandomBetweenTwo(3, 11);
            speed = RandomBetweenTwo(3, 11);
        }
        else
        {
            endurance = RandomBetweenTwo(0, 8);
            speed = RandomBetweenTwo(0, 8);
            leadership = RandomBetweenTwo(0, 8);
        }
        if (numbers > 5)
        {
            puzzles = RandomBetweenTwo(3, 11);
        }
        else
        {
            puzzles = RandomBetweenTwo(0, 7);
        }
    }

    public int RandomBetweenTwo(int x, int y)
    {
        return Mathf.Clamp(Mathf.RoundToInt((Random.Range(x, y) + Random.Range(x, y)) / 2f), 0, 9);
    }

    public void ToggleStatsView()
    {
        editStats.SetActive(!editStats.activeSelf);
    }

    private void ToggleTorch()
    {
        ParticleSystem.EmissionModule emission = TorchFire.emission;
        emission.enabled = !emission.enabled;
    }

    public void SetOriginalTribeColor(Color color)
    {
        SetNewTribeColor(color);
        if (Shirt) Shirt.color = GenerateCloseColor(color);
        if (Pants) Pants.color = GenerateCloseColor(color);
    }


    private Color GenerateCloseColor(Color color)
    {
        float newRed = Mathf.Clamp((color.r + Random.Range(-0.5f, 0.5f)), 0, 1);
        float newGreen = Mathf.Clamp((color.g + Random.Range(-0.5f, 0.5f)), 0, 1);
        float newBlue = Mathf.Clamp((color.b + Random.Range(-0.5f, 0.5f)), 0, 1);
        return new Color(newRed, newGreen, newBlue);
    }

    public void SetNewTribeColor(Color color)
    {
        Buff.color = color;
    }

    public void UpdateStrengths()
    {
        socialStrength = Mathf.RoundToInt((leadership + likeability + read + loyalty + temperament) /5);
        strategyStrength = Mathf.RoundToInt((aggressiveness + hunting + numbers + scramble + downplay) / 5);
        challengeStrength = Mathf.RoundToInt((strength + endurance + puzzles + speed) / 4);
        threatLevel = Mathf.RoundToInt((socialStrength + strategyStrength + challengeStrength) / 3);

        UpdateBars();
    }

    private void UpdateBars()
    {
        UpdateBar(challengeBar.GetComponentsInChildren<SpriteRenderer>(), challengeStrength);
        UpdateBar(socialBar.GetComponentsInChildren<SpriteRenderer>(), socialStrength);
        UpdateBar(strategyBar.GetComponentsInChildren<SpriteRenderer>(), strategyStrength);

    }

    private void UpdateBar(SpriteRenderer[] boxes, int strength)
    {
        int counter = 0;
        foreach (SpriteRenderer sr in boxes)
        {
            sr.enabled = counter < strength;
            counter++;
        }
    }

    private void RandomizeAppearance()
    {
        int beardNumber = Random.Range(0, 10);
        int glassesNumber = Random.Range(0, 10);

        if (beardNumber < 2)
        {
            Beard.enabled = true;
        } else
        {
            Beard.enabled = false;
        }

        if (glassesNumber < 1)
        {
            Glasses.enabled = true;
        }
        else
        {
            Glasses.enabled = false;
        }

        Hair = ChooseSprite(hairChoices);
        Shirt = ChooseSprite(shirtChoices);
        Pants = ChooseSprite(pantsChoices);
        Shoes = ChooseSprite(shoesChoices);

        SetOriginalTribeColor(Buff.color);

        DisableSpriteChoices(hairChoices);
        DisableSpriteChoices(shirtChoices);
        DisableSpriteChoices(pantsChoices);
        DisableSpriteChoices(shoesChoices);

        Hair.enabled = true;
        Shirt.enabled = true;
        Pants.enabled = true;
        Shoes.enabled = true;

        Color color;
        if(ColorUtility.TryParseHtmlString(GetStringFromList(HairColorsList), out color))
        {
            Hair.color = color;
            Beard.color = color;
        }
        if (ColorUtility.TryParseHtmlString(GetStringFromList(EyesColorsList), out color))
        {
            Eyes.color = color;
        }

        float skinColor = Random.Range(0.34f, 1);
        Skin.color = new Color(skinColor, skinColor, skinColor);
    }

    private SpriteRenderer ChooseSprite(List<SpriteRenderer> options)
    {
        return options[Mathf.RoundToInt(Random.Range(0, options.Count))];
    }

    private void DisableSpriteChoices(List<SpriteRenderer> options)
    {
        foreach (SpriteRenderer sr in options)
        {
            sr.enabled = false;
        }
    }

    private string GetStringFromList(TextAsset list)
    {
        char[] archDelim = new char[] { '\r', '\n' };
        string[] options = list.text.Split(archDelim, StringSplitOptions.RemoveEmptyEntries);
        return options[Mathf.RoundToInt(Random.Range(0, options.Length))];
    }

    // Update is called once per frame
    void Update()
    {
        ImmunityNecklace.enabled = isImmune;
    }

    private void OnMouseDown()
    {
        editTribeTitles = FindObjectsOfType<EditTribeTitle>();
        foreach (EditTribeTitle edt in editTribeTitles)
        {
            edt.gameObject.SetActive(false);
        }
        if (inEditMode)
        {
            hoverSquare.enabled = false;
            EditPlayer newEP = Instantiate(editPlayerPrefab, Vector3.zero, Quaternion.identity);
            newEP.setPlayer(this);
            ToggleAllPlayers(false);
        }

    }

    private void OnMouseEnter()
    {
        if (inEditMode)
        {
            hoverSquare.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        if (inEditMode)
        {
            hoverSquare.enabled = false;
        }
    }

    public void addRelationship(Tuple<Player, Player, int> relationship)
    {
        this.relationships.Add(relationship);
    }

    public void ToggleAllPlayers(bool boolValue)
    {
        foreach(Tuple<Player, Player, int> tuple in relationships)
        {
            tuple.Item2.gameObject.SetActive(boolValue);
        }
        this.gameObject.SetActive(boolValue);
        if(boolValue)
        {
            foreach (EditTribeTitle edt in editTribeTitles)
            {
                edt.gameObject.SetActive(true);
            }
        }
    }

    public void AllPlayersEditToggle(bool boolValue)
    {
        foreach (Tuple<Player, Player, int> tuple in relationships)
        {
            tuple.Item2.inEditMode = boolValue;
        }
        this.inEditMode = boolValue;
    }

}
