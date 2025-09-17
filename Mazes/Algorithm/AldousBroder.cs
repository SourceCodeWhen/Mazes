namespace Mazes.Algorithm;

public class AldousBroder : BaseAlgo
{
    private Random _rand = new Random();
    
    public BaseGrid On(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        Cell cell = baseGrid.RandomCell();
        int unvisited = baseGrid.Size() - 1;

        while (unvisited > 0)
        {
            Cell neighbour = cell.Neighbours()[_rand.Next(0, cell.Neighbours().Count -1)];

            if (neighbour.Links().Count == 0)
            {
                cell.Link(neighbour);
                unvisited--;
            }
            
            cell = neighbour;
        }
        return baseGrid;
    }

    public IEnumerable<BaseGrid> OnEnumerable(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        Cell cell = baseGrid.RandomCell();
        int unvisited = baseGrid.Size() - 1;

        while (unvisited > 0)
        {
            Cell neighbour = cell.Neighbours()[_rand.Next(0, cell.Neighbours().Count)];

            if (!neighbour.Links().Any())
            {
                cell.Link(neighbour);
                unvisited--;
            }
            
            cell = neighbour;
            cell.IsHead = true;
            yield return baseGrid;
            cell.IsHead = false;
        }
        yield return baseGrid;
    }

    public SortedDictionary<string, int> PairOptions()
    {
        return new SortedDictionary<string, int>();
    }
}