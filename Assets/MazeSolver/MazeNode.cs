using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class presents a node in a maze, uniform to 90 degree moves, 4-direction, equal-lenght 'wall' 
/// </summary>
[Serializable]
public class MazeNode : MonoBehaviour
{
    /* a square field and a 90 angle of movement leave four archtypical directions as signifying candidates:*/
    [SerializeField] private Opening up;
    [SerializeField] private Opening down;
    [SerializeField] private Opening left;
    [SerializeField] private Opening right;

    /* conceptual position in field, not 'world space' position */
    [SerializeField] private Vector2Int position;

    [SerializeField] private int visits;

    [SerializeField] private Stack<Direction> openDirections;

    [SerializeField] private int cost;

    /* starting nodes are ns that are on hW[0,0...hW.getLenght(1)] where Down is Opening.OPEN */
    [SerializeField] private bool startingPosition;

    /* winning positions are ns on a border of the maze that have an opening to the outside.
     Conditions on which a node is a winning candidate:
     * * NOT a starting position node (my rule)
     * * IF a node is top or bottom node, extreme left or right node.
     Conditions on which a candidate winning node IS A winning node:
     * * IF the direction in which the node is extreme is OPEN : much win = true = winningPosition. 
     */
    [SerializeField] private bool winningPosition;

    public void InitializeNode(Maze maze, int yCount, int xCount)
    {

        Cost = 1;
        Position = new Vector2Int(xCount, yCount);
        
        Openings = new Stack<Direction>();

        DetermineSurroundings(maze, yCount, xCount);
        IsStartingPosition();
        IsWinningPosition(maze);
    }

    /// <summary>
    /// Detects and sets directions up,down,left,right as either open or closed.
    /// </summary>
    private void DetermineSurroundings(Maze maze, int yCount, int xCount)
    {
        Up = maze.horizontalWalls[yCount + 1, xCount] == 1 ? Opening.CLOSED : Opening.OPEN;
        Down = maze.horizontalWalls[yCount, xCount] == 1 ? Opening.CLOSED : Opening.OPEN;
        Left = maze.verticalsWalls[xCount, yCount] == 1 ? Opening.CLOSED : Opening.OPEN;
        Right = maze.verticalsWalls[xCount + 1, yCount] == 1 ? Opening.CLOSED : Opening.OPEN;

        /* very significant; this is the order in which directions are checked */
        
        if(Right == Opening.OPEN){Openings.Push(Direction.RIGHT);}

        if(Up == Opening.OPEN){Openings.Push(Direction.UP);}

        if(Left == Opening.OPEN){Openings.Push(Direction.LEFT);}
        
        if(Down == Opening.OPEN){Openings.Push(Direction.DOWN);}

    }

    /// <summary>
    /// Nodes on bottom row with down open count as starting positions. Detects if node is a starting node.
    /// </summary>
    private void IsStartingPosition() { StartingPosition = Position.y == 0 && Down == Opening.OPEN; }

    /// <summary>
    /// Winning positions are nodes on a border of the maze that have an opening to the outside. 
    /// </summary>
    private void IsWinningPosition(Maze maze)
    {
        WinningPosition = false;

        /*  Conditions on which a node is a winning candidate:
            * * NOT a starting position node */
        if (StartingPosition) { return; }

        bool extremeUp = Position.y == maze.verticalsWalls.GetLength(1) - 1;
        bool extremeDown = Position.y == 0;
        bool extremeLeft = Position.x == 0;
        bool extremeRight = Position.x == maze.horizontalWalls.GetLength(1) - 1;

        /*  * * IF a node is top or bottom node, extreme left or right node.*/
        if (extremeUp || extremeDown || extremeLeft || extremeRight)
        {
            /* Conditions on which a candidate winning node IS A winning node:
                * * IF the direction in which the node is extreme is OPEN : much win = true = winningPosition. 
             * */
            bool winning = false;
            winning = up == Opening.OPEN && extremeUp;
            winning = down == Opening.OPEN && extremeDown || winning;
            winning = left == Opening.OPEN && extremeLeft || winning;
            winning = right == Opening.OPEN && extremeRight || winning;

            WinningPosition = winning;
        }
    }

    public Vector2Int Position
    {
        get => position;
        set => position = value;
    }

    public Opening Up
    {
        get => up;
        set => up = value;
    }

    public Opening Down
    {
        get => down;
        set => down = value;
    }

    public Opening Left
    {
        get => left;
        set => left = value;
    }

    public Opening Right
    {
        get => right;
        set => right = value;
    }


    public int Cost
    {
        get => cost;
        set => cost = value;
    }

    public bool StartingPosition
    {
        get => startingPosition;
        set => startingPosition = value;
    }

    public bool WinningPosition
    {
        get => winningPosition;
        set => winningPosition = value;
    }

    public int Visits
    {
        get => visits;
        set => visits = value;
    }
    
    public Stack<Direction> Openings
    {
        get => openDirections;
        set => openDirections = value;
    }
}

// I'm too stupid for abstract math, so I drew this and extrapolated (for calculating if directions are open/closed)
// Felt/Node 1:
// UP: hW[1,0]
// DN: hW[0,0]
// LT: vW[0,0]
// RT: vW[1,0]
//
// Felt 2:
// UP: hW[1,1]
// DN: hW[0,1]
// LT: vW[1,0]
// RT: vW[2,0]
//
// Felt 3:
// UP: hW[1,2]
// DN: hW[0,2]
// LT: vW[2,0]
// RT: vW[3,0]