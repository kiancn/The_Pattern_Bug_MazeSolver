using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private MazeField field;

    [SerializeField] private List<GameObject> pathMarkerPrefabs;


    private void Awake() { field = gameObject.GetComponent<MazeField>(); }

    public void FindRoutes()
    {
        List<Stack<MazeNode>> routes = new List<Stack<MazeNode>>();

        foreach (var node in field.StartingPositions)
        {
            Stack<MazeNode> route = FindRoute(node);

            field.ReinitializeNodeField();
            
            Debug.Log("PathFinder :: Route with " + route.Count + "returned.");

            routes.Add(route);
        }

        Debug.Log("Routes returned: " + routes.Count);

        DrawPathMarkers(routes);
    }

    private Stack<MazeNode> FindRoute(MazeNode node)
    {
        Stack<MazeNode> route = new Stack<MazeNode>();

        Debug.Log("Starting node " + node.name + " has " + node.Openings.Count + " open directions to search");

        route.Push(node); // insert node at top of stack

        /* while stack has members, which means a 'route' of at least 1 */
        while (route.Count > 0)
        {
            if (route.Peek().WinningPosition) // position 
            {
                Debug.Log("Found route, returning route of length: " + route.Count);
                return route;
            }

            /* direction to check on current node determined by top of stack */
            Debug.Log("Node " + route.Peek().name + " has " + route.Peek().Openings.Count + " open directions to search");

            if (route.Peek().Openings.Count > 0)
            {
                MazeNode nextNode = FindNodeInDirection(route.Peek(), route.Peek().Openings.Pop());

                if (nextNode == null) { continue; }

                if (!route.Contains(nextNode)) // dont allow reentry into already 'routing' nodes.
                {
                    route.Push(nextNode); // if not on route, put on top of route stack for investigation
                }
            }
            else
            {
                /* if next node node delivered node has no more available directions, pop it from route */
                route.Pop();
            }
        }
        
        return route;
    }

    private MazeNode FindNodeInDirection(MazeNode node, Direction direction)
    {
        /* if direction is opening DOWN and node position is first/botom line, skip it  */
        if (node.Position.y == 0 && direction == Direction.DOWN) { return null; }

        /* base position*/
        Vector2 nodePosition = node.Position;
        /*adding direction*/
        switch (direction)
        {
            case Direction.RIGHT:
                nodePosition += Vector2.right; // (x,y) += (1,0) = (x+1,y+0)
                break;
            case Direction.UP:
                nodePosition += Vector2.up; // (x,y) += (0,1) = (x+0,y+1)
                break;
            case Direction.LEFT:
                nodePosition += Vector2.left; // same principle...
                break;
            case Direction.DOWN:
                nodePosition += Vector2.down;
                break;
        }

        /* look for node with Position attribute equal to calculated position*/
        foreach (var n in field.NodeField)
        {    // this works because the grid is uniform, not under other conditions
            if (n.Position == nodePosition) { return n; }
        }

        Debug.Log("No neighbor node found in direction " + direction + " on " + node.name);
        return null;
    }

    private void DrawPathMarkers(List<Stack<MazeNode>> routes)
    {
        Debug.Log("Number of routes: " + routes.Count);
        int routeN = 0;
        foreach (var nStack in routes)
        {
            Debug.Log("Route " + routeN++ + " has "+ nStack.Count + " nodes.");
        }
        
        for (int i = 0; i < routes.Count; i++)
        {
            GameObject pathMarker = Instantiate(pathMarkerPrefabs[i], Vector2.down, Quaternion.identity);

            Debug.Log("Length of route #" + i + " : " + routes[0].Count);

            for (int j = 0;  routes[i].Count>0; j++)
            {
                Debug.Log("Route [" + i + "] " + " Node: [" + j + "] of " + routes[0].Count);
                Instantiate(pathMarker, routes[i].Pop().transform.position, Quaternion.identity, transform);
            }

            Destroy(pathMarker);
        }
    }
}