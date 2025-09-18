namespace Mazes.Algorithm;

public class Sidewinder : BaseAlgo
{
    private static Random _random = new Random();
    
    public BaseGrid On(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        int closeOut = 2;
        if (pairs.TryGetValue("CLOSEOUT", out var value))
        {
            closeOut = value;
        }

        foreach (Cell[] row in baseGrid.EachRow())
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

        return baseGrid;
    }

    public IEnumerable<BaseGrid> OnEnumerable(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        int closeOut = 2;
        if (pairs.TryGetValue("CLOSEOUT", out var value))
        {
            closeOut = value;
        }

        foreach (Cell[] row in baseGrid.EachRow())
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

                cell.IsHead = true;
                yield return baseGrid;
                cell.IsHead = false;
            }
        }

        yield return baseGrid;
    }

    public SortedDictionary<string, int> PairOptions()
    {
        return new SortedDictionary<string, int>() { {"CLOSEOUT", 2} };
    }

    public bool ForceAnimateAlgorithm() => true;
}