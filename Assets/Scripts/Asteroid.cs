using Managers;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 10.0f;
    
    [SerializeField] private GameObject explosionPrefab;
    private SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }

    void Update()
    {
        transform.Rotate(Time.deltaTime * rotateSpeed * Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            
            Destroy(other.gameObject);
            spawnManager.StartSpawning();
            
            Destroy(gameObject, 0.25f);
        }
    }
}