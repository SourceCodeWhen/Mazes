namespace Mazes;

public class GridBuilder
{
    public GridBuilder()
    {
    }
    
    private static Random _rand = new Random();

    private int _rows = 4;
    private int _columns = 4;
    private string _gridType = "basegrid";

    public GridBuilder WithRows(int rows)
    {
        _rows = rows;
        return this;
    }

    public GridBuilder WithColumns(int columns)
    {
        _columns = columns;
        return this;
    }
    
    public GridBuilder BaseGrid()
    {
            _gridType = "basegrid";
            return this;
    }
    
    public GridBuilder BaseGrid(int rows, int columns)
    {
        _gridType = "basegrid";
        _rows = rows;
        _columns = columns;
        return this;
    }

    public GridBuilder DistanceGrid()
    {
        _gridType = "distancegrid";
        return this;
    }
    
    public GridBuilder DistanceGrid(int rows, int columns)
    {
        _gridType = "distancegrid";
        _rows = rows;
        _columns = columns;
        return this;
    }

    public GridBuilder ColouredGrid()
    {
        _gridType = "colouredgrid";
        return this;
    }

    public GridBuilder ColouredGrid(int rows, int columns)
    {
        _gridType = "colouredgrid";
        _rows = rows;
        _columns = columns;
        return this;
    }

    public GridBuilder WithType(String gridType)
    {
        _gridType = gridType;
        return this;
    }

    public BaseGrid Build()
    {
        switch (_gridType)
        {
            case "basegrid":
                return new BaseGrid(_rows, _columns);
            case "distancegrid":
                return new DistanceGrid(_rows, _columns);
            case "colouredgrid":
                return new ColouredGrid(_rows, _columns);
            default:
                return new BaseGrid(_rows,  _columns);
        }
    }
}