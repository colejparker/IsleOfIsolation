using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public int numberOfPlayers;
    public int numberOfTribes;

    [SerializeField] GameInitializer gameInitializer;
    [SerializeField] Slider tribeSlider;
    [SerializeField] Slider playerSlider;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI tribeText;

    int[] allowedTwoValues = new int[] { 16, 18, 20 };
    int[] allowedThreeValues = new int[] { 15, 18 };

    public void PlayGame()
    {
        gameInitializer.SortTribeLists();
        gameInitializer.StartGame(numberOfTribes, numberOfPlayers);
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        numberOfPlayers = Mathf.RoundToInt(playerSlider.value);
        numberOfTribes = Mathf.RoundToInt(tribeSlider.value);
        UpdateText();
    }

    public void onTribeSlide()
    {
        numberOfTribes = Mathf.RoundToInt(tribeSlider.value);
        if (numberOfTribes == 2)
        {
            playerSlider.minValue = 16;
            playerSlider.maxValue = 20;
            playerSlider.value = 16;
        } else
        {
            playerSlider.minValue = 15;
            playerSlider.maxValue = 18;
            playerSlider.value = 15;
        }
        numberOfPlayers = Mathf.RoundToInt(playerSlider.value);
        UpdateText();
    }

    public void onPlayerSlide()
    {
        if (numberOfTribes == 2)
        {
            if (!(playerSlider.value % 2 == 0)) {
                playerSlider.value = numberOfPlayers;
            } else
            {
                numberOfPlayers = Mathf.RoundToInt(playerSlider.value);
            }
        } else
        {
            if (!(playerSlider.value % 3 == 0))
            {
                playerSlider.value = numberOfPlayers;
            }
            else
            {
                numberOfPlayers = Mathf.RoundToInt(playerSlider.value);
            }
        }
        UpdateText();
    }

    void UpdateText()
    {
        playerText.text = "Number of Players: " + numberOfPlayers.ToString();
        tribeText.text = "Number of Tribes: " + numberOfTribes.ToString();

    }


}
