using System.Collections.Generic;

namespace SpaceTravel.Game
{
    public interface IGameDefinitions
    {
        ModuleDefinition GetModuleDefinition(string id);
        ShipDefinition GetShipDefinition(string id);
        ResourceDefinition GetResourceDefinition(string id);

        IReadOnlyList<ModuleDefinition> GetAllModules();
        IReadOnlyList<ResourceDefinition> GetAllResources();
    }
}