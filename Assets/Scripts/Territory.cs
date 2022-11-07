using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Territory : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI troopCounterText;

    public Team teamOccupier;

    public int troops = 10;
    private int troopMax = 50;

    public float troopIncreaseFrequency = 1f;
    private float troopIncreaseTimer;

    private void Start()
    {
        troopIncreaseTimer = troopIncreaseFrequency;

		UpdateTroopCounter();
    }

    private void Update()
    {
        if(troops < troopMax) //If this territory isn't at max troop count
        {
			troopIncreaseTimer -= Time.deltaTime;
			if (troopIncreaseTimer <= 0)
			{
                troopIncreaseTimer = troopIncreaseFrequency;

                troops++;

				UpdateTroopCounter();
			}
		}
    }

    public void UpdateTroopCounter()
	{
		troopCounterText.text = troops.ToString();

        UpdateTerritoryVisuals();
	}

    public void UpdateTerritoryVisuals()
    {
        Color minimumControlColor = Color.white;
        Color maximumControlColor = Color.grey;
        if (teamOccupier != null)
		{
            minimumControlColor = Color.Lerp(teamOccupier.teamColor, Color.white, 0.7f); //Minimum color will be nearly white, but still retaining some of the team's color
			maximumControlColor = teamOccupier.teamColor;
		}

        float percentTowardsMaxControl = (float)troops / (float)troopMax;
        Color newTerritoryColor = Color.Lerp(minimumControlColor, maximumControlColor, percentTowardsMaxControl);

        fillImage.color = newTerritoryColor;
    }
}
