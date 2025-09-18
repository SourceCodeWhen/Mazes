namespace Mazes.Masking;

public class Mask
{
    public int Rows { get; }
    public int Columns { get; }

    private bool[][] bits;

    private Random random = new Random();

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
                return null;
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

    public bool RandomLocation()
    {
        while (true)
        {
            var row = random.Next(Rows);
            var col = random.Next(Columns);
            if (this[row, col])
            {
                return true;
            }
        }
    }
}