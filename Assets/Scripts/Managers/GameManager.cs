using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private bool IsGameOver;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && IsGameOver)
            {
                SceneManager.LoadScene(1);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }

        public void GameOver()
        {
            IsGameOver = true;
        }
    }
}