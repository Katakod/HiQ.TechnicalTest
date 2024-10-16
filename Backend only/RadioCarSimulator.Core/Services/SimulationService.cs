using NLog;
using RadioCarSimulator.Core.Model;

namespace RadioCarSimulator.Core.Services;

public class SimulationService(ICar car) : ISimulationService
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public ICar Car { get; } = car ?? throw new ArgumentNullException(nameof(car));

    public (bool success, string resultMessage) ProcessCommands(string commands)
    {
        int commandIndex = 0;

        try
        {
            commands = (commands ?? "").Replace(" ", ""); //Allow space separation in sequence
            if (commands.Length == 0)
                return CreateResult(false, $"Invalid input - no commands found. Simulation aborted."); //TODO: Perhaps throw an Exception instead?

            foreach (char command in commands)
            {
                switch (command)
                {
                    case 'F':
                        if (!Car.MoveForward())
                            return CreateResult(false, $"Car crashed into the wall at position ({Car.X},{Car.Y}) facing {Car.Direction}!");
                        break;
                    case 'B':
                        if (!Car.MoveBackward())
                            return CreateResult(false, $"Car crashed into the wall at position ({Car.X},{Car.Y}) facing {Car.Direction}!");
                        break;
                    case 'L':
                        Car.TurnLeft();
                        break;
                    case 'R':
                        Car.TurnRight();
                        break;
                    default:
                        return CreateResult(false, $"Invalid command '{command}' (at text index {commandIndex}). Simulation aborted.");
                }

                commandIndex++;
            }

            return CreateResult(true, $"Car moved to final position: ({Car.X},{Car.Y}) facing {Car.Direction}.");
        }
        catch (Exception ex)
        {
            return CreateExceptionResult(ex);
        }
    }

    private static (bool, string) CreateResult(bool success, string resultMessage)
    {
        var simulationResult = success ? "Simulation successful." : "Simulation failed.";
        _logger.Info($"{simulationResult} {resultMessage}");
        return (success, resultMessage);
    }

    private static (bool, string) CreateExceptionResult(Exception ex)
    {
        var errorMessage = "Simulation failed due to an unexpected error while processing commands.";
        _logger.Error(ex, errorMessage);
        return (false, errorMessage);
    }
}