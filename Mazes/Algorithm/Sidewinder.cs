namespace Mazes.Algorithm;

public static class Sidewinder
{
    private static Random _random = new Random();
    
    public static Grid On(Grid grid, int closeOut = 2)
    {
        foreach (Cell[] row in grid.EachRow())
        {
            List<Cell> run =  new List<Cell>();

            foreach (Cell cell in row)
            {
                run.Add(cell);

                bool atEasternBoundary = cell.East == null;
                bool atNorthernBoundary = cell.North == null;
                
                bool shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && _random.Next(closeOut) == 0);

                if (shouldCloseOut)
                {
                    Cell member = run[_random.Next(run.Count - 1)];
                    if (member.North != null)
                    {
                        member.Link(member.North);
                    }

                    run.Clear();
                }
                else
                {
                    cell.Link(cell.East);
                }
            }
        }

        return grid;
    }
}