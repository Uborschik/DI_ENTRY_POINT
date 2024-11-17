using Game.DI;
using Game.Gameplay.Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Root
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        [SerializeField] protected UIRootBinder SceneRootBinder;

        [Inject] protected UIRootView UIRoot;
        [Inject] protected SceneLoader SceneLoader;

        public void Run(DIContainer container)
        {
            //SceneInstaller(container);
            //new Injector(SceneManager.GetActiveScene(), container);

            //var uiScene = Instantiate(SceneRootBinder);
            //UIRoot.AttachSceneUI(uiScene.gameObject);
            //uiScene.Bind(SceneLoader.LoadAndStartSceneAt);
        }

        protected abstract void SceneInstaller(DIContainer container);
    }
}