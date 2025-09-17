// using Mazes.Algorithm;
//
// namespace Mazes;
//
// public static class Generator
// {
//     public static void LongestPath(int rows, int columns)
//     {
//         var grid = new ColouredGrid(rows, columns);
//         BinaryTree.On(grid, []);
//         
//         var start =  grid[0,0];
//         
//         var distances = start.Distances();
//         var newStartDistance = distances.Max();
//
//         var newDistances = newStartDistance.Key.Distances();
//         var goalDistance = newDistances.Max();
//
//         grid.distances = newDistances.PathTo(goalDistance.Key);
//         
//         Console.Write(grid.ToString());
//     }
//
//     public static BaseGrid GenBinaryTree(int rows, int columns)
//     {
//         var grid = new ColouredGrid(rows,columns);
//         BinaryTree.On(grid, []);
//         var start = grid[grid.Rows / 2, grid.Columns / 2];
//         grid.distances = start.Distances();
//         return grid;
//     }
//
//     public static BaseGrid GenSidewinder(int rows, int columns, int closeOut = 2)
//     {
//         var grid = new ColouredGrid(rows,columns);
//         Sidewinder.On(grid, closeOut);
//         var start = grid[grid.Rows / 2, grid.Columns / 2];
//         grid.distances = start.Distances();
//         return grid;
//     }
// }