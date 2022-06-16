using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gridd : MonoBehaviour
{
    #region Parameters

    /// <summary>
    /// Game objects for covering the map for search algorithm
    /// </summary>
    public GameObject bottomLeft;

    public GameObject topRight;

    public GameObject start;

    public GameObject goal;

    /// <summary>
    /// 2d array for griding the game map
    /// </summary>
    private Node[,] myGrid;

    public List<Node> clydePath;
    public List<Node> blinkyPath;
    public List<Node> inkyPath;
    public List<Node> pinkyPath;

    public LayerMask unwalkable;

    //Grid info
    private int xStart, zStart;
    private int xEnd, zEnd;
    private int vCells, hCells; //amount of cells in the grid
    private int cellWidth = 1;
    private int cellHeight = 1;

    #endregion

    #region Methods

    private void Awake()
    {
        MPGridCreate();
    }

    void MPGridCreate()
    {
        xStart = (int) bottomLeft.transform.position.x;
        zStart = (int) bottomLeft.transform.position.z;

        xEnd = (int) topRight.transform.position.z;
        zEnd = (int) topRight.transform.position.z;

        //For calculating the numbers of cells
        hCells = (int) ((xEnd - xStart) / cellWidth) + 2;
        vCells = (int) ((zEnd - zStart) / cellHeight) + 1;
        //The grid array has been initialised with respect to numbers of cells
        myGrid = new Node[hCells + 1, vCells + 1];

        UpdateGrid();
    }

    public void UpdateGrid()
    {
        for (int i = 0; i <= hCells; i++)
        {
            for (int j = 0; j <= vCells; j++)
            {
                //Returns true if there are any colliders overlapping the sphere defined by position and radius in world coordinates.
                bool walkable = !(Physics.CheckSphere(new Vector3(xStart + i, 0, zStart + j), 0.3f, unwalkable));
                myGrid[i, j] = new Node(i, j, 0, walkable);
            }
        }
    }

    /// <summary>
    /// For visual debugging of nodes
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void OnDrawGizmos()
    {
        if (myGrid != null)
        {
            foreach (Node node in myGrid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.black;

                if (clydePath != null)
                {
                    if (clydePath.Contains(node))
                    {
                        Gizmos.color = Color.yellow;
                    }
                }

                if (inkyPath != null)
                {
                    if (inkyPath.Contains(node))
                    {
                        Gizmos.color = Color.cyan;
                    }
                }

                if (pinkyPath != null)
                {
                    if (pinkyPath.Contains(node))
                    {
                        Gizmos.color = Color.magenta;
                    }
                }

                if (blinkyPath != null)
                {
                    if (blinkyPath.Contains(node))
                    {
                        Gizmos.color = Color.red;
                    }
                }

                Gizmos.DrawWireCube(new Vector3(xStart + node.posX, 0.5f, zStart + node.posZ),
                    new Vector3(0.8f, 0.8f, 0.8f));
            }
        }
    }

    public Node NodeRequest(Vector3 pos)
    {
        int gridX = (int) Vector3.Distance(new Vector3(pos.x, 0, 0), new Vector3(xStart, 0, 0));
        int gridZ = (int) Vector3.Distance(new Vector3(0, 0, pos.z), new Vector3(0, 0, zStart));

        return myGrid[gridX, gridZ];
    }

    /// <summary>
    /// Finding the neighbor nodes correctly
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<Node> GetNeighborNodes(Node node)
    {
        List<Node> neighbors = new List<Node>();
        //find all neighbors in a 3 x 3 square around current node
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                //ignore following fields since we can't move diagonal
                if (x == 0 && z == 0)
                {
                    continue;
                }

                //ignore top left
                if (x == -1 && z == 1)
                {
                    continue;
                }

                //ignore top right
                if (x == 1 && z == 1)
                {
                    continue;
                }

                //ignore bottom left
                if (x == 1 && z == -1)
                {
                    continue;
                }

                //ignore bottom right
                if (x == -1 && z == -1)
                {
                    continue;
                }

                int checkPosX = node.posX + x;
                int checkPosZ = node.posZ + z;

                if (checkPosX > 0 && checkPosX < (hCells) && checkPosZ >= 0 &&
                    checkPosZ < (vCells))
                {
                    neighbors.Add(myGrid[checkPosX, checkPosZ]);
                }
            }
        }

        return neighbors;
    }

    #endregion

    public Vector3 NextPathPoint(Node node)
    {
        int gridX = (int) (xStart + node.posX);
        int gridZ = (int) (zStart + node.posZ);

        return new Vector3(gridX, 0, gridZ);
    }

    /// <summary>
    /// Chekcs if the considered grid by the ghost is inside the grid or not (Since pinky is trying to move ahead of pacman)
    /// </summary>
    /// <param name="requestedPosition"></param>
    /// <returns></returns>
    public bool ChechInsideGrid(Vector3 requestedPosition)
    {
        int gridX = (int) (requestedPosition.x - xStart);
        int gridZ = (int) (requestedPosition.z - zStart);

        if (gridX > hCells)
        {
            return false;
        }

        else if (gridX < 0)
        {
            return false;
        }

        else if (gridZ > vCells)
        {
            return false;
        }

        else if (gridZ < 0)
        {
            return false;
        }

        if (!NodeRequest(requestedPosition).walkable)
        {
            return false;
        }

        return true;
    }

    
    public Vector3 GetNearestNonWallNode(Vector3 target)
    {
        float min = 1000;
        int minIndexI = 0;
        int minIndexJ = 0;

        for (int i = 0; i < hCells; i++)
        {
            for (int j = 0; j < vCells; j++)
            {
                if (myGrid[i,j].walkable)
                {
                    Vector3 nextPoint = NextPathPoint(myGrid[i, j]);
                    float distance = Vector3.Distance(nextPoint, target);
                    if (distance<min)
                    {
                        min = distance;
                        minIndexI = i;
                        minIndexJ = j;
                    }
                }
            }
        }

        return NextPathPoint(myGrid[minIndexI, minIndexJ]);
    }
}