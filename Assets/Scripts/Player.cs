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

    public int aggressiveness; // 0-9 on how willing they are to target threats other than themselves
    public int leadership; // 0-9 on how likely they are to be higher in alliance hierarchy
    public int challengeStrength;  // 0-9 on how good they are at challenges
    public int likeability;  // 0-9 on how likely they are to form good relationships
    public int read;  // 0-9 on how likely to know when they're the primary target
    public int hunting;  // 0-9 on how liekly they are to find advantages
    public int numbers;  // 0-9 on how strong they are at figuring out the likely numbers of tribal and adjusting accordingly
    public int manoeuvre;  // 0-9 on how good they are at avoiding being the target while they're in the minority

    public int socialStrength;  // 0-9 on how good they are socially
    public int strategyStrength;  // 0-9 on how good they are strategically

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
        aggressiveness = Random.Range(0, 10);
        leadership = Random.Range(0, 10);
        challengeStrength = Random.Range(0, 10);
        likeability = Random.Range(0, 10);
        read = Random.Range(0, 10);
        hunting = Random.Range(0, 10);
        numbers = Random.Range(0, 10);
        manoeuvre = Random.Range(0, 10);
        gameObject.name = GetStringFromList(PossibleNamesList);
        namePass.text = gameObject.name;
        //ToggleTorch();
        UpdateStrengths();
        RandomizeAppearance();
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
        socialStrength = Mathf.RoundToInt((leadership + likeability + read) / 3);
        strategyStrength = Mathf.RoundToInt((aggressiveness + hunting + numbers + manoeuvre) / 4);
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
