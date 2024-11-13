using System;

namespace Game.Root
{
    public class SceneParams
    {
        public string SceneName { get; }

        public Action<SceneParams> LoadScene;

        public SceneParams(string sceneName, Action<SceneParams> loadScene)
        {
            SceneName = sceneName;
            LoadScene = loadScene;
        }

        public void Subscribe(Action<SceneParams> loadScene)
        {
            LoadScene = loadScene;
        }
    }
}