namespace Mazes.Algorithm;

public class HuntAndKill : BaseAlgo
{
    private Random random = new Random();
    
    public BaseGrid On(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        return OnEnumerable(baseGrid, pairs).Last();
    }

    public IEnumerable<BaseGrid> OnEnumerable(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        var current = baseGrid.RandomCell();
        bool finished = false;
        while (!finished)
        {
            var unvisitedNeighbours = current.Neighbours().Where(x => x.Links().Count == 0).ToList();

            if (unvisitedNeighbours.Any())
            {
                var neighbour = unvisitedNeighbours[random.Next(unvisitedNeighbours.Count)];
                current.Link(neighbour);
                current = neighbour;
                current.IsHead = true;
                yield return baseGrid;
                current.IsHead = false;
            }
            else
            {
                bool found = false;

                foreach (Cell cell in baseGrid.EachCell())
                {
                    var visitedNeighbours = cell.Neighbours().Where(x => x.Links().Any()).ToList();
                    if (cell.Links().Count == 0 && visitedNeighbours.Any())
                    {
                        found = true;
                        current = cell;
                        
                        var neighbour = visitedNeighbours[random.Next(visitedNeighbours.Count)];
                        current.Link(neighbour);
                        current.IsHead = true;
                        yield return baseGrid;
                        current.IsHead = false;
                        break;
                    }
                }

                if (!found)
                {
                    finished = true;
                }
            }
        }
        yield return baseGrid;
    }

    public SortedDictionary<string, int> PairOptions() => new SortedDictionary<string, int>();

    public bool ForceAnimateAlgorithm() => true;
}