using System;
using System.Collections;
using System.Collections.Generic;
using MazeSolver.Djikstraesque;
using UnityEngine;
/// <summary>
/// Draws markers for routes of DjNodes
/// </summary>
public class DjMarkerDrawer : MonoBehaviour
{
    
    /// <summary>
    /// Since an unknown number of starting points might exist,
    /// list of GameObjects is used, accessed by index to spawn path from:
    /// In practise this supplies a different path marker for each path
    /// </summary>
    [SerializeField] private List<GameObject> pathMarkerPrefabs;

    private List<GameObject> spawnedPrefabs;

    private void OnEnable()
    {
        spawnedPrefabs = new List<GameObject>();
    }

    public void DrawPathMarkers(List<Stack<DjNode>> routes)
    {
        /* destroy current markers - refactor */
        foreach (var marker in spawnedPrefabs) { Destroy(marker); }
        
        spawnedPrefabs = new List<GameObject>();
        
        Debug.Log("Number of routes: " + routes.Count + " being drawn.");
        int routeNumber = 0;
        foreach (var nodeStack in routes)
        {
            Debug.Log("Route " + routeNumber++ + " has "+ nodeStack.Count + " nodes.");
        }
        
        for (int i = 0; i < routes.Count; i++)
        {
            GameObject pathMarker = Instantiate(pathMarkerPrefabs[i], Vector2.down, Quaternion.identity);

            for (int j = 0;  routes[i].Count>0; j++)
            {
                spawnedPrefabs.Add(Instantiate(pathMarker, routes[i].Pop().transform.position, Quaternion.identity, transform));
            }

            Destroy(pathMarker);
        }
    }
}
