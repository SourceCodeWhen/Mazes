using System.Text;
using System.Threading.Tasks.Dataflow;
using Mazes;
using Mazes.Algorithm;
using Mazes.Metadata;
using Mazes.Pathfinding;
using Raylib_cs;

int rows = 20;
int columns = 20;
int cellSize = 25;
float wallThickness = 3.0f;
float secondsSinceLastInput = 0.0f;
int targettedRow = rows / 2;
int targettedColumn = columns / 2;

int footPadding = 30;

int renderWidth = (rows * cellSize) + 1;
int renderHeight = (columns * cellSize) + footPadding;

int selectedAlgo = 0;
BaseAlgo[] algos = [new BinaryTree(), new Sidewinder(),  new Wilsons(), new AldousBroder()];

int selectedGrid = 0;
String[] grids = ["basegrid", "distancegrid", "colouredgrid"];
bool renderBackgrounds = false;

Raylib.InitWindow(renderWidth, renderHeight, "Maze");

GridBuilder gridBuilder = new GridBuilder();

var grid = gridBuilder.WithRows(rows).WithColumns(columns).WithType(grids[selectedGrid]).Build();

SortedDictionary<string, int> pairs = algos[selectedAlgo].PairOptions();

algos[selectedAlgo].On(grid, pairs);

bool animatePathfinding = false;
bool animateAlgorithm = false;

bool changedAlgoRecently = false;
bool changedGridRecently = false;
string footerText = Footer.CalculateFooter(grids[selectedGrid], algos[selectedAlgo].GetType().Name, pairs);
IEnumerator<Distances> distEnumerator = null;
IEnumerator<BaseGrid> baseGridEnumerator = null;
Raylib.SetTargetFPS(144);
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.White);
    if (Raylib.IsKeyDown(KeyboardKey.Space))
    {
        grid = gridBuilder.Build();
        algos[selectedAlgo].On(grid, pairs);
        distEnumerator = null;
            baseGridEnumerator = null;
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
            distEnumerator = null;
            baseGridEnumerator = null;
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
            distEnumerator = null;
            baseGridEnumerator = null;
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
            distEnumerator = null;
            baseGridEnumerator = null;
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
            distEnumerator = null;
            baseGridEnumerator = null;
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

            if (grids[selectedGrid].Equals("distancegrid") || grids[selectedGrid].Equals("colouredgrid"))
            {
                renderBackgrounds = true;
            }
            else
            {
                renderBackgrounds = false;
            }
            distEnumerator = null;
            baseGridEnumerator = null;
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
            if (selectedAlgo < algos.Length - 1)
            {
                selectedAlgo++;
            }
            else
            {
                selectedAlgo = 0;
            }

            grid = gridBuilder.Build();
            pairs = algos[selectedAlgo].PairOptions();
            if (!animateAlgorithm)
            {
                algos[selectedAlgo].On(grid, pairs);
            }

            footerText = Footer.CalculateFooter(grids[selectedGrid], algos[selectedAlgo].GetType().Name, pairs);
            distEnumerator = null;
            baseGridEnumerator = null;
        }
    }

    if (Raylib.IsKeyReleased(KeyboardKey.A))
    {
        changedAlgoRecently = false;
    }

    if (Raylib.IsKeyPressed(KeyboardKey.P))
    {
        animatePathfinding = !animatePathfinding;
    }

    if (Raylib.IsKeyPressed(KeyboardKey.O))
    {
        
        animateAlgorithm = !animateAlgorithm;
        
    }

    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        var mousePos = Raylib.GetMousePosition();
        targettedColumn = (int)mousePos.X / ((renderWidth - 1) / cellSize);
        targettedRow = (int) mousePos.Y / ((renderHeight - footPadding) / cellSize);
        if (targettedRow > grid.Rows || targettedRow < 0)
        {
            targettedRow = 0;
        }

        if (targettedColumn > grid.Columns || targettedColumn < 0)
        {
            targettedColumn = 0;
        }
        var targettedCell = grid[targettedRow, targettedColumn];
        if (targettedCell != null)
        {
            grid.distances = targettedCell.Distances();
            grid._rendered = false;
            distEnumerator = null;
            baseGridEnumerator = null;
        }
    }
    
    if (renderBackgrounds)
    {
        if (targettedRow > grid.Rows || targettedColumn > grid.Columns)
        {
            targettedRow = grid.Rows / 2;
            targettedColumn = grid.Columns / 2;
        }
        
        if (animatePathfinding)
        {
            if (distEnumerator == null)
            {
                distEnumerator = grid[targettedRow, targettedColumn].GetAnimatedDistance().GetEnumerator();
                grid.distances = distEnumerator.Current;
            }

            if (distEnumerator.MoveNext())
            {
                grid.distances = distEnumerator.Current;
            }
            else
            {
                animatePathfinding = false;
                distEnumerator = null;
            }

            grid._rendered = false;
        }
        else
        {
            if (!animateAlgorithm)
            {
                if (grid.distances == null)
                {
                    var start = grid[targettedRow, targettedColumn];
                    grid.distances = start.Distances();
                }
            }
        }
    }

    if (animateAlgorithm)
    { 
        if (baseGridEnumerator != null)
        {
            if (baseGridEnumerator.MoveNext())
            {
                grid = baseGridEnumerator.Current;
                grid._rendered = false;
            }
            else
            {
                baseGridEnumerator = null;
                animateAlgorithm = false;
            }
        }
        else
        {
                distEnumerator = null;
                baseGridEnumerator = algos[selectedAlgo].OnEnumerable(gridBuilder.Build(), pairs).GetEnumerator();
                baseGridEnumerator.MoveNext();
                grid = baseGridEnumerator.Current;
                grid._rendered = false;
        }
    }

    grid.toRaylib(renderWidth, renderHeight,animatePathfinding, cellSize, wallThickness, renderBackgrounds);
    Raylib.DrawText(footerText, 1, (columns * cellSize) + (footPadding / 2), 10, Color.Black);
    Raylib.DrawFPS((rows * cellSize) - 100, (columns * cellSize) + (footPadding / 5));
    Raylib.EndDrawing();
}

Raylib.CloseWindow();