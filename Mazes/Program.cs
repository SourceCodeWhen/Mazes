using Mazes;
using Mazes.Algorithm;
using Raylib_cs;

int rows = 10;
int columns = 10;
int cellSize = 100;
int sideWinderCloseOut = 2;
bool firstRender = true;
float wallThickness = 3.0f;
float secondsSinceLastRescale = 0.0f;

Raylib.InitWindow((rows*cellSize) + 1, (columns*cellSize) + 1, "Maze");

// var grid = Generator.GenBinaryTree(rows, columns);
var grid = Generator.GenSidewinder(rows, columns);



while (!Raylib.WindowShouldClose())
{
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);
        if (Raylib.IsKeyDown(KeyboardKey.Space))
        {
                grid = Generator.GenSidewinder(rows, columns, sideWinderCloseOut);
        }
        if (Raylib.IsKeyDown(KeyboardKey.Up))
        {
                if (secondsSinceLastRescale > 0.1f)
                {
                        secondsSinceLastRescale = 0.0f;
                        rows += 1;
                        columns += 1;
                        Raylib.SetWindowSize((rows * cellSize) + 1, (columns * cellSize) + 1);
                        grid = Generator.GenSidewinder(rows, columns, sideWinderCloseOut);
                }
                else
                {
                        secondsSinceLastRescale += Raylib.GetFrameTime();                        
                }
        }
        if (Raylib.IsKeyDown(KeyboardKey.Down))
        {
                if (secondsSinceLastRescale > 0.1f)
                {
                        secondsSinceLastRescale = 0.0f;
                        rows -= 1;
                        columns -= 1;
                        Raylib.SetWindowSize((rows * cellSize) + 1, (columns * cellSize) + 1);
                        grid = Generator.GenSidewinder(rows, columns, sideWinderCloseOut);
                }
                else
                {
                        secondsSinceLastRescale += Raylib.GetFrameTime();
                }
        }

        if (Raylib.IsKeyDown(KeyboardKey.Left))
        {
                if (secondsSinceLastRescale > 0.1f)
                {
                        secondsSinceLastRescale = 0.0f;
                        if (sideWinderCloseOut != 1)
                        {
                                sideWinderCloseOut -= 1;
                        }

                        grid = Generator.GenSidewinder(rows, columns, sideWinderCloseOut);
                }
                else
                {
                        secondsSinceLastRescale += Raylib.GetFrameTime();
                }
        }
        if (Raylib.IsKeyDown(KeyboardKey.Right))
        {
                if (secondsSinceLastRescale > 0.1f)
                {
                        secondsSinceLastRescale = 0.0f;
                        sideWinderCloseOut += 1;
                        grid = Generator.GenSidewinder(rows, columns, sideWinderCloseOut);
                }
                else
                {
                        secondsSinceLastRescale += Raylib.GetFrameTime();
                }
        }

        if (Raylib.IsKeyDown(KeyboardKey.Equal))
        {
                if (secondsSinceLastRescale > 0.1f)
                {
                        secondsSinceLastRescale = 0.0f;
                        cellSize += 1;
                        Raylib.SetWindowSize((rows * cellSize) + 1, (columns * cellSize) + 1);
                }
                else
                {
                        secondsSinceLastRescale += Raylib.GetFrameTime();
                }
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.Minus))
        {
                if (secondsSinceLastRescale > 0.1f)
                {
                        secondsSinceLastRescale = 0.0f;
                        if (cellSize != 1)
                        {
                                cellSize -= 1;
                        }
                        Raylib.SetWindowSize((rows * cellSize) + 1, (columns * cellSize) + 1);
                }
                else
                {
                        secondsSinceLastRescale += Raylib.GetFrameTime();
                }
        }
        grid.toRaylib(cellSize, wallThickness, false);
        Raylib.EndDrawing();
}

Raylib.CloseWindow();

// Generator.LongestPath(rows, columns);


// var grid = new DistanceGrid(5,5);
// // BinaryTree.On(grid);
// Sidewinder.On(grid);
//
// Cell start = grid[0,0];
// var distances = start.Distances();
// grid.distances = distances;
//
// Console.Write(grid.ToString());
//
// grid.distances = distances.PathTo(grid[grid.Rows -1, 0]);
//
// Console.Write(grid.ToString());
//
// // grid.toPNG(3);