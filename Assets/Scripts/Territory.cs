using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class Territory : MonoBehaviour, IDragHandler, IDropHandler
{
    public Image fillImage;
    public TextMeshProUGUI troopCounterText;

    public Team teamController;

    public int troops = 10;
    private int troopMax = 50;
    private int troopMax_Uncontrolled = 10;

    public float troopIncreaseFrequency = 1f;
    private float troopIncreaseTimer;

    public float troopInvasionFrequency = 0.2f;
    public float troopInvasionTimer;

    private GameController gameController;
	private Territory activeInvasionTarget;

	private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        troopIncreaseTimer = troopIncreaseFrequency;
        troopInvasionTimer = troopInvasionFrequency;

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

        if(activeInvasionTarget != null)
        {
            troopInvasionTimer -= Time.deltaTime;
            if(troopInvasionTimer <= 0)
            {
                troopInvasionTimer = troopInvasionFrequency;

                SendTroop();

                UpdateTroopCounter();

                if(troops == 0) //If the last troop has been sent, end the invasion
                {
                    activeInvasionTarget = null;
                }
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

    #region Drag Controls
    public void OnDrag(PointerEventData pointerEventData)
    {
        //Debug.Log("Dragging");
    }

    public void OnDrop(PointerEventData pointerEventData)
    {
        Territory invader = pointerEventData.pointerDrag.GetComponent<Territory>();

        if(invader != null)
        {
            invader.InvadeTerritory(this);
        }
    }

    public void InvadeTerritory(Territory invasionTarget)
    {
        Debug.Log("Sending troops from " + gameObject.name + " to " + invasionTarget.name);

        activeInvasionTarget = invasionTarget;


    }

    public void SendTroop()
    {
        troops--;

        Instantiate(gameController.troopPrefab, gameController.troopParent);

        //TODO: Move troops from this territory to invaded territory
    }
    #endregion
}
