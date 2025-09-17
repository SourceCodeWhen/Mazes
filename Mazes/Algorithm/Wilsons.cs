namespace Mazes.Algorithm;

public class Wilsons : BaseAlgo
{
    private static Random _random = new Random();


    public BaseGrid On(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        List<Cell> unvisited = new List<Cell>();
        unvisited.AddRange(baseGrid.EachCell());
        var first = unvisited[_random.Next(0, unvisited.Count)];
        unvisited.Remove(first);

        while (unvisited.Count > 0)
        {
            var cell = unvisited[_random.Next(0, unvisited.Count)];
            var path = new List<Cell>() { cell };

            while (unvisited.Contains(cell))
            {
                cell = cell.Neighbours()[_random.Next(0, cell.Neighbours().Count)];
                var position = path.IndexOf(cell);
                if (position != -1)
                {
                    path = path.Slice(0, position);
                }
                else
                {
                    path.Add(cell);
                }
            }

            for (int i = 0; i < path.Count - 2; i++)
            {
                path[i].Link(path[i + 1]);
                unvisited.Remove(path[i]);
            }
        }
        return baseGrid;
    }

    public IEnumerable<BaseGrid> OnEnumerable(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        List<Cell> unvisited = new List<Cell>();
        unvisited.AddRange(baseGrid.EachCell());
        var first = unvisited[_random.Next(0, unvisited.Count)];
        first.IsRoot = true;
        unvisited.Remove(first);

        while (unvisited.Count > 0)
        {
            var cell = unvisited[_random.Next(0, unvisited.Count)];
            var path = new List<Cell>() { cell };

            while (unvisited.Contains(cell))
            {
                cell = cell.Neighbours()[_random.Next(0, cell.Neighbours().Count)];
                var position = path.IndexOf(cell);
                if (position != -1)
                {
                    unvisited.ForEach(x => x.IsPath = false);
                    path = path.GetRange(0,  position + 1);
                    path.ForEach(x => x.IsPath = true);
                }
                else
                {
                    cell.IsPath = true;
                    path.Add(cell);
                }

                cell.IsHead = true;
                yield return baseGrid;
                cell.IsHead = false;
            }

            for (int i = 0; i <= path.Count - 2; i++)
            {
                path[i].Link(path[i + 1]);
                path[i].IsPath = false;
                path[i + 1].IsPath = false;
                unvisited.Remove(path[i]);
            }
            yield return baseGrid;
        }

        first.IsRoot = false;

        foreach (var cellio in baseGrid.EachCell().AsEnumerable())
        {
            cellio.IsPath = false;
        }
        
        yield return baseGrid;
    }

    public SortedDictionary<string, int> PairOptions()
    {
        return new SortedDictionary<string, int>();
    }
}