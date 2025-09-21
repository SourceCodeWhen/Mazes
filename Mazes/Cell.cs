using System.ComponentModel;
using System.Runtime.InteropServices.Swift;
using Mazes.Pathfinding;

namespace Mazes;

public class Cell
{
    public Cell(int row, int col)
    {
        Row = row;
        Col = col;
        LinkMap = new();
    }

    public int Row { get; }
    public int Col { get; }
    public Cell? North { get; set; }
    public Cell? South { get; set; }
    public Cell? East { get; set; }
    public Cell? West { get; set; }
    private Dictionary<Cell,Boolean> LinkMap { get; set; }

    public bool IsHead { get; set; } = false;
    public bool IsRoot { get; set; } = false;
    public int? PathNum { get; set; } = null;

    public void Link(Cell cell, Boolean bidi = true)
    {
        LinkMap.Add(cell, true);
        if (bidi)
        {
            cell.Link(this, false);
        }
    }

    public void Unlink(Cell cell, Boolean bidi = true)
    {
        LinkMap.Remove(cell);
        if (bidi)
        {
            cell.Unlink(this, false);
        }
    }

    public List<Cell> Links()
    {
        return LinkMap.Keys.ToList();
    }

    public Boolean Linked(Cell? cell)
    {
        if (cell is null)
        {
            return false;
        }
        
        return LinkMap.ContainsKey(cell);
    }

    public virtual List<Cell> Neighbours()
    {
        List<Cell> neighbours = new();
        if (North != null)
        {
            neighbours.Add(North);
        }

        if (South != null)
        {
            neighbours.Add(South);
        }

        if (East != null)
        {
            neighbours.Add(East);
        }

        if (West != null)
        {
            neighbours.Add(West);
        }
        return neighbours;
    }

    public Distances Distances()
    {
        Distances distances = new Distances(this);

        List<Cell> frontier = new List<Cell> { this };

        while (frontier.Any())
        {
            List<Cell> newFrontier = new();

            foreach (Cell cell in frontier)
            {
                foreach (Cell linked in cell.Links())
                {
                    if (distances[linked] != null)
                    {
                        continue;
                    }
                    distances.SetCell(linked, (int)distances[cell]! + 1);
                    newFrontier.Add(linked);
                }
            }

            frontier = newFrontier;
        }
        
        return distances;
    }

    public IEnumerable<Distances> GetAnimatedDistance()
    {
        Distances distances = new Distances(this);

        List<Cell> frontier = new List<Cell> { this };

        while (frontier.Any())
        {
            List<Cell> newFrontier = new();

            foreach (Cell cell in frontier)
            {
                foreach (Cell linked in cell.Links())
                {
                    if (distances[linked] != null)
                    {
                        continue;
                    }
                    distances.SetCell(linked, (int)distances[cell]! + 1);
                    newFrontier.Add(linked);

                    yield return distances;
                }
            }

            frontier = newFrontier;
        }
        
        // yield return distances;
    }
}