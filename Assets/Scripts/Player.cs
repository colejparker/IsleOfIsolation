using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public bool inEditMode = true;

    [SerializeField] SpriteRenderer Skin;
    [SerializeField] SpriteRenderer Eyes;
    public SpriteRenderer Hair;
    [SerializeField] Transform HairChoiceParent;
    public SpriteRenderer Shirt;
    [SerializeField] Transform ShirtChoiceParent;
    public SpriteRenderer Pants;
    [SerializeField] Transform PantsChoiceParent;
    public SpriteRenderer Shoes;
    [SerializeField] Transform ShoesChoiceParent;
    [SerializeField] SpriteRenderer Buff;
    [SerializeField] ParticleSystem TorchFire;
    [SerializeField] SpriteRenderer ImmunityNecklace;
    [SerializeField] SpriteRenderer Beard;
    [SerializeField] SpriteRenderer Glasses;


    [SerializeField] SpriteRenderer hoverSquare;

    [SerializeField] GameObject editStats;
    [SerializeField] SpriteRenderer voteParchment;
    [SerializeField] TextMesh namePass;
    [SerializeField] Transform challengeBar;
    [SerializeField] Transform socialBar;
    [SerializeField] Transform strategyBar;

    [SerializeField] TextAsset HairColorsList;
    [SerializeField] TextAsset EyesColorsList;
    [SerializeField] TextAsset PossibleNamesList;

    SpriteRenderer[] hairChoices;
    SpriteRenderer[] shirtChoices;
    SpriteRenderer[] pantsChoices;
    SpriteRenderer[] shoesChoices;

    public int leadership; // 0-9 on how likely they are to be higher in alliance hierarchy
    public int likeability;  // 0-9 on how likely they are to form good relationships
    public int read;  // 0-9 on how likely to know when they're the primary target
    public int manoeuvre;  // 0-9 on how good they are at avoiding being the target while they're in the minority
    public int downplay;  // 0-9 on how good they are at avoiding being the target while they're a large threat
    public int temperament;  // 0-9 on how likely they are to recover relationships after the other has wronged them

    public int aggressiveness; // 0-9 on how willing they are to target threats other than themselves
    public int hunting;  // 0-9 on how likely they are to find advantages
    public int numbers;  // 0-9 on how strong they are at figuring out the likely numbers of tribal and adjusting accordingly
    public int loyalty; // 0-9 on how likely they are to stick with an alliance based on age of the alliance

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

    public List<Advantage> advantages = new List<Advantage>(); // the advantages currently in possession of the player

    // Start is called before the first frame update
    //When initializing a player, SetOriginalTribeColor and SetPlayerID also need to be called.
    void Start()
    {
        RandomizeStats();
        GenerateName();
        //ToggleTorch();
        UpdateStrengths();
        RandomizeAppearance();
    }

    private void GenerateName()
    {
        gameObject.name = GetStringFromList(PossibleNamesList);
        namePass.text = gameObject.name;
    }

    private void RandomizeStats()
    {
        likeability = Random.Range(0, 10);
        read = Random.Range(0, 10);
        manoeuvre = Random.Range(0, 10);
        downplay = Random.Range(0, 10);
        temperament = Random.Range(0, 10);

        aggressiveness = Random.Range(0, 10);
        hunting = Random.Range(0, 10);
        numbers = Random.Range(0, 10);
        loyalty = Random.Range(0, 10);

        strength = Random.Range(0, 10);
        if (strength > 4)
        {
            leadership = Random.Range(3, 10);
            endurance = Random.Range(3, 10);
            speed = Random.Range(3, 10);
        }
        else
        {
            endurance = Random.Range(0, 8);
            speed = Random.Range(0, 8);
            leadership = Random.Range(0, 8);
        }
        if (numbers > 4)
        {
            puzzles = Random.Range(3, 10);
        }
        else
        {
            puzzles = Random.Range(0, 7);
        }
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

    public void SetPlayerId(int playerID)
    {
        this.playerID = playerID;
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

    private void UpdateStrengths()
    {
        socialStrength = Mathf.RoundToInt((leadership + likeability + read + manoeuvre + downplay + temperament) /6);
        strategyStrength = Mathf.RoundToInt((aggressiveness + hunting + numbers + loyalty) / 4);
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
        hairChoices = HairChoiceParent.GetComponentsInChildren<SpriteRenderer>();
        shirtChoices = ShirtChoiceParent.GetComponentsInChildren<SpriteRenderer>();
        pantsChoices = PantsChoiceParent.GetComponentsInChildren<SpriteRenderer>();
        shoesChoices = ShoesChoiceParent.GetComponentsInChildren<SpriteRenderer>();

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

    private SpriteRenderer ChooseSprite(SpriteRenderer[] options)
    {
        return options[Mathf.RoundToInt(Random.Range(0, options.Length))];
    }

    private void DisableSpriteChoices(SpriteRenderer[] options)
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
        if(inEditMode) print(gameObject.name);
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

}
