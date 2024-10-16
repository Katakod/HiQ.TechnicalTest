namespace RadioCarSimulator.Core.Model
{
    public interface IRoom
    {
        int Height { get; }
        int Width { get; }

        bool IsWithinBounds(int x, int y);
    }
}