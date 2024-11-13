using Game.DI;
using Game.Gameplay.Root;
using Game.MainMenu.Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Root
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        [SerializeField] protected UIRootBinder SceneRootBinder;
        [Inject] protected UIRootView UIRoot;

        protected SceneParams SceneParams;

        public SceneParams Run(DIContainer container)
        {
            SceneParams = container.Resolve<SceneParams>();
            var exitParams = new MainMenuParams(SceneParams.LoadScene);

            SceneInstaller(container);

            new Injector(SceneManager.GetActiveScene(), container);

            var uiScene = Instantiate(SceneRootBinder);
            UIRoot.AttachSceneUI(uiScene.gameObject);
            uiScene.Bind(exitParams);

            return exitParams;
        }

        protected abstract void SceneInstaller(DIContainer container);
    }
}