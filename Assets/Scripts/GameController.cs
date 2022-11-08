using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject troopPrefab;
	public Transform troopParent;

	public Transform territoryParent;
	public List<Territory> territories;

	public void Start()
	{
		AssignTerritories();
	}

	public void AssignTerritories()
	{
		territories = new List<Territory>();

		for(int i = 0; i < territoryParent.childCount; i++)
		{
			territories.Add(territoryParent.GetChild(i).GetComponent<Territory>());
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
