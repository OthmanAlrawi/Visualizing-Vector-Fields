using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extendedObject : MonoBehaviour
{
    public int gridResolution = 5;
    public float gridWidth = 5;
    public bool backGround = false;
    public int charge = -3;
    public GameObject sphere;
    public GameObject pointCharge;
    public GameObject tracer;
    public int tracerCount;
    Vector3[] tracerSpawnPositions;
    private void Awake()
    {


        InvokeRepeating("invokedSpawnTracers", 0, electricField.spawnInterval1);
        /*Vector3[,,] grid = electricField.createGrid(gridResolution, gridWidth, transform.position);

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                for (int k = 0; k < grid.GetLength(0); k++)
                {
*//*                    Instantiate(sphere, grid[i, j, k], Quaternion.identity);*//*
                    if (checkIfPointInside(grid[i, j, k]))
                    {
                        GameObject temp = Instantiate(pointCharge, grid[i, j, k], Quaternion.identity, gameObject.transform);
                        temp.GetComponent<Collider>().enabled = false;
                        temp.GetComponent<Renderer>().enabled = false;
                        temp.GetComponent<TrailRenderer>().enabled = false;
                    }
                }
            }
        }*/
    }

    void invokedSpawnTracers()
    {
        tracerSpawnPositions = electricField.findMeshTracerSpawnPositions(gameObject);
        electricField.spawnTracersForExtendedObjects(tracer, tracerSpawnPositions, electricField.lifeTime1, tracerCount, gameObject);
    }

    bool checkIfPointInside(Vector3 Goal)
    {
        Vector3 Point;
        Vector3 Start = new Vector3(0, 500, 0); // This is defined to be some arbitrary point far away from the collider.
        Vector3 Direction = Goal - Start; // This is the direction from start to goal.
        Direction.Normalize();
        int Itterations = 0; // If we know how many times the raycast has hit faces on its way to the target and back, we can tell through logic whether or not it is inside.
        Point = Start;


        while (Point != Goal) // Try to reach the point starting from the far off point.  This will pass through faces to reach its objective.
        {
            RaycastHit hit;
            if (Physics.Linecast(Point, Goal, out hit, 1 << LayerMask.NameToLayer("Extended Objects"))) // Progressively move the point forward, stopping everytime we see a new plane in the way.
            {
                Debug.DrawLine(Point, hit.point, Color.blue, 100);
                Itterations++;
                Point = hit.point + (Direction / 100.0f); // Move the Point to hit.point and push it forward just a touch to move it through the skin of the mesh (if you don't push it, it will read that same point indefinately).
            }
            else
            {
                Point = Goal; // If there is no obstruction to our goal, then we can reach it in one step.
            }
        }
        while (Point != Start) // Try to return to where we came from, this will make sure we see all the back faces too.
        {
            RaycastHit hit;
            if (Physics.Linecast(Point, Start, out hit, 1 << LayerMask.NameToLayer("Extended Objects")))
            {
                Debug.DrawLine(Point, hit.point, Color.red, 100);
                Itterations++;
                Point = hit.point + (-Direction / 100.0f);
            }
            else
            {
                Point = Start;
            }
        }

        return Itterations % 2 == 1;
    }

}
