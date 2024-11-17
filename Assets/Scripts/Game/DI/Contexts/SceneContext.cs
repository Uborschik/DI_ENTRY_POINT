using System.Linq;
using UnityEngine.SceneManagement;

namespace Game.DI
{
    public class SceneContext : Context
    {
        public override void Run(DIContainer rootContainer)
        {
            base.Run(rootContainer);

            InjectCurrentScene();
        }

        private void InjectCurrentScene()
        {
            var objects = SceneManager.GetActiveScene().GetRootGameObjects().ToList();
            objects.ForEach(Injector.InjectGameObjectRecursively);
        }
    }
}