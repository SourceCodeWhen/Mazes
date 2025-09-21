using System.Collections.Concurrent;
using System.Numerics;
using System.Xml;
using Mazes.Pathfinding;
using Raylib_cs;

namespace Mazes;

public class PolarGrid : BaseGrid
{
    private Random random = new Random();
    public override Distances distances { get; set; }
    public int Maximum => distances.Max().Value;
    public int MaxPath => EachCell().Max(x => x.PathNum ?? 0);

    private int imgSize = 0;
    private int center = 0;

    public PolarGrid(int rows) 
    {
        Rows = rows;
        PrepareGrid();
        ConfigureCells();
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
        _rendered = false;
        PrepareGrid();
        ConfigureCells();
    }
    public override bool RenderBackgrounds() => true;
    
    public override void toRaylib(int renderWidth, int renderHeight, bool animated, int cellSize = 10,
        float wallThickness = 2.0f, bool backgrounds = false, string colourMode = "distance")
    {
        cellSize = cellSize / 2;
        imgSize = 2 * Rows * cellSize;
        center = imgSize / 2;
        
        if (_rendered == false)
        {
            Raylib.UnloadRenderTexture(_target);
            _target = Raylib.LoadRenderTexture(renderWidth, renderHeight);
            Raylib.BeginTextureMode(_target);

            if (backgrounds)
            {
                RenderBackground(cellSize, colourMode);
            }

            RenderWalls(cellSize, wallThickness);

            Raylib.DrawCircleLines(center, center, Rows * cellSize, Color.Black);
            
            Raylib.EndTextureMode();

            _rendered = true;
        }

        Raylib.DrawTextureRec(_target.Texture, new Rectangle()
        {
            X = 0,
            Y = 0,
            Width = (float)_target.Texture.Width,
            Height = (float)-_target.Texture.Height
        }, new Vector2(0, 0), Color.White);
    }

    protected virtual void RenderBackground(int cellSize, string colourMode)
    {
        ConcurrentBag<(int innerRadius, double thetaCcw,double thetaCw, Color colour)>? _cellBag = new ConcurrentBag<(int innerRadius,double thetaCcw,double thetaCw, Color color)>();

        
        Parallel.ForEach(EachCell(), cell =>
        {
            var theta = 2 * Math.PI / _cells[cell.Row].Length;
            var innerRadius = cell.Row * cellSize;
            var outerRadius = (cell.Row + 1) * cellSize;
            var thetaCcw = cell.Col * theta;
            var thetaCw = (cell.Col + 1) * theta;
            
            var ax = center + (int)(innerRadius * Math.Cos(thetaCcw));
            var ay = center + (int)(innerRadius * Math.Sin(thetaCcw));
            var bx = center + (int)(outerRadius * Math.Cos(thetaCcw));
            var by = center + (int)(outerRadius * Math.Sin(thetaCcw));

            var cx = center + (int)(innerRadius * Math.Cos(thetaCw));
            var cy = center + (int)(innerRadius * Math.Sin(thetaCw));
            var dx = center + (int)(outerRadius * Math.Cos(thetaCw));
            var dy = center + (int)(outerRadius * Math.Sin(thetaCw));
            
            var colour = BackgroundColourForCell(cell, colourMode);

            // _cellBag.Add((outerRadius, Math.Cos(thetaCcw) * outerRadius, Math.Sin(thetaCw) * outerRadius, colour));
            _cellBag.Add((outerRadius, thetaCcw * (180/Math.PI), thetaCw * (180/Math.PI), colour));

            // _cellBag.Add((cx,cy,dx,dy, colour));
        });
        
        var orderedCells = _cellBag.OrderByDescending(x => x.innerRadius).ToList();
        
        foreach (var cell in orderedCells)
        {
            Raylib.DrawCircleSector(new Vector2(center, center),cell.innerRadius,  (float)cell.thetaCw,(float)cell.thetaCcw, 1, cell.colour );
            // Raylib.DrawCircleSector(new Vector2(center, center),cell.innerRadius - cellSize,  (float)cell.thetaCw,(float)cell.thetaCcw, 1, Color.White);

            // Raylib.DrawRectangle(cell.x1, cell.y1, cellSize, cellSize, cell.colour);
        }
    }

    protected virtual void RenderWalls(int cellSize, float wallThickness)
    {
        foreach (PolarCell cell in EachCell())
        {
            if (cell.Row == 0)
            {
                continue;
            }

            var theta = 2 * Math.PI / _cells[cell.Row].Length;
                var innerRadius = cell.Row * cellSize;
                var outerRadius = (cell.Row + 1) * cellSize;
                var thetaCcw = cell.Col * theta;
                var thetaCw = (cell.Col + 1) * theta;

                var ax = center + (int)(innerRadius * Math.Cos(thetaCcw));
                var ay = center + (int)(innerRadius * Math.Sin(thetaCcw));
                var bx = center + (int)(outerRadius * Math.Cos(thetaCcw));
                var by = center + (int)(outerRadius * Math.Sin(thetaCcw));

                var cx = center + (int)(innerRadius * Math.Cos(thetaCw));
                var cy = center + (int)(innerRadius * Math.Sin(thetaCw));
                var dx = center + (int)(outerRadius * Math.Cos(thetaCw));
                var dy = center + (int)(outerRadius * Math.Sin(thetaCw));

                if (!cell.Linked(cell.inward))
                {
                    Raylib.DrawLineEx(new Vector2(ax, ay), new Vector2(cx, cy), wallThickness, Color.Black);
                }

                if (!cell.Linked(cell.cw))
                {
                    Raylib.DrawLineEx(new Vector2(cx, cy), new Vector2(dx, dy), wallThickness, Color.Black);
                }
        }
    }

    public override void PrepareGrid()
    {
        List<PolarCell?[]> rows = new List<PolarCell?[]>{ new PolarCell?[]{new PolarCell(0, 0)}};

        var rowHeight = 1.0 / Rows;
        
        for (int i = 1; i < Rows; i++)
        {
            var radius = (float)i / Rows;
            var circumference = 2 * Math.PI * radius;

            var previousCount = rows[i - 1].Length;
            
            var estimatedCellWidth = circumference / previousCount;
            var ratio = (int)Math.Round(estimatedCellWidth / rowHeight);

            var cells = previousCount * ratio;
            rows.Add(new PolarCell[cells]);

            for (int j = 0; j < cells; j++)
            {
                rows[i][j] = new PolarCell(i, j);
            }
        }

        _cells = rows.ToArray();
    }

    protected override void ConfigureCells()
    {
        foreach (PolarCell cell in EachCell())
        {
            var row = cell.Row;
            var col = cell.Col;

            if (row > 0)
            {
                cell.cw = (PolarCell)this[row, col + 1];
                cell.ccw = (PolarCell)this[row, col -1];

                var ratio = (float)_cells[row].Length / (float)_cells[row - 1].Length;
                var parent = (PolarCell)_cells[row - 1][col / (int)ratio];
                parent.outward.Add(cell);
                cell.inward = parent;
            }
        }
    }
    
    public override Cell? this[int row, int col]
    {
        get
        {
            if (_cells.Length == 0)
            {
                return null;
            }
            if (row < 0 || row > Rows - 1)
            {
                return null;
            }

            if (col < 0)
            {
                return _cells[row][0];
            }

            return _cells[row][col % _cells[row].Length];
        }
    }
    
    public override Cell?[] this[int row]
    {
        get
        {
            if (_cells.Length == 0)
            {
                return null;
            }
            
            if (row < 0 || row > Rows - 1)
            {
                return null;
            }

            return _cells[row];
        }
    }

    public override Cell RandomCell()
    {
        var row = random.Next(Rows);
        var col = random.Next(Columns);
        return _cells[row][col];
    }
}