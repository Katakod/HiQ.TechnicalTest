namespace RadioCarSimulator.ConsoleApp.Tests;

using Xunit;

public class InputValidatorTests
{
    [Theory]
    [InlineData("5 5", 5, 5)]
    [InlineData("10 20", 10, 20)]
    public void ValidateRoomInput_ValidInput_ShouldReturnDimensions(string input, int expectedWidth, int expectedHeight)
    {
        //Act
        var (dimensions, _) = InputValidator.ValidateRoomInput(input);
        
        //Assert
        Assert.True(dimensions.HasValue);  

        var (width, height) = dimensions.Value;  
        Assert.Equal(expectedWidth, width);
        Assert.Equal(expectedHeight, height);
    }

    [Theory]
    [InlineData("invalid input")]
    [InlineData("5.5 10")]
    [InlineData("10")]
    [InlineData("0 0")]
    public void ValidateRoomInput_InvalidInput_ShouldReturnNull(string input)
    {
        // Act
        var (dimensions, resultMessage) = InputValidator.ValidateRoomInput(input);

        // Assert
        Assert.Null(dimensions);
        Assert.Contains("invalid", resultMessage?.ToLower());
    }

    [Theory]
    [InlineData("1 2 N", 1, 2, "N")]
    [InlineData("3 4 E", 3, 4, "E")]
    public void ValidateCarPositionInput_ValidInput_ShouldReturnPositionAndDirection(string input, int expectedX, int expectedY, string expectedDirection)
    {
        //Act
        var (startPosition, resultMessage) = InputValidator.ValidateCarPositionInput(input);
        
        //Assert
        Assert.True(startPosition.HasValue);

        var (x, y, direction) = startPosition.Value;
        Assert.Equal(expectedX, x);
        Assert.Equal(expectedY, y);
        Assert.Equal(expectedDirection, direction);
    }

    [Theory]
    [InlineData("1 2 InvalidDirection")]
    [InlineData("A b N")]
    [InlineData("1.1 2.2 N")]
    [InlineData("1,1 2,2 N")]
    [InlineData("N 5 5")]
    public void ValidateCarPositionInput_InvalidInput_ShouldReturnNull(string input)
    {
        // Act
        var (startPosition, resultMessage) = InputValidator.ValidateCarPositionInput(input);

        // Assert
        Assert.Null(startPosition);
        Assert.Contains("invalid", resultMessage?.ToLower());        
    }

    [Theory]
    [InlineData("FFLR")]
    [InlineData("FBL LRRF")]
    public void ValidateCommands_ValidInput_ShouldReturnTrue(string input)
    {
        //Act
        var (isValid, _) = InputValidator.ValidateCommandsInput(input);

        //Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData("FFLRX")]
    [InlineData("RL 1BF")]
    public void ValidateCommands_InvalidInput_ShouldReturnFalse(string input)
    {
        //Act
        var (isValid, resultMessage) = InputValidator.ValidateCommandsInput(input);
        
        //Assert
        Assert.False(isValid);
        Assert.Contains("invalid", resultMessage?.ToLower());
    }
}
