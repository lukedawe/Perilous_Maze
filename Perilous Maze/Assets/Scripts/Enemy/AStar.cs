using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStar : MonoBehaviour
{
    public GameObject[,] maze;
    readonly Vector3[] directions = { new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };

    public void Constructor(GameObject[,] maze)
    {
        this.maze = maze;
    }

    // Manhattan heuristic as we can only go 4 directions
    static int GetHeuristic(Vector3 start, Vector3 end)
    {
        return (int)(start.x - end.x) + (int)(start.y - end.y);
    }

    public Vector3[] AStarSearch(Vector3 start, Vector3 destination)
    {
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        Node startNode = new Node(null, start);

        open.Add(startNode);

        while (open.Count > 0)
        {
            Node q = open[0];

            // find the node with the least f
            foreach (Node n in open)
            {
                if (n.f < q.f)
                {
                    q = n;
                }
            }

            // remove that node from the open list
            open.Remove(q);

            // if q is at the destination, return an array of the route that it took to get to q
            if (q.position == destination)
            {
                List<Vector3> path = new List<Vector3>();
                Node current = q;
                while (current != null)
                {
                    path.Add(current.position);
                    current = current.parent;
                }
                return path.ToArray();
            }

            // go in each direction from q, as long as it's in range and possible to travel to and 

            foreach (Vector3 direction in directions)
            {
                Node successor = new Node(q, q.position + direction);

                bool xInRange = (successor.position.x > 0 && successor.position.x < Math.Sqrt(maze.Length) - 1);
                bool zInRange = (successor.position.z > 0 && successor.position.z < Math.Sqrt(maze.Length) - 1);

                if (!xInRange || !zInRange)
                {
                    continue;
                }
                if (maze[(int)successor.position.x, (int)successor.position.z] != null)
                {
                    continue;
                }

                successor.g = q.g + 1;
                successor.h = GetHeuristic(successor.position, destination);
                successor.f = successor.g + successor.h;
                // make sure that each resulting point isn't in open or closed list already with a lower f value.
                foreach (Node n in open)
                {
                    if (n.position == successor.position && n.f < successor.f)
                    {
                        goto outer;
                    }
                }
                foreach (Node n in closed)
                {
                    if (n.position == successor.position && n.f < successor.f)
                    {
                        goto outer;
                    }
                }

                // then add that to that node to the open list
                open.Add(successor);

            outer:
                continue;
            }

            closed.Add(q);
        }

        Debug.Log("No path found\n" + maze);
        return null;
    }

    // a class for holding the data of each point in the maze
    class Node
    {
        public Node parent;
        public Vector3 position;
        public float g, h, f;
        public Node(Node parent, Vector3 position)
        {
            this.parent = parent;
            this.position = position;
            g = h = f = 0;
        }
    }
}
