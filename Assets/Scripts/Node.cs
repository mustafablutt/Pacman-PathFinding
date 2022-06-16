using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    /// <summary>
    /// G cost is the current cost on the path used
    /// </summary>
    public int gCost;

    /// <summary>
    /// H cost is the heuristic cost (estimated cost)
    /// Calculation of h cost is based on Manhattan method
    /// </summary>
    public int hCost;

    /// <summary>
    /// F cost is the sum of h and g cost (f cost = g cost + h cost)
    /// </summary>
    public int FCost
    {
        get { return gCost + hCost; }
    }

    /// <summary>
    /// Parent object for the node
    /// </summary>
    public Node parentNode;

    /// <summary>
    /// X position of the node
    /// </summary>
    public int posX;

    /// <summary>
    /// Z position of the node
    /// </summary>
    public int posZ;

    /// <summary>
    /// state of the node enum could have been used also
    /// 0 = free, 1 = obstacle, 2 = start position, 3 = goal, 4 = pacman
    /// </summary>
    public int state;

    /// <summary>
    /// Determines whether to node is walkable or not
    /// </summary>
    public bool walkable;

    /// <summary>
    /// Constructor for the node
    /// </summary>
    /// <param name="_posX">X position</param>
    /// <param name="_posZ">Z position</param>
    /// <param name="_state">State</param>
    /// <param name="_walkable">isWalkable</param>
    public Node(int _posX, int _posZ, int _state, bool _walkable)
    {
        posX = _posX;
        posZ = _posZ;
        state = _state;
        walkable = _walkable;
    }
}