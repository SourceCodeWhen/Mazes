namespace Mazes.Algorithm;

public class Backtracker : BaseAlgo
{
    private Random random = new Random();

    private int currentMaxPath = 0;
    public BaseGrid On(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        return OnEnumerable(baseGrid, pairs).Last();
    }

    public IEnumerable<BaseGrid> OnEnumerable(BaseGrid baseGrid, SortedDictionary<string, int> pairs)
    {
        Stack<Cell> cellStack = new Stack<Cell>();
        cellStack.Push(baseGrid.RandomCell());
        cellStack.Peek().IsHead = true;
        yield return baseGrid;
        cellStack.Peek().IsHead = false;
        cellStack.Peek().PathNum = currentMaxPath;

        while (cellStack.Count > 0)
        {
            var current = cellStack.Peek();
            var neighbours = current.Neighbours().Where(x => x.Links().Count == 0).ToList();

            if (neighbours.Count == 0)
            {
                var popped = cellStack.Pop();
                popped.IsHead = false;
                
                currentMaxPath = (int)popped.PathNum -1;
                popped.PathNum = null;
                yield return baseGrid;
            }
            else
            {
                var neighbour = neighbours[random.Next(neighbours.Count)];
                current.Link(neighbour);
                currentMaxPath++;
                neighbour.PathNum = currentMaxPath;
                cellStack.Push(neighbour);
                neighbour.IsHead = true;
                current.IsHead = false;
                yield return baseGrid;
            }
        }

    }

    public SortedDictionary<string, int> PairOptions()
    {
        return new SortedDictionary<string, int>();
    }

    public bool ForceAnimateAlgorithm() => true;
}