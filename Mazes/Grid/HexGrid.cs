using Raylib_cs;

namespace Mazes;

public class HexGrid : BaseGrid
{
    public override void PrepareGrid()
    {
        _cells = new HexCell[Rows][];
        for (int i = 0; i < Rows; i++)
        {
            _cells[i] = new HexCell[Columns];
            for (int j = 0; j < Columns; j++)
            {
                _cells[i][j] = new HexCell(i, j);
            }
        }
    }

    protected override void ConfigureCells()
    {
        foreach (HexCell cell in EachCell())
        {
            var row = cell.Row;
            var col = cell.Col;

            int northDiagonal = 0;
            int southDiagonal = 0;
            
            if (Int32.IsEvenInteger(col))
            {
                northDiagonal = row - 1;
                southDiagonal = row;
            }
            else
            {
                northDiagonal = row;
                southDiagonal = row + 1;
            }
            
            cell.NorthWest = (HexCell?)_cells[northDiagonal][col -1];
            cell.North = (HexCell?)_cells[row - 1][col];
            cell.NorthEast = (HexCell?)_cells[northDiagonal][col + 1];
            cell.SouthWest = (HexCell?)_cells[southDiagonal][col -1];
            cell.South = (HexCell?)_cells[row + 1][col];
            cell.SouthEast = (HexCell?)_cells[southDiagonal][col + 1];
            
        }
        
    }
    
}