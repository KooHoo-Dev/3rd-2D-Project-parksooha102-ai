using Study.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _2DProject
{
    public class SceneMenuController : MonoBehaviour
    {
        [SerializeField] private List<MenuButtonConfig> buttonConfigs;

        private SceneController.GameScene currentScene;
        private readonly List<GameObject> labButtons = new();

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;

            currentScene = (SceneController.GameScene)SceneManager.GetActiveScene().buildIndex;
            ApplyForScene(currentScene);
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void Update()
        {
            if (currentScene == SceneController.GameScene.Lab)
            {
                bool show = SimpleInput.GetKey(Key.Tab);

                foreach (GameObject button in labButtons)
                    button.SetActive(show);
            }
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            currentScene = (SceneController.GameScene)scene.buildIndex;
            ApplyForScene(currentScene);
        }

        private void ApplyForScene(SceneController.GameScene scene)
        {
            labButtons.Clear();

            foreach (MenuButtonConfig config in buttonConfigs)
            {
                ScenePositionInfo matched = FindPositionInfo(config, scene);

                if (matched == null)
                {
                    config.Target.gameObject.SetActive(false);
                    continue;
                }

                config.Target.anchoredPosition = matched.AnchoredPosition;

                if (scene == SceneController.GameScene.Lab)
                {
                    if (matched.IsActive)
                        labButtons.Add(config.Target.gameObject);

                    config.Target.gameObject.SetActive(false);
                }
                else
                {
                    config.Target.gameObject.SetActive(matched.IsActive);
                }
            }
        }

        private ScenePositionInfo FindPositionInfo(MenuButtonConfig config, SceneController.GameScene scene)
        {
            foreach (ScenePositionInfo info in config.ScenePositions)
            {
                if (info.Scene == scene)
                    return info;
            }

            return null;
        }
    }
}