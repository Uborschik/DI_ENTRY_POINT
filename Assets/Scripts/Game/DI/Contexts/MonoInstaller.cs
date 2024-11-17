using UnityEngine;

namespace Game.DI
{
    public abstract class MonoInstaller : MonoBehaviour
    {
        public abstract void Bind(DIContainer container);
    }
}
