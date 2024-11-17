using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Root
{
    public class SceneLoader
    {
        private readonly UIRootView uiRoot;

        public SceneLoader(UIRootView uiRoot)
        {
            this.uiRoot = uiRoot;
        }

        public async Task LoadAndStartSceneAt(string sceneName)
        {
            await LoadAndStartSceneAt(sceneName, null);
        }

        public async Task LoadAndStartSceneAt(string sceneName, Action sceneParams)
        {
            uiRoot.ShowLoadingScreen();

            await LoadScene(sceneName);

            sceneParams?.Invoke();

            uiRoot.HideLoadingScreen();
        }

        private async Task LoadScene(string name)
        {
            await SceneManager.LoadSceneAsync(name);
        }
    }
}