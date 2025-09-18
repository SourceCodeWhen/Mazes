using Mazes.Pathfinding;

namespace Mazes;

public class DistanceGrid : BaseGrid
{
    public override Distances distances { get; set; }

    public DistanceGrid() : base()
    {
        
    }
    
    public DistanceGrid(int rows, int columns) : base(rows, columns)
    {
    }

    public override String ContentsOf(Cell? cell)
    {
        if (distances != null)
        {
            if (distances[cell] != null)
            {
                return distances[cell].Value.ToString("X1");
            }
        }
        return base.ContentsOf(cell);
    }

    public override void Reset()
    {
        distances = null;
        base.Reset();
    }
    
    public override bool RenderBackgrounds() => true;
}