using Game.DI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game.Root
{
    public class GameEntryPoint
    {
        private static GameEntryPoint instance;

        private readonly UIRootView uiRoot;
        private readonly DIContainer rootContainer = new();
        private DIContainer cachedSceneContainer;


        public GameEntryPoint()
        {
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(uiRoot.gameObject);

            rootContainer.RegisterInstance(uiRoot);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoStartGame()
        {
            Application.targetFrameRate = 60;
            
            instance = new GameEntryPoint();
            instance.RunGame();
        }

        private void RunGame()
        {
#if UNITY_EDITOR

            var sceneName = SceneManager.GetActiveScene().name;

            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if(SceneManager.GetSceneByBuildIndex(i).name == sceneName)
                {
                    var scene = SceneManager.GetSceneByBuildIndex(i);
                    var fitrstLoadParams = new SceneParams(scene.name, LoadAndStartSceneAt);
                    LoadAndStartSceneAt(fitrstLoadParams);
                }
            }

#endif
        }

        private async void LoadAndStartSceneAt(SceneParams enterParams = null)
        {
            uiRoot.ShowLoadingScreen();
            cachedSceneContainer?.Dispose();

            await LoadScene(SceneNames.BOOT);
            await LoadScene(enterParams.SceneName);

            await Awaitable.WaitForSecondsAsync(.2f);

            var sceneRootBinder = Object.FindFirstObjectByType<SceneEntryPoint>();
            var sceneContainer = cachedSceneContainer = new(rootContainer);

            sceneContainer.RegisterInstance(enterParams);
            var exitParams = sceneRootBinder.Run(sceneContainer);
            exitParams.Subscribe((e) => LoadAndStartSceneAt(e));

            uiRoot.HideLoadingScreen();
        }

        private async Task LoadScene(string name)
        {
            await SceneManager.LoadSceneAsync(name);
        }
    }
}