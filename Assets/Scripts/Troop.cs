using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : MonoBehaviour
{
    private float speed = 1f;

    private Territory targetTerritory;

    public Team teamController;

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetTerritory.transform.position, step);

        if(Vector3.Distance(transform.position, targetTerritory.transform.position) < 0.01f) //If the troop is nearly on top of the targetTerritory
        {
            Invade();
        }
    }

    public void SetTeamController(Team team)
    {
        teamController = team;
    }

    public void SetNewTargetTerritory(Territory newTarget)
    {
        targetTerritory = newTarget;
    }

    public void Invade()
    {
        targetTerritory.Invade(this);

        //TODO: animation

        Destroy(gameObject);
    }
}
