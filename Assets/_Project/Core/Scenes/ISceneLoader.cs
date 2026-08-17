using Cysharp.Threading.Tasks;

namespace Game.Core.Scenes
{
    public interface ISceneLoader
    {
        UniTask LoadMainMenuAsync();
        UniTask LoadGameAsync();
    }
}
