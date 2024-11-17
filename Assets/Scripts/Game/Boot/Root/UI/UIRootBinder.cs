using Game.Root;
using System;
using UnityEngine;

namespace Game.Gameplay.Root
{
    public class UIRootBinder : MonoBehaviour
    {
        public Action<string, Action> LoadScene;

        //public void GoToScene()
        //{
        //    LoadScene?.Invoke(SceneNames., null);
        //}

        //public void Bind(Action<string, Action> loadScene)
        //{
        //    LoadScene = loadScene;
        //}
    }
}