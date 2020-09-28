using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MazeSolver.Djikstraesque
{
    /// <summary>
    /// PathFinder for the DjNodes -
    /// 
    /// Script is an evolution of DjPathFinder and has very close parallels.
    /// S    Star - all directions will ultimately be searched
    /// AB    Anti-Backbend
    /// SE    Smart Evaluation
    /// </summary>
    public class DjPathFinderPlus : MonoBehaviour
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

        public void FindAndDrawRoutes() { markerDrawer.DrawPathMarkers(FindAllRoutes()); }

        [SerializeField] private int numberOfTries = 3;

        [SerializeField] private float weightIncreaseMultiplier;

        /// <summary>
        /// Method returns the shortest distance path between supplied node and
        /// the closest winning node (weight of nodes calculated on basis of distance
        /// to winning node + distance between nodes).
        /// NB. This algorithm is serverely hindered by dead-ends. Writing new and improved. 
        /// </summary>
        /// <param name="startNode"></param>
        /// <returns>Route or empty stack returned</returns>
        public Stack<DjNode> ShortestDistanceRoute(DjNode startNode)
        {
            Stack<DjNode> route = new Stack<DjNode>(200);

            int attemptCounter = 0;
            
            while (route.Count < 1 && attemptCounter < numberOfTries) /* try to find a route max numberOfTries times*/
            {
                Debug.Log("Making #" + attemptCounter + " attempt to find route from " + startNode.name);

                route.Push(startNode); // start node is now first node on stack

                while (route.Count > 0) // while stack has members (and is not returned because of success)
                {
                    DjNode currentNode = route.Peek(); // stored reference for efficiency of reference 

                    if (currentNode.IsWinningNode) { return route; } /* if top of stack is winning node, return it*/

                    /* if node visited has no neighbors to go to, pop it from route stack, and 'continue' */
                    if (currentNode.UnvisitedNeighborhood.Count == 0)
                    {
                        route.Pop();
                        continue;
                    }

                    /* find cheapest unvisited neighbor :: java-eyes-info: cheapestNode is created HERE,returned by out */
                    if (!FindCheapestUnvisitedNeighbor(currentNode, route, out var cheapestNode)) {continue;}

                    /* check to see it route in bending back on it self, if so, pop until top node is the original encounter with the node */
                    CheckForBackBend(currentNode, cheapestNode, route);

                    currentNode.UnvisitedNeighborhood.Remove(cheapestNode); /* remove it from currentNodes unvisited nodes */
                    
                    /* adjust price of neighbor examined, upping price for next run */
                    currentNode.Neighborhood[cheapestNode] = currentNode.Neighborhood[cheapestNode] * weightIncreaseMultiplier;

                    route.Push(cheapestNode); /* put cheapest node on top of stack */
                }

                /* reset unvisited neighbors across the field */
                field.ReInitializeUnvisitedNeighbors();

                /*on to next attempt*/
                attemptCounter++;
            }

            Debug.Log("Returning incomplete route, number of member: " + route.Count);
            return route;
        }


        private static bool FindCheapestUnvisitedNeighbor(DjNode currentNode, Stack<DjNode> route, out DjNode cheapestNode)
        {
            cheapestNode = currentNode;
            float cheapestCost = Single.MaxValue;

            foreach (var unvNeighNode in currentNode.UnvisitedNeighborhood)
            {
                if (cheapestCost > unvNeighNode.Value)
                {
                    cheapestNode = unvNeighNode.Key;
                    cheapestCost = unvNeighNode.Value;
                }
            }

            if (cheapestNode == currentNode) /*if there is no available neightbor, pop this node from route: take a step back */
            {
                route.Pop();
                return false; /* no neighbor found */
            }

            return true; /* neighbor found and cheapest */
        }

        private static void CheckForBackBend(DjNode currentNode, DjNode cheapestNode, Stack<DjNode> route)
        {
            DjNode[] routeSoFar = route.ToArray(); /* Simplest way to iterate through a stack */

            for (int i = 0; i < routeSoFar.Length; i++)
            {
                /* if cheapestNode is identified in stack */
                if (routeSoFar[i] == cheapestNode)
                {
                    // Debug.Log("Loopback encountered. Backtracking.");
                    /* then back track */
                    while (route.Count > 0 && route.Peek() != cheapestNode) { route.Pop(); }

                    break; /* one match is enough, so break out */
                }
            }
        }
    }
}