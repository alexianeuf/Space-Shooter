using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemySpeed = 4.0f;

    private Player Player;
    private Animator Anim;
    
    private AudioSource AudioSource;
    [SerializeField] private GameObject laserPrefab;
    
    private float FireRate = 3.0f;
    private float CanFire = -1;
    private static readonly int OnEnemyDeath = Animator.StringToHash("OnEnemyDeath");

    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Player>();
        AudioSource = GetComponent<AudioSource>();

        if (Player == null)
        {
            Debug.LogError("The Player in Enemy is NULL. ");
        }

        Anim = GetComponent<Animator>();

        if (Anim == null)
        {
            Debug.LogError("The Enemy Animator is NULL.");
        }

        if (AudioSource == null)
        {
            Debug.LogError("The Enemy AudioSource is NULL.");
        }
    }

    void Update()
    {
        CalculateMovement();
        
        if (Time.time > CanFire)

        {
            FireRate = Random.Range(3f, 7f);
            CanFire = Time.time + FireRate;
            
            GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach (var laser in lasers)
            {
                laser.AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Time.deltaTime * enemySpeed * Vector3.down);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Anim.SetTrigger(OnEnemyDeath);
            enemySpeed = 0;
            AudioSource.Play();
            Destroy(this.gameObject, 2.5f);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            
            if (Player != null)
            {
                Player.AddScore(10);
            }

            Anim.SetTrigger(OnEnemyDeath);
            enemySpeed = 0;
            
            CanFire = Time.time + 200;
            
            AudioSource.Play();
            
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
    }
}