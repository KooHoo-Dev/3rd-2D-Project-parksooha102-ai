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
            var mainCamera = Camera.main;
        }

        public enum GameScene
        {
            MainMenu = 0,
            Lab,
            WorkShop,
        }

        public void ChangeScene(int sceneIndex)
        {
            // 비효율적이지만 빠르게 버튼을 이용하기위해 함수를 제작함
            ChangeScene((GameScene)sceneIndex);
        }

        public void ChangeScene(GameScene targetScene)
        {
            SceneManager.LoadScene((int)targetScene);
        }

        public void QuitGame()
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }

    }
}