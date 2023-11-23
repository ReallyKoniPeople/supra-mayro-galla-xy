using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Browser : MonoBehaviour
{
    [SerializeField] public Transform target;
    public GameObject bullet;
    public Transform spawnPoint;

    public float minCooldown;
    public float maxCooldown;
    private float nextShot;

    public float bulletSpeed;

    bool directionRight = true;

    public AudioSource shootAudioSource1;
    public AudioSource shootAudioSource2;

    // Update is called once per frame
    void Update()
    {
        ShootAtPlayer();

        // movement
        CinemachineDollyCart dollyCart = transform.Find("Dolly Cart").GetComponent<CinemachineDollyCart>();
            
        if (directionRight && dollyCart.m_Position == 0.0)
        {
            directionRight = false;
        }
        else if (!directionRight && dollyCart.m_Position == 5.0)
        {
            directionRight = true;
        }

        if (directionRight)
        {
            dollyCart.m_Speed = -5.0f;
        }
        else
        {
            dollyCart.m_Speed = 5.0f;
        }
    }

    void ShootAtPlayer()
    {
        if (Time.time > nextShot)
        {
            nextShot = Time.time + Random.Range(minCooldown, maxCooldown);

            if (!shootAudioSource1.isPlaying && !shootAudioSource2.isPlaying)
            {
                int random = Random.Range(1, 10);
                if (random == 1)
                {
                    shootAudioSource1.PlayOneShot(shootAudioSource1.clip, 1f);
                }
                else if (random == 2) 
                {
                    shootAudioSource2.PlayOneShot(shootAudioSource2.clip, 1f);
                }
            }

            // shoot the bullet
            GameObject bulletObj = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
            bulletRig.AddForce(bulletRig.transform.forward * bulletSpeed);
            Destroy(bulletObj, 15f);
        }
    }
}
