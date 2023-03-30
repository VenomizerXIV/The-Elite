using System.Collections;
using System.Collections.Generic;
using Chronos;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 1f;
    private int waypointIndex = 0;

    void Update()
    {
        // if (!PlayerMovement.superPowerActivated)
        // {
        Clock clock = Timekeeper.instance.Clock("NonPlayer");
        if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.deltaTime * clock.localTimeScale);
    }
    // }
}
