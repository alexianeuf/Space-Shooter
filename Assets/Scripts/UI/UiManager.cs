using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Image livesImg;
        [SerializeField] private Sprite[] liveSprites;
        [SerializeField] private Text gameOverText;
        [SerializeField] private Text restartText;

        private GameManager GameManager;

        void Start()
        {
            scoreText.text = "Score: " + 0;
            gameOverText.gameObject.SetActive(false);
            restartText.gameObject.SetActive(false);

            GameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

            if (GameManager == null)
            {
                Debug.LogError("The Game Manager is NULL");
            }
        }

        public void UpdateScore(int playerScore)
        {
            scoreText.text = "Score: " + playerScore;
        }

        public void UpdateLives(int currentLives)
        {
            livesImg.sprite = liveSprites[currentLives];

            if (currentLives <= 0)
            {
                GameOverSequence();
            }
        }

        private void GameOverSequence()
        {
            GameManager.GameOver();
            gameOverText.gameObject.SetActive(true);
            restartText.gameObject.SetActive(true);

            StartCoroutine(GameOverFlickerRoutine());
        }

        private IEnumerator GameOverFlickerRoutine()
        {
            while (true)
            {
                gameOverText.enabled = true;
                yield return new WaitForSeconds(0.5f);
                gameOverText.enabled = false;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}