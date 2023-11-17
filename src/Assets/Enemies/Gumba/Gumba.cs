using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gumba : MonoBehaviour
{
    public float dyingAnimationDuration = 1;
    public float dyingAnimationScale = 0.5f;

    public AudioSource randomAudioSource;
    public AudioSource walkAudioSource;
    public AudioSource deathAudioSource;
    public PlayerController player;

    public void Update()
    {
        int random = Random.Range(1, 10000);
        print(random);
        if (random == 1)
        {
            if (!randomAudioSource.isPlaying)
            {
                randomAudioSource.PlayOneShot(randomAudioSource.clip, 1f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CompareTag("EnemyWeakpoint"))
            {
                if (!deathAudioSource.isPlaying)
                {
                    deathAudioSource.PlayOneShot(deathAudioSource.clip, 1f);
                }

                foreach (Transform child in transform.parent)
                {
                    if (child.TryGetComponent(out Collider collider))
                        collider.enabled = false;
                }

                if (transform.parent.parent.TryGetComponent(out CinemachineDollyCart dollyCart))
                {
                    dollyCart.enabled = false;
                }

                var currentScale = transform.parent.parent.parent.localScale;
                transform.parent.parent.parent.localScale = new(currentScale.x, currentScale.y * dyingAnimationScale, currentScale.z);
                Destroy(transform.parent.parent.parent.gameObject, dyingAnimationDuration);
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                player.PlayerDeath();
            }
        }
    }
}
