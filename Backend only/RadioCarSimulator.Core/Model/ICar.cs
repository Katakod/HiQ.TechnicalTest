namespace RadioCarSimulator.Core.Model
{
    public interface ICar
    {
        string Direction { get; }
        int X { get; }
        int Y { get; }

        bool MoveBackward();
        bool MoveForward();
        void TurnLeft();
        void TurnRight();
    }
}