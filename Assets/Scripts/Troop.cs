using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Troop : MonoBehaviour
{
    public Image image;

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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision entered");

        Troop collidingTroop = collision.gameObject.GetComponent<Troop>();

        if(collidingTroop != null)
        {
            DestroyTroop();
            collidingTroop.DestroyTroop();
        }
    }

    public void SetTeamController(Team team)
    {
        teamController = team;

        image.color = team.teamColor;
    }

    public void SetNewTargetTerritory(Territory newTarget)
    {
        targetTerritory = newTarget;
    }

    public void Invade()
    {
        targetTerritory.Invade(this);

        DestroyTroop();
    }

    public void DestroyTroop()
    {
		//TODO: animation

		Destroy(gameObject);
	}
}
