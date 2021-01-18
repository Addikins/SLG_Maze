using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    MazeGenerator maze;
    public Cell targetCell;

    void Start()
    {
        maze = FindObjectOfType<MazeGenerator>();
    }

    public void FindPath()
    {
        ResetPath();
        List<Cell> cellPath = new List<Cell>();
        Cell currentCell = maze.GetCellAt(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        currentCell.visited = true;

        //Failsafe to break out of loop
        int i = 0;
        while (!cellPath.Contains(targetCell) && i < 500)
        {
            i++;
            if (i >= 500)
            {
                print("Hit Loop Max");
            }

            List<Cell> neighborCells = new List<Cell>();
            for (int j = 0; j < currentCell.neighbors.Count; j++)
            {
                Cell neighbor = currentCell.neighbors[j];
                if (neighbor.visited)
                { continue; }
                neighbor.cost = currentCell.cost + 1;
                neighbor.visited = true;

                neighborCells.Add(neighbor);

                currentCell = neighbor;
                if (neighbor == targetCell)
                {
                    cellPath.Add(neighbor);
                    break;
                }

                if (neighborCells.Count > 1)
                {
                    int cheapestCell = 0;
                    foreach (Cell adjacentCell in neighborCells)
                    {
                        if (cellPath.Count < 1 || cheapestCell == 0)
                        {
                            cheapestCell = adjacentCell.cost;
                            cellPath.Add(adjacentCell);
                            continue;
                        }
                        if (adjacentCell.cost < cheapestCell)
                        {
                            cellPath.RemoveAt(cellPath.Count - 1);
                            cellPath.Add(adjacentCell);
                            cheapestCell = adjacentCell.cost;
                        }
                    }
                    break;
                }
                cellPath.Add(neighbor);
            }
        }
        foreach (Cell cell in cellPath)
        {
            print(cell.name);
        }
    }

    private void ResetPath()
    {
        foreach (Cell cell in maze.cells)
        {
            cell.cost = 0;
            cell.visited = false;
        }
    }
}
