namespace Mazes;

public class PolarCell : Cell
{
    public PolarCell? cw { get; set; }
    public PolarCell? ccw { get; set; }
    public PolarCell? inward { get; set; }
    public List<PolarCell> outward { get; }
    
    public PolarCell(int row, int col) : base(row, col)
    {
        outward = new List<PolarCell>();
    }

    public override List<Cell> Neighbours()
    {
        List<Cell> neighbours = new List<Cell>();
        if (cw != null)
        {
            neighbours.Add(cw);
        }

        if (ccw != null)
        {
            neighbours.Add(ccw);
        }

        if (inward != null)
        {
            neighbours.Add(inward);
        }
        
        neighbours.AddRange(outward);
        
        return neighbours;
    }
}