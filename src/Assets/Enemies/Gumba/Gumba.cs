using Cinemachine;
using UnityEngine;

public class Gumba : MonoBehaviour
{
    public float dyingAnimationDuration = 1;
    public float dyingAnimationScale = 0.5f;

    public AudioSource randomAudioSource;
    public AudioSource walkAudioSource;
    public AudioSource deathAudioSource;

    public void Update()
    {
        int random = Random.Range(1, 10000);
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
        if (!other.CompareTag("Player")) return;

        if (!CompareTag("EnemyWeakpoint"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayerDeath();
            return;
        }

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
        other.transform.parent.GetComponent<PlayerController>().JumpWithoutSound();
        Destroy(transform.parent.parent.parent.gameObject, dyingAnimationDuration);
    }
}
