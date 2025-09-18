namespace Mazes.Algorithm;

public class BinaryTree : BaseAlgo
{
    private static Random _random = new Random();
    
    public BaseGrid On(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        foreach (Cell cell in baseGrid.EachCell())
        {
            List<Cell> neighbours = new List<Cell>();

            if (cell.North != null)
            {
                neighbours.Add(cell.North);
            }

            if (cell.East != null)
            {
                neighbours.Add(cell.East);
            }

            if (neighbours.Count >= 1)
            {
                var index = _random.Next(neighbours.Count);
                var neighbour = neighbours[index];
                cell.Link(neighbour);
            }

        }

        return baseGrid;
    }

    public IEnumerable<BaseGrid> OnEnumerable(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        foreach (Cell cell in baseGrid.EachCell())
        {
            List<Cell> neighbours = new List<Cell>();

            if (cell.North != null)
            {
                neighbours.Add(cell.North);
            }

            if (cell.East != null)
            {
                neighbours.Add(cell.East);
            }

            if (neighbours.Count >= 1)
            {
                var index = _random.Next(neighbours.Count);
                var neighbour = neighbours[index];
                cell.Link(neighbour);
            }

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
    
    public bool ForceAnimateAlgorithm() => true;
    
}