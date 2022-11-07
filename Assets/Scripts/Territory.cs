using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Territory : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI troopCounterText;

    public Team teamController;

    public int troops = 10;
    private int troopMax = 50;
    private int troopMax_Uncontrolled = 10;

    public float troopIncreaseFrequency = 1f;
    private float troopIncreaseTimer;

    private void Start()
    {
        troopIncreaseTimer = troopIncreaseFrequency;

		UpdateTroopCounter();
    }

    private void Update()
    {
        if (CanIncreaseTroops())
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

    public bool CanIncreaseTroops()
    {
        if (teamController != null)
        {
            if (troops < troopMax)
            {
                return true;
            }
        }
        else
        {
            if (troops < troopMax_Uncontrolled)
            {
                return true;
            }
        }

        return false;
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
        if (teamController != null)
		{
            minimumControlColor = Color.Lerp(teamController.teamColor, Color.white, 0.7f); //Minimum color will be nearly white, but still retaining some of the team's color
			maximumControlColor = teamController.teamColor;
		}

        float percentTowardsMaxControl = (float)troops / (float)troopMax;
        Color newTerritoryColor = Color.Lerp(minimumControlColor, maximumControlColor, percentTowardsMaxControl);

        fillImage.color = newTerritoryColor;
    }
}
