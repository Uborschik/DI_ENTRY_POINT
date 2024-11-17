using Game.DI;

namespace Game.Gameplay.Root
{
    public class GameplayEntryPoint : MonoInstaller
    {
        public override void Bind(DIContainer container)
        {
            container.RegisterInstance(new TestService());
            container.RegisterInstance(new TestServiceTwo());
        }
    }
}