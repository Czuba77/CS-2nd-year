using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] Waypoints;
    [Range(0.01f, 20.0f)] [SerializeField] private float speed = 2.0f;
    int currentWaypoint=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, Waypoints[currentWaypoint].transform.position)<0.1f)
        {
            currentWaypoint++;
            currentWaypoint = currentWaypoint % Waypoints.Length;
        }
        transform.position=Vector2.MoveTowards(transform.position, Waypoints[currentWaypoint].transform.position,speed*Time.deltaTime);

    }
}
