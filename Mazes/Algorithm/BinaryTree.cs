namespace Mazes.Algorithm;

public static class BinaryTree
{
    private static Random _random = new Random();
    
    public static Grid On(Grid grid)
    {
        foreach (Cell cell in grid.EachCell())
        {
            List<Cell> neighbours = new List<Cell>();

            if (cell.North != null)
            {
                neighbours.Add(cell.North);
            }

            if (cell.East != null)
            {
                neighbours.Add(cell.East);
            }

            if (neighbours.Count >= 1)
            {
                var index = _random.Next(neighbours.Count);
                var neighbour = neighbours[index];
                cell.Link(neighbour);
            }

        }

        return grid;
    }
}