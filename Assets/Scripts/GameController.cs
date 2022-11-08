using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject troopCounterPrefab;
	public Transform troopCounterParent;

	public GameObject troopPrefab;
	public Transform troopParent;

	public Transform territoryParent;
	public List<Territory> territories;

	public DragIndicator dragIndicator;

	public void Start()
	{
		SetupTerritories();
	}

	public void SetupTerritories()
	{
		territories = new List<Territory>();

		for(int i = 0; i < territoryParent.childCount; i++)
		{
			Territory territory = territoryParent.GetChild(i).GetComponent<Territory>();
			territories.Add(territory);

			GameObject newTroopCounter = Instantiate(troopCounterPrefab, territory.transform.position, Quaternion.identity, troopCounterParent);
			territory.troopCounterText = newTroopCounter.GetComponentInChildren<TextMeshProUGUI>();

			territory.UpdateTerritoryVisuals();
		}
	}

	public List<Territory> GetControlledTerritories(Team team)
	{
		List<Territory> controlledTerritories = new List<Territory>();

		for(int i = 0; i < territories.Count; i++)
		{
			if (territories[i].teamController == team)
			{
				controlledTerritories.Add(territories[i]);
			}
		}

		return controlledTerritories;
	}
}
