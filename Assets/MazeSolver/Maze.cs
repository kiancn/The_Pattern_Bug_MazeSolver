using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Maze : MonoBehaviour
{
    [SerializeField] public int[,] horizontalWalls;

    [SerializeField] public int[,] verticalsWalls;


    // Start is called before the first frame update
    void Awake()
    {
        if (horizontalWalls == null)
        {
            ImplementDummyMaze();
        }
    }

    private void ImplementDummyMaze()
    {
        horizontalWalls = new int[6, 11]
        {
            {1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1},
            {0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0},
            {1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0},
            {0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1},
            {1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1}
        };
        
        verticalsWalls = new int[12, 5]
        {
            {1, 1, 1, 1, 1},
            {0, 0, 1, 0, 0},
            {0, 1, 1, 1, 0},
            {0, 0, 0, 0, 0},
            {1, 1, 0, 1, 0},
            {0, 1, 1, 0, 0},
            {0, 0, 1, 0, 0},
            {0, 1, 1, 1, 0},
            {0, 0, 0, 0, 0},
            {1, 1, 0, 1, 0},
            {0, 1, 1, 0, 0},
            {1, 1, 1, 1, 1}
        };
        //
        // horizontalWalls = new int[6, 6]
        // {
        //     {1, 0, 1, 1, 1, 1},
        //     {0, 1, 1, 0, 0, 0},
        //     {1, 0, 0, 1, 0, 0},
        //     {0, 0, 1, 0, 1, 0},
        //     {1, 1, 1, 1, 0, 1},
        //     {1, 1, 0, 1, 1, 1}
        // };
        //
        // verticalsWalls = new int[7, 5]
        // {
        //     {1, 1, 1, 1, 1},
        //     {0, 0, 1, 0, 0},
        //     {0, 1, 1, 1, 0},
        //     {0, 0, 0, 0, 0},
        //     {1, 1, 0, 1, 0},
        //     {0, 1, 1, 0, 0},
        //     {1, 1, 1, 1, 1}
        // };
    }


    // Update is called once per frame
    void Update() { }
}