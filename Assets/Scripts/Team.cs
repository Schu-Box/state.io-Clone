using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public Color teamColor;

    public bool userControlled;

    private bool eliminated = false;

    private float computerActionFrequencyMin = 6f;
    private float computerActionFrequencyMax = 10f;
    private float computerActionFrequency; //Random frequency between min/max, determined after each action
    private float computerActionTimer;

    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (!userControlled && !eliminated)
		{
			computerActionTimer -= Time.deltaTime;

			if (computerActionTimer <= 0)
			{
				computerActionFrequency = Random.Range(computerActionFrequencyMin, computerActionFrequencyMax);
				computerActionTimer = computerActionFrequency;

				DetermineComputerAction();
			}
		}
    }

    private void DetermineComputerAction()
    {
        List<ComputerAction> potentialActions = new List<ComputerAction>();
        List<Territory> controlledTerritories = gameController.GetControlledTerritories(this);

        if(controlledTerritories.Count == 0)
        {
            eliminated = true; //There's an edge case where the computer could still have troops invading and thus not be totally eliminated. This would be solved by checking if all their troops are dead too.
        }

        for(int i = 0; i < controlledTerritories.Count; i++)
        {
            Territory potentialAggressorTerritory = controlledTerritories[i];

            for(int j = 0; j < gameController.territories.Count; j++)
            {
                Territory potentialInvasionTargetTerritory = gameController.territories[j];

                int troopDifferential = potentialAggressorTerritory.troops - potentialInvasionTargetTerritory.troops;

				if (potentialInvasionTargetTerritory.teamController != this && troopDifferential > 1) //If the target territory isn't controlled by this team and has less troops
                {
                    potentialActions.Add(new ComputerAction(potentialAggressorTerritory, potentialInvasionTargetTerritory, troopDifferential));
                }
            }
        }

        if(potentialActions.Count > 0)
        {
            ExecuteComputerAction(potentialActions[Random.Range(0, potentialActions.Count)]);

            //Attempted to make it a bit more difficult by making the AI target the weakest territories. This makes it far too hard for this demo though. The ideal would likely be a mix of this + randomness
            /*
			ComputerAction highestTroopDifferentialAction = null;
            int highestTroopDifferential = 0;
            for(int i = 0; i < potentialActions.Count; i++)
            {
                ComputerAction potentialAction = potentialActions[i];
                if (potentialAction.troopDifferential > highestTroopDifferential)
                {
                    highestTroopDifferential = potentialAction.troopDifferential;
                    highestTroopDifferentialAction = potentialAction;
                }
            }

            ExecuteComputerAction(highestTroopDifferentialAction);
            */
        }
    }

    public void ExecuteComputerAction(ComputerAction computerAction)
    {
        computerAction.aggressorTerritory.SetNewActiveInvasionTarget(computerAction.invasionTargetTerritory);
    }
}

public class ComputerAction
{
    public Territory aggressorTerritory;
    public Territory invasionTargetTerritory;

    public int troopDifferential;

    public ComputerAction(Territory aggressor, Territory invadee, int differential)
    {
        aggressorTerritory = aggressor;
        invasionTargetTerritory = invadee;

        troopDifferential = differential;
    }
} 
