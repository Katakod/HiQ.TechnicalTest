# Answers to Backend Coding Test - Radio Car Simulator  #

## About
This solution was developed using Visual Studio 2022 and C# .NET 8 to implement the basic User Story below.  
Open RadioCarSimulator.sln in Visual Studio and run debug with RadioCarSimulator.ConsoleApp as start-up project to test it.  
Open Test Explorer to run related unit tests.

#### User Story
As a **user running the simulation**
I want to **provide the simulator with command input**
And get **simulation results based on said command input**
So that **I know if a route is successful or not**. 

    
## Questions & Answers

#### *1. How long time did you end up spending on this coding test?*  
  I'm guessing around 5-6 hours. Personally I think I spent too much time on getting friendly I/O messages. I could have spent that on other things. 

#### *2. Explain why you chose the code structure(s) you used in your solution.* 
I chose to split the code in two main projects. ConsoleApp for handling input and Core implementing the "business" logic. 
I did this to make it a little more scalable and separating concerns, as I/O may change. 
For the same reason I created 2 separate test projects related to the main projects.  
Further on I used folders for Model and Service files to make a little purpose/readability separation inside Core project.
I didn't do this for the other projects, but that would be a good idea if the solution grows.
I added basic Exception handling at a "global" level for each project. I didn't use any real DI and hence I took a shortcut by implementing NLog without using an injected ILogger interface.
This shouldn't matter too much in a small solution as this, but for scaling it would be a good idea. For now, I don't think we care about the logging in e.g. Test projects as it's a side effect and not core functionality.
  
#### *3. What would you add to your solution if you had more time? This question is especially important if you did not spend much time on the coding test - use this as an opportunity to explain what your solution is missing.*    
For better scalability and testability I would add DI so that i e.g could inject a different logger if I wanted.
Perhaps I would then also refactor to follow a Factory pattern encapsulating the creation of objects.
I used tuples extensively in the current solution, but I think it would be more readable if I added some DTO:s to hold result and states (like a CarMovementState for example).
Also, I would probably change the type exchange and have some request/response objects for the Core service, to make it more similar to a web api with possible JSON serialization.
In this case, Room and Car would probably be required as part of a process request. 
And I don't think processing of directions and movement need to be case-sensitive as today. And a need for localization may come up.
I could perhaps add some more tests, like CarTests and RoomTests, and probably refine the one's I setup. I guess they're not all needed if I try to find and remove "duplicate cases". 
And finally, regarding future use cases, I think users may wanna run simulations with multiple cars, perhaps for multiple rooms, and with different type of cars with different behaviours. The more cases, the bigger the need for structure..  

#### *4. What did you think of this recruitment test?*    
I thought it was a fun assignment that raised a lot of questions to consider. Normally you work in a larger context with teams and colleagues to talk to and have a pre-existing base of conventions and patterns, as well as better access to scope and feedback. Depending on context, it's a good thing to keep things simple and let it grow when needed, with a YAGNI mindset. Anyway, I hope my solution for this assignment is a good starting point for a discussion. :)  

