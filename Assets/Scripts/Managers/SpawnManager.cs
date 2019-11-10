using System.Collections;
using UnityEngine;

namespace Managers
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject enemyContainer;
        [SerializeField] private GameObject[] powerUpPrefabs;

        private bool StopSpawning;

        public void StartSpawning()
        {
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerUpRoutine());
        }

        private IEnumerator SpawnEnemyRoutine()
        {
            yield return new WaitForSeconds(3.5f);
        
            while (!StopSpawning)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            
                GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
            
                newEnemy.transform.parent = enemyContainer.transform;

                yield return new WaitForSeconds(5.0f);
            }
        }

        IEnumerator SpawnPowerUpRoutine()

        {
            yield return new WaitForSeconds(3.5f);
        
            while (!StopSpawning)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            
                int randomPowerUp = Random.Range(0, 3);
                Instantiate(powerUpPrefabs[randomPowerUp], posToSpawn, Quaternion.identity);
            
                yield return new WaitForSeconds(Random.Range(3, 8));
            }
        }

        public void OnPlayerDeath()
        {
            StopSpawning = true;
        }
    }
}