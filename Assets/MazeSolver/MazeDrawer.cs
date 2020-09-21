using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MazeDrawer : MonoBehaviour
{
    [SerializeField] private Vector2 drawingZero;
    [SerializeField] private float horizontalIncrement;
    [SerializeField] private float verticalIncrement;

    [SerializeField] private Maze maze;

    [SerializeField] private GameObject horizontalWall;
    [SerializeField] private GameObject verticalWall;

    [SerializeField] private List<GameObject> walls;

    public void DrawMaze()
    {
        GameObject hWall = Instantiate(horizontalWall, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < maze.horizontalWalls.GetLength(0); i++)
        {
            for (int j = 0; j < maze.horizontalWalls.GetLength(1);j++)
            {
                if (maze.horizontalWalls[i, j] == 1)
                {
                    GameObject newHWall = Instantiate(hWall,
                        new Vector2(drawingZero.x + (j * horizontalIncrement),
                            drawingZero.y + (i * verticalIncrement)), Quaternion.identity, this.transform);
                }
            }
        }
        
        GameObject vWall = Instantiate(verticalWall, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < maze.verticalsWalls.GetLength(0); i++)
        {
            for (int j = 0; j < maze.verticalsWalls.GetLength(1);j++)
            {
                if (maze.verticalsWalls[i, j] == 1)
                {
                    
                    /* This point is not easy to refactor, so the repetition will remain. */
                    GameObject newVWall = Instantiate(vWall,
                        new Vector2(drawingZero.x + (i * verticalIncrement),drawingZero.y + (j * horizontalIncrement)
                            ), Quaternion.identity, this.transform);
                }
            }
        }
        
        Destroy(hWall,0.1f);
        Destroy(vWall,0.1f);
    }

   
    
    // Start is called before the first frame update
    void Start()
    {
        DrawMaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
