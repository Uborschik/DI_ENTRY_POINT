using Game.DI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game.Root
{
    public class GameEntryPoint
    {
        private static GameEntryPoint instance;

        private readonly UIRootView uiRoot;
        private readonly SceneLoader sceneLoader;
        private readonly DIContainer rootContainer = new();

        public GameEntryPoint()
        {
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(uiRoot.gameObject);

            sceneLoader = new(uiRoot);

            rootContainer.RegisterInstance(uiRoot);
            rootContainer.RegisterInstance(sceneLoader);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoStartGame()
        {
            Application.targetFrameRate = 60;
            
            instance = new GameEntryPoint();
            instance.RunGame();
        }

        private async void RunGame()
        {
#if UNITY_EDITOR

            var sceneName = SceneManager.GetActiveScene().name;

            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if(SceneManager.GetSceneByBuildIndex(i).name == sceneName)
                {
                    var scene = SceneManager.GetSceneByBuildIndex(i);
                    await sceneLoader.LoadAndStartSceneAt(sceneName, CreateSceneParams);
                }
            }

            void CreateSceneParams()
            {
                var sceneRootBinder = Object.FindFirstObjectByType<Context>();

                sceneRootBinder.Run(rootContainer);
            }

#endif
        }
    }
}