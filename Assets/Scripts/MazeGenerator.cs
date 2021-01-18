using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Min(1)]
    public int width = 5;
    [Min(1)]
    public int height = 5;
    [HideInInspector]
    public int mapSize;
    int cellsVisited = 0;

    [HideInInspector]
    public bool mazeBuilt = false;

    [SerializeField]
    Cell cellPrefab;
    [SerializeField]
    Player player;

    [HideInInspector]
    public List<Cell> cells = new List<Cell>();
    List<Cell> visitedCells = new List<Cell>();

    Cell currentCell;
    Cell startCell;
    Cell exitCell;

    void Start()
    {
        mapSize = width * height;

        GenerateMap();
        currentCell = GetCellAt(0, 0);
        currentCell.visited = true;
        visitedCells.Add(currentCell);
        cellsVisited = 1;

        startCell = currentCell;
        startCell.southWall.SetActive(false);

    }

    void Update()
    {
        if (!mazeBuilt)
        {
            CreateMaze();
        }
    }

    private void GenerateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell newCell = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity, this.transform);
                newCell.xPosition = x;
                newCell.yPosition = y;
                newCell.name = string.Format("Cell ({0}, {1})", x, y);
                cells.Add(newCell);
            }
        }
    }

    private void CreateMaze()
    {
        Cell nextCell;
        if (cellsVisited < mapSize)
        {
            SetColor(currentCell, Color.white);
            currentCell = visitedCells[visitedCells.Count - 1];
            List<Cell> neighbors = new List<Cell>();

            //North
            if (currentCell.yPosition < height)
            {
                nextCell = GetCellAt(currentCell.xPosition, currentCell.yPosition + 1);
                if (nextCell && !nextCell.visited)
                {
                    nextCell.direction = Cell.Direction.North;
                    neighbors.Add(nextCell);
                }
            }
            //South
            if (currentCell.yPosition > 0)
            {
                nextCell = GetCellAt(currentCell.xPosition, currentCell.yPosition - 1);
                if (nextCell && !nextCell.visited)
                {
                    nextCell.direction = Cell.Direction.South;
                    neighbors.Add(nextCell);
                }
            }
            //East
            if (currentCell.xPosition < width)
            {
                nextCell = GetCellAt(currentCell.xPosition + 1, currentCell.yPosition);
                if (nextCell && !nextCell.visited)
                {
                    nextCell.direction = Cell.Direction.East;
                    neighbors.Add(nextCell);
                }
            }
            //West
            if (currentCell.xPosition > 0)
            {
                nextCell = GetCellAt(currentCell.xPosition - 1, currentCell.yPosition);
                if (nextCell && !nextCell.visited)
                {
                    nextCell.direction = Cell.Direction.West;
                    neighbors.Add(nextCell);
                }
            }

            if (neighbors.Count > 0)
            {
                Cell previousCell = currentCell;
                currentCell = neighbors[Random.Range(0, neighbors.Count)];
                previousCell.neighbors.Add(currentCell);
                currentCell.neighbors.Add(previousCell);


                switch (currentCell.direction)
                {
                    case Cell.Direction.North:
                        currentCell.southWall.SetActive(false);
                        previousCell.northWall.SetActive(false);
                        break;
                    case Cell.Direction.South:
                        currentCell.northWall.SetActive(false);
                        previousCell.southWall.SetActive(false);
                        break;
                    case Cell.Direction.East:
                        currentCell.westWall.SetActive(false);
                        previousCell.eastWall.SetActive(false);
                        break;
                    case Cell.Direction.West:
                        currentCell.eastWall.SetActive(false);
                        previousCell.westWall.SetActive(false);
                        break;
                }
                currentCell.visited = true;
                cellsVisited += 1;
                visitedCells.Add(currentCell);
            }
            else
            {
                visitedCells.RemoveAt(visitedCells.Count - 1);
            }
            SetColor(currentCell, Color.magenta);
        }
        else
        {
            exitCell = cells[cells.Count - 1];
            exitCell.northWall.SetActive(false);
            SetColor(exitCell, Color.red);
            SetColor(startCell, Color.green);
            SetColor(currentCell, Color.white);

            Player playerInstance = Instantiate(player, startCell.transform.position, Quaternion.identity);
            foreach (Cell cell in cells)
            {
                cell.player = playerInstance;
            }

            mazeBuilt = true;
        }
    }

    void SetColor(Cell cell, Color color)
    {
        SpriteRenderer renderer = cell.GetComponent<SpriteRenderer>();
        renderer.color = color;
    }

    public Cell GetCellAt(int x, int y)
    {
        return cells.SingleOrDefault(t =>
        {
            Cell cell = t.GetComponent<Cell>();
            return cell.xPosition == x && cell.yPosition == y;
        });
    }
}
