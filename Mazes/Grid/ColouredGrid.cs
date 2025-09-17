using Mazes.Pathfinding;
using Raylib_cs;

namespace Mazes;

public class ColouredGrid : BaseGrid
{
    public override Distances distances { get; set; }
    public int Maximum => distances.Max().Value;
    
    public ColouredGrid(int rows, int columns) : base(rows, columns)
    {
        
    }

    public override Color BackgroundColourForCell(Cell cell)
    {
        if (cell.IsHead)
        {
            return Color.Red;
        }

        if (cell.IsRoot)
        {
            return Color.Blue;
        }

        if (cell.IsPath)
        {
            return Color.Purple;
        }
        if (distances == null)
        {
            return Color.White;
        }
        var distance = distances[cell];
        if (distance == null)
        {
            return Color.White;
        }

        
        var intensity = (Maximum - (int)distance) / (float)Maximum;
        byte dark = (byte)(255 * intensity);
        byte bright = (byte)(128 + (127 * intensity));
        var outColour = new Color();
        outColour.R = dark;
        outColour.G = bright;
        outColour.B = dark;
        outColour.A = 255;
        return outColour;
    }
}