using UnityEngine;

namespace Game.Root
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Transform uiSceneContainer;

        private void Awake()
        {
            HideLoadingScreen();
        }

        public void ShowLoadingScreen()
        {
            loadingScreen.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            loadingScreen.SetActive(false);
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();

            sceneUI.transform.SetParent(uiSceneContainer, false);
        }

        private void ClearSceneUI()
        {
            var childCount = uiSceneContainer.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(uiSceneContainer.GetChild(i).gameObject);
            }
        }
    }
}