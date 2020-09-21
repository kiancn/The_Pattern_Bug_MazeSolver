using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class instance represents a field of MazeNodes, which conceptually make up a 'live' maze.
/// 
/// </summary>
public class MazeField : MonoBehaviour
{
    [SerializeField] private Vector2 drawingZero;
    [SerializeField] private float horizontalIncrement;
    [SerializeField] private float verticalIncrement;

    [SerializeField] private Maze maze;

    [SerializeField] private List<MazeNode> nodeField;

    [SerializeField] private GameObject nodeTokenPrefab;

    [SerializeField] private List<MazeNode> startingPositions;
    [SerializeField] private List<MazeNode> winningPositions;

    public List<MazeNode> StartingPositions
    {
        get => startingPositions;
        set => startingPositions = value;
    }

    public List<MazeNode> WinningPositions
    {
        get => winningPositions;
        set => winningPositions = value;
    }

    #region PROPERTIES

    public Vector2 DrawingZero
    {
        get => drawingZero;
        set => drawingZero = value;
    }

    public float HorizontalIncrement
    {
        get => horizontalIncrement;
        set => horizontalIncrement = value;
    }

    public float VerticalIncrement
    {
        get => verticalIncrement;
        set => verticalIncrement = value;
    }

    public Maze Maze
    {
        get => maze;
        set => maze = value;
    }

    public List<MazeNode> NodeField
    {
        get => nodeField;
        set => nodeField = value;
    }

    public GameObject NodeTokenPrefab
    {
        get => nodeTokenPrefab;
        set => nodeTokenPrefab = value;
    }
    #endregion
    
    private void Awake()
    {
        startingPositions = new List<MazeNode>();
        winningPositions = new List<MazeNode>();
        nodeField = new List<MazeNode>();
    }

    // Start is called before the first frame update
    void Start()
    {
        int horizontalSpan = maze.horizontalWalls.GetLength(1);
        int verticalSpan = maze.verticalsWalls.GetLength(1);
        int fieldSize = horizontalSpan * verticalSpan;

        /* spawing a base token token from supplied prefab file */
        GameObject tokenBase = Instantiate(nodeTokenPrefab, Vector2.zero, Quaternion.identity); // best practise.

        for (int round = 1, xCount = 0, yCount = 0; round <= fieldSize; round++, xCount++)
        {    /* Spawning token prefab at right location */
            GameObject nodeToken =
                Instantiate(tokenBase,
                    new Vector2(drawingZero.x + (xCount * horizontalIncrement), drawingZero.y + (yCount * verticalIncrement)),
                    Quaternion.identity,
                    this.transform);

            /* naming token for editor sanity */
            nodeToken.name = "Node #" + round + " : (x " + xCount + ") (y " + yCount + ")";

            /* gettting node from GO and initializing data*/
            MazeNode node = nodeToken.GetComponent<MazeNode>();
            node.InitializeNode(maze, yCount, xCount);
            nodeField.Add(node);
            
            if(node.StartingPosition){startingPositions.Add(node);}
            if(node.WinningPosition){winningPositions.Add(node);}

            /* change 'lines' based on modulus of width of maze (as given by number of horizontal walls)*/
            if ((round % horizontalSpan == 0 && round != 0))
            {
                xCount = -1;
                yCount++;
            }
        }

        Destroy(tokenBase, 0.03f);
    }

    public void ReinitializeNodeField()
    {
        foreach (var n in nodeField)
        {
            // works because Position is 
            n.InitializeNode(maze, n.Position.y, n.Position.x); 
        }
    }
}