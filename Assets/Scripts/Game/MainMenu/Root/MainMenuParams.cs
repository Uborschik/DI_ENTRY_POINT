using Game.Root;
using System;
using UnityEngine;

namespace Game.MainMenu.Root
{
    public class MainMenuParams : SceneParams
    {
        public MainMenuParams(Action<SceneParams> loadScene) : base(SceneNames.MAIN_MENU, loadScene)
        {
        }
    }
}