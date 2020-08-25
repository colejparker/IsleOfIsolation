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

    [SerializeField] Slider leadership; // 0-9 on how likely they are to be higher in alliance hierarchy
    [SerializeField] Slider likeability;  // 0-9 on how likely they are to form good relationships
    [SerializeField] Slider read;  // 0-9 on how likely to know when they're the primary target
    [SerializeField] Slider loyalty; // 0-9 on how likely they are to stick with an alliance based on age of the alliance
    [SerializeField] Slider temperament;  // 0-9 on how likely they are to recover relationships after the other has wronged them

    [SerializeField] Slider aggressiveness; // 0-9 on how willing they are to target threats other than themselves
    [SerializeField] Slider hunting;  // 0-9 on how likely they are to find advantages
    [SerializeField] Slider numbers;  // 0-9 on how strong they are at figuring out the likely numbers of tribal and adjusting accordingly
    [SerializeField] Slider scramble;  // 0-9 on how good they are at avoiding being the target while they're in the minority
    [SerializeField] Slider downplay;  // 0-9 on how good they are at avoiding being the target while they're a large threat

    [SerializeField] Slider strength;  // 0-9 on how good they are at strength challenges
    [SerializeField] Slider endurance;  // 0-9 on how good they are at endurance challenges
    [SerializeField] Slider puzzles;  // 0-9 on how good they are at puzzle challenges
    [SerializeField] Slider speed;  // 0-9 on how fast they are on land and sea

    bool showingStats = true;

    [SerializeField] Transform statsParent;
    [SerializeField] Transform relationshipsParent;


    public void setPlayer(Player incomingPlayer)
    {
        if(incomingPlayer)
        {
            player = incomingPlayer;
            mainPlayerImage.CreatePlayerImage(player);
            hairColorImage.color = player.Hair.color;
            InputText.text = player.name;
            UpdateSliders();
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
        UpdatePlayerFromSliders();
        CloseWindow();
    }

    private void UpdateSliders()
    {
        leadership.value = player.leadership;
        likeability.value = player.likeability;
        read.value = player.read;
        loyalty.value = player.loyalty;
        temperament.value = player.temperament;

        aggressiveness.value = player.aggressiveness;
        hunting.value = player.hunting;
        numbers.value = player.numbers;
        scramble.value = player.scramble;
        downplay.value = player.downplay;

        strength.value = player.strength;
        endurance.value = player.endurance;
        puzzles.value = player.puzzles;
        speed.value = player.speed;
    }

    private void UpdatePlayerFromSliders()
    {
        player.leadership = Mathf.RoundToInt(leadership.value);
        player.likeability = Mathf.RoundToInt(likeability.value);
        player.read = Mathf.RoundToInt(read.value);
        player.loyalty = Mathf.RoundToInt(loyalty.value);
        player.temperament = Mathf.RoundToInt(temperament.value);

        player.aggressiveness = Mathf.RoundToInt(aggressiveness.value);
        player.hunting = Mathf.RoundToInt(hunting.value);
        player.numbers = Mathf.RoundToInt(numbers.value);
        player.scramble = Mathf.RoundToInt(scramble.value);
        player.downplay = Mathf.RoundToInt(downplay.value);

        player.strength = Mathf.RoundToInt(strength.value);
        player.endurance = Mathf.RoundToInt(endurance.value);
        player.puzzles = Mathf.RoundToInt(puzzles.value);
        player.speed = Mathf.RoundToInt(speed.value);

        player.UpdateStrengths();
    }

    public void RelOrStatsClick()
    {
        showingStats = !showingStats;
        statsParent.gameObject.SetActive(showingStats);
        relationshipsParent.gameObject.SetActive(!showingStats);
    }
}
