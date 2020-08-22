using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllPlayer : MonoBehaviour
{
    [SerializeField] Transform twoTribesCanvas;
    [SerializeField] Transform threeTribesCanvas;
    // Start is called before the first frame update

    [SerializeField] float[] twoTribesRows = new float[] { 13.3f, 3f, -7f, -15f };
    [SerializeField] float[] threeTribesRows = new float[] {10.5f, 2.5f, -14.5f };

    [SerializeField] float maximumX = 25f;

    int playersPerTribe;
    List<Tribe> tribes = new List<Tribe>();

    public void InitializeGame(List<Tribe> incomingTribes)
    {
        this.tribes = incomingTribes;
        playersPerTribe = tribes[0].members.Count;
        float[] rowYValues;
        int rowCounter = 0;
        if (tribes.Count == 2)
        {
            twoTribesCanvas.gameObject.SetActive(true);
            threeTribesCanvas.gameObject.SetActive(false);
            NameTribes(twoTribesCanvas);
            rowYValues = twoTribesRows;
            foreach (Tribe tribe in tribes)
            {
                if (playersPerTribe == 10)
                {
                    SortRow(tribe.members.GetRange(0, 5), rowYValues[rowCounter]);
                    rowCounter++;
                    SortRow(tribe.members.GetRange(5, 5), rowYValues[rowCounter]);
                    rowCounter++;
                }
                else if (playersPerTribe == 9)
                {
                    SortRow(tribe.members.GetRange(0, 5), rowYValues[rowCounter]);
                    rowCounter++;
                    SortRow(tribe.members.GetRange(5, 4), rowYValues[rowCounter]);
                    rowCounter++;
                }
                else
                {
                    SortRow(tribe.members.GetRange(0, 4), rowYValues[rowCounter]);
                    rowCounter++;
                    SortRow(tribe.members.GetRange(4, 4), rowYValues[rowCounter]);
                    rowCounter++;
                }
            }
        }
        else
        {
            twoTribesCanvas.gameObject.SetActive(false);
            threeTribesCanvas.gameObject.SetActive(true);
            NameTribes(threeTribesCanvas);
            rowYValues = threeTribesRows;
            foreach (Tribe tribe in tribes)
            {
                SortRow(tribe.members, rowYValues[rowCounter]);
                rowCounter++;
            }
        }
    }

    private void SortRow(List<Player> playersInRow, float yValue)
    {
        int counter = 0;
        float startingX = -maximumX;
        float stepX = 0f;
        if (playersInRow.Count == 6)
        {
            stepX = maximumX * 2f / 5f;
        }
        else if (playersInRow.Count == 5)
        {
            stepX = maximumX * 2f / 4f;

        }
        else if (playersInRow.Count == 4)
        {
            stepX = maximumX * 2f / 4f;
            startingX = startingX + (stepX / 2f);
        }
        foreach (Player p in playersInRow)
        {
            p.transform.localPosition = new Vector3(startingX + (stepX * counter), yValue);
            counter++;
        }
    }


    private void NameTribes(Transform canvas)
    {
        int counter = 0;
        foreach (TextMesh text in canvas.GetComponentsInChildren<TextMesh>())
        {
            text.text = tribes[counter].name;
            counter++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
