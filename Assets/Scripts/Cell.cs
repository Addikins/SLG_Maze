using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int xPosition;
    public int yPosition;
    public bool visited = false;

    public Player player;

    public GameObject northWall;
    public GameObject southWall;
    public GameObject eastWall;
    public GameObject westWall;

    public List<Cell> neighbors = new List<Cell>();

    // Used to determine relative distance from start point
    public int cost;

    public enum Direction
    {
        North,
        South,
        East,
        West
    }
    public Direction direction;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.targetCell = this;
            player.FindPath();
        }
    }
}
