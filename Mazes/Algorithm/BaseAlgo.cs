namespace Mazes.Algorithm;

public interface BaseAlgo
{
    public BaseGrid On(BaseGrid baseGrid, SortedDictionary<string, int> pairs);
    
    public IEnumerable<BaseGrid> OnEnumerable(BaseGrid baseGrid, SortedDictionary<string, int> pairs);

    public SortedDictionary<string, int> PairOptions();
}