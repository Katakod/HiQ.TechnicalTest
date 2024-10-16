namespace RadioCarSimulator.Core.Model;

// Handles the movement, position, and commands for the car in a given Room
public class Car : ICar
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public string Direction { get; private set; }

    private readonly IRoom _room;

    private static readonly string[] _possibleDirections = { "N", "E", "S", "W" }; // North, East, South, West

    public Car(int x, int y, string direction, IRoom room)
    {
        X = x;
        Y = y;
        Direction = direction ?? throw new ArgumentNullException(nameof(direction));
        _room = room ?? throw new ArgumentNullException(nameof(room));

        if (!_room.IsWithinBounds(x, y))
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                $"Initial position ({x},{y}) is out of bounds for the Room ({room.Width},{room.Height}).");
        }
    }

    public void TurnLeft()
    {
        int currentIndex = Array.IndexOf(_possibleDirections, Direction);
        Direction = _possibleDirections[(currentIndex + 3) % 4]; // Left turn (counter-clockwise)
    }

    public void TurnRight()
    {
        int currentIndex = Array.IndexOf(_possibleDirections, Direction);
        Direction = _possibleDirections[(currentIndex + 1) % 4]; // Right turn (clockwise)
    }

    public bool MoveForward()
    {
        int newX = X, newY = Y;

        switch (Direction)
        {
            case "N": newY++; break;
            case "E": newX++; break;
            case "S": newY--; break;
            case "W": newX--; break;
        }

        if (!_room.IsWithinBounds(newX, newY))
            return false;

        X = newX;
        Y = newY;

        return true;
    }

    public bool MoveBackward()
    {
        int newX = X, newY = Y;

        switch (Direction)
        {
            case "N": newY--; break;
            case "E": newX--; break;
            case "S": newY++; break;
            case "W": newX++; break;
        }

        if (!_room.IsWithinBounds(newX, newY))
            return false;

        X = newX;
        Y = newY;

        return true;
    }
}
