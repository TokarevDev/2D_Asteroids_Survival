using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.Configuration
{
    public interface IGameConfigLoader
    {
        UniTask LoadAsync(CancellationToken cancellationToken);
    }
}
