using System.Text;
using Mazes;
using Mazes.Algorithm;
using Mazes.Metadata;
using Raylib_cs;

int rows = 25;
int columns = 25;
int cellSize = 25;
float wallThickness = 3.0f;
float secondsSinceLastInput = 0.0f;

int footPadding = 30;

int renderWidth = (rows * cellSize) + 1;
int renderHeight = (columns * cellSize) + footPadding;

int selectedAlgo = 0;
BaseAlgo[] algos = [new BinaryTree(), new Sidewinder()];

int selectedGrid = 0;
String[] grids = ["basegrid", "distancegrid", "colouredgrid"];
bool renderBackgrounds = false;

Raylib.InitWindow(renderWidth, renderHeight, "Maze");

GridBuilder gridBuilder = new GridBuilder();

var grid = gridBuilder.WithRows(rows).WithColumns(columns).WithType(grids[selectedGrid]).Build();

SortedDictionary<string, int> pairs = algos[selectedAlgo].PairOptions();

algos[selectedAlgo].On(grid, pairs);

bool changedAlgoRecently = false;
bool changedGridRecently = false;
string footerText = Footer.CalculateFooter(grids[selectedGrid], algos[selectedAlgo].GetType().Name, pairs);
while (!Raylib.WindowShouldClose())
{
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);
        if (Raylib.IsKeyDown(KeyboardKey.Space))
        {
                grid = gridBuilder.Build();
                algos[selectedAlgo].On(grid, pairs);
        }
        if (Raylib.IsKeyDown(KeyboardKey.Up))
        {
                if (secondsSinceLastInput > 0.1f)
                {
                        secondsSinceLastInput = 0.0f;
                        rows += 1;
                        columns += 1;
                        renderHeight = (columns * cellSize) + footPadding;
                        renderWidth = (rows * cellSize) + 1;
                        Raylib.SetWindowSize(renderWidth, renderHeight);
                        grid = gridBuilder.WithRows(rows).WithColumns(columns).Build();
                        algos[selectedAlgo].On(grid, pairs);
                        footerText = Footer.CalculateFooter(grids[selectedGrid], algos[selectedAlgo].GetType().Name, pairs);
                }
                else
                {
                        secondsSinceLastInput += Raylib.GetFrameTime();                        
                }
        }
        if (Raylib.IsKeyDown(KeyboardKey.Down))
        {
                if (secondsSinceLastInput > 0.1f)
                {
                        secondsSinceLastInput = 0.0f;
                        rows -= 1;
                        columns -= 1;
                        renderHeight = (columns * cellSize) + footPadding;
                        renderWidth = (rows * cellSize) + 1;
                        Raylib.SetWindowSize(renderWidth, renderHeight);
                        grid = gridBuilder.WithRows(rows).WithColumns(columns).Build();
                        algos[selectedAlgo].On(grid, pairs);
                        footerText = Footer.CalculateFooter(grids[selectedGrid], algos[selectedAlgo].GetType().Name, pairs);
                }
                else
                {
                        secondsSinceLastInput += Raylib.GetFrameTime();
                }
        }

        if (Raylib.IsKeyDown(KeyboardKey.Left))
        {
                if (secondsSinceLastInput > 0.1f)
                {
                        secondsSinceLastInput = 0.0f;
                        if (pairs.Any())
                        {
                                if (pairs.First().Value != 1)
                                {
                                        pairs[pairs.First().Key] -= 1;
                                }
                        }
                        grid = gridBuilder.WithRows(rows).WithColumns(columns).Build();
                        algos[selectedAlgo].On(grid, pairs);
                        footerText = Footer.CalculateFooter(grids[selectedGrid], algos[selectedAlgo].GetType().Name, pairs);
                }
                else
                {
                        secondsSinceLastInput += Raylib.GetFrameTime();
                }
        }
        if (Raylib.IsKeyDown(KeyboardKey.Right))
        {
                if (secondsSinceLastInput > 0.1f)
                {
                        secondsSinceLastInput = 0.0f;
                        if (pairs.Any())
                        {
                                        pairs[pairs.First().Key] += 1;
                        }
                        grid = gridBuilder.WithRows(rows).WithColumns(columns).Build();
                        algos[selectedAlgo].On(grid, pairs);
                        footerText = Footer.CalculateFooter(grids[selectedGrid], algos[selectedAlgo].GetType().Name, pairs);
                        
                }
                else
                {
                        secondsSinceLastInput += Raylib.GetFrameTime();
                }
        }

        if (Raylib.IsKeyDown(KeyboardKey.Equal))
        {
                if (secondsSinceLastInput > 0.1f)
                {
                        secondsSinceLastInput = 0.0f;
                        cellSize += 1;
                        renderHeight = (columns * cellSize) + footPadding;
                        renderWidth = (rows * cellSize) + 1;
                        Raylib.SetWindowSize(renderWidth, renderHeight);
                        grid._rendered = false;
                }
                else
                {
                        secondsSinceLastInput += Raylib.GetFrameTime();
                }
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.Minus))
        {
                if (secondsSinceLastInput > 0.1f)
                {
                        secondsSinceLastInput = 0.0f;
                        if (cellSize != 1)
                        {
                                cellSize -= 1;
                        }
                        renderHeight = (columns * cellSize) + footPadding;
                        renderWidth = (rows * cellSize) + 1;
                        Raylib.SetWindowSize(renderWidth, renderHeight);
                        grid._rendered = false;
                }
                else
                {
                        secondsSinceLastInput += Raylib.GetFrameTime();
                }
        }

        if (Raylib.IsKeyDown(KeyboardKey.G))
        {
                if (!changedGridRecently)
                {
                        changedGridRecently = true;
                        if (selectedGrid < grids.Length - 1)
                        {
                                selectedGrid++;
                        }
                        else
                        {
                                selectedGrid = 0;
                        }
                        grid = gridBuilder.WithType(grids[selectedGrid]).Build();
                        algos[selectedAlgo].On(grid, pairs);
                        
                        footerText = Footer.CalculateFooter(grids[selectedGrid], algos[selectedAlgo].GetType().Name, pairs);
                        
                        if(grids[selectedGrid].Equals("distancegrid") ||  grids[selectedGrid].Equals("colouredgrid"))
                        {
                                renderBackgrounds = true;
                        }
                        else
                        {
                                renderBackgrounds = false;
                        }

                }
        }

        if (Raylib.IsKeyReleased(KeyboardKey.G))
        {
                changedGridRecently = false;
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
                if (!changedAlgoRecently)
                {
                        changedAlgoRecently = true;
                        if (selectedAlgo < algos.Length -1)
                        {
                                selectedAlgo++;
                        }
                        else
                        {
                                selectedAlgo = 0;
                        }
                        grid = gridBuilder.Build();
                        pairs = algos[selectedAlgo].PairOptions();
                        algos[selectedAlgo].On(grid, pairs);
                        
                        footerText = Footer.CalculateFooter(grids[selectedGrid], algos[selectedAlgo].GetType().Name, pairs);
                }
        }

        if (Raylib.IsKeyReleased(KeyboardKey.A))
        {
                changedAlgoRecently = false;
        }
        if (renderBackgrounds)
        {
                if (grid.distances == null)
                {
                        var start = grid[grid.Rows / 2, grid.Columns / 2];
                        grid.distances = start.Distances();
                }
        }
        grid.toRaylib(renderWidth, renderHeight, cellSize, wallThickness, renderBackgrounds);
        Raylib.DrawText(footerText, 1, (columns*cellSize) + (footPadding / 2), 10, Color.Black);
        Raylib.DrawFPS((rows*cellSize) -100, (columns*cellSize) + (footPadding / 5));
        Raylib.EndDrawing();
}

Raylib.CloseWindow();