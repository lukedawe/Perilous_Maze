using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public List<List<Vector3>> PossibleRoutes;
    public List<Vector3> PointsGrid;
    public List<Vector3> FindFastestPath(Vector3 current, Vector3 destination)
    {
        PossibleRoutes = new List<List<Vector3>>();

        if (current == destination)
        {
            return null;
        }

        // now, we start at the point closest to the enemy, and then explore
        // nearby points from there

        List<Vector3> temp = new List<Vector3>();
        FindAPath(current, destination, temp);

        List<Vector3> fastestRoute = new List<Vector3>();
        int min = 100;

        if (PossibleRoutes.Count > 0)
        {
            fastestRoute.AddRange(PossibleRoutes[0]);
            min = PossibleRoutes[0].Count;
            foreach (List<Vector3> route in PossibleRoutes)
            {
                if (route.Count < min)
                {
                    min = route.Count;
                    fastestRoute.Clear();
                    fastestRoute.AddRange(route);
                }
            }
        }

        return fastestRoute;
    }

    void FindAPath(Vector3 current, Vector3 destination, List<Vector3> routeToDestination)
    {
        if (routeToDestination.Count > 30)
        {
            return;
        }
        Vector3[] directions = { new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };

        if (current != destination)
        {
            foreach (Vector3 direction in directions)
            {
                Vector3 newPoint = current + direction;
                if (PointsGrid.Contains(newPoint) && !routeToDestination.Contains(newPoint))
                {
                    List<Vector3> newRoute = new List<Vector3>();
                    newRoute.AddRange(routeToDestination);
                    newRoute.Add(newPoint);
                    FindAPath(newPoint, destination, newRoute);
                }
            }
        }
        else
        {
            if (routeToDestination.Count > 0)
            {
                List<Vector3> temp = new List<Vector3>();
                temp.AddRange(routeToDestination);
                PossibleRoutes.Add(temp);
            }
        }
    }
}
