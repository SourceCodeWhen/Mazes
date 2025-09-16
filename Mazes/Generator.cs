using Mazes.Algorithm;

namespace Mazes;

public static class Generator
{
    public static void LongestPath()
    {
        var grid = new DistanceGrid(5, 5);
        BinaryTree.On(grid);
        
        var start =  grid[0,0];
        
        var distances = start.Distances();
        var newStartDistance = distances.Max();

        var newDistances = newStartDistance.Key.Distances();
        var goalDistance = newDistances.Max();

        grid.distances = newDistances.PathTo(goalDistance.Key);
        
        Console.Write(grid.ToString());
    }
}