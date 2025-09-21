namespace Mazes;

public class HexCell : Cell
{
    public HexCell? NorthEast { get; set; }
    public HexCell? NorthWest { get; set; }
    public HexCell? SouthEast { get; set; }
    public HexCell? SouthWest { get; set; }
    
    
    public HexCell(int row, int col) : base(row, col)
    {
    }

    public override List<Cell> Neighbours()
    {
        List<Cell> neighbours = new List<Cell>();
        if (NorthEast != null)
        {
            neighbours.Add(NorthEast);
        }

        if (NorthWest != null)
        {
            neighbours.Add(NorthWest);
        }

        if (SouthEast != null)
        {
            neighbours.Add(SouthEast);
        }

        if (SouthWest != null)
        {
            neighbours.Add(SouthWest);
        }
        
        return neighbours;
    }
}