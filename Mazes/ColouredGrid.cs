using Mazes.Pathfinding;

namespace Mazes;

public class ColouredGrid : Grid
{
    public Distances distances { get; set; }
    
    public ColouredGrid(int rows, int columns) : base(rows, columns)
    {
    }
}