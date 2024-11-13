using Game.DI;
using Game.Root;

namespace Game.Gameplay.Root
{
    public class GameplayEntryPoint : SceneEntryPoint
    {
        protected override void SceneInstaller(DIContainer container)
        {
            container.RegisterInstance(new TestService());
            container.RegisterInstance(new TestServiceTwo());
        }
    }
}