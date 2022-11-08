using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class Territory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
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

				UpdateTerritoryVisuals();
            }
        }

        if(activeInvasionTarget != null)
        {
            troopInvasionTimer -= Time.deltaTime;
            if(troopInvasionTimer <= 0)
            {
                troopInvasionTimer = troopInvasionFrequency;

                SendTroop();

				UpdateTerritoryVisuals();

                if(troops <= 0) //If the last troop has been sent, end the invasion
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

    public void UpdateTerritoryVisuals()
    {
		troopCounterText.text = troops.ToString();

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
    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if(teamController != null && teamController.userControlled)
		{
			gameController.dragIndicator.StartedDragging(transform.position);
		}
    }

    public void OnDrag(PointerEventData pointerEventData)
	{
		gameController.dragIndicator.Dragging();
	}
    
    public void OnEndDrag(PointerEventData pointerEventData)
    {
		gameController.dragIndicator.StoppedDragging();
	}

    public void OnDrop(PointerEventData pointerEventData)
    {
        Territory invader = pointerEventData.pointerDrag.GetComponent<Territory>();

        if (invader != null && invader.teamController != null && invader.teamController.userControlled) //If the user is attempting an invasion
        {
            if (invader == this) //If the invasion target is itself, cancel any active invasion
            {
                activeInvasionTarget = null;
            }
            else
            {
                invader.SetNewActiveInvasionTarget(this);
            }
        }
	}
	#endregion

	public void SetNewActiveInvasionTarget(Territory invasionTarget)
    {
        //Debug.Log("Sending troops from " + gameObject.name + " to " + invasionTarget.name);

        activeInvasionTarget = invasionTarget;
    }

    public void SendTroop()
    {
        troops--;

        Troop newTroop = Instantiate(gameController.troopPrefab, transform.position, Quaternion.identity, gameController.troopParent).GetComponent<Troop>();
        newTroop.SetTeamController(teamController);
        newTroop.SetNewTargetTerritory(activeInvasionTarget);
    }

    public void Invade(Troop troop)
    {
        if(teamController == troop.teamController) //If the invading troop is on the same team as this territory's teamController
        {
            troops++;
        }
        else //If the invading troop is an opponent
        {
            if(troops <= 0)
            {
                SetNewTeamController(troop.teamController);
            }
            else
			{
				troops--;
			}
		}

		UpdateTerritoryVisuals();
    }

    public void SetNewTeamController(Team newTeamController)
    {
        teamController = newTeamController;
    }
}
