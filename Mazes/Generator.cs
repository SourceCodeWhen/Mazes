using Mazes.Algorithm;

namespace Mazes;

public static class Generator
{
    public static void LongestPath(int rows, int columns)
    {
        var grid = new DistanceGrid(rows, columns);
        BinaryTree.On(grid);
        
        var start =  grid[0,0];
        
        var distances = start.Distances();
        var newStartDistance = distances.Max();

        var newDistances = newStartDistance.Key.Distances();
        var goalDistance = newDistances.Max();

        grid.distances = newDistances.PathTo(goalDistance.Key);
        
        Console.Write(grid.ToString());
    }

    public static Grid GenBinaryTree(int rows, int columns)
    {
        var grid = new DistanceGrid(rows,columns);
        BinaryTree.On(grid);
        return grid;
    }

    public static Grid GenSidewinder(int rows, int columns, int closeOut = 2)
    {
        var grid = new DistanceGrid(rows,columns);
        Sidewinder.On(grid, closeOut);
        return grid;
    }
}