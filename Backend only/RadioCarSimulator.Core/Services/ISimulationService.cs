using RadioCarSimulator.Core.Model;

namespace RadioCarSimulator.Core.Services
{
    public interface ISimulationService
    {
        ICar Car { get; }

        (bool success, string resultMessage) ProcessCommands(string commands);
    }
}