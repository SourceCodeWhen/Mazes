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

int footPadding = 60;

int renderWidth = (rows * cellSize) + 1;
int renderHeight = (columns * cellSize) + footPadding;

int selectedAlgo = 0;
BaseAlgo[] algos = [new BinaryTree(), new Sidewinder(),  new Wilsons(), new AldousBroder(), new HuntAndKill(), new Backtracker()];

int selectedGrid = 0;
BaseGrid[] grids = [new ColouredGrid(rows, columns), new BaseGrid(rows, columns), new DistanceGrid(rows, columns) ];
bool renderBackgrounds = grids[selectedGrid].RenderBackgrounds();

int selectedColorMode = 0;
string[] colorMode = ["distance", "links"];

Raylib.InitWindow(renderWidth, renderHeight, "Maze");

var grid = grids[selectedGrid];

SortedDictionary<string, int> pairs = algos[selectedAlgo].PairOptions();

algos[selectedAlgo].On(grid, pairs);

bool animatePathfinding = false;
bool animateAlgorithm = algos[selectedAlgo].ForceAnimateAlgorithm();

bool changedAlgoRecently = false;
bool changedGridRecently = false;
string footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs, grid.DeadEnds().Length);
IEnumerator<Distances> distEnumerator = null;
IEnumerator<BaseGrid> baseGridEnumerator = null;
Raylib.SetTargetFPS(144);
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.White);
    if (Raylib.IsKeyDown(KeyboardKey.Space))
    {
        grid.Reset();
        algos[selectedAlgo].On((BaseGrid)grid, pairs);
        distEnumerator = null;
        baseGridEnumerator = null;
        footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs, grid.DeadEnds().Length);
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
            grid.Rows = rows;
            grid.Columns = columns;
            grid.Reset();
            algos[selectedAlgo].On(grid, pairs);
            footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs,grid.DeadEnds().Length);
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
            grid.Rows = rows;
            grid.Columns = columns;
            grid.Reset();
            algos[selectedAlgo].On(grid, pairs);
            footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs,grid.DeadEnds().Length);
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

            grid.Rows = rows;
            grid.Columns = columns;
            grid.Reset();
            algos[selectedAlgo].On(grid, pairs);
            footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs,grid.DeadEnds().Length);
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

            grid.Rows = rows;
            grid.Columns = columns;
            grid.Reset();
            algos[selectedAlgo].On(grid, pairs);
            footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs,grid.DeadEnds().Length);
            distEnumerator = null;
            baseGridEnumerator = null;
        }
        else
        {
            secondsSinceLastInput += Raylib.GetFrameTime();
        }
    }

    if (Raylib.IsKeyPressed(KeyboardKey.C))
    {
        if (selectedColorMode == colorMode.Length - 1)
        {
            selectedColorMode = 0;
        }
        else
        {
            selectedColorMode++;
        }

        grid._rendered = false;
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

            grid = grids[selectedGrid];
            grid.Reset();
            algos[selectedAlgo].On(grid, pairs);

            footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs,grid.DeadEnds().Length);

            renderBackgrounds = grid.RenderBackgrounds();
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

            grid.Reset();
            pairs = algos[selectedAlgo].PairOptions();
            animateAlgorithm = algos[selectedAlgo].ForceAnimateAlgorithm();
            if (!animateAlgorithm)
            {
                algos[selectedAlgo].On(grid, pairs);
            }
            
            footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs,grid.DeadEnds().Length);
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
                footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs,grid.DeadEnds().Length);
            }
            else
            {
                baseGridEnumerator = null;
                animateAlgorithm = false;
                animatePathfinding = true;
                footerText = Footer.CalculateFooter(grids[selectedGrid].GetType().Name, algos[selectedAlgo].GetType().Name, pairs,grid.DeadEnds().Length);
            }
        }
        else
        {
                distEnumerator = null;
                grid.Reset();
                baseGridEnumerator = algos[selectedAlgo].OnEnumerable(grid, pairs).GetEnumerator();
                baseGridEnumerator.MoveNext();
                grid = baseGridEnumerator.Current;
                grid._rendered = false;
        }
    }

    grid.toRaylib(renderWidth, renderHeight,animatePathfinding, cellSize, wallThickness, renderBackgrounds, colorMode[selectedColorMode]);
    Raylib.DrawText(footerText, 1, (columns * cellSize) + (footPadding / 5), 10, Color.Black);
    Raylib.DrawFPS((rows * cellSize) - 100, (columns * cellSize) + (footPadding / 5));
    Raylib.EndDrawing();
}

Raylib.CloseWindow();