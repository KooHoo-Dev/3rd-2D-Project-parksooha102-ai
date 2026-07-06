using UnityEngine;
using UnityEngine.SceneManagement;
using Jay;

namespace _2DProject
{
    public class SceneController : SingletonBase<SceneController>
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
            Camera mainCamera = Camera.main;
        }

        public enum GameScene
        {
            MainMenu = 0,
            Lab,
            WorkShop,
        }

        public void ChangeScene(int sceneIndex)
        {
            ChangeScene((GameScene)sceneIndex);
        }

        public void ChangeScene(GameScene targetScene)
        {
            SceneManager.LoadScene((int)targetScene);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

    }
}