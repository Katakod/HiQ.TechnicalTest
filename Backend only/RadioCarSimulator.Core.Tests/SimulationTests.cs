namespace RadioCarSimulator.Tests;

using RadioCarSimulator.Core.Model;
using RadioCarSimulator.Core.Services;
using Xunit;

public class SimulationTests
{

    private static SimulationService SetupSimulationOfCarInMiddleOf5x5RoomWithDirection(string initialDirection)
    {
        var room = new Room(5, 5); 
        var car = new Car(3, 3, initialDirection, room); // The middle of the 5x5 Room is 3,3.
        return new SimulationService(car);

        /* Below is the 5 x 5 Test Room visualization of x,y points. 
         * A complex route trace for moving from middle to north boundary, 
         * then to each corner and heading back south to the middle: FF RFF RFFFF RFFFF RFFFF RFF RFF 
         
                          N                   
           ------------------------------- 
           | 1,5 | 2,5 | 3,5 | 4,5 | 5,5 |    
           | 1,4 | 2,4 | 3,4 | 4,4 | 5,4 |    
         W | 1,3 | 2,3 | 3,3 | 4,3 | 5,3 | E  
           | 1,2 | 2,2 | 3,2 | 4,2 | 5,2 |    
           | 1,1 | 2,1 | 3,1 | 4,1 | 5,1 |    
           -------------------------------
                          S        		          
        
         */
    }


    [Theory]
    [InlineData("N", "L", "W")] // Turn left from North should result in West
    [InlineData("W", "L", "S")] // Turn left from West should result in South
    [InlineData("S", "L", "E")] // Turn left from South should result in East
    [InlineData("E", "L", "N")] // Turn left from East should result in North
    [InlineData("N", "R", "E")] // Turn right from North should result in East
    [InlineData("E", "R", "S")] // Turn right from East should result in South
    [InlineData("S", "R", "W")] // Turn right from South should result in West
    [InlineData("W", "R", "N")] // Turn right from West should result in North
    [InlineData("N", "LL", "S")] // Turn left twice from North should result in South
    [InlineData("S", "RR", "N")] // Turn right twice from South should result in North
    [InlineData("W", "LR LR", "W")] // Turn left/right twice (with a blank space) from West should result in West
    public void CarTurn_ShouldChangeDirectionCorrectly(string initialDirection, string turnCommand, string expectedDirection)
    {
        // Arrange
        var simulation = SetupSimulationOfCarInMiddleOf5x5RoomWithDirection(initialDirection); // Car starts in the middle (3,3) of a 5 x 5 room

        // Act
        simulation.ProcessCommands(turnCommand);

        // Assert
        Assert.Equal(expectedDirection, simulation.Car.Direction);
    }

    [Theory]
    [InlineData("N", "F", 3, 4)] // Move forward facing North from (3,3) to (3,4)
    [InlineData("E", "F", 4, 3)] // Move forward facing East from (3,3) to (4,3)
    [InlineData("S", "F", 3, 2)] // Move forward facing South from (3,3) to (3,2)
    [InlineData("W", "F", 2, 3)] // Move forward facing West from (3,3) to (2,3)
    [InlineData("N", "B", 3, 2)] // Move backward facing North from (3,3) to (3,2)
    [InlineData("E", "B", 2, 3)] // Move backward facing East from (3,3) to (2,3)
    [InlineData("S", "B", 3, 4)] // Move backward facing South from (3,3) to (3,4)
    [InlineData("W", "B", 4, 3)] // Move backward facing West from (3,3) to (4,3)
    public void CarMove_WithinRoom_ShouldUpdatePosition(string initialDirection, string commands, int expectedX, int expectedY)
    {
        // Arrange
        var simulation = SetupSimulationOfCarInMiddleOf5x5RoomWithDirection(initialDirection); // Car starts at (3,3)

        // Act
        (bool success, _) = simulation.ProcessCommands(commands);

        // Assert
        Assert.True(success); 
        Assert.Equal(expectedX, simulation.Car.X);
        Assert.Equal(expectedY, simulation.Car.Y);
    }


    [Theory]
    [InlineData("N", "FFF", 3, 5)] // Crash moving North after reaching (3,5) and trying one more move
    [InlineData("E", "FFF", 5, 3)] // Crash moving East after reaching (5,3) and trying one more move
    [InlineData("S", "FFF", 3, 1)] // Crash moving South after reaching (3,1) and trying one more move
    [InlineData("W", "FFF", 1, 3)] // Crash moving West after reaching (1,3) and trying one more move
    public void CarMove_CrashAtBoundary_ShouldFail(string initialDirection, string commands, int finalX, int finalY)
    {
        // Arrange
        var simulation = SetupSimulationOfCarInMiddleOf5x5RoomWithDirection(initialDirection); // Car starts at (3,3)

        // Act
        (bool success, _) = simulation.ProcessCommands(commands);

        // Assert
        Assert.False(success); 
        Assert.Equal(finalX, simulation.Car.X); // Car should be at the X before the crash 
        Assert.Equal(finalY, simulation.Car.Y); // Car should be at the Y before the crash
        
        //TODO: Perhaps refactor and move these outer boundary checks to a boundary success test?
    }

    [Fact]
    public void CarMove_TraceCompleteBoundaryPerimeterAndBackToMiddle_ShouldReachExpectedPosition()
    {
        // Arrange
        var simulation = SetupSimulationOfCarInMiddleOf5x5RoomWithDirection("N"); // Car starts at (3,3)

        // Act
        string commands = "FF RFF RFFFF RFFFF RFFFF RFF RFF"; //Move N to boundary, to each corner (NE, SE, SW, NW) and back to the middle (3,3)
        (bool success, _) = simulation.ProcessCommands(commands); 
                                       
        // Assert
        Assert.True(success);
        Assert.Equal(3, simulation.Car.X);  // Expected X = 3 after full trace
        Assert.Equal(3, simulation.Car.Y);  // Expected Y = 3 after full trace
        Assert.Equal("S", simulation.Car.Direction);  // Facing South after full boundary trace
    }



    [Theory]
    [InlineData(" ")]
    [InlineData("B A L")]
    public void CarInput_InvalidCommand_ShouldFailGracefully(string input)
    {
        // Arrange
        var simulation = SetupSimulationOfCarInMiddleOf5x5RoomWithDirection("N"); // Car starts at (3,3)

        // Act
        (bool success, string resultMessage) = simulation.ProcessCommands(input);

        // Assert
        Assert.False(success);  
        Assert.Contains("invalid", resultMessage?.ToLower());
    }
}
