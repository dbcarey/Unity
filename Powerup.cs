using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3.0f;

    //ID for Powerups, 0=TripleShot, 1=Speed, 2=Shields
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _powerUpClip;

   


    void Update()
    {
        float randomX = Random.Range(-8.5f, 8.5f);
        transform.position += (Vector3.down * _powerupSpeed * Time.deltaTime);
        if (transform.position.y < -5.7)
        {
            Destroy(this.gameObject);
        } 
            
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerUpClip, transform.position);
            if (player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }


    }

}
