namespace Mazes.Pathfinding;

public class Distances
{
    private Cell _root;
    private Dictionary<Cell, int> _cells;
    
    public Distances(Cell Root)
    {
        _root = Root;
        _cells = new Dictionary<Cell, int>();
        _cells.Add(_root, 0);
    }

    public int? this[Cell cell]
    {
        get
        {
            if (_cells.TryGetValue(cell, out int value))
            {
                return value;
            }
            return null;
        }
    }

    public void SetCell(Cell cell, int distance)
    {
        if (_cells.TryGetValue(cell, out int value))
        {
            _cells[cell] = distance;
        }
        else
        {
            _cells.Add(cell, distance);
        }
    }
    
    public Cell[] Cells => _cells.Keys.ToArray();

    public Distances PathTo(Cell goal)
    {
        var current = goal;

        var breadcrumbs = new Distances(_root);
        breadcrumbs.SetCell(current, _cells[current]);

        while (current != _root)
        {
            foreach (Cell neighbour in current.Links())
            {
                if (_cells[neighbour] < _cells[current])
                {
                    breadcrumbs.SetCell(neighbour, _cells[neighbour]);
                    current = neighbour;
                    break;
                }
            }
        }
        return breadcrumbs;
    }

    public KeyValuePair<Cell, int> Max()
    {
        var maxDistance = 0;
        var maxCell = _root;

        foreach (var cellDistance in _cells)
        {
            if (cellDistance.Value > maxDistance)
            {
                maxCell = cellDistance.Key;
                maxDistance = cellDistance.Value;
            }
        }
        return new KeyValuePair<Cell, int>(maxCell, maxDistance);
    }
}