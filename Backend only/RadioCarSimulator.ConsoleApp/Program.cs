using NLog;
using RadioCarSimulator.ConsoleApp;
using RadioCarSimulator.Core.Model;
using RadioCarSimulator.Core.Services;


internal class Program
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    private const int MAXIMUM_BAD_INPUT_DEFAULT = 3; //Users may try 3 times for each input request 

    private static void Main(string[] args)
    {
        _logger.Info("RadioCarSimulator.ConsoleApp start..");
        Console.WriteLine(" ***** Welcome to the Radio Car Simulator! *****");

        try
        {
            IRoom? room = TryToGetRoomFromInput();
            if (room == null)
                ExitAppWithError("Simulation failed - Bad input. Room could not be created from input."); 

            ICar? car = TryToGetCarFromInput(room!);
            if (car == null)
                ExitAppWithError("Simulation failed - Bad input. Car could not be created from input."); 

            string commands = TryToGetCommandsFromInput();
            if (string.IsNullOrWhiteSpace(commands))
                ExitAppWithError("Simulation failed - Bad input. No valid sequence of movement commands for the simulation where found in input.");

            ISimulationService simulation = new SimulationService(car!);
            var (success, resultMessage) = simulation.ProcessCommands(commands);

            DisplaySimulationResult(success, resultMessage);
            
        }
        catch (Exception ex) 
        {
            //TODO: Perhaps we could throw and catch ArgumentExceptions instead of calling ExitAppWithError() when critical objects are null?

            ExitAppWithException($"Simulation failed due to an unexpected error. {ex.Message}", ex);            
        }
        finally
        {
            _logger.Info("RadioCarSimulator.ConsoleApp exit..");
        }
    }

    private static void DisplaySimulationResult(bool success, string resultMessage)
    {
        if (success)
        {
            var message = $"Simulation successful! {resultMessage}";
            Console.WriteLine(message);
            _logger.Info(message);
        }
        else
        {
            var message = $"Simulation failed. {resultMessage}";
            Console.WriteLine(message);
            _logger.Info(message);
        }
    }
    private static Room? TryToGetRoomFromInput()
    {
        Room? room = null;
        int inputCount = 1;

        Console.WriteLine("Enter Room dimensions (width height) with a space between.");        

        while (room == null && inputCount <= MAXIMUM_BAD_INPUT_DEFAULT)
        {            
            var input = ReadInput();
            var validationResult = InputValidator.ValidateRoomInput(input);
            if (validationResult.dimensions.HasValue)
            {
                var dimensions = validationResult.dimensions.Value;
                room = new Room(dimensions.width, dimensions.height);
                _logger.Info($"Room created with valid input: '{input}'.");
            }
            else
            {
                Console.WriteLine(validationResult.resultMessage);
                _logger.Info($"No Room created due to invalid input: '{input}'. {validationResult.resultMessage}");
            }

            ++inputCount;
        }

        return room;
    }

    private static Car? TryToGetCarFromInput(IRoom room)
    {
        Car? car = null;
        int inputCount = 1;

        Console.WriteLine("Enter Car starting position and direction (x y direction) with spaces between.");

        while (car == null && inputCount <= MAXIMUM_BAD_INPUT_DEFAULT)
        {            
            var input = ReadInput();
            var validationResult = InputValidator.ValidateCarPositionInput(input);
            if (validationResult.startPosition.HasValue)
            {
                var (x, y, direction) = validationResult.startPosition.Value;

                if (room.IsWithinBounds(x, y))
                {
                    car = new Car(x, y, direction, room);
                    _logger.Info($"Car created with valid input: '{input}'.");
                }                    
                else
                {
                    var outOfBoundsMessage = $"The position ({x},{y}) is out of bounds for this Room ({room.Width},{room.Height}).";
                    Console.WriteLine(outOfBoundsMessage);
                    _logger.Info($"No Car created due to invalid input: '{input}'. {outOfBoundsMessage}");
                }
            }
            else
            {
                Console.WriteLine(validationResult.resultMessage);
                _logger.Info($"No Car created due to invalid input: '{input}'. {validationResult.resultMessage}");
            }

            ++inputCount;
        }

        return car;
    }

    private static string TryToGetCommandsFromInput()
    {
        string commands = "";
        int inputCount = 1;

        Console.WriteLine("Enter movement commands (F B L R) in a sequence (with or without spaces).");
        Console.WriteLine("F=Forward, B=Backward, L=Left, R=Right  (Example: B R FF L F).");

        while (string.IsNullOrEmpty(commands) && inputCount <= MAXIMUM_BAD_INPUT_DEFAULT)
        {
            var input = ReadInput();
            var validationResult = InputValidator.ValidateCommandsInput(input);
            if (validationResult.isValid)
            {
                commands = input;
                _logger.Info($"Valid Commands input: '{input}'.");
            }
            else
            {
                Console.WriteLine(validationResult.resultMessage);
                _logger.Info($"Invalid Commands input: '{input}'. {validationResult.resultMessage}");
            }

            ++inputCount;
        }

        return commands;
    }

    private static string ReadInput()
    {
        Console.WriteLine("Please enter your input:");
        return Console.ReadLine() ?? "";
    }

    private static void ExitAppWithError(string errorMessage)
    {
        Console.WriteLine(errorMessage);
        _logger.Error(errorMessage);
        Environment.Exit(1);
    }

    private static void ExitAppWithException(string errorMessage, Exception ex)
    {
        Console.WriteLine(errorMessage);
        _logger.Error(ex, errorMessage);
        Environment.Exit(1);
    }
}

