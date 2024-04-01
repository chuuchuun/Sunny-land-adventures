using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 1.0f;
    private int currentWaypoint = 0;

    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (Vector2.Distance(this.transform.position, waypoints[currentWaypoint].transform.position) < 0.1f)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            }
            this.transform.position = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);
        }
    }
}
