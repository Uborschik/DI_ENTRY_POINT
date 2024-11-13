using Game.Root;
using System;
using UnityEngine;

namespace Game.Gameplay.Root
{
    public class GameplayParams : SceneParams
    {
        public GameplayParams(Action<SceneParams> toScene) : base(SceneNames.GAMEPLAY, toScene)
        {
        }
    }
}