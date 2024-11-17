using Game.DI;
using Game.Root;

namespace Game.MainMenu.Root
{
    public class MainMenuEntryPoint : SceneEntryPoint
    {
        protected override void SceneInstaller(DIContainer container)
        {
            //var uiScene = Instantiate(SceneRootBinder);
            //UIRoot.AttachSceneUI(uiScene.gameObject);
            //uiScene.Bind(SceneLoader.LoadAndStartSceneAt, SceneNames.GAMEPLAY);
        }
    }
}