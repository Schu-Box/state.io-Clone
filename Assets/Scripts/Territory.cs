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
	}
}
