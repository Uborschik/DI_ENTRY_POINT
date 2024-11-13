using Game.Root;
using System;
using UnityEngine;

namespace Game.Gameplay.Root
{
    public class UIRootBinder : MonoBehaviour
    {
        public Action<SceneParams> ToScene;

        private SceneParams targetParams;

        public void GoToScene()
        {
            ToScene?.Invoke(targetParams);
        }

        public void Bind(SceneParams currentParams)
        {
            targetParams = currentParams;
            ToScene = currentParams.LoadScene;
        }
    }
}