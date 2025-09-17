using System.Text;

namespace Mazes.Metadata;

public static class Footer
{
    public static string CalculateFooter(string GridType,string AlgorithmType, SortedDictionary<string, int> pairs)
    {
        StringBuilder sb = new  StringBuilder();
        sb.Append("Grid: ");
        sb.Append(GridType);
        sb.Append(" - Algorithm: ");
        sb.Append(AlgorithmType);
        if (pairs.Any())
        {
            foreach (var pair in pairs)
            {
                sb.Append(" - ");
                sb.Append(pair.Key);
                sb.Append(": ");
                sb.Append(pair.Value);
            }
        }
        return sb.ToString();
    }
}