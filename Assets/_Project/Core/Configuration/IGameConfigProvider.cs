namespace Game.Core.Configuration
{
    public interface IGameConfigProvider
    {
        bool IsInitialized { get; }

        PlayerConfig Player { get; }
        EnemyConfig Enemy { get; }
        WorldConfig World { get; }
    }
}
