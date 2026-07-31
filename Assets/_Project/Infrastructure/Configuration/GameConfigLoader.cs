using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Configuration;

namespace Game.Infrastructure.Configuration
{
    public sealed class GameConfigLoader : IGameConfigLoader
    {
        private const string PlayerConfigPath = "Configs/player.json";
        private const string EnemyConfigPath = "Configs/enemy.json";
        private const string WorldConfigPath = "Configs/world.json";

        private readonly JsonConfigReader _reader;
        private readonly GameConfigValidator _validator;
        private readonly GameConfigProvider _provider;

        public GameConfigLoader(JsonConfigReader reader, GameConfigValidator validator, GameConfigProvider provider)
        {
            _reader = reader;
            _validator = validator;
            _provider = provider;
        }

        public async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            PlayerConfig player = await _reader.ReadAsync<PlayerConfig>(PlayerConfigPath, cancellationToken);

            EnemyConfig enemy = await _reader.ReadAsync<EnemyConfig>(EnemyConfigPath, cancellationToken);

            WorldConfig world = await _reader.ReadAsync<WorldConfig>(WorldConfigPath, cancellationToken);

            _validator.Validate(player);
            _validator.Validate(enemy);
            _validator.Validate(world);

            _provider.Initialize(player, enemy, world);
        }
    }
}
