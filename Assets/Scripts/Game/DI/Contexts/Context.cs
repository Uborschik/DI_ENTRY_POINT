using System.Collections.Generic;
using UnityEngine;

namespace Game.DI
{
    public abstract class Context : MonoBehaviour
    {
        [SerializeField] private List<MonoInstaller> monoInstallers;

        private DIContainer sceneContainer;
        private Injector injector;

        internal DIContainer DIContainer => sceneContainer;
        internal Injector Injector => injector;

        public virtual void Run(DIContainer rootContainer)
        {
            sceneContainer = new DIContainer(rootContainer);
            injector = new Injector(sceneContainer);

            InstallMonoInstallers();
        }

        private void InstallMonoInstallers()
        {
            monoInstallers.ForEach(x => x.Bind(sceneContainer));
        }
    }
}