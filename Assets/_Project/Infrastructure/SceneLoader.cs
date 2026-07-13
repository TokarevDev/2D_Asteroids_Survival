using Cysharp.Threading.Tasks;
using Game.Core;
using UnityEngine.SceneManagement;

namespace Game.Infrastructure
{
    public class SceneLoader : ISceneLoader
    {
        private const string MainMenuScene = "MainMenu";
        private const string GameScene = "Game";

        public async UniTask LoadMainMenuAsync()
        {
            await SceneManager.LoadSceneAsync(MainMenuScene);
        }

        public async UniTask LoadGameAsync()
        {
            await SceneManager.LoadSceneAsync(GameScene);
        }
    }
}
