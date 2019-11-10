using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float powerUpSpeed = 3.0f;
    [SerializeField] private int powerupId;
    
    [SerializeField] private AudioClip powerUpClip;


    void Update()
    {
        transform.Translate(Time.deltaTime * powerUpSpeed * Vector3.down);

        if (transform.position.y < -4.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(powerUpClip, transform.position);
            
            if (player != null)

                switch (powerupId)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }

            Destroy(gameObject);
        }
    }
}