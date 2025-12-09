namespace SpaceTravel.Game
{
    public enum ResourceGroup
    {
        Raw,       // Scrap
        Refined,   // Metal
        Currency   // Gold
    }
    
    public enum ModuleCategory
    {
        None,
        Extraction,
        Production,
        Energy,
        Storage,
        Control,
        Research,
        Crew,
        Drone
    }

    public enum TileType
    {
        Empty,
        Normal,
        ReactorMount,
        EngineMount
    }
}