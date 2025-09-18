using Mazes.Pathfinding;
using Raylib_cs;

namespace Mazes;

public class ColouredGrid : BaseGrid
{
    public override Distances distances { get; set; }
    public int Maximum => distances.Max().Value;
    public int MaxPath => EachCell().Max(x => x.PathNum ?? 0);

    public ColouredGrid() : base()
    {
        
    }
    
    public ColouredGrid(int rows, int columns) : base(rows, columns)
    {
        
    }

    public override Color BackgroundColourForCell(Cell cell, string colourMode)
    {
        if (cell.IsHead)
        {
            return Color.Red;
        }

        if (cell.IsRoot)
        {
            return Color.Blue;
        }

        if (cell.PathNum != null)
        {
            return CalculateColour((int)cell.PathNum, "path");
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

        if (colourMode == "distance")
        {
            return CalculateColour((int)distance, colourMode);
        }

        if (colourMode == "links")
        {
            return CalculateColour(cell.Links().Count, colourMode);
        }
        
            return CalculateColour((int)distance, colourMode);
    }

    private Color CalculateColour(int scaler, string type)
    {
        var max = 0;
        if (type == "path")
        {
            max = MaxPath;
        }
        else if (type == "distance")
        {
            max = Maximum;
        }
        else if (type == "links")
        {
            max = 4;
        }
        var intensity = (max - (int)scaler) / (float)max;
        byte dark = (byte)(255 * intensity);
        byte bright = (byte)(128 + (127 * intensity));
        var outColour = new Color();
        outColour.R = dark;
        outColour.G = bright;
        outColour.B = dark;
        outColour.A = 255;
        return outColour;
    }

    public override void Reset()
    {
        distances = null;
        base.Reset();
    }
    public override bool RenderBackgrounds() => true;
}