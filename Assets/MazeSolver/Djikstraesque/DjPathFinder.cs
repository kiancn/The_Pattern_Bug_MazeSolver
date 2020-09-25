using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MazeSolver.Djikstraesque;
using UnityEngine;

/// <summary>
/// PathFinder for the DjNodes -
/// writing has just begun.
/// </summary>
public class DjPathFinder : MonoBehaviour
{
    /// <summary>
    /// Field is used to gather information about the field of nodes to path.
    /// </summary>
    [SerializeField] private DjNodeField field;

    [SerializeField] private DjMarkerDrawer markerDrawer;

    /// <summary>
    /// Method returns all routes from stating nodes to closest winning node on field. 
    /// </summary>
    /// <returns>All routes from nodes marked as starting position to closest winning node.
    /// The format is a List containing Stacks of DjNodes, with the bottom node being
    /// the starting node and last node being the winning node.</returns>
    public List<Stack<DjNode>> FindAllRoutes()
    {
        List<Stack<DjNode>> routes = new List<Stack<DjNode>>();

        /* field initialized before pathing */
        field.InitializeField();
        /*implement*/

        /* find starting nodes on field */
        List<DjNode> startingNodes = field.Field.FindAll(node => node.IsStartingNode);

        foreach (var node in startingNodes)
        {
            routes.Add(ShortestDistanceRoute(node));
            field.InitializeField();
        }

        Debug.Log("DjPathFinder :: Found " + routes.Count + " routes.");
        
        return routes;
    }

    public void FindRoutes() { markerDrawer.DrawPathMarkers(FindAllRoutes());}

    /// <summary>
    /// Method returns the shortest distance path between supplied node and
    /// the closest winning node (weight of nodes calculated on basis of distance
    /// to winning node + distance between nodes).
    /// </summary>
    /// <param name="node"></param>
    /// <returns>Route or empty stack returned</returns>
    public Stack<DjNode> ShortestDistanceRoute(DjNode node)
    {
        Stack<DjNode> route = new Stack<DjNode>(50);
        
        node.Route.AddFirst(node); // updating route internal to start node
        
        route.Push(node); // start node is now first node on stack

        while (route.Count > 0) // while stack has members (and is not returned because of success)
        {
            /*finding cheapest node*/
            DjNode cheapestNode = route.Peek();
            float lowestPrice = Single.MaxValue;
            foreach (var nNode in route.Peek().Neighborhood)
            {
                if (nNode.Value < lowestPrice)
                {
                    cheapestNode = nNode.Key;
                    lowestPrice = nNode.Value;
                }
            }

            if (!route.Peek().Route.Contains(cheapestNode))
            // if (cheapestNode != route.Peek() && !cheapestNode.IsStartingNode && !route.Peek().IsStartingNode)
            {
                route.Peek().Route.AddLast(cheapestNode);
                cheapestNode.Route = route.Peek().Route;
                
                route.Push(cheapestNode);
                
                
                if (cheapestNode.IsWinningNode)
                {
                    return route;
                }
            }
            else // this will only happen if a starting node is isolated, i think
            {
                route.Pop();
            }
        }
        return route;
    }
}