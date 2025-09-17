using System.Collections.Concurrent;
using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using ImageMagick;
using ImageMagick.Drawing;
using Mazes.Pathfinding;
using Raylib_cs;

namespace Mazes;

public class BaseGrid
{
    public virtual Distances distances { get; set; }

    private RenderTexture2D _target;

    public Boolean _rendered = false;

    public int Rows { get; }
    public int Columns { get; }

    private Cell?[][] _cells;

    private Random _rand;

    public BaseGrid(int rows, int columns)
    {
        _rand = new Random();
        Rows = rows;
        Columns = columns;
        PrepareGrid();
        ConfigureCells();
    }

    private void PrepareGrid()
    {
        _cells = new Cell[Rows][];
        for (int row = 0; row < Rows; row++)
        {
            _cells[row] = new Cell[Columns];
            for (int column = 0; column < Columns; column++)
            {
                _cells[row][column] = new Cell(row, column);
            }
        }
    }

    private void ConfigureCells()
    {
        foreach (Cell?[] array in _cells)
        {
            foreach (Cell? cell in array)
            {
                var row = cell.Row;
                var col = cell.Col;

                if (row >= 1)
                {
                    cell.North = _cells[row - 1][col];
                }

                if (row < Rows - 1)
                {
                    cell.South = _cells[row + 1][col];
                }

                if (col >= 1)
                {
                    cell.West = _cells[row][col - 1];
                }

                if (col < Columns - 1)
                {
                    cell.East = _cells[row][col + 1];
                }
            }
        }
    }

    public Cell? this[int row, int col]
    {
        get
        {
            if (row < 0 || row > Rows - 1)
            {
                return null;
            }

            if (col < 0 || col > this[row].Length - 1)
            {
                return null;
            }

            return _cells[row][col];
        }
    }

    public Cell?[] this[int row]
    {
        get
        {
            if (row < 0 || row > Rows - 1)
            {
                return null;
            }

            return _cells[row];
        }
    }

    public Cell? RandomCell()
    {
        var row = _rand.Next(Rows - 1);
        var col = _rand.Next(this[row].Length - 1);
        return this[row, col];
    }

    public int Size()
    {
        return Rows * Columns;
    }

    public IEnumerable<Cell?[]> EachRow()
    {
        foreach (Cell?[] array in _cells)
        {
            yield return array;
        }
    }

    public IEnumerable<Cell> EachCell()
    {
        foreach (var array in this.EachRow())
        {
            foreach (var cell in array)
            {
                if (cell != null)
                {
                    yield return cell;
                }
            }
        }
    }

    public virtual String ContentsOf(Cell? cell)
    {
        return " ";
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();


        // Top of maze
        sb.Append("+");
        for (int i = 0; i < Columns; i++)
        {
            sb.Append("---+");
        }

        sb.Append(Environment.NewLine);

        String corner = "+";

        for (int row = 0; row < _cells.Length; row++)
        {
            StringBuilder top = new StringBuilder().Append("|");
            StringBuilder bottom = new StringBuilder().Append("+");

            for (int col = 0; col < _cells[row].Length; col++)
            {
                if (_cells[row][col] == null)
                {
                    _cells[row][col] = new Cell(-1, -1);
                }

                String body = $" {ContentsOf(_cells[row][col])} ";

                String eastboundary = _cells[row][col].Linked(_cells[row][col].East) ? " " : "|";

                top.Append(body);
                top.Append(eastboundary);

                String southboundary = _cells[row][col].Linked(_cells[row][col].South) ? "   " : "---";

                bottom.Append(southboundary);
                bottom.Append(corner);
            }

            sb.Append(top);
            sb.Append(Environment.NewLine);
            sb.Append(bottom);
            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }

    public void toPNG(int cellSize = 10)
    {
        uint width = (uint)(cellSize * Columns);
        uint height = (uint)(cellSize * Rows);

        using (var image = new MagickImage(MagickColors.White, width, height))
        {
            Drawables graphic = new Drawables();

            graphic.StrokeColor(MagickColors.Black);

            foreach (Cell cell in EachCell())
            {
                int x1 = cell.Col * cellSize;
                int y1 = cell.Row * cellSize;
                int x2 = (cell.Col + 1) * cellSize;
                int y2 = (cell.Row + 1) * cellSize;

                if (cell.North == null)
                {
                    graphic.Line(x1, y1, x2, y1);
                }

                if (cell.West == null)
                {
                    graphic.Line(x1, y1, x1, y2);
                }

                if (!cell.Linked(cell.East))
                {
                    graphic.Line(x2, y1, x2, y2);
                }

                if (!cell.Linked(cell.South))
                {
                    graphic.Line(x1, y2, x2, y2);
                }
            }

            graphic.Draw(image);
            image.Write("/home/jack/Pictures/Mazes/" + "export_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".jpg");
        }
    }

    public virtual Color BackgroundColourForCell(Cell cell)
    {
        if (cell.IsHead)
        {
            return Color.Red;
        }
        return Color.White;
    }

    public void toRaylib(int renderWidth, int renderHeight, bool animated, int cellSize = 10,
        float wallThickness = 2.0f, bool backgrounds = false)
    {
        if (_rendered == false)
        {
            _target = Raylib.LoadRenderTexture(renderWidth, renderHeight);
            Raylib.BeginTextureMode(_target);

            if (backgrounds)
            {
                RenderBackground(cellSize);
            }

            RenderWalls(cellSize, wallThickness);

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

    private void RenderBackground(int cellSize)
    {
        ConcurrentBag<(int x1, int y1, Color colour)>? _cellBag = new ConcurrentBag<(int x1, int y1, Color color)>();

        Parallel.ForEach(EachCell(), cell =>
        {
            int x1 = cell.Col * cellSize;
            int y1 = cell.Row * cellSize;

            var colour = BackgroundColourForCell(cell);

            _cellBag.Add((x1, y1, colour));
        });

        foreach (var cell in _cellBag)
        {
            Raylib.DrawRectangle(cell.x1, cell.y1, cellSize, cellSize, cell.colour);
        }
    }

    private void RenderWalls(int cellSize, float wallThickness)
    {
        foreach (Cell cell in EachCell())
        {
            int x1 = cell.Col * cellSize;
            int y1 = cell.Row * cellSize;
            int x2 = (cell.Col + 1) * cellSize;
            int y2 = (cell.Row + 1) * cellSize;

            if (cell.North == null)
            {
                Raylib.DrawLineEx(new Vector2(x1, y1), new Vector2(x2, y1), wallThickness, Color.Black);
            }

            if (cell.West == null)
            {
                Raylib.DrawLineEx(new Vector2(x1, y1), new Vector2(x1, y2), wallThickness, Color.Black);
            }

            if (!cell.Linked(cell.East))
            {
                Raylib.DrawLineEx(new Vector2(x2, y1), new Vector2(x2, y2), wallThickness, Color.Black);
            }

            if (!cell.Linked(cell.South))
            {
                Raylib.DrawLineEx(new Vector2(x1, y2), new Vector2(x2, y2), wallThickness, Color.Black);
            }
        }
    }
}