using ImageMagick;

namespace Mazes.Masking;

public class Mask
{
    public int Rows { get; }
    public int Columns { get; }

    private bool[][] bits;

    private Random random = new Random();

    public Mask(FileInfo fileInfo)
    {
        var image = new MagickImage(fileInfo);
        Rows = (int)image.Width;
        Columns =  (int)image.Height;
        bits = new bool[Rows][];
        var pixels = image.GetPixels();
        for (int i = 0; i < Rows; i++)
        {
            bits[i] = new bool[Columns];
            for (int j = 0; j < Columns; j++)
            {
                if (pixels[i, j].ToColor().Equals(MagickColors.Black))
                {
                    bits[i][j] = false;
                }
                else
                {
                    bits[i][j] = true;
                }
            }
        }
    }
    
    public Mask(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        bits = new bool[Rows][];
        for (int i = 0; i < Rows; i++)
        {
            bits[i] = new bool[Columns];
            for (int j = 0; j < Columns; j++)
            {
                bits[i][j] = true;
            }
        }
    }
    
    public bool this[int row, int col]
    {
        get
        {
            if (row < 0 || row > Rows - 1)
            {
                return false;
            }

            if (col < 0 || col > this[row].Length - 1)
            {
                return false;
            }

            return bits[row][col];
        }

        set
        {
            if (row < 0 || row > Rows - 1)
            {
                throw new IndexOutOfRangeException();
            }

            if (col < 0 || col > this[row].Length - 1)
            {
                throw new IndexOutOfRangeException();
            }
            
            bits[row][col] = value;
        }
    }

    public bool[] this[int row]
    {
        get
        {
            if (row < 0 || row > Rows - 1)
            {
                throw new IndexOutOfRangeException();
            }

            return bits[row];
        }
    }

    public int Count()
    {
        var count = 0;

        foreach (var row in bits)
        {
            foreach (var col in row)
            {
                if (col)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public KeyValuePair<int, int> RandomLocation()
    {
        while (true)
        {
            var row = random.Next(Rows);
            var col = random.Next(Columns);
            if (this[row, col])
            {
                return new  KeyValuePair<int, int>(row, col);
            }
        }
    }
}