using Cysharp.Threading.Tasks;

namespace Game.Core
{
    public interface ISceneLoader
    {
        UniTask LoadMainMenuAsync();
        UniTask LoadGameAsync();
    }
}
