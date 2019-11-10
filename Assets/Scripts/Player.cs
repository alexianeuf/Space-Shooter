using System.Collections;
using System.Collections.Generic;
using Managers;
using UI;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;
    private float SpeedMultiplier = 2;
    [SerializeField] private float fireRate = 0.15f;
    private float CanFire = -1.0f;

    [SerializeField] private int lives = 3;
    private int Score;

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleShotPrefab;

    private SpawnManager SpawnManager;
    private UiManager UiManager;

    private bool IstripleshotActive;
    private bool IsShieldsActive;
    
    private bool IsLeftEngineActive;
    private bool IsRightEngineActive;

    [SerializeField] private GameObject shieldVisualizer;
    [SerializeField] private GameObject leftEngine, rightEngine;

    [SerializeField] private AudioClip laserSoundClip;
    private AudioSource AudioSource;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        SpawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        UiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        AudioSource = GetComponent<AudioSource>();

        if (SpawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null!");
        }

        if (UiManager == null)
        {
            Debug.LogError("The UI Manager is null!");
        }

        if (AudioSource == null)
        {
            Debug.LogError("The Audio Source on the Player is null!");
        }
        else
        {
            AudioSource.clip = laserSoundClip;
        }
    }


    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > CanFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(Time.deltaTime * speed * direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 0), 0);

        if (transform.position.x >= 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        CanFire = Time.time + fireRate;

        if (IstripleshotActive)
        {
            Instantiate(tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        AudioSource.Play();
    }


    public void Damage()

    {
        if (IsShieldsActive)
        {
            IsShieldsActive = false;
            shieldVisualizer.SetActive(false);
            return;
        }

        lives--;

        if (lives == 2)
        {
            var engine = Random.Range(0, 2);

            if (engine == 0)
            {
                leftEngine.SetActive(true);
                IsLeftEngineActive = true;
            }
            else
            {
                rightEngine.SetActive(true);
                IsRightEngineActive = true;
            }
        }
        else if (lives == 1)
        {
            if (IsLeftEngineActive)
            {
                rightEngine.SetActive(true);

                IsRightEngineActive = true;
            }
            else if (IsRightEngineActive)
            {
                leftEngine.SetActive(true);

                IsLeftEngineActive = true;
            }
        }

        UiManager.UpdateLives(lives);

        if (lives < 1)
        {
            if (lives < 0)
            {
                lives = 0;
            }
            
            SpawnManager.OnPlayerDeath();

            Destroy(gameObject);
        }
    }

    public void TripleShotActive()
    {
        IstripleshotActive = true;

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        IstripleshotActive = false;
    }

    public void SpeedBoostActive()
    {
        speed *= SpeedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        speed /= SpeedMultiplier;
    }

    private IEnumerator ShieldPowerUpDownRoutine()

    {
        yield return new WaitForSeconds(5.0f);
        IsShieldsActive = false;
        shieldVisualizer.SetActive(false);
    }

    public void ShieldActive()
    {
        IsShieldsActive = true;
        shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldPowerUpDownRoutine());
    }

    public void AddScore(int points)
    {
        Score += points;
        UiManager.UpdateScore(Score);
    }
}