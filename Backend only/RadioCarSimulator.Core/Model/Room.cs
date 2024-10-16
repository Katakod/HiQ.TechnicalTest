namespace RadioCarSimulator.Core.Model;

// Defines the boundary of the room where a car moves
public class Room : IRoom
{
    public int Width { get; }
    public int Height { get; }

    public Room(int width, int height)
    {
        Width = width < 1 ? throw new ArgumentOutOfRangeException(nameof(width), "Width of a Room must be greater than 0.") : width;
        Height = height < 1 ? throw new ArgumentOutOfRangeException(nameof(height), "Height of a Room must be greater than 0.") : height;
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 1 && x <= Width && y >= 1 && y <= Height;
    }
}
