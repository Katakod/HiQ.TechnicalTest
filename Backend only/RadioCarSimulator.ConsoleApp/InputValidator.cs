namespace RadioCarSimulator.ConsoleApp;

public class InputValidator
{
    public static ((int width, int height)? dimensions, string resultMessage) ValidateRoomInput(string input)
    {
        var parts = input.Split(' ');
        if (parts.Length == 2 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
        {
            if (width > 0 && height > 0)
                return ((width, height), $"Room created with dimensions of width: {width} and height: {height}.");
        }
         
        return (null, "Invalid room dimensions. Valid numbers for width and height must be entered.");
    }

    public static ((int, int, string)? startPosition, string resultMessage) ValidateCarPositionInput(string input)
    {
        var parts = input.Split(' ');
        if (parts.Length == 3 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y) && IsValidDirection(parts[2]))
        {
            var direction = parts[2];
            return ((x, y, direction), $"Car start position set at x: {x} and y: {y}, with direction: {direction}.");
        }

        return (null, "Invalid car starting position. Valid numbers for x and y, together with a direction (N, E, S, W) must be entered.");
    }

    public static (bool isValid, string resultMessage) ValidateCommandsInput(string input)
    {
        input = (input ?? "").Replace(" ", "");

        if (input.All("FBLR".Contains))
        {
            return (true, "Valid commands entered."); ;
        }
        
        return (false, "Invalid commands entered. Only F, B, L, R (and space) are allowed.");
    }

    private static bool IsValidDirection(string direction)
    {
        return direction == "N" || direction == "E" || direction == "S" || direction == "W";
    }
}
