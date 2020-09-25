using System;
using UnityEngine;

namespace MazeSolver.Djikstraesque
{
    
    /// <summary>
    /// Spawned class instances represent an obstacle on a field.
    /// </summary>
    public class Obstacle : MonoBehaviour
    {
        private static int series = 0;

        private int serialNumber;

        private void Awake()
        {
            serialNumber = series++; // 
            gameObject.name = "Obstacle #" + serialNumber.ToString() + " " + gameObject.name;
        }

        private void Update()
        {
            
        }
    }
}